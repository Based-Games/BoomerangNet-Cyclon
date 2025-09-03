using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020000AA RID: 170
[AddComponentMenu("NGUI/UI/Font")]
[ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000532 RID: 1330 RVA: 0x000072B2 File Offset: 0x000054B2
	public BMFont bmFont
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mFont : this.mReplacement.bmFont;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000533 RID: 1331 RVA: 0x000072DB File Offset: 0x000054DB
	public int texWidth
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texWidth) : this.mReplacement.texWidth;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x0000731A File Offset: 0x0000551A
	public int texHeight
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texHeight) : this.mReplacement.texHeight;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000535 RID: 1333 RVA: 0x00007359 File Offset: 0x00005559
	public bool hasSymbols
	{
		get
		{
			return (!(this.mReplacement != null)) ? (this.mSymbols.Count != 0) : this.mReplacement.hasSymbols;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000536 RID: 1334 RVA: 0x0000738D File Offset: 0x0000558D
	public List<BMSymbol> symbols
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mSymbols : this.mReplacement.symbols;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000537 RID: 1335 RVA: 0x000073B6 File Offset: 0x000055B6
	// (set) Token: 0x06000538 RID: 1336 RVA: 0x00029F54 File Offset: 0x00028154
	public UIAtlas atlas
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mAtlas : this.mReplacement.atlas;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
			}
			else if (this.mAtlas != value)
			{
				if (value == null)
				{
					if (this.mAtlas != null)
					{
						this.mMat = this.mAtlas.spriteMaterial;
					}
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.mPMA = -1;
				this.mAtlas = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x00029FF0 File Offset: 0x000281F0
	// (set) Token: 0x0600053A RID: 1338 RVA: 0x0002A0B4 File Offset: 0x000282B4
	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat != null)
			{
				if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
				{
					this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
				}
				return this.mMat;
			}
			if (this.mDynamicFont != null)
			{
				return this.mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
			}
			else if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x0002A108 File Offset: 0x00028308
	// (set) Token: 0x0600053C RID: 1340 RVA: 0x0002A158 File Offset: 0x00028358
	public float pixelSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.pixelSize;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.pixelSize;
			}
			return this.mPixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
			}
			else if (this.mAtlas != null)
			{
				this.mAtlas.pixelSize = value;
			}
			else
			{
				float num = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != num)
				{
					this.mPixelSize = num;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x0002A1D4 File Offset: 0x000283D4
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x0002A27C File Offset: 0x0002847C
	public Texture2D texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			return (!(material != null)) ? null : (material.mainTexture as Texture2D);
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x0002A2CC File Offset: 0x000284CC
	// (set) Token: 0x06000540 RID: 1344 RVA: 0x0002A3F4 File Offset: 0x000285F4
	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			if (this.mAtlas != null && this.mSprite == null && this.sprite != null)
			{
				Texture texture = this.mAtlas.texture;
				if (texture != null)
				{
					this.mUVRect = new Rect((float)(this.mSprite.x - this.mSprite.paddingLeft), (float)(this.mSprite.y - this.mSprite.paddingTop), (float)(this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float)(this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
					this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
					if (this.mSprite.hasPadding)
					{
						this.Trim();
					}
				}
			}
			return this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
			}
			else if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000541 RID: 1345 RVA: 0x000073DF File Offset: 0x000055DF
	// (set) Token: 0x06000542 RID: 1346 RVA: 0x0002A44C File Offset: 0x0002864C
	public string spriteName
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mFont.spriteName : this.mReplacement.spriteName;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
			}
			else if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000543 RID: 1347 RVA: 0x0000740D File Offset: 0x0000560D
	public bool isValid
	{
		get
		{
			return this.mDynamicFont != null || this.mFont.isValid;
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x0000742E File Offset: 0x0000562E
	// (set) Token: 0x06000545 RID: 1349 RVA: 0x00007436 File Offset: 0x00005636
	[Obsolete("Use UIFont.defaultSize instead")]
	public int size
	{
		get
		{
			return this.defaultSize;
		}
		set
		{
			this.defaultSize = value;
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x0002A4A4 File Offset: 0x000286A4
	// (set) Token: 0x06000547 RID: 1351 RVA: 0x0000743F File Offset: 0x0000563F
	public int defaultSize
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((!this.isDynamic) ? this.mFont.charSize : this.mDynamicFontSize) : this.mReplacement.defaultSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.defaultSize = value;
			}
			else
			{
				this.mDynamicFontSize = value;
			}
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000548 RID: 1352 RVA: 0x0002A4F4 File Offset: 0x000286F4
	public UISpriteData sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (!this.mSpriteSet)
			{
				this.mSprite = null;
			}
			if (this.mSprite == null)
			{
				if (this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
				{
					this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
					if (this.mSprite == null)
					{
						this.mSprite = this.mAtlas.GetSprite(base.name);
					}
					this.mSpriteSet = true;
					if (this.mSprite == null)
					{
						this.mFont.spriteName = null;
					}
				}
				int i = 0;
				int count = this.mSymbols.Count;
				while (i < count)
				{
					this.symbols[i].MarkAsChanged();
					i++;
				}
			}
			return this.mSprite;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x0000746A File Offset: 0x0000566A
	// (set) Token: 0x0600054A RID: 1354 RVA: 0x0002A5F4 File Offset: 0x000287F4
	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uifont = value;
			if (uifont == this)
			{
				uifont = null;
			}
			if (this.mReplacement != uifont)
			{
				if (uifont != null && uifont.replacement == this)
				{
					uifont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uifont;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x00007472 File Offset: 0x00005672
	public bool isDynamic
	{
		get
		{
			return (!(this.mReplacement != null)) ? (this.mDynamicFont != null) : this.mReplacement.isDynamic;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600054C RID: 1356 RVA: 0x000074A1 File Offset: 0x000056A1
	// (set) Token: 0x0600054D RID: 1357 RVA: 0x0002A66C File Offset: 0x0002886C
	public Font dynamicFont
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mDynamicFont : this.mReplacement.dynamicFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
			}
			else if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600054E RID: 1358 RVA: 0x000074CA File Offset: 0x000056CA
	// (set) Token: 0x0600054F RID: 1359 RVA: 0x000074F3 File Offset: 0x000056F3
	public FontStyle dynamicFontStyle
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
			}
			else if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0002A6D4 File Offset: 0x000288D4
	private void Trim()
	{
		Texture texture = this.mAtlas.texture;
		if (texture != null && this.mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect2 = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			int num = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int num2 = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int num3 = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int num4 = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			this.mFont.Trim(num, num2, num3, num4);
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002A7C8 File Offset: 0x000289C8
	private bool References(UIFont font)
	{
		return !(font == null) && (font == this || (this.mReplacement != null && this.mReplacement.References(font)));
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002A814 File Offset: 0x00028A14
	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		return !(a == null) && !(b == null) && ((a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]) || a == b || a.References(b) || b.References(a));
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000553 RID: 1363 RVA: 0x00007530 File Offset: 0x00005730
	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0002A89C File Offset: 0x00028A9C
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		this.mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UILabel uilabel = array[i];
			if (uilabel.enabled && NGUITools.GetActive(uilabel.gameObject) && UIFont.CheckIfRelated(this, uilabel.bitmapFont))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			i++;
		}
		int j = 0;
		int count = this.mSymbols.Count;
		while (j < count)
		{
			this.symbols[j].MarkAsChanged();
			j++;
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0002A968 File Offset: 0x00028B68
	public Vector2 CalculatePrintedSize(string text)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePrintedSize(text);
		}
		if (this.isDynamic)
		{
			NGUIText.current.size = this.mDynamicFontSize;
			NGUIText.current.style = this.mDynamicFontStyle;
			return NGUIText.CalculatePrintedSize(this.mDynamicFont, text);
		}
		Vector2 zero = Vector2.zero;
		if (this.mFont != null && this.mFont.isValid && !string.IsNullOrEmpty(text))
		{
			if (NGUIText.current.encoding)
			{
				text = NGUIText.StripSymbols(text);
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int length = text.Length;
			int num5 = NGUIText.current.size + NGUIText.current.spacingY;
			bool flag = NGUIText.current.encoding && NGUIText.current.symbolStyle != NGUIText.SymbolStyle.None && this.hasSymbols;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num > num4)
					{
						num4 = num;
					}
					num = 0;
					num2 += num5;
					num3 = 0;
				}
				else if (c < ' ')
				{
					num3 = 0;
				}
				else
				{
					BMSymbol bmsymbol = ((!flag) ? null : this.MatchSymbol(text, i, length));
					if (bmsymbol == null)
					{
						BMGlyph glyph = this.mFont.GetGlyph((int)c);
						if (glyph != null)
						{
							num += NGUIText.current.spacingX + ((num3 == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(num3)));
							num3 = (int)c;
						}
					}
					else
					{
						num += NGUIText.current.spacingX + bmsymbol.width;
						i += bmsymbol.length - 1;
						num3 = 0;
					}
				}
			}
			zero.x = (float)((num <= num4) ? num4 : num);
			zero.y = (float)(num2 + NGUIText.current.size);
		}
		return zero;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0002AB74 File Offset: 0x00028D74
	public string GetEndOfLineThatFits(string text)
	{
		int length = text.Length;
		int num = this.CalculateOffsetToFit(text);
		return text.Substring(num, length - num);
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0002AB9C File Offset: 0x00028D9C
	public int CalculateOffsetToFit(string text)
	{
		if (NGUIText.current.lineWidth < 1)
		{
			return 0;
		}
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculateOffsetToFit(text);
		}
		if (this.isDynamic)
		{
			NGUIText.current.size = this.mDynamicFontSize;
			NGUIText.current.style = this.mDynamicFontStyle;
			return NGUIText.CalculateOffsetToFit(this.mDynamicFont, text);
		}
		int length = text.Length;
		int num = NGUIText.current.lineWidth;
		BMGlyph bmglyph = null;
		int num2 = length;
		bool flag = NGUIText.current.encoding && NGUIText.current.symbolStyle != NGUIText.SymbolStyle.None && this.hasSymbols;
		while (num2 > 0 && num > 0)
		{
			char c = text[--num2];
			BMSymbol bmsymbol = ((!flag) ? null : this.MatchSymbol(text, num2, length));
			int num3 = NGUIText.current.spacingX;
			if (bmsymbol != null)
			{
				num3 += bmsymbol.advance;
			}
			else
			{
				BMGlyph glyph = this.mFont.GetGlyph((int)c);
				if (glyph == null)
				{
					bmglyph = null;
					continue;
				}
				num3 += glyph.advance + ((bmglyph != null) ? bmglyph.GetKerning((int)c) : 0);
				bmglyph = glyph;
			}
			num -= num3;
		}
		if (num < 0)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0002AD04 File Offset: 0x00028F04
	public bool WrapText(string text, out string finalText)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.WrapText(text, out finalText);
		}
		if (this.isDynamic)
		{
			NGUIText.current.size = this.mDynamicFontSize;
			NGUIText.current.style = this.mDynamicFontStyle;
			return NGUIText.WrapText(this.mDynamicFont, text, out finalText);
		}
		if (NGUIText.current.lineWidth < 1 || NGUIText.current.lineHeight < 1)
		{
			finalText = string.Empty;
			return false;
		}
		int num = ((NGUIText.current.maxLines <= 0) ? NGUIText.current.lineHeight : Mathf.Min(NGUIText.current.lineHeight, NGUIText.current.size * NGUIText.current.maxLines));
		int num2 = ((NGUIText.current.maxLines <= 0) ? 1000000 : NGUIText.current.maxLines);
		int num3 = NGUIText.current.size + NGUIText.current.spacingY;
		num2 = ((num3 <= 0) ? 0 : Mathf.Min(num2, num / num3));
		if (num2 == 0)
		{
			finalText = string.Empty;
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int length = text.Length;
		int num4 = NGUIText.current.lineWidth;
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int num7 = 1;
		bool flag = true;
		bool flag2 = NGUIText.current.encoding && NGUIText.current.symbolStyle != NGUIText.SymbolStyle.None && this.hasSymbols;
		while (i < length)
		{
			char c = text[i];
			if (c == '\n')
			{
				if (num7 == num2)
				{
					break;
				}
				num4 = NGUIText.current.lineWidth;
				if (num6 < i)
				{
					stringBuilder.Append(text.Substring(num6, i - num6 + 1));
				}
				else
				{
					stringBuilder.Append(c);
				}
				flag = true;
				num7++;
				num6 = i + 1;
				num5 = 0;
			}
			else
			{
				if (c == ' ' && num5 != 32 && num6 < i)
				{
					stringBuilder.Append(text.Substring(num6, i - num6 + 1));
					flag = false;
					num6 = i + 1;
					num5 = (int)c;
				}
				if (NGUIText.ParseSymbol(text, ref i))
				{
					i--;
				}
				else
				{
					BMSymbol bmsymbol = ((!flag2) ? null : this.MatchSymbol(text, i, length));
					int num8;
					if (bmsymbol != null)
					{
						num8 = NGUIText.current.spacingX + bmsymbol.advance;
					}
					else
					{
						BMGlyph bmglyph = ((bmsymbol != null) ? null : this.mFont.GetGlyph((int)c));
						if (bmglyph == null)
						{
							goto IL_3EB;
						}
						num8 = NGUIText.current.spacingX + ((num5 == 0) ? bmglyph.advance : (bmglyph.advance + bmglyph.GetKerning(num5)));
					}
					num4 -= num8;
					if (num4 < 0)
					{
						if (flag || num7 == num2)
						{
							stringBuilder.Append(text.Substring(num6, Mathf.Max(0, i - num6)));
							if (num7++ == num2)
							{
								num6 = i;
								break;
							}
							NGUIText.EndLine(ref stringBuilder);
							flag = true;
							if (c == ' ')
							{
								num6 = i + 1;
								num4 = NGUIText.current.lineWidth;
							}
							else
							{
								num6 = i;
								num4 = NGUIText.current.lineWidth - num8;
							}
							num5 = 0;
						}
						else
						{
							while (num6 < length && text[num6] == ' ')
							{
								num6++;
							}
							flag = true;
							num4 = NGUIText.current.lineWidth;
							i = num6 - 1;
							num5 = 0;
							if (num7++ == num2)
							{
								break;
							}
							NGUIText.EndLine(ref stringBuilder);
							goto IL_3EB;
						}
					}
					else
					{
						num5 = (int)c;
					}
					if (bmsymbol != null)
					{
						i += bmsymbol.length - 1;
						num5 = 0;
					}
				}
			}
			IL_3EB:
			i++;
		}
		if (num6 < i)
		{
			stringBuilder.Append(text.Substring(num6, i - num6));
		}
		finalText = stringBuilder.ToString();
		return i == length || num7 <= Mathf.Min(NGUIText.current.maxLines, num2);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0002B154 File Offset: 0x00029354
	public void Print(string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.Print(text, verts, uvs, cols);
		}
		else if (!string.IsNullOrEmpty(text))
		{
			if (!this.isValid)
			{
				Debug.LogError("Attempting to print using an invalid font!");
				return;
			}
			if (this.isDynamic)
			{
				NGUIText.current.size = this.mDynamicFontSize;
				NGUIText.current.style = this.mDynamicFontStyle;
				NGUIText.Print(this.dynamicFont, text, verts, uvs, cols);
				return;
			}
			UIFont.mColors.Add(Color.white);
			int size = NGUIText.current.size;
			int num = verts.size;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = size + NGUIText.current.spacingY;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			Color color = NGUIText.current.tint * NGUIText.current.gradientBottom;
			Color color2 = NGUIText.current.tint * NGUIText.current.gradientTop;
			Color32 color3 = NGUIText.current.tint;
			float num7 = this.uvRect.width / (float)this.mFont.texWidth;
			float num8 = this.mUVRect.height / (float)this.mFont.texHeight;
			int length = text.Length;
			bool flag = NGUIText.current.encoding && NGUIText.current.symbolStyle != NGUIText.SymbolStyle.None && this.hasSymbols && this.sprite != null;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num3 > num2)
					{
						num2 = num3;
					}
					if (NGUIText.current.alignment != TextAlignment.Left)
					{
						NGUIText.Align(verts, num, (float)(num3 - NGUIText.current.spacingX));
						num = verts.size;
					}
					num3 = 0;
					num4 += num6;
					num5 = 0;
				}
				else if (c < ' ')
				{
					num5 = 0;
				}
				else if (NGUIText.current.encoding && NGUIText.ParseSymbol(text, ref i, UIFont.mColors, NGUIText.current.premultiply))
				{
					Color color4 = NGUIText.current.tint * UIFont.mColors[UIFont.mColors.size - 1];
					color3 = color4;
					if (NGUIText.current.gradient)
					{
						color = NGUIText.current.gradientBottom * color4;
						color2 = NGUIText.current.gradientTop * color4;
					}
					i--;
				}
				else
				{
					BMSymbol bmsymbol = ((!flag) ? null : this.MatchSymbol(text, i, length));
					if (bmsymbol == null)
					{
						BMGlyph glyph = this.mFont.GetGlyph((int)c);
						if (glyph == null)
						{
							goto IL_7A8;
						}
						if (num5 != 0)
						{
							num3 += glyph.GetKerning(num5);
						}
						if (c == ' ')
						{
							num3 += NGUIText.current.spacingX + glyph.advance;
							num5 = (int)c;
							goto IL_7A8;
						}
						zero.x = (float)(num3 + glyph.offsetX);
						zero.y = (float)(-(float)(num4 + glyph.offsetY));
						zero2.x = zero.x + (float)glyph.width;
						zero2.y = zero.y - (float)glyph.height;
						zero3.x = this.mUVRect.xMin + num7 * (float)glyph.x;
						zero3.y = this.mUVRect.yMax - num8 * (float)glyph.y;
						zero4.x = zero3.x + num7 * (float)glyph.width;
						zero4.y = zero3.y - num8 * (float)glyph.height;
						num3 += NGUIText.current.spacingX + glyph.advance;
						num5 = (int)c;
						if (glyph.channel == 0 || glyph.channel == 15)
						{
							if (NGUIText.current.gradient)
							{
								float num9 = (float)(NGUIText.current.size - glyph.offsetY);
								float num10 = num9 - (float)glyph.height;
								num9 /= (float)NGUIText.current.size;
								num10 /= (float)NGUIText.current.size;
								UIFont.s_c0 = Color.Lerp(color, color2, num9);
								UIFont.s_c1 = Color.Lerp(color, color2, num10);
								cols.Add(UIFont.s_c0);
								cols.Add(UIFont.s_c1);
								cols.Add(UIFont.s_c1);
								cols.Add(UIFont.s_c0);
							}
							else
							{
								for (int j = 0; j < 4; j++)
								{
									cols.Add(color3);
								}
							}
						}
						else
						{
							Color color5 = color3;
							color5 *= 0.49f;
							switch (glyph.channel)
							{
							case 1:
								color5.b += 0.51f;
								break;
							case 2:
								color5.g += 0.51f;
								break;
							case 4:
								color5.r += 0.51f;
								break;
							case 8:
								color5.a += 0.51f;
								break;
							}
							for (int k = 0; k < 4; k++)
							{
								cols.Add(color5);
							}
						}
					}
					else
					{
						zero.x = (float)(num3 + bmsymbol.offsetX);
						zero.y = (float)(-(float)(num4 + bmsymbol.offsetY));
						zero2.x = zero.x + (float)bmsymbol.width;
						zero2.y = zero.y - (float)bmsymbol.height;
						Rect uvRect = bmsymbol.uvRect;
						zero3.x = uvRect.xMin;
						zero3.y = uvRect.yMax;
						zero4.x = uvRect.xMax;
						zero4.y = uvRect.yMin;
						num3 += NGUIText.current.spacingX + bmsymbol.advance;
						i += bmsymbol.length - 1;
						num5 = 0;
						if (NGUIText.current.symbolStyle == NGUIText.SymbolStyle.Colored)
						{
							for (int l = 0; l < 4; l++)
							{
								cols.Add(color3);
							}
						}
						else
						{
							Color32 color6 = Color.white;
							color6.a = color3.a;
							for (int m = 0; m < 4; m++)
							{
								cols.Add(color6);
							}
						}
					}
					verts.Add(new Vector3(zero2.x, zero.y));
					verts.Add(new Vector3(zero2.x, zero2.y));
					verts.Add(new Vector3(zero.x, zero2.y));
					verts.Add(new Vector3(zero.x, zero.y));
					uvs.Add(new Vector2(zero4.x, zero3.y));
					uvs.Add(new Vector2(zero4.x, zero4.y));
					uvs.Add(new Vector2(zero3.x, zero4.y));
					uvs.Add(new Vector2(zero3.x, zero3.y));
				}
				IL_7A8:;
			}
			if (NGUIText.current.alignment != TextAlignment.Left && num < verts.size)
			{
				NGUIText.Align(verts, num, (float)(num3 - NGUIText.current.spacingX));
				num = verts.size;
			}
			UIFont.mColors.Clear();
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0002B958 File Offset: 0x00029B58
	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		int count = this.mSymbols.Count;
		while (i < count)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			if (bmsymbol.sequence == sequence)
			{
				return bmsymbol;
			}
			i++;
		}
		if (createIfMissing)
		{
			BMSymbol bmsymbol2 = new BMSymbol();
			bmsymbol2.sequence = sequence;
			this.mSymbols.Add(bmsymbol2);
			return bmsymbol2;
		}
		return null;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0002B9C8 File Offset: 0x00029BC8
	private BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			int length = bmsymbol.length;
			if (length != 0 && textLength >= length)
			{
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (text[offset + j] != bmsymbol.sequence[j])
					{
						flag = false;
						break;
					}
				}
				if (flag && bmsymbol.Validate(this.atlas))
				{
					return bmsymbol;
				}
			}
		}
		return null;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0002BA80 File Offset: 0x00029C80
	public void AddSymbol(string sequence, string spriteName)
	{
		BMSymbol symbol = this.GetSymbol(sequence, true);
		symbol.spriteName = spriteName;
		this.MarkAsChanged();
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0002BAA4 File Offset: 0x00029CA4
	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsChanged();
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0002BAD4 File Offset: 0x00029CD4
	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsChanged();
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0002BB00 File Offset: 0x00029D00
	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int i = 0;
			int count = this.symbols.Count;
			while (i < count)
			{
				BMSymbol bmsymbol = this.symbols[i];
				if (s.Equals(bmsymbol.spriteName))
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04000416 RID: 1046
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	// Token: 0x04000417 RID: 1047
	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04000418 RID: 1048
	[SerializeField]
	[HideInInspector]
	private BMFont mFont = new BMFont();

	// Token: 0x04000419 RID: 1049
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	// Token: 0x0400041A RID: 1050
	[SerializeField]
	[HideInInspector]
	private UIFont mReplacement;

	// Token: 0x0400041B RID: 1051
	[SerializeField]
	[HideInInspector]
	private float mPixelSize = 1f;

	// Token: 0x0400041C RID: 1052
	[SerializeField]
	[HideInInspector]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	// Token: 0x0400041D RID: 1053
	[HideInInspector]
	[SerializeField]
	private Font mDynamicFont;

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	[HideInInspector]
	private int mDynamicFontSize = 16;

	// Token: 0x0400041F RID: 1055
	[SerializeField]
	[HideInInspector]
	private FontStyle mDynamicFontStyle;

	// Token: 0x04000420 RID: 1056
	private UISpriteData mSprite;

	// Token: 0x04000421 RID: 1057
	private int mPMA = -1;

	// Token: 0x04000422 RID: 1058
	private bool mSpriteSet;

	// Token: 0x04000423 RID: 1059
	private static BetterList<Color> mColors = new BetterList<Color>();

	// Token: 0x04000424 RID: 1060
	private static Color32 s_c0;

	// Token: 0x04000425 RID: 1061
	private static Color32 s_c1;
}
