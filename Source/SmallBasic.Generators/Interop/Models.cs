// <copyright file="Models.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using SmallBasic.Utilities;

    public static class InteropTypeExtensions
    {
        public static string ToCSharpType(this string type)
        {
            switch (type)
            {
                // Native Types
                case "number": return "decimal";
                case "string": return "string";
                case "boolean": return "bool";

                // Blazor Types
                case "HTMLElement": return "ElementRef";

                // Monaco Types
                case "monaco.languages.CompletionItem": return "MonacoCompletionItem";
                case "monaco.IPosition": return "MonacoPosition";
                case "monaco.IRange": return "MonacoRange";

                case string array when array.EndsWith("[]", StringComparison.InvariantCulture):
                    return ToCSharpType(array.RemoveSuffix("[]")) + "[]";

                default: throw ExceptionUtilities.UnexpectedValue(type);
            }
        }
    }

    public sealed class Parameter
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }
    }

    public sealed class Method
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string ReturnType { get; set; }

        [XmlArray(nameof(Parameters))]
        [XmlArrayItem(typeof(Parameter))]
        public List<Parameter> Parameters { get; set; }
    }

    public sealed class InteropType
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray(nameof(Methods))]
        [XmlArrayItem(typeof(Method))]
        public List<Method> Methods { get; set; }
    }

    [XmlRoot("root")]
    public sealed class InteropTypeCollection : List<InteropType>
    {
    }
}