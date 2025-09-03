using System;
using UnityEngine;

// Token: 0x020001ED RID: 493
public class ResultTrophy : MonoBehaviour
{
	// Token: 0x06000E4E RID: 3662 RVA: 0x000671A4 File Offset: 0x000653A4
	private void Awake()
	{
		this.m_Icon_TR = base.transform.FindChild("tr_Panel").FindChild("Sprite_tr").GetComponent<TweenScale>();
		this.m_gIcon_TR_eff = base.transform.FindChild("Sprite_tr_eff").gameObject;
		this.m_Icon_MK = base.transform.FindChild("mk_Panel").FindChild("Sprite_mk").GetComponent<TweenScale>();
		this.m_gIcon_MK_eff = base.transform.FindChild("Sprite_mk_eff").gameObject;
		this.m_Icon_PF = base.transform.FindChild("pf_Panel").FindChild("Sprite_pf").GetComponent<TweenScale>();
		this.m_gIcon_PF_eff = base.transform.FindChild("Sprite_pf_eff").gameObject;
		switch (this.m_ResultKind)
		{
		case ResultKind_e.AllClearResult:
			this.m_ResultAllClearPlayResult = base.transform.parent.GetComponent<ResultAllClearPlayResult>();
			break;
		case ResultKind_e.RaveUpClearResult:
			this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
			break;
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x000672CC File Offset: 0x000654CC
	public void DefaultTrophySetting()
	{
		this.m_bPerfectPlay = true;
		this.m_bAllCombo = true;
		if (this.m_ResultKind == ResultKind_e.ClearResult)
		{
			this.m_bTrophy = true;
			this.m_sisTrophyName = "CPOOER";
			this.m_sTrophySpriteName = Singleton<GameManager>.instance.ResultData.PTTYPE.ToString().ToLower() + "_" + this.m_sisTrophyName;
		}
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00067338 File Offset: 0x00065538
	public void TrophySetting()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		if (this.m_bUseTrohpy)
		{
			this.m_bTrophy = true;
			switch (resultData.GRADETYPE)
			{
			case GRADE.B:
				this.m_sisTrophyName = "CPOOER";
				this.m_sTrophySpriteName = Singleton<GameManager>.instance.ResultData.PTTYPE.ToString().ToLower() + "_" + this.m_sisTrophyName;
				break;
			case GRADE.A:
			case GRADE.A_P:
			case GRADE.A_PP:
				this.m_sisTrophyName = "SILVER";
				this.m_sTrophySpriteName = Singleton<GameManager>.instance.ResultData.PTTYPE.ToString().ToLower() + "_" + this.m_sisTrophyName;
				break;
			case GRADE.S:
			case GRADE.S_P:
			case GRADE.S_PP:
				this.m_sisTrophyName = "GOLD";
				this.m_sTrophySpriteName = Singleton<GameManager>.instance.ResultData.PTTYPE.ToString().ToLower() + "_" + this.m_sisTrophyName;
				break;
			default:
				this.m_bTrophy = false;
				this.m_sisTrophyName = string.Empty;
				break;
			}
			this.m_sTrophySpriteName = resultData.TrophyName;
			if (this.m_sTrophySpriteName == string.Empty)
			{
				this.m_bTrophy = false;
			}
		}
		this.m_bPerfectPlay = resultData.IsPerfectPlay();
		this.m_bAllCombo = resultData.IsAllComboPlay();
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0000C754 File Offset: 0x0000A954
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x000674B8 File Offset: 0x000656B8
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		switch (this.m_AniStep)
		{
		case ResultManager.StepKind_e.Step1:
			if (this.m_bUseAllCombo)
			{
				base.Invoke("AllComboStep", this.m_fAllcomboStartTime);
				this.m_bUseAllCombo = false;
			}
			this.m_AniStep++;
			break;
		case ResultManager.StepKind_e.Step2:
			if (this.m_bUsePF)
			{
				base.Invoke("pfStep", this.m_fPerfectPlayStartTime);
				base.Invoke("SoundTest", 0f);
				this.m_bUsePF = false;
			}
			this.m_AniStep++;
			break;
		case ResultManager.StepKind_e.Step3:
			if (this.m_bUseTrohpy)
			{
				base.Invoke("trStep", this.m_fTrophyStartTime);
				this.m_bUseTrohpy = false;
			}
			this.m_AniStep++;
			break;
		case ResultManager.StepKind_e.Step4:
			this.m_bAniEndState = true;
			break;
		}
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x000675B4 File Offset: 0x000657B4
	private void trStep()
	{
		if (!this.m_bTrophy)
		{
			this.m_AniStep++;
			return;
		}
		this.m_Icon_TR.enabled = true;
		this.m_Icon_TR.GetComponent<TweenAlpha>().enabled = true;
		this.m_Icon_TR.GetComponent<UISprite>().spriteName = this.m_sTrophySpriteName;
		this.m_gIcon_TR_eff.GetComponent<UISprite>().spriteName = this.m_sTrophySpriteName;
		this.m_gIcon_TR_eff.GetComponent<TweenAlpha>().enabled = true;
		this.m_gIcon_TR_eff.GetComponent<TweenScale>().enabled = true;
		this.m_Icon_TR.transform.parent.transform.GetComponent<TweenScale>().enabled = true;
		if (!this.m_bTRSound)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_TROPHY, false);
			this.m_bTRSound = true;
		}
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x00067688 File Offset: 0x00065888
	private void AllComboStep()
	{
		if (!this.m_bAllCombo)
		{
			this.m_AniStep++;
			return;
		}
		this.m_Icon_MK.enabled = true;
		this.m_Icon_MK.GetComponent<TweenAlpha>().enabled = true;
		this.m_gIcon_MK_eff.GetComponent<TweenAlpha>().enabled = true;
		this.m_gIcon_MK_eff.GetComponent<TweenScale>().enabled = true;
		this.m_Icon_MK.transform.parent.transform.GetComponent<TweenScale>().enabled = true;
		if (!this.m_bMKSound)
		{
			if (!this.m_bPerfectPlay)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_ALLCOMBO, false);
			}
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_TROPHY, false);
			this.m_bMKSound = true;
		}
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00067748 File Offset: 0x00065948
	private void pfStep()
	{
		if (!this.m_bPerfectPlay)
		{
			this.m_AniStep++;
			return;
		}
		this.m_Icon_PF.enabled = true;
		this.m_Icon_PF.GetComponent<TweenAlpha>().enabled = true;
		this.m_gIcon_PF_eff.GetComponent<TweenAlpha>().enabled = true;
		this.m_gIcon_PF_eff.GetComponent<TweenScale>().enabled = true;
		this.m_Icon_PF.transform.parent.transform.GetComponent<TweenScale>().enabled = true;
		if (!this.m_bPFSound)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_TROPHY, false);
			this.m_bPFSound = true;
		}
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0000C75D File Offset: 0x0000A95D
	private void SoundTest()
	{
		if (!this.m_bPerfectPlay)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_PERFECTPLAY, false);
	}

	// Token: 0x04000F61 RID: 3937
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000F62 RID: 3938
	public ResultKind_e m_ResultKind;

	// Token: 0x04000F63 RID: 3939
	public bool m_bUseAllCombo;

	// Token: 0x04000F64 RID: 3940
	public bool m_bUsePF;

	// Token: 0x04000F65 RID: 3941
	public bool m_bUseTrohpy;

	// Token: 0x04000F66 RID: 3942
	private TweenScale m_Icon_TR;

	// Token: 0x04000F67 RID: 3943
	private GameObject m_gIcon_TR_eff;

	// Token: 0x04000F68 RID: 3944
	private bool m_bTRSound;

	// Token: 0x04000F69 RID: 3945
	private TweenScale m_Icon_MK;

	// Token: 0x04000F6A RID: 3946
	private GameObject m_gIcon_MK_eff;

	// Token: 0x04000F6B RID: 3947
	private bool m_bMKSound;

	// Token: 0x04000F6C RID: 3948
	private TweenScale m_Icon_PF;

	// Token: 0x04000F6D RID: 3949
	private GameObject m_gIcon_PF_eff;

	// Token: 0x04000F6E RID: 3950
	private bool m_bPFSound;

	// Token: 0x04000F6F RID: 3951
	private bool m_bAllCombo;

	// Token: 0x04000F70 RID: 3952
	private bool m_bPerfectPlay;

	// Token: 0x04000F71 RID: 3953
	private bool m_bTrophy;

	// Token: 0x04000F72 RID: 3954
	private string m_sTrophySpriteName;

	// Token: 0x04000F73 RID: 3955
	private string m_sisTrophyName = string.Empty;

	// Token: 0x04000F74 RID: 3956
	private bool m_bAniEndState;

	// Token: 0x04000F75 RID: 3957
	private float m_fAllcomboStartTime = 0.05f;

	// Token: 0x04000F76 RID: 3958
	private float m_fPerfectPlayStartTime = 0.15f;

	// Token: 0x04000F77 RID: 3959
	private float m_fTrophyStartTime = 0.25f;

	// Token: 0x04000F78 RID: 3960
	private RaveUpResult m_RaveUpResult;

	// Token: 0x04000F79 RID: 3961
	private ResultAllClearPlayResult m_ResultAllClearPlayResult;
}
