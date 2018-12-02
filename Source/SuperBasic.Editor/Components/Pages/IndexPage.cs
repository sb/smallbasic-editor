// <copyright file="IndexPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Edit
{
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Services;
    using SuperBasic.Editor.Components.Layout;

    [Route("/")] // For browser entry
    [Route("/index.html")] // For electron entry
    public sealed class IndexPage : MainLayout
    {
        [Inject]
        private IUriHelper UriHelper { get; set; }

        protected override void OnInit()
        {
            this.UriHelper.NavigateTo("/edit");
        }
    }
}
