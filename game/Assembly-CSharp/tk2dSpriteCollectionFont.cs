using System;
using UnityEngine;

// Token: 0x0200026C RID: 620
[Serializable]
public class tk2dSpriteCollectionFont
{
	// Token: 0x060011EF RID: 4591 RVA: 0x0007C8A0 File Offset: 0x0007AAA0
	public void CopyFrom(tk2dSpriteCollectionFont src)
	{
		this.active = src.active;
		this.bmFont = src.bmFont;
		this.texture = src.texture;
		this.dupeCaps = src.dupeCaps;
		this.flipTextureY = src.flipTextureY;
		this.charPadX = src.charPadX;
		this.data = src.data;
		this.editorData = src.editorData;
		this.materialId = src.materialId;
		this.gradientCount = src.gradientCount;
		this.gradientTexture = src.gradientTexture;
		this.useGradient = src.useGradient;
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0007C940 File Offset: 0x0007AB40
	public string Name
	{
		get
		{
			if (this.bmFont == null || this.texture == null)
			{
				return "Empty";
			}
			if (this.data == null)
			{
				return this.bmFont.name + " (Inactive)";
			}
			return this.bmFont.name;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0007C9A8 File Offset: 0x0007ABA8
	public bool InUse
	{
		get
		{
			return this.active && this.bmFont != null && this.texture != null && this.data != null && this.editorData != null;
		}
	}

	// Token: 0x04001394 RID: 5012
	public bool active;

	// Token: 0x04001395 RID: 5013
	public TextAsset bmFont;

	// Token: 0x04001396 RID: 5014
	public Texture2D texture;

	// Token: 0x04001397 RID: 5015
	public bool dupeCaps;

	// Token: 0x04001398 RID: 5016
	public bool flipTextureY;

	// Token: 0x04001399 RID: 5017
	public int charPadX;

	// Token: 0x0400139A RID: 5018
	public tk2dFontData data;

	// Token: 0x0400139B RID: 5019
	public tk2dFont editorData;

	// Token: 0x0400139C RID: 5020
	public int materialId;

	// Token: 0x0400139D RID: 5021
	public bool useGradient;

	// Token: 0x0400139E RID: 5022
	public Texture2D gradientTexture;

	// Token: 0x0400139F RID: 5023
	public int gradientCount = 1;
}
