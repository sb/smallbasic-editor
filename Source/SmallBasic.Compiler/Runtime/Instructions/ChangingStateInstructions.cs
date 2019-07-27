// <copyright file="ChangingStateInstructions.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using SmallBasic.Compiler.Scanning;

    internal sealed class PauseInstruction : BaseNonJumpInstruction
    {
        public PauseInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SmallBasicEngine engine) => engine.Pause();
    }

    internal sealed class TerminateInstruction : BaseNonJumpInstruction
    {
        public TerminateInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SmallBasicEngine engine) => engine.Terminate();
    }

    internal sealed class BlockOnStringInputInstruction : BaseNonJumpInstruction
    {
        public BlockOnStringInputInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SmallBasicEngine engine) => engine.BlockOnStringInput();
    }

    internal sealed class BlockOnNumberInputInstruction : BaseNonJumpInstruction
    {
        public BlockOnNumberInputInstruction(TextRange range)
            : base(range)
        {
        }

        protected override void Execute(SmallBasicEngine engine) => engine.BlockOnNumberInput();
    }
}
