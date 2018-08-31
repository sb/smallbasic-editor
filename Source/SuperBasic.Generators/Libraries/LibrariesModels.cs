// <copyright file="LibrariesModels.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public static class LibrariesModels
    {
        public sealed class Parameter
        {
            [XmlAttribute]
            public string Name { get; set; }
        }

        public sealed class Method
        {
            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute]
            public bool ReturnsValue { get; set; }

            [XmlArray(nameof(Parameters))]
            [XmlArrayItem(typeof(Parameter))]
            public List<Parameter> Parameters { get; set; }
        }

        public sealed class Property
        {
            [XmlAttribute]
            public string Name { get; set; }

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
}
