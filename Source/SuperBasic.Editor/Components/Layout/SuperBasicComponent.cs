// <copyright file="SuperBasicComponent.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Layout
{
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.RenderTree;

    public abstract class SuperBasicComponent : BlazorComponent
    {
        protected abstract void ComposeTree(TreeComposer composer);

        protected override sealed void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            this.ComposeTree(new TreeComposer(builder));
        }
    }
}
