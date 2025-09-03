using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Sprite")]
public class UISprite : UIWidget
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000626 RID: 1574 RVA: 0x00007EFC File Offset: 0x000060FC
	// (set) Token: 0x06000627 RID: 1575 RVA: 0x00007F04 File Offset: 0x00006104
	public virtual UISprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000628 RID: 1576 RVA: 0x00007F1F File Offset: 0x0000611F
	public override Material material
	{
		get
		{
			return (!(this.mAtlas != null)) ? null : this.mAtlas.spriteMaterial;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000629 RID: 1577 RVA: 0x00007F43 File Offset: 0x00006143
	// (set) Token: 0x0600062A RID: 1578 RVA: 0x0002FFF8 File Offset: 0x0002E1F8
	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string text = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = text;
					this.MarkAsChanged();
				}
				UIPanel.RebuildAllDrawCalls(false);
			}
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x0600062B RID: 1579 RVA: 0x00007F4B File Offset: 0x0000614B
	// (set) Token: 0x0600062C RID: 1580 RVA: 0x000300C8 File Offset: 0x0002E2C8
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(this.mSpriteName))
				{
					return;
				}
				this.mSpriteName = string.Empty;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600062D RID: 1581 RVA: 0x00007F53 File Offset: 0x00006153
	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x00007F61 File Offset: 0x00006161
	// (set) Token: 0x0600062F RID: 1583 RVA: 0x00007F69 File Offset: 0x00006169
	public bool fillCenter
	{
		get
		{
			return this.mFillCenter;
		}
		set
		{
			if (this.mFillCenter != value)
			{
				this.mFillCenter = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x00007F84 File Offset: 0x00006184
	// (set) Token: 0x06000631 RID: 1585 RVA: 0x00007F8C File Offset: 0x0000618C
	public UISprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x00007FA8 File Offset: 0x000061A8
	// (set) Token: 0x06000633 RID: 1587 RVA: 0x00030144 File Offset: 0x0002E344
	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000634 RID: 1588 RVA: 0x00007FB0 File Offset: 0x000061B0
	// (set) Token: 0x06000635 RID: 1589 RVA: 0x00007FB8 File Offset: 0x000061B8
	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000636 RID: 1590 RVA: 0x00030174 File Offset: 0x0002E374
	public override Vector4 border
	{
		get
		{
			if (this.type != UISprite.Type.Sliced)
			{
				return base.border;
			}
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return Vector2.zero;
			}
			return new Vector4((float)atlasSprite.borderLeft, (float)atlasSprite.borderBottom, (float)atlasSprite.borderRight, (float)atlasSprite.borderTop);
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x000301D0 File Offset: 0x0002E3D0
	public override int minWidth
	{
		get
		{
			if (this.type == UISprite.Type.Sliced)
			{
				Vector4 vector = this.border;
				if (this.atlas != null)
				{
					vector *= this.atlas.pixelSize;
				}
				int num = Mathf.RoundToInt(vector.x + vector.z);
				return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minWidth;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000638 RID: 1592 RVA: 0x0003024C File Offset: 0x0002E44C
	public override int minHeight
	{
		get
		{
			if (this.type == UISprite.Type.Sliced)
			{
				Vector4 vector = this.border;
				if (this.atlas != null)
				{
					vector *= this.atlas.pixelSize;
				}
				int num = Mathf.RoundToInt(vector.y + vector.w);
				return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minHeight;
		}
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x000302C8 File Offset: 0x0002E4C8
	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData uispriteData = this.mAtlas.spriteList[0];
				if (uispriteData == null)
				{
					return null;
				}
				this.SetAtlasSprite(uispriteData);
				if (this.mSprite == null)
				{
					Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x000303B4 File Offset: 0x0002E5B4
	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
		}
		else
		{
			this.mSpriteName = ((this.mSprite == null) ? string.Empty : this.mSprite.name);
			this.mSprite = sp;
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00030420 File Offset: 0x0002E620
	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		base.MakePixelPerfect();
		UISprite.Type type = this.type;
		if (type == UISprite.Type.Simple || type == UISprite.Type.Filled)
		{
			Texture mainTexture = this.mainTexture;
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (mainTexture != null && atlasSprite != null)
			{
				int num = Mathf.RoundToInt(this.atlas.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
				int num2 = Mathf.RoundToInt(this.atlas.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				base.width = num;
				base.height = num2;
			}
		}
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00007FD4 File Offset: 0x000061D4
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x000304F0 File Offset: 0x0002E6F0
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture != null)
		{
			if (this.mSprite == null)
			{
				this.mSprite = this.atlas.GetSprite(this.spriteName);
			}
			if (this.mSprite == null)
			{
				return;
			}
			this.mOuterUV.Set((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			this.mInnerUV.Set((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
			this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, mainTexture.width, mainTexture.height);
			this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, mainTexture.width, mainTexture.height);
		}
		switch (this.type)
		{
		case UISprite.Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			break;
		case UISprite.Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			break;
		case UISprite.Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			break;
		case UISprite.Type.Filled:
			this.FilledFill(verts, uvs, cols);
			break;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x00030690 File Offset: 0x0002E890
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.mSprite != null)
			{
				int paddingLeft = this.mSprite.paddingLeft;
				int paddingBottom = this.mSprite.paddingBottom;
				int num5 = this.mSprite.paddingRight;
				int num6 = this.mSprite.paddingTop;
				int num7 = this.mSprite.width + paddingLeft + num5;
				int num8 = this.mSprite.height + paddingBottom + num6;
				if (this.mType != UISprite.Type.Sliced && this.mType != UISprite.Type.Tiled)
				{
					if ((num7 & 1) != 0)
					{
						num5++;
					}
					if ((num8 & 1) != 0)
					{
						num6++;
					}
					float num9 = 1f / (float)num7 * (float)this.mWidth;
					float num10 = 1f / (float)num8 * (float)this.mHeight;
					num += (float)paddingLeft * num9;
					num3 -= (float)num5 * num9;
					num2 += (float)paddingBottom * num10;
					num4 -= (float)num6 * num10;
				}
				else
				{
					num += (float)paddingLeft;
					num3 -= (float)num5;
					num2 += (float)paddingBottom;
					num4 -= (float)num6;
				}
			}
			Vector4 vector = new Vector4((this.mDrawRegion.x != 0f) ? Mathf.Lerp(num, num3, this.mDrawRegion.x) : num, (this.mDrawRegion.y != 0f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.y) : num2, (this.mDrawRegion.z != 1f) ? Mathf.Lerp(num, num3, this.mDrawRegion.z) : num3, (this.mDrawRegion.w != 1f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.w) : num4);
			float num11 = (float)this.minWidth;
			float num12 = (float)this.minHeight;
			if (vector.z - vector.x < num11)
			{
				float num13 = (vector.x + vector.z) * 0.5f;
				vector.x = Mathf.Round(num13 - num11 * 0.5f);
				vector.z = vector.x + num11;
			}
			if (vector.w - vector.y < num12)
			{
				float num14 = (vector.y + vector.w) * 0.5f;
				vector.y = Mathf.Round(num14 - num12 * 0.5f);
				vector.w = vector.y + num11;
			}
			return vector;
		}
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00030960 File Offset: 0x0002EB60
	protected void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector2 vector = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMin);
		Vector2 vector2 = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMax);
		Vector4 drawingDimensions = this.drawingDimensions;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(vector);
		uvs.Add(new Vector2(vector.x, vector2.y));
		uvs.Add(vector2);
		uvs.Add(new Vector2(vector2.x, vector.y));
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 color2 = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		cols.Add(color2);
		cols.Add(color2);
		cols.Add(color2);
		cols.Add(color2);
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00030AAC File Offset: 0x0002ECAC
	protected void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mSprite == null)
		{
			return;
		}
		if (!this.mSprite.hasBorder)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector = this.border * this.atlas.pixelSize;
		UISprite.mTempPos[0].x = drawingDimensions.x;
		UISprite.mTempPos[0].y = drawingDimensions.y;
		UISprite.mTempPos[3].x = drawingDimensions.z;
		UISprite.mTempPos[3].y = drawingDimensions.w;
		UISprite.mTempPos[1].x = UISprite.mTempPos[0].x + vector.x;
		UISprite.mTempPos[1].y = UISprite.mTempPos[0].y + vector.y;
		UISprite.mTempPos[2].x = UISprite.mTempPos[3].x - vector.z;
		UISprite.mTempPos[2].y = UISprite.mTempPos[3].y - vector.w;
		UISprite.mTempUVs[0] = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMin);
		UISprite.mTempUVs[1] = new Vector2(this.mInnerUV.xMin, this.mInnerUV.yMin);
		UISprite.mTempUVs[2] = new Vector2(this.mInnerUV.xMax, this.mInnerUV.yMax);
		UISprite.mTempUVs[3] = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMax);
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 color2 = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.mFillCenter || i != 1 || j != 1)
				{
					int num2 = j + 1;
					verts.Add(new Vector3(UISprite.mTempPos[i].x, UISprite.mTempPos[j].y));
					verts.Add(new Vector3(UISprite.mTempPos[i].x, UISprite.mTempPos[num2].y));
					verts.Add(new Vector3(UISprite.mTempPos[num].x, UISprite.mTempPos[num2].y));
					verts.Add(new Vector3(UISprite.mTempPos[num].x, UISprite.mTempPos[j].y));
					uvs.Add(new Vector2(UISprite.mTempUVs[i].x, UISprite.mTempUVs[j].y));
					uvs.Add(new Vector2(UISprite.mTempUVs[i].x, UISprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UISprite.mTempUVs[num].x, UISprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UISprite.mTempUVs[num].x, UISprite.mTempUVs[j].y));
					cols.Add(color2);
					cols.Add(color2);
					cols.Add(color2);
					cols.Add(color2);
				}
			}
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00030EB8 File Offset: 0x0002F0B8
	protected void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.material.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector2 vector = new Vector2(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		vector *= this.atlas.pixelSize;
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 color2 = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		float num = drawingDimensions.x;
		float num2 = drawingDimensions.y;
		float xMin = this.mInnerUV.xMin;
		float yMin = this.mInnerUV.yMin;
		while (num2 < drawingDimensions.w)
		{
			num = drawingDimensions.x;
			float num3 = num2 + vector.y;
			float num4 = this.mInnerUV.yMax;
			if (num3 > drawingDimensions.w)
			{
				num4 = Mathf.Lerp(this.mInnerUV.yMin, this.mInnerUV.yMax, (drawingDimensions.w - num2) / vector.y);
				num3 = drawingDimensions.w;
			}
			while (num < drawingDimensions.z)
			{
				float num5 = num + vector.x;
				float num6 = this.mInnerUV.xMax;
				if (num5 > drawingDimensions.z)
				{
					num6 = Mathf.Lerp(this.mInnerUV.xMin, this.mInnerUV.xMax, (drawingDimensions.z - num) / vector.x);
					num5 = drawingDimensions.z;
				}
				verts.Add(new Vector3(num, num2));
				verts.Add(new Vector3(num, num3));
				verts.Add(new Vector3(num5, num3));
				verts.Add(new Vector3(num5, num2));
				uvs.Add(new Vector2(xMin, yMin));
				uvs.Add(new Vector2(xMin, num4));
				uvs.Add(new Vector2(num6, num4));
				uvs.Add(new Vector2(num6, yMin));
				cols.Add(color2);
				cols.Add(color2);
				cols.Add(color2);
				cols.Add(color2);
				num += vector.x;
			}
			num2 += vector.y;
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00031124 File Offset: 0x0002F324
	protected void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 color2 = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		Vector4 drawingDimensions = this.drawingDimensions;
		float num = this.mOuterUV.xMin;
		float num2 = this.mOuterUV.yMin;
		float num3 = this.mOuterUV.xMax;
		float num4 = this.mOuterUV.yMax;
		if (this.mFillDirection == UISprite.FillDirection.Horizontal || this.mFillDirection == UISprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UISprite.FillDirection.Horizontal)
			{
				float num5 = (num3 - num) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					num = num3 - num5;
				}
				else
				{
					drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					num3 = num + num5;
				}
			}
			else if (this.mFillDirection == UISprite.FillDirection.Vertical)
			{
				float num6 = (num4 - num2) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					num2 = num4 - num6;
				}
				else
				{
					drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					num4 = num2 + num6;
				}
			}
		}
		UISprite.mTempPos[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
		UISprite.mTempPos[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
		UISprite.mTempPos[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
		UISprite.mTempPos[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
		UISprite.mTempUVs[0] = new Vector2(num, num2);
		UISprite.mTempUVs[1] = new Vector2(num, num4);
		UISprite.mTempUVs[2] = new Vector2(num3, num4);
		UISprite.mTempUVs[3] = new Vector2(num3, num2);
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UISprite.FillDirection.Radial90)
			{
				if (UISprite.RadialCut(UISprite.mTempPos, UISprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(UISprite.mTempPos[i]);
						uvs.Add(UISprite.mTempUVs[i]);
						cols.Add(color2);
					}
				}
				return;
			}
			if (this.mFillDirection == UISprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float num7 = 0f;
					float num8 = 1f;
					float num9;
					float num10;
					if (j == 0)
					{
						num9 = 0f;
						num10 = 0.5f;
					}
					else
					{
						num9 = 0.5f;
						num10 = 1f;
					}
					UISprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num9);
					UISprite.mTempPos[1].x = UISprite.mTempPos[0].x;
					UISprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num10);
					UISprite.mTempPos[3].x = UISprite.mTempPos[2].x;
					UISprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num7);
					UISprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num8);
					UISprite.mTempPos[2].y = UISprite.mTempPos[1].y;
					UISprite.mTempPos[3].y = UISprite.mTempPos[0].y;
					UISprite.mTempUVs[0].x = Mathf.Lerp(num, num3, num9);
					UISprite.mTempUVs[1].x = UISprite.mTempUVs[0].x;
					UISprite.mTempUVs[2].x = Mathf.Lerp(num, num3, num10);
					UISprite.mTempUVs[3].x = UISprite.mTempUVs[2].x;
					UISprite.mTempUVs[0].y = Mathf.Lerp(num2, num4, num7);
					UISprite.mTempUVs[1].y = Mathf.Lerp(num2, num4, num8);
					UISprite.mTempUVs[2].y = UISprite.mTempUVs[1].y;
					UISprite.mTempUVs[3].y = UISprite.mTempUVs[0].y;
					float num11 = (this.mInvert ? (this.mFillAmount * 2f - (float)(1 - j)) : (this.fillAmount * 2f - (float)j));
					if (UISprite.RadialCut(UISprite.mTempPos, UISprite.mTempUVs, Mathf.Clamp01(num11), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(UISprite.mTempPos[k]);
							uvs.Add(UISprite.mTempUVs[k]);
							cols.Add(color2);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UISprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					float num12;
					float num13;
					if (l < 2)
					{
						num12 = 0f;
						num13 = 0.5f;
					}
					else
					{
						num12 = 0.5f;
						num13 = 1f;
					}
					float num14;
					float num15;
					if (l == 0 || l == 3)
					{
						num14 = 0f;
						num15 = 0.5f;
					}
					else
					{
						num14 = 0.5f;
						num15 = 1f;
					}
					UISprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num12);
					UISprite.mTempPos[1].x = UISprite.mTempPos[0].x;
					UISprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num13);
					UISprite.mTempPos[3].x = UISprite.mTempPos[2].x;
					UISprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num14);
					UISprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num15);
					UISprite.mTempPos[2].y = UISprite.mTempPos[1].y;
					UISprite.mTempPos[3].y = UISprite.mTempPos[0].y;
					UISprite.mTempUVs[0].x = Mathf.Lerp(num, num3, num12);
					UISprite.mTempUVs[1].x = UISprite.mTempUVs[0].x;
					UISprite.mTempUVs[2].x = Mathf.Lerp(num, num3, num13);
					UISprite.mTempUVs[3].x = UISprite.mTempUVs[2].x;
					UISprite.mTempUVs[0].y = Mathf.Lerp(num2, num4, num14);
					UISprite.mTempUVs[1].y = Mathf.Lerp(num2, num4, num15);
					UISprite.mTempUVs[2].y = UISprite.mTempUVs[1].y;
					UISprite.mTempUVs[3].y = UISprite.mTempUVs[0].y;
					float num16 = ((!this.mInvert) ? (this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4))) : (this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4)));
					if (UISprite.RadialCut(UISprite.mTempPos, UISprite.mTempUVs, Mathf.Clamp01(num16), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(UISprite.mTempPos[m]);
							uvs.Add(UISprite.mTempUVs[m]);
							cols.Add(color2);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(UISprite.mTempPos[n]);
			uvs.Add(UISprite.mTempUVs[n]);
			cols.Add(color2);
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00031ADC File Offset: 0x0002FCDC
	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (invert)
		{
			num = 1f - num;
		}
		num *= 1.5707964f;
		float num2 = Mathf.Cos(num);
		float num3 = Mathf.Sin(num);
		UISprite.RadialCut(xy, num2, num3, invert, corner);
		UISprite.RadialCut(uv, num2, num3, invert, corner);
		return true;
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00031B5C File Offset: 0x0002FD5C
	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) == 1)
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num2].x = xy[num].x;
				}
			}
			else if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (!invert)
			{
				xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
			else
			{
				xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
		}
		else
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
			else
			{
				xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
		}
	}

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	[HideInInspector]
	private UIAtlas mAtlas;

	// Token: 0x040004B8 RID: 1208
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x040004B9 RID: 1209
	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	// Token: 0x040004BA RID: 1210
	[SerializeField]
	[HideInInspector]
	private UISprite.Type mType;

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	[HideInInspector]
	private UISprite.FillDirection mFillDirection = UISprite.FillDirection.Radial360;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	[HideInInspector]
	[Range(0f, 1f)]
	private float mFillAmount = 1f;

	// Token: 0x040004BD RID: 1213
	[HideInInspector]
	[SerializeField]
	private bool mInvert;

	// Token: 0x040004BE RID: 1214
	protected UISpriteData mSprite;

	// Token: 0x040004BF RID: 1215
	protected Rect mInnerUV = default(Rect);

	// Token: 0x040004C0 RID: 1216
	protected Rect mOuterUV = default(Rect);

	// Token: 0x040004C1 RID: 1217
	private bool mSpriteSet;

	// Token: 0x040004C2 RID: 1218
	private static Vector2[] mTempPos = new Vector2[4];

	// Token: 0x040004C3 RID: 1219
	private static Vector2[] mTempUVs = new Vector2[4];

	// Token: 0x020000BD RID: 189
	public enum Type
	{
		// Token: 0x040004C5 RID: 1221
		Simple,
		// Token: 0x040004C6 RID: 1222
		Sliced,
		// Token: 0x040004C7 RID: 1223
		Tiled,
		// Token: 0x040004C8 RID: 1224
		Filled
	}

	// Token: 0x020000BE RID: 190
	public enum FillDirection
	{
		// Token: 0x040004CA RID: 1226
		Horizontal,
		// Token: 0x040004CB RID: 1227
		Vertical,
		// Token: 0x040004CC RID: 1228
		Radial90,
		// Token: 0x040004CD RID: 1229
		Radial180,
		// Token: 0x040004CE RID: 1230
		Radial360
	}
}
