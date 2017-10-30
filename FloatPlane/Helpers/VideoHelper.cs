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
using HtmlAgilityPack;

namespace FloatPlane.Helpers
{
    public static class VideoHelper
    {
        public static async Task<string> GetVideoStreamUrlAsync(VideoModel video)
        {
            // If the ID is null, we have to find the ID
            if (string.IsNullOrEmpty(video.Id))
            {
                using (var client = new HttpClient())
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                        new Uri(video.Url));

                    using (var request = await client.SendRequestAsync(requestMessage))
                    {
                        // Ensure the request was successful
                        request.EnsureSuccessStatusCode();

                        // Read the content
                        var content = await request.Content.ReadAsStringAsync();

                        // Load the document
                        var doc = new HtmlDocument();
                        doc.LoadHtml(content);

                        var i = 0;
                    }

                }
            }

            // Get the video URL (1080p for now)
            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                    new Uri($"https://linustechtips.com/main/applications/floatplane/interface/video_url.php?video_guid={video.Id}&video_quality=1080&download=1"));

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
