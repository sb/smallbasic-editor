// <copyright file="EngineDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Store;

    public sealed class EngineDisplay : SuperBasicComponent
    {
        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<EngineDisplay>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("engine-display", body: () =>
            {
                if (CompilationStore.Compilation.Analysis.UsesTextWindow)
                {
                    TextDisplay.Inject(composer);
                }

                if (CompilationStore.Compilation.Analysis.UsesGraphicsWindow)
                {
                    GraphicsDisplay.Inject(composer);
                }
            });
        }
    }
}
