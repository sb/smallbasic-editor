// <copyright file="ScanningModels.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public static class ScanningModels
    {
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
}
