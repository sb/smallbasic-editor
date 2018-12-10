// <copyright file="LibraryExplorer.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Edit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    // TODO-later: show warning on deprecated types or members.
    // TODO-later: show when member/library is desktop only.
    public sealed class LibraryExplorer : SuperBasicComponent
    {
        private Method selectedMethod = default;

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<LibraryExplorer>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            SideBar.Inject(
                composer,
                title: EditorResources.SideBar_Libraries,
                showScrollArrows: true,
                items: Libraries.Types.Values.Select(library => new SideBar.Descriptor(
                    iconName: library.ExplorerIcon,
                    title: library.Name,
                    body: bodyComposer => this.ComposeLibraryBody(bodyComposer, library))));
        }

        private void ComposeLibraryBody(TreeComposer composer, Library library)
        {
            composer.Element("library-body", body: () =>
            {
                composer.Element("library-title", body: () =>
                {
                    composer.Element("icon", body: () => Micro.FontAwesome(composer, library.ExplorerIcon));
                    composer.Element("name", body: () => composer.Text(library.Name));
                });

                composer.Element("library-description", body: () => composer.Text(library.Description));

                if (library.Methods.Any())
                {
                    composer.Element("group-title", body: () =>
                    {
                        composer.Element("icon", body: () => Micro.FontAwesome(composer, "cube"));
                        composer.Element("name", body: () => composer.Text(EditorResources.LibraryExplorer_Methods));
                    });

                    foreach (var method in library.Methods.Values)
                    {
                        LibraryMethod.Inject(
                            composer,
                            isSelected: method == this.selectedMethod,
                            library: library,
                            method: method,
                            onHeaderClick: () => this.OnMethodHeaderClick(method));
                    }
                }

                if (library.Properties.Any())
                {
                    composer.Element("group-title", body: () =>
                    {
                        composer.Element("icon", body: () => Micro.FontAwesome(composer, "wrench"));
                        composer.Element("name", body: () => composer.Text(EditorResources.LibraryExplorer_Properties));
                    });

                    foreach (var property in library.Properties.Values)
                    {
                        composer.Element("member", body: () =>
                        {
                            composer.Element(name: "member-header-selected", body: () =>
                            {
                                composer.Element("member-title", body: () =>
                                {
                                    composer.Element("caret");
                                    composer.Element("name", body: () => composer.Text($"{library.Name}.{property.Name}"));
                                });

                                composer.Element("member-description", body: () => composer.Text(property.Description));
                            });
                        });
                    }
                }

                if (library.Events.Any())
                {
                    composer.Element("group-title", body: () =>
                    {
                        composer.Element("icon", body: () => Micro.FontAwesome(composer, "neuter"));
                        composer.Element("name", body: () => composer.Text(EditorResources.LibraryExplorer_Events));
                    });

                    foreach (var @event in library.Events.Values)
                    {
                        composer.Element("member", body: () =>
                        {
                            composer.Element(name: "member-header-selected", body: () =>
                            {
                                composer.Element("member-title", body: () =>
                                {
                                    composer.Element("caret");
                                    composer.Element("name", body: () => composer.Text($"{library.Name}.{@event.Name}"));
                                });

                                composer.Element("member-description", body: () => composer.Text(@event.Description));
                            });
                        });
                    }
                }
            });
        }

        private void OnMethodHeaderClick(Method method)
        {
            if (this.selectedMethod == method)
            {
                this.selectedMethod = default;
            }
            else
            {
                this.selectedMethod = method;
            }

            this.StateHasChanged();
        }

        private sealed class LibraryMethod : SuperBasicComponent
        {
            [Parameter]
            private bool IsSelected { get; set; }

            [Parameter]
            private Library Library { get; set; }

            [Parameter]
            private Method Method { get; set; }

            [Parameter]
            private Action OnHeaderClick { get; set; }

            internal static void Inject(TreeComposer composer, bool isSelected, Library library, Method method, Action onHeaderClick)
            {
                composer.Inject<LibraryMethod>(new Dictionary<string, object>
                {
                    { nameof(LibraryMethod.IsSelected), isSelected },
                    { nameof(LibraryMethod.Library), library },
                    { nameof(LibraryMethod.Method), method },
                    { nameof(LibraryMethod.OnHeaderClick), onHeaderClick }
                });
            }

            protected override void ComposeTree(TreeComposer composer)
            {
                composer.Element("member", body: () =>
                {
                    composer.Element(
                        name: this.IsSelected ? "member-header-selected" : "member-header",
                        events: new TreeComposer.Events
                        {
                            OnClick = arg => this.OnHeaderClick()
                        },
                        body: () =>
                        {
                            composer.Element("member-title", body: () =>
                            {
                                composer.Element("caret", body: () =>
                                {
                                    Micro.FontAwesome(composer, this.IsSelected ? "caret-down" : "caret-right");
                                });

                                composer.Element("name", body: () => composer.Text($"{this.Library.Name}.{this.Method.Name}()"));
                            });

                            composer.Element("member-description", body: () => composer.Text(this.Method.Description));
                        });

                    if (this.IsSelected)
                    {
                        composer.Element("member-body", body: () =>
                        {
                            composer.Element("example", body: () =>
                            {
                                composer.Text(string.Format(
                                    CultureInfo.CurrentCulture,
                                    "{0}{1}.{2}({3})",
                                    this.Method.ReturnsValue ? "result = " : string.Empty,
                                    this.Library.Name,
                                    this.Method.Name,
                                    this.Method.Parameters.Keys.Join(", ")));
                            });

                            if (this.Method.Parameters.Any())
                            {
                                composer.Element("block", body: () =>
                                {
                                    composer.Element("contents", body: () =>
                                    {
                                        foreach (var parameter in this.Method.Parameters.Values)
                                        {
                                            composer.Element("name", body: () => composer.Text(parameter.Name));
                                            composer.Element("description", body: () => composer.Text(parameter.Description));
                                        }
                                    });
                                });
                            }

                            composer.Element("block", body: () =>
                            {
                                composer.Element("contents", body: () =>
                                {
                                    if (this.Method.ReturnsValue)
                                    {
                                        composer.Element("name", body: () => composer.Text("result"));
                                        composer.Element("description", body: () => composer.Text(this.Method.ReturnValueDescription));
                                    }
                                    else
                                    {
                                        composer.Element("description", body: () => composer.Text(EditorResources.LibraryExplorer_ReturnsNothing));
                                    }
                                });
                            });
                        });
                    }
                });
            }
        }
    }
}
