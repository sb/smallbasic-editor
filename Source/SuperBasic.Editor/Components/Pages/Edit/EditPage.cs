// <copyright file="EditPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Edit
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Services;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities.Resources;

    public sealed class EditPage : MainLayout, IDisposable
    {
        public void Dispose()
        {
            JSInterop.Monaco.Dispose().ConfigureAwait(false);
        }

        public static void Inject(TreeComposer composer)
        {
            composer.Inject<EditPage>();
        }

        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("edit-page", body: () =>
            {
                LibraryExplorer.Inject(composer);

                composer.Element("main-space", body: () =>
                {
                    composer.Element("editor-space", body: () =>
                    {
                        MonacoEditor.Inject(composer, isReadOnly: false);
                    });

                    ErrorsSpace.Inject(composer);
                });
            });
        }

        protected override void ComposeLeftActions(TreeComposer composer)
        {
            Actions.Action(composer, "new", EditorResources.Actions_New, onClick: () => JSInterop.Monaco.ClearEditor(EditorResources.Actions_ClearEverythingConfirmation));
            Actions.Action(composer, "save", EditorResources.Actions_Save, onClick: JSInterop.Monaco.SaveToFile);
            Actions.Action(composer, "open", EditorResources.Actions_Open, onClick: () => JSInterop.Monaco.OpenFile(EditorResources.Actions_ClearEverythingConfirmation));
            Actions.Separator(composer);
            Actions.Action(composer, "cut", EditorResources.Actions_Cut, onClick: JSInterop.Monaco.Cut);
            Actions.Action(composer, "copy", EditorResources.Actions_Copy, onClick: JSInterop.Monaco.Copy);
            Actions.Action(composer, "paste", EditorResources.Actions_Paste, onClick: JSInterop.Monaco.Paste);
            Actions.Separator(composer);
            Actions.Action(composer, "undo", EditorResources.Actions_Undo, onClick: JSInterop.Monaco.Undo);
            Actions.Action(composer, "redo", EditorResources.Actions_Redo, onClick: JSInterop.Monaco.Redo);
            Actions.Separator(composer);
            Actions.DisabledAction(composer, "import", EditorResources.Actions_Import, message: EditorResources.Actions_DisabledMessage_ComingSoon);
            Actions.DisabledAction(composer, "publish", EditorResources.Actions_Publish, message: EditorResources.Actions_DisabledMessage_ComingSoon);
        }

        protected override void ComposeRightActions(TreeComposer composer)
        {
            ExeuctionActions.Inject(composer);
        }
    }

    public sealed class ExeuctionActions : SuperBasicComponent
    {
        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<ExeuctionActions>();
        }

        protected override void OnInit()
        {
            CompilationStore.CodeChanged += this.StateHasChanged;
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            if (CompilationStore.Compilation.Diagnostics.Any())
            {
                string message = string.Format(CultureInfo.CurrentCulture, EditorResources.Errors_Count, CompilationStore.Compilation.Diagnostics.Count);
                Actions.DisabledAction(composer, "run", EditorResources.Actions_Run, message: message);
                Actions.DisabledAction(composer, "debug", EditorResources.Actions_Debug, message: message);
            }
            else
            {
                Actions.Action(composer, "run", EditorResources.Actions_Run, onClick: () =>
                {
                    NavigationStore.NagivateTo(NavigationStore.PageId.Run);
                    return Task.CompletedTask;
                });

                Actions.Action(composer, "debug", EditorResources.Actions_Debug, onClick: null);
            }
        }
    }
}
