using Avalonia;
using Avalonia.Media;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using vaYolo.Model;

namespace vaYolo
{
    public partial class Manager
    {
        public static ISolidColorBrush[] MyBrushes = new ISolidColorBrush[] {
            Brushes.Green,
            Brushes.LightCoral,
            Brushes.LightCyan,
            Brushes.LightGreen,
            Brushes.LightPink,
            Brushes.LightSalmon,
            Brushes.LightSlateGray,
            Brushes.LightSeaGreen,
            Brushes.LightSkyBlue,
            Brushes.LightSteelBlue,
            Brushes.Yellow,
            Brushes.AliceBlue,
            Brushes.PaleGoldenrod,
            Brushes.Orchid,
            Brushes.OrangeRed,
            Brushes.Orange,
            Brushes.OliveDrab,
            Brushes.Olive,
            Brushes.OldLace,
            Brushes.Navy,
            Brushes.NavajoWhite,
            Brushes.Moccasin,
            Brushes.MistyRose,
            Brushes.MintCream,
            Brushes.MidnightBlue,
            Brushes.MediumVioletRed,
            Brushes.MediumTurquoise,
            Brushes.MediumSpringGreen,
            Brushes.MediumSlateBlue,
            Brushes.LightSkyBlue,
            Brushes.LightSlateGray,
            Brushes.LightSteelBlue,
            Brushes.LightYellow,
            Brushes.Lime,
            Brushes.LimeGreen,
            Brushes.PaleGreen,
            Brushes.Linen,
            Brushes.Maroon,
            Brushes.MediumAquamarine,
            Brushes.MediumBlue,
            Brushes.MediumOrchid,
            Brushes.MediumPurple,
            Brushes.MediumSeaGreen,
            Brushes.Magenta,
            Brushes.PaleTurquoise,
            Brushes.PaleVioletRed,
            Brushes.PapayaWhip,
            Brushes.SlateGray,
            Brushes.Snow,
            Brushes.SpringGreen,
            Brushes.SteelBlue,
            Brushes.Tan,
            Brushes.Teal,
            Brushes.SlateBlue,
            Brushes.Thistle,
            Brushes.Transparent,
            Brushes.Turquoise,
            Brushes.Violet,
            Brushes.Wheat,
            Brushes.White,
            Brushes.WhiteSmoke,
            Brushes.Tomato,
            Brushes.LightSeaGreen,
            Brushes.SkyBlue,
            Brushes.Sienna,
            Brushes.PeachPuff,
            Brushes.Peru,
            Brushes.Pink,
            Brushes.Plum,
            Brushes.PowderBlue,
            Brushes.Purple,
            Brushes.Silver,
            Brushes.RoyalBlue,
            Brushes.SaddleBrown,
            Brushes.Salmon,
            Brushes.SandyBrown,
            Brushes.SeaGreen,
            Brushes.SeaShell,
            Brushes.RosyBrown,
            Brushes.Yellow,
            Brushes.LightSalmon,
            Brushes.LightGreen,
            Brushes.DarkRed,
            Brushes.DarkOrchid,
            Brushes.DarkOrange,
            Brushes.DarkOliveGreen,
            Brushes.DarkMagenta,
            Brushes.DarkKhaki,
            Brushes.DarkGreen,
            Brushes.DarkGray,
            Brushes.DarkGoldenrod,
            Brushes.DarkCyan,
            Brushes.DarkBlue,
            Brushes.Cyan,
            Brushes.Crimson,
            Brushes.Cornsilk,
            Brushes.CornflowerBlue,
            Brushes.Coral,
            Brushes.Chocolate,
            Brushes.AntiqueWhite,
            Brushes.Aqua,
            Brushes.Aquamarine,
            Brushes.Azure,
            Brushes.Beige,
            Brushes.Bisque,
            Brushes.DarkSalmon,
            Brushes.Black,
            Brushes.Blue,
            Brushes.BlueViolet,
            Brushes.Brown,
            Brushes.BurlyWood,
            Brushes.CadetBlue,
            Brushes.Chartreuse,
            Brushes.BlanchedAlmond,
            Brushes.DarkSeaGreen,
            Brushes.DarkSlateBlue,
            Brushes.DarkSlateGray,
            Brushes.HotPink,
            Brushes.IndianRed,
            Brushes.Indigo,
            Brushes.Ivory,
            Brushes.Khaki,
            Brushes.Lavender,
            Brushes.Honeydew,
            Brushes.LavenderBlush,
            Brushes.LemonChiffon,
            Brushes.LightBlue,
            Brushes.LightCoral,
            Brushes.LightCyan,
            Brushes.LightGoldenrodYellow,
            Brushes.LightGray,
            Brushes.LawnGreen,
            Brushes.LightPink,
            Brushes.GreenYellow,
            Brushes.Gray,
            Brushes.DarkTurquoise,
            Brushes.DarkViolet,
            Brushes.DeepPink,
            Brushes.DeepSkyBlue,
            Brushes.DimGray,
            Brushes.DodgerBlue,
            Brushes.Green,
            Brushes.Firebrick,
            Brushes.ForestGreen,
            Brushes.Fuchsia,
            Brushes.Gainsboro,
            Brushes.GhostWhite,
            Brushes.Gold,
            Brushes.Goldenrod,
            Brushes.FloralWhite
            };

        public static ISolidColorBrush SelectedBrush = Brushes.YellowGreen;
        public static Pen SelectedPen = new Pen(SelectedBrush);
        public static ISolidColorBrush ErrorBrush = Brushes.Red;
        public static Pen ErrorPen = new Pen(ErrorBrush);
    }
}