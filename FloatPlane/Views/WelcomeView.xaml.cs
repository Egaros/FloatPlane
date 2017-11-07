using System;
using Windows.UI.Xaml;
using FloatPlane.Dialogs;

namespace FloatPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomeView
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginView));
        }

        private  async void AutomaticDownloadSetup(object sender, RoutedEventArgs e)
        {
            await new SetupDownloadDialog().ShowAsync();
        }
    }
}
