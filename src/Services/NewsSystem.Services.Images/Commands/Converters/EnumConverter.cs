﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Globalization;
using System.Linq;

namespace NewsSystem.Services.Images.Commands.Converters
{
    /// <summary>
    /// The enum converter. Allows conversion to enumerations.
    /// </summary>
    internal sealed class EnumConverter : CommandConverter
    {
        /// <inheritdoc/>
        public override object ConvertFrom(CultureInfo culture, string value, Type propertyType)
        {
            if (value == null)
            {
                // TODO: How can we get the default enum, or should we return null?
                return base.ConvertFrom(culture, null, propertyType);
            }

            try
            {
                char separator = culture.TextInfo.ListSeparator[0];
                if (value.IndexOf(separator) != -1)
                {
                    long convertedValue = 0;
                    foreach (string pill in GetStringArray(value, separator))
                    {
                        convertedValue |= Convert.ToInt64((Enum)Enum.Parse(propertyType, pill, true), culture);
                    }

                    return Enum.ToObject(propertyType, convertedValue);
                }

                return Enum.Parse(propertyType, value, true);
            }
            catch (Exception e)
            {
                throw new FormatException($"{value} is not a valid value for {propertyType.Name}", e);
            }
        }

        /// <summary>
        /// Splits a string by separator to return an array of string values.
        /// </summary>
        /// <param name="input">The input string to split.</param>
        /// <param name="separator">The separator to split string by.</param>
        /// <returns>The <see cref="T:String[]"/>.</returns>
        private static string[] GetStringArray(string input, char separator) => input.Split(separator).Select(s => s.Trim()).ToArray();
    }
}