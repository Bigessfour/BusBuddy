// BusBuddy/MainFormNavigator.cs
using System.Windows.Forms;

namespace BusBuddy
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