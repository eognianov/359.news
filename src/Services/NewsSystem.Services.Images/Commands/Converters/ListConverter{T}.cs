﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NewsSystem.Services.Images.Commands.Converters
{
    /// <summary>
    /// Converts the value of an string to a generic list.
    /// </summary>
    /// <typeparam name="T">The type to convert from.</typeparam>
    internal class ListConverter<T> : CommandConverter
    {
        /// <inheritdoc/>
        public override object ConvertFrom(CultureInfo culture, string value, Type propertyType)
        {
            Type type = typeof(T);
            ICommandConverter paramConverter = CommandDescriptor.GetConverter(type);

            if (paramConverter == null)
            {
                throw new InvalidOperationException("No type converter exists for type " + type.FullName);
            }

            var result = new List<T>();

            if (value != null)
            {
                foreach (string pill in this.GetStringArray(value, culture))
                {
                    object item = paramConverter.ConvertFromInvariantString(pill, type);
                    if (item != null)
                    {
                        result.Add((T)item);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Splits a string by separator to return an array of string values.
        /// </summary>
        /// <param name="input">The input string to split.</param>
        /// <param name="culture">A <see cref="CultureInfo"/>. The current culture to split string by.</param>
        /// <returns>The <see cref="T:String[]"/>.</returns>
        protected string[] GetStringArray(string input, CultureInfo culture)
        {
            char separator = culture.TextInfo.ListSeparator[0];
            return input.Split(separator).Select(s => s.Trim()).ToArray();
        }
    }
}