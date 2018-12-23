// <copyright file="DebugPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor.Components;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Compiler.Services;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Libraries;
    using SuperBasic.Editor.Libraries.Utilities;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    public sealed class DebugPage : MainLayout, IDisposable
    {
        private readonly AsyncEngine engine = new AsyncEngine(isDebugging: true);

        private bool isInitialized;

        public static void Inject(TreeComposer composer)
        {
            composer.Inject<DebugPage>();
        }

        public void Dispose()
        {
            this.engine.Dispose();
        }

        protected override void OnInit()
        {
            if (CompilationStore.Compilation.Diagnostics.Any())
            {
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return;
            }
        }

        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("debug-page", body: () =>
            {
                // LibraryExplorer.Inject(composer);
                composer.Element("main-space", body: () =>
                {
                    composer.Element("editor-space", body: () =>
                    {
                        MonacoEditor.Inject(composer, isReadOnly: true);
                    });

                    EngineDisplay.Inject(composer);
                });
            });
        }

        protected override void ComposeLeftActions(TreeComposer composer)
        {
            Actions.Action(composer, "back", EditorResources.Actions_Back, () =>
            {
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return Task.CompletedTask;
            });
        }

        protected override void ComposeRightActions(TreeComposer composer)
        {
            DebugPageExeuctionActions.Inject(composer, this.engine);
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                await Task.Run(() => this.engine.StartLoop()).ConfigureAwait(false);
            }
        }
    }

    public sealed class DebugPageExeuctionActions : SuperBasicComponent
    {
        [Parameter]
        private AsyncEngine Engine { get; set; }

        internal static void Inject(TreeComposer composer, AsyncEngine engine)
        {
            composer.Inject<DebugPageExeuctionActions>(new Dictionary<string, object>
            {
                { nameof(DebugPageExeuctionActions.Engine), engine }
            });
        }

        protected override void OnInit()
        {
            this.Engine.ExecutedStep += this.StateHasChanged;
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            switch (this.Engine.State)
            {
                case ExecutionState.Paused:
                    {
                        Actions.Action(composer, "nextline", EditorResources.Actions_NextLine, () =>
                        {
                            this.Engine.Continue(pauseAtNextLine: true);
                            // Adding one because monaco is one based.
                            return JSInterop.Monaco.HighlightLine(this.Engine.CurrentSourceLine + 1);
                        });

                        Actions.Action(composer, "continue", EditorResources.Actions_Continue, () =>
                        {
                            this.Engine.Continue(pauseAtNextLine: false);
                            return JSInterop.Monaco.RemoveDecorations();
                        });

                        break;
                    }

                case ExecutionState.Running:
                    {
                        Actions.Action(composer, "pause", EditorResources.Actions_Pause, () =>
                        {
                            this.Engine.Pause();
                            // Adding one because monaco is one based.
                            return JSInterop.Monaco.HighlightLine(this.Engine.CurrentSourceLine + 1);
                        });

                        break;
                    }
            }
        }
    }
}
