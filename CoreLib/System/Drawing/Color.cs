//Copyright © 2022 Contributors of moose-org, This code is licensed under the BSD 3-Clause "New" or "Revised" License.
namespace System.Drawing
{
	public struct Color
	{
		public byte A, R, G, B;

		public static Color AliceBlue => FromArgb(240, 248, 255);
		public static Color LightSalmon => FromArgb(255, 160, 122);
		public static Color AntiqueWhite => FromArgb(250, 235, 215);
		public static Color LightSeaGreen => FromArgb(32, 178, 170);
		public static Color Aqua => FromArgb(0, 255, 255);
		public static Color LightSkyBlue => FromArgb(135, 206, 250);
		public static Color Aquamarine => FromArgb(127, 255, 212);
		public static Color LightSlateGray => FromArgb(119, 136, 153);
		public static Color Azure => FromArgb(240, 255, 255);
		public static Color LightSteelBlue => FromArgb(176, 196, 222);
		public static Color Beige => FromArgb(245, 245, 220);
		public static Color LightYellow => FromArgb(255, 255, 224);
		public static Color Bisque => FromArgb(255, 228, 196);
		public static Color Lime => FromArgb(0, 255, 0);
		public static Color Black => FromArgb(0, 0, 0);
		public static Color LimeGreen => FromArgb(50, 205, 50);
		public static Color BlanchedAlmond => FromArgb(255, 255, 205);
		public static Color Linen => FromArgb(250, 240, 230);
		public static Color Blue => FromArgb(0, 0, 255);
		public static Color Magenta => FromArgb(255, 0, 255);
		public static Color BlueViolet => FromArgb(138, 43, 226);
		public static Color Maroon => FromArgb(128, 0, 0);
		public static Color Brown => FromArgb(165, 42, 42);
		public static Color MediumAquamarine => FromArgb(102, 205, 170);
		public static Color BurlyWood => FromArgb(222, 184, 135);
		public static Color MediumBlue => FromArgb(0, 0, 205);
		public static Color CadetBlue => FromArgb(95, 158, 160);
		public static Color MediumOrchid => FromArgb(186, 85, 211);
		public static Color Chartreuse => FromArgb(127, 255, 0);
		public static Color MediumPurple => FromArgb(147, 112, 219);
		public static Color Chocolate => FromArgb(210, 105, 30);
		public static Color MediumSeaGreen => FromArgb(60, 179, 113);
		public static Color Coral => FromArgb(255, 127, 80);
		public static Color MediumSlateBlue => FromArgb(123, 104, 238);
		public static Color CornflowerBlue => FromArgb(100, 149, 237);
		public static Color MediumSpringGreen => FromArgb(0, 250, 154);
		public static Color Cornsilk => FromArgb(255, 248, 220);
		public static Color MediumTurquoise => FromArgb(72, 209, 204);
		public static Color Crimson => FromArgb(220, 20, 60);
		public static Color MediumVioletRed => FromArgb(199, 21, 112);
		public static Color Cyan => FromArgb(0, 255, 255);
		public static Color MidnightBlue => FromArgb(25, 25, 112);
		public static Color DarkBlue => FromArgb(0, 0, 139);
		public static Color MintCream => FromArgb(245, 255, 250);
		public static Color DarkCyan => FromArgb(0, 139, 139);
		public static Color MistyRose => FromArgb(255, 228, 225);
		public static Color DarkGoldenrod => FromArgb(184, 134, 11);
		public static Color Moccasin => FromArgb(255, 228, 181);
		public static Color DarkGray => FromArgb(169, 169, 169);
		public static Color NavajoWhite => FromArgb(255, 222, 173);
		public static Color DarkGreen => FromArgb(0, 100, 0);
		public static Color Navy => FromArgb(0, 0, 128);
		public static Color DarkKhaki => FromArgb(189, 183, 107);
		public static Color OldLace => FromArgb(253, 245, 230);
		public static Color DarkMagena => FromArgb(139, 0, 139);
		public static Color Olive => FromArgb(128, 128, 0);
		public static Color DarkOliveGreen => FromArgb(85, 107, 47);
		public static Color OliveDrab => FromArgb(107, 142, 45);
		public static Color DarkOrange => FromArgb(255, 140, 0);
		public static Color Orange => FromArgb(255, 165, 0);
		public static Color DarkOrchid => FromArgb(153, 50, 204);
		public static Color OrangeRed => FromArgb(255, 69, 0);
		public static Color DarkRed => FromArgb(139, 0, 0);
		public static Color Orchid => FromArgb(218, 112, 214);
		public static Color DarkSalmon => FromArgb(233, 150, 122);
		public static Color PaleGoldenrod => FromArgb(238, 232, 170);
		public static Color DarkSeaGreen => FromArgb(143, 188, 143);
		public static Color PaleGreen => FromArgb(152, 251, 152);
		public static Color DarkSlateBlue => FromArgb(72, 61, 139);
		public static Color PaleTurquoise => FromArgb(175, 238, 238);
		public static Color DarkSlateGray => FromArgb(40, 79, 79);
		public static Color PaleVioletRed => FromArgb(219, 112, 147);
		public static Color DarkTurquoise => FromArgb(0, 206, 209);
		public static Color PapayaWhip => FromArgb(255, 239, 213);
		public static Color DarkViolet => FromArgb(148, 0, 211);
		public static Color PeachPuff => FromArgb(255, 218, 155);
		public static Color DeepPink => FromArgb(255, 20, 147);
		public static Color Peru => FromArgb(205, 133, 63);
		public static Color DeepSkyBlue => FromArgb(0, 191, 255);
		public static Color Pink => FromArgb(255, 192, 203);
		public static Color DimGray => FromArgb(105, 105, 105);
		public static Color Plum => FromArgb(221, 160, 221);
		public static Color DodgerBlue => FromArgb(30, 144, 255);
		public static Color PowderBlue => FromArgb(176, 224, 230);
		public static Color Firebrick => FromArgb(178, 34, 34);
		public static Color Purple => FromArgb(128, 0, 128);
		public static Color FloralWhite => FromArgb(255, 250, 240);
		public static Color Red => FromArgb(255, 0, 0);
		public static Color ForestGreen => FromArgb(34, 139, 34);
		public static Color RosyBrown => FromArgb(188, 143, 143);
		public static Color Fuschia => FromArgb(255, 0, 255);
		public static Color RoyalBlue => FromArgb(65, 105, 225);
		public static Color Gainsboro => FromArgb(220, 220, 220);
		public static Color SaddleBrown => FromArgb(139, 69, 19);
		public static Color GhostWhite => FromArgb(248, 248, 255);
		public static Color Salmon => FromArgb(250, 128, 114);
		public static Color Gold => FromArgb(255, 215, 0);
		public static Color SandyBrown => FromArgb(244, 164, 96);
		public static Color Goldenrod => FromArgb(218, 165, 32);
		public static Color SeaGreen => FromArgb(46, 139, 87);
		public static Color Gray => FromArgb(128, 128, 128);
		public static Color Seashell => FromArgb(255, 245, 238);
		public static Color Green => FromArgb(0, 128, 0);
		public static Color Sienna => FromArgb(160, 82, 45);
		public static Color GreenYellow => FromArgb(173, 255, 47);
		public static Color Silver => FromArgb(192, 192, 192);
		public static Color Honeydew => FromArgb(240, 255, 240);
		public static Color SkyBlue => FromArgb(135, 206, 235);
		public static Color HotPink => FromArgb(255, 105, 180);
		public static Color SlateBlue => FromArgb(106, 90, 205);
		public static Color IndianRed => FromArgb(205, 92, 92);
		public static Color SlateGray => FromArgb(112, 128, 144);
		public static Color Indigo => FromArgb(75, 0, 130);
		public static Color Snow => FromArgb(255, 250, 250);
		public static Color Ivory => FromArgb(255, 240, 240);
		public static Color SpringGreen => FromArgb(0, 255, 127);
		public static Color Khaki => FromArgb(240, 230, 140);
		public static Color SteelBlue => FromArgb(70, 130, 180);
		public static Color Lavender => FromArgb(230, 230, 250);
		public static Color Tan => FromArgb(210, 180, 140);
		public static Color LavenderBlush => FromArgb(255, 240, 245);
		public static Color Teal => FromArgb(0, 128, 128);
		public static Color LawnGreen => FromArgb(124, 252, 0);
		public static Color Thistle => FromArgb(216, 191, 216);
		public static Color LemonChiffon => FromArgb(255, 250, 205);
		public static Color Tomato => FromArgb(253, 99, 71);
		public static Color LightBlue => FromArgb(173, 216, 230);
		public static Color Turquoise => FromArgb(64, 224, 208);
		public static Color LightCoral => FromArgb(240, 128, 128);
		public static Color Violet => FromArgb(238, 130, 238);
		public static Color LightCyan => FromArgb(224, 255, 255);
		public static Color Wheat => FromArgb(245, 222, 179);
		public static Color LightGoldenrodYellow => FromArgb(250, 250, 210);
		public static Color White => FromArgb(255, 255, 255);
		public static Color LightGreen => FromArgb(144, 238, 144);
		public static Color WhiteSmoke => FromArgb(245, 245, 245);
		public static Color LightGray => FromArgb(211, 211, 211);
		public static Color Yellow => FromArgb(255, 255, 0);
		public static Color LightPink => FromArgb(255, 182, 193);
		public static Color YellowGreen => FromArgb(154, 205, 50);
		public static Color DarkMagenta => FromArgb(255, 139, 0, 139);
		public static Color Fuchsia => FromArgb(255, 255, 0, 255);
		public static Color Transparent => FromArgb(0, 0, 0, 0);

