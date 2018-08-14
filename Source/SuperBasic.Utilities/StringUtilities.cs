// <copyright file="StringUtilities.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    using System.Collections.Generic;
    using System.Globalization;

    public static class StringUtilities
    {
        public static string Join(this IEnumerable<string> enumerable, string separator = "") => string.Join(separator, enumerable);

        public static bool IsSupportedCharacter(in this char ch) => ch >= 32 && ch <= 126;

        public static string ToDisplayString(this string value) => value;

        public static string ToDisplayString(in this char value) => value.ToString(CultureInfo.CurrentCulture);

        public static string LowerFirstChar(this string value) => char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}
