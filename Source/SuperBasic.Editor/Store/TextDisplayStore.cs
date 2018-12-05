// <copyright file="TextDisplayStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Store
{
    using System.Diagnostics;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Utilities;

    internal delegate void TextInputEventSignature(string text);

    internal static class TextDisplayStore
    {
        private static TextDisplay display;

        public static event TextInputEventSignature TextInput;

        public static TextDisplay Display
        {
            get
            {
                Debug.Assert(!display.IsDefault(), "Display instance not set.");
                return display;
            }
        }

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
    }
}
