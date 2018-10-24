// <copyright file="TextLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Compiler.Runtime;

    public sealed class TextLibrary : ITextLibrary
    {
        public string Append(string text1, string text2) => text1 + text2;

        public string ConvertToLowerCase(string text) => text.ToLower(CultureInfo.CurrentCulture);

        public string ConvertToUpperCase(string text) => text.ToUpper(CultureInfo.CurrentCulture);

        public bool EndsWith(string text, string subText) => text.EndsWith(subText, StringComparison.CurrentCulture);

        public string GetCharacter(decimal characterCode) => ((char)characterCode).ToString(CultureInfo.CurrentCulture);

        public decimal GetCharacterCode(string character) => character.Any() ? (decimal)character[0] : 0;

        public decimal GetIndexOf(string text, string subText) => text.IndexOf(subText, StringComparison.CurrentCulture) + 1;

        public decimal GetLength(string text) => text.Length;

        public string GetSubText(string text, decimal start, decimal length) => text.Substring((int)Math.Max(start - 1, 0), (int)Math.Min(length - start - 1, text.Length));

        public string GetSubTextToEnd(string text, decimal start) => text.Substring((int)Math.Max(start - 1, 0));

        public bool IsSubText(string text, string subText) => text.Contains(subText);

        public bool StartsWith(string text, string subText) => text.StartsWith(subText, StringComparison.CurrentCulture);
    }
}
