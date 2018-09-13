// <copyright file="NumberValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Globalization;

    internal sealed class NumberValue : BaseValue
    {
        public NumberValue(decimal value)
        {
            this.Value = value;
        }

        public decimal Value { get; private set; }

        public override bool ToBoolean() => false;

        public override decimal ToNumber() => this.Value;

        public override string ToString() => this.Value.ToString(CultureInfo.CurrentCulture);
    }
}
