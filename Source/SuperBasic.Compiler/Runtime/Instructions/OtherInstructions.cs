// <copyright file="OtherInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using SuperBasic.Compiler.Scanning;

    internal sealed class InvokeSubModuleInstruction : BaseNonJumpInstruction
    {
        private readonly string subModuleName;

        public InvokeSubModuleInstruction(string subModuleName, TextRange range)
            : base(range)
        {
            this.subModuleName = subModuleName;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            Frame frame = new Frame(engine.Modules[this.subModuleName]);
            engine.ExecutionStack.AddLast(frame);
        }
    }

    internal sealed class MethodInvocationInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string method;

        public MethodInvocationInstruction(string library, string method, TextRange range)
            : base(range)
        {
            this.library = library;
            this.method = method;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            Libraries.Types[this.library].Methods[this.method].Execute(engine);
        }
    }

    internal sealed class StorePropertyInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string property;

        public StorePropertyInstruction(string library, string property, TextRange range)
            : base(range)
        {
            this.library = library;
            this.property = property;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            Libraries.Types[this.library].Properties[this.property].Setter(engine);
        }
    }

    internal sealed class LoadPropertyInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string property;

        public LoadPropertyInstruction(string library, string property, TextRange range)
            : base(range)
        {
            this.library = library;
            this.property = property;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            Libraries.Types[this.library].Properties[this.property].Getter(engine);
        }
    }

    internal sealed class SetEventCallBackInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string eventName;
        private readonly string subModule;

        public SetEventCallBackInstruction(string library, string eventName, string subModule, TextRange range)
            : base(range)
        {
            this.library = library;
            this.eventName = eventName;
            this.subModule = subModule;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            engine.SetEventCallback(this.library, this.eventName, this.subModule);
        }
    }
}
