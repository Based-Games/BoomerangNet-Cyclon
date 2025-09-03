using System;
using UnityEngine;

// Token: 0x0200023A RID: 570
[AddComponentMenu("2D Toolkit/Backend/tk2dFont")]
public class tk2dFont : MonoBehaviour
{
	// Token: 0x06001044 RID: 4164 RVA: 0x00074728 File Offset: 0x00072928
	public void Upgrade()
	{
		if (this.version >= tk2dFont.CURRENT_VERSION)
		{
			return;
		}
		Debug.Log("Font '" + base.name + "' - Upgraded from version " + this.version.ToString());
		if (this.version == 0)
		{
			this.sizeDef.CopyFromLegacy(this.useTk2dCamera, this.targetOrthoSize, (float)this.targetHeight);
		}
		this.version = tk2dFont.CURRENT_VERSION;
	}

	// Token: 0x040011F2 RID: 4594
	public TextAsset bmFont;

	// Token: 0x040011F3 RID: 4595
	public Material material;

	// Token: 0x040011F4 RID: 4596
	public Texture texture;

	// Token: 0x040011F5 RID: 4597
	public Texture2D gradientTexture;

	// Token: 0x040011F6 RID: 4598
	public bool dupeCaps;

	// Token: 0x040011F7 RID: 4599
	public bool flipTextureY;

	// Token: 0x040011F8 RID: 4600
	[HideInInspector]
	public bool proxyFont;

	// Token: 0x040011F9 RID: 4601
	[SerializeField]
	[HideInInspector]
	private bool useTk2dCamera;

	// Token: 0x040011FA RID: 4602
	[SerializeField]
	[HideInInspector]
	private int targetHeight = 640;

	// Token: 0x040011FB RID: 4603
	[SerializeField]
	[HideInInspector]
	private float targetOrthoSize = 1f;

	// Token: 0x040011FC RID: 4604
	public tk2dSpriteCollectionSize sizeDef = tk2dSpriteCollectionSize.Default();

	// Token: 0x040011FD RID: 4605
	public int gradientCount = 1;

	// Token: 0x040011FE RID: 4606
	public bool manageMaterial;

	// Token: 0x040011FF RID: 4607
	[HideInInspector]
	public bool loadable;

	// Token: 0x04001200 RID: 4608
	public int charPadX;

	// Token: 0x04001201 RID: 4609
	public tk2dFontData data;

	// Token: 0x04001202 RID: 4610
	public static int CURRENT_VERSION = 1;

	// Token: 0x04001203 RID: 4611
	public int version;
}
