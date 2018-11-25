// <copyright file="MonacoEditor.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Toolbox
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Compiler.Services;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Utilities;

    public sealed class MonacoEditor : SuperBasicComponent
    {
        private ElementRef editorElement = default;

        [Parameter]
        private bool IsReadOnly { get; set; }

        public static void Inject(TreeComposer composer, bool isReadOnly)
        {
            composer.Inject<MonacoEditor>(new Dictionary<string, object>
            {
                { nameof(MonacoEditor.IsReadOnly), isReadOnly },
            });
        }

        protected override async Task OnAfterRenderAsync()
        {
            await JSInterop.Monaco.Initialize(this.editorElement, StaticStore.Compilation.Text, this.IsReadOnly).ConfigureAwait(false);
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("editor-container", body: () =>
            {
                composer.Element("editor", capture: element => this.editorElement = element);
            });
        }
    }
}
