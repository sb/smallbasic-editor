// <copyright file="GraphicsDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;

    public sealed class GraphicsDisplay : SuperBasicComponent
    {
        public GraphicsDisplay()
        {
            GraphicsDisplayStore.SetDisplay(this);
        }

        public ElementRef RenderArea { get; private set; }

        public Action<TreeComposer> ControlsLibraryComposer { get; set; }

        public Action<TreeComposer> GraphicsLibraryComposer { get; set; }

        public void Update()
        {
            this.StateHasChanged();
        }

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<GraphicsDisplay>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "graphics-display",
                attributes: new Dictionary<string, string>
                {
                    { "tabindex", "0" }, // required to receive focus
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
                            OnMouseDown = args => GraphicsDisplayStore.NotifyMouseDown(args.ClientX, args.ClientY),
                            OnMouseUp = args => GraphicsDisplayStore.NotifyMouseUp(args.ClientX, args.ClientY),
                            OnMouseMove = args => GraphicsDisplayStore.NotifyMouseMove(args.ClientX, args.ClientY),
                            OnKeyDown = args => GraphicsDisplayStore.NotifyKeyDown(args.Key),
                            OnKeyUp = args => GraphicsDisplayStore.NotifyKeyUp(args.Key),
                        },
                        body: () =>
                        {
                            if (!this.GraphicsLibraryComposer.IsDefault())
                            {
                                this.GraphicsLibraryComposer(composer);
                            }
                        });

                    if (!this.ControlsLibraryComposer.IsDefault())
                    {
                        this.ControlsLibraryComposer(composer);
                    }
                });
        }
    }
}
