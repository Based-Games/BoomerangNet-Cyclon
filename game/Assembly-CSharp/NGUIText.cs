using System;
using System.Text;
using UnityEngine;

// Token: 0x02000073 RID: 115
public static class NGUIText
{
	// Token: 0x06000302 RID: 770 RVA: 0x000200DC File Offset: 0x0001E2DC
	public static Color ParseColor(string text, int offset)
	{
		int num = (NGUIMath.HexToDecimal(text[offset]) << 4) | NGUIMath.HexToDecimal(text[offset + 1]);
		int num2 = (NGUIMath.HexToDecimal(text[offset + 2]) << 4) | NGUIMath.HexToDecimal(text[offset + 3]);
		int num3 = (NGUIMath.HexToDecimal(text[offset + 4]) << 4) | NGUIMath.HexToDecimal(text[offset + 5]);
		float num4 = 0.003921569f;
		return new Color(num4 * (float)num, num4 * (float)num2, num4 * (float)num3);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00020160 File Offset: 0x0001E360
	public static string EncodeColor(Color c)
	{
		int num = 16777215 & (NGUIMath.ColorToInt(c) >> 8);
		return NGUIMath.DecimalToHex(num);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00020184 File Offset: 0x0001E384
	public static int ParseSymbol(string text, int index)
	{
		int length = text.Length;
		if (index + 2 < length && text[index] == '[')
		{
			if (text[index + 1] == '-')
			{
				if (text[index + 2] == ']')
				{
					return 3;
				}
			}
			else if (index + 7 < length && text[index + 7] == ']')
			{
				Color color = NGUIText.ParseColor(text, index + 1);
				if (NGUIText.EncodeColor(color) == text.Substring(index + 1, 6).ToUpper())
				{
					return 8;
				}
			}
		}
		return 0;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0002021C File Offset: 0x0001E41C
	public static bool ParseSymbol(string text, ref int index)
	{
		int num = NGUIText.ParseSymbol(text, index);
		if (num != 0)
		{
			index += num;
			return true;
		}
		return false;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00020244 File Offset: 0x0001E444
	public static bool ParseSymbol(string text, ref int index, BetterList<Color> colors, bool premultiply)
	{
		if (colors == null)
		{
			return NGUIText.ParseSymbol(text, ref index);
		}
		int length = text.Length;
		if (index + 2 < length && text[index] == '[')
		{
			if (text[index + 1] == '-')
			{
				if (text[index + 2] == ']')
				{
					if (colors != null && colors.size > 1)
					{
						colors.RemoveAt(colors.size - 1);
					}
					index += 3;
					return true;
				}
			}
			else if (index + 7 < length && text[index + 7] == ']')
			{
				if (colors != null)
				{
					Color color = NGUIText.ParseColor(text, index + 1);
					if (NGUIText.EncodeColor(color) != text.Substring(index + 1, 6).ToUpper())
					{
						return false;
					}
					color.a = colors[colors.size - 1].a;
					if (premultiply && color.a != 1f)
					{
						color = Color.Lerp(NGUIText.mInvisible, color, color.a);
					}
					colors.Add(color);
				}
				index += 8;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00020374 File Offset: 0x0001E574
	public static string StripSymbols(string text)
	{
		if (text != null)
		{
			int i = 0;
			int num = text.Length;
			while (i < num)
			{
				char c = text[i];
				if (c == '[')
				{
					int num2 = NGUIText.ParseSymbol(text, i);
					if (num2 != 0)
					{
						text = text.Remove(i, num2);
						num = text.Length;
						continue;
					}
				}
				i++;
			}
		}
		return text;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000203D8 File Offset: 0x0001E5D8
	public static void Align(BetterList<Vector3> verts, int indexOffset, float offset)
	{
		if (NGUIText.current.alignment != TextAlignment.Left)
		{
			float finalLineWidth = NGUIText.current.finalLineWidth;
			float num;
			if (NGUIText.current.alignment == TextAlignment.Right)
			{
				num = finalLineWidth - offset;
				if (num < 0f)
				{
					num = 0f;
				}
			}
			else
			{
				num = (finalLineWidth - offset) * 0.5f;
				if (num < 0f)
				{
					num = 0f;
				}
				int num2 = Mathf.RoundToInt(finalLineWidth - offset);
				if ((num2 & 1) == 1)
				{
					num += 0.5f;
				}
				else if ((Mathf.RoundToInt(finalLineWidth) & 1) == 1)
				{
					num += 0.5f;
				}
			}
			num /= NGUIText.current.pixelDensity;
			for (int i = indexOffset; i < verts.size; i++)
			{
				verts.buffer[i] = verts.buffer[i];
				Vector3[] buffer = verts.buffer;
				int num3 = i;
				buffer[num3].x = buffer[num3].x + num;
			}
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x000204E0 File Offset: 0x0001E6E0
	public static void EndLine(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && s[num] == ' ')
		{
			s[num] = '\n';
		}
		else
		{
			s.Append('\n');
		}
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00020528 File Offset: 0x0001E728
	public static Vector2 CalculatePrintedSize(Font font, string text)
	{
		Vector2 vector = Vector2.zero;
		if (font != null && !string.IsNullOrEmpty(text))
		{
			if (NGUIText.current.encoding)
			{
				text = NGUIText.StripSymbols(text);
			}
			int finalSize = NGUIText.current.finalSize;
			font.RequestCharactersInTexture(text, finalSize, NGUIText.current.style);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = (float)finalSize + NGUIText.current.finalSpacingY;
			float finalSpacingX = NGUIText.current.finalSpacingX;
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num > num3)
					{
						num3 = num;
					}
					num = 0f;
					num2 += num4;
				}
				else if (c >= ' ')
				{
					if (font.GetCharacterInfo(c, out NGUIText.mTempChar, finalSize, NGUIText.current.style))
					{
						num += NGUIText.mTempChar.width + finalSpacingX;
					}
				}
			}
			vector.x = ((num <= num3) ? num3 : num);
			vector.y = num2 + (float)finalSize;
			vector /= NGUIText.current.pixelDensity;
		}
		return vector;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00020670 File Offset: 0x0001E870
	public static int CalculateOffsetToFit(Font font, string text)
	{
		if (font == null || string.IsNullOrEmpty(text) || NGUIText.current.lineWidth < 1)
		{
			return 0;
		}
		int finalSize = NGUIText.current.finalSize;
		font.RequestCharactersInTexture(text, finalSize, NGUIText.current.style);
		float num = NGUIText.current.finalLineWidth;
		int length = text.Length;
		int num2 = length;
		while (num2 > 0 && num > 0f)
		{
			char c = text[--num2];
			if (font.GetCharacterInfo(c, out NGUIText.mTempChar, finalSize, NGUIText.current.style))
			{
				num -= NGUIText.mTempChar.width;
			}
		}
		if (num < 0f)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000598B File Offset: 0x00003B8B
	public static void RequestCharactersInTexture(Font font, string text)
	{
		if (font != null)
		{
			font.RequestCharactersInTexture(text, NGUIText.current.finalSize, NGUIText.current.style);
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00020738 File Offset: 0x0001E938
	public static bool WrapText(Font font, string text, out string finalText)
	{
		if (NGUIText.current.lineWidth < 1 || NGUIText.current.lineHeight < 1 || string.IsNullOrEmpty(text))
		{
			finalText = string.Empty;
			return false;
		}
		int num = ((NGUIText.current.maxLines <= 0) ? 1000000 : NGUIText.current.maxLines);
		int finalSize = NGUIText.current.finalSize;
		float num2 = ((NGUIText.current.maxLines <= 0) ? NGUIText.current.finalLineHeight : Mathf.Min(NGUIText.current.finalLineHeight, (float)(finalSize * NGUIText.current.maxLines)));
		float num3 = (float)finalSize + NGUIText.current.finalSpacingY;
		num = Mathf.FloorToInt((num3 <= 0f) ? 0f : Mathf.Min((float)num, num2 / num3));
		if (num == 0)
		{
			finalText = string.Empty;
			return false;
		}
		if (font != null)
		{
			font.RequestCharactersInTexture(text, finalSize, NGUIText.current.style);
		}
		StringBuilder stringBuilder = new StringBuilder();
		int length = text.Length;
		float finalLineWidth = NGUIText.current.finalLineWidth;
		float num4 = finalLineWidth;
		float finalSpacingX = NGUIText.current.finalSpacingX;
		int num5 = 0;
		int i = 0;
		int num6 = 1;
		int num7 = 0;
		bool flag = true;
		while (i < length)
		{
			char c = text[i];
			if (c == '\n')
			{
				if (num6 == num)
				{
					break;
				}
				num4 = finalLineWidth;
				if (num5 < i)
				{
					stringBuilder.Append(text.Substring(num5, i - num5 + 1));
				}
				else
				{
					stringBuilder.Append(c);
				}
				flag = true;
				num6++;
				num5 = i + 1;
				num7 = 0;
			}
			else
			{
				if (c == ' ' && num7 != 32 && num5 < i)
				{
					stringBuilder.Append(text.Substring(num5, i - num5 + 1));
					flag = false;
					num5 = i + 1;
					num7 = (int)c;
				}
				if (NGUIText.ParseSymbol(text, ref i))
				{
					i--;
				}
				else if (font.GetCharacterInfo(c, out NGUIText.mTempChar, finalSize, NGUIText.current.style))
				{
					float num8 = finalSpacingX + NGUIText.mTempChar.width;
					num4 -= num8;
					if (num4 < 0f)
					{
						if (flag || num6 == num)
						{
							stringBuilder.Append(text.Substring(num5, Mathf.Max(0, i - num5)));
							if (num6++ == num)
							{
								num5 = i;
								break;
							}
							NGUIText.EndLine(ref stringBuilder);
							flag = true;
							if (c == ' ')
							{
								num5 = i + 1;
								num4 = finalLineWidth;
							}
							else
							{
								num5 = i;
								num4 = finalLineWidth - num8;
							}
							num7 = 0;
						}
						else
						{
							while (num5 < length && text[num5] == ' ')
							{
								num5++;
							}
							flag = true;
							num4 = finalLineWidth;
							i = num5 - 1;
							num7 = 0;
							if (num6++ == num)
							{
								break;
							}
							NGUIText.EndLine(ref stringBuilder);
						}
					}
					else
					{
						num7 = (int)c;
					}
				}
			}
			i++;
		}
		if (num5 < i)
		{
			stringBuilder.Append(text.Substring(num5, i - num5));
		}
		finalText = stringBuilder.ToString();
		return i == length || num6 <= Mathf.Min(NGUIText.current.maxLines, num);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00020AB4 File Offset: 0x0001ECB4
	public static void Print(Font font, string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (font == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int finalSize = NGUIText.current.finalSize;
		int num = verts.size;
		float num2 = (float)finalSize + NGUIText.current.finalSpacingY;
		font.RequestCharactersInTexture("j", finalSize, NGUIText.current.style);
		font.GetCharacterInfo('j', out NGUIText.mTempChar, finalSize, NGUIText.current.style);
		float num3 = NGUIText.mTempChar.vert.yMin + (float)(Mathf.RoundToInt((float)finalSize - Mathf.Abs(NGUIText.mTempChar.vert.height)) >> 1);
		font.RequestCharactersInTexture(text, finalSize, NGUIText.current.style);
		NGUIText.mColors.Add(Color.white);
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float finalSpacingX = NGUIText.current.finalSpacingX;
		float num7 = 1f / NGUIText.current.pixelDensity;
		float num8 = (float)finalSize;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Color color = NGUIText.current.tint * NGUIText.current.gradientBottom;
		Color color2 = NGUIText.current.tint * NGUIText.current.gradientTop;
		Color32 color3 = NGUIText.current.tint;
		int length = text.Length;
		for (int i = 0; i < length; i++)
		{
			char c = text[i];
			if (c == '\n')
			{
				if (num4 > num6)
				{
					num6 = num4;
				}
				if (NGUIText.current.alignment != TextAlignment.Left)
				{
					NGUIText.Align(verts, num, num4 - finalSpacingX);
					num = verts.size;
				}
				num4 = 0f;
				num5 += num2;
			}
			else if (c >= ' ')
			{
				if (NGUIText.current.encoding && NGUIText.ParseSymbol(text, ref i, NGUIText.mColors, NGUIText.current.premultiply))
				{
					Color color4 = NGUIText.current.tint * NGUIText.mColors[NGUIText.mColors.size - 1];
					color3 = color4;
					if (NGUIText.current.gradient)
					{
						color = NGUIText.current.gradientBottom * color4;
						color2 = NGUIText.current.gradientTop * color4;
					}
					i--;
				}
				else if (font.GetCharacterInfo(c, out NGUIText.mTempChar, finalSize, NGUIText.current.style))
				{
					vector.x = num4 + NGUIText.mTempChar.vert.xMin;
					vector.y = NGUIText.mTempChar.vert.yMax - num3 - num5;
					vector2.x = vector.x + NGUIText.mTempChar.vert.width;
					vector2.y = vector.y - NGUIText.mTempChar.vert.height;
					if (num7 != 1f)
					{
						vector *= num7;
						vector2 *= num7;
					}
					zero.x = NGUIText.mTempChar.uv.xMin;
					zero.y = NGUIText.mTempChar.uv.yMin;
					zero2.x = NGUIText.mTempChar.uv.xMax;
					zero2.y = NGUIText.mTempChar.uv.yMax;
					num4 += NGUIText.mTempChar.width + finalSpacingX;
					verts.Add(new Vector3(vector2.x, vector.y));
					verts.Add(new Vector3(vector.x, vector.y));
					verts.Add(new Vector3(vector.x, vector2.y));
					verts.Add(new Vector3(vector2.x, vector2.y));
					if (NGUIText.mTempChar.flipped)
					{
						uvs.Add(new Vector2(zero.x, zero2.y));
						uvs.Add(new Vector2(zero.x, zero.y));
						uvs.Add(new Vector2(zero2.x, zero.y));
						uvs.Add(new Vector2(zero2.x, zero2.y));
					}
					else
					{
						uvs.Add(new Vector2(zero2.x, zero.y));
						uvs.Add(new Vector2(zero.x, zero.y));
						uvs.Add(new Vector2(zero.x, zero2.y));
						uvs.Add(new Vector2(zero2.x, zero2.y));
					}
					if (NGUIText.current.gradient)
					{
						float num9 = num8 - (-NGUIText.mTempChar.vert.yMax + num3);
						float num10 = num9 - NGUIText.mTempChar.vert.height;
						num9 /= num8;
						num10 /= num8;
						NGUIText.s_c0 = Color.Lerp(color, color2, num9);
						NGUIText.s_c1 = Color.Lerp(color, color2, num10);
						cols.Add(NGUIText.s_c0);
						cols.Add(NGUIText.s_c0);
						cols.Add(NGUIText.s_c1);
						cols.Add(NGUIText.s_c1);
					}
					else
					{
						for (int j = 0; j < 4; j++)
						{
							cols.Add(color3);
						}
					}
				}
			}
		}
		if (NGUIText.current.alignment != TextAlignment.Left && num < verts.size)
		{
			NGUIText.Align(verts, num, num4 - finalSpacingX);
			num = verts.size;
		}
		NGUIText.mColors.Clear();
	}

	// Token: 0x040002AA RID: 682
	public static NGUIText.Settings current = new NGUIText.Settings();

	// Token: 0x040002AB RID: 683
	private static Color mInvisible = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040002AC RID: 684
	private static BetterList<Color> mColors = new BetterList<Color>();

	// Token: 0x040002AD RID: 685
	private static CharacterInfo mTempChar;

	// Token: 0x040002AE RID: 686
	private static Color32 s_c0;

	// Token: 0x040002AF RID: 687
	private static Color32 s_c1;

	// Token: 0x02000074 RID: 116
	public enum SymbolStyle
	{
		// Token: 0x040002B1 RID: 689
		None,
		// Token: 0x040002B2 RID: 690
		Uncolored,
		// Token: 0x040002B3 RID: 691
		Colored
	}

	// Token: 0x02000075 RID: 117
	public class Settings
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000310 RID: 784 RVA: 0x000059B4 File Offset: 0x00003BB4
		public int finalSize
		{
			get
			{
				return Mathf.RoundToInt((float)this.size * this.pixelDensity);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000059C9 File Offset: 0x00003BC9
		public float finalSpacingX
		{
			get
			{
				return (float)this.spacingX * this.pixelDensity;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000059D9 File Offset: 0x00003BD9
		public float finalSpacingY
		{
			get
			{
				return (float)this.spacingY * this.pixelDensity;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000059E9 File Offset: 0x00003BE9
		public float finalLineWidth
		{
			get
			{
				return (float)this.lineWidth * this.pixelDensity;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000314 RID: 788 RVA: 0x000059F9 File Offset: 0x00003BF9
		public float finalLineHeight
		{
			get
			{
				return (float)this.lineHeight * this.pixelDensity;
			}
		}

		// Token: 0x040002B4 RID: 692
		public int size = 16;

		// Token: 0x040002B5 RID: 693
		public float pixelDensity = 1f;

		// Token: 0x040002B6 RID: 694
		public FontStyle style;

		// Token: 0x040002B7 RID: 695
		public TextAlignment alignment;

		// Token: 0x040002B8 RID: 696
		public Color tint = Color.white;

		// Token: 0x040002B9 RID: 697
		public int lineWidth = 1000000;

		// Token: 0x040002BA RID: 698
		public int lineHeight = 1000000;

		// Token: 0x040002BB RID: 699
		public int maxLines;

		// Token: 0x040002BC RID: 700
		public bool gradient;

		// Token: 0x040002BD RID: 701
		public Color gradientBottom = Color.white;

		// Token: 0x040002BE RID: 702
		public Color gradientTop = Color.white;

		// Token: 0x040002BF RID: 703
		public bool encoding;

		// Token: 0x040002C0 RID: 704
		public int spacingX;

		// Token: 0x040002C1 RID: 705
		public int spacingY;

		// Token: 0x040002C2 RID: 706
		public bool premultiply;

		// Token: 0x040002C3 RID: 707
		public NGUIText.SymbolStyle symbolStyle;
	}
}
