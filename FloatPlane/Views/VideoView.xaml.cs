
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Xml;

namespace FloatPlane.Views
{
    /// <summary>
    /// Shows a list of float plane videos
    /// </summary>
    public sealed partial class VideoView : Page
    {
        // This url is where we can get the videos
        private const string RssFeedUrl = "https://linustechtips.com/main/forum/91-the-floatplane-club.xml/";



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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var result = await client.SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, new Uri(RssFeedUrl)));
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                // Load the XML
                var doc = new XmlDocument();
                doc.LoadXml(content);

                // Grab the list of videos and loop through them
                var videoList = doc.DocumentElement.SelectNodes("//channel/item");
                foreach (XmlNode video in videoList)
                {
                    var title = video.ChildNodes[0].InnerText;
                    var link = video.ChildNodes[1].InnerText;
                    var description = video.ChildNodes[2].InnerText;
                    var date = DateTime.Parse(video.ChildNodes[4].InnerText);

                    Videos.Add(new VideoItem { Title = title, Url = link });
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
