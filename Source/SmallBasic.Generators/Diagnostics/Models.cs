// <copyright file="Models.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Diagnostics
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public sealed class Parameter
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }
    }

    public sealed class Diagnostic
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray(nameof(Parameters))]
        [XmlArrayItem(typeof(Parameter))]
        public List<Parameter> Parameters { get; set; }
    }

    [XmlRoot("root")]
    public sealed class DiagnosticsCollection : List<Diagnostic>
    {
    }
}
