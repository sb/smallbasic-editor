// <copyright file="GenerateBoundNodes.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateBoundNodes : BaseGeneratorTask<BoundNodeCollection>
    {
        private readonly string[] nonChildrenTypes = new[] { "string", "bool", "decimal", "TokenKind" };

        protected override void Generate(BoundNodeCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Binding");
            this.Brace();

            this.Line("using System.Collections.Generic;");
            this.Line("using System.Diagnostics;");
            this.Line("using System.Linq;");
            this.Line("using SuperBasic.Compiler.Parsing;");
            this.Line("using SuperBasic.Compiler.Scanning;");
            this.Line("using SuperBasic.Utilities;");
            this.Blank();

            foreach (var node in model)
            {
                this.ValidateNode(model, node);
                this.GenerateNode(model, node);
            }

            this.GenerateBaseVisitor(model);

            this.Unbrace();
        }

        private void ValidateNode(BoundNodeCollection model, BoundNode node)
        {
            bool foundRequired = this.GetParentMembers(model, node.Inherits).Any(member => !member.IsOptional);

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

                if (!node.Syntax.IsDefault())
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

        private void GenerateNode(BoundNodeCollection model, BoundNode node)
        {
            string getFullType(Member member) => member.IsList ? $"IReadOnlyList<{member.Type}>" : member.Type;

            this.Line($"internal {(node.IsAbstract ? "abstract" : "sealed")} class {node.Name} : {node.Inherits}");
            this.Brace();

            IEnumerable<Member> parentMembers = this.GetParentMembers(model, node.Inherits);
            IEnumerable<Member> allMembers = parentMembers.Concat(node.Members);

            if (allMembers.Any())
            {
                IEnumerable<string> constructorParameters = allMembers.Select(member => $"{getFullType(member)} {member.Name.ToLowerFirstChar()}");

                if (!node.IsAbstract)
                {
                    constructorParameters = constructorParameters.Prepend($"{node.Syntax} syntax");
                }

                this.Line($"public {node.Name}({constructorParameters.Join(", ")})");

                if (parentMembers.Any())
                {
                    this.Indent();
                    this.Line($": base({parentMembers.Select(member => member.Name.ToLowerFirstChar()).Join(", ")})");
                    this.Unindent();
                }

                this.Brace();

                if (!node.IsAbstract)
                {
                    this.Line($@"Debug.Assert(!syntax.IsDefault(), ""'syntax' must not be null."");");
                }

                foreach (var member in node.Members)
                {
                    if (!member.IsOptional)
                    {
                        this.Line($@"Debug.Assert(!{member.Name.ToLowerFirstChar()}.IsDefault(), ""'{member.Name.ToLowerFirstChar()}' must not be null."");");
                    }
                }

                this.Blank();

                if (!node.IsAbstract)
                {
                    this.Line("this.Syntax = syntax;");
                }

                foreach (var member in node.Members)
                {
                    this.Line($"this.{member.Name} = {member.Name.ToLowerFirstChar()};");
                }

                this.Unbrace();

                if (!node.IsAbstract)
                {
                    this.Line($"public {node.Syntax} Syntax {{ get; private set; }}");
                }

                foreach (var member in node.Members)
                {
                    this.Blank();
                    this.Line($"public {getFullType(member)} {member.Name} {{ get; private set; }}");
                }

                if (!node.IsAbstract)
                {
                    this.Blank();
                    this.GenerateChildrenProperty(allMembers.Where(member => !this.nonChildrenTypes.Contains(member.Type)));
                }
            }

            this.Unbrace();
        }

        private void GenerateChildrenProperty(IEnumerable<Member> members)
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

        private IEnumerable<Member> GetParentMembers(BoundNodeCollection model, string parentName)
        {
            if (string.IsNullOrWhiteSpace(parentName) || parentName == "BaseBoundNode")
            {
                yield break;
            }

            BoundNode parent = model.SingleOrDefault(parentNode => parentNode.Name == parentName);

            if (parent.IsDefault())
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

        private void GenerateBaseVisitor(BoundNodeCollection model)
        {
            this.Line("internal abstract class BaseBoundNodeVisitor");
            this.Brace();

            this.Line("public void Visit(BaseBoundNode node)");
            this.Brace();

            this.Line("switch (node)");
            this.Brace();

            foreach (var node in model.Where(node => !node.IsAbstract))
            {
                this.Line($"case {node.Name} {node.Name.RemovePrefix("Bound").ToLowerFirstChar()}:");
                this.Indent();
                this.Line($"this.Visit{node.Name.RemovePrefix("Bound")}({node.Name.RemovePrefix("Bound").ToLowerFirstChar()});");
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
                this.Line($"public virtual void Visit{node.Name.RemovePrefix("Bound")}({node.Name} node)");
                this.Brace();
                this.Line("this.DefaultVisit(node);");
                this.Unbrace();
            }

            this.Line("private void DefaultVisit(BaseBoundNode node)");
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
