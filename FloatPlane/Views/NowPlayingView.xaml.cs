/* 
 * Copyright (C) 2017 Dominic Maas
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */

using System;
using Windows.UI.Xaml.Navigation;
using FloatPlane.Helpers;
using FloatPlane.Models;

namespace FloatPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NowPlayingView
    {
        public NowPlayingView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is VideoModel param))
                return;

            // Get the video URL
            var videoUrl = await VideoHelper.GetVideoStreamUrlAsync(param);

            this.MediaElement.Source = new Uri(videoUrl);
            this.MediaElement.Play();
        }
    }
}
