// <copyright file="ArrayLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Compiler.Runtime;

    public sealed class ArrayLibrary : IArrayLibrary
    {
        public bool ContainsIndex(ArrayValue array, string index) => array.ContainsKey(index);

        public bool ContainsValue(ArrayValue array, string value) => array.Values.Any(existing => existing.ToString() == value);

        public ArrayValue GetAllIndices(ArrayValue array)
        {
            int i = 1;
            return new ArrayValue(array.Values.ToDictionary(value => (i++).ToString(CultureInfo.CurrentCulture)));
        }

        public decimal GetItemCount(ArrayValue array) => array.Count;

        public bool IsArray(BaseValue array) => array is ArrayValue;
    }
}
