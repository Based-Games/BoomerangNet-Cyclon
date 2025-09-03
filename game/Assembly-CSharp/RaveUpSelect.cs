using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class RaveUpSelect : MonoBehaviour
{
	// Token: 0x06000F02 RID: 3842 RVA: 0x0006C2C4 File Offset: 0x0006A4C4
	private void Awake()
	{
		this.m_RaveUpSelectAni = base.transform.parent.GetComponent<RaveUpSelectAni>();
		this.m_gCheckMark = base.transform.FindChild("selectMark_Check").gameObject;
		this.m_gSelectNumber = base.transform.FindChild("selectNumber").gameObject;
		this.m_gNumber = base.transform.FindChild("selectnum").gameObject;
		this.m_spBG = base.transform.FindChild("BG").GetComponent<UISprite>();
		this.m_gReadyBG = base.transform.FindChild("BGReady").gameObject;
		this.m_spName = base.transform.FindChild("Name").GetComponent<UISprite>();
		this.m_tAttachObj = base.transform.FindChild("DiscAttach");
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x0000D02D File Offset: 0x0000B22D
	private void Start()
	{
		this.m_iSelectIndex = -1;
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x0006C3A0 File Offset: 0x0006A5A0
	public void SelectReady()
	{
		this.isReady = true;
		float num = 1.25f;
		base.transform.localScale = new Vector3(num, num, 0f);
		if (!this.m_gReadyBG.activeSelf)
		{
			this.m_gReadyBG.SetActive(true);
			this.m_gReadyBG.GetComponent<TweenAlpha>().Play(true);
			if (this.m_bisHaveCD)
			{
				this.m_gReadyBG.GetComponent<UISprite>().spriteName = "raveup_selectdisc_bg_select";
			}
			else
			{
				this.m_gReadyBG.GetComponent<UISprite>().spriteName = "raveup_selectdisc_bg";
			}
		}
		this.m_RaveUpSelectAni.SpacePosSetting(this.m_iisIndex);
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0006C44C File Offset: 0x0006A64C
	public void NoneReady()
	{
		this.isReady = false;
		this.m_gCheckMark.SetActive(false);
		this.m_gSelectNumber.SetActive(false);
		this.m_gNumber.SetActive(true);
		this.m_gReadyBG.SetActive(false);
		base.transform.localScale = Vector3.one;
		this.m_spBG.spriteName = "raveup_selectdisc_bg";
		this.m_spName.spriteName = "raveup_stage";
		this.m_spName.MakePixelPerfect();
		this.m_spName.transform.localScale = Vector3.one * 2f;
		this.m_bisHaveCD = false;
		this.m_iSelectIndex = -1;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0006C4F8 File Offset: 0x0006A6F8
	public void Attach(GameObject CD)
	{
		this.isReady = false;
		this.m_gReadyBG.SetActive(false);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_MOUNT, false);
		this.m_iSelectLevel = CD.GetComponent<RaveUpCD>().m_iLevel;
		base.transform.localScale = Vector3.one;
		this.m_gCheckMark.SetActive(true);
		this.m_gSelectNumber.SetActive(true);
		this.m_gNumber.SetActive(false);
		this.m_spBG.spriteName = "raveup_selectdisc_bg_select";
		this.m_spName.spriteName = "raveup_stage_select";
		this.m_spName.MakePixelPerfect();
		this.m_spName.transform.localScale = Vector3.one * 2f;
		this.m_bisHaveCD = true;
		this.m_gcd = CD;
		this.m_gcd.GetComponent<RaveUpCD>().m_bisAttach = true;
		this.m_gcd.transform.localScale = Vector3.one;
		this.m_gcd.transform.GetChild(0).gameObject.SetActive(false);
		this.m_iSelectIndex = this.m_gcd.GetComponent<RaveUpCD>().m_iIndex;
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0006C61C File Offset: 0x0006A81C
	private void PressProcess()
	{
		if (this.m_gcd == null || this.m_iSelectIndex == -1)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_UNMOUNT, false);
		this.m_gcd.transform.GetChild(0).gameObject.SetActive(true);
		this.m_gcd.transform.localPosition = new Vector3(-1147f - base.transform.localPosition.x, -705f - base.transform.localPosition.y, 0f);
		this.m_gcd.GetComponent<RaveUpCD>().m_bisAttach = false;
		this.m_gcd.GetComponent<UIPanel>().depth = 401;
		this.m_gcd.GetComponent<RaveUpCD>().m_bMoveState = true;
		this.m_gcd.GetComponent<TweenPosition>().enabled = false;
		this.NoneReady();
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0006C70C File Offset: 0x0006A90C
	private void DragEndProcess()
	{
		if (this.m_gcd == null)
		{
			return;
		}
		this.m_gcd.GetComponent<RaveUpCD>().m_bMoveState = false;
		this.m_gcd.GetComponent<RaveUpCD>().TweenAni();
		this.m_gcd.GetComponent<UIPanel>().depth = 400;
	}

	// Token: 0x04001079 RID: 4217
	public int m_iisIndex;

	// Token: 0x0400107A RID: 4218
	[HideInInspector]
	public RaveUpSelectAni m_RaveUpSelectAni;

	// Token: 0x0400107B RID: 4219
	[HideInInspector]
	public int m_iSelectLevel;

	// Token: 0x0400107C RID: 4220
	[HideInInspector]
	public int m_iSelectIndex;

	// Token: 0x0400107D RID: 4221
	[HideInInspector]
	public bool m_bisHaveCD;

	// Token: 0x0400107E RID: 4222
	[HideInInspector]
	public GameObject m_gcd;

	// Token: 0x0400107F RID: 4223
	[HideInInspector]
	public bool isReady;

	// Token: 0x04001080 RID: 4224
	[HideInInspector]
	public Transform m_tAttachObj;

	// Token: 0x04001081 RID: 4225
	[HideInInspector]
	public GameObject m_gReadyBG;

	// Token: 0x04001082 RID: 4226
	[HideInInspector]
	public GameObject m_gNumber;

	// Token: 0x04001083 RID: 4227
	private GameObject m_gCheckMark;

	// Token: 0x04001084 RID: 4228
	private GameObject m_gSelectNumber;

	// Token: 0x04001085 RID: 4229
	private UISprite m_spBG;

	// Token: 0x04001086 RID: 4230
	private UISprite m_spName;
}
