// <copyright file="ControlsLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Interop;

    public sealed class ControlsLibrary : IControlsLibrary
    {
        public ControlsLibrary()
        {
            this.LastClickedButton = string.Empty;
            this.LastTypedTextBox = string.Empty;
        }

        public event Action ButtonClicked;

        public event Action TextTyped;

        public string LastClickedButton { get; private set; }

        public string LastTypedTextBox { get; private set; }

        public string AddButton(string caption, decimal left, decimal top)
        {
            return JSInterop.Shapes.AddButton(caption, left, top).Result;
        }

        public string AddMultiLineTextBox(decimal left, decimal top)
        {
            return JSInterop.Shapes.AddTextBox(isMultiLine: true, left, top).Result;
        }

        public string AddTextBox(decimal left, decimal top)
        {
            return JSInterop.Shapes.AddTextBox(isMultiLine: false, left, top).Result;
        }

        public string GetButtonCaption(string buttonName)
        {
            return JSInterop.Shapes.GetText(buttonName).Result;
        }

        public string GetTextBoxText(string textBoxName)
        {
            return JSInterop.Shapes.GetText(textBoxName).Result;
        }

        public void HideControl(string controlName)
        {
            JSInterop.Shapes.HideControl(controlName).Wait();
        }

        public void Move(string control, decimal x, decimal y)
        {
            JSInterop.Shapes.MoveControl(control, x, y).Wait();
        }

        public void Remove(string controlName)
        {
            JSInterop.Shapes.RemoveControl(controlName).Wait();
        }

        public void SetButtonCaption(string buttonName, string caption)
        {
            JSInterop.Shapes.SetControlText(buttonName, caption).Wait();
        }

        public void SetSize(string control, decimal width, decimal height)
        {
            JSInterop.Shapes.SetControlSize(control, width, height).Wait();
        }

        public void SetTextBoxText(string textBoxName, string text)
        {
            JSInterop.Shapes.SetControlText(text, text).Wait();
        }

        public void ShowControl(string controlName)
        {
            JSInterop.Shapes.ShowControl(controlName).Wait();
        }

        internal void NotifyButtonClicked(string buttonName)
        {
            this.LastClickedButton = buttonName;
            this.ButtonClicked();
        }

        internal void NotifyTextTyped(string textBoxName)
        {
            this.LastTypedTextBox = textBoxName;
            this.TextTyped();
        }
    }
}
