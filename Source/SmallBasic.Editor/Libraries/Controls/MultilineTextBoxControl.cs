// <copyright file="MultilineTextBoxControl.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Controls
{
    using System.Collections.Generic;
    using SmallBasic.Editor.Components;

    internal sealed class MultilineTextBoxControl : BaseControl
    {
        public MultilineTextBoxControl(string name, decimal left, decimal top, decimal width, decimal height)
            : base(name, left, top, width, height)
        {
            this.Text = string.Empty;
        }

        public string Text { get; set; }

        public override void ComposeTree(ControlsLibrary library, TreeComposer composer)
        {
            composer.Element(
                name: "textarea",
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
                    { "value", this.Text },
                });
        }
    }
}
