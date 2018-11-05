// <copyright file="ColorParser.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    public static class ColorParser
    {
        public static bool TryParseColorName(string name, out string hexResult)
        {
            switch (name)
            {
                case "aliceblue": hexResult = "#F0F8FF"; return true;
                case "antiquewhite": hexResult = "#FAEBD7"; return true;
                case "aqua": hexResult = "#00FFFF"; return true;
                case "aquamarine": hexResult = "#7FFFD4"; return true;
                case "azure": hexResult = "#F0FFFF"; return true;
                case "beige": hexResult = "#F5F5DC"; return true;
                case "bisque": hexResult = "#FFE4C4"; return true;
                case "black": hexResult = "#000000"; return true;
                case "blanchedalmond": hexResult = "#FFEBCD"; return true;
                case "blue": hexResult = "#0000FF"; return true;
                case "blueviolet": hexResult = "#8A2BE2"; return true;
                case "brown": hexResult = "#A52A2A"; return true;
                case "burlywood": hexResult = "#DEB887"; return true;
                case "cadetblue": hexResult = "#5F9EA0"; return true;
                case "chartreuse": hexResult = "#7FFF00"; return true;
                case "chocolate": hexResult = "#D2691E"; return true;
                case "coral": hexResult = "#FF7F50"; return true;
                case "cornflowerblue": hexResult = "#6495ED"; return true;
                case "cornsilk": hexResult = "#FFF8DC"; return true;
                case "crimson": hexResult = "#DC143C"; return true;
                case "cyan": hexResult = "#00FFFF"; return true;
                case "darkblue": hexResult = "#00008B"; return true;
                case "darkcyan": hexResult = "#008B8B"; return true;
                case "darkgoldenrod": hexResult = "#B8860B"; return true;
                case "darkgray": hexResult = "#A9A9A9"; return true;
                case "darkgreen": hexResult = "#006400"; return true;
                case "darkkhaki": hexResult = "#BDB76B"; return true;
                case "darkmagenta": hexResult = "#8B008B"; return true;
                case "darkolivegreen": hexResult = "#556B2F"; return true;
                case "darkorange": hexResult = "#FF8C00"; return true;
                case "darkorchid": hexResult = "#9932CC"; return true;
                case "darkred": hexResult = "#8B0000"; return true;
                case "darksalmon": hexResult = "#E9967A"; return true;
                case "darkseagreen": hexResult = "#8FBC8B"; return true;
                case "darkslateblue": hexResult = "#483D8B"; return true;
                case "darkslategray": hexResult = "#2F4F4F"; return true;
                case "darkturquoise": hexResult = "#00CED1"; return true;
                case "darkviolet": hexResult = "#9400D3"; return true;
                case "darkyellow": hexResult = "#808000"; return true;
                case "deeppink": hexResult = "#FF1493"; return true;
                case "deepskyblue": hexResult = "#00BFFF"; return true;
                case "dimgray": hexResult = "#696969"; return true;
                case "dodgerblue": hexResult = "#1E90FF"; return true;
                case "firebrick": hexResult = "#B22222"; return true;
                case "floralwhite": hexResult = "#FFFAF0"; return true;
                case "forestgreen": hexResult = "#228B22"; return true;
                case "fuchsia": hexResult = "#FF00FF"; return true;
                case "gainsboro": hexResult = "#DCDCDC"; return true;
                case "ghostwhite": hexResult = "#F8F8FF"; return true;
                case "gold": hexResult = "#FFD700"; return true;
                case "goldenrod": hexResult = "#DAA520"; return true;
                case "gray": hexResult = "#808080"; return true;
                case "green": hexResult = "#008000"; return true;
                case "greenyellow": hexResult = "#ADFF2F"; return true;
                case "honeydew": hexResult = "#F0FFF0"; return true;
                case "hotpink": hexResult = "#FF69B4"; return true;
                case "indianred": hexResult = "#CD5C5C"; return true;
                case "indigo": hexResult = "#4B0082"; return true;
                case "ivory": hexResult = "#FFFFF0"; return true;
                case "khaki": hexResult = "#F0E68C"; return true;
                case "lavender": hexResult = "#E6E6FA"; return true;
                case "lavenderblush": hexResult = "#FFF0F5"; return true;
                case "lawngreen": hexResult = "#7CFC00"; return true;
                case "lemonchiffon": hexResult = "#FFFACD"; return true;
                case "lightblue": hexResult = "#ADD8E6"; return true;
                case "lightcoral": hexResult = "#F08080"; return true;
                case "lightcyan": hexResult = "#E0FFFF"; return true;
                case "lightgoldenrodyellow": hexResult = "#FAFAD2"; return true;
                case "lightgreen": hexResult = "#90EE90"; return true;
                case "lightgray": hexResult = "#D3D3D3"; return true;
                case "lightpink": hexResult = "#FFB6C1"; return true;
                case "lightsalmon": hexResult = "#FFA07A"; return true;
                case "lightseagreen": hexResult = "#20B2AA"; return true;
                case "lightskyblue": hexResult = "#87CEFA"; return true;
                case "lightslategray": hexResult = "#778899"; return true;
                case "lightsteelblue": hexResult = "#B0C4DE"; return true;
                case "lightyellow": hexResult = "#FFFFE0"; return true;
                case "lime": hexResult = "#00FF00"; return true;
                case "limegreen": hexResult = "#32CD32"; return true;
                case "linen": hexResult = "#FAF0E6"; return true;
                case "magenta": hexResult = "#FF00FF"; return true;
                case "maroon": hexResult = "#800000"; return true;
                case "mediumaquamarine": hexResult = "#66CDAA"; return true;
                case "mediumblue": hexResult = "#0000CD"; return true;
                case "mediumorchid": hexResult = "#BA55D3"; return true;
                case "mediumpurple": hexResult = "#9370DB"; return true;
                case "mediumseagreen": hexResult = "#3CB371"; return true;
                case "mediumslateblue": hexResult = "#7B68EE"; return true;
                case "mediumspringgreen": hexResult = "#00FA9A"; return true;
                case "mediumturquoise": hexResult = "#48D1CC"; return true;
                case "mediumvioletred": hexResult = "#C71585"; return true;
                case "midnightblue": hexResult = "#191970"; return true;
                case "mintcream": hexResult = "#F5FFFA"; return true;
                case "mistyrose": hexResult = "#FFE4E1"; return true;
                case "moccasin": hexResult = "#FFE4B5"; return true;
                case "navajowhite": hexResult = "#FFDEAD"; return true;
                case "navy": hexResult = "#000080"; return true;
                case "oldlace": hexResult = "#FDF5E6"; return true;
                case "olive": hexResult = "#808000"; return true;
                case "olivedrab": hexResult = "#6B8E23"; return true;
                case "orange": hexResult = "#FFA500"; return true;
                case "orangered": hexResult = "#FF4500"; return true;
                case "orchid": hexResult = "#DA70D6"; return true;
                case "palegoldenrod": hexResult = "#EEE8AA"; return true;
                case "palegreen": hexResult = "#98FB98"; return true;
                case "paleturquoise": hexResult = "#AFEEEE"; return true;
                case "palevioletred": hexResult = "#DB7093"; return true;
                case "papayawhip": hexResult = "#FFEFD5"; return true;
                case "peachpuff": hexResult = "#FFDAB9"; return true;
                case "peru": hexResult = "#CD853F"; return true;
                case "pink": hexResult = "#FFC0CB"; return true;
                case "plum": hexResult = "#DDA0DD"; return true;
                case "powderblue": hexResult = "#B0E0E6"; return true;
                case "purple": hexResult = "#800080"; return true;
                case "red": hexResult = "#FF0000"; return true;
                case "rosybrown": hexResult = "#BC8F8F"; return true;
                case "royalblue": hexResult = "#4169E1"; return true;
                case "saddlebrown": hexResult = "#8B4513"; return true;
                case "salmon": hexResult = "#FA8072"; return true;
                case "sandybrown": hexResult = "#F4A460"; return true;
                case "seagreen": hexResult = "#2E8B57"; return true;
                case "seashell": hexResult = "#FFF5EE"; return true;
                case "sienna": hexResult = "#A0522D"; return true;
                case "silver": hexResult = "#C0C0C0"; return true;
                case "skyblue": hexResult = "#87CEEB"; return true;
                case "slateblue": hexResult = "#6A5ACD"; return true;
                case "slategray": hexResult = "#708090"; return true;
                case "snow": hexResult = "#FFFAFA"; return true;
                case "springgreen": hexResult = "#00FF7F"; return true;
                case "steelblue": hexResult = "#4682B4"; return true;
                case "tan": hexResult = "#D2B48C"; return true;
                case "teal": hexResult = "#008080"; return true;
                case "thistle": hexResult = "#D8BFD8"; return true;
                case "tomato": hexResult = "#FF6347"; return true;
                case "turquoise": hexResult = "#40E0D0"; return true;
                case "violet": hexResult = "#EE82EE"; return true;
                case "wheat": hexResult = "#F5DEB3"; return true;
                case "white": hexResult = "#FFFFFF"; return true;
                case "whitesmoke": hexResult = "#F5F5F5"; return true;
                case "yellow": hexResult = "#FFFF00"; return true;
                case "yellowgreen": hexResult = "#9ACD32"; return true;
                default: hexResult = null; return false;
            }
        }
    }
}
