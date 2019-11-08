// <copyright file="ErrorsSpace.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Pages.Edit
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Components.Toolbox;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities.Resources;

    public sealed class ErrorsSpace : SmallBasicComponent, IDisposable
    {
        private bool areErrorsExpanded = false;

        public void Dispose()
        {
            CompilationStore.CodeChanged -= this.StateHasChanged;
        }

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
                composer.Element(
                    name: "title-row",
                    events: new TreeComposer.Events
                    {
                        OnClick = args => this.areErrorsExpanded = !this.areErrorsExpanded
                    },
                    body: () =>
                    {
                        int errorCount = CompilationStore.Compilation.Diagnostics.Where(error => error.IsFatal()).Count();
                        if (errorCount > 0)
                        {
                            composer.Element("error-icon");

                            composer.Element("errors-count", body: () =>
                            {
                                composer.Text(string.Format(CultureInfo.CurrentCulture, EditorResources.Errors_Count, errorCount));
                            });
                        }

                        int warningCount = CompilationStore.Compilation.Diagnostics.Where(error => !error.IsFatal()).Count();
                        if (warningCount > 0)
                        {
                            composer.Element("warning-icon");

                            composer.Element("errors-count", body: () =>
                            {
                                composer.Text(string.Format(CultureInfo.CurrentCulture, EditorResources.Warnings_Count, warningCount));
                            });
                        }

                        composer.Element(this.areErrorsExpanded ? "caret-opened" : "caret-closed");
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
                    foreach (var error in CompilationStore.Compilation.Diagnostics.Where(error => error.IsFatal()).OrderBy(d => d.Range.Start.Line))
                    {
                        var range = error.Range.ToMonacoRange();

                        composer.Element(
                            name: "error-line",
                            events: new TreeComposer.Events
                            {
                                OnClickAsync = args => JSInterop.Monaco.SelectRange(range)
                            },
                            body: () =>
                            {
                                composer.Element("error-icon");
                                composer.Element("line-number", body: () => composer.Text(range.startLineNumber.ToString(CultureInfo.CurrentCulture)));
                                composer.Element("description", body: () => composer.Text(error.ToDisplayString()));
                            });
                    }

                    foreach (var error in CompilationStore.Compilation.Diagnostics.Where(error => !error.IsFatal()).OrderBy(d => d.Range.Start.Line))
                    {
                        var range = error.Range.ToMonacoRange();

                        composer.Element(
                            name: "error-line",
                            events: new TreeComposer.Events
                            {
                                OnClickAsync = args => JSInterop.Monaco.SelectRange(range)
                            },
                            body: () =>
                            {
                                composer.Element("warning-icon");
                                composer.Element("line-number", body: () => composer.Text(range.startLineNumber.ToString(CultureInfo.CurrentCulture)));
                                composer.Element("description", body: () => composer.Text(error.ToDisplayString()));
                            });
                    }
                });
            });
        }
    }
}
