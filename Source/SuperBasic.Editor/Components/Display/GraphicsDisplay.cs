// <copyright file="GraphicsDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Store;

    public sealed class GraphicsDisplay : SuperBasicComponent
    {
        private bool isInitialized = false;
        private ElementRef graphicsElementRef;

        public GraphicsDisplay()
        {
            GraphicsDisplayStore.SetDisplay(this);
        }

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<GraphicsDisplay>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("graphics-display", capture: element => this.graphicsElementRef = element);
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                await JSInterop.Controls.Initialize(this.graphicsElementRef).ConfigureAwait(false);
                this.isInitialized = true;
            }
        }
    }
}
