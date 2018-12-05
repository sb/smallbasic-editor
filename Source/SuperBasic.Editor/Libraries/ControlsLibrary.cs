// <copyright file="ControlsLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;

    internal sealed class ControlsLibrary : IControlsLibrary
    {
        private readonly NamedCounter counters = new NamedCounter();
        private readonly Dictionary<string, string> buttons = new Dictionary<string, string>();
        private readonly Dictionary<string, string> textBoxes = new Dictionary<string, string>();

        private string lastClickedButton = string.Empty;
        private string lastTypedTextBox = string.Empty;

        public ControlsLibrary()
        {
            GraphicsDisplayStore.ButtonClicked += buttonName =>
            {
                this.lastClickedButton = buttonName;
                this.ButtonClicked();
            };

            GraphicsDisplayStore.TextTyped += (textBoxName, text) =>
            {
                this.lastTypedTextBox = textBoxName;
                this.textBoxes[textBoxName] = text;
                this.TextTyped();
            };
        }

        public event Action ButtonClicked;

        public event Action TextTyped;

        public async Task<string> AddButton(string caption, decimal left, decimal top)
        {
            string name = this.counters.GetNext("Button");
            this.buttons.Add(name, caption);
            await JSInterop.Controls.AddButton(name, caption, left, top).ConfigureAwait(false);
            return name;
        }

        public async Task<string> AddMultiLineTextBox(decimal left, decimal top)
        {
            string name = this.counters.GetNext("TextBox");
            this.textBoxes.Add(name, string.Empty);
            await JSInterop.Controls.AddMultiLineTextBox(name, left, top).ConfigureAwait(false);
            return name;
        }

        public async Task<string> AddTextBox(decimal left, decimal top)
        {
            string name = this.counters.GetNext("TextBox");
            this.textBoxes.Add(name, string.Empty);
            await JSInterop.Controls.AddTextBox(name, left, top).ConfigureAwait(false);
            return name;
        }

        public string GetButtonCaption(string buttonName)
        {
            if (this.buttons.TryGetValue(buttonName, out string caption))
            {
                return caption;
            }

            return string.Empty;
        }

        public string GetTextBoxText(string textBoxName)
        {
            if (this.textBoxes.TryGetValue(textBoxName, out string text))
            {
                return text;
            }

            return string.Empty;
        }

        public string Get_LastClickedButton() => this.lastClickedButton;

        public string Get_LastTypedTextBox() => this.lastTypedTextBox;

        public Task HideControl(string controlName)
        {
            if (this.buttons.ContainsKey(controlName) || this.textBoxes.ContainsKey(controlName))
            {
                return JSInterop.Controls.HideControl(controlName);
            }

            return Task.CompletedTask;
        }

        public Task Move(string control, decimal x, decimal y)
        {
            if (this.buttons.ContainsKey(control) || this.textBoxes.ContainsKey(control))
            {
                return JSInterop.Controls.Move(control, x, y);
            }

            return Task.CompletedTask;
        }

        public Task Remove(string controlName)
        {
            if (this.buttons.ContainsKey(controlName))
            {
                this.buttons.Remove(controlName);
                return JSInterop.Controls.Remove(controlName);
            }
            else if (this.textBoxes.ContainsKey(controlName))
            {
                this.textBoxes.Remove(controlName);
                return JSInterop.Controls.Remove(controlName);
            }

            return Task.CompletedTask;
        }

        public Task SetButtonCaption(string buttonName, string caption)
        {
            if (this.buttons.ContainsKey(buttonName))
            {
                this.buttons[buttonName] = caption;
                return JSInterop.Controls.SetButtonCaption(buttonName, caption);
            }

            return Task.CompletedTask;
        }

        public Task SetSize(string control, decimal width, decimal height)
        {
            if (this.buttons.ContainsKey(control) || this.textBoxes.ContainsKey(control))
            {
                return JSInterop.Controls.SetSize(control, width, height);
            }

            return Task.CompletedTask;
        }

        public Task SetTextBoxText(string textBoxName, string text)
        {
            if (this.textBoxes.ContainsKey(textBoxName))
            {
                this.textBoxes[textBoxName] = text;
                return JSInterop.Controls.SetTextBoxText(textBoxName, text);
            }

            return Task.CompletedTask;
        }

        public Task ShowControl(string controlName)
        {
            if (this.buttons.ContainsKey(controlName) || this.textBoxes.ContainsKey(controlName))
            {
                return JSInterop.Controls.ShowControl(controlName);
            }

            return Task.CompletedTask;
        }
    }
}
