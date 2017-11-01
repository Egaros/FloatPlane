/* 
 * Copyright (C) 2017 Dominic Maas
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using FloatPlane.Models;

namespace FloatPlane.Helpers
{
    public static class VideoHelper
    {
        public static async Task<string> GetVideoStreamUrlAsync(VideoModel video, bool download = false)
        {
            // Get the video URL (720p for now)
            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                    new Uri($"https://linustechtips.com/main/applications/floatplane/interface/video_url.php?video_guid={video.Id}&video_quality=720&download=" + (download ? "1" : "0")));

                using (var request = await client.SendRequestAsync(requestMessage))
                {
                    // Ensure the request was successful
                    request.EnsureSuccessStatusCode();

                    // Read the content
                    var content = await request.Content.ReadAsStringAsync();

                    // The content is a url, so we return it
                    return content;
                }
            }
        }
    }
}
