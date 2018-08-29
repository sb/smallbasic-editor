// <copyright file="GenerateSyntaxNodes.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateSyntaxNodes : BaseGeneratorTask<ParsingModels.SyntaxNodeCollection>
    {
        protected override string Convert(ParsingModels.SyntaxNodeCollection root) => $@"
namespace SuperBasic.Compiler.Parsing
{{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SuperBasic.Compiler.Scanning;
{root.Select(this.GenerateNodeType).Join(Environment.NewLine)}
}}
";

        private string GenerateNodeType(ParsingModels.SyntaxNode node)
        {
            if (!node.Name.EndsWith("Syntax", StringComparison.CurrentCulture))
            {
                this.Log.LogError($"Node '{node.Name}' should end with 'Syntax'.");
            }

            if (!node.IsAbstract && !node.Members.Any())
            {
                this.Log.LogError($"Node '{node.Name}' should should have at least one member.");
            }

            bool foundRequired = false;

            foreach (var member in node.Members)
            {
                foundRequired |= !member.IsOptional;

                if (member.IsOptional != member.Name.EndsWith("Opt", StringComparison.CurrentCulture))
                {
                    this.Log.LogError($"Member '{node.Name}.{member.Name}' must end with 'Opt' if it was optional, and vice versa.");
                }

                if (member.IsOptional && member.IsList)
                {
                    this.Log.LogError($"Member '{node.Name}.{member.Name}' cannot be both optional and a list.");
                }

                if ((member.Type == "Token") == member.TokenKinds is null)
                {
                    this.Log.LogError($"Member '{node.Name}.{member.Name}' of type 'Token' must specify 'TokenKinds', and vice versa.");
                }
            }

            if (!node.IsAbstract && !foundRequired)
            {
                this.Log.LogError($"Node '{node.Name}' must have at least one non-optional member.");
            }

            return $@"
    internal {(node.IsAbstract ? "abstract" : "sealed")} class {node.Name} : {node.Inherits}
    {{{(node.IsAbstract ? string.Empty : GenerateNodeTypeMembers(node))}
    }}";
        }

        private static string GenerateNodeTypeMembers(ParsingModels.SyntaxNode node)
        {
            string getFullType(ParsingModels.Member member) => member.IsList ? $"IReadOnlyList<{member.Type}>" : member.Type;

            return $@"
        public {node.Name}({node.Members.Select(member => $"{getFullType(member)} {member.Name.LowerFirstChar()}").Join(", ")})
        {{
{GetMembersDebugAsserts(node.Members).Join(Environment.NewLine)}

{node.Members.Select(member => $"            this.{member.Name} = {member.Name.LowerFirstChar()};").Join(Environment.NewLine)}
        }}

{node.Members.Select(member => $"        public {getFullType(member)} {member.Name} {{ get; private set; }}").Join(Environment.NewLine + Environment.NewLine)}

        public override IEnumerable<BaseSyntax> Children
        {{
            get
            {{
{GetChildrenPropertyContents(node.Members.Where(member => member.Type != "Token")).Join(Environment.NewLine)}
            }}
        }}";
        }

        private static IEnumerable<string> GetMembersDebugAsserts(IEnumerable<ParsingModels.Member> members)
        {
            foreach (var member in members)
            {
                if (!member.IsOptional)
                {
                    yield return $@"            Debug.Assert(!ReferenceEquals({member.Name.LowerFirstChar()}, null), ""'{member.Name.LowerFirstChar()}' must not be null."");";

                    if (member.Type == "Token")
                    {
                        yield return $@"            Debug.Assert({member.TokenKinds.Split(',').Select(kind => $"{member.Name.LowerFirstChar()}.Kind == TokenKind.{kind}").Join(" || ")}, ""'{member.Name.LowerFirstChar()}' must have a TokenKind of '{member.TokenKinds}'."");";
                    }
                }
            }
        }

        private static IEnumerable<string> GetChildrenPropertyContents(IEnumerable<ParsingModels.Member> members)
        {
            if (!members.Any())
            {
                yield return "                return Enumerable.Empty<BaseSyntax>();";
            }
            else
            {
                bool anythingPrinted = false;
                bool curlyBracketPrinted = false;

                foreach (var child in members)
                {
                    bool curlyBracketsNeeded = child.IsList || child.IsOptional;

                    if (curlyBracketPrinted || (anythingPrinted && curlyBracketsNeeded))
                    {
                        yield return string.Empty;
                    }

                    curlyBracketPrinted = curlyBracketsNeeded;
                    anythingPrinted = true;

                    if (child.IsList)
                    {
                        yield return $@"                foreach (var child in this.{child.Name})";
                        yield return $@"                {{";
                        yield return $@"                    yield return child;";
                        yield return $@"                }}";
                    }
                    else if (child.IsOptional)
                    {
                        yield return $@"                if (!ReferenceEquals(this.{child.Name}, null))";
                        yield return $@"                {{";
                        yield return $@"                    yield return this.{child.Name};";
                        yield return $@"                }}";
                    }
                    else
                    {
                        yield return $"                yield return this.{child.Name};";
                    }
                }
            }
        }
    }
}
