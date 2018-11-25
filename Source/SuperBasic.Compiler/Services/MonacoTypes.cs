// <copyright file="MonacoTypes.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Services
{
    using SuperBasic.Compiler.Scanning;

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
        public MonacoCompletionItemKind kind { get; set; }

        public string label { get; set; }

        public string insertText { get; set; }

        public string detail { get; set; }
#pragma warning restore SA1300 // Element must begin with upper-case letter
    }

    public class MonacoPosition
    {
#pragma warning disable SA1300 // Element must begin with upper-case letter
        public int lineNumber { get; set; }

        public int column { get; set; }
#pragma warning restore SA1300 // Element must begin with upper-case letter

        public TextPosition ToCompilerPosition() => (this.lineNumber - 1, this.column - 1);
    }

    public class MonacoRange
    {
#pragma warning disable SA1300 // Element must begin with upper-case letter
        public int startLineNumber { get; set; }

        public int startColumn { get; set; }

        public int endLineNumber { get; set; }

        public int endColumn { get; set; }
#pragma warning restore SA1300 // Element must begin with upper-case letter

        public TextRange ToCompilerRange() => ((this.startLineNumber - 1, this.startColumn - 1), (this.endLineNumber - 1, this.endColumn - 2));
    }
}
