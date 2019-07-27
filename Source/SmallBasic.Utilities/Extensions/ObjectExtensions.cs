// <copyright file="ObjectExtensions.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Utilities
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
