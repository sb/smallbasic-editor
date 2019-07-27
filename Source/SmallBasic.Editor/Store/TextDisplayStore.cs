// <copyright file="TextDisplayStore.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Store
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using SmallBasic.Editor.Components.Display;
    using SmallBasic.Utilities;

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

        public static Task SetInputMode(AcceptedInputMode mode)
        {
            if (!display.IsDefault())
            {
                return display.SetInputMode(mode);
            }

            return Task.CompletedTask;
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
