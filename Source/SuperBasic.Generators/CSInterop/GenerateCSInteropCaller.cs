// <copyright file="GenerateCSInteropCaller.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.CSInterop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateCSInteropCaller : BaseGeneratorTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Blank();
            this.Line("// Definition of this file is not published yet to somewhere we can consume. Declare for now:");
            this.Line("// https://github.com/aspnet/Blazor/issues/1452");
            this.Line("// https://github.com/aspnet/Blazor/blob/0.5.1/src/Microsoft.JSInterop/JavaScriptRuntime/src/Microsoft.JSInterop.ts");
            this.Line("export declare module DotNet {");
            this.Indent();
            this.Line("function invokeMethodAsync<T>(assemblyName: string, methodIdentifier: string, ...args: any[]): Promise<T>;");
            this.Unbrace();

            this.Line("export module CSIntrop {");
            this.Indent();

            foreach (InteropType type in model)
            {
                this.Line($"export module {type.Name} {{");
                this.Indent();

                foreach (Method method in type.Methods)
                {
                    this.Line($"export function {method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => $"{p.Name.ToLowerFirstChar()}: {p.Type}").Join(", ")}): Promise<{method.ReturnType}> {{");
                    this.Indent();

                    IEnumerable<string> arguments = new string[]
                    {
                        @"""SuperBasic.Editor""",
                        $@"""CSIntrop.{type.Name}.{method.Name}"""
                    }.Concat(method.Parameters.Select(p => p.Name.ToLowerFirstChar()));

                    this.Line($"return DotNet.invokeMethodAsync<{method.ReturnType}>({arguments.Join(", ")});");
                    this.Unbrace();
                }

                this.Unbrace();
            }

            this.Unbrace();
        }
    }
}
