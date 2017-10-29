using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using HtmlAgilityPack;

namespace FloatPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NowPlayingView : Page
    {
        public NowPlayingView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is VideoView.VideoItem param))
                return;

            TextBlock.Text = param.Title;

            using (var client = new HttpClient())
            {
               

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(param.Url));
                // add any request-specific headers here
                // more code been omitted
                var result = await client.SendRequestAsync(request);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();
                // now we can do whatever we need with the html content we got here 

                var doc = new HtmlDocument();
                doc.LoadHtml(content);


                

                var src = doc.DocumentNode.SelectNodes("//iframe[@src]")[0].Attributes["src"].Value;

                var view = new WebView();
                view.DOMContentLoaded += async (sender, args) =>
                {
                    await Task.Delay(1000);

                    var html = await view.InvokeScriptAsync("eval", new [] { "document.documentElement.outerHTML;" });

                    

                    doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var vidsrc = doc.DocumentNode.SelectSingleNode("//*[@id=\"floatplane_player_html5_api\"]").Attributes["src"].Value;

                    this.MediaElement.Source = new Uri(vidsrc);
                    this.MediaElement.Play();


                    var i = 0;
                };


                view.Navigate(new Uri(src));        
            }
        }
    }
}
