using Microsoft.UI.Xaml.Controls;
using SettingsUI.Demo.Pages;
using SettingsUI.ViewModel;

namespace SettingsUI.Demo
{
    public sealed partial class Shell : UserControl
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public Shell()
        {
            this.InitializeComponent();
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
            shellFrame.Navigate(typeof(GeneralPage));
        }
    }
}
