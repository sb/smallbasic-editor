// <copyright file="OperatorInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using SuperBasic.Compiler.Scanning;

    internal sealed class UnaryMinusInstruction : BaseUnaryInstruction
    {
        public UnaryMinusInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue value)
            => new NumberValue(-value.ToNumber());
    }

    internal sealed class NegateBooleanInstruction : BaseUnaryInstruction
    {
        public NegateBooleanInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue value)
            => new BooleanValue(!value.ToBoolean());
    }

    internal sealed class EqualInstruction : BaseBinaryInstruction
    {
        public EqualInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => first.ToString().Equals(second.ToString(), StringComparison.CurrentCulture) ? StringValue.True : StringValue.False;
    }

    internal sealed class LessThanInstruction : BaseBinaryInstruction
    {
        public LessThanInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => first.ToNumber() < second.ToNumber() ? StringValue.True : StringValue.False;
    }

    internal sealed class LessThanOrEqualInstruction : BaseBinaryInstruction
    {
        public LessThanOrEqualInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second)
            => first.ToNumber() <= second.ToNumber() ? StringValue.True : StringValue.False;
    }

    internal sealed class AddInstruction : BaseBinaryInstruction
    {
        public AddInstruction(TextRange range)
            : base(range)
        {
        }

        protected override BaseValue Execute(BaseValue first, BaseValue second) => new NumberValue(first.ToNumber() + second.ToNumber());
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
