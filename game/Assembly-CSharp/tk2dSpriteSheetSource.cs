using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
[Serializable]
public class tk2dSpriteSheetSource
{
	// Token: 0x060011EB RID: 4587 RVA: 0x0007C668 File Offset: 0x0007A868
	public void CopyFrom(tk2dSpriteSheetSource src)
	{
		this.texture = src.texture;
		this.tilesX = src.tilesX;
		this.tilesY = src.tilesY;
		this.numTiles = src.numTiles;
		this.anchor = src.anchor;
		this.pad = src.pad;
		this.scale = src.scale;
		this.colliderType = src.colliderType;
		this.version = src.version;
		this.active = src.active;
		this.tileWidth = src.tileWidth;
		this.tileHeight = src.tileHeight;
		this.tileSpacingX = src.tileSpacingX;
		this.tileSpacingY = src.tileSpacingY;
		this.tileMarginX = src.tileMarginX;
		this.tileMarginY = src.tileMarginY;
		this.splitMethod = src.splitMethod;
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x0007C744 File Offset: 0x0007A944
	public bool CompareTo(tk2dSpriteSheetSource src)
	{
		return !(this.texture != src.texture) && this.tilesX == src.tilesX && this.tilesY == src.tilesY && this.numTiles == src.numTiles && this.anchor == src.anchor && this.pad == src.pad && !(this.scale != src.scale) && this.colliderType == src.colliderType && this.version == src.version && this.active == src.active && this.tileWidth == src.tileWidth && this.tileHeight == src.tileHeight && this.tileSpacingX == src.tileSpacingX && this.tileSpacingY == src.tileSpacingY && this.tileMarginX == src.tileMarginX && this.tileMarginY == src.tileMarginY && this.splitMethod == src.splitMethod;
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x060011ED RID: 4589 RVA: 0x0000F4B6 File Offset: 0x0000D6B6
	public string Name
	{
		get
		{
			return (!(this.texture != null)) ? "New Sprite Sheet" : this.texture.name;
		}
	}

	// Token: 0x04001375 RID: 4981
	public const int CURRENT_VERSION = 1;

	// Token: 0x04001376 RID: 4982
	public Texture2D texture;

	// Token: 0x04001377 RID: 4983
	public int tilesX;

	// Token: 0x04001378 RID: 4984
	public int tilesY;

	// Token: 0x04001379 RID: 4985
	public int numTiles;

	// Token: 0x0400137A RID: 4986
	public tk2dSpriteSheetSource.Anchor anchor = tk2dSpriteSheetSource.Anchor.MiddleCenter;

	// Token: 0x0400137B RID: 4987
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x0400137C RID: 4988
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x0400137D RID: 4989
	public bool additive;

	// Token: 0x0400137E RID: 4990
	public bool active;

	// Token: 0x0400137F RID: 4991
	public int tileWidth;

	// Token: 0x04001380 RID: 4992
	public int tileHeight;

	// Token: 0x04001381 RID: 4993
	public int tileMarginX;

	// Token: 0x04001382 RID: 4994
	public int tileMarginY;

	// Token: 0x04001383 RID: 4995
	public int tileSpacingX;

	// Token: 0x04001384 RID: 4996
	public int tileSpacingY;

	// Token: 0x04001385 RID: 4997
	public tk2dSpriteSheetSource.SplitMethod splitMethod;

	// Token: 0x04001386 RID: 4998
	public int version;

	// Token: 0x04001387 RID: 4999
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;

	// Token: 0x0200026A RID: 618
	public enum Anchor
	{
		// Token: 0x04001389 RID: 5001
		UpperLeft,
		// Token: 0x0400138A RID: 5002
		UpperCenter,
		// Token: 0x0400138B RID: 5003
		UpperRight,
		// Token: 0x0400138C RID: 5004
		MiddleLeft,
		// Token: 0x0400138D RID: 5005
		MiddleCenter,
		// Token: 0x0400138E RID: 5006
		MiddleRight,
		// Token: 0x0400138F RID: 5007
		LowerLeft,
		// Token: 0x04001390 RID: 5008
		LowerCenter,
		// Token: 0x04001391 RID: 5009
		LowerRight
	}

	// Token: 0x0200026B RID: 619
	public enum SplitMethod
	{
		// Token: 0x04001393 RID: 5011
		UniformDivision
	}
}
