using Microsoft.EntityFrameworkCore;
using BusBuddy.Data;
using BusBuddy.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Configure SQLite with BusBuddyContext
var useSqlite = Environment.GetEnvironmentVariable("USE_SQLITE")?.ToLower() == "true";
if (useSqlite)
{
    var sqliteConnectionString = builder.Configuration.GetConnectionString("SqliteConnection") ?? "Data Source=/data/BusBuddy.db";
    Console.WriteLine($"Using SQLite connection: {sqliteConnectionString}");
    builder.Services.AddDbContext<BusBuddyContext>(options => options.UseSqlite(sqliteConnectionString));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine("Using SQL Server connection");
    builder.Services.AddDbContext<BusBuddyContext>(options => options.UseSqlServer(connectionString));
}

// Register IBusScheduleHelper with BusScheduleHelper implementation
builder.Services.AddScoped<IBusScheduleHelper, BusScheduleHelper>();

// Add logging
builder.Services.AddLogging(logging => 
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BusBuddyContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// Redirect root to Dashboard
app.MapGet("/", context => {
    context.Response.Redirect("/Dashboard");
    return Task.CompletedTask;
});

app.Run();