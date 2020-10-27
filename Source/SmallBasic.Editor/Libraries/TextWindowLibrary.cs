// <copyright file="TextWindowLibrary.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Components.Display;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;

    internal sealed class TextWindowLibrary : ITextWindowLibrary, IDisposable
    {
        private string backgroundColorName = "Black";
        private string foregroundColorName = "White";
        private string inputBuffer = string.Empty;

        public TextWindowLibrary()
        {
            TextDisplayStore.TextInput += this.OnTextInput;
        }

        public void Dispose()
        {
            TextDisplayStore.TextInput -= this.OnTextInput;
        }

        public void Clear() => TextDisplayStore.Clear();

        public string Get_BackgroundColor() => this.backgroundColorName;

        public string Get_ForegroundColor() => this.foregroundColorName;

        public string Get_Title() => TextDisplayStore.Title;

        public string Read() => this.inputBuffer;

        public decimal ReadNumber() => decimal.Parse(this.inputBuffer, CultureInfo.CurrentCulture);

        public void Set_BackgroundColor(string value)
        {
            if (decimal.TryParse(value, out decimal number) && TryGetColorName(number, out string name) && PredefinedColors.TryGetHexColor(name, out string hexColor))
            {
                this.backgroundColorName = name;
                TextDisplayStore.SetBackgroundColor(hexColor);
            }
            else if (PredefinedColors.TryGetHexColor(value, out string hex))
            {
                this.backgroundColorName = value;
                TextDisplayStore.SetBackgroundColor(hex);
            }
        }

        public void Set_ForegroundColor(string value)
        {
            if (decimal.TryParse(value, out decimal number) && TryGetColorName(number, out string name))
            {
                this.foregroundColorName = name;
            }
            else if (PredefinedColors.ContainsName(value))
            {
                this.foregroundColorName = value;
            }
        }

        public void Set_Title(string value) => TextDisplayStore.Title = value;

        public Task Write(string data)
        {
            return TextDisplayStore.AppendOutput(new OutputChunk(data, this.foregroundColorName, appendNewLine: false));
        }

        public Task WriteLine(string data)
        {
            return TextDisplayStore.AppendOutput(new OutputChunk(data, this.foregroundColorName, appendNewLine: true));
        }

        private void OnTextInput(string text)
        {
            this.inputBuffer = text;
        }

        private static bool TryGetColorName(decimal number, out string result)
        {
            switch (number)
            {
                case 0: result = "Black"; return true;
                case 1: result = "DarkBlue"; return true;
                case 2: result = "DarkGreen"; return true;
                case 3: result = "DarkCyan"; return true;
                case 4: result = "DarkRed"; return true;
                case 5: result = "DarkMagenta"; return true;
                case 6: result = "DarkYellow"; return true;
                case 7: result = "Gray"; return true;
                case 8: result = "DarkGray"; return true;
                case 9: result = "Blue"; return true;
                case 10: result = "Green"; return true;
                case 11: result = "Cyan"; return true;
                case 12: result = "Red"; return true;
                case 13: result = "Magenta"; return true;
                case 14: result = "Yellow"; return true;
                case 15: result = "White"; return true;
                default: result = default; return false;
            }
        }
    }
}
