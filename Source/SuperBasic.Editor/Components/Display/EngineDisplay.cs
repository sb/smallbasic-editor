// <copyright file="EngineDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Editor.Components.Layout;

    public sealed class EngineDisplay : SuperBasicComponent
    {
        [Parameter]
        private RuntimeAnalysis Analysis { get; set; }

        internal static void Inject(TreeComposer composer, RuntimeAnalysis analysis)
        {
            composer.Inject<EngineDisplay>(new Dictionary<string, object>
            {
                { nameof(EngineDisplay.Analysis), analysis },
            });
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("engine-display", body: () =>
            {
                if (this.Analysis.UsesTextWindow)
                {
                    TextDisplay.Inject(composer);
                }

                if (this.Analysis.UsesGraphicsWindow)
                {
                    GraphicsDisplay.Inject(composer);
                }
            });
        }
    }
}
