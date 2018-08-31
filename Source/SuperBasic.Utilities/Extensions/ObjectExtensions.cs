// <copyright file="ObjectExtensions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    // TODO write new analyzers for all disabled warnings
    // TODO write analyzer for replacing "is null" and "ReferenceEquals" with ".IsDefault"
    // TODO write analyzer to replace Assert.Anything with Should() calls
    public static class ObjectExtensions
    {
        public static bool IsDefault(this object value) => ReferenceEquals(value, default);
    }
}
