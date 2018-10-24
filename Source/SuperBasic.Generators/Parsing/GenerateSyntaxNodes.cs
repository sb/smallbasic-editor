// <copyright file="GenerateSyntaxNodes.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateSyntaxNodes : BaseGeneratorTask<SyntaxNodeCollection>
    {
        protected override void Generate(SyntaxNodeCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Parsing");
            this.Brace();

            this.Line("using System;");
            this.Line("using System.Collections.Generic;");
            this.Line("using System.Diagnostics;");
            this.Line("using System.Linq;");
            this.Line("using SuperBasic.Compiler.Scanning;");
            this.Line("using SuperBasic.Utilities;");
            this.Blank();

            foreach (var node in model)
            {
                this.ValidateNode(node);
                this.GenerateNode(node);
            }

            this.GenerateBaseVisitor(model);
            this.Unbrace();
        }

        private void ValidateNode(SyntaxNode node)
        {
            bool foundRequired = false;

            foreach (var member in node.Members)
            {
                foundRequired |= !member.IsOptional;

                if (member.IsOptional && member.IsList)
                {
                    this.LogError($"Member '{node.Name}.{member.Name}' cannot be both optional and a list.");
                }

                if ((member.Type == "Token") == member.TokenKinds.IsDefault())
                {
                    this.LogError($"Member '{node.Name}.{member.Name}' of type 'Token' must specify 'TokenKinds', and vice versa.");
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
                    this.LogError($"Member '{node.Name}.{member.Name}' must end with '{requiredSuffix}'.");
                }
            }

            if (node.IsAbstract)
            {
                if (node.Members.Any())
                {
                    this.LogError($"Abstract node '{node.Name}' should not have any members.");
                }

                if (!node.Name.StartsWith("Base", StringComparison.CurrentCulture))
                {
                    this.LogError($"Abstract node '{node.Name}' name should start with 'Base'.");
                }
            }
            else if (!foundRequired)
            {
                this.LogError($"Node '{node.Name}' must have at least one non-optional member.");
            }

            if (!node.Name.EndsWith("Syntax", StringComparison.CurrentCulture))
            {
                this.LogError($"Node '{node.Name}' name should end with 'Syntax'.");
            }

            if (node.Name.Contains("Bound"))
            {
                this.LogError($"Node '{node.Name}' name should not contain 'Bound'.");
            }
        }

        private void GenerateNode(SyntaxNode node)
        {
            string getFullType(Member member) => member.IsList ? $"IReadOnlyList<{member.Type}>" : member.Type;

            this.Line($"internal {(node.IsAbstract ? "abstract" : "sealed")} class {node.Name} : {node.Inherits}");
            this.Brace();

            if (!node.IsAbstract)
            {
                string parameters = node.Members.Select(member => $"{getFullType(member)} {member.Name.ToLowerFirstChar()}").Join(", ");
                this.Line($"public {node.Name}({parameters})");
                this.Brace();

                this.GenerateNullAsserts(node);
                this.Blank();

                foreach (var member in node.Members)
                {
                    this.Line($"this.{member.Name} = {member.Name.ToLowerFirstChar()};");
                }

                this.Unbrace();

                foreach (var member in node.Members)
                {
                    this.Line($"public {getFullType(member)} {member.Name} {{ get; private set; }}");
                    this.Blank();
                }

                this.GenerateChildrenProperty(node.Members.Where(member => member.Type != "Token"));
                this.GenerateRangeProperty(node.Members);
            }

            this.Unbrace();
        }

        private void GenerateNullAsserts(SyntaxNode node)
        {
            foreach (var member in node.Members)
            {
                if (!member.IsOptional)
                {
                    this.Line($@"Debug.Assert(!{member.Name.ToLowerFirstChar()}.IsDefault(), ""'{member.Name.ToLowerFirstChar()}' must not be null."");");
                }

                if (member.Type == "Token" && member.TokenKinds != "*")
                {
                    string conditions = member.TokenKinds.Split(',').Select(kind => $"{member.Name.ToLowerFirstChar()}.Kind == TokenKind.{kind}").Join(" || ");
                    string tokenKindAssert = $@"Debug.Assert({conditions}, ""'{member.Name.ToLowerFirstChar()}' must have a TokenKind of '{member.TokenKinds}'."");";

                    if (member.IsOptional)
                    {
                        this.Line($"if (!{member.Name.ToLowerFirstChar()}.IsDefault())");
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

        private void GenerateChildrenProperty(IEnumerable<Member> members)
        {
            this.Line("public override IEnumerable<BaseSyntaxNode> Children");
            this.Brace();
            this.Line("get");
            this.Brace();

            if (!members.Any())
            {
                this.Line("return Enumerable.Empty<BaseSyntaxNode>();");
            }
            else
            {
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
                        this.Line($"if (!this.{member.Name}.IsDefault())");
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

            this.Unbrace();
            this.Unbrace();
        }

        private void GenerateRangeProperty(IEnumerable<Member> members)
        {
            this.Line("public override TextRange Range");
            this.Brace();
            this.Line("get");
            this.Brace();

            this.Line("return (calculateStart(), calculateEnd());");
            this.Blank();

            generatePositionMethod("calculateStart", "Start", "FirstOrDefault", members);
            generatePositionMethod("calculateEnd", "End", "LastOrDefault", members.Reverse());

            this.Unbrace();
            this.Unbrace();

            void generatePositionMethod(string methodName, string positionProperty, string listSelector, IEnumerable<Member> ordered)
            {
                this.Line($"TextPosition {methodName}()");
                this.Brace();

                bool returnedValue = false;
                foreach (var member in ordered)
                {
                    if (member.IsList)
                    {
                        string localName = $"{member.Name.ToLowerFirstChar()}Child";
                        this.Line($"var {localName} = this.{member.Name}.{listSelector}();");
                        this.Line($"if (!{localName}.IsDefault())");
                        this.Brace();
                        this.Line($"return {localName}.Range.{positionProperty};");
                        this.Unbrace();
                    }
                    else if (member.IsOptional)
                    {
                        this.Line($"if (!this.{member.Name}.IsDefault())");
                        this.Brace();
                        this.Line($"return this.{member.Name}.Range.{positionProperty};");
                        this.Unbrace();
                    }
                    else
                    {
                        this.Line($"return this.{member.Name}.Range.{positionProperty};");
                        returnedValue = true;
                        break;
                    }
                }

                if (!returnedValue)
                {
                    this.Line(@"throw new InvalidOperationException(""Cannot calculate range for a node with no children"");");
                }

                this.Unbrace();
            }
        }

        private void GenerateBaseVisitor(SyntaxNodeCollection model)
        {
            this.Line("internal abstract class BaseSyntaxNodeVisitor");
            this.Brace();

            this.Line("public void Visit(BaseSyntaxNode node)");
            this.Brace();

            this.Line("switch (node)");
            this.Brace();

            foreach (var node in model.Where(node => !node.IsAbstract))
            {
                this.Line($"case {node.Name} {node.Name.RemoveSuffix("Syntax").ToLowerFirstChar()}:");
                this.Indent();
                this.Line($"this.Visit{node.Name.RemoveSuffix("Syntax")}({node.Name.RemoveSuffix("Syntax").ToLowerFirstChar()});");
                this.Line("break;");
                this.Unindent();
            }

            this.Line("default:");
            this.Indent();
            this.Line($"throw ExceptionUtilities.UnexpectedValue(node);");
            this.Unindent();

            this.Unbrace();
            this.Unbrace();

            foreach (var node in model.Where(node => !node.IsAbstract))
            {
                this.Line($"public virtual void Visit{node.Name.RemoveSuffix("Syntax")}({node.Name} node)");
                this.Brace();
                this.Line("this.DefaultVisit(node);");
                this.Unbrace();
            }

            this.Line("private void DefaultVisit(BaseSyntaxNode node)");
            this.Brace();
            this.Line("foreach (var child in node.Children)");
            this.Brace();
            this.Line("this.Visit(child);");
            this.Unbrace();
            this.Unbrace();

            this.Unbrace();
        }
    }
}
