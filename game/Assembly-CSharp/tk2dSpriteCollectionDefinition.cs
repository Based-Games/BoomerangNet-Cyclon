using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000260 RID: 608
[Serializable]
public class tk2dSpriteCollectionDefinition
{
	// Token: 0x060011E6 RID: 4582 RVA: 0x0007BF08 File Offset: 0x0007A108
	public void CopyFrom(tk2dSpriteCollectionDefinition src)
	{
		this.name = src.name;
		this.disableTrimming = src.disableTrimming;
		this.additive = src.additive;
		this.scale = src.scale;
		this.texture = src.texture;
		this.materialId = src.materialId;
		this.anchor = src.anchor;
		this.anchorX = src.anchorX;
		this.anchorY = src.anchorY;
		this.overrideMesh = src.overrideMesh;
		this.doubleSidedSprite = src.doubleSidedSprite;
		this.customSpriteGeometry = src.customSpriteGeometry;
		this.geometryIslands = src.geometryIslands;
		this.dice = src.dice;
		this.diceUnitX = src.diceUnitX;
		this.diceUnitY = src.diceUnitY;
		this.diceFilter = src.diceFilter;
		this.pad = src.pad;
		this.source = src.source;
		this.fromSpriteSheet = src.fromSpriteSheet;
		this.hasSpriteSheetId = src.hasSpriteSheetId;
		this.spriteSheetX = src.spriteSheetX;
		this.spriteSheetY = src.spriteSheetY;
		this.spriteSheetId = src.spriteSheetId;
		this.extractRegion = src.extractRegion;
		this.regionX = src.regionX;
		this.regionY = src.regionY;
		this.regionW = src.regionW;
		this.regionH = src.regionH;
		this.regionId = src.regionId;
		this.colliderType = src.colliderType;
		this.boxColliderMin = src.boxColliderMin;
		this.boxColliderMax = src.boxColliderMax;
		this.polyColliderCap = src.polyColliderCap;
		this.colliderColor = src.colliderColor;
		this.colliderConvex = src.colliderConvex;
		this.colliderSmoothSphereCollisions = src.colliderSmoothSphereCollisions;
		this.extraPadding = src.extraPadding;
		if (src.polyColliderIslands != null)
		{
			this.polyColliderIslands = new tk2dSpriteColliderIsland[src.polyColliderIslands.Length];
			for (int i = 0; i < this.polyColliderIslands.Length; i++)
			{
				this.polyColliderIslands[i] = new tk2dSpriteColliderIsland();
				this.polyColliderIslands[i].CopyFrom(src.polyColliderIslands[i]);
			}
		}
		else
		{
			this.polyColliderIslands = new tk2dSpriteColliderIsland[0];
		}
		if (src.geometryIslands != null)
		{
			this.geometryIslands = new tk2dSpriteColliderIsland[src.geometryIslands.Length];
			for (int j = 0; j < this.geometryIslands.Length; j++)
			{
				this.geometryIslands[j] = new tk2dSpriteColliderIsland();
				this.geometryIslands[j].CopyFrom(src.geometryIslands[j]);
			}
		}
		else
		{
			this.geometryIslands = new tk2dSpriteColliderIsland[0];
		}
		this.attachPoints = new List<tk2dSpriteDefinition.AttachPoint>(src.attachPoints.Count);
		foreach (tk2dSpriteDefinition.AttachPoint attachPoint in src.attachPoints)
		{
			tk2dSpriteDefinition.AttachPoint attachPoint2 = new tk2dSpriteDefinition.AttachPoint();
			attachPoint2.CopyFrom(attachPoint);
			this.attachPoints.Add(attachPoint2);
		}
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x0007C22C File Offset: 0x0007A42C
	public void Clear()
	{
		tk2dSpriteCollectionDefinition tk2dSpriteCollectionDefinition = new tk2dSpriteCollectionDefinition();
		this.CopyFrom(tk2dSpriteCollectionDefinition);
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0007C248 File Offset: 0x0007A448
	public bool CompareTo(tk2dSpriteCollectionDefinition src)
	{
		if (this.name != src.name)
		{
			return false;
		}
		if (this.additive != src.additive)
		{
			return false;
		}
		if (this.scale != src.scale)
		{
			return false;
		}
		if (this.texture != src.texture)
		{
			return false;
		}
		if (this.materialId != src.materialId)
		{
			return false;
		}
		if (this.anchor != src.anchor)
		{
			return false;
		}
		if (this.anchorX != src.anchorX)
		{
			return false;
		}
		if (this.anchorY != src.anchorY)
		{
			return false;
		}
		if (this.overrideMesh != src.overrideMesh)
		{
			return false;
		}
		if (this.dice != src.dice)
		{
			return false;
		}
		if (this.diceUnitX != src.diceUnitX)
		{
			return false;
		}
		if (this.diceUnitY != src.diceUnitY)
		{
			return false;
		}
		if (this.diceFilter != src.diceFilter)
		{
			return false;
		}
		if (this.pad != src.pad)
		{
			return false;
		}
		if (this.extraPadding != src.extraPadding)
		{
			return false;
		}
		if (this.doubleSidedSprite != src.doubleSidedSprite)
		{
			return false;
		}
		if (this.customSpriteGeometry != src.customSpriteGeometry)
		{
			return false;
		}
		if (this.geometryIslands != src.geometryIslands)
		{
			return false;
		}
		if (this.geometryIslands != null && src.geometryIslands != null)
		{
			if (this.geometryIslands.Length != src.geometryIslands.Length)
			{
				return false;
			}
			for (int i = 0; i < this.geometryIslands.Length; i++)
			{
				if (!this.geometryIslands[i].CompareTo(src.geometryIslands[i]))
				{
					return false;
				}
			}
		}
		if (this.source != src.source)
		{
			return false;
		}
		if (this.fromSpriteSheet != src.fromSpriteSheet)
		{
			return false;
		}
		if (this.hasSpriteSheetId != src.hasSpriteSheetId)
		{
			return false;
		}
		if (this.spriteSheetId != src.spriteSheetId)
		{
			return false;
		}
		if (this.spriteSheetX != src.spriteSheetX)
		{
			return false;
		}
		if (this.spriteSheetY != src.spriteSheetY)
		{
			return false;
		}
		if (this.extractRegion != src.extractRegion)
		{
			return false;
		}
		if (this.regionX != src.regionX)
		{
			return false;
		}
		if (this.regionY != src.regionY)
		{
			return false;
		}
		if (this.regionW != src.regionW)
		{
			return false;
		}
		if (this.regionH != src.regionH)
		{
			return false;
		}
		if (this.regionId != src.regionId)
		{
			return false;
		}
		if (this.colliderType != src.colliderType)
		{
			return false;
		}
		if (this.boxColliderMin != src.boxColliderMin)
		{
			return false;
		}
		if (this.boxColliderMax != src.boxColliderMax)
		{
			return false;
		}
		if (this.polyColliderIslands != src.polyColliderIslands)
		{
			return false;
		}
		if (this.polyColliderIslands != null && src.polyColliderIslands != null)
		{
			if (this.polyColliderIslands.Length != src.polyColliderIslands.Length)
			{
				return false;
			}
			for (int j = 0; j < this.polyColliderIslands.Length; j++)
			{
				if (!this.polyColliderIslands[j].CompareTo(src.polyColliderIslands[j]))
				{
					return false;
				}
			}
		}
		if (this.polyColliderCap != src.polyColliderCap)
		{
			return false;
		}
		if (this.colliderColor != src.colliderColor)
		{
			return false;
		}
		if (this.colliderSmoothSphereCollisions != src.colliderSmoothSphereCollisions)
		{
			return false;
		}
		if (this.colliderConvex != src.colliderConvex)
		{
			return false;
		}
		if (this.attachPoints.Count != src.attachPoints.Count)
		{
			return false;
		}
		for (int k = 0; k < this.attachPoints.Count; k++)
		{
			if (!this.attachPoints[k].CompareTo(src.attachPoints[k]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400131F RID: 4895
	public string name = string.Empty;

	// Token: 0x04001320 RID: 4896
	public bool disableTrimming;

	// Token: 0x04001321 RID: 4897
	public bool additive;

	// Token: 0x04001322 RID: 4898
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04001323 RID: 4899
	public Texture2D texture;

	// Token: 0x04001324 RID: 4900
	[NonSerialized]
	public Texture2D thumbnailTexture;

	// Token: 0x04001325 RID: 4901
	public int materialId;

	// Token: 0x04001326 RID: 4902
	public tk2dSpriteCollectionDefinition.Anchor anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;

	// Token: 0x04001327 RID: 4903
	public float anchorX;

	// Token: 0x04001328 RID: 4904
	public float anchorY;

	// Token: 0x04001329 RID: 4905
	public UnityEngine.Object overrideMesh;

	// Token: 0x0400132A RID: 4906
	public bool doubleSidedSprite;

	// Token: 0x0400132B RID: 4907
	public bool customSpriteGeometry;

	// Token: 0x0400132C RID: 4908
	public tk2dSpriteColliderIsland[] geometryIslands = new tk2dSpriteColliderIsland[0];

	// Token: 0x0400132D RID: 4909
	public bool dice;

	// Token: 0x0400132E RID: 4910
	public int diceUnitX = 64;

	// Token: 0x0400132F RID: 4911
	public int diceUnitY = 64;

	// Token: 0x04001330 RID: 4912
	public tk2dSpriteCollectionDefinition.DiceFilter diceFilter;

	// Token: 0x04001331 RID: 4913
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04001332 RID: 4914
	public int extraPadding;

	// Token: 0x04001333 RID: 4915
	public tk2dSpriteCollectionDefinition.Source source;

	// Token: 0x04001334 RID: 4916
	public bool fromSpriteSheet;

	// Token: 0x04001335 RID: 4917
	public bool hasSpriteSheetId;

	// Token: 0x04001336 RID: 4918
	public int spriteSheetId;

	// Token: 0x04001337 RID: 4919
	public int spriteSheetX;

	// Token: 0x04001338 RID: 4920
	public int spriteSheetY;

	// Token: 0x04001339 RID: 4921
	public bool extractRegion;

	// Token: 0x0400133A RID: 4922
	public int regionX;

	// Token: 0x0400133B RID: 4923
	public int regionY;

	// Token: 0x0400133C RID: 4924
	public int regionW;

	// Token: 0x0400133D RID: 4925
	public int regionH;

	// Token: 0x0400133E RID: 4926
	public int regionId;

	// Token: 0x0400133F RID: 4927
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;

	// Token: 0x04001340 RID: 4928
	public Vector2 boxColliderMin;

	// Token: 0x04001341 RID: 4929
	public Vector2 boxColliderMax;

	// Token: 0x04001342 RID: 4930
	public tk2dSpriteColliderIsland[] polyColliderIslands;

	// Token: 0x04001343 RID: 4931
	public tk2dSpriteCollectionDefinition.PolygonColliderCap polyColliderCap = tk2dSpriteCollectionDefinition.PolygonColliderCap.FrontAndBack;

	// Token: 0x04001344 RID: 4932
	public bool colliderConvex;

	// Token: 0x04001345 RID: 4933
	public bool colliderSmoothSphereCollisions;

	// Token: 0x04001346 RID: 4934
	public tk2dSpriteCollectionDefinition.ColliderColor colliderColor;

	// Token: 0x04001347 RID: 4935
	public List<tk2dSpriteDefinition.AttachPoint> attachPoints = new List<tk2dSpriteDefinition.AttachPoint>();

	// Token: 0x02000261 RID: 609
	public enum Anchor
	{
		// Token: 0x04001349 RID: 4937
		UpperLeft,
		// Token: 0x0400134A RID: 4938
		UpperCenter,
		// Token: 0x0400134B RID: 4939
		UpperRight,
		// Token: 0x0400134C RID: 4940
		MiddleLeft,
		// Token: 0x0400134D RID: 4941
		MiddleCenter,
		// Token: 0x0400134E RID: 4942
		MiddleRight,
		// Token: 0x0400134F RID: 4943
		LowerLeft,
		// Token: 0x04001350 RID: 4944
		LowerCenter,
		// Token: 0x04001351 RID: 4945
		LowerRight,
		// Token: 0x04001352 RID: 4946
		Custom
	}

	// Token: 0x02000262 RID: 610
	public enum Pad
	{
		// Token: 0x04001354 RID: 4948
		Default,
		// Token: 0x04001355 RID: 4949
		BlackZeroAlpha,
		// Token: 0x04001356 RID: 4950
		Extend,
		// Token: 0x04001357 RID: 4951
		TileXY
	}

	// Token: 0x02000263 RID: 611
	public enum ColliderType
	{
		// Token: 0x04001359 RID: 4953
		UserDefined,
		// Token: 0x0400135A RID: 4954
		ForceNone,
		// Token: 0x0400135B RID: 4955
		BoxTrimmed,
		// Token: 0x0400135C RID: 4956
		BoxCustom,
		// Token: 0x0400135D RID: 4957
		Polygon
	}

	// Token: 0x02000264 RID: 612
	public enum PolygonColliderCap
	{
		// Token: 0x0400135F RID: 4959
		None,
		// Token: 0x04001360 RID: 4960
		FrontAndBack,
		// Token: 0x04001361 RID: 4961
		Front,
		// Token: 0x04001362 RID: 4962
		Back
	}

	// Token: 0x02000265 RID: 613
	public enum ColliderColor
	{
		// Token: 0x04001364 RID: 4964
		Default,
		// Token: 0x04001365 RID: 4965
		Red,
		// Token: 0x04001366 RID: 4966
		White,
		// Token: 0x04001367 RID: 4967
		Black
	}

	// Token: 0x02000266 RID: 614
	public enum Source
	{
		// Token: 0x04001369 RID: 4969
		Sprite,
		// Token: 0x0400136A RID: 4970
		SpriteSheet,
		// Token: 0x0400136B RID: 4971
		Font
	}

	// Token: 0x02000267 RID: 615
	public enum DiceFilter
	{
		// Token: 0x0400136D RID: 4973
		Complete,
		// Token: 0x0400136E RID: 4974
		SolidOnly,
		// Token: 0x0400136F RID: 4975
		TransparentOnly
	}
}
