using System;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class ResultScore : MonoBehaviour
{
	// Token: 0x06000E46 RID: 3654 RVA: 0x00066D50 File Offset: 0x00064F50
	private void Awake()
	{
		this.m_Step_1_Ani = base.transform.FindChild("Step_1_Ani").GetComponent<TweenPosition>();
		this.m_Step_2_Ani = base.transform.FindChild("Step_2_Ani").GetComponent<TweenPosition>();
		this.m_lFeverBonus = base.transform.FindChild("1_feverbonus").FindChild("Label_score").GetComponent<UILabel>();
		this.m_lExtreamBonus = base.transform.FindChild("2_extreambonus").FindChild("Label_score").GetComponent<UILabel>();
		this.m_lMaxComboBonus = base.transform.FindChild("3_maxcombobonus").FindChild("Label_score").GetComponent<UILabel>();
		this.m_ilTotalScore = base.transform.FindChild("4_totalscore").FindChild("text").GetComponent<ImageFontLabel>();
		switch (this.m_ResultKind)
		{
		case ResultKind_e.RaveUpClearResult:
			this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
			break;
		}
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x00066E74 File Offset: 0x00065074
	private void Start()
	{
		this.m_Step_1_Ani.duration = (this.m_Step_2_Ani.duration = this.m_fDurationTime);
		this.m_Step_1_Ani.to = (this.m_Step_2_Ani.to = this.m_v3TweenToPos);
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x00066EC0 File Offset: 0x000650C0
	public void DefaultScoreSetting()
	{
		this.m_iFeverBonus = 232323;
		this.m_iExtreamBonus = 454545;
		this.m_iMaxComboBonus = 151515;
		int num = 123124;
		this.m_iTotalScore = num + this.m_iFeverBonus + this.m_iExtreamBonus + this.m_iMaxComboBonus;
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x00066F10 File Offset: 0x00065110
	public void ScoreSetting()
	{
		this.m_lFeverBonus.text = "0";
		this.m_lExtreamBonus.text = "0";
		this.m_lMaxComboBonus.text = "0";
		this.m_ilTotalScore.text = "0";
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		this.m_iFeverBonus = resultData.GetFeverBonus();
		this.m_iExtreamBonus = resultData.EXTREMEBONUS;
		this.m_iMaxComboBonus = resultData.GetMaxComboBonus();
		this.m_iTotalScore = resultData.SCORE;
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x0000C70F File Offset: 0x0000A90F
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x00055FC8 File Offset: 0x000541C8
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

	// Token: 0x06000E4C RID: 3660 RVA: 0x00066F98 File Offset: 0x00065198
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		ResultManager.StepKind_e aniStep = this.m_AniStep;
		if (aniStep != ResultManager.StepKind_e.Step1)
		{
			if (aniStep == ResultManager.StepKind_e.Step2)
			{
				if (this.m_iTotalScore > 0)
				{
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_SCORE_COUNT, true);
				}
				this.m_Step_2_Ani.enabled = true;
				this.PlayAni(this.m_bTotalScoreAniState, this.m_Step_2_Ani.transform.localPosition.x, this.m_iTotalScore, 0, this.m_ilTotalScore.gameObject, null, false, false, false, null);
				if (this.m_Step_2_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
				{
					Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT);
					this.m_bAniEndState = true;
				}
			}
		}
		else
		{
			if (this.m_iFeverBonus > 0 || this.m_iExtreamBonus > 0 || this.m_iMaxComboBonus > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_SCORE_COUNT, true);
			}
			this.m_Step_1_Ani.enabled = true;
			this.PlayAni(this.m_bFeverBonusAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iFeverBonus, 0, this.m_lFeverBonus.gameObject, null, true, false, false, null);
			this.PlayAni(this.m_bExtreamBonusAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iExtreamBonus, 0, this.m_lExtreamBonus.gameObject, null, true, false, false, null);
			this.PlayAni(this.m_bMaxComboBonusAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iMaxComboBonus, 0, this.m_lMaxComboBonus.gameObject, null, true, false, false, null);
			if (this.m_Step_1_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
				this.m_AniStep++;
			}
		}
	}

	// Token: 0x04000F4D RID: 3917
	public ResultKind_e m_ResultKind;

	// Token: 0x04000F4E RID: 3918
	private RaveUpResult m_RaveUpResult;

	// Token: 0x04000F4F RID: 3919
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000F50 RID: 3920
	private float m_fDurationTime = 0.25f;

	// Token: 0x04000F51 RID: 3921
	private Vector3 m_v3TweenToPos = new Vector3(1f, 0f, 0f);

	// Token: 0x04000F52 RID: 3922
	private TweenPosition m_Step_1_Ani;

	// Token: 0x04000F53 RID: 3923
	private TweenPosition m_Step_2_Ani;

	// Token: 0x04000F54 RID: 3924
	private UILabel m_lFeverBonus;

	// Token: 0x04000F55 RID: 3925
	private int m_iFeverBonus;

	// Token: 0x04000F56 RID: 3926
	private bool m_bFeverBonusAniState;

	// Token: 0x04000F57 RID: 3927
	private UILabel m_lExtreamBonus;

	// Token: 0x04000F58 RID: 3928
	private int m_iExtreamBonus;

	// Token: 0x04000F59 RID: 3929
	private bool m_bExtreamBonusAniState;

	// Token: 0x04000F5A RID: 3930
	private UILabel m_lMaxComboBonus;

	// Token: 0x04000F5B RID: 3931
	private int m_iMaxComboBonus;

	// Token: 0x04000F5C RID: 3932
	private bool m_bMaxComboBonusAniState;

	// Token: 0x04000F5D RID: 3933
	private ImageFontLabel m_ilTotalScore;

	// Token: 0x04000F5E RID: 3934
	private int m_iTotalScore;

	// Token: 0x04000F5F RID: 3935
	private bool m_bTotalScoreAniState;

	// Token: 0x04000F60 RID: 3936
	private bool m_bAniEndState;
}
