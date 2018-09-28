// <copyright file="GenerateCSInteropCallee.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.CSInterop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateCSInteropCallee : BaseGeneratorTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Line("namespace SuperBasic.Editor.Interop");
            this.Brace();

            this.Line("using System.Threading.Tasks;");
            this.Line("using Microsoft.JSInterop;");
            this.Blank();

            this.GenerateTypesInterfaces(model);
            this.GenerateReceivers(model);

            this.Unbrace();
        }

        private void GenerateTypesInterfaces(InteropTypeCollection model)
        {
            foreach (InteropType type in model)
            {
                this.Line($"internal interface I{type.Name}Interop");
                this.Brace();

                for (int i = 0; i < type.Methods.Count; i++)
                {
                    if (i > 0)
                    {
                        this.Blank();
                    }

                    Method method = type.Methods[i];
                    this.Line($"Task<{method.ReturnType.ToCSharpType()}> {method.Name}({method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name.ToLowerFirstChar()}").Join(", ")});");
                }

                this.Unbrace();
            }
        }

        private void GenerateReceivers(InteropTypeCollection model)
        {
            this.Line("public static class CSInterop");
            this.Brace();

            foreach (InteropType type in model)
            {
                this.Line($"private static readonly I{type.Name}Interop {type.Name} = new {type.Name}Interop();");
            }

            foreach (InteropType type in model)
            {
                foreach (Method method in type.Methods)
                {
                    this.Blank();
                    this.Line($@"[JSInvokable(""CSIntrop.{type.Name}.{method.Name}"")]");
                    this.Line($"public static async Task<{method.ReturnType.ToCSharpType()}> {type.Name}_{method.Name}({method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name.ToLowerFirstChar()}").Join(", ")})");
                    this.Indent();
                    this.Line($"=> await {type.Name}.{method.Name}({method.Parameters.Select(p => p.Name.ToLowerFirstChar()).Join(", ")});");
                    this.Unindent();
                }
            }

            this.Unbrace();
        }
    }
}
