// <copyright file="TextWindowLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System.Globalization;
    using System.Threading.Tasks;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    internal sealed class TextWindowLibrary : ITextWindowLibrary
    {
        private string backgroundColorName = "Black";
        private string foregroundColorName = "White";
        private string inputBuffer = string.Empty;

        public TextWindowLibrary()
        {
            TextDisplayStore.TextInput += value =>
            {
                this.inputBuffer = value;
            };
        }

        public void Clear() => TextDisplayStore.Display.Clear();

        public Task<string> Get_BackgroundColor() => Task.FromResult(this.backgroundColorName);

        public string Get_ForegroundColor() => this.foregroundColorName;

        public string Read() => this.inputBuffer;

        public decimal ReadNumber() => decimal.Parse(this.inputBuffer, CultureInfo.CurrentCulture);

        public Task Set_BackgroundColor(string value)
        {
            if (ColorParser.TryGetNameFromNumber(value, out string name))
            {
                this.backgroundColorName = name;
            }
            else if (ColorParser.TryGetHexFromName(value, out _))
            {
                this.backgroundColorName = value;
            }
            else
            {
                return Task.CompletedTask;
            }

            if (ColorParser.TryGetHexFromName(this.backgroundColorName, out string hexColor))
            {
                return JSInterop.TextDisplay.SetBackgroundColor(hexColor);
            }
            else
            {
                throw ExceptionUtilities.UnexpectedValue(this.backgroundColorName);
            }
        }

        public void Set_ForegroundColor(string value)
        {
            if (ColorParser.TryGetNameFromNumber(value, out string name))
            {
                this.foregroundColorName = name;
            }
            else if (ColorParser.TryGetHexFromName(value, out _))
            {
                this.foregroundColorName = value;
            }
        }

        public void Write(string data)
        {
            TextDisplayStore.Display.AppendOutput(new OutputChunk(data, this.foregroundColorName, appendNewLine: false));
        }

        public void WriteLine(string data)
        {
            TextDisplayStore.Display.AppendOutput(new OutputChunk(data, this.foregroundColorName, appendNewLine: true));
        }
    }
}
