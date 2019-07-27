// <copyright file="GenerateJSClientInterop.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Interop
{
    using System.Collections.Generic;
    using System.Linq;
    using SmallBasic.Utilities;

    public sealed class GenerateJSClientInterop : BaseConverterTask<InteropTypeCollection>
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
                this.WriteImplementationInterface(type);
            }

            this.WriteDefinition(model);
            this.WriteInitializationMethod(model);
        }

        private void WriteImplementationInterface(InteropType type)
        {
            this.Line($"export interface I{type.Name}Interop {{");
            this.Indent();

            foreach (Method method in type.Methods)
            {
                string returnType = method.ReturnType.IsDefault() ? "void" : method.ReturnType;
                this.Line($"{method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => $"{p.Name}: {p.Type}").Join(", ")}): Promise<{returnType}>;");
            }

            this.Unbrace();
        }

        private void WriteDefinition(InteropTypeCollection model)
        {
            this.Line("declare global {");
            this.Indent();

            this.Line("export module JSInterop {");
            this.Indent();

            foreach (InteropType type in model)
            {
                this.Line($"export const {type.Name}: I{type.Name}Interop;");
            }

            this.Unbrace();
            this.Unbrace();
        }

        private void WriteInitializationMethod(InteropTypeCollection model)
        {
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
                    string parameters = method.Parameters.Select(p => $"{p.Name}: {p.Type}").Join(", ");
                    string returnType = method.ReturnType.IsDefault() ? "boolean" : method.ReturnType;

                    this.Line($"{method.Name.ToLowerFirstChar()}: async ({parameters}) : Promise<{returnType}> => {{");
                    this.Indent();

                    if (method.ReturnType.IsDefault())
                    {
                        this.Line($"await {type.Name.ToLowerFirstChar()}.{method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => p.Name).Join(", ")});");
                        this.Line("return true;");
                    }
                    else
                    {
                        this.Line($"return await {type.Name.ToLowerFirstChar()}.{method.Name.ToLowerFirstChar()}({method.Parameters.Select(p => p.Name).Join(", ")});");
                    }

                    this.Unindent();
                    this.Line($"}}{(j + 1 < type.Methods.Count ? "," : string.Empty)}");
                }

                this.Unindent();
                this.Line($"}}{(i + 1 == model.Count ? string.Empty : ",")}");
            }

            this.Unindent();
            this.Line("};");
        }
    }
}