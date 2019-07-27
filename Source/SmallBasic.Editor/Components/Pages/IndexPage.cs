// <copyright file="IndexPage.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Pages.Edit
{
    using System;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Services;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Components.Pages.Debug;
    using SmallBasic.Editor.Components.Pages.Run;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;

    [Route("/")] // For browser entry
    [Route("/index.html")] // For electron entry
    public sealed class IndexPage : SmallBasicComponent, IDisposable
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
