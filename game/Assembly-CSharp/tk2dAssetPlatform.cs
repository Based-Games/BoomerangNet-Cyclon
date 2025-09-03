using System;

// Token: 0x0200024A RID: 586
[Serializable]
public class tk2dAssetPlatform
{
	// Token: 0x060010C0 RID: 4288 RVA: 0x0000E482 File Offset: 0x0000C682
	public tk2dAssetPlatform(string name, float scale)
	{
		this.name = name;
		this.scale = scale;
	}

	// Token: 0x040012A0 RID: 4768
	public string name = string.Empty;

	// Token: 0x040012A1 RID: 4769
	public float scale = 1f;
}
