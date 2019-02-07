// <copyright file="NavigationStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Store
{
    using System;

    internal static class NavigationStore
    {
        static NavigationStore()
        {
            CurrentPage = PageId.Edit;
        }

        public static event Action PageChanged;

        internal enum PageId
        {
            Edit,
            Run,
            Debug,
        }

        public static PageId CurrentPage { get; private set; }

        public static void NagivateTo(PageId page)
        {
            CurrentPage = page;
            PageChanged();
        }
    }
}
