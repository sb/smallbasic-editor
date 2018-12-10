// <copyright file="SideBar.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Toolbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Utilities;

    public sealed class SideBar : SuperBasicComponent
    {
        private ElementRef scrollArea = default;
        private ElementRef scrollAreaContents = default;

        private double currentScrollMargin = 0;
        private Descriptor selectedItem = default;

        [Parameter]
        private string Title { get; set; }

        [Parameter]
        private bool ShowScrollArrows { get; set; }

        [Parameter]
        private IEnumerable<Descriptor> Items { get; set; }

        internal static void Inject(TreeComposer composer, string title, bool showScrollArrows, IEnumerable<Descriptor> items)
        {
            composer.Inject<SideBar>(new Dictionary<string, object>
            {
                { nameof(SideBar.Title), title },
                { nameof(SideBar.ShowScrollArrows), showScrollArrows },
                { nameof(SideBar.Items), items }
            });
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("side-bar", body: () =>
            {
                composer.Element("navigation-area", body: () =>
                {
                    composer.Element(
                        name: "logo-area",
                        events: new TreeComposer.Events
                        {
                            OnClick = args =>
                            {
                                if (this.selectedItem.IsDefault())
                                {
                                    this.selectedItem = this.Items.First();
                                }
                                else
                                {
                                    this.selectedItem = default;
                                }
                            }
                        },
                        body: () =>
                        {
                            composer.Element("logo");
                        });

                    if (this.ShowScrollArrows)
                    {
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
                    }

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
                                foreach (var item in this.Items)
                                {
                                    composer.Element(
                                        name: this.selectedItem?.Title == item.Title ? "side-bar-item-selected" : "side-bar-item",
                                        events: new TreeComposer.Events
                                        {
                                            OnClick = args => this.selectedItem = item
                                        },
                                        body: () =>
                                        {
                                            composer.Element("icon", body: () => Micro.FontAwesome(composer, item.IconName));
                                            composer.Element(
                                                name: item.Title.Length > 10 ? "long-name" : "short-name",
                                                body: () => composer.Text(item.Title));
                                        });
                                }
                            });
                    });

                    if (this.ShowScrollArrows)
                    {
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
                    }
                });

                if (this.selectedItem.IsDefault())
                {
                    return;
                }

                composer.Element("content-area", body: () =>
                {
                    composer.Element("header", body: () =>
                    {
                        composer.Element("content-title", body: () => composer.Text(this.Title));
                        composer.Element(
                            name: "minimize-button",
                            events: new TreeComposer.Events
                            {
                                OnClick = args => this.selectedItem = null
                            },
                            body: () =>
                            {
                                composer.Element("angle-left");
                            });
                    });

                    composer.Element("content", body: () => this.selectedItem.Body(composer));
                });
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

        internal sealed class Descriptor
        {
            public Descriptor(string iconName, string title, Action<TreeComposer> body)
            {
                this.IconName = iconName;
                this.Title = title;
                this.Body = body;
            }

            public string IconName { get; private set; }

            public string Title { get; private set; }

            public Action<TreeComposer> Body { get; private set; }
        }
    }
}
