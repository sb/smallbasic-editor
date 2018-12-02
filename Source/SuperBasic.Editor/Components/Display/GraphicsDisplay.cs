// <copyright file="GraphicsDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler;
    using SuperBasic.Editor.Components.Layout;

    public sealed class GraphicsDisplay : SuperBasicComponent
    {
        public GraphicsDisplay()
        {
            StaticStore.SetGraphicsDisplay(this);
        }

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<GraphicsDisplay>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("graphics-display", body: () =>
            {
                composer.Text("graphics");
            });
        }
    }
}
