using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class StarSetting : MonoBehaviour
{
	// Token: 0x06000DBE RID: 3518 RVA: 0x0000C1CC File Offset: 0x0000A3CC
	private void Awake()
	{
		this.m_spSprite = base.transform.FindChild("Sprite_st").GetComponent<UISprite>();
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x00062550 File Offset: 0x00060750
	public void setStar(bool ss, string colornum = "")
	{
		if (!this.ResultMode)
		{
			if (ss)
			{
				this.m_spSprite.spriteName = "star" + colornum;
			}
			else
			{
				this.m_spSprite.spriteName = "star_void";
			}
		}
		else if (ss)
		{
			this.m_spSprite.spriteName = "result_star" + colornum;
		}
		else
		{
			this.m_spSprite.spriteName = "result_star_void";
		}
		this.m_spSprite.MakePixelPerfect();
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0000C1E9 File Offset: 0x0000A3E9
	public void SetComma()
	{
		this.m_spSprite.spriteName = "star_comma";
		this.m_spSprite.MakePixelPerfect();
	}

	// Token: 0x04000E2A RID: 3626
	[HideInInspector]
	public bool ResultMode;

	// Token: 0x04000E2B RID: 3627
	private UISprite m_spSprite;
}
