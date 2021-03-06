﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using System.Threading.Tasks;
using NewsSystem.Services.Images.Resolvers;

// TODO: Do we add cleanup to this? Scalable caches probably shouldn't do so.
namespace NewsSystem.Services.Images.Caching
{
    /// <summary>
    /// Specifies the contract for caching images.
    /// </summary>
    public interface IImageCache
    {
        /// <summary>
        /// Gets the image resolver associated with the specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>The <see cref="IImageResolver"/>.</returns>
        Task<IImageResolver> GetAsync(string key);

        /// <summary>
        /// Sets the value associated with the specified key.
        /// Returns the date and time, in coordinated universal time (UTC), that the value was last written to.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="stream">The stream containing the image to store.</param>
        /// <param name="metadata">The <see cref="ImageMetaData"/> associated with the image to store.</param>
        /// <returns>The task.</returns>
        Task SetAsync(string key, Stream stream, ImageMetaData metadata);
    }
}