using System;

// Token: 0x0200026D RID: 621
[Serializable]
public class tk2dSpriteCollectionPlatform
{
	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x060011F3 RID: 4595 RVA: 0x0000F500 File Offset: 0x0000D700
	public bool Valid
	{
		get
		{
			return this.name.Length > 0 && this.spriteCollection != null;
		}
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x0000F522 File Offset: 0x0000D722
	public void CopyFrom(tk2dSpriteCollectionPlatform source)
	{
		this.name = source.name;
		this.spriteCollection = source.spriteCollection;
	}

	// Token: 0x040013A0 RID: 5024
	public string name = string.Empty;

	// Token: 0x040013A1 RID: 5025
	public tk2dSpriteCollection spriteCollection;
}
