// <copyright file="BooleanValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    internal sealed class BooleanValue : BaseValue
    {
        public BooleanValue(bool value)
        {
            this.Value = value;
        }

        public bool Value { get; private set; }

        public override bool ToBoolean() => this.Value;

        public override decimal ToNumber() => 0;

        public override string ToString() => this.Value ? "True" : "False";

        public override ArrayValue ToArray() => new ArrayValue();
    }
}
