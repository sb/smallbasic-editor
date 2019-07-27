// <copyright file="LibraryExplorer.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Pages.Edit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Components.Toolbox;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Resources;

    public sealed class LibraryExplorer : SmallBasicComponent
    {
        private ElementRef scrollArea = default;
        private ElementRef scrollAreaContents = default;

        private double currentScrollMargin = 0;
        private Library selectedLibrary = default;

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<LibraryExplorer>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("library-explorer", body: () =>
            {
                composer.Element("navigation-area", body: () =>
                {
                    composer.Element(
                        name: "logo-area",
                        events: new TreeComposer.Events
                        {
                            OnClick = args =>
                            {
                                if (this.selectedLibrary.IsDefault())
                                {
                                    this.selectedLibrary = Libraries.Types.Values.First();
                                }
                                else
                                {
                                    this.selectedLibrary = default;
                                }
                            }
                        },
                        body: () =>
                        {
                            composer.Element("logo");
                        });

                    composer.Element(
                        name: "scroll-button",
                        events: new TreeComposer.Events
                        {
                            OnClick = args => this.ScrollUp(200)
                        },
                        body: () =>
                        {
                            composer.Element("scroll-up");
                        });

                    composer.Element("scroll-area", capture: element => this.scrollArea = element, body: () =>
                    {
                        composer.Element(
                            name: "scroll-area-contents",
                            capture: element => this.scrollAreaContents = element,
                            styles: new Dictionary<string, string>
                            {
                                { "margin-top", $"{this.currentScrollMargin}px" }
                            },
                            events: new TreeComposer.Events
                            {
                                OnMouseWheelAsync = async args =>
                                {
                                    if (args.DeltaY < 0)
                                    {
                                        this.ScrollUp(-args.DeltaY);
                                    }
                                    else
                                    {
                                        await this.ScrollDown(args.DeltaY).ConfigureAwait(false);
                                    }
                                }
                            },
                            body: () =>
                            {
                                foreach (var library in Libraries.Types.Values)
                                {
                                    composer.Element(
                                        name: this.selectedLibrary?.Name == library.Name ? "library-explorer-item-selected" : "library-explorer-item",
                                        events: new TreeComposer.Events
                                        {
                                            OnClick = args => this.selectedLibrary = library
                                        },
                                        body: () =>
                                        {
                                            composer.Element("icon", body: () => Micro.FontAwesome(composer, library.ExplorerIcon));
                                            composer.Element(
                                                name: library.Name.Length > 10 ? "long-name" : "short-name",
                                                body: () => composer.Text(library.Name));
                                        });
                                }
                            });
                    });

                    composer.Element(
                        name: "scroll-button",
                        events: new TreeComposer.Events
                        {
                            OnClick = async args => await this.ScrollDown(200).ConfigureAwait(false)
                        },
                        body: () =>
                        {
                            composer.Element("scroll-down");
                        });
                });

                if (!this.selectedLibrary.IsDefault())
                {
                    composer.Element("content-area", body: () =>
                    {
                        composer.Element("header", body: () =>
                        {
                            composer.Element("content-title", body: () => composer.Text(EditorResources.LibraryExplorer_Title));
                            composer.Element(
                                name: "minimize-button",
                                events: new TreeComposer.Events
                                {
                                    OnClick = args => this.selectedLibrary = default
                                },
                                body: () =>
                                {
                                    composer.Element("angle-left");
                                });
                        });

                        composer.Element("content", body: () => LibraryBody.Inject(composer, this.selectedLibrary));
                    });
                }
            });
        }

        private void ScrollUp(double amount)
        {
            if (this.currentScrollMargin != 0)
            {
                this.currentScrollMargin = Math.Min(0, this.currentScrollMargin + amount);
                this.StateHasChanged();
            }
        }

        private async Task ScrollDown(double amount)
        {
            double areaHeight = (double)await JSInterop.Layout.GetElementHeight(this.scrollArea).ConfigureAwait(false);
            double contentsHeight = (double)await JSInterop.Layout.GetElementHeight(this.scrollAreaContents).ConfigureAwait(false);

            double hiddenOffset = Math.Max(0, contentsHeight + this.currentScrollMargin - areaHeight);
            if (hiddenOffset != 0)
            {
                this.currentScrollMargin = this.currentScrollMargin - Math.Min(hiddenOffset, amount);
                this.StateHasChanged();
            }
        }
    }
}
