// <copyright file="GenerateBoundNodes.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateBoundNodes : BaseGeneratorTask<BindingModels.BoundNodeCollection>
    {
        private readonly string[] nonChildrenTypes = new[] { "string", "bool", "double", "TokenKind" };

        protected override void Generate(BindingModels.BoundNodeCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Binding");
            this.Brace();

            this.Line("using System.Collections.Generic;");
            this.Line("using System.Diagnostics;");
            this.Line("using System.Linq;");
            this.Line("using SuperBasic.Compiler.Parsing;");
            this.Line("using SuperBasic.Compiler.Scanning;");
            this.Blank();

            foreach (var node in model)
            {
                this.ValidateNode(node);
                this.GenerateNode(model, node);
            }

            this.Unbrace();
        }

        private void ValidateNode(BindingModels.BoundNode node)
        {
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
            }

            if (node.IsAbstract)
            {
                if (!node.Name.StartsWith("BaseBound", StringComparison.CurrentCulture))
                {
                    this.Log.LogError($"Abstract node '{node.Name}' name should start with 'BaseBound'.");
                }

                if (!ReferenceEquals(node.Syntax, null))
                {
                    this.Log.LogError($"Abstract node '{node.Name}' should not define 'Syntax'.");
                }
            }
            else
            {
                if (!node.Name.StartsWith("Bound", StringComparison.CurrentCulture))
                {
                    this.Log.LogError($"Node '{node.Name}' should start with 'Bound'.");
                }

                if (string.IsNullOrWhiteSpace(node.Syntax))
                {
                    this.Log.LogError($"Node '{node.Name}' should define 'Syntax'.");
                }

                if (!foundRequired)
                {
                    this.Log.LogError($"Node '{node.Name}' must have at least one non-optional member.");
                }
            }

            if (node.Name.Contains("Syntax"))
            {
                this.Log.LogError($"Node '{node.Name}' name should not contain 'Syntax'.");
            }
        }

        private void GenerateNode(BindingModels.BoundNodeCollection model, BindingModels.BoundNode node)
        {
            string getFullType(BindingModels.Member member) => member.IsList ? $"IReadOnlyList<{member.Type}>" : member.Type;

            this.Line($"internal {(node.IsAbstract ? "abstract" : "sealed")} class {node.Name} : {node.Inherits}");
            this.Brace();

            IEnumerable<BindingModels.Member> parentMembers = this.GetParentMembers(model, node.Inherits);
            IEnumerable<BindingModels.Member> allMembers = parentMembers.Concat(node.Members);

            if (allMembers.Any())
            {
                string constructorParameters = allMembers.Select(member => $"{getFullType(member)} {member.Name.LowerFirstChar()}").Join(", ");

                if (!node.IsAbstract)
                {
                    this.Line($"private {node.Syntax} syntax;");
                    this.Blank();

                    constructorParameters = $"{node.Syntax} syntax, " + constructorParameters;
                }

                this.Line($"public {node.Name}({constructorParameters})");

                if (parentMembers.Any())
                {
                    this.Indent();
                    this.Line($": base({parentMembers.Select(member => member.Name.LowerFirstChar()).Join(", ")})");
                    this.Unindent();
                }

                this.Brace();

                if (!node.IsAbstract)
                {
                    this.Line($@"Debug.Assert(!ReferenceEquals(syntax, null), ""'syntax' must not be null."");");
                }

                foreach (var member in node.Members)
                {
                    if (!member.IsOptional)
                    {
                        this.Line($@"Debug.Assert(!ReferenceEquals({member.Name.LowerFirstChar()}, null), ""'{member.Name.LowerFirstChar()}' must not be null."");");
                    }
                }

                this.Blank();

                if (!node.IsAbstract)
                {
                    this.Line("this.syntax = syntax;");
                }

                foreach (var member in node.Members)
                {
                    this.Line($"this.{member.Name} = {member.Name.LowerFirstChar()};");
                }

                this.Unbrace();

                if (!node.IsAbstract)
                {
                    this.Line("public override BaseSyntaxNode Syntax => this.syntax;");
                }

                foreach (var member in node.Members)
                {
                    this.Blank();
                    this.Line($"public {getFullType(member)} {member.Name} {{ get; private set; }}");
                }

                if (!node.IsAbstract)
                {
                    this.Blank();
                    this.GenerateChildrenPropertyContents(allMembers.Where(member => !this.nonChildrenTypes.Contains(member.Type)));
                }
            }

            this.Unbrace();
        }

        private void GenerateChildrenPropertyContents(IEnumerable<BindingModels.Member> members)
        {
            this.Line("public override IEnumerable<BaseBoundNode> Children");
            this.Brace();
            this.Line("get");
            this.Brace();

            if (!members.Any())
            {
                this.Line("return Enumerable.Empty<BaseBoundNode>();");
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

            this.Unbrace();
            this.Unbrace();
        }

        private IEnumerable<BindingModels.Member> GetParentMembers(BindingModels.BoundNodeCollection model, string parentName)
        {
            if (string.IsNullOrWhiteSpace(parentName) || parentName == "BaseBoundNode")
            {
                yield break;
            }

            BindingModels.BoundNode parent = model.SingleOrDefault(parentNode => parentNode.Name == parentName);

            if (parent is null)
            {
                this.Log.LogError($"Cannot find parent node '{parentName}'.");
                yield break;
            }

            foreach (var member in this.GetParentMembers(model, parent.Inherits))
            {
                yield return member;
            }

            foreach (var member in parent.Members)
            {
                yield return member;
            }
        }
    }
}
