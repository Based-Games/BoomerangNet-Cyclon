using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
[Serializable]
public class BMSymbol
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600027E RID: 638 RVA: 0x000054B3 File Offset: 0x000036B3
	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600027F RID: 639 RVA: 0x000054D7 File Offset: 0x000036D7
	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000280 RID: 640 RVA: 0x000054DF File Offset: 0x000036DF
	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000281 RID: 641 RVA: 0x000054E7 File Offset: 0x000036E7
	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000282 RID: 642 RVA: 0x000054EF File Offset: 0x000036EF
	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000283 RID: 643 RVA: 0x000054F7 File Offset: 0x000036F7
	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000284 RID: 644 RVA: 0x000054FF File Offset: 0x000036FF
	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00005507 File Offset: 0x00003707
	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0001D998 File Offset: 0x0001BB98
	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((!(atlas != null)) ? null : atlas.GetSprite(this.spriteName));
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}

	// Token: 0x04000289 RID: 649
	public string sequence;

	// Token: 0x0400028A RID: 650
	public string spriteName;

	// Token: 0x0400028B RID: 651
	private UISpriteData mSprite;

	// Token: 0x0400028C RID: 652
	private bool mIsValid;

	// Token: 0x0400028D RID: 653
	private int mLength;

	// Token: 0x0400028E RID: 654
	private int mOffsetX;

	// Token: 0x0400028F RID: 655
	private int mOffsetY;

	// Token: 0x04000290 RID: 656
	private int mWidth;

	// Token: 0x04000291 RID: 657
	private int mHeight;

	// Token: 0x04000292 RID: 658
	private int mAdvance;

	// Token: 0x04000293 RID: 659
	private Rect mUV;
}
