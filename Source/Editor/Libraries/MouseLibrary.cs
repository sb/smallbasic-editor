// <copyright file="MouseLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;

    public interface IMousePlugin
    {
        void SetMouseMoveCallback(Action<decimal, decimal> callback);

        void SetLeftMouseDownCallback(Action callback);

        void SetLeftMouseUpCallback(Action callback);

        void SetRightMouseDownCallback(Action callback);

        void SetRightMouseUpCallback(Action callback);

        void HideCursor();

        void ShowCursor();
    }

    internal sealed class MouseLibrary : IMouseLibrary
    {
        private readonly IMousePlugin plugin;

        public MouseLibrary(IMousePlugin plugin)
        {
            this.plugin = plugin;

            this.MouseX = 0;
            this.MouseY = 0;

            this.IsLeftButtonDown = false;
            this.IsRightButtonDown = false;

            this.plugin.SetMouseMoveCallback((mouseX, mouseY) =>
            {
                this.MouseX = mouseX;
                this.MouseY = mouseY;
            });

            this.plugin.SetLeftMouseDownCallback(() => this.IsLeftButtonDown = true);
            this.plugin.SetLeftMouseUpCallback(() => this.IsLeftButtonDown = false);

            this.plugin.SetRightMouseDownCallback(() => this.IsRightButtonDown = true);
            this.plugin.SetRightMouseUpCallback(() => this.IsRightButtonDown = false);
        }

        public decimal MouseX { get; set; }

        public decimal MouseY { get; set; }

        public bool IsLeftButtonDown { get; private set; }

        public bool IsRightButtonDown { get; private set; }

        public void HideCursor() => this.plugin.HideCursor();

        public void ShowCursor() => this.plugin.ShowCursor();
    }
}
