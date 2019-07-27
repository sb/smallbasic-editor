// <copyright file="TextLibrary.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SmallBasic.Compiler.Runtime;

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

        public string GetSubText(string text, decimal start, decimal length)
        {
            start--;

            if (start < 0 || start >= text.Length || length < 1)
            {
                return string.Empty;
            }

            length = Math.Min(length, text.Length - start);
            return text.Substring((int)start, (int)length);
        }

        public string GetSubTextToEnd(string text, decimal start)
        {
            start--;
            if (start < 0 || start >= text.Length)
            {
                return string.Empty;
            }

            return text.Substring((int)start);
        }

        public bool IsSubText(string text, string subText) => text.Contains(subText);

        public bool StartsWith(string text, string subText) => text.StartsWith(subText, StringComparison.CurrentCulture);
    }
}
