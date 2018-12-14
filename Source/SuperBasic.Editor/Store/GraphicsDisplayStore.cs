// <copyright file="GraphicsDisplayStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Store
{
    using System;
    using Microsoft.AspNetCore.Blazor;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Editor.Libraries;
    using SuperBasic.Utilities;

    internal delegate void KeyEventSignature(string key);

    internal delegate void MouseEventSignature(decimal x, decimal y);

    internal static class GraphicsDisplayStore
    {
        private static GraphicsDisplay display;

        public static event KeyEventSignature KeyDown;

        public static event KeyEventSignature KeyUp;

        public static event MouseEventSignature MouseDown;

        public static event MouseEventSignature MouseMove;

        public static event MouseEventSignature MouseUp;

        public static ElementRef RenderArea
        {
            get
            {
                if (display.IsDefault() || display.RenderArea.IsDefault())
                {
                    throw new NullReferenceException("Graphics display is not initialized yet");
                }

                return display.RenderArea;
            }
        }

        public static void SetDisplay(GraphicsDisplay instance)
        {
            display = instance;
        }

        public static void SetLibraries(LibrariesCollection libraries)
        {
            if (!display.IsDefault())
            {
                display.Libraries = libraries;
            }
        }

        public static void UpdateDisplay()
        {
            if (!display.IsDefault())
            {
                display.Update();
            }
        }

        public static void NotifyKeyDown(string key)
        {
            if (!KeyDown.IsDefault())
            {
                KeyDown(key);
            }
        }

        public static void NotifyKeyUp(string key)
        {
            if (!KeyUp.IsDefault())
            {
                KeyUp(key);
            }
        }

        public static void NotifyMouseMove(decimal x, decimal y)
        {
            if (!MouseMove.IsDefault())
            {
                MouseMove(x, y);
            }
        }

        public static void NotifyMouseUp(decimal x, decimal y)
        {
            if (!MouseUp.IsDefault())
            {
                MouseUp(x, y);
            }
        }

        public static void NotifyMouseDown(decimal x, decimal y)
        {
            if (!MouseDown.IsDefault())
            {
                MouseDown(x, y);
            }
        }
    }
}
