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
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    [Route("/run")]
    public sealed class RunPage : MainLayout, IDisposable
    {
        private bool isInitialized = false;
        private bool isDisposed = false;
        private RuntimeAnalysis analysis;
        private SuperBasicEngine engine;

        [Inject]
        private IUriHelper UriHelper { get; set; }

        public void Dispose()
        {
            this.isDisposed = true;
        }

        protected override void OnInit()
        {
            if (StaticStore.Compilation.Diagnostics.Any())
            {
                this.UriHelper.NavigateTo("/edit");
                return;
            }

            this.analysis = new RuntimeAnalysis(StaticStore.Compilation);
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
                this.UriHelper.NavigateTo("/edit");
                return Task.CompletedTask;
            });
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;

                var libraries = new LibrariesCollection();
                this.engine = new SuperBasicEngine(StaticStore.Compilation, this.analysis, libraries, isDebugging: false);

                await Task.Run(async () =>
                {
                    StaticStore.TextDisplay.InputReceived += value =>
                    {
                        libraries.SetInputBuffer(value);
                        this.engine.InputReceived();
                    };

                    while (!this.isDisposed)
                    {
                        switch (this.engine.State)
                        {
                            case ExecutionState.Running:
                                StaticStore.TextDisplay.AcceptedInput = AcceptedInputKind.None;
                                await this.engine.Execute(pauseAtNextStatement: false).ConfigureAwait(false);
                                continue;
                            case ExecutionState.BlockedOnNumberInput:
                                StaticStore.TextDisplay.AcceptedInput = AcceptedInputKind.Numbers;
                                await Task.Delay(1).ConfigureAwait(false);
                                break;
                            case ExecutionState.BlockedOnStringInput:
                                StaticStore.TextDisplay.AcceptedInput = AcceptedInputKind.Strings;
                                await Task.Delay(1).ConfigureAwait(false);
                                break;
                            case ExecutionState.Paused:
                                StaticStore.TextDisplay.AcceptedInput = AcceptedInputKind.None;
                                await Task.Delay(1).ConfigureAwait(false);
                                break;
                            case ExecutionState.Terminated:
                                StaticStore.TextDisplay.AcceptedInput = AcceptedInputKind.None;
                                libraries.TerminateTextDisplay();
                                return;
                            default:
                                throw ExceptionUtilities.UnexpectedValue(this.engine.State);
                        }
                    }
                }).ConfigureAwait(false);
            }
        }
    }
}
