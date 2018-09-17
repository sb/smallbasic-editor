// <copyright file="SuperBasicEngine.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    public enum ExecutionState
    {
        Running,
        Paused,
        BlockedOnStringInput,
        BlockedOnNumberInput,
        Terminated,
    }

    public sealed class SuperBasicEngine
    {
        private bool isDebugging;
        private int currentSourceLine;
        private PluginsCollection plugins;
        private Dictionary<(string library, string eventName), string> eventCallbacks;

        public SuperBasicEngine(
            SuperBasicCompilation compilation,
            bool isDebugging = false,
            PluginsCollection plugins = null)
        {
            Debug.Assert(!compilation.Diagnostics.Any(), "Cannot execute a compilation with errors.");

            this.isDebugging = isDebugging;
            this.currentSourceLine = 0;
            this.plugins = plugins;
            this.eventCallbacks = new Dictionary<(string library, string eventName), string>();

            this.State = ExecutionState.Running;
            this.ExecutionStack = new Stack<Frame>();
            this.EvaluationStack = new Stack<BaseValue>();
            this.Memory = new ArrayValue();
            this.Modules = new Dictionary<string, RuntimeModule>();

            if (!this.plugins.IsDefault())
            {
                this.plugins.SetEventsCallback(this);
            }

            RuntimeModule mainModule = this.EmitAndSaveModule("Program", compilation.MainModule);

            foreach (BoundSubModule subModule in compilation.SubModules.Values)
            {
                this.EmitAndSaveModule(subModule.Name, subModule.Body);
            }

            this.ExecutionStack.Push(new Frame(mainModule));
        }

        public ExecutionState State { get; private set; }

        internal Stack<Frame> ExecutionStack { get; private set; }

        internal Stack<BaseValue> EvaluationStack { get; private set; }

        internal ArrayValue Memory { get; private set; }

        internal Dictionary<string, RuntimeModule> Modules { get; private set; }

        internal PluginsCollection Plugins
        {
            get
            {
                if (this.plugins.IsDefault())
                {
                    throw new InvalidOperationException("No plugins were provided to the engine.");
                }

                return this.plugins;
            }
        }

        public DebuggerSnapshot GetSnapshot()
        {
            return new DebuggerSnapshot(this.currentSourceLine, this.ExecutionStack, this.Memory);
        }

        public void Execute(bool pauseAtNextStatement = false)
        {
            Debug.Assert(this.isDebugging || !pauseAtNextStatement, $"Cannot {nameof(pauseAtNextStatement)} if not debugging.");
            Debug.Assert(this.State == ExecutionState.Running || this.State == ExecutionState.Paused, "Engine is not in a executable state.");

            if (this.State == ExecutionState.Paused)
            {
                this.State = ExecutionState.Running;
            }

            while (this.State == ExecutionState.Running)
            {
                if (this.ExecutionStack.Count == 0)
                {
                    this.State = ExecutionState.Terminated;
                    return;
                }

                Frame frame = this.ExecutionStack.Peek();
                if (frame.InstructionIndex == frame.Module.Instructions.Count)
                {
                    this.ExecutionStack.Pop();
                    continue;
                }

                BaseInstruction instruction = frame.Module.Instructions[frame.InstructionIndex];
                int instructionLine = instruction.Range.Start.Line;

                bool shouldPause = pauseAtNextStatement && this.State == ExecutionState.Running && this.currentSourceLine != instructionLine;
                this.currentSourceLine = instructionLine;

                if (shouldPause)
                {
                    this.Pause();
                    return;
                }
                else
                {
                    instruction.Execute(this, frame);
                }
            }
        }

        internal void SetEventCallback(string library, string eventName, string subModule)
        {
            this.eventCallbacks[(library, eventName)] = subModule;
        }

        internal void RaiseEvent(string library, string eventName)
        {
            if (this.eventCallbacks.TryGetValue((library, eventName), out string subModule))
            {
                this.ExecutionStack.Push(new Frame(this.Modules[subModule]));
            }
        }

        internal void Pause()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be paused.");
            if (this.isDebugging)
            {
                this.State = ExecutionState.Paused;
            }
        }

        internal void Terminate()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be terminated.");
            this.ExecutionStack.Clear();
        }

        internal void BlockOnStringInput()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be blocked.");
            this.State = ExecutionState.BlockedOnStringInput;
        }

        internal void BlockOnNumberInput()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be blocked.");
            this.State = ExecutionState.BlockedOnNumberInput;
        }

        private RuntimeModule EmitAndSaveModule(string name, BoundStatementBlock body)
        {
            ModuleEmitter emitter = new ModuleEmitter(body);
            RuntimeModule module = new RuntimeModule(name, emitter.Instructions);

            this.Modules.Add(module.Name, module);
            return module;
        }
    }
}
