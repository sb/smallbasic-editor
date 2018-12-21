// <copyright file="GraphicsDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Libraries;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;

    public sealed class GraphicsDisplay : SuperBasicComponent
    {
        private readonly DateTime animationStartTime = DateTime.Now;

        public GraphicsDisplay()
        {
            this.IsVisible = true;
            this.IsMouseVisible = true;
            GraphicsDisplayStore.SetDisplay(this);
        }

        public TimeSpan NextAnimationTime => DateTime.Now - this.animationStartTime;

        public ElementRef RenderArea { get; private set; }

        public LibrariesCollection Libraries { get; set; }

        public bool IsVisible { get; set; }

        public bool IsMouseVisible { get; set; }

        public void Update() => this.StateHasChanged();

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<GraphicsDisplay>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            if (!this.IsVisible)
            {
                return;
            }

            composer.Element(
                name: "graphics-display",
                attributes: new Dictionary<string, string>
                {
                    { "tabindex", "0" }, // required to receive focus
                },
                styles: new Dictionary<string, string>
                {
                    { "cursor", this.IsMouseVisible ? "initial" : "none" }
                },
                body: () =>
                {
                    composer.Element(
                        name: "svg",
                        capture: element => this.RenderArea = element,
                        attributes: new Dictionary<string, string>
                        {
                            { "height", "100%" },
                            { "width", "100%" },
                        },
                        events: new TreeComposer.Events
                        {
                            OnMouseDown = args => GraphicsDisplayStore.NotifyMouseDown(args.ClientX, args.ClientY, GetMouseButton(args.Button)),
                            OnMouseUp = args => GraphicsDisplayStore.NotifyMouseUp(args.ClientX, args.ClientY, GetMouseButton(args.Button)),
                            OnMouseMove = args => GraphicsDisplayStore.NotifyMouseMove(args.ClientX, args.ClientY),
                            OnKeyDown = args => GraphicsDisplayStore.NotifyKeyDown(args.Key),
                            OnKeyUp = args => GraphicsDisplayStore.NotifyKeyUp(args.Key),
                        },
                        body: () =>
                        {
                            if (!this.Libraries.IsDefault())
                            {
                                this.Libraries.ImageList.EmbedImages(composer);
                                this.Libraries.GraphicsWindow.ComposeTree(composer);
                                this.Libraries.Shapes.ComposeTree(composer);
                                this.Libraries.Turtle.ComposeTree(composer);
                            }
                        });

                    if (!this.Libraries.IsDefault())
                    {
                        this.Libraries.Controls.ComposeTree(composer);
                    }
                });
        }

        private static MouseButton GetMouseButton(long buttonNumber)
        {
            switch (buttonNumber)
            {
                case 0: return MouseButton.Left;
                case 1: return MouseButton.Middle;
                case 2: return MouseButton.Right;
                default: throw ExceptionUtilities.UnexpectedValue(buttonNumber);
            }
        }
    }
}
