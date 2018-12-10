// <copyright file="TextBoxControl.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Controls
{
    using System.Collections.Generic;
    using SuperBasic.Editor.Components;

    internal sealed class TextBoxControl : BaseControl
    {
        public TextBoxControl(string name, decimal left, decimal top, decimal width, decimal height)
            : base(name, left, top, width, height)
        {
            this.Text = string.Empty;
        }

        public string Text { get; set; }

        public override void ComposeTree(ControlsLibrary library, TreeComposer composer)
        {
            composer.Element(
                name: "input",
                styles: this.Styles,
                events: new TreeComposer.Events
                {
                    OnInput = args =>
                    {
                        this.Text = args.Value.ToString();
                        library.NotifyTextTyped(this.Name);
                    }
                },
                attributes: new Dictionary<string, string>()
                {
                        { "type", "text" },
                });
        }
    }
}
