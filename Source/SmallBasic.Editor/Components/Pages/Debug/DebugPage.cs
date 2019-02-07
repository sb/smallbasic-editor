// <copyright file="DebugPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Pages.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor.Components;
    using SmallBasic.Compiler;
    using SmallBasic.Compiler.Scanning;
    using SmallBasic.Compiler.Services;
    using SmallBasic.Editor.Components.Display;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Components.Toolbox;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Editor.Libraries;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Resources;

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
            this.engine.ExecutedStep -= this.ExecutedStep;
            this.engine.Dispose();
        }

        protected override void OnInit()
        {
            if (CompilationStore.Compilation.Diagnostics.Any())
            {
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return;
            }

            this.engine.ExecutedStep += this.ExecutedStep;
        }

        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("debug-page", body: () =>
            {
                MemoryExplorer.Inject(composer, this.engine);

                composer.Element("main-space", body: () =>
                {
                    composer.Element("editor-space", body: () =>
                    {
                        MonacoEditor.Inject(composer, isReadOnly: true);
                    });

                    EngineDisplay.Inject(composer, this.engine);
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

        private void ExecutedStep()
        {
            if (this.engine.State == ExecutionState.Paused)
            {
                // Adding one because monaco is one based.
                Task.Run(() => JSInterop.Monaco.HighlightLine(this.engine.CurrentSourceLine + 1));
            }
        }
    }

    public sealed class DebugPageExeuctionActions : SmallBasicComponent
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
                            return Task.CompletedTask;
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
                            return Task.CompletedTask;
                        });

                        break;
                    }
            }
        }
    }
}
