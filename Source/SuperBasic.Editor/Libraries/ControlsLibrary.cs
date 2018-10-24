// <copyright file="ControlsLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    public interface IControlsPlugin
    {
        void AddButton(string name, string caption, decimal left, decimal top, Action onClick);

        void AddTextBox(string name, decimal left, decimal top, bool isMutliLine, Action<string> onTextChange);

        void SetButtonCaption(string name, string caption);

        void SetTextBoxText(string name, string text);

        void SetSize(string name, decimal width, decimal height);

        void Hide(string name);

        void Show(string name);

        void Move(string name, decimal x, decimal y);

        void Remove(string name);
    }

    internal sealed class ControlsLibrary : IControlsLibrary
    {
        private readonly NamedCounter counters;
        private readonly IControlsPlugin plugin;

        private readonly Dictionary<string, string> buttons;
        private readonly Dictionary<string, string> textBoxes;

        public ControlsLibrary(IControlsPlugin plugin)
        {
            this.counters = new NamedCounter();
            this.plugin = plugin;

            this.buttons = new Dictionary<string, string>();
            this.textBoxes = new Dictionary<string, string>();

            this.LastClickedButton = string.Empty;
            this.LastTypedTextBox = string.Empty;
        }

        public event Action ButtonClicked;

        public event Action TextTyped;

        public string LastClickedButton { get; private set; }

        public string LastTypedTextBox { get; private set; }

        public string AddButton(string caption, decimal left, decimal top)
        {
            string name = this.counters.GetNext("Button");
            this.plugin.AddButton(name, caption, left, top, () =>
            {
                this.LastClickedButton = name;
                this.ButtonClicked();
            });

            this.buttons.Add(name, caption);
            return name;
        }

        public string AddMultiLineTextBox(decimal left, decimal top)
        {
            return this.AddTextBoxAux(left, top, isMultiLine: true);
        }

        public string AddTextBox(decimal left, decimal top)
        {
            return this.AddTextBoxAux(left, top, isMultiLine: false);
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

        public void HideControl(string controlName)
        {
            if (this.buttons.ContainsKey(controlName) || this.textBoxes.ContainsKey(controlName))
            {
                this.plugin.Hide(controlName);
            }
        }

        public void Move(string control, decimal x, decimal y)
        {
            if (this.buttons.ContainsKey(control) || this.textBoxes.ContainsKey(control))
            {
                this.plugin.Move(control, x, y);
            }
        }

        public void Remove(string controlName)
        {
            if (this.buttons.ContainsKey(controlName))
            {
                this.buttons.Remove(controlName);
                this.plugin.Remove(controlName);
            }
            else if (this.textBoxes.ContainsKey(controlName))
            {
                this.textBoxes.Remove(controlName);
                this.plugin.Remove(controlName);
            }
        }

        public void SetButtonCaption(string buttonName, string caption)
        {
            if (this.buttons.ContainsKey(buttonName))
            {
                this.buttons[buttonName] = caption;
                this.plugin.SetButtonCaption(buttonName, caption);
            }
        }

        public void SetSize(string control, decimal width, decimal height)
        {
            if (this.buttons.ContainsKey(control) || this.textBoxes.ContainsKey(control))
            {
                this.plugin.SetSize(control, width, height);
            }
        }

        public void SetTextBoxText(string textBoxName, string text)
        {
            if (this.textBoxes.ContainsKey(textBoxName))
            {
                this.textBoxes[textBoxName] = text;
                this.plugin.SetTextBoxText(textBoxName, text);
            }
        }

        public void ShowControl(string controlName)
        {
            if (this.buttons.ContainsKey(controlName) || this.textBoxes.ContainsKey(controlName))
            {
                this.plugin.Show(controlName);
            }
        }

        private string AddTextBoxAux(decimal left, decimal top, bool isMultiLine)
        {
            string name = this.counters.GetNext("TextBox");
            this.plugin.AddTextBox(name, left, top, isMultiLine, text =>
            {
                this.textBoxes[name] = text;

                this.LastTypedTextBox = name;
                this.TextTyped();
            });

            this.textBoxes.Add(name, string.Empty);
            return name;
        }
    }
}
