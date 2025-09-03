using System;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class RaveUpStarSetting : MonoBehaviour
{
	// Token: 0x06000F17 RID: 3863 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
	public void setStar(bool ss)
	{
		if (ss)
		{
			this.m_Sprite.spriteName = "raveup_star";
		}
		else
		{
			this.m_Sprite.spriteName = "raveup_star_void";
		}
		this.m_Sprite.MakePixelPerfect();
	}

	// Token: 0x04001098 RID: 4248
	public UISprite m_Sprite;
}
