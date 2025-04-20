// BusBuddy/MainFormNavigator.cs
using System.Windows.Forms;
using BusBuddy.UI.Interfaces; // Add self-reference for IFormNavigator
using BusBuddy.Utilities; // For FormFactory

namespace BusBuddy.UI.Interfaces
{
    public class MainFormNavigator : IFormNavigator
    {
        private readonly FormFactory _formFactory;

        public MainFormNavigator()
        {
            _formFactory = new FormFactory();
        }

        public void NavigateTo(string formName)
        {
            using (var form = _formFactory.CreateForm(formName))
            {
                form.ShowDialog();
            }
        }
    }
}