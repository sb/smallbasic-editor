// <copyright file="IndexPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Edit
{
    using System;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Services;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Pages.Debug;
    using SuperBasic.Editor.Components.Pages.Run;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;

    [Route("/")] // For browser entry
    [Route("/index.html")] // For electron entry
    public sealed class IndexPage : SuperBasicComponent, IDisposable
    {
        private IUriHelper UriHelper { get; set; }

        public void Dispose()
        {
            NavigationStore.PageChanged -= this.StateHasChanged;
        }

        protected override void OnInit()
        {
            NavigationStore.PageChanged += this.StateHasChanged;
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            switch (NavigationStore.CurrentPage)
            {
                case NavigationStore.PageId.Edit:
                    EditPage.Inject(composer);
                    break;
                case NavigationStore.PageId.Run:
                    RunPage.Inject(composer);
                    break;
                case NavigationStore.PageId.Debug:
                    DebugPage.Inject(composer);
                    break;
                default:
                    throw ExceptionUtilities.UnexpectedValue(NavigationStore.CurrentPage);
            }
        }
    }
}
