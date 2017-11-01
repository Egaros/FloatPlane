using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FloatPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomeView : Page
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginView));
        }

        private void AutomaticDownloadSetup(object sender, RoutedEventArgs e)
        {

        }
    }
}
