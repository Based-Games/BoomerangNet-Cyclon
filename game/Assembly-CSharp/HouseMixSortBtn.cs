using System;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class HouseMixSortBtn : MonoBehaviour
{
	// Token: 0x06000DA9 RID: 3497 RVA: 0x00061EDC File Offset: 0x000600DC
	private void Awake()
	{
		this.m_spIcon = base.transform.FindChild("Sprite_icon").GetComponent<UISprite>();
		this.m_spLoadingBG = base.transform.FindChild("loading").GetComponent<UISprite>();
		this.m_gArrowAni = base.transform.FindChild("Sprite_ArrowAni").gameObject;
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0000C0CE File Offset: 0x0000A2CE
	private void Start()
	{
		this.m_spLoadingBG.gameObject.SetActive(false);
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x0000C0E1 File Offset: 0x0000A2E1
	public void ClickProcess()
	{
		if (this.m_bisSelect)
		{
			return;
		}
		this.ShowBtn(true);
		this.m_HouseMixManager.DiscRange(this.m_isBtnKind, this.m_isBtnState);
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x00061F3C File Offset: 0x0006013C
	public void ShowBtn(bool Sound = true)
	{
		switch (this.m_isBtnState)
		{
		case HouseMixSortBtn.SortState_e._none:
			this.m_isBtnState = HouseMixSortBtn.SortState_e._up;
			if (Sound)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_INCREASE, false);
			}
			break;
		case HouseMixSortBtn.SortState_e._up:
			this.m_isBtnState = HouseMixSortBtn.SortState_e._down;
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_DECREASE, false);
			break;
		case HouseMixSortBtn.SortState_e._down:
			this.m_isBtnState = HouseMixSortBtn.SortState_e._up;
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_INCREASE, false);
			break;
		}
		this.m_gArrowAni.SetActive(true);
		this.m_gArrowAni.GetComponent<UISprite>().spriteName = "sort_arrow" + this.m_isBtnState.ToString();
		TweenPosition component = this.m_gArrowAni.GetComponent<TweenPosition>();
		if (this.m_isBtnState == HouseMixSortBtn.SortState_e._up)
		{
			component.from = new Vector3(0f, (float)(-(float)this.m_iAniPosValue), 0f);
			component.to = new Vector3(0f, (float)this.m_iAniPosValue, 0f);
		}
		else if (this.m_isBtnState == HouseMixSortBtn.SortState_e._down)
		{
			component.from = new Vector3(0f, (float)this.m_iAniPosValue, 0f);
			component.to = new Vector3(0f, (float)(-(float)this.m_iAniPosValue), 0f);
		}
		this.m_spIcon.spriteName = this.m_isBtnKind.ToString() + "_up";
		this.m_spLoadingBG.gameObject.SetActive(true);
		this.m_spLoadingBG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		this.m_bisSelect = true;
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x000620EC File Offset: 0x000602EC
	public void HideBtn()
	{
		this.m_isBtnState = HouseMixSortBtn.SortState_e._none;
		this.m_spIcon.spriteName = this.m_isBtnKind.ToString() + this.m_isBtnState.ToString();
		this.m_bisSelect = true;
		this.m_gArrowAni.SetActive(false);
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x0000C10D File Offset: 0x0000A30D
	public void SortEnd()
	{
		this.m_bisSelect = false;
		this.m_spLoadingBG.gameObject.SetActive(false);
	}

	// Token: 0x04000E04 RID: 3588
	public HouseMixManager m_HouseMixManager;

	// Token: 0x04000E05 RID: 3589
	public HouseMixSortBtn.DiscSortKind_e m_isBtnKind;

	// Token: 0x04000E06 RID: 3590
	public HouseMixSortBtn.SortState_e m_isBtnState;

	// Token: 0x04000E07 RID: 3591
	[HideInInspector]
	public bool m_bisSelect;

	// Token: 0x04000E08 RID: 3592
	private UISprite m_spLoadingBG;

	// Token: 0x04000E09 RID: 3593
	private UISprite m_spIcon;

	// Token: 0x04000E0A RID: 3594
	private GameObject m_gArrowAni;

	// Token: 0x04000E0B RID: 3595
	private int m_iAniPosValue = 17;

	// Token: 0x020001D2 RID: 466
	public enum DiscSortKind_e
	{
		// Token: 0x04000E0D RID: 3597
		sort_lv,
		// Token: 0x04000E0E RID: 3598
		sort_name,
		// Token: 0x04000E0F RID: 3599
		sort_artist
	}

	// Token: 0x020001D3 RID: 467
	public enum SortState_e
	{
		// Token: 0x04000E11 RID: 3601
		_none,
		// Token: 0x04000E12 RID: 3602
		_up,
		// Token: 0x04000E13 RID: 3603
		_down
	}
}
