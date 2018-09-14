// <copyright file="ArrayLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Globalization;
    using System.Linq;

    internal static partial class Libraries
    {
        private static bool Execute_Array_ContainsIndex(ArrayValue array, string index) => array.Contents.ContainsKey(index);

        private static bool Execute_Array_ContainsValue(ArrayValue array, string value) => array.Contents.Values.Any(existing => existing.ToString() == value);

        private static ArrayValue Execute_Array_GetAllIndices(ArrayValue array)
        {
            ArrayValue result = new ArrayValue();

            int i = 1;
            foreach (var pair in array.Contents)
            {
                result.Contents.Add((i++).ToString(CultureInfo.CurrentCulture), StringValue.Create(pair.Key));
            }

            return result;
        }

        private static decimal Execute_Array_GetItemCount(ArrayValue array) => array.Contents.Count;

        private static bool Execute_Array_IsArray(BaseValue array) => array is ArrayValue;
    }
}
