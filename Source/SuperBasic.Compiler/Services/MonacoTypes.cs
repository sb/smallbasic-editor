// <copyright file="MonacoTypes.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Services
{
    public enum MonacoCompletionItemKind
    {
        Class = 4, /* monaco.lanaguages.CompletionItemKind.Variable */
        Method = 1, /* monaco.lanaguages.CompletionItemKind.Function */
        Property = 9, /* monaco.lanaguages.CompletionItemKind.Property */
        Event = 7, /* monaco.lanaguages.CompletionItemKind.Interface */
    }

    public class MonacoCompletionItem
    {
        public MonacoCompletionItem(MonacoCompletionItemKind kind, string name, string description)
        {
            this.kind = kind;
            this.label = name;
            this.insertText = name;
            this.detail = description;
        }

#pragma warning disable SA1300 // Element must begin with upper-case letter
        public MonacoCompletionItemKind kind { get; private set; }

        public string label { get; private set; }

        public string insertText { get; private set; }

        public string detail { get; private set; }
#pragma warning restore SA1300 // Element must begin with upper-case letter
    }
}
