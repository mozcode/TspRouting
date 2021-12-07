﻿using System.Collections.Generic;
using GoogleApi.Entities.Maps.Geocoding.Common;
using Newtonsoft.Json;

namespace GoogleApi.Entities.Maps.Geocoding
{
    /// <summary>
    /// Geocode Response.
    /// </summary>
    public class GeocodeResponse : BaseResponse
    {
        /// <summary>
        /// Results.
        /// When the geocoder returns results, it places them within a (JSON) results array. 
        /// Even if the geocoder returns no results (such as if the address doesn't exist) it still returns an empty results array.
        /// </summary>
        [JsonProperty("results")]
        public virtual IEnumerable<Result> Results { get; set; }
    }
}