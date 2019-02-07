// <copyright file="Micro.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Toolbox
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;

    internal static class Micro
    {
        public static void FontAwesome(TreeComposer composer, string iconName)
        {
            composer.Element(name: "font-awesome-icon", attributes: new Dictionary<string, string>
            {
                { "class", $"fas fa-{iconName}" }
            });
        }
    }
}
