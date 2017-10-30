using System;

namespace FloatPlane.Models
{
    public class VideoModel
    {
        /// <summary>
        /// Title of the video
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url to the video page (float plane)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The time this object was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Video GUID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Link to the video image url
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
