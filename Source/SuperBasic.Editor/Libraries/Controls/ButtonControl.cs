// <copyright file="ButtonControl.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Controls
{
    using SuperBasic.Editor.Components;

    internal sealed class ButtonControl : BaseControl
    {
        public ButtonControl(string name, string caption, decimal left, decimal top, decimal width, decimal height)
            : base(name, left, top, width, height)
        {
            this.Caption = caption;
        }

        public string Caption { get; set; }

        public override void ComposeTree(ControlsLibrary library, TreeComposer composer)
        {
            composer.Element(
                name: "button",
                body: () => composer.Text(this.Caption),
                events: new TreeComposer.Events
                {
                    OnClick = args => library.NotifyButtonClicked(this.Name)
                },
                styles: this.Styles);
        }
    }
}
