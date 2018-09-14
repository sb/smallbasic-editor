// <copyright file="BaseValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    internal abstract class BaseValue
    {
        public abstract bool ToBoolean();

        public abstract decimal ToNumber();

        public new abstract string ToString();

        public abstract ArrayValue ToArray();
    }
}
