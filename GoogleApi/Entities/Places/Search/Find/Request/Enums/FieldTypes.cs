﻿using System;

namespace GoogleApi.Entities.Places.Search.Find.Request.Enums
{
    /// <summary>
    /// Field Types.
    /// </summary>
    [Flags]
    public enum FieldTypes
    {
        /// <summary>
        /// Formatted Address (billing: basic).
        /// </summary>
        Formatted_Address = 1 << 1,

        /// <summary>
        /// Geometry (billing: basic).
        /// </summary>
        Geometry = 1 << 2,

        /// <summary>
        /// Icon (billing: basic).
        /// </summary>
        Icon = 1 << 3,

        /// <summary>
        /// Name (billing: basic).
        /// </summary>
        Name = 1 << 4,

        /// <summary>
        /// Photos (billing: basic).
        /// </summary>
        Photos = 1 << 5,

        /// <summary>
        /// Place_Id (billing: basic).
        /// </summary>
        Place_Id = 1 << 6,

        /// <summary>
        /// Plus_Code (billing: basic).
        /// </summary>
        Plus_Code = 1 << 7,

        /// <summary>
        /// Types (billing: basic).
        /// </summary>
        Types = 1 << 8,

        /// <summary>
        /// Opening Hours (billing: contact).
        /// </summary>
        Opening_Hours = 1 << 9,

        /// <summary>
        /// Price Level (billing: atmosphere)
        /// </summary>
        Price_Level = 1 << 10,

        /// <summary>
        /// Rating (billing: atmosphere).
        /// </summary>
        Rating = 1 << 11,

        /// <summary>
        /// User Ratings Total (billing: atmosphere).
        /// </summary>
        User_Ratings_Total = 1 << 12,

        /// <summary>
        /// Business Status.
        /// </summary>
        Business_Status = 1 << 13,

        /// <summary>
        /// Basic (all).
        /// </summary>
        Basic = Formatted_Address | Geometry | Icon | Name | Photos | Place_Id | Plus_Code | Types | Business_Status,

        /// <summary>
        /// Contact (all).
        /// </summary>
        Contact = Opening_Hours,

        /// <summary>
        /// Atmosphere (all).
        /// </summary>
        Atmosphere = Price_Level | Rating | User_Ratings_Total
    }
}