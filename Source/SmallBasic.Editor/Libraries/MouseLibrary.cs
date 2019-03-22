// <copyright file="MouseLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Store;

    internal sealed class MouseLibrary : IMouseLibrary, IDisposable
    {
        private readonly LibrariesCollection libraries;

        private bool isLeftButtonDown;
        private bool isRightButtonDown;

        private decimal mouseX = 0;
        private decimal mouseY = 0;

        public MouseLibrary(LibrariesCollection libraries)
        {
            this.libraries = libraries;
            this.isLeftButtonDown = false;
            this.isRightButtonDown = false;
            GraphicsDisplayStore.MouseUp += this.MouseUpCallback;
            GraphicsDisplayStore.MouseDown += this.MouseDownCallback;
            GraphicsDisplayStore.MouseMove += this.MouseMoveCallback;
        }

        public void Dispose()
        {
            GraphicsDisplayStore.MouseUp -= this.MouseUpCallback;
            GraphicsDisplayStore.MouseDown -= this.MouseDownCallback;
            GraphicsDisplayStore.MouseMove -= this.MouseMoveCallback;
        }

        public bool Get_IsLeftButtonDown() => this.isLeftButtonDown;

        public bool Get_IsRightButtonDown() => this.isRightButtonDown;

        public decimal Get_MouseX() => this.mouseX;

        public decimal Get_MouseY() => this.mouseY;

        public void HideCursor() => GraphicsDisplayStore.SetMouseVisibility(false);

        public void ShowCursor() => GraphicsDisplayStore.SetMouseVisibility(true);

        private void MouseDownCallback(decimal x, decimal y, MouseButton button)
        {
            this.mouseX = x;
            this.mouseY = y;

            switch (button)
            {
                case MouseButton.Left:
                    this.isLeftButtonDown = true;
                    break;
                case MouseButton.Right:
                    this.isRightButtonDown = true;
                    break;
            }
        }

        private void MouseUpCallback(decimal x, decimal y, MouseButton button)
        {
            this.mouseX = x;
            this.mouseY = y;

            switch (button)
            {
                case MouseButton.Left:
                    this.isLeftButtonDown = false;
                    break;
                case MouseButton.Right:
                    this.isRightButtonDown = false;
                    break;
            }
        }

        private void MouseMoveCallback(decimal x, decimal y)
        {
            this.mouseX = x;
            this.mouseY = y;
        }
    }
}
