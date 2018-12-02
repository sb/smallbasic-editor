// <copyright file="GenerateCSEditorInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateCSEditorInterop : BaseConverterTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            this.Line("namespace SuperBasic.Editor.Interop");
            this.Brace();

            this.Line("using System.Threading.Tasks;");
            this.Line("using Microsoft.JSInterop;");
            this.Line("using SuperBasic.Compiler.Services;");
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
                    string returnType = method.ReturnType.IsDefault() ? "Task" : $"Task<{method.ReturnType.ToCSharpType()}>";
                    this.Line($"{returnType} {method.Name}({method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name}").Join(", ")});");
                }

                this.Unbrace();
            }
        }

        private void GenerateReceivers(InteropTypeCollection model)
        {
            this.Line("public static class CSInterop");
            this.Brace();

            int i = 0;
            foreach (InteropType type in model)
            {
                if (i++ > 0)
                {
                    this.Blank();
                }

                this.Line($"private static readonly I{type.Name}Interop {type.Name} = new {type.Name}Interop();");
            }

            foreach (InteropType type in model)
            {
                foreach (Method method in type.Methods)
                {
                    string returnType = $"Task<{(method.ReturnType.IsDefault() ? "bool" : method.ReturnType.ToCSharpType())}>";

                    this.Blank();
                    this.Line($@"[JSInvokable(""CSIntrop.{type.Name}.{method.Name}"")]");
                    this.Line($"public static {(method.ReturnType.IsDefault() ? "async " : string.Empty)}{returnType} {type.Name}_{method.Name}({method.Parameters.Select(p => $"{p.Type.ToCSharpType()} {p.Name}").Join(", ")})");
                    this.Brace();

                    if (method.ReturnType.IsDefault())
                    {
                        this.Line($"await {type.Name}.{method.Name}({method.Parameters.Select(p => p.Name).Join(", ")}).ConfigureAwait(false);");
                        this.Line("return true;");
                    }
                    else
                    {
                        this.Line($"return {type.Name}.{method.Name}({method.Parameters.Select(p => p.Name).Join(", ")});");
                    }

                    this.Unbrace();
                }
            }

            this.Unbrace();
        }
    }
}