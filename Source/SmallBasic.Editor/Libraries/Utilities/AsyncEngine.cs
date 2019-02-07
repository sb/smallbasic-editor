// <copyright file="AsyncEngine.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Utilities
{
    using System;
    using System.Threading.Tasks;
    using SmallBasic.Compiler;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Compiler.Scanning;
    using SmallBasic.Editor.Components.Display;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Resources;

    public sealed class AsyncEngine : IDisposable
    {
        private readonly SmallBasicEngine engine;

        public AsyncEngine(bool isDebugging)
        {
            this.Libraries = new LibrariesCollection();
            this.engine = new SmallBasicEngine(CompilationStore.Compilation, this.Libraries);

            if (isDebugging)
            {
                this.engine.Mode = ExecutionMode.Debug;
                this.engine.Pause();
            }
            else
            {
                this.engine.Mode = ExecutionMode.RunToEnd;
            }
        }

        public event Action ExecutedStep;

        public LibrariesCollection Libraries { get; private set; }

        public ExecutionState State => this.engine.State;

        public int CurrentSourceLine => this.engine.CurrentSourceLine;

        public DebuggerSnapshot GetSnapshot() => this.engine.GetSnapshot();

        public void Pause() => this.engine.Pause();

        public void Continue(bool pauseAtNextLine)
        {
            this.engine.Mode = pauseAtNextLine ? ExecutionMode.NextLine : ExecutionMode.Debug;
            this.engine.Continue();
        }

        public void Dispose() => this.Libraries.Dispose();

        public async Task StartLoop()
        {
            void onTextInput(string text)
            {
                this.engine.InputReceived();
            }

            TextDisplayStore.TextInput += onTextInput;

            while (true)
            {
                switch (this.engine.State)
                {
                    case ExecutionState.Running:
                        TextDisplayStore.SetInputMode(AcceptedInputMode.None);
                        await this.engine.Execute().ConfigureAwait(false);
                        this.ExecutedStep?.Invoke();
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
                        if (CompilationStore.Compilation.Analysis.UsesTextWindow)
                        {
                            await this.Libraries.TextWindow.WriteLine(EditorResources.TextDisplay_TerminateMessage).ConfigureAwait(false);
                        }

                        this.ExecutedStep?.Invoke();
                        TextDisplayStore.TextInput -= onTextInput;
                        return;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(this.engine.State);
                }

                // Libraries should not call this, so that we actually refresh the UI once every batch
                GraphicsDisplayStore.UpdateDisplay();

                // Important to prevent th UI from freezing
                await Task.Delay(1).ConfigureAwait(false);
            }
        }
    }
}
