using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Utilities;
using MaterialSkin.Controls;

namespace BusBuddy.UI.Forms
{
    public static class FormManager
    {
        private static readonly Dictionary<string, Type> _formRegistry = new Dictionary<string, Type>();
        private static readonly Dictionary<string, Form> _formInstances = new Dictionary<string, Form>();
        private static readonly ILogger _logger = CreateLogger("FormManager");
        
        static FormManager()
        {
            // Register all form types here
            RegisterFormType<Welcome>("Welcome");
            RegisterFormType<VehiclesForm>("Vehicles");
            RegisterFormType<FuelForm>("Fuel");
            RegisterFormType<DriverForm>("Driver");
            RegisterFormType<MaintenanceForm>("Maintenance");
            RegisterFormType<SchoolCalendarForm>("SchoolCalendar");
            RegisterFormType<ActivityForm>("Activity");
            RegisterFormType<RoutesForm>("Routes");
            RegisterFormType<ScheduledRoutesForm>("ScheduledRoutes");
            RegisterFormType<TripSchedulerForm>("TripScheduler");
            RegisterFormType<Settings>("Settings");
            
            // Initialize ThemeManager
            ThemeManager.Initialize();
        }
        
        /// <summary>
        /// Registers a form type for later instantiation
        /// </summary>
        /// <typeparam name="T">The form type to register</typeparam>
        /// <param name="formName">The name to register the form under</param>
        public static void RegisterFormType<T>(string formName) where T : Form
        {
            if (!_formRegistry.ContainsKey(formName))
            {
                _formRegistry.Add(formName, typeof(T));
                _logger.Debug($"Registered form type: {formName} -> {typeof(T).Name}");
            }
        }
        
        /// <summary>
        /// Creates a new instance of a form by name
        /// </summary>
        /// <param name="formName">The registered name of the form to create</param>
        /// <returns>A new instance of the requested form</returns>
        private static Form CreateForm(string formName)
        {
            if (_formRegistry.TryGetValue(formName, out Type formType))
            {
                try
                {
                    Form form = (Form)Activator.CreateInstance(formType);
                    return form;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Failed to create form: {formName}");
                    throw;
                }
            }
            
            _logger.Error($"Form type not registered: {formName}");
            throw new ArgumentException($"Form '{formName}' is not registered.");
        }
        
        /// <summary>
        /// Gets an instance of a form, creating it if necessary
        /// </summary>
        /// <param name="formName">The registered name of the form to get</param>
        /// <param name="createNew">Whether to create a new instance even if one exists</param>
        /// <returns>The requested form instance</returns>
        public static Form GetForm(string formName, bool createNew = false)
        {
            // Return existing instance if available and not requesting a new one
            if (!createNew && _formInstances.TryGetValue(formName, out Form existingForm) && !existingForm.IsDisposed)
            {
                _logger.Debug($"Returning existing form instance: {formName}");
                return existingForm;
            }
            
            // Create a new instance
            Form form = CreateForm(formName);
            
            // Store the instance unless explicitly told not to
            if (!createNew)
            {
                // Replace any existing instance
                if (_formInstances.ContainsKey(formName))
                {
                    _formInstances[formName] = form;
                }
                else
                {
                    _formInstances.Add(formName, form);
                }
            }
            
            _logger.Debug($"Created new form instance: {formName}");
            return form;
        }
        
        /// <summary>
        /// Displays a form by name
        /// </summary>
        /// <param name="formName">The registered name of the form to display</param>
        /// <param name="parentForm">The parent form to hide when displaying the new form</param>
        /// <param name="createNew">Whether to create a new instance even if one exists</param>
        public static void DisplayForm(string formName, Form parentForm = null, bool createNew = false)
        {
            // Get or create the form
            Form form = GetForm(formName, createNew);
            
            // Display the form with proper parent-child relationship
            DisplayFormWithProperParent(form, parentForm);
        }
        
        /// <summary>
        /// Displays a form with proper parent-child relationship
        /// </summary>
        private static void DisplayFormWithProperParent(Form form, Form parentForm)
        {
            // Ensure consistent styling is applied right before showing the form
            try 
            {
                // Apply Material Design styling based on the form type
                if (form is MaterialForm materialForm)
                {
                    ThemeManager.ApplyTheme(materialForm);
                }
                else
                {
                    // For non-Material forms, apply traditional styling
                    ThemeManager.StyleControl(form);
                }
            }
            catch (Exception ex) 
            {
                _logger.Warning(ex, "Failed to apply styling to form {FormName}. Will continue with default styling.", form.Name);
            }
            
            if (parentForm != null)
            {
                parentForm.Hide();
                form.FormClosed += (s, e) => { 
                    if (!parentForm.IsDisposed) 
                    {
                        parentForm.Show();
                        parentForm.BringToFront();
                    } 
                };
                form.ShowDialog();
            }
            else
            {
                if (form.WindowState == FormWindowState.Minimized)
                {
                    form.WindowState = FormWindowState.Normal;
                }
                form.Show();
                form.BringToFront();
            }
        }
        
        /// <summary>
        /// Closes all open forms
        /// </summary>
        public static void CloseAllForms()
        {
            foreach (Form form in _formInstances.Values)
            {
                if (form != null && !form.IsDisposed)
                {
                    _logger.Debug($"Closing form: {form.Name}");
                    form.Close();
                }
            }
            
            _formInstances.Clear();
        }
        
        /// <summary>
        /// Creates a logger for a specified component
        /// </summary>
        /// <param name="componentName">The name of the component to create a logger for</param>
        /// <returns>A configured logger instance</returns>
        public static ILogger GetLogger(string componentName)
        {
            return Serilog.Log.Logger.ForContext("Component", componentName);
        }
        
        /// <summary>
        /// Creates a logger instance with a default configuration
        /// </summary>
        private static ILogger CreateLogger(string name)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File($"logs/busbuddy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()
                .ForContext("Component", name);
        }
    }
}