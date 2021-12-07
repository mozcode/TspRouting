﻿using System.IO;

namespace GoogleApi.Entities
{
    /// <summary>
    /// Base Response Stream.
    /// </summary>
    public class BaseResponseStream : BaseResponse
    {
        /// <summary>
        /// Buffer.
        /// </summary>
        public virtual byte[] Buffer { get; set; }

        /// <summary>
        /// Stream.
        /// </summary>
        public virtual MemoryStream Stream => new MemoryStream(this.Buffer ?? new byte[0]);
    }
}