using System;
using UnityEngine;

// Token: 0x020000B0 RID: 176
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Label")]
public class UILabel : UIWidget
{
	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x00007769 File Offset: 0x00005969
	// (set) Token: 0x06000583 RID: 1411 RVA: 0x00007771 File Offset: 0x00005971
	private bool hasChanged
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
			}
			else
			{
				this.mShouldBeProcessed = false;
			}
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x0002C914 File Offset: 0x0002AB14
	// (set) Token: 0x06000585 RID: 1413 RVA: 0x00007793 File Offset: 0x00005993
	public override Material material
	{
		get
		{
			if (this.mMaterial != null)
			{
				return this.mMaterial;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			if (this.mMaterial != value)
			{
				this.MarkAsChanged();
				this.mMaterial = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000586 RID: 1414 RVA: 0x000077B9 File Offset: 0x000059B9
	// (set) Token: 0x06000587 RID: 1415 RVA: 0x000077C1 File Offset: 0x000059C1
	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x000077CA File Offset: 0x000059CA
	// (set) Token: 0x06000589 RID: 1417 RVA: 0x0002C974 File Offset: 0x0002AB74
	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				if (value != null && value.dynamicFont != null)
				{
					this.trueTypeFont = value.dynamicFont;
					return;
				}
				if (this.trueTypeFont != null)
				{
					this.trueTypeFont = null;
				}
				else
				{
					base.RemoveFromPanel();
				}
				this.mFont = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x000077D2 File Offset: 0x000059D2
	// (set) Token: 0x0600058B RID: 1419 RVA: 0x0002C9EC File Offset: 0x0002ABEC
	public Font trueTypeFont
	{
		get
		{
			return this.mTrueTypeFont;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.hasChanged = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x000077DA File Offset: 0x000059DA
	// (set) Token: 0x0600058D RID: 1421 RVA: 0x0002CA50 File Offset: 0x0002AC50
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			return (!(this.mFont != null)) ? this.mTrueTypeFont : this.mFont;
		}
		set
		{
			UIFont uifont = value as UIFont;
			if (uifont != null)
			{
				this.bitmapFont = uifont;
			}
			else
			{
				this.trueTypeFont = value as Font;
			}
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x0600058E RID: 1422 RVA: 0x000077FE File Offset: 0x000059FE
	// (set) Token: 0x0600058F RID: 1423 RVA: 0x0002CA88 File Offset: 0x0002AC88
	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = string.Empty;
					this.hasChanged = true;
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.hasChanged = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000590 RID: 1424 RVA: 0x00007806 File Offset: 0x00005A06
	// (set) Token: 0x06000591 RID: 1425 RVA: 0x0000782B File Offset: 0x00005A2B
	public int fontSize
	{
		get
		{
			if (this.mFont != null)
			{
				return this.mFont.defaultSize;
			}
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 144);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.hasChanged = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000592 RID: 1426 RVA: 0x0000785B File Offset: 0x00005A5B
	// (set) Token: 0x06000593 RID: 1427 RVA: 0x00007863 File Offset: 0x00005A63
	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.hasChanged = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000594 RID: 1428 RVA: 0x00007885 File Offset: 0x00005A85
	// (set) Token: 0x06000595 RID: 1429 RVA: 0x0000788D File Offset: 0x00005A8D
	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000596 RID: 1430 RVA: 0x000078A8 File Offset: 0x00005AA8
	// (set) Token: 0x06000597 RID: 1431 RVA: 0x000078B0 File Offset: 0x00005AB0
	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000598 RID: 1432 RVA: 0x000078DB File Offset: 0x00005ADB
	// (set) Token: 0x06000599 RID: 1433 RVA: 0x000078E3 File Offset: 0x00005AE3
	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600059A RID: 1434 RVA: 0x0000790E File Offset: 0x00005B0E
	// (set) Token: 0x0600059B RID: 1435 RVA: 0x00007916 File Offset: 0x00005B16
	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600059C RID: 1436 RVA: 0x00007931 File Offset: 0x00005B31
	// (set) Token: 0x0600059D RID: 1437 RVA: 0x00007939 File Offset: 0x00005B39
	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600059E RID: 1438 RVA: 0x00007954 File Offset: 0x00005B54
	private bool usePrintedSize
	{
		get
		{
			return this.trueTypeFont != null && this.keepCrispWhenShrunk != UILabel.Crispness.Never;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x0600059F RID: 1439 RVA: 0x00007975 File Offset: 0x00005B75
	// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0000797D File Offset: 0x00005B7D
	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.hasChanged = true;
			}
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00007999 File Offset: 0x00005B99
	// (set) Token: 0x060005A2 RID: 1442 RVA: 0x000079A1 File Offset: 0x00005BA1
	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.hasChanged = true;
			}
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000079BD File Offset: 0x00005BBD
	// (set) Token: 0x060005A4 RID: 1444 RVA: 0x000079C5 File Offset: 0x00005BC5
	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.hasChanged = true;
			}
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060005A5 RID: 1445 RVA: 0x000079E1 File Offset: 0x00005BE1
	// (set) Token: 0x060005A6 RID: 1446 RVA: 0x000079E9 File Offset: 0x00005BE9
	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060005A7 RID: 1447 RVA: 0x000079F2 File Offset: 0x00005BF2
	// (set) Token: 0x060005A8 RID: 1448 RVA: 0x000079FA File Offset: 0x00005BFA
	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00007A03 File Offset: 0x00005C03
	// (set) Token: 0x060005AA RID: 1450 RVA: 0x00007A11 File Offset: 0x00005C11
	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = ((!value) ? 1 : 0);
				this.hasChanged = true;
			}
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x00007A3F File Offset: 0x00005C3F
	public override Vector3[] localCorners
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060005AC RID: 1452 RVA: 0x00007A58 File Offset: 0x00005C58
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060005AD RID: 1453 RVA: 0x00007A71 File Offset: 0x00005C71
	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060005AE RID: 1454 RVA: 0x00007A8A File Offset: 0x00005C8A
	// (set) Token: 0x060005AF RID: 1455 RVA: 0x00007A92 File Offset: 0x00005C92
	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.hasChanged = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00007AC5 File Offset: 0x00005CC5
	// (set) Token: 0x060005B1 RID: 1457 RVA: 0x00007ACD File Offset: 0x00005CCD
	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.hasChanged = true;
			}
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00007AE9 File Offset: 0x00005CE9
	// (set) Token: 0x060005B3 RID: 1459 RVA: 0x00007AF1 File Offset: 0x00005CF1
	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.hasChanged = true;
				}
			}
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00007B1D File Offset: 0x00005D1D
	// (set) Token: 0x060005B5 RID: 1461 RVA: 0x00007B25 File Offset: 0x00005D25
	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.hasChanged = true;
			}
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00007B46 File Offset: 0x00005D46
	// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00007B51 File Offset: 0x00005D51
	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0002CAF4 File Offset: 0x0002ACF4
	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00007B60 File Offset: 0x00005D60
	public Vector2 printedSize
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return this.mCalculatedSize;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00007B79 File Offset: 0x00005D79
	public override Vector2 localSize
	{
		get
		{
			if (this.hasChanged)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x00007B92 File Offset: 0x00005D92
	private bool isValid
	{
		get
		{
			return this.mFont != null || this.mTrueTypeFont != null;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x060005BC RID: 1468 RVA: 0x00007BB4 File Offset: 0x00005DB4
	private float pixelSize
	{
		get
		{
			return (!(this.mFont != null)) ? 1f : this.mFont.pixelSize;
		}
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002CB5C File Offset: 0x0002AD5C
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.mTrueTypeFont == null && this.mFont != null && this.mFont.isDynamic)
		{
			this.mTrueTypeFont = this.mFont.dynamicFont;
			this.mFontSize = this.mFont.defaultSize;
			this.mFontStyle = this.mFont.dynamicFontStyle;
			this.mFont = null;
		}
		this.SetActiveFont(this.mTrueTypeFont);
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00007BDC File Offset: 0x00005DDC
	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		base.OnDisable();
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0002CBE8 File Offset: 0x0002ADE8
	protected void SetActiveFont(Font fnt)
	{
		if (this.mActiveTTF != fnt)
		{
			if (this.mActiveTTF != null)
			{
				Font font = this.mActiveTTF;
				font.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Remove(font.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
			}
			this.mActiveTTF = fnt;
			if (this.mActiveTTF != null)
			{
				Font font2 = this.mActiveTTF;
				font2.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(font2.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
			}
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00007BEB File Offset: 0x00005DEB
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.hasChanged)
		{
			this.ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0002CC80 File Offset: 0x0002AE80
	protected override void UpgradeFrom265()
	{
		this.ProcessText(true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount <= 0) ? UILabel.Overflow.ShrinkContent : UILabel.Overflow.ResizeHeight);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int num = Mathf.RoundToInt((float)this.mFont.defaultSize * this.mFont.pixelSize);
			if (base.height < num)
			{
				base.height = num;
			}
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		if (base.GetComponent<BoxCollider>() != null)
		{
			NGUITools.AddWidgetCollider(base.gameObject, true);
		}
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00007C05 File Offset: 0x00005E05
	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00007C2C File Offset: 0x00005E2C
	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText();
			if (this.mActiveTTF != null)
			{
				NGUIText.RequestCharactersInTexture(this.mActiveTTF, this.mText);
			}
		}
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0002CD78 File Offset: 0x0002AF78
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = this.material != null && this.material.shader != null && this.material.shader.name.Contains("Premultiplied");
		this.ProcessAndRequest();
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00007C67 File Offset: 0x00005E67
	public override void MarkAsChanged()
	{
		this.hasChanged = true;
		base.MarkAsChanged();
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00007C76 File Offset: 0x00005E76
	private void ProcessText()
	{
		this.ProcessText(false);
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0002CE20 File Offset: 0x0002B020
	private void ProcessText(bool legacyMode)
	{
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.hasChanged = false;
		int fontSize = this.fontSize;
		float num = 1f / (float)fontSize;
		float pixelSize = this.pixelSize;
		float num2 = 1f / pixelSize;
		float num3 = ((!legacyMode) ? ((float)base.width * num2) : ((this.mMaxLineWidth == 0) ? 1000000f : ((float)this.mMaxLineWidth * num2)));
		float num4 = ((!legacyMode) ? ((float)base.height * num2) : ((this.mMaxLineHeight == 0) ? 1000000f : ((float)this.mMaxLineHeight * num2)));
		this.mScale = 1f;
		this.mPrintedSize = Mathf.Abs((!legacyMode) ? fontSize : Mathf.RoundToInt(base.cachedTransform.localScale.x));
		NGUIText.current.size = fontSize;
		this.UpdateNGUIText();
		if (this.mPrintedSize > 0)
		{
			for (;;)
			{
				this.mScale = (float)this.mPrintedSize * num;
				bool flag = true;
				NGUIText.current.lineWidth = ((this.mOverflow != UILabel.Overflow.ResizeFreely) ? Mathf.RoundToInt(num3 / this.mScale) : 1000000);
				if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
				{
					NGUIText.current.lineHeight = 1000000;
				}
				else
				{
					NGUIText.current.lineHeight = Mathf.RoundToInt(num4 / this.mScale);
				}
				if (num3 > 0f || num4 > 0f)
				{
					if (this.mFont != null)
					{
						flag = this.mFont.WrapText(this.mText, out this.mProcessedText);
					}
					else
					{
						flag = NGUIText.WrapText(this.mTrueTypeFont, this.mText, out this.mProcessedText);
					}
				}
				else
				{
					this.mProcessedText = this.mText;
				}
				if (!string.IsNullOrEmpty(this.mProcessedText))
				{
					if (this.mFont != null)
					{
						this.mCalculatedSize = this.mFont.CalculatePrintedSize(this.mProcessedText);
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mTrueTypeFont, this.mProcessedText);
					}
				}
				else
				{
					this.mCalculatedSize = Vector2.zero;
				}
				if (this.mOverflow == UILabel.Overflow.ResizeFreely)
				{
					break;
				}
				if (this.mOverflow == UILabel.Overflow.ResizeHeight)
				{
					goto Block_15;
				}
				if (this.mOverflow != UILabel.Overflow.ShrinkContent || flag || --this.mPrintedSize <= 1)
				{
					goto IL_2F1;
				}
			}
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x * pixelSize);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y * pixelSize);
			goto IL_2F1;
			Block_15:
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y * pixelSize);
			IL_2F1:
			if (legacyMode)
			{
				base.width = Mathf.RoundToInt(this.mCalculatedSize.x * pixelSize);
				base.height = Mathf.RoundToInt(this.mCalculatedSize.y * pixelSize);
				base.cachedTransform.localScale = Vector3.one;
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = string.Empty;
			this.mScale = 1f;
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002D19C File Offset: 0x0002B39C
	public override void MakePixelPerfect()
	{
		if (this.ambigiousFont != null)
		{
			float num = ((!(this.bitmapFont != null)) ? 1f : this.bitmapFont.pixelSize);
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = Vector3.one;
			if (this.mOverflow == UILabel.Overflow.ResizeFreely)
			{
				this.AssumeNaturalSize();
			}
			else
			{
				UILabel.Overflow overflow = this.mOverflow;
				this.mOverflow = UILabel.Overflow.ShrinkContent;
				this.ProcessText(false);
				this.mOverflow = overflow;
				int num2 = Mathf.RoundToInt(this.mCalculatedSize.x * num);
				int num3 = Mathf.RoundToInt(this.mCalculatedSize.y * num);
				if (this.bitmapFont != null)
				{
					num2 = Mathf.Max(new int[] { this.bitmapFont.defaultSize });
					num3 = Mathf.Max(new int[] { this.bitmapFont.defaultSize });
				}
				else
				{
					num2 = Mathf.Max(new int[] { base.minWidth });
					num3 = Mathf.Max(new int[] { base.minHeight });
				}
				if (base.width < num2)
				{
					base.width = num2;
				}
				if (base.height < num3)
				{
					base.height = num3;
				}
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002D348 File Offset: 0x0002B548
	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText(false);
			float num = ((!(this.bitmapFont != null)) ? 1f : this.bitmapFont.pixelSize);
			base.width = Mathf.RoundToInt(this.mCalculatedSize.x * num);
			base.height = Mathf.RoundToInt(this.mCalculatedSize.y * num);
		}
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0002D3C4 File Offset: 0x0002B5C4
	private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= this.finalAlpha;
		Color32 color2 = ((!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			cols.buffer[i] = color2;
		}
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0002D4D0 File Offset: 0x0002B6D0
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.size;
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlpha)
		{
			color = NGUITools.ApplyPMA(color);
		}
		string processedText = this.processedText;
		float num2 = ((!(this.mFont != null)) ? 1f : this.mFont.pixelSize);
		float num3 = this.mScale * num2;
		bool usePrintedSize = this.usePrintedSize;
		int size = verts.size;
		this.UpdateNGUIText();
		NGUIText.current.size = ((!usePrintedSize) ? this.fontSize : this.mPrintedSize);
		NGUIText.current.lineWidth = ((!usePrintedSize) ? Mathf.RoundToInt((float)this.mWidth / num3) : this.mWidth);
		NGUIText.current.tint = color;
		if (this.mFont != null)
		{
			this.mFont.Print(processedText, verts, uvs, cols);
		}
		else
		{
			NGUIText.Print(this.mTrueTypeFont, processedText, verts, uvs, cols);
		}
		Vector2 pivotOffset = base.pivotOffset;
		float num4 = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num5 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y);
		num5 = (float)Mathf.RoundToInt(num5 + Mathf.Lerp(this.mCalculatedSize.y * num3 - (float)this.mHeight, 0f, pivotOffset.y));
		if (usePrintedSize || num3 == 1f)
		{
			for (int i = size; i < verts.size; i++)
			{
				Vector3[] buffer = verts.buffer;
				int num6 = i;
				buffer[num6].x = buffer[num6].x + num4;
				Vector3[] buffer2 = verts.buffer;
				int num7 = i;
				buffer2[num7].y = buffer2[num7].y + num5;
			}
		}
		else
		{
			for (int j = size; j < verts.size; j++)
			{
				verts.buffer[j].x = num4 + verts.buffer[j].x * num3;
				verts.buffer[j].y = num5 + verts.buffer[j].y * num3;
			}
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int num8 = verts.size;
			float num9 = num2;
			num4 = num9 * this.mEffectDistance.x;
			num5 = num9 * this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, num8, num4, -num5);
			if (this.effectStyle == UILabel.Effect.Outline)
			{
				num = num8;
				num8 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, num8, -num4, num5);
				num = num8;
				num8 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, num8, num4, num5);
				num = num8;
				num8 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, num8, -num4, -num5);
			}
		}
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.current.encoding = false;
		NGUIText.current.symbolStyle = NGUIText.SymbolStyle.None;
		if (this.bitmapFont != null)
		{
			return this.bitmapFont.CalculateOffsetToFit(text);
		}
		return NGUIText.CalculateOffsetToFit(this.trueTypeFont, text);
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0002D84C File Offset: 0x0002BA4C
	public void UpdateNGUIText()
	{
		NGUIText.current.size = this.fontSize;
		NGUIText.current.style = this.mFontStyle;
		NGUIText.current.lineWidth = this.mWidth;
		NGUIText.current.lineHeight = this.mHeight;
		NGUIText.current.gradient = this.mApplyGradient;
		NGUIText.current.gradientTop = this.mGradientTop;
		NGUIText.current.gradientBottom = this.mGradientBottom;
		NGUIText.current.encoding = this.mEncoding;
		NGUIText.current.premultiply = this.mPremultiply;
		NGUIText.current.symbolStyle = this.mSymbols;
		NGUIText.current.spacingX = this.mSpacingX;
		NGUIText.current.spacingY = this.mSpacingY;
		NGUIText.current.maxLines = this.mMaxLineCount;
		UIRoot root = base.root;
		NGUIText.current.pixelDensity = ((!this.usePrintedSize || !(root != null)) ? 1f : (1f / root.pixelSizeAdjustment));
		UIWidget.Pivot pivot = base.pivot;
		if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
		{
			NGUIText.current.alignment = TextAlignment.Left;
		}
		else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
		{
			NGUIText.current.alignment = TextAlignment.Right;
		}
		else
		{
			NGUIText.current.alignment = TextAlignment.Center;
		}
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x00007C7F File Offset: 0x00005E7F
	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002D9C4 File Offset: 0x0002BBC4
	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			this.text = ((!UIPopupList.current.isLocalized) ? UIPopupList.current.value : Localization.Localize(UIPopupList.current.value));
		}
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00007CBB File Offset: 0x00005EBB
	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0002DA14 File Offset: 0x0002BC14
	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.current.lineHeight = height;
		if (this.mFont != null)
		{
			return this.mFont.WrapText(text, out final);
		}
		if (this.mTrueTypeFont != null)
		{
			return NGUIText.WrapText(this.mTrueTypeFont, text, out final);
		}
		final = null;
		return false;
	}

	// Token: 0x04000451 RID: 1105
	public UILabel.Crispness keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;

	// Token: 0x04000452 RID: 1106
	[SerializeField]
	[HideInInspector]
	private Font mTrueTypeFont;

	// Token: 0x04000453 RID: 1107
	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	// Token: 0x04000454 RID: 1108
	[Multiline(6)]
	[HideInInspector]
	[SerializeField]
	private string mText = string.Empty;

	// Token: 0x04000455 RID: 1109
	[HideInInspector]
	[SerializeField]
	private int mFontSize = 16;

	// Token: 0x04000456 RID: 1110
	[HideInInspector]
	[SerializeField]
	private FontStyle mFontStyle;

	// Token: 0x04000457 RID: 1111
	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	// Token: 0x04000458 RID: 1112
	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	// Token: 0x04000459 RID: 1113
	[HideInInspector]
	[SerializeField]
	private UILabel.Effect mEffectStyle;

	// Token: 0x0400045A RID: 1114
	[SerializeField]
	[HideInInspector]
	private Color mEffectColor = Color.black;

	// Token: 0x0400045B RID: 1115
	[HideInInspector]
	[SerializeField]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Uncolored;

	// Token: 0x0400045C RID: 1116
	[HideInInspector]
	[SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	// Token: 0x0400045D RID: 1117
	[HideInInspector]
	[SerializeField]
	private UILabel.Overflow mOverflow;

	// Token: 0x0400045E RID: 1118
	[HideInInspector]
	[SerializeField]
	private Material mMaterial;

	// Token: 0x0400045F RID: 1119
	[HideInInspector]
	[SerializeField]
	private bool mApplyGradient;

	// Token: 0x04000460 RID: 1120
	[SerializeField]
	[HideInInspector]
	private Color mGradientTop = Color.white;

	// Token: 0x04000461 RID: 1121
	[HideInInspector]
	[SerializeField]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x04000462 RID: 1122
	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	// Token: 0x04000463 RID: 1123
	[HideInInspector]
	[SerializeField]
	private int mSpacingY;

	// Token: 0x04000464 RID: 1124
	[SerializeField]
	[HideInInspector]
	private bool mShrinkToFit;

	// Token: 0x04000465 RID: 1125
	[SerializeField]
	[HideInInspector]
	private int mMaxLineWidth;

	// Token: 0x04000466 RID: 1126
	[HideInInspector]
	[SerializeField]
	private int mMaxLineHeight;

	// Token: 0x04000467 RID: 1127
	[SerializeField]
	[HideInInspector]
	private float mLineWidth;

	// Token: 0x04000468 RID: 1128
	[SerializeField]
	[HideInInspector]
	private bool mMultiline = true;

	// Token: 0x04000469 RID: 1129
	private Font mActiveTTF;

	// Token: 0x0400046A RID: 1130
	private bool mShouldBeProcessed = true;

	// Token: 0x0400046B RID: 1131
	private string mProcessedText;

	// Token: 0x0400046C RID: 1132
	private bool mPremultiply;

	// Token: 0x0400046D RID: 1133
	private Vector2 mCalculatedSize = Vector2.zero;

	// Token: 0x0400046E RID: 1134
	private float mScale = 1f;

	// Token: 0x0400046F RID: 1135
	private int mLastWidth;

	// Token: 0x04000470 RID: 1136
	private int mLastHeight;

	// Token: 0x04000471 RID: 1137
	private int mPrintedSize;

	// Token: 0x020000B1 RID: 177
	public enum Effect
	{
		// Token: 0x04000473 RID: 1139
		None,
		// Token: 0x04000474 RID: 1140
		Shadow,
		// Token: 0x04000475 RID: 1141
		Outline
	}

	// Token: 0x020000B2 RID: 178
	public enum Overflow
	{
		// Token: 0x04000477 RID: 1143
		ShrinkContent,
		// Token: 0x04000478 RID: 1144
		ClampContent,
		// Token: 0x04000479 RID: 1145
		ResizeFreely,
		// Token: 0x0400047A RID: 1146
		ResizeHeight
	}

	// Token: 0x020000B3 RID: 179
	public enum Crispness
	{
		// Token: 0x0400047C RID: 1148
		Never,
		// Token: 0x0400047D RID: 1149
		OnDesktop,
		// Token: 0x0400047E RID: 1150
		Always
	}
}
