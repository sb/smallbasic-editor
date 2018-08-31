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
        protected override void Generate(ParsingModels.SyntaxNodeCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Parsing");
            this.Brace();

            this.Line("using System.Collections.Generic;");
            this.Line("using System.Diagnostics;");
            this.Line("using System.Linq;");
            this.Line("using SuperBasic.Compiler.Scanning;");
            this.Blank();

            foreach (var node in model)
            {
                this.ValidateNode(node);
                this.GenerateNode(node);
            }

            this.Unbrace();
        }

        private void ValidateNode(ParsingModels.SyntaxNode node)
        {
            bool foundRequired = false;

            foreach (var member in node.Members)
            {
                foundRequired |= !member.IsOptional;

                if (member.IsOptional && member.IsList)
                {
                    this.Log.LogError($"Member '{node.Name}.{member.Name}' cannot be both optional and a list.");
                }

                if ((member.Type == "Token") == member.TokenKinds is null)
                {
                    this.Log.LogError($"Member '{node.Name}.{member.Name}' of type 'Token' must specify 'TokenKinds', and vice versa.");
                }

                string requiredSuffix = string.Empty;

                if (member.Type == "Token")
                {
                    requiredSuffix += "Token";
                }

                if (member.IsOptional)
                {
                    requiredSuffix += "Opt";
                }

                if (!member.Name.EndsWith(requiredSuffix, StringComparison.CurrentCulture))
                {
                    this.Log.LogError($"Member '{node.Name}.{member.Name}' must end with '{requiredSuffix}'.");
                }
            }

            if (node.IsAbstract)
            {
                if (node.Members.Any())
                {
                    this.Log.LogError($"Abstract node '{node.Name}' should not have any members.");
                }

                if (!node.Name.StartsWith("Base", StringComparison.CurrentCulture))
                {
                    this.Log.LogError($"Abstract node '{node.Name}' name should start with 'Base'.");
                }
            }
            else if (!foundRequired)
            {
                this.Log.LogError($"Node '{node.Name}' must have at least one non-optional member.");
            }

            if (!node.Name.EndsWith("Syntax", StringComparison.CurrentCulture))
            {
                this.Log.LogError($"Node '{node.Name}' name should end with 'Syntax'.");
            }

            if (node.Name.Contains("Bound"))
            {
                this.Log.LogError($"Node '{node.Name}' name should not contain 'Bound'.");
            }
        }

        private void GenerateNode(ParsingModels.SyntaxNode node)
        {
            string getFullType(ParsingModels.Member member) => member.IsList ? $"IReadOnlyList<{member.Type}>" : member.Type;

            this.Line($"internal {(node.IsAbstract ? "abstract" : "sealed")} class {node.Name} : {node.Inherits}");
            this.Brace();

            if (!node.IsAbstract)
            {
                string parameters = node.Members.Select(member => $"{getFullType(member)} {member.Name.LowerFirstChar()}").Join(", ");
                this.Line($"public {node.Name}({parameters})");
                this.Brace();

                this.GenerateNullAsserts(node);
                this.Blank();

                foreach (var member in node.Members)
                {
                    this.Line($"this.{member.Name} = {member.Name.LowerFirstChar()};");
                }

                this.Unbrace();

                foreach (var member in node.Members)
                {
                    this.Line($"public {getFullType(member)} {member.Name} {{ get; private set; }}");
                    this.Blank();
                }

                this.Line("public override IEnumerable<BaseSyntaxNode> Children");
                this.Brace();
                this.Line("get");
                this.Brace();

                this.GenerateChildrenPropertyContents(node.Members.Where(member => member.Type != "Token"));

                this.Unbrace();
                this.Unbrace();
            }

            this.Unbrace();
        }

        private void GenerateNullAsserts(ParsingModels.SyntaxNode node)
        {
            foreach (var member in node.Members)
            {
                if (!member.IsOptional)
                {
                    this.Line($@"Debug.Assert(!ReferenceEquals({member.Name.LowerFirstChar()}, null), ""'{member.Name.LowerFirstChar()}' must not be null."");");
                }

                if (member.Type == "Token" && member.TokenKinds != "*")
                {
                    string conditions = member.TokenKinds.Split(',').Select(kind => $"{member.Name.LowerFirstChar()}.Kind == TokenKind.{kind}").Join(" || ");
                    string tokenKindAssert = $@"Debug.Assert({conditions}, ""'{member.Name.LowerFirstChar()}' must have a TokenKind of '{member.TokenKinds}'."");";

                    if (member.IsOptional)
                    {
                        this.Line($"if (!ReferenceEquals({member.Name.LowerFirstChar()}, null))");
                        this.Brace();
                        this.Line(tokenKindAssert);
                        this.Unbrace();
                    }
                    else
                    {
                        this.Line(tokenKindAssert);
                    }
                }
            }
        }

        private void GenerateChildrenPropertyContents(IEnumerable<ParsingModels.Member> members)
        {
            if (!members.Any())
            {
                this.Line("return Enumerable.Empty<BaseSyntaxNode>();");
                return;
            }

            foreach (var member in members)
            {
                if (member.IsList)
                {
                    this.Line($"foreach (var child in this.{member.Name})");
                    this.Brace();
                    this.Line("yield return child;");
                    this.Unbrace();
                }
                else if (member.IsOptional)
                {
                    this.Line($"if (!ReferenceEquals(this.{member.Name}, null))");
                    this.Brace();
                    this.Line($"yield return this.{member.Name};");
                    this.Unbrace();
                }
                else
                {
                    this.Line($"yield return this.{member.Name};");
                }
            }
        }
    }
}
