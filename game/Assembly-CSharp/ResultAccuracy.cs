using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class ResultAccuracy : MonoBehaviour
{
	// Token: 0x06000DC5 RID: 3525 RVA: 0x000626A4 File Offset: 0x000608A4
	private void Awake()
	{
		this.m_Step_1_Ani = base.transform.FindChild("Step_1_Ani").GetComponent<TweenPosition>();
		this.m_Step_2_Ani = base.transform.FindChild("Step_2_Ani").GetComponent<TweenPosition>();
		this.m_Step_3_Ani = base.transform.FindChild("Step_3_Ani").GetComponent<TweenPosition>();
		Transform transform = base.transform.FindChild("1.perfect");
		this.m_lPerfect = transform.FindChild("Label_num1").GetComponent<UILabel>();
		this.m_lPerfectPer = transform.FindChild("Label_num2").GetComponent<UILabel>();
		Transform transform2 = base.transform.FindChild("2.great");
		this.m_lGreat = transform2.FindChild("Label_num1").GetComponent<UILabel>();
		this.m_lGreatPer = transform2.FindChild("Label_num2").GetComponent<UILabel>();
		Transform transform3 = base.transform.FindChild("3.good");
		this.m_lGood = transform3.FindChild("Label_num1").GetComponent<UILabel>();
		this.m_lGoodPer = transform3.FindChild("Label_num2").GetComponent<UILabel>();
		Transform transform4 = base.transform.FindChild("4.poor");
		this.m_lPoor = transform4.FindChild("Label_num1").GetComponent<UILabel>();
		this.m_lPoorPer = transform4.FindChild("Label_num2").GetComponent<UILabel>();
		Transform transform5 = base.transform.FindChild("5.break");
		this.m_lBreak = transform5.FindChild("Label_num1").GetComponent<UILabel>();
		this.m_lBreakPer = transform5.FindChild("Label_num2").GetComponent<UILabel>();
		Transform transform6 = base.transform.FindChild("6.TotalAccuracy");
		this.m_lTotalAccuracy = transform6.FindChild("Label_score1").GetComponent<UILabel>();
		this.m_lTotalAccuracyPer = transform6.FindChild("Label_score2").GetComponent<UILabel>();
		this.m_spTotalAccuracyUpDownIcon = transform6.FindChild("Sprite_UpDown").GetComponent<UISprite>();
		Transform transform7 = base.transform.FindChild("7.maxcombo");
		this.m_lMaxCombo = transform7.FindChild("Label_score1").GetComponent<UILabel>();
		this.m_lMaxComboBefore = transform7.FindChild("Label_score2").GetComponent<UILabel>();
		this.m_spMaxComboUpDownIcon = transform7.FindChild("Sprite_UpDown").GetComponent<UISprite>();
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x000628E4 File Offset: 0x00060AE4
	private void Start()
	{
		this.m_Step_1_Ani.duration = (this.m_Step_2_Ani.duration = (this.m_Step_3_Ani.duration = this.m_fDurationTime));
		this.m_Step_1_Ani.to = (this.m_Step_2_Ani.to = (this.m_Step_3_Ani.to = this.m_v3TweenToPos));
		this.m_lPerfect.text = "0";
		this.m_lPerfectPer.text = "0";
		this.m_lGreat.text = "0";
		this.m_lGreatPer.text = "0";
		this.m_lGood.text = "0";
		this.m_lGoodPer.text = "0";
		this.m_lPoor.text = "0";
		this.m_lPoorPer.text = "0";
		this.m_lBreak.text = "0";
		this.m_lBreakPer.text = "0";
		this.m_lTotalAccuracy.text = "0";
		this.m_lTotalAccuracyPer.text = "0";
		this.m_lMaxCombo.text = "0";
		this.m_lMaxComboBefore.text = "0";
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00062A2C File Offset: 0x00060C2C
	public void DefaultAccuracySetting()
	{
		this.m_iPerfect = 182;
		this.m_iGreat = 50;
		this.m_iGood = 6;
		this.m_iPoor = 4;
		this.m_iBreak = 2;
		this.m_iMaxNodeCount = this.m_iPerfect + this.m_iGreat + this.m_iGood + this.m_iPoor + this.m_iBreak;
		this.m_iPerfectPer = (int)((float)this.m_iPerfect / (float)this.m_iMaxNodeCount * 100f);
		this.m_iGreatPer = (int)((float)this.m_iGreat / (float)this.m_iMaxNodeCount * 100f);
		this.m_iGoodPer = (int)((float)this.m_iGood / (float)this.m_iMaxNodeCount * 100f);
		this.m_iPoorPer = (int)((float)this.m_iPoor / (float)this.m_iMaxNodeCount * 100f);
		this.m_iBreakPer = (int)((float)this.m_iBreak / (float)this.m_iMaxNodeCount * 100f);
		this.m_iMaxCombo = 4523;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00062B20 File Offset: 0x00060D20
	public void AccuracySetting()
	{
		this.m_iPerfect = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.PERFECT);
		this.m_iGreat = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.GREAT);
		this.m_iGood = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.GOOD);
		this.m_iPoor = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.POOR);
		this.m_iBreak = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.BREAK);
		this.m_iMaxNodeCount = this.m_iPerfect + this.m_iGreat + this.m_iGood + this.m_iPoor + this.m_iBreak;
		this.m_iPerfectPer = (int)((float)this.m_iPerfect / (float)this.m_iMaxNodeCount * 100f);
		this.m_iGreatPer = (int)((float)this.m_iGreat / (float)this.m_iMaxNodeCount * 100f);
		this.m_iGoodPer = (int)((float)this.m_iGood / (float)this.m_iMaxNodeCount * 100f);
		this.m_iPoorPer = (int)((float)this.m_iPoor / (float)this.m_iMaxNodeCount * 100f);
		this.m_iBreakPer = (int)((float)this.m_iBreak / (float)this.m_iMaxNodeCount * 100f);
		int num = GameData.Judgment_Score[4] * this.m_iMaxNodeCount;
		int num2 = this.m_iPerfect * GameData.Judgment_Score[4] + this.m_iGreat * GameData.Judgment_Score[3] + this.m_iGood * GameData.Judgment_Score[2] + this.m_iPoor * GameData.Judgment_Score[1] + this.m_iBreak * GameData.Judgment_Score[0];
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		this.m_iMaxCombo = resultData.MAXCOMBO;
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0000C24E File Offset: 0x0000A44E
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00055FC8 File Offset: 0x000541C8
	private void PlayAni(bool State, float _TweenValue, int value1, int value2, GameObject text1 = null, GameObject text2 = null, bool _PlusIcon = false, bool _PercentIcon_1 = false, bool _PercentIcon_2 = false, UISprite UpDownIcon = null)
	{
		if (State)
		{
			return;
		}
		if (text1.GetComponent<ImageFontLabel>() != null)
		{
			if (text1 != null)
			{
				if (_PercentIcon_1)
				{
					text1.GetComponent<ImageFontLabel>().text = ((int)((float)value1 * _TweenValue)).ToString();
				}
				else
				{
					text1.GetComponent<ImageFontLabel>().text = ((int)((float)value1 * _TweenValue)).ToString();
				}
			}
			if (text2 != null && text2.activeSelf)
			{
				if (_PercentIcon_2)
				{
					text2.GetComponent<ImageFontLabel>().text = ((int)((float)value2 * _TweenValue)).ToString();
				}
				else
				{
					text2.GetComponent<ImageFontLabel>().text = ((int)((float)value2 * _TweenValue)).ToString();
				}
			}
		}
		else if (text1.GetComponent<UILabel>() != null)
		{
			int num = 4;
			int num2 = 0;
			int num3 = (int)((float)value1 * _TweenValue);
			int num4 = (int)((float)value2 * _TweenValue);
			if (text1 != null)
			{
				for (int i = 0; i < num3.ToString().Length; i++)
				{
					if (num3.ToString()[i] == '1')
					{
						num2++;
					}
				}
				if (num2 > num)
				{
					num2 = num;
				}
				text1.GetComponent<UILabel>().spacingX = 1 + num2 * 3;
				if (_PercentIcon_1)
				{
					text1.GetComponent<UILabel>().text = num3.ToString();
				}
				else
				{
					text1.GetComponent<UILabel>().text = num3.ToString();
				}
			}
			num2 = 0;
			if (text2 != null && text2.activeSelf)
			{
				for (int j = 0; j < num4.ToString().Length; j++)
				{
					if (num4.ToString()[j] == '1')
					{
						num2++;
					}
				}
				if (num2 > num)
				{
					num2 = num;
				}
				text2.GetComponent<UILabel>().spacingX = 1 + num2 * 3;
				if (_PercentIcon_2)
				{
					text2.GetComponent<UILabel>().text = num4.ToString();
				}
				else
				{
					text2.GetComponent<UILabel>().text = num4.ToString();
				}
			}
		}
		if (_TweenValue >= 1f)
		{
			Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
			Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_BP_COUNT);
			Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT);
			State = true;
			if (_PlusIcon)
			{
				if (text1.GetComponent<ImageFontLabel>() != null)
				{
					string text3 = text1.GetComponent<ImageFontLabel>().text;
					text1.GetComponent<ImageFontLabel>().text = "+" + text3;
				}
				else if (text1.GetComponent<UILabel>() != null)
				{
					string text4 = text1.GetComponent<UILabel>().text;
					text1.GetComponent<UILabel>().text = "+" + text4;
				}
			}
			if (UpDownIcon != null && text2.activeSelf)
			{
				UpDownIcon.spriteName = "result_arrow_up";
				UpDownIcon.MakePixelPerfect();
				UpDownIcon.transform.localScale = Vector3.one * 2f;
				if (value1 < value2)
				{
					UpDownIcon.spriteName = "result_arrow_down";
					UpDownIcon.MakePixelPerfect();
					UpDownIcon.transform.localScale = Vector3.one * 2f;
				}
				else if (value1 == value2)
				{
					UpDownIcon.spriteName = "result_arrow_none";
					UpDownIcon.MakePixelPerfect();
					UpDownIcon.transform.localScale = Vector3.one * 2f;
				}
			}
		}
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00062CC0 File Offset: 0x00060EC0
	private void Update()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		if (this.m_bAniEndState)
		{
			return;
		}
		switch (this.m_AniStep)
		{
		case ResultManager.StepKind_e.Step1:
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_SCORE_COUNT, true);
			this.m_Step_1_Ani.enabled = true;
			this.PlayAni(this.m_bPerfectAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iPerfect, this.m_iPerfectPer, this.m_lPerfect.gameObject, this.m_lPerfectPer.gameObject, false, false, true, null);
			this.PlayAni(this.m_bGreatAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iGreat, this.m_iGreatPer, this.m_lGreat.gameObject, this.m_lGreatPer.gameObject, false, false, true, null);
			this.PlayAni(this.m_bGoodAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iGood, this.m_iGoodPer, this.m_lGood.gameObject, this.m_lGoodPer.gameObject, false, false, true, null);
			this.PlayAni(this.m_bPoorAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iPoor, this.m_iPoorPer, this.m_lPoor.gameObject, this.m_lPoorPer.gameObject, false, false, true, null);
			this.PlayAni(this.m_bBreakAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iBreak, this.m_iBreakPer, this.m_lBreak.gameObject, this.m_lBreakPer.gameObject, false, false, true, null);
			if (this.m_Step_1_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
				this.m_AniStep++;
			}
			break;
		case ResultManager.StepKind_e.Step2:
			if (resultData.GetAccuracy() > 0 || this.m_iTotalAccuracyPer > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT, true);
			}
			this.m_Step_2_Ani.enabled = true;
			this.PlayAni(this.m_bTotalAccuracyAniState, this.m_Step_2_Ani.transform.localPosition.x, resultData.GetAccuracy(), this.m_iTotalAccuracyPer, this.m_lTotalAccuracy.gameObject, this.m_lTotalAccuracyPer.gameObject, false, true, true, this.m_spTotalAccuracyUpDownIcon);
			if (this.m_Step_2_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT);
				this.m_AniStep++;
			}
			break;
		case ResultManager.StepKind_e.Step3:
			if (this.m_iMaxCombo > 0 || this.m_iMaxComboBefore > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_SCORE_COUNT, true);
			}
			this.m_Step_3_Ani.enabled = true;
			this.PlayAni(this.m_bMaxComboAniState, this.m_Step_3_Ani.transform.localPosition.x, this.m_iMaxCombo, this.m_iMaxComboBefore, this.m_lMaxCombo.gameObject, this.m_lMaxComboBefore.gameObject, false, false, false, this.m_spMaxComboUpDownIcon);
			if (this.m_Step_3_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
				this.m_bAniEndState = true;
			}
			break;
		}
	}

	// Token: 0x04000E2E RID: 3630
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000E2F RID: 3631
	private float m_fDurationTime = 0.25f;

	// Token: 0x04000E30 RID: 3632
	private Vector3 m_v3TweenToPos = new Vector3(1f, 0f, 0f);

	// Token: 0x04000E31 RID: 3633
	[HideInInspector]
	public int m_iMaxNodeCount;

	// Token: 0x04000E32 RID: 3634
	private TweenPosition m_Step_1_Ani;

	// Token: 0x04000E33 RID: 3635
	private TweenPosition m_Step_2_Ani;

	// Token: 0x04000E34 RID: 3636
	private TweenPosition m_Step_3_Ani;

	// Token: 0x04000E35 RID: 3637
	private UILabel m_lPerfect;

	// Token: 0x04000E36 RID: 3638
	private UILabel m_lPerfectPer;

	// Token: 0x04000E37 RID: 3639
	private TweenPosition m_PerfectAni;

	// Token: 0x04000E38 RID: 3640
	[HideInInspector]
	public int m_iPerfect;

	// Token: 0x04000E39 RID: 3641
	[HideInInspector]
	public int m_iPerfectPer;

	// Token: 0x04000E3A RID: 3642
	private bool m_bPerfectAniState;

	// Token: 0x04000E3B RID: 3643
	private UILabel m_lGreat;

	// Token: 0x04000E3C RID: 3644
	private UILabel m_lGreatPer;

	// Token: 0x04000E3D RID: 3645
	private TweenPosition m_GreatAni;

	// Token: 0x04000E3E RID: 3646
	[HideInInspector]
	public int m_iGreat;

	// Token: 0x04000E3F RID: 3647
	[HideInInspector]
	public int m_iGreatPer;

	// Token: 0x04000E40 RID: 3648
	private bool m_bGreatAniState;

	// Token: 0x04000E41 RID: 3649
	private UILabel m_lGood;

	// Token: 0x04000E42 RID: 3650
	private UILabel m_lGoodPer;

	// Token: 0x04000E43 RID: 3651
	private TweenPosition m_GoodAni;

	// Token: 0x04000E44 RID: 3652
	[HideInInspector]
	public int m_iGood;

	// Token: 0x04000E45 RID: 3653
	[HideInInspector]
	public int m_iGoodPer;

	// Token: 0x04000E46 RID: 3654
	private bool m_bGoodAniState;

	// Token: 0x04000E47 RID: 3655
	private UILabel m_lPoor;

	// Token: 0x04000E48 RID: 3656
	private UILabel m_lPoorPer;

	// Token: 0x04000E49 RID: 3657
	private TweenPosition m_PoorAni;

	// Token: 0x04000E4A RID: 3658
	[HideInInspector]
	public int m_iPoor;

	// Token: 0x04000E4B RID: 3659
	[HideInInspector]
	public int m_iPoorPer;

	// Token: 0x04000E4C RID: 3660
	private bool m_bPoorAniState;

	// Token: 0x04000E4D RID: 3661
	private UILabel m_lBreak;

	// Token: 0x04000E4E RID: 3662
	private UILabel m_lBreakPer;

	// Token: 0x04000E4F RID: 3663
	private TweenPosition m_BreakAni;

	// Token: 0x04000E50 RID: 3664
	[HideInInspector]
	public int m_iBreak;

	// Token: 0x04000E51 RID: 3665
	[HideInInspector]
	public int m_iBreakPer;

	// Token: 0x04000E52 RID: 3666
	private bool m_bBreakAniState;

	// Token: 0x04000E53 RID: 3667
	private UILabel m_lTotalAccuracy;

	// Token: 0x04000E54 RID: 3668
	private UILabel m_lTotalAccuracyPer;

	// Token: 0x04000E55 RID: 3669
	private TweenPosition m_TotalAccuracyAni;

	// Token: 0x04000E56 RID: 3670
	private UISprite m_spTotalAccuracyUpDownIcon;

	// Token: 0x04000E57 RID: 3671
	[HideInInspector]
	public int m_iTotalAccuracyPer;

	// Token: 0x04000E58 RID: 3672
	private bool m_bTotalAccuracyAniState;

	// Token: 0x04000E59 RID: 3673
	private UILabel m_lMaxCombo;

	// Token: 0x04000E5A RID: 3674
	private UILabel m_lMaxComboBefore;

	// Token: 0x04000E5B RID: 3675
	private TweenPosition m_MaxComboAni;

	// Token: 0x04000E5C RID: 3676
	private UISprite m_spMaxComboUpDownIcon;

	// Token: 0x04000E5D RID: 3677
	[HideInInspector]
	public int m_iMaxComboBefore;

	// Token: 0x04000E5E RID: 3678
	[HideInInspector]
	public int m_iMaxCombo;

	// Token: 0x04000E5F RID: 3679
	private bool m_bMaxComboAniState;

	// Token: 0x04000E60 RID: 3680
	private bool m_bAniEndState;
}
