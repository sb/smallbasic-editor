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
            if (ColorParser.HexFromNumber(value, out string result))
            {
                this.settings.BackgroundColor = result;
            }
        }

        public string Get_ForegroundColor() => this.settings.PenColor;

        public void Set_ForegroundColor(string value)
        {
            if (ColorParser.HexFromNumber(value, out string result))
            {
                this.settings.PenColor = result;
            }
        }

        public void Clear() => this.plugin.Clear();

        public string Read() => this.plugin.Read();

        public decimal ReadNumber() => this.plugin.ReadNumber();

        public void Write(string data) => this.plugin.Write(data);

        public void WriteLine(string data) => this.plugin.WriteLine(data);
    }
}
