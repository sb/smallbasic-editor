// <copyright file="ErrorsSpace.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Edit
{
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities.Resources;

    public sealed class ErrorsSpace : SuperBasicComponent
    {
        private bool areErrorsExpanded = false;

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<ErrorsSpace>();
        }

        protected override void OnInit()
        {
            CompilationStore.CodeChanged += this.StateHasChanged;
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            if (!CompilationStore.Compilation.Diagnostics.Any())
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
                            composer.Text(string.Format(CultureInfo.CurrentCulture, EditorResources.Errors_Count, CompilationStore.Compilation.Diagnostics.Count));
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
                    foreach (var error in CompilationStore.Compilation.Diagnostics.OrderBy(d => d.Range.Start.Line))
                    {
                        var range = error.Range.ToMonacoRange();

                        Micro.ClickableAsync(
                            composer,
                            onClick: () => JSInterop.Monaco.SelectRange(range),
                            body: () =>
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
        }
    }
}
