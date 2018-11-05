// <copyright file="GenerateEditorInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateEditorInterop : BaseConverterTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Line("namespace SuperBasic.Editor");
            this.Brace();

            this.Line("using System.Threading.Tasks;");
            this.Line("using Microsoft.AspNetCore.Blazor;");
            this.Line("using Microsoft.JSInterop;");
            this.Blank();

            this.Line("internal static class Interop");
            this.Brace();

            foreach (InteropType type in model)
            {
                this.Line($"public static class {type.Name}");
                this.Brace();

                foreach (Method method in type.Methods)
                {
                    string returnType = method.ReturnType.IsDefault() ? "Task" : $"Task<{method.ReturnType.ToCSharpType()}>";
                    string parameters = method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name}").Join(", ");
                    this.Line($"public static async {returnType} {method.Name}({parameters})");
                    this.Brace();

                    IEnumerable<string> arguments = new string[]
                    {
                        $@"""Interop.{type.Name}.{method.Name.ToLowerFirstChar()}"""
                    }.Concat(method.Parameters.Select(p => p.Name));

                    if (method.ReturnType.IsDefault())
                    {
                        this.Line($@"await JSRuntime.Current.InvokeAsync<bool>({arguments.Join(", ")});");
                    }
                    else
                    {
                        this.Line($@"return await JSRuntime.Current.InvokeAsync<{returnType}>({arguments.Join(", ")});");
                    }

                    this.Unbrace();
                }

                this.Unbrace();
            }

            this.Unbrace();
            this.Unbrace();
        }
    }
}