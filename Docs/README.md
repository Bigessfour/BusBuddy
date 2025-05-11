# MessageBoxHandler for BusBuddy Tests

## Overview

The `MessageBoxHandler` is a utility class for handling Windows Forms MessageBox dialogs during automated tests. It automatically detects and closes MessageBox dialogs that appear during test execution, preventing tests from hanging indefinitely waiting for user input.

## Problem Solved

In Windows Forms applications, tests can hang when code under test displays MessageBox dialogs because these dialogs require user interaction. The `MessageBoxHandler` automatically dismisses these dialogs during test execution, allowing tests to continue running without manual intervention.

## How It Works

1. The `MessageBoxHandler` uses Win32 API calls to:
   - Find dialog windows shown during test execution
   - Automatically click buttons (OK, Yes, etc.) to dismiss dialogs
   - Record information about the dialogs for test assertions

2. It runs on a background thread that monitors for new dialogs while tests execute

3. The `MessageBoxHandlerTestBase` class makes it easy to add this capability to any test class

## Usage

### Basic Usage

To use the MessageBoxHandler in your tests:

1. Make your test class inherit from `MessageBoxHandlerTestBase`:

```csharp
public class MyTests : MessageBoxHandlerTestBase
{
    // Your test methods here
}
```

2. In your test methods, you can assert that a MessageBox was shown:

```csharp
[Fact]
public void MyTest()
{
    // Call code that shows a MessageBox
    ClearCapturedDialogs(); // Clear any previous dialogs
    myObject.MethodThatShowsMessageBox();
    
    // Assert that the MessageBox was shown
    Assert.Contains(CapturedDialogs, dialog => dialog.Title == "Expected Title");
}
```

### Advanced Usage

You can control which button gets automatically clicked when a MessageBox appears:

```csharp
// Use OK button (default)
public class MyTests : MessageBoxHandlerTestBase
{
    public MyTests() : base(1) { } // 1 = IDOK (OK button)
}

// Use Yes button
public class YesTests : MessageBoxHandlerTestBase
{
    public YesTests() : base(6) { } // 6 = IDYES (Yes button)
}
```

Button ID values:
- 1: OK
- 2: Cancel
- 6: Yes
- 7: No

## Implementation Details

- Uses P/Invoke to access Win32 APIs like FindWindow, FindWindowEx, and SendMessage
- Runs a background task to continuously check for dialogs
- Thread-safe implementation using locks for shared state
- Properly implements IDisposable to clean up resources

## Troubleshooting

If tests are still hanging:

1. Make sure your test class inherits from `MessageBoxHandlerTestBase`
2. Call `ClearCapturedDialogs()` before executing code that might show dialogs
3. Verify that the correct button ID is being used for auto-clicking (default is IDOK = 1)
4. Check if non-standard dialog windows are being used that might not be detected
