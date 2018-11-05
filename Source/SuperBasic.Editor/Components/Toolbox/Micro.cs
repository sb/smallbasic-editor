// <copyright file="Micro.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Toolbox
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal static class Micro
    {
        public static void Clickable(TreeComposer composer, Action onClick, Action body)
        {
            composer.Element("clickable", body: body, attributes: new Dictionary<string, object>
            {
                { "onclick", onClick }
            });
        }

        public static void ClickableAsync(TreeComposer composer, Func<Task> onClick, Action body)
        {
            composer.Element("clickable", body: body, attributes: new Dictionary<string, object>
            {
                { "onclick", onClick }
            });
        }

        public static void FontAwesome(TreeComposer composer, string iconName)
        {
            composer.Element(name: "font-awesome-icon", attributes: new Dictionary<string, object>
            {
                { "class", $"fas fa-{iconName}" }
            });
        }
    }
}
