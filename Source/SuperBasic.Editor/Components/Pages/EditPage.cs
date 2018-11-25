// <copyright file="EditPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Utilities.Resources;

    [Route("/")] // For browser entry
    [Route("/edit")] // For navigation
    [Route("/index.html")] // For electron entry
    public sealed class EditPage : MainLayout
    {
        private const string InitialValue =
@"' Below is a sample code to print 'Hello, World!' on the screen.
' Press Run for output.
TextWindow.WriteLine(""Hello, World!"")";

        private MonacoEditor editor = default;
        private bool areErrorsExpanded = false;
        private IReadOnlyList<Diagnostic> currentErrors = Array.Empty<Diagnostic>();

        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("edit-page", body: () =>
            {
                LibraryExplorer.Inject(composer);

                composer.Element("main-space", body: () =>
                {
                    composer.Element("editor-space", body: () =>
                    {
                        MonacoEditor.Inject(composer, InitialValue, onChange: this.OnEditorChange, onInitialized: editor => this.editor = editor);
                    });

                    if (!this.currentErrors.Any())
                    {
                        return;
                    }

                    composer.Element("errors-space", body: () =>
                    {
                        Micro.Clickable(composer, onClick: () => this.areErrorsExpanded = !this.areErrorsExpanded, body: () =>
                        {
                            composer.Element("title-row", body: () =>
                            {
                                composer.Element("icon");

                                composer.Element("errors-count", body: () =>
                                {
                                    composer.Text(string.Format(CultureInfo.CurrentCulture, EditorResources.Errors_Count, this.currentErrors.Count));
                                });

                                composer.Element(this.areErrorsExpanded ? "caret-opened" : "caret-closed");
                            });
                        });

                        if (!this.areErrorsExpanded)
                        {
                            return;
                        }

                        composer.Element("header-line", body: () =>
                        {
                            composer.Element("line-number", body: () => composer.Text(EditorResources.Errors_Line));
                            composer.Element("description", body: () => composer.Text(EditorResources.Errors_Description));
                        });

                        composer.Element("errors-list", body: () =>
                        {
                            foreach (var error in this.currentErrors.OrderBy(d => d.Range.Start.Line))
                            {
                                var range = error.Range.ToMonacoRange();

                                Micro.ClickableAsync(composer, onClick: () => this.editor.SetSelection(range), body: () =>
                                {
                                    composer.Element("error-line", body: () =>
                                    {
                                        composer.Element("line-number", body: () => composer.Text(range.startLineNumber.ToString(CultureInfo.CurrentCulture)));
                                        composer.Element("description", body: () => composer.Text(error.ToDisplayString()));
                                    });
                                });
                            }
                        });
                    });
                });
            });
        }

        protected override void ComposeLeftActions(TreeComposer composer)
        {
            Actions.Button(composer, "new", EditorResources.Actions_New, onClick: null);
            Actions.Button(composer, "save", EditorResources.Actions_Save, onClick: null);
            Actions.Separator(composer);
            Actions.Button(composer, "import", EditorResources.Actions_Import, onClick: null);
            Actions.Button(composer, "publish", EditorResources.Actions_Publish, onClick: null);
            Actions.Separator(composer);
            Actions.Button(composer, "cut", EditorResources.Actions_Cut, onClick: null);
            Actions.Button(composer, "copy", EditorResources.Actions_Copy, onClick: null);
            Actions.Button(composer, "paste", EditorResources.Actions_Paste, onClick: null);
            Actions.Separator(composer);
            Actions.Button(composer, "undo", EditorResources.Actions_Undo, onClick: null);
            Actions.Button(composer, "redo", EditorResources.Actions_Redo, onClick: null);
        }

        protected override void ComposeRightActions(TreeComposer composer)
        {
            Actions.Button(composer, "run", EditorResources.Actions_Run, onClick: null);
            Actions.Button(composer, "debug", EditorResources.Actions_Debug, onClick: null);
        }

        private Task<IEnumerable<TextRange>> OnEditorChange(string newCode)
        {
            this.currentErrors = new SuperBasicCompilation(newCode).Diagnostics;
            this.StateHasChanged();
            return Task.FromResult(this.currentErrors.Select(d => d.Range));
        }
    }
}
