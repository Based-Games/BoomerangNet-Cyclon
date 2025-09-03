using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class WWWTexture : WWWObject
{
	// Token: 0x06000AB9 RID: 2745 RVA: 0x0000A5E7 File Offset: 0x000087E7
	public override void StartLoad()
	{
		this.www = new WWW(this.strPath);
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0004C3D4 File Offset: 0x0004A5D4
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			return;
		}
		if (null != this.VIEWTEXTURE)
		{
			this.VIEWTEXTURE.mainTexture = this.www.texture;
		}
	}

	// Token: 0x04000A6F RID: 2671
	public UITexture VIEWTEXTURE;
}
