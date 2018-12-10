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
        private bool shouldTerminate = false;

        public static void Inject(TreeComposer composer)
        {
            composer.Inject<RunPage>();
        }

        public void Dispose()
        {
            this.shouldTerminate = true;
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
            composer.Element("run-page", body: () =>
            {
                EngineDisplay.Inject(composer);
            });
        }

        protected override void ComposeLeftActions(TreeComposer composer)
        {
            Actions.Action(composer, "back", EditorResources.Actions_Back, () =>
            {
                this.shouldTerminate = true;
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return Task.CompletedTask;
            });
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                await Task.Run(async () =>
                {
                    using (var libraries = new LibrariesCollection())
                    {
                        var engine = new SuperBasicEngine(CompilationStore.Compilation, libraries, isDebugging: false);

                        void onTextInput(string text)
                        {
                            engine.InputReceived();
                        }

                        TextDisplayStore.TextInput += onTextInput;

                        while (!this.shouldTerminate)
                        {
                            switch (engine.State)
                            {
                                case ExecutionState.Running:
                                    TextDisplayStore.SetInputMode(AcceptedInputMode.None);
                                    await engine.Execute(pauseAtNextStatement: false).ConfigureAwait(false);
                                    break;
                                case ExecutionState.BlockedOnNumberInput:
                                    TextDisplayStore.SetInputMode(AcceptedInputMode.Numbers);
                                    break;
                                case ExecutionState.BlockedOnStringInput:
                                    TextDisplayStore.SetInputMode(AcceptedInputMode.Strings);
                                    break;
                                case ExecutionState.Paused:
                                    TextDisplayStore.SetInputMode(AcceptedInputMode.None);
                                    break;
                                case ExecutionState.Terminated:
                                    TextDisplayStore.SetInputMode(AcceptedInputMode.None);
                                    await libraries.TextWindow.WriteLine(EditorResources.TextDisplay_TerminateMessage).ConfigureAwait(false);
                                    TextDisplayStore.TextInput -= onTextInput;
                                    this.shouldTerminate = true;
                                    break;
                                default:
                                    throw ExceptionUtilities.UnexpectedValue(engine.State);
                            }

                            // Libraries should not call this, so that we actually refresh the UI once every batch
                            GraphicsDisplayStore.UpdateDisplay();

                            // Important to prevent th UI from freezing
                            await Task.Delay(1).ConfigureAwait(false);
                        }
                    }
                }).ConfigureAwait(false);
            }
        }
    }
}
