// <copyright file="AsyncEngine.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Utilities
{
    using System;
    using System.Threading.Tasks;
    using SuperBasic.Compiler;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Editor.Store;
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    public sealed class AsyncEngine
    {
        private readonly bool isDebugging;
        private State state;

        public AsyncEngine(bool isDebugging)
        {
            this.state = State.Initialized;
            this.isDebugging = isDebugging;
        }

        private enum State
        {
            Initialized,
            Running,
            Stopped,
        }

        public void Stop()
        {
            this.state = State.Stopped;
        }

        public async Task Start()
        {
            this.state = State.Running;

            using (var libraries = new LibrariesCollection())
            {
                var engine = new SuperBasicEngine(CompilationStore.Compilation, libraries, this.isDebugging);

                void onTextInput(string text)
                {
                    engine.InputReceived();
                }

                TextDisplayStore.TextInput += onTextInput;

                while (this.state == State.Running)
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
                            this.Stop();
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
        }
    }
}
