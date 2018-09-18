// <copyright file="BaseValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    public abstract class BaseValue
    {
        internal abstract bool ToBoolean();

        internal abstract decimal ToNumber();

        internal new abstract string ToString();

        internal abstract ArrayValue ToArray();
    }
}
