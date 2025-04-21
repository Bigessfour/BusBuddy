// BusBuddy/AppSettings.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms; // Required for Point and Size

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
            // Modern color palette
            public static readonly Color PrimaryColor = Color.FromArgb(25, 118, 210);     // Rich blue
            public static readonly Color SecondaryColor = Color.FromArgb(41, 182, 246);   // Light blue
            public static readonly Color AccentColor = Color.FromArgb(255, 152, 0);       // Orange accent
            public static readonly Color BackgroundColor = Color.FromArgb(250, 250, 250); // Off-white background
            public static readonly Color CardColor = Color.White;                         // Pure white for cards
            public static readonly Color NavBackgroundColor = Color.FromArgb(25, 118, 210); // Rich blue for nav
            public static readonly Color NavHoverColor = Color.FromArgb(21, 101, 192);    // Darker blue for hover
            
            // Status colors
            public static readonly Color SuccessColor = Color.FromArgb(76, 175, 80);      // Green
            public static readonly Color ErrorColor = Color.FromArgb(244, 67, 54);        // Red
            public static readonly Color WarningColor = Color.FromArgb(255, 152, 0);      // Orange
            public static readonly Color InfoColor = Color.FromArgb(33, 150, 243);        // Blue
            
            // Text colors
            public static readonly Color TextColor = Color.FromArgb(33, 33, 33);          // Near black
            public static readonly Color TextLightColor = Color.FromArgb(250, 250, 250);  // Off-white text
            public static readonly Color TextSecondaryColor = Color.FromArgb(117, 117, 117); // Gray
            
            // Panel colors
            public static readonly Color PanelColor = Color.White;
            public static readonly Color PanelHeaderColor = Color.FromArgb(232, 244, 255); // Light blue header
            public static readonly Color GroupBoxColor = Color.FromArgb(245, 245, 245);     // Light gray
            
            // Shadows and borders
            public static readonly Color ShadowColor = Color.FromArgb(0, 0, 0, 30);
            public static readonly Color BorderColor = Color.FromArgb(224, 224, 224);      // Light gray
            
            // Fonts
            public static readonly Font LabelFont = new Font("Segoe UI", 9F);
            public static readonly Font NormalFont = new Font("Segoe UI", 10F);
            public static readonly Font HeaderFont = new Font("Segoe UI", 18F, FontStyle.Bold);
            public static readonly Font SubheaderFont = new Font("Segoe UI", 14F, FontStyle.Bold);
            public static readonly Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            public static readonly Font NavButtonFont = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            public static readonly Font DataFont = new Font("Segoe UI", 9.5F);
        }

        public static class Layout
        {
            private static readonly string LayoutSettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.layout.json");
            private static Dictionary<string, FormLayoutSettings> _formLayouts = new Dictionary<string, FormLayoutSettings>();

            static Layout()
            {
                LoadLayoutSettings();
            }

            public class FormLayoutSettings
            {
                public Size FormSize { get; set; }
                public Dictionary<string, Point> ControlLocations { get; set; } = new Dictionary<string, Point>();
            }

            public static FormLayoutSettings GetLayout(string formName)
            {
                _formLayouts.TryGetValue(formName, out var settings);
                return settings; // Returns null if not found
            }

            public static void SaveLayout(string formName, Size formSize, Dictionary<string, Point> controlLocations)
            {
                if (string.IsNullOrEmpty(formName)) return;

                var settings = new FormLayoutSettings
                {
                    FormSize = formSize,
                    ControlLocations = controlLocations ?? new Dictionary<string, Point>()
                };
                _formLayouts[formName] = settings;
                SaveLayoutSettingsToFile();
            }

            private static void LoadLayoutSettings()
            {
                try
                {
                    if (File.Exists(LayoutSettingsFile))
                    {
                        var json = File.ReadAllText(LayoutSettingsFile);
                        _formLayouts = JsonSerializer.Deserialize<Dictionary<string, FormLayoutSettings>>(json) ?? new Dictionary<string, FormLayoutSettings>();
                    }
                    else
                    {
                        _formLayouts = new Dictionary<string, FormLayoutSettings>();
                    }
                }
                catch (Exception ex)
                {
                    // Log error or handle gracefully - for now, just reset
                    Console.WriteLine($"Error loading layout settings: {ex.Message}"); // Replace with proper logging if available
                    _formLayouts = new Dictionary<string, FormLayoutSettings>();
                }
            }

            private static void SaveLayoutSettingsToFile()
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var json = JsonSerializer.Serialize(_formLayouts, options);
                    File.WriteAllText(LayoutSettingsFile, json);
                }
                catch (Exception ex)
                {
                    // Log error or handle gracefully
                    Console.WriteLine($"Error saving layout settings: {ex.Message}"); // Replace with proper logging if available
                }
            }
        }
    }
}