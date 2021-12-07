﻿using System;

namespace GoogleApi.Entities.Maps.Common.Enums
{
    /// <summary>
    /// Transit Routing Preference.
    /// </summary>
    [Flags]
    public enum TransitRoutingPreference
    {
        /// <summary>
        /// No TransitRoutingPreference.
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Indicates that the calculated route should prefer limited amounts of walking.
        /// </summary>
        LessWalking = 1 << 1,

        /// <summary>
        /// Indicates that the calculated route should prefer a limited number of transfers.
        /// </summary>
        FewerTransfers = 1 << 2
    }
}