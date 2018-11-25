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

    // TODO: logo link in header should not navigate
    public sealed class MonacoEditor : SuperBasicComponent, IDisposable
    {
        private static readonly Dictionary<string, MonacoEditor> ActiveEditors = new Dictionary<string, MonacoEditor>();

        private string id = default;
        private ElementRef editorElement = default;

        [Parameter]
        private string InitialValue { get; set; }

        [Parameter]
        private bool IsReadOnly { get; set; }

        [Parameter]
        private Func<string, Task<IEnumerable<TextRange>>> OnChange { get; set; }

        [Parameter]
        private Action<MonacoEditor> OnInitialized { get; set; }

        public static void Inject(
            TreeComposer composer,
            string initialValue, 
            bool isReadOnly = false,
            Func<string, Task<IEnumerable<TextRange>>> onChange = null,
            Action<MonacoEditor> onInitialized = null)
        {
            composer.Inject<MonacoEditor>(new Dictionary<string, object>
            {
                { nameof(MonacoEditor.InitialValue), initialValue },
                { nameof(MonacoEditor.IsReadOnly), isReadOnly },
                { nameof(MonacoEditor.OnChange), onChange },
                { nameof(MonacoEditor.OnInitialized), onInitialized }
            });
        }

        public static async Task<IEnumerable<TextRange>> TriggerOnChange(string id, string code)
        {
            if (ActiveEditors.TryGetValue(id, out MonacoEditor editor) && !editor.OnChange.IsDefault())
            {
                return await editor.OnChange(code).ConfigureAwait(false);
            }

            return Array.Empty<TextRange>();
        }

        public void Dispose()
        {
            ActiveEditors.Remove(this.id);
        }

        public Task SetSelection(MonacoRange range) => JSInterop.Monaco.SelectRange(this.id, range);

        protected override async Task OnAfterRenderAsync()
        {
            if (this.id.IsDefault())
            {
                this.id = await JSInterop.Monaco.Initialize(this.editorElement, this.InitialValue, this.IsReadOnly).ConfigureAwait(false);
                ActiveEditors.Add(this.id, this);

                if (!this.OnInitialized.IsDefault())
                {
                    this.OnInitialized(this);
                }
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
