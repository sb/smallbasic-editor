// <copyright file="Models.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Bridge
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public sealed class Method
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string InputType { get; set; }

        [XmlAttribute]
        public string InputName { get; set; }

        [XmlAttribute]
        public string OutputType { get; set; }
    }

    public sealed class BridgeType
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray(nameof(Methods))]
        [XmlArrayItem(typeof(Method))]
        public List<Method> Methods { get; set; }
    }

    [XmlRoot("root")]
    public sealed class BridgeTypeCollection : List<BridgeType>
    {
    }
}
