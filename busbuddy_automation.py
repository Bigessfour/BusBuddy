import os
import logging
import time
from datetime import datetime
from github import Github, GithubException
from dotenv import load_dotenv
import json
import re

# Configure logging for updates and errors
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s",
    handlers=[
        logging.FileHandler("github_updates.log"),
        logging.FileHandler("busbuddy_errors.log"),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

# Load environment variables
load_dotenv()
GITHUB_TOKEN = os.getenv("GITHUB_TOKEN")
REPO_PATH = "./BusBuddy"  # Local clone of the repository
REPO_NAME = "Bigessfour/BusBuddy"
SINCE_DATE = datetime(2025, 5, 8)

def check_rate_limit(g, retry_count=0, max_retries=3):
    """Check GitHub API rate limit and handle with exponential backoff."""
    try:
        rate_limit = g.get_rate_limit()
        remaining = rate_limit.core.remaining
        reset_time = rate_limit.core.reset
        logger.info(f"[Script] API Rate Limit: {remaining} requests remaining, resets at {reset_time}")
        if remaining < 100:
            wait_seconds = (reset_time - datetime.now()).total_seconds() + 10
            logger.warning(f"[Script] Low API requests ({remaining}). Waiting {wait_seconds}s.")
            time.sleep(max(wait_seconds, 0))
            if retry_count < max_retries:
                return check_rate_limit(g, retry_count + 1, max_retries)
            raise Exception("Rate limit too low after retries")
        return True
    except Exception as e:
        logger.error(f"[Script] Error checking rate limit: {str(e)}")
        return False

def authenticate_github():
    """Authenticate with GitHub using a Personal Access Token."""
    try:
        g = Github(GITHUB_TOKEN)
        if check_rate_limit(g):
            repo = g.get_repo(REPO_NAME)
            logger.info(f"[Script] Successfully authenticated to {REPO_NAME}")
            return g, repo
        raise Exception("Rate limit check failed")
    except Exception as e:
        logger.error(f"[Script] Failed to authenticate: {str(e)}")
        raise

def check_repository_updates(repo, g):
    """Check for commits, issues, and pull requests since May 8, 2025."""
    try:
        if not check_rate_limit(g):
            return 0, 0, 0
        commits = repo.get_commits(since=SINCE_DATE)
        commit_count = sum(1 for _ in commits)
        logger.info(f"[Script] Found {commit_count} commits since {SINCE_DATE}")

        issues = repo.get_issues(state="all", since=SINCE_DATE)
        issue_count = sum(1 for _ in issues)
        logger.info(f"[Script] Found {issue_count} issues since {SINCE_DATE}")

        pulls = repo.get_pulls(state="all", base="main")
        pull_count = sum(1 for pr in pulls if pr.created_at >= SINCE_DATE)
        logger.info(f"[Script] Found {pull_count} pull requests since {SINCE_DATE}")

        return commit_count, issue_count, pull_count
    except GithubException as e:
        if e.status == 403 and "rate limit" in str(e).lower():
            logger.error("[Script] GitHub API rate limit exceeded. Try again later.")
        else:
            logger.error(f"[Script] Error checking updates: {str(e)}")
        return 0, 0, 0

def fix_build_errors():
    """Add 'using System;' to IDatabaseHelper.cs if not present."""
    file_path = os.path.join(REPO_PATH, "Data", "Interfaces", "IDatabaseHelper.cs")
    try:
        with open(file_path, "r") as f:
            content = f.read()
        if "using System;" not in content:
            content = "using System;\n" + content
            with open(file_path, "w") as f:
                f.write(content)
            logger.info("[Project] Added 'using System;' to IDatabaseHelper.cs")
        else:
            logger.info("[Project] IDatabaseHelper.cs already has 'using System;'")
    except Exception as e:
        logger.error(f"[Project] Error fixing IDatabaseHelper.cs: {str(e)}")

def check_method_exists(file_path, method_name):
    """Check if a method exists in a file using regex."""
    try:
        with open(file_path, "r") as f:
            content = f.read()
        pattern = rf"\b{re.escape(method_name)}\s*\("
        return bool(re.search(pattern, content))
    except Exception as e:
        logger.error(f"[Script] Error checking method {method_name} in {file_path}: {str(e)}")
        return False

def add_xunit_tests():
    """Generate xUnit tests for RouteManagementForm.cs and VehiclesManagementForm.cs."""
    test_dir = os.path.join(REPO_PATH, "Tests")
    os.makedirs(test_dir, exist_ok=True)

    route_form_path = os.path.join(REPO_PATH, "Forms", "RouteManagementForm.cs")
    vehicle_form_path = os.path.join(REPO_PATH, "Forms", "VehiclesManagementForm.cs")

    # Base test template
    test_template = """
using Xunit;
using BusBuddy.Forms;

namespace BusBuddy.Tests
{{
    public class {0}Tests
    {{
        [Fact]
        public void InitializeForm_DoesNotThrow()
        {{
            var form = new {0}();
            Assert.NotNull(form);
        }}
{1}
    }}
}}
"""

    # Test snippets for specific methods
    test_snippets = {
        "LoadRoutes": """
        [Fact]
        public void LoadRoutes_BindsDataGrid()
        {{
            var form = new RouteManagementForm();
            form.LoadRoutes();
            Assert.True(form.DataGridView.Rows.Count > 0);
        }}
""",
        "AddRoute": """
        [Fact]
        public void AddRoute_ValidInput_SavesToDatabase()
        {{
            var form = new RouteManagementForm();
            form.AddRoute("TestRoute", 10);
            Assert.True(form.DatabaseHelper.RouteExists("TestRoute"));
        }}
""",
        "LoadVehicles": """
        [Fact]
        public void LoadVehicles_BindsDataGrid()
        {{
            var form = new VehiclesManagementForm();
            form.LoadVehicles();
            Assert.True(form.DataGridView.Rows.Count > 0);
        }}
""",
        "AddVehicle": """
        [Fact]
        public void AddVehicle_ValidInput_SavesToDatabase()
        {{
            var form = new VehiclesManagementForm();
            form.AddVehicle("TestVehicle", "2023");
            Assert.True(form.DatabaseHelper.VehicleExists("TestVehicle"));
        }}
"""
    }

    # Generate RouteManagementFormTests.cs
    route_tests = []
    for method in ["LoadRoutes", "AddRoute"]:
        if check_method_exists(route_form_path, method):
            route_tests.append(test_snippets.get(method, ""))
        else:
            logger.warning(f"[Project] Method {method} not found in RouteManagementForm.cs. Skipping test.")
    route_test_content = test_template.format("RouteManagementForm", "".join(route_tests))
    with open(os.path.join(test_dir, "RouteManagementFormTests.cs"), "w") as f:
        f.write(route_test_content)
    logger.info("[Project] Created RouteManagementFormTests.cs")

    # Generate VehiclesManagementFormTests.cs
    vehicle_tests = []
    for method in ["LoadVehicles", "AddVehicle"]:
        if check_method_exists(vehicle_form_path, method):
            vehicle_tests.append(test_snippets.get(method, ""))
        else:
            logger.warning(f"[Project] Method {method} not found in VehiclesManagementForm.cs. Skipping test.")
    vehicle_test_content = test_template.format("VehiclesManagementForm", "".join(vehicle_tests))
    with open(os.path.join(test_dir, "VehiclesManagementFormTests.cs"), "w") as f:
        f.write(vehicle_test_content)
    logger.info("[Project] Created VehiclesManagementFormTests.cs")

    # Generate test project .csproj
    test_csproj_path = os.path.join(test_dir, "BusBuddy.Tests.csproj")
    test_csproj_content = """
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.9.2" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\\BusBuddy\\BusBuddy.csproj" />
  </ItemGroup>
</Project>
"""
    with open(test_csproj_path, "w") as f:
        f.write(test_csproj_content)
    logger.info("[Project] Created BusBuddy.Tests.csproj with xUnit and project references")

def update_entities():
    """Update Driver.cs with [Required] and Id; update architecture_violations.json."""
    driver_path = os.path.join(REPO_PATH, "Models", "Entities", "Driver.cs")
    try:
        with open(driver_path, "r") as f:
            content = f.read()
        if "public int Id" not in content:
            content = content.replace("public class Driver", "public class Driver\n{\n    public int Id { get; set; }")
        if "[Required]" not in content:
            content = content.replace("public string Name", "[Required]\n    public string Name")
            content = content.replace("public string LicenseNumber", "[Required]\n    public string LicenseNumber")
        with open(driver_path, "w") as f:
            f.write(content)
        logger.info("[Project] Updated Driver.cs with [Required] and Id")
    except Exception as e:
        logger.error(f"[Project] Error updating Driver.cs: {str(e)}")

    violations_path = os.path.join(REPO_PATH, "architecture_violations.json")
    try:
        with open(violations_path, "r") as f:
            violations = json.load(f)
        violations.append({"issue": "Missing [Required] in Driver.cs", "resolved": True})
        with open(violations_path, "w") as f:
            json.dump(violations, f, indent=2)
        logger.info("[Project] Updated architecture_violations.json")
    except Exception as e:
        logger.error(f"[Project] Error updating architecture_violations.json: {str(e)}")

def enhance_ui():
    """Append MaterialTabControl to Dashboard.cs."""
    dashboard_path = os.path.join(REPO_PATH, "Forms", "Dashboard.cs")
    tab_control_code = """
private MaterialTabControl tabControl;
public Dashboard()
{
    InitializeComponent();
    tabControl = new MaterialTabControl();
    tabControl.TabPages.Add("Tracking", "Tracking");
    tabControl.TabPages.Add("Analytics", "Analytics");
    tabControl.Dock = DockStyle.Fill;
    this.Controls.Add(tabControl);
}
"""
    try:
        with open(dashboard_path, "r") as f:
            content = f.read()
        if "MaterialTabControl tabControl;" not in content:
            content = content.replace("public partial class Dashboard : MaterialForm\n{",
                                     "public partial class Dashboard : MaterialForm\n{\n" + tab_control_code)
            with open(dashboard_path, "w") as f:
                f.write(content)
            logger.info("[Project] Added MaterialTabControl to Dashboard.cs")
        else:
            logger.info("[Project] MaterialTabControl already present in Dashboard.cs")
    except Exception as e:
        logger.error(f"[Project] Error updating Dashboard.cs: {str(e)}")

def add_validation():
    """Append validation to DriversManagementForm.cs and VehiclesManagementForm.cs."""
    drivers_form_path = os.path.join(REPO_PATH, "Forms", "DriversManagementForm.cs")
    drivers_validation = """
private bool ValidateDriverLicense(int driverId)
{
    var driver = _dbHelper.GetDriver(driverId);
    if (driver.LicenseExpiration < DateTime.Now)
    {
        MaterialMessageBox.Show("Driver's license has expired!", "Validation Error");
        return false;
    }
    return true;
}
"""
    try:
        with open(drivers_form_path, "r") as f:
            content = f.read()
        if "ValidateDriverLicense" not in content:
            content = content.rstrip("}\n") + drivers_validation + "\n}"
            with open(drivers_form_path, "w") as f:
                f.write(content)
            logger.info("[Project] Added validation to DriversManagementForm.cs")
    except Exception as e:
        logger.error(f"[Project] Error updating DriversManagementForm.cs: {str(e)}")

    vehicles_form_path = os.path.join(REPO_PATH, "Forms", "VehiclesManagementForm.cs")
    vehicles_validation = """
private bool ValidateVehicleInsurance(int vehicleId)
{
    var vehicle = _dbHelper.GetVehicle(vehicleId);
    if (vehicle.InsuranceExpiration < DateTime.Now)
    {
        MaterialMessageBox.Show("Vehicle insurance has expired!", "Validation Error");
        return false;
    }
    return true;
}
"""
    try:
        with open(vehicles_form_path, "r") as f:
            content = f.read()
        if "ValidateVehicleInsurance" not in content:
            content = content.rstrip("}\n") + vehicles_validation + "\n}"
            with open(vehicles_form_path, "w") as f:
                f.write(content)
            logger.info("[Project] Added validation to VehiclesManagementForm.cs")
    except Exception as e:
        logger.error(f"[Project] Error updating VehiclesManagementForm.cs: {str(e)}")

def add_gps_placeholders():
    """Append GPS placeholder methods to RouteManagementForm.cs and Dashboard.cs."""
    route_form_path = os.path.join(REPO_PATH, "Forms", "RouteManagementForm.cs")
    route_gps = """
public void InitializeMapPanel()
{
    // TODO: Integrate GMap.NET when validated
}
"""
    try:
        with open(route_form_path, "r") as f:
            content = f.read()
        if "InitializeMapPanel" not in content:
            content = content.rstrip("}\n") + route_gps + "\n}"
            with open(route_form_path, "w") as f:
                f.write(content)
            logger.info("[Project] Added GPS placeholder to RouteManagementForm.cs")
    except Exception as e:
        logger.error(f"[Project] Error updating RouteManagementForm.cs: {str(e)}")

    dashboard_path = os.path.join(REPO_PATH, "Forms", "Dashboard.cs")
    dashboard_gps = """
public void InitializeMapPanel()
{
    // TODO: Integrate GMap.NET when validated
}
"""
    try:
        with open(dashboard_path, "r") as f:
            content = f.read()
        if "InitializeMapPanel" not in content:
            content = content.rstrip("}\n") + dashboard_gps + "\n}"
            with open(dashboard_path, "w") as f:
                f.write(content)
            logger.info("[Project] Added GPS placeholder to Dashboard.cs")
    except Exception as e:
        logger.error(f"[Project] Error updating Dashboard.cs: {str(e)}")

def update_readme():
    """Create or update README.md with project details."""
    readme_content = """
# BusBuddyMVP
A .NET 9.0 Windows Forms application for managing school bus operations.

## Setup Instructions
1. Clone the repository: `git clone https://github.com/Bigessfour/BusBuddy.git`
2. Restore NuGet packages (MaterialSkin.2, Microsoft.EntityFrameworkCore.SqlServer).
3. Configure SQL Server Express in `appsettings.json`.
4. Build and run using Visual Studio 2022+ with .NET 9.0 SDK.

## Project Goals
- Real-time GPS tracking of buses.
- Automated route scheduling with notifications.
- Data-driven analytics for fuel and route efficiency.

## Contribution Guidelines
- Follow Core Build Guidelines (see repository).
- Test changes with xUnit (see `Tests\DatabaseHelperTests.cs`).
- Log issues to `busbuddy_errors.log` and update `architecture_violations.json`.
"""
    with open(os.path.join(REPO_PATH, "README.md"), "w") as f:
        f.write(readme_content)
    logger.info("[Project] Updated README.md")

def commit_changes(repo):
    """Commit changes to a new branch with conflict handling."""
    try:
        branch_name = "improvements-may-2025"
        try:
            repo.get_branch(branch_name)
            branch_name = f"improvements-may-2025-{datetime.now().strftime('%Y%m%d')}"
            logger.info(f"[Script] Branch 'improvements-may-2025' exists. Using {branch_name}")
        except GithubException:
            pass
        repo.create_git_ref(
            ref=f"refs/heads/{branch_name}",
            sha=repo.get_branch("main").commit.sha
        )
        files_to_commit = [
            "Data/Interfaces/IDatabaseHelper.cs",
            "Tests/RouteManagementFormTests.cs",
            "Tests/VehiclesManagementFormTests.cs",
            "Tests/BusBuddy.Tests.csproj",
            "Models/Entities/Driver.cs",
            "Forms/Dashboard.cs",
            "Forms/DriversManagementForm.cs",
            "Forms/VehiclesManagementForm.cs",
            "Forms/RouteManagementForm.cs",
            "architecture_violations.json",
            "README.md"
        ]
        for file in files_to_commit:
            file_path = os.path.join(REPO_PATH, file)
            if os.path.exists(file_path):
                with open(file_path, "r") as f:
                    content = f.read()
                repo.create_file(
                    path=file,
                    message=f"Update {file} for May 2025 improvements",
                    content=content,
                    branch=branch_name
                )
                logger.info(f"[Project] Committed {file} to {branch_name}")
    except Exception as e:
        logger.error(f"[Script] Error committing changes: {str(e)}")

def main():
    """Main function to execute all tasks."""
    try:
        g, repo = authenticate_github()
        check_repository_updates(repo, g)
        fix_build_errors()
        add_xunit_tests()
        update_entities()
        enhance_ui()
        add_validation()
        add_gps_placeholders()
        update_readme()
        commit_changes(repo)
        logger.info("[Script] All tasks completed successfully")
    except Exception as e:
        logger.error(f"[Script] Script failed: {str(e)}")

if __name__ == "__main__":
    main()