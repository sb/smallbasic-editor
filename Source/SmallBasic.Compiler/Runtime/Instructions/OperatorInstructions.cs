// <copyright file="OperatorInstructions.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System;
    using SmallBasic.Compiler.Scanning;

    internal sealed class UnaryMinusInstruction : BaseUnaryInstruction
    {
        public UnaryMinusInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue value)
            => new NumberValue(-value.ToNumber());
    }

    internal sealed class EqualInstruction : BaseBinaryInstruction
    {
        public EqualInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => new BooleanValue(first.ToString() == second.ToString());
    }

    internal sealed class NotEqualInstruction : BaseBinaryInstruction
    {
        public NotEqualInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => new BooleanValue(first.ToString() != second.ToString());
    }

    internal sealed class LessThanInstruction : BaseBinaryInstruction
    {
        public LessThanInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => new BooleanValue(first.ToNumber() < second.ToNumber());
    }

    internal sealed class GreaterThanInstruction : BaseBinaryInstruction
    {
        public GreaterThanInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => new BooleanValue(first.ToNumber() > second.ToNumber());
    }

    internal sealed class LessThanOrEqualInstruction : BaseBinaryInstruction
    {
        public LessThanOrEqualInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => new BooleanValue(first.ToNumber() <= second.ToNumber());
    }

    internal sealed class GreaterThanOrEqualInstruction : BaseBinaryInstruction
    {
        public GreaterThanOrEqualInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => new BooleanValue(first.ToNumber() >= second.ToNumber());
    }

    internal sealed class AddInstruction : BaseBinaryInstruction
    {
        public AddInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
        {
            if (first is NumberValue && second is NumberValue)
            {
                return new NumberValue(first.ToNumber() + second.ToNumber());
            }

            return StringValue.Create(first.ToString() + second.ToString());
        }
    }

    internal sealed class SubtractInstruction : BaseBinaryInstruction
    {
        public SubtractInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second) => new NumberValue(first.ToNumber() - second.ToNumber());
    }

    internal sealed class MultiplyInstruction : BaseBinaryInstruction
    {
        public MultiplyInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second) => new NumberValue(first.ToNumber() * second.ToNumber());
    }

    internal sealed class DivideInstruction : BaseBinaryInstruction
    {
        public DivideInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
        {
            decimal divisor = second.ToNumber();
            if (divisor == 0)
            {
                divisor = 1;
            }

            return new NumberValue(first.ToNumber() / divisor);
        }
    }
}
