using System;
using UnityEngine;

// Token: 0x0200023E RID: 574
public static class tk2dTextGeomGen
{
	// Token: 0x0600104F RID: 4175 RVA: 0x0000DE17 File Offset: 0x0000C017
	public static tk2dTextGeomGen.GeomData Data(tk2dTextMeshData textMeshData, tk2dFontData fontData, string formattedText)
	{
		tk2dTextGeomGen.tmpData.textMeshData = textMeshData;
		tk2dTextGeomGen.tmpData.fontInst = fontData;
		tk2dTextGeomGen.tmpData.formattedText = formattedText;
		return tk2dTextGeomGen.tmpData;
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x00074B2C File Offset: 0x00072D2C
	public static Vector2 GetMeshDimensionsForString(string str, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = false;
		int num4 = 0;
		int num5 = 0;
		while (num5 < str.Length && num4 < textMeshData.maxChars)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				int num6 = (int)str[num5];
				if (num6 == 10)
				{
					num = Mathf.Max(num2, num);
					num2 = 0f;
					num3 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
				}
				else
				{
					if (textMeshData.inlineStyling && num6 == 94 && num5 + 1 < str.Length)
					{
						if (str[num5 + 1] != '^')
						{
							int num7 = 0;
							char c = str[num5 + 1];
							if (c != 'C')
							{
								if (c != 'G')
								{
									if (c != 'c')
									{
										if (c == 'g')
										{
											num7 = 9;
										}
									}
									else
									{
										num7 = 5;
									}
								}
								else
								{
									num7 = 17;
								}
							}
							else
							{
								num7 = 9;
							}
							num5 += num7;
							goto IL_233;
						}
						flag = true;
					}
					bool flag2 = num6 == 94;
					tk2dFontChar tk2dFontChar;
					if (fontInst.useDictionary)
					{
						if (!fontInst.charDict.ContainsKey(num6))
						{
							num6 = 0;
						}
						tk2dFontChar = fontInst.charDict[num6];
					}
					else
					{
						if (num6 >= fontInst.chars.Length)
						{
							num6 = 0;
						}
						tk2dFontChar = fontInst.chars[num6];
					}
					if (flag2)
					{
					}
					num2 += (tk2dFontChar.advance + textMeshData.spacing) * textMeshData.scale.x;
					if (textMeshData.kerning && num5 < str.Length - 1)
					{
						foreach (tk2dFontKerning tk2dFontKerning in fontInst.kerning)
						{
							if (tk2dFontKerning.c0 == (int)str[num5] && tk2dFontKerning.c1 == (int)str[num5 + 1])
							{
								num2 += tk2dFontKerning.amount * textMeshData.scale.x;
								break;
							}
						}
					}
					num4++;
				}
			}
			IL_233:
			num5++;
		}
		num = Mathf.Max(num2, num);
		num3 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
		return new Vector2(num, num3);
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x00074DBC File Offset: 0x00072FBC
	public static float GetYAnchorForHeight(float textHeight, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		int num = (int)(textMeshData.anchor / TextAnchor.MiddleLeft);
		float num2 = (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
		switch (num)
		{
		case 0:
			return -num2;
		case 1:
		{
			float num3 = -textHeight / 2f - num2;
			if (fontInst.version >= 2)
			{
				float num4 = fontInst.texelSize.y * textMeshData.scale.y;
				return Mathf.Floor(num3 / num4) * num4;
			}
			return num3;
		}
		case 2:
			return -textHeight - num2;
		default:
			return -num2;
		}
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x00074E64 File Offset: 0x00073064
	public static float GetXAnchorForWidth(float lineWidth, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		switch (textMeshData.anchor % TextAnchor.MiddleLeft)
		{
		case TextAnchor.UpperLeft:
			return 0f;
		case TextAnchor.UpperCenter:
		{
			float num = -lineWidth / 2f;
			if (fontInst.version >= 2)
			{
				float num2 = fontInst.texelSize.x * textMeshData.scale.x;
				return Mathf.Floor(num / num2) * num2;
			}
			return num;
		}
		case TextAnchor.UpperRight:
			return -lineWidth;
		default:
			return 0f;
		}
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x00074EF0 File Offset: 0x000730F0
	private static void PostAlignTextData(Vector3[] pos, int offset, int targetStart, int targetEnd, float offsetX)
	{
		for (int i = targetStart * 4; i < targetEnd * 4; i++)
		{
			Vector3 vector = pos[offset + i];
			vector.x += offsetX;
			pos[offset + i] = vector;
		}
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x00074F44 File Offset: 0x00073144
	private static int GetFullHexColorComponent(int c1, int c2)
	{
		int num = 0;
		if (c1 >= 48 && c1 <= 57)
		{
			num += (c1 - 48) * 16;
		}
		else if (c1 >= 97 && c1 <= 102)
		{
			num += (10 + c1 - 97) * 16;
		}
		else
		{
			if (c1 < 65 || c1 > 70)
			{
				return -1;
			}
			num += (10 + c1 - 65) * 16;
		}
		if (c2 >= 48 && c2 <= 57)
		{
			num += c2 - 48;
		}
		else if (c2 >= 97 && c2 <= 102)
		{
			num += 10 + c2 - 97;
		}
		else
		{
			if (c2 < 65 || c2 > 70)
			{
				return -1;
			}
			num += 10 + c2 - 65;
		}
		return num;
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x00075018 File Offset: 0x00073218
	private static int GetCompactHexColorComponent(int c)
	{
		if (c >= 48 && c <= 57)
		{
			return (c - 48) * 17;
		}
		if (c >= 97 && c <= 102)
		{
			return (10 + c - 97) * 17;
		}
		if (c >= 65 && c <= 70)
		{
			return (10 + c - 65) * 17;
		}
		return -1;
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x00075074 File Offset: 0x00073274
	private static int GetStyleHexColor(string str, bool fullHex, ref Color32 color)
	{
		int num;
		int num2;
		int num3;
		int num4;
		if (fullHex)
		{
			if (str.Length < 8)
			{
				return 1;
			}
			num = tk2dTextGeomGen.GetFullHexColorComponent((int)str[0], (int)str[1]);
			num2 = tk2dTextGeomGen.GetFullHexColorComponent((int)str[2], (int)str[3]);
			num3 = tk2dTextGeomGen.GetFullHexColorComponent((int)str[4], (int)str[5]);
			num4 = tk2dTextGeomGen.GetFullHexColorComponent((int)str[6], (int)str[7]);
		}
		else
		{
			if (str.Length < 4)
			{
				return 1;
			}
			num = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[0]);
			num2 = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[1]);
			num3 = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[2]);
			num4 = tk2dTextGeomGen.GetCompactHexColorComponent((int)str[3]);
		}
		if (num == -1 || num2 == -1 || num3 == -1 || num4 == -1)
		{
			return 1;
		}
		color = new Color32((byte)num, (byte)num2, (byte)num3, (byte)num4);
		return 0;
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x0007515C File Offset: 0x0007335C
	private static int SetColorsFromStyleCommand(string args, bool twoColors, bool fullHex)
	{
		int num = ((!twoColors) ? 1 : 2) * ((!fullHex) ? 4 : 8);
		bool flag = false;
		if (args.Length >= num)
		{
			if (tk2dTextGeomGen.GetStyleHexColor(args, fullHex, ref tk2dTextGeomGen.meshTopColor) != 0)
			{
				flag = true;
			}
			if (twoColors)
			{
				string text = args.Substring((!fullHex) ? 4 : 8);
				if (tk2dTextGeomGen.GetStyleHexColor(text, fullHex, ref tk2dTextGeomGen.meshBottomColor) != 0)
				{
					flag = true;
				}
			}
			else
			{
				tk2dTextGeomGen.meshBottomColor = tk2dTextGeomGen.meshTopColor;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			tk2dTextGeomGen.meshTopColor = (tk2dTextGeomGen.meshBottomColor = tk2dTextGeomGen.errorColor);
		}
		return num;
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0000DE3F File Offset: 0x0000C03F
	private static void SetGradientTexUFromStyleCommand(int arg)
	{
		tk2dTextGeomGen.meshGradientTexU = (float)(arg - 48) / (float)((tk2dTextGeomGen.curGradientCount <= 0) ? 1 : tk2dTextGeomGen.curGradientCount);
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x00075200 File Offset: 0x00073400
	private static int HandleStyleCommand(string cmd)
	{
		if (cmd.Length == 0)
		{
			return 0;
		}
		int num = (int)cmd[0];
		string text = cmd.Substring(1);
		int num2 = 0;
		int num3 = num;
		if (num3 != 67)
		{
			if (num3 != 71)
			{
				if (num3 != 99)
				{
					if (num3 == 103)
					{
						num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, true, false);
					}
				}
				else
				{
					num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, false, false);
				}
			}
			else
			{
				num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, true, true);
			}
		}
		else
		{
			num2 = 1 + tk2dTextGeomGen.SetColorsFromStyleCommand(text, false, true);
		}
		if (num >= 48 && num <= 57)
		{
			tk2dTextGeomGen.SetGradientTexUFromStyleCommand(num);
			num2 = 1;
		}
		return num2;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x000752AC File Offset: 0x000734AC
	public static void GetTextMeshGeomDesc(out int numVertices, out int numIndices, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		numVertices = textMeshData.maxChars * 4;
		numIndices = textMeshData.maxChars * 6;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x000752D4 File Offset: 0x000734D4
	public static int SetTextMeshGeom(Vector3[] pos, Vector2[] uv, Vector2[] uv2, Color32[] color, int offset, tk2dTextGeomGen.GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		string formattedText = geomData.formattedText;
		tk2dTextGeomGen.meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		tk2dTextGeomGen.meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		tk2dTextGeomGen.meshGradientTexU = (float)textMeshData.textureGradient / (float)((fontInst.gradientCount <= 0) ? 1 : fontInst.gradientCount);
		tk2dTextGeomGen.curGradientCount = fontInst.gradientCount;
		float yanchorForHeight = tk2dTextGeomGen.GetYAnchorForHeight(tk2dTextGeomGen.GetMeshDimensionsForString(geomData.formattedText, geomData).y, geomData);
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		while (num5 < formattedText.Length && num3 < textMeshData.maxChars)
		{
			int num6 = (int)formattedText[num5];
			bool flag = num6 == 94;
			tk2dFontChar tk2dFontChar;
			if (fontInst.useDictionary)
			{
				if (!fontInst.charDict.ContainsKey(num6))
				{
					num6 = 0;
				}
				tk2dFontChar = fontInst.charDict[num6];
			}
			else
			{
				if (num6 >= fontInst.chars.Length)
				{
					num6 = 0;
				}
				tk2dFontChar = fontInst.chars[num6];
			}
			if (flag)
			{
				num6 = 94;
			}
			if (num6 == 10)
			{
				float num7 = num;
				int num8 = num3;
				if (num4 != num3)
				{
					float xanchorForWidth = tk2dTextGeomGen.GetXAnchorForWidth(num7, geomData);
					tk2dTextGeomGen.PostAlignTextData(pos, offset, num4, num8, xanchorForWidth);
				}
				num4 = num3;
				num = 0f;
				num2 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
			}
			else
			{
				if (textMeshData.inlineStyling && num6 == 94)
				{
					if (num5 + 1 >= formattedText.Length || formattedText[num5 + 1] != '^')
					{
						num5 += tk2dTextGeomGen.HandleStyleCommand(formattedText.Substring(num5 + 1));
						goto IL_755;
					}
					num5++;
				}
				pos[offset + num3 * 4] = new Vector3(num + tk2dFontChar.p0.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p0.y * textMeshData.scale.y, 0f);
				pos[offset + num3 * 4 + 1] = new Vector3(num + tk2dFontChar.p1.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p0.y * textMeshData.scale.y, 0f);
				pos[offset + num3 * 4 + 2] = new Vector3(num + tk2dFontChar.p0.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p1.y * textMeshData.scale.y, 0f);
				pos[offset + num3 * 4 + 3] = new Vector3(num + tk2dFontChar.p1.x * textMeshData.scale.x, yanchorForHeight + num2 + tk2dFontChar.p1.y * textMeshData.scale.y, 0f);
				if (tk2dFontChar.flipped)
				{
					uv[offset + num3 * 4] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv0.y);
				}
				else
				{
					uv[offset + num3 * 4] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv0.y);
					uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar.uv0.x, tk2dFontChar.uv1.y);
					uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar.uv1.x, tk2dFontChar.uv1.y);
				}
				if (fontInst.textureGradients)
				{
					uv2[offset + num3 * 4] = tk2dFontChar.gradientUv[0] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 1] = tk2dFontChar.gradientUv[1] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 2] = tk2dFontChar.gradientUv[2] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
					uv2[offset + num3 * 4 + 3] = tk2dFontChar.gradientUv[3] + new Vector2(tk2dTextGeomGen.meshGradientTexU, 0f);
				}
				if (fontInst.isPacked)
				{
					Color32 color2 = tk2dTextGeomGen.channelSelectColors[tk2dFontChar.channel];
					color[offset + num3 * 4] = color2;
					color[offset + num3 * 4 + 1] = color2;
					color[offset + num3 * 4 + 2] = color2;
					color[offset + num3 * 4 + 3] = color2;
				}
				else
				{
					color[offset + num3 * 4] = tk2dTextGeomGen.meshTopColor;
					color[offset + num3 * 4 + 1] = tk2dTextGeomGen.meshTopColor;
					color[offset + num3 * 4 + 2] = tk2dTextGeomGen.meshBottomColor;
					color[offset + num3 * 4 + 3] = tk2dTextGeomGen.meshBottomColor;
				}
				num += (tk2dFontChar.advance + textMeshData.spacing) * textMeshData.scale.x;
				if (textMeshData.kerning && num5 < formattedText.Length - 1)
				{
					foreach (tk2dFontKerning tk2dFontKerning in fontInst.kerning)
					{
						if (tk2dFontKerning.c0 == (int)formattedText[num5] && tk2dFontKerning.c1 == (int)formattedText[num5 + 1])
						{
							num += tk2dFontKerning.amount * textMeshData.scale.x;
							break;
						}
					}
				}
				num3++;
			}
			IL_755:
			num5++;
		}
		if (num4 != num3)
		{
			float num9 = num;
			int num10 = num3;
			float xanchorForWidth2 = tk2dTextGeomGen.GetXAnchorForWidth(num9, geomData);
			tk2dTextGeomGen.PostAlignTextData(pos, offset, num4, num10, xanchorForWidth2);
		}
		for (int j = num3; j < textMeshData.maxChars; j++)
		{
			pos[offset + j * 4] = (pos[offset + j * 4 + 1] = (pos[offset + j * 4 + 2] = (pos[offset + j * 4 + 3] = Vector3.zero)));
			uv[offset + j * 4] = (uv[offset + j * 4 + 1] = (uv[offset + j * 4 + 2] = (uv[offset + j * 4 + 3] = Vector2.zero)));
			if (fontInst.textureGradients)
			{
				uv2[offset + j * 4] = (uv2[offset + j * 4 + 1] = (uv2[offset + j * 4 + 2] = (uv2[offset + j * 4 + 3] = Vector2.zero)));
			}
			if (!fontInst.isPacked)
			{
				color[offset + j * 4] = (color[offset + j * 4 + 1] = tk2dTextGeomGen.meshTopColor);
				color[offset + j * 4 + 2] = (color[offset + j * 4 + 3] = tk2dTextGeomGen.meshBottomColor);
			}
			else
			{
				color[offset + j * 4] = (color[offset + j * 4 + 1] = (color[offset + j * 4 + 2] = (color[offset + j * 4 + 3] = Color.clear)));
			}
		}
		return num3;
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x00075CA8 File Offset: 0x00073EA8
	public static void SetTextMeshIndices(int[] indices, int offset, int vStart, tk2dTextGeomGen.GeomData geomData, int target)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		for (int i = 0; i < textMeshData.maxChars; i++)
		{
			indices[offset + i * 6] = vStart + i * 4;
			indices[offset + i * 6 + 1] = vStart + i * 4 + 1;
			indices[offset + i * 6 + 2] = vStart + i * 4 + 3;
			indices[offset + i * 6 + 3] = vStart + i * 4 + 2;
			indices[offset + i * 6 + 4] = vStart + i * 4;
			indices[offset + i * 6 + 5] = vStart + i * 4 + 3;
		}
	}

	// Token: 0x0400122A RID: 4650
	private static tk2dTextGeomGen.GeomData tmpData = new tk2dTextGeomGen.GeomData();

	// Token: 0x0400122B RID: 4651
	private static readonly Color32[] channelSelectColors = new Color32[]
	{
		new Color32(0, 0, byte.MaxValue, 0),
		new Color(0f, 255f, 0f, 0f),
		new Color(255f, 0f, 0f, 0f),
		new Color(0f, 0f, 0f, 255f)
	};

	// Token: 0x0400122C RID: 4652
	private static Color32 meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400122D RID: 4653
	private static Color32 meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400122E RID: 4654
	private static float meshGradientTexU = 0f;

	// Token: 0x0400122F RID: 4655
	private static int curGradientCount = 1;

	// Token: 0x04001230 RID: 4656
	private static Color32 errorColor = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

	// Token: 0x0200023F RID: 575
	public class GeomData
	{
		// Token: 0x04001231 RID: 4657
		internal tk2dTextMeshData textMeshData;

		// Token: 0x04001232 RID: 4658
		internal tk2dFontData fontInst;

		// Token: 0x04001233 RID: 4659
		internal string formattedText = string.Empty;
	}
}
