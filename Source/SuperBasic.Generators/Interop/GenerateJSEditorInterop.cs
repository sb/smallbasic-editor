// <copyright file="GenerateJSEditorInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateJSEditorInterop : BaseConverterTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Line("namespace SuperBasic.Editor.Interop");
            this.Brace();

            this.Line("using System.Threading.Tasks;");
            this.Line("using Microsoft.AspNetCore.Blazor;");
            this.Line("using Microsoft.JSInterop;");
            this.Line("using SuperBasic.Compiler.Services;");
            this.Blank();

            this.Line("internal static class JSInterop");
            this.Brace();

            foreach (InteropType type in model)
            {
                this.Line($"public static class {type.Name}");
                this.Brace();

                foreach (Method method in type.Methods)
                {
                    string returnType = method.ReturnType.IsDefault() ? "Task" : $"Task<{method.ReturnType.ToCSharpType()}>";
                    string parameters = method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name}").Join(", ");
                    this.Line($"public static {(method.ReturnType.IsDefault() ? "async " : string.Empty)}{returnType} {method.Name}({parameters})");
                    this.Brace();

                    IEnumerable<string> arguments = new string[]
                    {
                        $@"""JSInterop.{type.Name}.{method.Name.ToLowerFirstChar()}"""
                    }.Concat(method.Parameters.Select(p => p.Name));

                    if (method.ReturnType.IsDefault())
                    {
                        this.Line($@"await JSRuntime.Current.InvokeAsync<bool>({arguments.Join(", ")}).ConfigureAwait(false);");
                    }
                    else
                    {
                        this.Line($@"return JSRuntime.Current.InvokeAsync<{method.ReturnType.ToCSharpType()}>({arguments.Join(", ")});");
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