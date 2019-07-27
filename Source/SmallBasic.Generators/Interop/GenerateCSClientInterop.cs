// <copyright file="GenerateCSClientInterop.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Interop
{
    using System.Collections.Generic;
    using System.Linq;
    using SmallBasic.Utilities;

    public sealed class GenerateCSClientInterop : BaseConverterTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Blank();
            this.Line(@"/// <reference path=""../node_modules/@dotnet/jsinterop/dist/Microsoft.JSInterop.d.ts"" />");
            this.Blank();

            this.Line("export module CSIntrop {");
            this.Indent();

            foreach (InteropType type in model)
            {
                this.Line($"export module {type.Name} {{");
                this.Indent();

                foreach (Method method in type.Methods)
                {
                    this.Line($"export function {method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => $"{p.Name}: {p.Type}").Join(", ")}): Promise<{method.ReturnType ?? "void"}> {{");
                    this.Indent();

                    IEnumerable<string> arguments = new string[]
                    {
                        @"""SmallBasic.Editor""",
                        $@"""CSIntrop.{type.Name}.{method.Name}"""
                    }.Concat(method.Parameters.Select(p => p.Name));

                    if (method.ReturnType.IsDefault())
                    {
                        this.Line($"return DotNet.invokeMethodAsync<boolean>({arguments.Join(", ")}).then(() => {{");
                        this.Indent();
                        this.Line("Promise.resolve();");
                        this.Unindent();
                        this.Line("});");
                    }
                    else
                    {
                        this.Line($"return DotNet.invokeMethodAsync<{method.ReturnType}>({arguments.Join(", ")});");
                    }

                    this.Unbrace();
                }

                this.Unbrace();
            }

            this.Unbrace();
        }
    }
}