﻿using Newtonsoft.Json;

namespace GoogleApi.Entities.Maps.Common
{
	/// <summary>
	/// Distance.
	/// </summary>
	public class Distance
    {
        /// <summary>
        /// Value indicates the distance in meters
        /// </summary>
        [JsonProperty("value")]
        public virtual int Value { get; set; }

        /// <summary>
        /// Text contains a human-readable representation of the distance, displayed in units as used at the origin, in the language specified in the request. 
        /// (For example, miles and feet will be used for any origin within the United States.) Note that regardless of what unit system is displayed as text, 
        /// the distance.value field always contains a value expressed in meters.
        /// </summary>
        [JsonProperty("text")]
        public virtual string Text { get; set; }
    }
}