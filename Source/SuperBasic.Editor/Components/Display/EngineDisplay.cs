// <copyright file="EngineDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Libraries.Utilities;
    using SuperBasic.Editor.Store;

    public sealed class EngineDisplay : SuperBasicComponent
    {
        [Parameter]
        private AsyncEngine Engine { get; set; }

        internal static void Inject(TreeComposer composer, AsyncEngine engine)
        {
            composer.Inject<EngineDisplay>(new Dictionary<string, object>
            {
                { nameof(EngineDisplay.Engine), engine }
            });
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "engine-display",
                attributes: new Dictionary<string, string>
                {
                    // Important to prevent the right-click menu
                    { "oncontextmenu", "return false;" }
                },
                body: () =>
                {
                    if (CompilationStore.Compilation.Analysis.UsesTextWindow)
                    {
                        TextDisplay.Inject(composer);
                    }

                    if (CompilationStore.Compilation.Analysis.UsesGraphicsWindow)
                    {
                        GraphicsDisplay.Inject(composer, this.Engine.Libraries);
                    }
                });
        }
    }
}
