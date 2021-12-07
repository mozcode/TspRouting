﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleApi.Entities.Places.Common
{
    /// <summary>
    /// Photo.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// PhotoReference — a string used to identify the photo when you perform a Photo request.
        /// </summary>
        [JsonProperty("photo_reference")]
        public virtual string PhotoReference { get; set; }

        /// <summary>
        /// Height — the maximum height of the image.
        /// </summary>
        [JsonProperty("height")]
        public virtual int Height { get; set; }

        /// <summary>
        /// Width — the maximum width of the image.
        /// </summary>
        [JsonProperty("width")]
        public virtual int Width { get; set; }

        /// <summary>
        /// HtmlAttributions — contains any required attributions. 
        /// This field will always be present, but may be empty.
        /// </summary>
        [JsonProperty("html_attributions")]
        public virtual IEnumerable<string> HtmlAttributions { get; set; }
    }
}