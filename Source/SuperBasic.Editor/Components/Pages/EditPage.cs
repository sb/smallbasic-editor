// <copyright file="EditPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages
{
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Utilities.Resources;

    [Route("/")] // For browser entry
    [Route("/edit")] // For navigation
    [Route("/index.html")] // For electron entry
    public sealed class EditPage : MainLayout
    {
        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("edit-page", body: () =>
            {
                LibraryExplorer.Inject(composer);

                composer.Element("main-space", body: () =>
                {
                    composer.Element("editor-space", body: () =>
                    {
                        MonacoEditor.Inject(composer);
                    });

                    composer.Element("errors-space", body: () =>
                    {
                        composer.Text("errros");
                    });
                });
            });
        }

        protected override void ComposeLeftActions(TreeComposer composer)
        {
            Actions.Button(composer, "new", EditorResources.Actions_New, onClick: null);
            Actions.Button(composer, "save", EditorResources.Actions_Save, onClick: null);
            Actions.Separator(composer);
            Actions.Button(composer, "import", EditorResources.Actions_Import, onClick: null);
            Actions.Button(composer, "publish", EditorResources.Actions_Publish, onClick: null);
            Actions.Separator(composer);
            Actions.Button(composer, "cut", EditorResources.Actions_Cut, onClick: null);
            Actions.Button(composer, "copy", EditorResources.Actions_Copy, onClick: null);
            Actions.Button(composer, "paste", EditorResources.Actions_Paste, onClick: null);
            Actions.Separator(composer);
            Actions.Button(composer, "undo", EditorResources.Actions_Undo, onClick: null);
            Actions.Button(composer, "redo", EditorResources.Actions_Redo, onClick: null);
        }

        protected override void ComposeRightActions(TreeComposer composer)
        {
            Actions.Button(composer, "run", EditorResources.Actions_Run, onClick: null);
            Actions.Button(composer, "debug", EditorResources.Actions_Debug, onClick: null);
        }
    }
}
