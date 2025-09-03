using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class ItemBtn : MonoBehaviour
{
	// Token: 0x06000DB5 RID: 3509 RVA: 0x0000C182 File Offset: 0x0000A382
	private void ClickProcess()
	{
		this.m_isSelect = !this.m_isSelect;
		this.SetBtnImage(this.m_isSelect);
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x00062144 File Offset: 0x00060344
	private void SetBtnImage(bool st)
	{
		if (st)
		{
			this.BG.spriteName = "itembg_select";
			this.BG.MakePixelPerfect();
			this.BG.transform.localScale = Vector3.one * 2f;
			this.Icon.spriteName = this.isItem.ToString() + "_select";
			this.Icon.MakePixelPerfect();
			this.Icon.transform.localScale = Vector3.one * 1.5f;
			this.Icon.transform.localPosition = new Vector3(0f, -5f, 0f);
		}
		else
		{
			this.BG.spriteName = "itembg";
			this.BG.MakePixelPerfect();
			this.BG.transform.localScale = Vector3.one * 2f;
			this.Icon.spriteName = this.isItem.ToString();
			this.Icon.MakePixelPerfect();
			this.Icon.transform.localScale = Vector3.one * 1.5f;
			this.Icon.transform.localPosition = new Vector3(0f, 5f, 0f);
		}
	}

	// Token: 0x04000E1B RID: 3611
	public ItemBtn.ItemKind_e isItem;

	// Token: 0x04000E1C RID: 3612
	public UISprite BG;

	// Token: 0x04000E1D RID: 3613
	public UISprite Icon;

	// Token: 0x04000E1E RID: 3614
	[HideInInspector]
	public bool m_isSelect;

	// Token: 0x020001D6 RID: 470
	public enum ItemKind_e
	{
		// Token: 0x04000E20 RID: 3616
		potion,
		// Token: 0x04000E21 RID: 3617
		shield
	}
}
