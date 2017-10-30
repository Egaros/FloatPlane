/* 
 * Copyright (C) 2017 Dominic Maas
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */

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


            Videos.OnStartLoading += () =>
            {
                ProgressRing.IsActive = true;
            };

            Videos.OnEndLoading += () =>
            {
                ProgressRing.IsActive = false;
            };
        }

        public void NavigateToVideo(object sender, ItemClickEventArgs e)
        {
            // Grab the video item
            var item = e.ClickedItem as VideoModel;
            // Navigate to the now playing page with the video
            Frame.Navigate(typeof(NowPlayingView), item);
        }
    }
}
