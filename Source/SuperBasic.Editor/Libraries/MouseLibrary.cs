// <copyright file="MouseLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Store;

    internal sealed class MouseLibrary : IMouseLibrary, IDisposable
    {
        private readonly LibrariesCollection libraries;

        private bool isLeftButtonDown;
        private bool isRightButtonDown;

        public MouseLibrary(LibrariesCollection libraries)
        {
            this.libraries = libraries;
            this.isLeftButtonDown = false;
            this.isRightButtonDown = false;
            GraphicsDisplayStore.MouseUp += this.MouseUpCallback;
            GraphicsDisplayStore.MouseDown += this.MouseDownCallback;
        }

        public void Dispose()
        {
            GraphicsDisplayStore.MouseUp -= this.MouseUpCallback;
            GraphicsDisplayStore.MouseDown -= this.MouseDownCallback;
        }

        public bool Get_IsLeftButtonDown() => this.isLeftButtonDown;

        public bool Get_IsRightButtonDown() => this.isRightButtonDown;

        public decimal Get_MouseX() => this.libraries.GraphicsWindow.Get_MouseX();

        public decimal Get_MouseY() => this.libraries.GraphicsWindow.Get_MouseY();

        public void HideCursor() => GraphicsDisplayStore.SetMouseVisibility(false);

        public void ShowCursor() => GraphicsDisplayStore.SetMouseVisibility(true);

        private void MouseDownCallback(decimal x, decimal y, MouseButton button)
        {
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
    }
}
