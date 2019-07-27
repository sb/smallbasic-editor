// <copyright file="AsyncEngine.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
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

        private bool keepRunning = true;

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

        public void Dispose()
        {
            this.keepRunning = false;
            this.Libraries.Dispose();
        }

        public async Task StartLoop()
        {
            void onTextInput(string text)
            {
                this.engine.InputReceived();
            }

            TextDisplayStore.TextInput += onTextInput;

            await this.StartLookAux().ConfigureAwait(false);

            TextDisplayStore.TextInput -= onTextInput;
        }

        private async Task StartLookAux()
        {
            while (this.keepRunning)
            {
                switch (this.engine.State)
                {
                    case ExecutionState.Running:
                        await TextDisplayStore.SetInputMode(AcceptedInputMode.None).ConfigureAwait(false);
                        await this.engine.Execute().ConfigureAwait(false);
                        this.ExecutedStep?.Invoke();
                        break;
                    case ExecutionState.BlockedOnNumberInput:
                        await TextDisplayStore.SetInputMode(AcceptedInputMode.Numbers).ConfigureAwait(false);
                        break;
                    case ExecutionState.BlockedOnStringInput:
                        await TextDisplayStore.SetInputMode(AcceptedInputMode.Strings).ConfigureAwait(false);
                        break;
                    case ExecutionState.Paused:
                        await TextDisplayStore.SetInputMode(AcceptedInputMode.None).ConfigureAwait(false);
                        break;
                    case ExecutionState.Terminated:
                        await TextDisplayStore.SetInputMode(AcceptedInputMode.None).ConfigureAwait(false);
                        if (CompilationStore.Compilation.Analysis.UsesTextWindow)
                        {
                            await this.Libraries.TextWindow.WriteLine(EditorResources.TextDisplay_TerminateMessage).ConfigureAwait(false);
                        }

                        this.ExecutedStep?.Invoke();
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