		public static Color FromName(string name)
		{
			return name switch
			{
				"AliceBlue" => Color.AliceBlue,
				"AntiqueWhite" => Color.AntiqueWhite,
				"Aqua" => Color.Aqua,
				"Aquamarine" => Color.Aquamarine,
				"Azure" => Color.Azure,
				"Beige" => Color.Beige,
				"Bisque" => Color.Bisque,
				"Black" => Color.Black,
				"BlueViolet" => Color.BlueViolet,
				"Brown" => Color.Brown,
				"BurlyWood" => Color.BurlyWood,
				"CadetBlue" => Color.CadetBlue,
				"Chartreuse" => Color.Chartreuse,
				"Chocolate" => Color.Chocolate,
				"Coral" => Color.Coral,
				"CornflowerBlue" => Color.CornflowerBlue,
				"Cornsilk" => Color.Cornsilk,
				"Crimson" => Color.Crimson,
				"Cyan" => Color.Cyan,
				"DarkBlue" => Color.DarkBlue,
				"DarkCyan" => Color.DarkCyan,
				"DarkGoldenrod" => Color.DarkGoldenrod,
				"DarkGray" => Color.DarkGray,
				"DarkGreen" => Color.DarkGreen,
				"DarkKhaki" => Color.DarkKhaki,
				"DarkMagenta" => Color.DarkMagenta,
				"DarkOliveGreen" => Color.DarkOliveGreen,
				"AliceOrange" => Color.DarkOrange,
				"DarkOrchid" => Color.DarkOrchid,
				"DarkRed" => Color.DarkRed,
				"DarkSalmon" => Color.DarkSalmon,
				"DarkSeaGreen" => Color.DarkSeaGreen,
				"DarkSlateBlue" => Color.DarkSlateBlue,
				"DarkSlateGray" => Color.DarkSlateGray,
				"DarkTurquoise" => Color.DarkTurquoise,
				"DarkViolet" => Color.DarkViolet,
				"DeepPink" => Color.DeepPink,
				"DeepSkyBlue" => Color.DeepSkyBlue,
				"DimGray" => Color.DimGray,
				"DodgerBlue" => Color.DodgerBlue,
				"Firebrick" => Color.Firebrick,
				"FloralWhite" => Color.FloralWhite,
				"ForestGreen" => Color.ForestGreen,
				"Fuchsia" => Color.Fuchsia,
				"Gainsboro" => Color.Gainsboro,
				"GhostWhite" => Color.GhostWhite,
				"Gold" => Color.Gold,
				"Goldenrod" => Color.Goldenrod,
				"Gray" => Color.Gray,
				"Green" => Color.Green,
				"GreenYellow" => Color.GreenYellow,
				"Honeydew" => Color.Honeydew,
				"HotPink" => Color.HotPink,
				"IndianRed" => Color.IndianRed,
				"Indigo" => Color.Indigo,
				"Ivory" => Color.Ivory,
				"Khaki" => Color.Khaki,
				"Lavender" => Color.Lavender,
				"LavenderBlush" => Color.LavenderBlush,
				"LawnGreen" => Color.LawnGreen,
				"LemonChiffon" => Color.LemonChiffon,
				"LightBlue" => Color.LightBlue,
				"LightCoral" => Color.LightCoral,
				"LightCyan" => Color.LightCyan,
				"LightGoldenrodYellow" => Color.LightGoldenrodYellow,
				"LightGreen" => Color.LightGreen,
				"LightGray" => Color.LightGray,
				"LightPink" => Color.LightPink,
				"LightSalmon" => Color.LightSalmon,
				"LightSeaGreen" => Color.LightSeaGreen,
				"LightSkyBlue" => Color.LightSkyBlue,
				"LightSlateGray" => Color.LightSlateGray,
				"LightSteelBlue" => Color.LightSteelBlue,
				"LightYellow" => Color.LightYellow,
				"Lime" => Color.Lime,
				"LimeGreen" => Color.LimeGreen,
				"Linen" => Color.Linen,
				"Magenta" => Color.Magenta,
				"Maroon" => Color.Maroon,
				"MediumAquamarine" => Color.MediumAquamarine,
				"MediumBlue" => Color.MediumBlue,
				"MediumOrchid" => Color.MediumOrchid,
				"MediumPurple" => Color.MediumPurple,
				"MediumSeaGreen" => Color.MediumSeaGreen,
				"MediumSlateBlue" => Color.MediumSlateBlue,
				"MediumSpringGreen" => Color.MediumSpringGreen,
				"MediumTurquoise" => Color.MediumTurquoise,
				"MediumVioletRed" => Color.MediumVioletRed,
				"MidnightBlue" => Color.MidnightBlue,
				"MintCream" => Color.MintCream,
				"MistyRose" => Color.MistyRose,
				"Moccasin" => Color.Moccasin,
				"NavajoWhite" => Color.NavajoWhite,
				"Navy" => Color.Navy,
				"OldLace" => Color.OldLace,
				"Olive" => Color.Olive,
				"OliveDrab" => Color.OliveDrab,
				"Orange" => Color.Orange,
				"OrangeRed" => Color.OrangeRed,
				"Orchid" => Color.Orchid,
				"PaleGoldenrod" => Color.PaleGoldenrod,
				"PaleGreen" => Color.PaleGreen,
				"PaleTurquoise" => Color.PaleTurquoise,
				"PaleVioletRed" => Color.PaleVioletRed,
				"PapayaWhip" => Color.PapayaWhip,
				"PeachPuff" => Color.PeachPuff,
				"Peru" => Color.Peru,
				"Pink" => Color.Pink,
				"Plum" => Color.Plum,
				"PowderBlue" => Color.PowderBlue,
				"Purple" => Color.Purple,
				"Red" => Color.Red,
				"RosyBrown" => Color.RosyBrown,
				"RoyalBlue" => Color.RoyalBlue,
				"SaddleBrown" => Color.SaddleBrown,
				"Salmon" => Color.Salmon,
				"SandyBrown" => Color.SandyBrown,
				"SeaGreen" => Color.SeaGreen,
				"Sienna" => Color.Sienna,
				"Silver" => Color.Silver,
				"SkyBlue" => Color.SkyBlue,
				"SlateBlue" => Color.SlateBlue,
				"SlateGray" => Color.SlateGray,
				"Snow" => Color.Snow,
				"SpringGreen" => Color.SpringGreen,
				"SteelBlue" => Color.SteelBlue,
				"Tan" => Color.Tan,
				"Thistle" => Color.Thistle,
				"Tomato" => Color.Tomato,
				"Transparent" => Color.Transparent,
				"Turquoise" => Color.Turquoise,
				"Violet" => Color.Violet,
				"Wheat" => Color.Wheat,
				"White" => Color.White,
				"WhiteSmoke" => Color.WhiteSmoke,
				"Yellow" => Color.Yellow,
				"YellowGreen" => Color.YellowGreen,
				_ => Transparent
			};
		}

