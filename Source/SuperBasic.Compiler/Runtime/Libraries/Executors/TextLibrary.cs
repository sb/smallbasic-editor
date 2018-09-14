// <copyright file="TextLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Globalization;
    using System.Linq;

    internal static partial class Libraries
    {
        private static string Execute_Text_Append(string text1, string text2) => text1 + text2;

        private static string Execute_Text_ConvertToLowerCase(string text) => text.ToLower(CultureInfo.CurrentCulture);

        private static string Execute_Text_ConvertToUpperCase(string text) => text.ToUpper(CultureInfo.CurrentCulture);

        private static bool Execute_Text_EndsWith(string text, string subText) => text.EndsWith(subText, StringComparison.CurrentCulture);

        private static string Execute_Text_GetCharacter(decimal characterCode) => ((char)characterCode).ToString(CultureInfo.CurrentCulture);

        private static decimal Execute_Text_GetCharacterCode(string character) => character.Any() ? (decimal)character[0] : 0;

        private static decimal Execute_Text_GetIndexOf(string text, string subText) => text.IndexOf(subText, StringComparison.CurrentCulture) + 1;

        private static decimal Execute_Text_GetLength(string text) => text.Length;

        private static string Execute_Text_GetSubText(string text, decimal start, decimal length) => text.Substring((int)Math.Max(start - 1, 0), (int)Math.Min(length - start - 1, text.Length));

        private static string Execute_Text_GetSubTextToEnd(string text, decimal start) => text.Substring((int)Math.Max(start - 1, 0));

        private static bool Execute_Text_IsSubText(string text, string subText) => text.Contains(subText);

        private static bool Execute_Text_StartsWith(string text, string subText) => text.StartsWith(subText, StringComparison.CurrentCulture);
    }
}
