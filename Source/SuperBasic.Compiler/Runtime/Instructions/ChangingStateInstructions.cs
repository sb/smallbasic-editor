// <copyright file="ChangingStateInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using SuperBasic.Compiler.Scanning;

    internal sealed class PauseInstruction : BaseNonJumpInstruction
    {
        public PauseInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SuperBasicEngine engine) => engine.Pause();
    }

    internal sealed class TerminateInstruction : BaseNonJumpInstruction
    {
        public TerminateInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SuperBasicEngine engine) => engine.Terminate();
    }

    internal sealed class BlockOnStringInputInstruction : BaseNonJumpInstruction
    {
        public BlockOnStringInputInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SuperBasicEngine engine) => engine.BlockOnStringInput();
    }

    internal sealed class BlockOnNumberInputInstruction : BaseNonJumpInstruction
    {
        public BlockOnNumberInputInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SuperBasicEngine engine) => engine.BlockOnNumberInput();
    }
}
