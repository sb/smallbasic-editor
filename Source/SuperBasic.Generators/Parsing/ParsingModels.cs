// <copyright file="ParsingModels.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Parsing
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public static class ParsingModels
    {
        public sealed class Member
        {
            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute]
            public string Type { get; set; }

            [XmlAttribute]
            public bool IsOptional { get; set; }

            [XmlAttribute]
            public bool IsList { get; set; }

            [XmlAttribute]
            public string TokenKinds { get; set; }
        }

        public sealed class SyntaxNode
        {
            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute]
            public string Inherits { get; set; }

            [XmlAttribute]
            public bool IsAbstract { get; set; }

            [XmlArray(nameof(Members))]
            [XmlArrayItem(typeof(Member))]
            public List<Member> Members { get; set; }
        }

        [XmlRoot("root")]
        public sealed class SyntaxNodeCollection : List<SyntaxNode>
        {
        }
    }
}
