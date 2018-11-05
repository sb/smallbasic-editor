// <copyright file="MonacoEditor.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Toolbox
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using SuperBasic.Editor.Components.Layout;

    // TODO: it messes up size when the explorer expands
    // TODO: logo link in header should not navigate
    public sealed class MonacoEditor : SuperBasicComponent
    {
        private const string InitialValue =
@"' Below is a sample code to print 'Hello, World!' on the screen.
' Press Run for output.
TextWindow.WriteLine(""Hello, World!"")";

        private bool alreadyRendered = false;
        private ElementRef editorElement = default;

        public static void Inject(TreeComposer composer)
        {
            composer.Inject<MonacoEditor>();
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.alreadyRendered)
            {
                await Interop.Monaco.Initialize(this.editorElement, InitialValue, isReadOnly: false).ConfigureAwait(false);
                this.alreadyRendered = true;
            }
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
