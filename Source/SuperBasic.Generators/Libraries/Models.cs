// <copyright file="Models.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using SuperBasic.Utilities;

    public static class LibraryTypeExtensions
    {
        public static string ToNativeType(this string type)
        {
            switch (type)
            {
                case "BaseValue": return "BaseValue";
                case "NumberValue": return "decimal";
                case "StringValue": return "string";
                case "BooleanValue": return "bool";
                case "ArrayValue": return "ArrayValue";
                default: throw ExceptionUtilities.UnexpectedValue(type);
            }
        }

        public static string ToNativeTypeConverter(this string type)
        {
            switch (type)
            {
                case "BaseValue": return string.Empty;
                case "NumberValue": return ".ToNumber()";
                case "StringValue": return ".ToString()";
                case "BooleanValue": return ".ToBoolean()";
                case "ArrayValue": return ".ToArray()";
                default: throw ExceptionUtilities.UnexpectedValue(type);
            }
        }

        public static string ToValueConstructor(this string value, string type)
        {
            switch (type)
            {
                case "BaseValue": return value;
                case "NumberValue": return $"new NumberValue({value})";
                case "StringValue": return $"StringValue.Create({value})";
                case "BooleanValue": return $"new BooleanValue({value})";
                case "ArrayValue": return value;
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
        public bool IsDeprecated { get; set; }

        [XmlAttribute]
        public string ReturnType { get; set; }

        [XmlArray(nameof(Parameters))]
        [XmlArrayItem(typeof(Parameter))]
        public List<Parameter> Parameters { get; set; }
    }

    public sealed class Property
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool IsDeprecated { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public bool HasGetter { get; set; }

        [XmlAttribute]
        public bool HasSetter { get; set; }
    }

    public sealed class Event
    {
        [XmlAttribute]
        public string Name { get; set; }
    }

    public sealed class Library
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string CompilationKind { get; set; }

        [XmlArray(nameof(Methods))]
        [XmlArrayItem(typeof(Method))]
        public List<Method> Methods { get; set; }

        [XmlArray(nameof(Properties))]
        [XmlArrayItem(typeof(Property))]
        public List<Property> Properties { get; set; }

        [XmlArray(nameof(Events))]
        [XmlArrayItem(typeof(Event))]
        public List<Event> Events { get; set; }
    }

    [XmlRoot("root")]
    public sealed class LibraryCollection : List<Library>
    {
    }
}
