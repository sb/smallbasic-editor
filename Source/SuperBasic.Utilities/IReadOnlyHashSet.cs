// <copyright file="IReadOnlyHashSet.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class IReadOnlyHashSet<T> : IReadOnlyCollection<T>
    {
        private readonly HashSet<T> hashSet;

        private IReadOnlyHashSet(HashSet<T> hashSet)
        {
            this.hashSet = hashSet;
        }

        int IReadOnlyCollection<T>.Count => throw new System.NotImplementedException();

        public static implicit operator IReadOnlyHashSet<T>(HashSet<T> hashSet)
        {
            return new IReadOnlyHashSet<T>(hashSet);
        }

        public bool Contains(T item) => this.hashSet.Contains(item);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.hashSet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.hashSet.GetEnumerator();
    }
}
