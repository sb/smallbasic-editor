// <copyright file="RunPage.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Pages.Run
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Services;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Components.Toolbox;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Editor.Libraries;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    public sealed class RunPage : MainLayout, IDisposable
    {
        private bool isInitialized = false;
        private bool isDisposed = false;
        private RuntimeAnalysis analysis;

        public static void Inject(TreeComposer composer)
        {
            composer.Inject<RunPage>();
        }

        public void Dispose()
        {
            this.isDisposed = true;
        }

        protected override void OnInit()
        {
            if (CompilationStore.Compilation.Diagnostics.Any())
            {
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return;
            }

            this.analysis = new RuntimeAnalysis(CompilationStore.Compilation);
        }

        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("run-page", body: () =>
            {
                EngineDisplay.Inject(composer, this.analysis);
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

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;

                var libraries = new LibrariesCollection();
                var engine = new SuperBasicEngine(CompilationStore.Compilation, this.analysis, libraries, isDebugging: false);

                TextDisplayStore.TextInput += value =>
                {
                    if (!this.isDisposed)
                    {
                        engine.InputReceived();
                    }
                };

                await Task.Run(async () =>
                {
                    while (!this.isDisposed)
                    {
                        switch (engine.State)
                        {
                            case ExecutionState.Running:
                                TextDisplayStore.Display.AcceptedInput = AcceptedInputKind.None;
                                await engine.Execute(pauseAtNextStatement: false).ConfigureAwait(false);
                                break;
                            case ExecutionState.BlockedOnNumberInput:
                                TextDisplayStore.Display.AcceptedInput = AcceptedInputKind.Numbers;
                                break;
                            case ExecutionState.BlockedOnStringInput:
                                TextDisplayStore.Display.AcceptedInput = AcceptedInputKind.Strings;
                                break;
                            case ExecutionState.Paused:
                                TextDisplayStore.Display.AcceptedInput = AcceptedInputKind.None;
                                break;
                            case ExecutionState.Terminated:
                                TextDisplayStore.Display.AcceptedInput = AcceptedInputKind.None;
                                libraries.TextWindow.WriteLine(EditorResources.TextDisplay_TerminateMessage);
                                return;
                            default:
                                throw ExceptionUtilities.UnexpectedValue(engine.State);
                        }

                        // Important to prevent th UI from freezing
                        await Task.Delay(1).ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);
            }
        }
    }
}
