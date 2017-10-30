using Windows.UI.Xaml.Controls;
using FloatPlane.Models;
using FloatPlane.Sources;
using Microsoft.Toolkit.Uwp;

namespace FloatPlane.Views
{
    /// <summary>
    /// Shows a list of float plane videos
    /// </summary>
    public sealed partial class VideoView : Page
    {
        /// <summary>
        /// This source collection allows us to view all float plane videos 
        /// using the RSS feed. Currently this grabs the entire float plane history.
        /// </summary>
        public IncrementalLoadingCollection<RecentVideoSource, VideoModel> Videos { get; } = new IncrementalLoadingCollection<RecentVideoSource, VideoModel>();

        public VideoView()
        {
            InitializeComponent();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as VideoModel;

            Frame.Navigate(typeof(NowPlayingView), item);
        }
    }
}
