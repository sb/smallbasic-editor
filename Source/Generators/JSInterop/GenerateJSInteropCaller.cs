// <copyright file="GenerateJSInteropCaller.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.JSInterop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateJSInteropCaller : BaseGeneratorTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Line("namespace SuperBasic.Editor.Interop");
            this.Brace();

            this.Line("using System.Threading.Tasks;");
            this.Line("using Microsoft.JSInterop;");
            this.Blank();

            this.Line("internal static class JSInterop");
            this.Brace();

            foreach (InteropType type in model)
            {
                this.Line($"public static class {type.Name}");
                this.Brace();

                for (int i = 0; i < type.Methods.Count; i++)
                {
                    if (i > 0)
                    {
                        this.Blank();
                    }

                    Method method = type.Methods[i];

                    string returnType = method.ReturnType == "void" ? "Task" : $"Task<{method.ReturnType.ToCSharpType()}>";
                    this.Line($"public static async {returnType} {method.Name}({method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name}").Join(", ")})");
                    this.Brace();

                    IEnumerable<string> arguments = new string[]
                    {
                        $@"""JSIntrop.{type.Name}.{method.Name.ToLowerFirstChar()}"""
                    }.Concat(method.Parameters.Select(p => p.Name));

                    if (method.ReturnType == "void")
                    {
                        this.Line($@"await JSRuntime.Current.InvokeAsync<bool>({arguments.Join(", ")});");
                    }
                    else
                    {
                        this.Line($@"return await JSRuntime.Current.InvokeAsync<{method.ReturnType.ToCSharpType()}>({arguments.Join(", ")});");
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
