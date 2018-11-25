// <copyright file = "CompletionItemProvider.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Utilities;

    internal static class CompletionItemProvider
    {
        public static MonacoCompletionItem[] Provide(Parser parser, TextPosition position)
        {
            if (!parser.SyntaxTree.Body.Any())
            {
                return Array.Empty<MonacoCompletionItem>();
            }

            // column - 1, as we want to check inside the previous node, not after it.
            position = (position.Line, position.Column - 1);
            var node = parser.SyntaxTree.FindNodeAt(position);
            if (node.IsDefault())
            {
                return Array.Empty<MonacoCompletionItem>();
            }

            if (node is IdentifierExpressionSyntax identifier)
            {
                if (identifier.Parent is ObjectAccessExpressionSyntax objectAccess &&
                    objectAccess.BaseExpression is IdentifierExpressionSyntax library)
                {
                    return GetMembers(library.IdentifierToken.Text, identifier.IdentifierToken.Text);
                }
                else
                {
                    return GetLibraries(identifier.IdentifierToken.Text);
                }
            }
            else if (node is ObjectAccessExpressionSyntax objectAccess &&
                objectAccess.BaseExpression is IdentifierExpressionSyntax library)
            {
                return GetMembers(library.IdentifierToken.Text, objectAccess.IdentifierToken.Text);
            }

            return Array.Empty<MonacoCompletionItem>();
        }

        private static MonacoCompletionItem[] GetMembers(string libraryName, string memberPrefix)
        {
            var items = new List<MonacoCompletionItem>();

            if (Libraries.Types.TryGetValue(libraryName, out Library library))
            {
                foreach (var method in library.Methods.Values.Where(m => m.Name.StartsWith(memberPrefix, StringComparison.CurrentCultureIgnoreCase)))
                {
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Method, method.Name, method.Description));
                }

                foreach (var property in library.Properties.Values.Where(p => p.Name.StartsWith(memberPrefix, StringComparison.CurrentCultureIgnoreCase)))
                {
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Property, property.Name, property.Description));
                }

                foreach (var @event in library.Events.Values.Where(e => e.Name.StartsWith(memberPrefix, StringComparison.CurrentCultureIgnoreCase)))
                {
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Event, @event.Name, @event.Description));
                }
            }

            return items.ToArray();
        }

        private static MonacoCompletionItem[] GetLibraries(string libraryPrefix)
        {
            return Libraries.Types.Values
                .Where(library => library.Name.StartsWith(libraryPrefix, StringComparison.CurrentCultureIgnoreCase))
                .Select(library => new MonacoCompletionItem(MonacoCompletionItemKind.Class, library.Name, library.Description))
                .ToArray();
        }
    }
}
