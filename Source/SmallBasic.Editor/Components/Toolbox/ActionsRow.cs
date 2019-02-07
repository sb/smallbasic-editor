// <copyright file="ActionsRow.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Toolbox
{
    using System;
    using System.Threading.Tasks;

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

        public static void Action(TreeComposer composer, string name, string title, Func<Task> onClick)
        {
            composer.Element(
                name: "action",
                events: new TreeComposer.Events
                {
                    OnClickAsync = args => onClick()
                },
                body: () =>
                {
                    composer.Element("icon-" + name);
                    composer.Text(title);
                });
        }

        public static void DisabledAction(TreeComposer composer, string name, string title, string message)
        {
            composer.Element("disabled-action-container", body: () =>
            {
                composer.Element("action", body: () =>
                {
                    composer.Element("icon-" + name);
                    composer.Text(title);
                });

                composer.Element("disabled-message", body: () => composer.Text(message));
            });
        }

        public static void Separator(TreeComposer composer)
        {
            composer.Element("separator");
        }
    }
}
