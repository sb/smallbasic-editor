// <copyright file="TextDisplayStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Store
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Utilities;

    internal delegate void TextInputEventSignature(string text);

    internal static class TextDisplayStore
    {
        private static TextDisplay display;

        public static event TextInputEventSignature TextInput;

        public static void SetDisplay(TextDisplay instance)
        {
            display = instance;
        }

        public static void NotifyTextInput(string text)
        {
            if (!TextInput.IsDefault())
            {
                TextInput(text);
            }
        }

        public static Task AppendOutput(OutputChunk chunk)
        {
            if (!display.IsDefault())
            {
                return display.AppendOutput(chunk);
            }

            return Task.CompletedTask;
        }

        public static void SetInputMode(AcceptedInputMode mode)
        {
            if (!display.IsDefault())
            {
                display.SetInputMode(mode);
            }
        }

        public static void Clear()
        {
            if (!display.IsDefault())
            {
                display.Clear();
            }
        }

        public static void SetBackgroundColor(string hexColor)
        {
            if (!display.IsDefault())
            {
                display.SetBackgroundColor(hexColor);
            }
        }
    }
}
