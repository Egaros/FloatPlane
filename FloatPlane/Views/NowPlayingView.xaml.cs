using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using HtmlAgilityPack;
using FloatPlane.Models;

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
            if (!(e.Parameter is VideoModel param))
                return;

            if (!string.IsNullOrEmpty(param.Id))
            {
                var mediaSource = new Uri($"https://linustechtips.com/main/applications/floatplane/interface/video_url.php?video_guid={param.Id}&video_quality=1080&download=1");

                this.MediaElement.Source = mediaSource;
                this.MediaElement.Play();

                return;
            }

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

                // run in a webview so javascript works
                var view = new WebView();
                view.DOMContentLoaded += async (sender, args) =>
                {
                    // We have to delay for javascript to set the src value on video
                    await Task.Delay(1000);

                    var html = await view.InvokeScriptAsync("eval", new [] { "document.documentElement.outerHTML;" });

                    

                    doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var vidsrc = doc.DocumentNode.SelectSingleNode("//*[@id=\"floatplane_player_html5_api\"]").Attributes["src"].Value;

                    var mediaSource = await AdaptiveMediaSource.CreateFromUriAsync(new Uri(vidsrc));

                    this.MediaElement.SetMediaStreamSource(mediaSource.MediaSource);
                    this.MediaElement.Play();


                    var i = 0;
                };


                view.Navigate(new Uri(src));        
            }
        }
    }
}
