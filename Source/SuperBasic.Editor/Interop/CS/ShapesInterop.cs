// <copyright file="ShapesInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using SuperBasic.Editor.Libraries;

    internal class ShapesInterop : IShapesInterop
    {
        private static ControlsLibrary controlsLibrary;

        public static void Initialize(string containerId, ControlsLibrary controlsLibrary)
        {
            JSInterop.Shapes.Initialize(containerId).Wait();
            ShapesInterop.controlsLibrary = controlsLibrary;
        }

        public Task<bool> NotifyButtonClicked(string buttonName)
        {
            ShapesInterop.controlsLibrary.NotifyButtonClicked(buttonName);
            return Task.FromResult(true);
        }

        public Task<bool> NotifyTextTyped(string textBoxName)
        {
            ShapesInterop.controlsLibrary.NotifyTextTyped(textBoxName);
            return Task.FromResult(true);
        }
    }
}
