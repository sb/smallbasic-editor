// <copyright file="ControlsInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using SuperBasic.Editor.Store;

    public class ControlsInterop : IControlsInterop
    {
        public Task ButtonClicked(string controlName)
        {
            GraphicsDisplayStore.NotifyButtonClicked(controlName);
            return Task.CompletedTask;
        }

        public Task TextBoxTyped(string controlName, string value)
        {
            GraphicsDisplayStore.NotifyTextTyped(controlName, value);
            return Task.CompletedTask;
        }
    }
}
