/* 
 * Copyright (C) 2017 Dominic Maas
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
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
            ProgressRing.IsActive = true;
            var videoUrl = await VideoHelper.GetVideoStreamUrlAsync(param);
            ProgressRing.IsActive = false;


            NowPlaying.Text = param.Title.ToUpper();


            this.MediaElement.Source = new Uri(videoUrl);
            this.MediaElement.Play();
        }

        private void MediaElement_OnCurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (MediaElement.CurrentState)
            {
                case MediaElementState.Closed:
                    ProgressRing.IsActive = false;
                    break;
                case MediaElementState.Buffering:
                    ProgressRing.IsActive = true;
                    break;
                case MediaElementState.Opening:
                    ProgressRing.IsActive = true;
                    break;
                case MediaElementState.Paused:
                    ProgressRing.IsActive = false;
                    break;
                case MediaElementState.Playing:
                    ProgressRing.IsActive = false;
                    break;
                case MediaElementState.Stopped:
                    ProgressRing.IsActive = false;
                    break;
            }


        }
    }
}
