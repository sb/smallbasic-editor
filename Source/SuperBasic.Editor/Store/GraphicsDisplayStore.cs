// <copyright file="GraphicsDisplayStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Store
{
    using System;
    using System.Diagnostics;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Editor.Libraries;
    using SuperBasic.Utilities;

    internal delegate void ButtonClickedEventSignature(string buttonName);

    internal delegate void TextTypedEventSignature(string textBoxName, string text);

    internal static class GraphicsDisplayStore
    {
        private static GraphicsDisplay display;

        public static event ButtonClickedEventSignature ButtonClicked;

        public static event TextTypedEventSignature TextTyped;

        public static GraphicsDisplay Display
        {
            get
            {
                Debug.Assert(!display.IsDefault(), "Display instance not set.");
                return display;
            }
        }

        public static void SetDisplay(GraphicsDisplay instance)
        {
            display = instance;
        }

        public static void NotifyButtonClicked(string buttonName)
        {
            if (!ButtonClicked.IsDefault())
            {
                ButtonClicked(buttonName);
            }
        }

        public static void NotifyTextTyped(string textBoxName, string text)
        {
            if (!TextTyped.IsDefault())
            {
                TextTyped(textBoxName, text);
            }
        }
    }
}
