// <copyright file="Models.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public sealed class TokenKind
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Display { get; set; }
    }

    [XmlRoot("root")]
    public sealed class TokenKindCollection : List<TokenKind>
    {
    }
}
