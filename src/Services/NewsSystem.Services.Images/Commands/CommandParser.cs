﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using NewsSystem.Services.Images.Commands.Converters;

namespace NewsSystem.Services.Images.Commands
{
    /// <summary>
    /// Parses URI derived command values into usable commands for processors.
    /// </summary>
    public sealed class CommandParser
    {
        /// <summary>
        /// A new instance of the <see cref="CommandParser"/> class with lazy initialization.
        /// </summary>
        private static readonly Lazy<CommandParser> Lazy = new Lazy<CommandParser>(() => new CommandParser());

        /// <summary>
        /// Prevents a default instance of the <see cref="CommandParser"/> class from being created.
        /// </summary>
        private CommandParser()
        {
            this.AddSimpleConverters();
            this.AddListConverters();
            this.AddArrayConverters();
            this.AddColorConverters();
        }

        /// <summary>
        /// Gets the current <see cref="CommandParser"/> instance.
        /// </summary>
        public static CommandParser Instance => Lazy.Value;

        /// <summary>
        /// Adds a command converter to the parser.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to add a converter for. </param>
        /// <param name="converterType">The type of <see cref="CommandConverter"/> to add.</param>
        public void AddConverter(Type type, Type converterType) => CommandDescriptor.AddConverter(type, converterType);

        /// <summary>
        /// Parses the given string value converting it to the given using the invariant culture.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <typeparam name="T">
        /// The <see cref="Type"/> to convert the string to.
        /// </typeparam>
        /// <returns>The converted instance or the default.</returns>
        public T ParseValue<T>(string value) => this.ParseValue<T>(value, CultureInfo.InvariantCulture);

        /// <summary>
        /// Parses the given string value converting it to the given type.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <typeparam name="T">
        /// The <see cref="Type"/> to convert the string to.
        /// </typeparam>
        /// <returns>The converted instance or the default.</returns>
        public T ParseValue<T>(string value, CultureInfo culture)
        {
            if (culture == null)
            {
                culture = CultureInfo.InvariantCulture;
            }

            Type type = typeof(T);
            ICommandConverter converter = CommandDescriptor.GetConverter(type);
            try
            {
                return (T)converter.ConvertFrom(culture, WebUtility.UrlDecode(value), type);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Add the generic converters.
        /// </summary>
        private void AddSimpleConverters()
        {
            this.AddConverter(TypeConstants.Sbyte, typeof(IntegralNumberConverter<sbyte>));
            this.AddConverter(TypeConstants.Byte, typeof(IntegralNumberConverter<byte>));

            this.AddConverter(TypeConstants.Short, typeof(IntegralNumberConverter<short>));
            this.AddConverter(TypeConstants.UShort, typeof(IntegralNumberConverter<ushort>));

            this.AddConverter(TypeConstants.Int, typeof(IntegralNumberConverter<int>));
            this.AddConverter(TypeConstants.UInt, typeof(IntegralNumberConverter<uint>));

            this.AddConverter(TypeConstants.Long, typeof(IntegralNumberConverter<long>));
            this.AddConverter(TypeConstants.ULong, typeof(IntegralNumberConverter<ulong>));

            this.AddConverter(typeof(decimal), typeof(SimpleCommandConverter<decimal>));
            this.AddConverter(typeof(float), typeof(SimpleCommandConverter<float>));

            this.AddConverter(typeof(double), typeof(SimpleCommandConverter<double>));
            this.AddConverter(typeof(string), typeof(SimpleCommandConverter<string>));

            this.AddConverter(typeof(bool), typeof(SimpleCommandConverter<bool>));
        }

        /// <summary>
        /// Adds a selection of default list type converters.
        /// </summary>
        private void AddListConverters()
        {
            this.AddConverter(typeof(List<sbyte>), typeof(ListConverter<sbyte>));
            this.AddConverter(typeof(List<byte>), typeof(ListConverter<byte>));

            this.AddConverter(typeof(List<short>), typeof(ListConverter<short>));
            this.AddConverter(typeof(List<ushort>), typeof(ListConverter<ushort>));

            this.AddConverter(typeof(List<int>), typeof(ListConverter<int>));
            this.AddConverter(typeof(List<uint>), typeof(ListConverter<uint>));

            this.AddConverter(typeof(List<long>), typeof(ListConverter<long>));
            this.AddConverter(typeof(List<ulong>), typeof(ListConverter<ulong>));

            this.AddConverter(typeof(List<decimal>), typeof(ListConverter<decimal>));
            this.AddConverter(typeof(List<float>), typeof(ListConverter<float>));
            this.AddConverter(typeof(List<double>), typeof(ListConverter<double>));

            this.AddConverter(typeof(List<string>), typeof(ListConverter<string>));
        }

        /// <summary>
        /// Adds a selection of default array type converters.
        /// </summary>
        private void AddArrayConverters()
        {
            this.AddConverter(typeof(sbyte[]), typeof(ArrayConverter<sbyte>));
            this.AddConverter(typeof(byte[]), typeof(ArrayConverter<byte>));

            this.AddConverter(typeof(short[]), typeof(ArrayConverter<short>));
            this.AddConverter(typeof(ushort[]), typeof(ArrayConverter<ushort>));

            this.AddConverter(typeof(int[]), typeof(ArrayConverter<int>));
            this.AddConverter(typeof(uint[]), typeof(ArrayConverter<uint>));

            this.AddConverter(typeof(long[]), typeof(ArrayConverter<long>));
            this.AddConverter(typeof(ulong[]), typeof(ArrayConverter<ulong>));

            this.AddConverter(typeof(decimal[]), typeof(ArrayConverter<decimal>));
            this.AddConverter(typeof(float[]), typeof(ArrayConverter<float>));
            this.AddConverter(typeof(double[]), typeof(ArrayConverter<double>));

            this.AddConverter(typeof(string[]), typeof(ArrayConverter<string>));
        }

        /// <summary>
        /// Adds the default color converters.
        /// </summary>
        private void AddColorConverters() => this.AddConverter(TypeConstants.Rgba32, typeof(Rgba32Converter));
    }
}