// BusBuddy/AppSettings.cs
namespace BusBuddy
{
    public static class AppSettings
    {
        public static class Database
        {
            public const int MaxRetries = 3;
            public const int RetryDelayMs = 500;
        }

        public static class Theme
        {
            public static readonly System.Drawing.Color BackgroundColor = System.Drawing.Color.FromArgb(240, 248, 255); // Alice Blue
            public static readonly System.Drawing.Color GroupBoxColor = System.Drawing.Color.FromArgb(245, 245, 245); // Light Gray
            public static readonly System.Drawing.Color AccentColor = System.Drawing.Color.FromArgb(0, 102, 204); // Blue
            public static readonly System.Drawing.Color SuccessColor = System.Drawing.Color.Green;
            public static readonly System.Drawing.Color ErrorColor = System.Drawing.Color.Red;
            public static readonly System.Drawing.Color InfoColor = System.Drawing.Color.Blue;
            public static readonly System.Drawing.Color TextColor = System.Drawing.Color.Black;
            public static readonly System.Drawing.Color PanelColor = System.Drawing.Color.FromArgb(250, 250, 250); // Light Panel
            public static readonly System.Drawing.Color PanelHeaderColor = System.Drawing.Color.FromArgb(230, 240, 250); // Light Header
            public static readonly System.Drawing.Font LabelFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            public static readonly System.Drawing.Font NormalFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            public static readonly System.Drawing.Font HeaderFont = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            public static readonly System.Drawing.Font SubheaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            public static readonly System.Drawing.Font ButtonFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        }
    }
}