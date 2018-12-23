// <copyright file = "CompletionItemProvider.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Utilities;

    internal static class CompletionItemProvider
    {
        public static MonacoCompletionItem[] Provide(Parser parser, Binder binder, TextPosition position)
        {
            if (!parser.SyntaxTree.Body.Any())
            {
                return GetItemsBeforeDot(binder, string.Empty);
            }

            // column - 1, as we want to check inside the previous node, not after it.
            position = (position.Line, position.Column - 1);
            var node = parser.SyntaxTree.FindNodeAt(position);

            switch (node)
            {
                case IdentifierExpressionSyntax identifier:
                    {
                        if (identifier.Parent is ObjectAccessExpressionSyntax objectAccess &&
                            objectAccess.BaseExpression is IdentifierExpressionSyntax library)
                        {
                            return GetItemsAfterDot(library.IdentifierToken.Text, identifier.IdentifierToken.Text);
                        }
                        else
                        {
                            return GetItemsBeforeDot(binder, identifier.IdentifierToken.Text);
                        }
                    }

                case ObjectAccessExpressionSyntax objectAccess when objectAccess.BaseExpression is IdentifierExpressionSyntax library:
                    {
                        return GetItemsAfterDot(library.IdentifierToken.Text, objectAccess.IdentifierToken.Text);
                    }

                default:
                    {
                        return Array.Empty<MonacoCompletionItem>();
                    }
            }
        }

        private static MonacoCompletionItem[] GetItemsAfterDot(string libraryName, string prefix)
        {
            var items = new List<MonacoCompletionItem>();

            if (Libraries.Types.TryGetValue(libraryName, out Library library))
            {
                foreach (var method in library.Methods.Values.Where(m => m.Name.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)))
                {
                    string arguments = method.Parameters.Values.Select((p, i) => $"${{{i + 1}:{p.Name}}}").Join(", ");
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Method, method.Name, method.Description, $"{method.Name}({arguments})"));
                }

                foreach (var property in library.Properties.Values.Where(p => p.Name.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)))
                {
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Property, property.Name, property.Description));
                }

                foreach (var @event in library.Events.Values.Where(e => e.Name.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)))
                {
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Event, @event.Name, @event.Description));
                }
            }

            return items.ToArray();
        }

        private static MonacoCompletionItem[] GetItemsBeforeDot(Binder binder, string prefix)
        {
            var items = new List<MonacoCompletionItem>();

            foreach (var variable in new VariableNamesCollector(binder).Names.Where(name => name.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)))
            {
                items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Variable, variable, variable));
            }

            foreach (var library in Libraries.Types.Values.Where(library => library.Name.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)))
            {
                items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Class, library.Name, library.Description));
            }

            void addSnippet(string name, params string[] lines)
            {
                if (name.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                {
                    items.Add(new MonacoCompletionItem(MonacoCompletionItemKind.Snippet, name, name, lines.Join(Environment.NewLine)));
                }
            }

            addSnippet("If", "If ${1:condition} Then", "EndIf");
            addSnippet("ElseIf", "ElseIf ${1:condition} Then");
            addSnippet("Else", "Else");
            addSnippet("EndIf", "EndIf");

            addSnippet("GoTo", "GoTo ${1:label}");

            addSnippet("While", "While ${1:condition}", "EndWhile");
            addSnippet("EndWhile", "EndWhile");

            addSnippet("For", "For ${1:name} = ${2:start} To ${3:end}", "EndFor");
            addSnippet("For Step", "For ${1:name} = ${2:start} To ${3:end} Step ${4:increment}", "EndFor");
            addSnippet("EndFor", "EndFor");

            addSnippet("Sub", "Sub ${1:name}", "EndSub");
            addSnippet("EndSub", "EndSub");

            return items.ToArray();
        }
    }
}
