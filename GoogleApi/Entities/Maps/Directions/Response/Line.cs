using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleApi.Entities.Maps.Directions.Response
{
    /// <summary>
    /// Line
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Contains the full name of this transit line. eg. "7 Avenue Express".
        /// </summary>
        [JsonProperty("name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Contains the short name of this transit line. This will normally be a line number, such as "M7" or "355".
        /// </summary>
        [JsonProperty("short_name")]
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Contains the color commonly used in signage for this transit line. The color will be specified as a hex string such as: #FF0033. 
        /// </summary>
        [JsonProperty("color")]
        public virtual string Color { get; set; }

        /// <summary>
        /// Contains a List of TransitAgency objects that each provide information about the operator of the line.
        /// </summary>
        [JsonProperty("agencies")]
        public virtual List<TransitAgency> Agencies { get; set; }

        /// <summary>
        /// Contains the URL for this transit line as provided by the transit agency.
        /// </summary>
        [JsonProperty("url")]
        public virtual string Url { get; set; }

        /// <summary>
        /// Contains the URL for the icon associated with this line.
        /// </summary>
        [JsonProperty("icon")]
        public virtual string Icon { get; set; }

        /// <summary>
        /// Contains the color of text commonly used for signage of this line. The color will be specified as a hex string.
        /// </summary>
        [JsonProperty("text_color")]
        public virtual string TextColor { get; set; }

        /// <summary>
        /// Contains the type of vehicle used on this line.
        /// </summary>
        [JsonProperty("vehicle")]
        public virtual Vehicle Vehicle { get; set; }
    }
}