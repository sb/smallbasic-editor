// <copyright file="ObjectExtensions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static class ObjectExtensions
    {
#pragma warning disable SB1002 // Use IsDefault Helper
        public static bool IsDefault(this object value) => ReferenceEquals(value, default);
#pragma warning restore SB1002 // Use IsDefault Helper
    }
}
