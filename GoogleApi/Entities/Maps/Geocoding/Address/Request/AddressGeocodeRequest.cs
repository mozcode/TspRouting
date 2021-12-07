﻿using System;
using System.Collections.Generic;
using System.Linq;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Common.Extensions;

namespace GoogleApi.Entities.Maps.Geocoding.Address.Request
{
    /// <summary>
    /// Geocoding Request.
    /// </summary>
    public class AddressGeocodeRequest : BaseGeocodeRequest
    {
        /// <summary>
        /// Address (required).
        /// The address that you want to geocode.
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Region (optional) — The region code, specified as a ccTLD ("top-level domain") two-character value. (For more information see Region Biasing below.)
        /// The bounds and region parameters will only influence, not fully restrict, results from the geocoder.
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// Bounds (optional) — The bounding box of the viewport within which to bias geocode results more prominently. (For more information see Viewport Biasing below.)
        /// The bounds and region parameters will only influence, not fully restrict, results from the geocoder.
        /// For more information see: https://developers.google.com/maps/documentation/geocoding/intro#Viewports
        /// </summary>
        public virtual ViewPort Bounds { get; set; }

        /// <summary>
        /// Components. The component filters, separated by a pipe (|).
        /// Each component filter consists of a component:value pair and will fully restrict the results from the geocoder.
        /// For more information see Component Filtering: https://developers.google.com/maps/documentation/geocoding/intro#ComponentFiltering
        /// </summary>
        public virtual IEnumerable<KeyValuePair<Component, string>> Components { get; set; }

        /// <summary>
        /// See <see cref="BaseGeocodeRequest.GetQueryStringParameters()"/>.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair}"/> collection.</returns>
        public override IList<KeyValuePair<string, string>> GetQueryStringParameters()
        {
            if (string.IsNullOrWhiteSpace(this.Address) && (this.Components == null || !this.Components.Any()))
                throw new ArgumentException("Address or Components is required");

            var parameters = base.GetQueryStringParameters();

            if (!string.IsNullOrEmpty(this.Address))
                parameters.Add("address", this.Address);

            if (!string.IsNullOrEmpty(this.Region))
                parameters.Add("region", this.Region);

            if (this.Bounds != null)
                parameters.Add("bounds", $"{this.Bounds.SouthWest}|{this.Bounds.NorthEast}");

            if (this.Components != null && this.Components.Any())
                parameters.Add("components", string.Join("|", this.Components.Select(x => $"{x.Key.ToString().ToLower()}:{x.Value}")));

            return parameters;
        }
    }
}