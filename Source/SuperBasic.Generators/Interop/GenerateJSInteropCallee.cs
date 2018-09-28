// <copyright file="GenerateJSInteropCallee.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Interop
{
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateJSInteropCallee : BaseGeneratorTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            foreach (InteropType type in model)
            {
                this.Line($@"import {{ {type.Name}Interop }} from ""../JS/{type.Name}Interop"";");
            }

            this.Blank();

            foreach (InteropType type in model)
            {
                this.Line($"export interface I{type.Name}Interop {{");
                this.Indent();

                foreach (Method method in type.Methods)
                {
                    this.Line($"{method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => $"{p.Name.ToLowerFirstChar()}: {p.Type}").Join(", ")}): Promise<{method.ReturnType}>;");
                }

                this.Unbrace();
            }

            this.Line("export function initializeGlobalNamespace(): void {");
            this.Indent();

            this.Line("(<any>global).JSIntrop = {");
            this.Indent();

            for (int i = 0; i < model.Count; i++)
            {
                InteropType type = model[i];
                this.Line($"{type.Name}: <I{type.Name}Interop>new {type.Name}Interop(){(i + 1 < model.Count ? "," : string.Empty)}");
            }

            this.Unindent();
            this.Line("};");
            this.Unbrace();
        }
    }
}
