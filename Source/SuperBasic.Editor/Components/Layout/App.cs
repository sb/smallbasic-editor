// <copyright file="App.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Layout
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Routing;

    // Configuring this here is temporary. Later we'll move the app config
    // into Program.cs, and it won't be necessary to specify AppAssembly.
    public sealed class App : SuperBasicComponent
    {
        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Inject<Router>(new Dictionary<string, object>
            {
                { "AppAssembly", RuntimeHelpers.TypeCheck(typeof(App).Assembly) }
            });
        }
    }
}
