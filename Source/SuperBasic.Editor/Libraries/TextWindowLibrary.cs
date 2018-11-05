// <copyright file="TextWindowLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    public interface ITextWindowPlugin
    {
        void Clear();

        string Read();

        decimal ReadNumber();

        void Write(string data);

        void WriteLine(string data);
    }

    internal sealed class TextWindowLibrary : ITextWindowLibrary
    {
        private readonly ITextWindowPlugin plugin;
        private readonly StylesSettings settings;

        public TextWindowLibrary(ITextWindowPlugin plugin, StylesSettings settings)
        {
            this.plugin = plugin;
            this.settings = settings;
        }

        public string Get_BackgroundColor() => this.settings.BackgroundColor;

        public void Set_BackgroundColor(string value)
        {
            if (TryGetColorFromNumber(value, out string result))
            {
                this.settings.BackgroundColor = result;
            }
        }

        public string Get_ForegroundColor() => this.settings.PenColor;

        public void Set_ForegroundColor(string value)
        {
            if (TryGetColorFromNumber(value, out string result))
            {
                this.settings.PenColor = result;
            }
        }

        public void Clear() => this.plugin.Clear();

        public string Read() => this.plugin.Read();

        public decimal ReadNumber() => this.plugin.ReadNumber();

        public void Write(string data) => this.plugin.Write(data);

        public void WriteLine(string data) => this.plugin.WriteLine(data);

        private static bool TryGetColorFromNumber(string number, out string result)
        {
            if (decimal.TryParse(number, out decimal value))
            {
                switch (value)
                {
                    case 0: return ColorParser.TryParseColorName("Black", out result);
                    case 1: return ColorParser.TryParseColorName("DarkBlue", out result);
                    case 2: return ColorParser.TryParseColorName("DarkGreen", out result);
                    case 3: return ColorParser.TryParseColorName("DarkCyan", out result);
                    case 4: return ColorParser.TryParseColorName("DarkRed", out result);
                    case 5: return ColorParser.TryParseColorName("DarkMagenta", out result);
                    case 6: return ColorParser.TryParseColorName("DarkYellow", out result);
                    case 7: return ColorParser.TryParseColorName("Gray", out result);
                    case 8: return ColorParser.TryParseColorName("DarkGray", out result);
                    case 9: return ColorParser.TryParseColorName("Blue", out result);
                    case 10: return ColorParser.TryParseColorName("Green", out result);
                    case 11: return ColorParser.TryParseColorName("Cyan", out result);
                    case 12: return ColorParser.TryParseColorName("Red", out result);
                    case 13: return ColorParser.TryParseColorName("Magenta", out result);
                    case 14: return ColorParser.TryParseColorName("Yellow", out result);
                    case 15: return ColorParser.TryParseColorName("White", out result);
                }
            }

            result = default;
            return false;
        }
    }
}
