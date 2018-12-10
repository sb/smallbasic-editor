// <copyright file="ControlsLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Libraries.Controls;
    using SuperBasic.Editor.Libraries.Utilities;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;

    internal sealed class ControlsLibrary : IControlsLibrary
    {
        private readonly NamedCounter counters = new NamedCounter();
        private readonly Dictionary<string, BaseControl> controls = new Dictionary<string, BaseControl>();

        private string lastClickedButton = string.Empty;
        private string lastTypedTextBox = string.Empty;

        public ControlsLibrary()
        {
            GraphicsDisplayStore.SetControlsComposer(this.ComposeTree);
        }

        public event Action ButtonClicked;

        public event Action TextTyped;

        public string AddButton(string caption, decimal left, decimal top)
        {
            string name = this.counters.GetNext("Button");
            this.controls.Add(name, new ButtonControl(name, caption, left, top, width: 80, height: 30));
            return name;
        }

        public string AddMultiLineTextBox(decimal left, decimal top)
        {
            string name = this.counters.GetNext("TextBox");
            this.controls.Add(name, new MultilineTextBoxControl(name, left, top, width: 200, height: 50));
            return name;
        }

        public string AddTextBox(decimal left, decimal top)
        {
            string name = this.counters.GetNext("TextBox");
            this.controls.Add(name, new TextBoxControl(name, left, top, width: 200, height: 20));
            return name;
        }

        public string GetButtonCaption(string buttonName)
        {
            if (this.controls.TryGetValue(buttonName, out BaseControl control) && control is ButtonControl button)
            {
                return button.Caption;
            }

            return string.Empty;
        }

        public string GetTextBoxText(string textBoxName)
        {
            if (this.controls.TryGetValue(textBoxName, out BaseControl control))
            {
                if (control is TextBoxControl textBox)
                {
                    return textBox.Text;
                }
                else if (control is MultilineTextBoxControl multilineTextBox)
                {
                    return multilineTextBox.Text;
                }
            }

            return string.Empty;
        }

        public string Get_LastClickedButton() => this.lastClickedButton;

        public string Get_LastTypedTextBox() => this.lastTypedTextBox;

        public void HideControl(string controlName)
        {
            if (this.controls.TryGetValue(controlName, out BaseControl control))
            {
                control.Visible = false;
            }
        }

        public void Move(string control, decimal x, decimal y)
        {
            if (this.controls.TryGetValue(control, out BaseControl controlObj))
            {
                controlObj.Left = x;
                controlObj.Top = y;
            }
        }

        public void Remove(string controlName)
        {
            if (this.controls.ContainsKey(controlName))
            {
                this.controls.Remove(controlName);
            }
        }

        public void SetButtonCaption(string buttonName, string caption)
        {
            if (this.controls.TryGetValue(buttonName, out BaseControl control) && control is ButtonControl button)
            {
                button.Caption = caption;
            }
        }

        public void SetSize(string control, decimal width, decimal height)
        {
            if (this.controls.TryGetValue(control, out BaseControl controlObj))
            {
                controlObj.Width = width;
                controlObj.Height = height;
            }
        }

        public void SetTextBoxText(string textBoxName, string text)
        {
            if (this.controls.TryGetValue(textBoxName, out BaseControl control))
            {
                if (control is TextBoxControl textBox)
                {
                    textBox.Text = text;
                }
                else if (control is MultilineTextBoxControl multilineTextBox)
                {
                    multilineTextBox.Text = text;
                }
                else
                {
                    return;
                }
            }
        }

        public void ShowControl(string controlName)
        {
            if (this.controls.TryGetValue(controlName, out BaseControl control))
            {
                control.Visible = true;
            }
        }

        internal void NotifyButtonClicked(string buttonName)
        {
            this.lastClickedButton = buttonName;
            this.ButtonClicked();
        }

        internal void NotifyTextTyped(string textBoxName)
        {
            this.lastTypedTextBox = textBoxName;
            this.TextTyped();
        }

        internal void Clear()
        {
            this.controls.Clear();
        }

        private void ComposeTree(TreeComposer composer)
        {
            foreach (var control in this.controls.Values)
            {
                control.ComposeTree(this, composer);
            }
        }
    }
}
