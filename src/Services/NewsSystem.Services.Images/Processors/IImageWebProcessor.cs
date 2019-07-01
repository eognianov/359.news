// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace NewsSystem.Services.Images.Processors
{
    /// <summary>
    /// Specifies the contract for processing images using a querystring URI API.
    /// </summary>
    public interface IImageWebProcessor
    {
        /// <summary>
        /// Gets the collection of recognized querystring commands.
        /// </summary>
        IEnumerable<string> Commands { get; }

        /// <summary>
        /// Processes the image based on the querystring commands.
        /// </summary>
        /// <param name="image">The image to process.</param>
        /// <param name="logger">The type used for performing logging.</param>
        /// <param name="commands">The querystring collection containing the processing commands.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        FormattedImage Process(FormattedImage image, ILogger logger, IDictionary<string, string> commands);
    }
}
