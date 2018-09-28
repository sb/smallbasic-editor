// <copyright file="GenerateJSInteropCallee.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.JSInterop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateJSInteropCallee : BaseGeneratorTask<InteropTypeCollection>
    {
        protected override void Generate(InteropTypeCollection model)
        {
            foreach (InteropType type in model)
            {
                this.Line($@"import {{ {type.Name}Interop }} from ""./{type.Name}Interop"";");
            }

            this.Blank();

            foreach (InteropType type in model)
            {
                this.WriteExternalInterface(type);
            }

            foreach (InteropType type in model)
            {
                this.WriteImplementationInterface(type);
            }

            this.WriteExternalJSInterop(model);
            this.WriteInitializationMethod(model);
        }

        private void WriteImplementationInterface(InteropType type)
        {
            this.Line($"export interface I{type.Name}Interop {{");
            this.Indent();

            foreach (Method method in type.Methods)
            {
                this.Line($"{method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => $"{p.Name}: {p.Type}").Join(", ")}): Promise<{method.ReturnType}>;");
            }

            this.Unbrace();
        }

        private void WriteExternalInterface(InteropType type)
        {
            this.Line($"interface I{type.Name}External {{");
            this.Indent();

            foreach (Method method in type.Methods)
            {
                string returnType = method.ReturnType;
                if (returnType == "void")
                {
                    returnType = "boolean";
                }

                this.Line($"{method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => $"{p.Name}: {p.Type}").Join(", ")}): Promise<{returnType}>;");
            }

            this.Unbrace();
        }

        private void WriteExternalJSInterop(InteropTypeCollection model)
        {
            this.Line("declare global {");
            this.Indent();

            this.Line("export module JSInterop {");
            this.Indent();

            foreach (InteropType type in model)
            {
                this.Line($"export const {type.Name}: I{type.Name}External;");
            }

            this.Unbrace();
            this.Unbrace();
        }

        private void WriteInitializationMethod(InteropTypeCollection model)
        {
            this.Line("export function initializeGlobalNamespace(): void {");
            this.Indent();

            foreach (InteropType type in model)
            {
                this.Line($"const {type.Name.ToLowerFirstChar()}: I{type.Name}Interop = new {type.Name}Interop();");
            }

            this.Blank();
            this.Line("(<any>global).JSInterop = {");
            this.Indent();

            for (int i = 0; i < model.Count; i++)
            {
                InteropType type = model[i];
                this.Line($"{type.Name}: {{");
                this.Indent();

                for (int j = 0; j < type.Methods.Count; j++)
                {
                    Method method = type.Methods[j];
                    List<string> parts = new List<string>();
                    parts.Add($"{method.Name.ToLowerFirstChar()}: ");

                    if (method.ReturnType == "void")
                    {
                        parts.Add($"({method.Parameters.Select(p => $"{p.Name}: {p.Type}").Join(", ")}) => ");
                        parts.Add($"{type.Name.ToLowerFirstChar()}.{method.Name.ToLowerFirstChar()}");
                        parts.Add($"({method.Parameters.Select(p => p.Name).Join(", ")})");
                        parts.Add(".then(() => Promise.resolve(true))");
                    }
                    else
                    {
                        parts.Add($"{type.Name.ToLowerFirstChar()}.{method.Name.ToLowerFirstChar()}");
                    }

                    if (j + 1 < type.Methods.Count)
                    {
                        parts.Add(",");
                    }

                    this.Line(parts.Join());
                }

                this.Unindent();
                this.Line($"}}{(i + 1 == model.Count ? string.Empty : ",")}");
            }

            this.Unindent();
            this.Line("};");
            this.Unbrace();
        }
    }
}
