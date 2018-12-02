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
        private Descriptor selectedItem = default;

        private ElementRef upButton = default;
        private ElementRef scrollContentsArea = default;
        private ElementRef downButton = default;

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
                    Micro.Clickable(
                        composer,
                        onClick: () =>
                        {
                            if (this.selectedItem.IsDefault())
                            {
                                this.selectedItem = this.Items.First();
                            }
                            else
                            {
                                this.selectedItem = default;
                            }
                        },
                        body: () =>
                        {
                            composer.Element("logo-area", body: () =>
                            {
                                composer.Element("logo");
                            });
                        });

                    if (this.ShowScrollArrows)
                    {
                        composer.Element("scroll-button", body: () =>
                        {
                            composer.Element("scroll-up", capture: element => this.upButton = element);
                        });
                    }

                    composer.Element("scroll-area", body: () =>
                    {
                        composer.Element(
                            name: "scroll-area-contents",
                            capture: element => this.scrollContentsArea = element,
                            body: () =>
                            {
                                foreach (var item in this.Items)
                                {
                                    Micro.Clickable(composer, onClick: () => this.selectedItem = item, body: () =>
                                    {
                                        composer.Element(name: this.selectedItem?.Title == item.Title ? "side-bar-item-selected" : "side-bar-item", body: () =>
                                        {
                                            composer.Element("icon", body: () => Micro.FontAwesome(composer, item.IconName));
                                            composer.Element(
                                                name: item.Title.Length > 10 ? "long-name" : "short-name",
                                                body: () => composer.Text(item.Title));
                                        });
                                    });
                                }
                            });
                    });

                    if (this.ShowScrollArrows)
                    {
                        composer.Element("scroll-button", body: () =>
                        {
                            composer.Element("scroll-down", capture: element => this.downButton = element);
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
                        composer.Element("minimize-button", body: () =>
                        {
                            Micro.Clickable(composer, onClick: () => this.selectedItem = null, body: () =>
                            {
                                composer.Element("angle-left");
                            });
                        });
                    });

                    composer.Element("content", body: () => this.selectedItem.Body(composer));
                });
            });
        }

        protected override Task OnAfterRenderAsync()
        {
            if (this.ShowScrollArrows)
            {
                return JSInterop.Layout.AttachSideBarEvents(this.upButton, this.scrollContentsArea, this.downButton);
            }
            else
            {
                return Task.CompletedTask;
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
