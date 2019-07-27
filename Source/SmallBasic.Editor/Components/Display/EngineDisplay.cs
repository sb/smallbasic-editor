// <copyright file="EngineDisplay.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Display
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor.Components;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;

    public sealed class EngineDisplay : SmallBasicComponent
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
