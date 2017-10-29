
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using HtmlAgilityPack;
using System.Linq;
using Windows.UI.Xaml;

namespace FloatPlane.Views
{
    /// <summary>
    /// Shows a list of float plane videos
    /// </summary>
    public sealed partial class VideoView : Page
    {
        public class VideoItem
        {
            public string Title { get; set; }

            public string Url { get; set; }
        }

        public ObservableCollection<VideoItem> Videos { get; } = new ObservableCollection<VideoItem>();

        public VideoView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://linustechtips.com/main/forum/91-the-floatplane-club/"));
                // add any request-specific headers here
                // more code been omitted
                var result = await client.SendRequestAsync(request);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();
                // now we can do whatever we need with the html content we got here 

                var doc = new HtmlDocument();
                doc.LoadHtml(content);


                var nodes = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/main[1]/div[1]/div[1]/div[1]/div[3]/div[1]/ol[1]")?.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element);

                if (nodes != null)
                {
                    foreach (var htmlNode in nodes)
                    {
                        try
                        {
                            var title = htmlNode.ChildNodes[3].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText.Trim('\n');

                            var uri = htmlNode.ChildNodes[3].ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes
                                .FirstOrDefault(x => x.Name == "href");

                            if (!string.IsNullOrEmpty(title))
                            {

                                Videos.Add(new VideoItem { Title = Uri.UnescapeDataString(title), Url = uri?.Value});

                            }
                        }
                        catch
                        {
                            // Not today
                        }
                    }
                }

                


                
            }
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as VideoItem;

            Frame.Navigate(typeof(NowPlayingView), item);
        }
    }
}
