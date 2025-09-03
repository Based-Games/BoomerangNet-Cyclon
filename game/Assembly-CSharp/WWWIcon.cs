using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class WWWIcon : WWWObject
{
	// Token: 0x06000ABC RID: 2748 RVA: 0x0004C428 File Offset: 0x0004A628
	public override void StartLoad()
	{
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		this.www = new WWW(userData.WebIcon);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0004C454 File Offset: 0x0004A654
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
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		userData.TexIcon = this.www.texture;
	}
}
