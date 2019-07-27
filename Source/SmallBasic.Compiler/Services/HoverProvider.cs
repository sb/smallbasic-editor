// <copyright file="HoverProvider.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Services
{
    using System;
    using System.Linq;
    using SmallBasic.Compiler.Binding;
    using SmallBasic.Compiler.Diagnostics;
    using SmallBasic.Compiler.Parsing;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Compiler.Scanning;
    using SmallBasic.Utilities;

    internal static class HoverProvider
    {
        public static string[] Provide(DiagnosticBag diagnostics, Parser parser, TextPosition position)
        {
            Diagnostic diagnostic = diagnostics.Contents.FirstOrDefault(d => d.Range.Contains(position));
            if (!diagnostic.IsDefault())
            {
                return new[] { diagnostic.ToDisplayString() };
            }

            if (!parser.SyntaxTree.Body.Any())
            {
                return Array.Empty<string>();
            }

            var node = parser.SyntaxTree.FindNodeAt(position);

            if (node is ObjectAccessExpressionSyntax objectAccess &&
                objectAccess.BaseExpression is IdentifierExpressionSyntax baseExpression)
            {
                if (Libraries.Types.TryGetValue(baseExpression.IdentifierToken.Text, out Library library))
                {
                    if (baseExpression.Range.Contains(position))
                    {
                        return new[] { library.Name, library.Description };
                    }
                    else if (objectAccess.IdentifierToken.Range.Contains(position))
                    {
                        string memberName = objectAccess.IdentifierToken.Text;
                        if (library.Methods.TryGetValue(memberName, out Method method))
                        {
                            return new[] { method.Name, method.Description };
                        }
                        else if (library.Properties.TryGetValue(memberName, out Property property))
                        {
                            return new[] { property.Name, property.Description };
                        }
                        else if (library.Events.TryGetValue(memberName, out Event @event))
                        {
                            return new[] { @event.Name, @event.Description };
                        }
                    }
                }
            }
            else if (node is IdentifierExpressionSyntax identifier)
            {
                if (Libraries.Types.TryGetValue(identifier.IdentifierToken.Text, out Library library))
                {
                    return new[] { library.Name, library.Description };
                }
            }

            return Array.Empty<string>();
        }
    }
}
