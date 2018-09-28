// <copyright file="Models.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Interop
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using SuperBasic.Utilities;

    public static class InteropTypeExtensions
    {
        public static string ToCSharpType(this string type)
        {
            switch (type)
            {
                case "number": return "decimal";
                case "string": return "string";
                case "void": return "void";
                case "boolean": return "bool";
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
