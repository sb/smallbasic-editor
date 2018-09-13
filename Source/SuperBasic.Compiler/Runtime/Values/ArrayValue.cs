// <copyright file="ArrayValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    internal sealed class ArrayValue : BaseValue
    {
        public ArrayValue()
        {
            this.Contents = new Dictionary<string, BaseValue>();
        }

        public Dictionary<string, BaseValue> Contents { get; private set; }

        public override bool ToBoolean() => false;

        public override decimal ToNumber() => 0;

        public override string ToString() => $"[{this.Contents.Select(pair => $"{pair.Key}={pair.Value.ToString()}").Join(", ")}]";
    }
}
