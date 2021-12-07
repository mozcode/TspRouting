﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleApi.Entities.Places.Details.Response
{
    /// <summary>
    /// Aspect.
    /// </summary>
    public class Aspect
    {
        /// <summary>
        /// Aspects contains a collection of AspectRating objects, each of which provides a rating of a single attribute of the establishment. 
        /// The first object in the collection is considered the primary aspect. Each AspectRating is described as:
        /// </summary>
        [JsonProperty("aspects")]
        public virtual IEnumerable<AspectRating> AspectRatings { get; set; }
    }
}