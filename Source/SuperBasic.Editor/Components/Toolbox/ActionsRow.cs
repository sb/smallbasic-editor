// <copyright file="ActionsRow.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Toolbox
{
    using System;

    internal static class Actions
    {
        public static void Row(TreeComposer composer, Action left = null, Action right = null)
        {
            composer.Element("actions-row", body: () =>
            {
                composer.Element("left-actions", body: left);
                composer.Element("right-actions", body: right);
            });
        }

        public static void Button(TreeComposer composer, string name, string title, Action onClick)
        {
            Micro.Clickable(composer, onClick, body: () =>
            {
                composer.Element("action", body: () =>
                {
                    composer.Element("icon-" + name);
                    composer.Text(title);
                });
            });
        }

        public static void Separator(TreeComposer composer)
        {
            composer.Element("separator");
        }
    }
}