		public static bool operator ==(Color a, Color b)
		{
			return Equals(a, b);
		}

		public static bool operator !=(Color a, Color b)
		{
			return !Equals(a, b);
		}

		public override bool Equals(object o)
		{
			return base.Equals(o);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool Equals(Color a, Color b)
		{
			return
			a.A == b.A &&
			a.R == b.R &&
			a.G == b.G &&
			a.B == b.B;
		}

		public uint ToArgb()
		{
			return (uint)((A << 24) | (R << 16) | (G << 8) | B);
		}

		public static Color FromArgb(uint argb)
		{
			return new Color() { A = (byte)((argb >> 24) & 0xFF), R = (byte)((argb >> 16) & 0xFF), G = (byte)((argb >> 8) & 0xFF), B = (byte)((argb) & 0xFF) };
		}

		public static Color FromArgb(byte alpha, Color rgb)
		{
			uint argb = rgb.ToArgb();
			return new Color()
			{
				A = alpha,
				R = (byte)((argb >> 16) & 0xFF),
				G = (byte)((argb >> 8) & 0xFF),
				B = (byte)((argb) & 0xFF)
			};
		}

		public static Color FromArgb(byte red, byte green, byte blue)
		{
			return new Color() { A = 0xFF, R = red, G = green, B = blue };
		}

		public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
		{
			return new Color() { A = alpha, R = red, G = green, B = blue };
		}
	}
}
