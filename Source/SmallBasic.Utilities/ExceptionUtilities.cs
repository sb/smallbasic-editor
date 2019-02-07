// <copyright file="ExceptionUtilities.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Utilities
{
    using System;

    public static class ExceptionUtilities
    {
        public static InvalidOperationException UnexpectedValue<TValue>(TValue value)
        {
            return new InvalidOperationException($"Unexpected value '{value}' of type '{typeof(TValue).FullName}'");
        }
    }
}
