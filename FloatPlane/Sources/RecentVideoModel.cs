using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FloatPlane.Models;
using Windows.Web.Http;
using System.Xml;
using System.Text.RegularExpressions;

namespace FloatPlane.Sources
{
    /// <summary>
    /// A source that goes through the floatplane RSS feed
    /// </summary>
    public class RecentVideoSource : IIncrementalSource<VideoModel>
    {
        private const string RssFeedUrl = "https://linustechtips.com/main/forum/91-the-floatplane-club.xml";

        // The page we are on
        private int pageNumber = 1;

        public async Task<IEnumerable<VideoModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = new List<VideoModel>();

            // HTTP Client using cache from web view
            using (var client = new HttpClient())
            {
                // Grab the RSS feed with the next page
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(RssFeedUrl + "?page=" + pageNumber));

                using (var request = await client.SendRequestAsync(requestMessage))
                {
                    // Ensure the request was successful
                    request.EnsureSuccessStatusCode();

                    // Read the content
                    var content = await request.Content.ReadAsStringAsync();

                    // Load the XML
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(content);

                    // Grab the list of videos and loop through them
                    var videoList = xmlDocument.DocumentElement.SelectNodes("//channel/item");
                    foreach (XmlNode video in videoList)
                    {
                        // Get required fields
                        var title = video.ChildNodes[0].InnerText;
                        var link = video.ChildNodes[1].InnerText;
                        var description = video.ChildNodes[2].InnerText;
                        var date = DateTime.Parse(video.ChildNodes[4].InnerText);

                        // The description can sometimes contain the GUID (which makes life really easy and awesome) we need to grab that GUID.
                        // We get this using regex
                        var regex = new Regex("(data-video-guid=\\\")[^\"]*");
                        string guid = regex.Match(description)?.Value;

                        // If the guid does exist split by the quotation and take the
                        // second half (data-video-guid=G873FK)
                        if (!string.IsNullOrEmpty(guid))
                            guid = guid.Split('"')[1];

                        items.Add(new VideoModel
                        {
                            Id = guid,
                            Title = title,
                            Url = link,
                            Created = date.ToLocalTime(),
                            ImageUrl = "https://cms.linustechtips.com/get/thumbnails/by_guid/" + guid
                        });
                    }
                }
            }

            pageNumber++;

            return items;
        }
    }
}
