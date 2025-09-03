using System;
using UnityEngine;

// Token: 0x0200027C RID: 636
[Serializable]
public class tk2dBatchedSprite
{
	// Token: 0x0600123F RID: 4671 RVA: 0x0007F694 File Offset: 0x0007D894
	public tk2dBatchedSprite()
	{
		this.parentId = -1;
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06001240 RID: 4672 RVA: 0x0000F960 File Offset: 0x0000DB60
	// (set) Token: 0x06001241 RID: 4673 RVA: 0x0000F96D File Offset: 0x0000DB6D
	public float BoxColliderOffsetZ
	{
		get
		{
			return this.colliderData.x;
		}
		set
		{
			this.colliderData.x = value;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06001242 RID: 4674 RVA: 0x0000F97B File Offset: 0x0000DB7B
	// (set) Token: 0x06001243 RID: 4675 RVA: 0x0000F988 File Offset: 0x0000DB88
	public float BoxColliderExtentZ
	{
		get
		{
			return this.colliderData.y;
		}
		set
		{
			this.colliderData.y = value;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06001244 RID: 4676 RVA: 0x0000F996 File Offset: 0x0000DB96
	// (set) Token: 0x06001245 RID: 4677 RVA: 0x0000F99E File Offset: 0x0000DB9E
	public string FormattedText
	{
		get
		{
			return this.formattedText;
		}
		set
		{
			this.formattedText = value;
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06001246 RID: 4678 RVA: 0x0000F9A7 File Offset: 0x0000DBA7
	// (set) Token: 0x06001247 RID: 4679 RVA: 0x0000F9AF File Offset: 0x0000DBAF
	public Vector2 ClippedSpriteRegionBottomLeft
	{
		get
		{
			return this.internalData0;
		}
		set
		{
			this.internalData0 = value;
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06001248 RID: 4680 RVA: 0x0000F9B8 File Offset: 0x0000DBB8
	// (set) Token: 0x06001249 RID: 4681 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
	public Vector2 ClippedSpriteRegionTopRight
	{
		get
		{
			return this.internalData1;
		}
		set
		{
			this.internalData1 = value;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x0600124A RID: 4682 RVA: 0x0000F9A7 File Offset: 0x0000DBA7
	// (set) Token: 0x0600124B RID: 4683 RVA: 0x0000F9AF File Offset: 0x0000DBAF
	public Vector2 SlicedSpriteBorderBottomLeft
	{
		get
		{
			return this.internalData0;
		}
		set
		{
			this.internalData0 = value;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600124C RID: 4684 RVA: 0x0000F9B8 File Offset: 0x0000DBB8
	// (set) Token: 0x0600124D RID: 4685 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
	public Vector2 SlicedSpriteBorderTopRight
	{
		get
		{
			return this.internalData1;
		}
		set
		{
			this.internalData1 = value;
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x0600124E RID: 4686 RVA: 0x0000F9C9 File Offset: 0x0000DBC9
	// (set) Token: 0x0600124F RID: 4687 RVA: 0x0000F9D1 File Offset: 0x0000DBD1
	public Vector2 Dimensions
	{
		get
		{
			return this.internalData2;
		}
		set
		{
			this.internalData2 = value;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06001250 RID: 4688 RVA: 0x0000F9DA File Offset: 0x0000DBDA
	public bool IsDrawn
	{
		get
		{
			return this.type != tk2dBatchedSprite.Type.EmptyGameObject;
		}
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x0000F9E8 File Offset: 0x0000DBE8
	public bool CheckFlag(tk2dBatchedSprite.Flags mask)
	{
		return (this.flags & mask) != tk2dBatchedSprite.Flags.None;
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
	public void SetFlag(tk2dBatchedSprite.Flags mask, bool value)
	{
		if (value)
		{
			this.flags |= mask;
		}
		else
		{
			this.flags &= ~mask;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06001253 RID: 4691 RVA: 0x0000FA22 File Offset: 0x0000DC22
	// (set) Token: 0x06001254 RID: 4692 RVA: 0x0000FA2A File Offset: 0x0000DC2A
	public Vector3 CachedBoundsCenter
	{
		get
		{
			return this.cachedBoundsCenter;
		}
		set
		{
			this.cachedBoundsCenter = value;
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06001255 RID: 4693 RVA: 0x0000FA33 File Offset: 0x0000DC33
	// (set) Token: 0x06001256 RID: 4694 RVA: 0x0000FA3B File Offset: 0x0000DC3B
	public Vector3 CachedBoundsExtents
	{
		get
		{
			return this.cachedBoundsExtents;
		}
		set
		{
			this.cachedBoundsExtents = value;
		}
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x0000FA44 File Offset: 0x0000DC44
	public tk2dSpriteDefinition GetSpriteDefinition()
	{
		if (this.spriteCollection != null && this.spriteId != -1)
		{
			return this.spriteCollection.inst.spriteDefinitions[this.spriteId];
		}
		return null;
	}

	// Token: 0x04001430 RID: 5168
	public tk2dBatchedSprite.Type type = tk2dBatchedSprite.Type.Sprite;

	// Token: 0x04001431 RID: 5169
	public string name = string.Empty;

	// Token: 0x04001432 RID: 5170
	public int parentId = -1;

	// Token: 0x04001433 RID: 5171
	public int spriteId;

	// Token: 0x04001434 RID: 5172
	public int xRefId = -1;

	// Token: 0x04001435 RID: 5173
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04001436 RID: 5174
	public Quaternion rotation = Quaternion.identity;

	// Token: 0x04001437 RID: 5175
	public Vector3 position = Vector3.zero;

	// Token: 0x04001438 RID: 5176
	public Vector3 localScale = Vector3.one;

	// Token: 0x04001439 RID: 5177
	public Color color = Color.white;

	// Token: 0x0400143A RID: 5178
	public Vector3 baseScale = Vector3.one;

	// Token: 0x0400143B RID: 5179
	public int renderLayer;

	// Token: 0x0400143C RID: 5180
	[SerializeField]
	private Vector2 internalData0;

	// Token: 0x0400143D RID: 5181
	[SerializeField]
	private Vector2 internalData1;

	// Token: 0x0400143E RID: 5182
	[SerializeField]
	private Vector2 internalData2;

	// Token: 0x0400143F RID: 5183
	[SerializeField]
	private Vector2 colliderData = new Vector2(0f, 1f);

	// Token: 0x04001440 RID: 5184
	[SerializeField]
	private string formattedText = string.Empty;

	// Token: 0x04001441 RID: 5185
	[SerializeField]
	private tk2dBatchedSprite.Flags flags;

	// Token: 0x04001442 RID: 5186
	public tk2dBaseSprite.Anchor anchor;

	// Token: 0x04001443 RID: 5187
	public Matrix4x4 relativeMatrix = Matrix4x4.identity;

	// Token: 0x04001444 RID: 5188
	private Vector3 cachedBoundsCenter = Vector3.zero;

	// Token: 0x04001445 RID: 5189
	private Vector3 cachedBoundsExtents = Vector3.zero;

	// Token: 0x0200027D RID: 637
	public enum Type
	{
		// Token: 0x04001447 RID: 5191
		EmptyGameObject,
		// Token: 0x04001448 RID: 5192
		Sprite,
		// Token: 0x04001449 RID: 5193
		TiledSprite,
		// Token: 0x0400144A RID: 5194
		SlicedSprite,
		// Token: 0x0400144B RID: 5195
		ClippedSprite,
		// Token: 0x0400144C RID: 5196
		TextMesh
	}

	// Token: 0x0200027E RID: 638
	[Flags]
	public enum Flags
	{
		// Token: 0x0400144E RID: 5198
		None = 0,
		// Token: 0x0400144F RID: 5199
		Sprite_CreateBoxCollider = 1,
		// Token: 0x04001450 RID: 5200
		SlicedSprite_BorderOnly = 2
	}
}
