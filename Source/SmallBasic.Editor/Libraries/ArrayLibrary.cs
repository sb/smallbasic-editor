// <copyright file="ArrayLibrary.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using SmallBasic.Compiler.Runtime;

    internal sealed class ArrayLibrary : IArrayLibrary
    {
        private readonly Dictionary<string, ArrayValue> arrays = new Dictionary<string, ArrayValue>();

        public bool ContainsIndex(ArrayValue array, string index) => array.ContainsKey(index);

        public bool ContainsValue(ArrayValue array, string value) => array.Values.Any(existing => existing.ToString() == value);

        public ArrayValue GetAllIndices(ArrayValue array)
        {
            int i = 1;
            return new ArrayValue(array.Values.ToDictionary(value => (i++).ToString(CultureInfo.CurrentCulture)));
        }

        public decimal GetItemCount(ArrayValue array) => array.Count;

        public BaseValue GetValue(string arrayName, string index)
        {
            if (this.arrays.TryGetValue(arrayName, out ArrayValue array) && array.TryGetValue(index, out BaseValue value))
            {
                return value;
            }
            else
            {
                return StringValue.Create(string.Empty);
            }
        }

        public bool IsArray(BaseValue array) => array is ArrayValue;

        public void RemoveValue(string arrayName, string index)
        {
            if (this.arrays.TryGetValue(arrayName, out ArrayValue array) && array.ContainsKey(index))
            {
                array.RemoveIndex(index);
                this.arrays[arrayName] = array;
            }
        }

        public void SetValue(string arrayName, string index, BaseValue value)
        {
            if (this.arrays.TryGetValue(arrayName, out ArrayValue array))
            {
                array.SetIndex(index, value);
                this.arrays[arrayName] = array;
            }
        }
    }
}
