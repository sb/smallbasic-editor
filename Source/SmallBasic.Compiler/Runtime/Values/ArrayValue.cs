// <copyright file="ArrayValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SmallBasic.Utilities;

    public sealed class ArrayValue : BaseValue, IReadOnlyDictionary<string, BaseValue>
    {
        private readonly Dictionary<string, BaseValue> contents;

        public ArrayValue()
        {
            this.contents = new Dictionary<string, BaseValue>();
        }

        public ArrayValue(IReadOnlyDictionary<string, BaseValue> contents)
        {
            this.contents = contents.ToDictionary(p => p.Key, p => p.Value);
        }

        public IEnumerable<string> Keys => this.contents.Keys;

        public IEnumerable<BaseValue> Values => this.contents.Values;

        public int Count => this.contents.Count;

        public BaseValue this[string key] => this.contents[key];

        public Dictionary<string, BaseValue> ToDictionary() => new Dictionary<string, BaseValue>(this.contents);

        public bool ContainsKey(string key) => this.contents.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, BaseValue>> GetEnumerator() => this.contents.GetEnumerator();

        public bool TryGetValue(string key, out BaseValue value) => this.contents.TryGetValue(key, out value);

        public override string ToDisplayString()
        {
            StringBuilder builder = new StringBuilder();

            void escape(string value)
            {
                foreach (char ch in value)
                {
                    switch (ch)
                    {
                        case ';':
                        case '=':
                        case '\\':
                            builder.Append("\\");
                            break;
                    }

                    builder.Append(ch);
                }
            }

            foreach (KeyValuePair<string, BaseValue> pair in this.contents)
            {
                builder.Append($"{pair.Key}=");
                escape(pair.Value.ToDisplayString());
                builder.Append(";");
            }

            return builder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.contents.GetEnumerator();

        internal override bool ToBoolean() => false;

        internal override decimal ToNumber() => 0;

        internal override ArrayValue ToArray() => this;
    }
}
