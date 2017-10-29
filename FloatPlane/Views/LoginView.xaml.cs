using System;
using Windows.UI.Xaml.Navigation;

namespace FloatPlane.Views
{
    /// <summary>
    /// This page is designed to let the user login. This is required for scraping the website for videos.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WebView.ContentLoading += WebView_ContentLoading;
            WebView.DOMContentLoaded += WebView_DOMContentLoaded;

            WebView.Navigate(new Uri("https://linustechtips.com/main/forum/91-the-floatplane-club/"));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WebView.ContentLoading -= WebView_ContentLoading;
            WebView.DOMContentLoaded -= WebView_DOMContentLoaded;
        }

        private void WebView_DOMContentLoaded(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            ProgressRing.IsActive = false;
        }

        private void WebView_ContentLoading(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs args)
        {
            ProgressRing.IsActive = true;
        }

        private void StartProcessing(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(VideoView));
        }
    }
}
