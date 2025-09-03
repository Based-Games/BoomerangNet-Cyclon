using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class ClubTourResultAccuracy : MonoBehaviour
{
	// Token: 0x06000C1F RID: 3103 RVA: 0x00055DA0 File Offset: 0x00053FA0
	private void Awake()
	{
		this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
		this.m_Step_1_Ani = base.transform.FindChild("Step_1_Ani").GetComponent<TweenPosition>();
		this.m_Step_2_Ani = base.transform.FindChild("Step_2_Ani").GetComponent<TweenPosition>();
		this.m_Step_3_Ani = base.transform.FindChild("Step_3_Ani").GetComponent<TweenPosition>();
		Transform transform = base.transform.FindChild("6.TotalAccuracy");
		this.m_lTotalAccuracy = transform.FindChild("Label_score1").GetComponent<UILabel>();
		this.m_lTotalAccuracyPer = transform.FindChild("Label_score2").GetComponent<UILabel>();
		this.m_spTotalAccuracyUpDownIcon = transform.FindChild("Sprite_UpDown").GetComponent<UISprite>();
		Transform transform2 = base.transform.FindChild("7.maxcombo");
		this.m_lMaxCombo = transform2.FindChild("Label_score1").GetComponent<UILabel>();
		this.m_lMaxComboBefore = transform2.FindChild("Label_score2").GetComponent<UILabel>();
		this.m_spMaxComboUpDownIcon = transform2.FindChild("Sprite_UpDown").GetComponent<UISprite>();
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00055EBC File Offset: 0x000540BC
	private void Start()
	{
		this.m_Step_1_Ani.duration = (this.m_Step_2_Ani.duration = this.m_fDurationTime);
		this.m_Step_1_Ani.to = (this.m_Step_2_Ani.to = this.m_v3TweenToPos);
		this.m_lTotalAccuracy.text = "0";
		this.m_lTotalAccuracyPer.text = "0";
		this.m_lMaxCombo.text = "0";
		this.m_lMaxComboBefore.text = "0";
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0000B25E File Offset: 0x0000945E
	public void DefaultAccuracySetting()
	{
		this.m_iTotalAccuracy = 99;
		this.m_iMaxCombo = 4523;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00055F48 File Offset: 0x00054148
	public void AccuracySetting()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		this.m_iTotalAccuracy = resultData.GetAccuracy();
		this.m_iMaxCombo = resultData.MAXCOMBO;
		this.m_iMaxComboBefore = Singleton<GameManager>.instance.netUserRecordData.MaxCombo;
		if (this.m_iMaxComboBefore == -1)
		{
			this.m_iMaxComboBefore = 0;
		}
		this.m_iTotalAccuracyPer = Singleton<GameManager>.instance.netUserRecordData.Accuracy;
		if (this.m_iTotalAccuracyPer == -1)
		{
			this.m_iTotalAccuracyPer = 0;
		}
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x0000B273 File Offset: 0x00009473
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00055FC8 File Offset: 0x000541C8
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

	// Token: 0x06000C25 RID: 3109 RVA: 0x00056358 File Offset: 0x00054558
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		switch (this.m_AniStep)
		{
		case ResultManager.StepKind_e.Step1:
			if (this.m_iTotalAccuracy > 0 || this.m_iTotalAccuracyPer > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT, true);
			}
			this.m_Step_1_Ani.enabled = true;
			this.PlayAni(this.m_bTotalAccuracyAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iTotalAccuracy, this.m_iTotalAccuracyPer, this.m_lTotalAccuracy.gameObject, this.m_lTotalAccuracyPer.gameObject, false, true, true, this.m_spTotalAccuracyUpDownIcon);
			if (this.m_Step_1_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT);
				this.m_AniStep++;
			}
			break;
		case ResultManager.StepKind_e.Step2:
			if (this.m_iMaxCombo > 0 || this.m_iMaxComboBefore > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_SCORE_COUNT, true);
			}
			this.m_Step_2_Ani.enabled = true;
			this.PlayAni(this.m_bMaxComboAniState, this.m_Step_2_Ani.transform.localPosition.x, this.m_iMaxCombo, this.m_iMaxComboBefore, this.m_lMaxCombo.gameObject, this.m_lMaxComboBefore.gameObject, false, false, false, this.m_spMaxComboUpDownIcon);
			if (this.m_Step_2_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
				this.m_bAniEndState = true;
			}
			break;
		}
	}

	// Token: 0x04000BEA RID: 3050
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000BEB RID: 3051
	private float m_fDurationTime = 0.25f;

	// Token: 0x04000BEC RID: 3052
	private Vector3 m_v3TweenToPos = new Vector3(1f, 0f, 0f);

	// Token: 0x04000BED RID: 3053
	[HideInInspector]
	public int m_iMaxNodeCount;

	// Token: 0x04000BEE RID: 3054
	private RaveUpResult m_RaveUpResult;

	// Token: 0x04000BEF RID: 3055
	private TweenPosition m_Step_1_Ani;

	// Token: 0x04000BF0 RID: 3056
	private TweenPosition m_Step_2_Ani;

	// Token: 0x04000BF1 RID: 3057
	private TweenPosition m_Step_3_Ani;

	// Token: 0x04000BF2 RID: 3058
	private UILabel m_lTotalAccuracy;

	// Token: 0x04000BF3 RID: 3059
	private UILabel m_lTotalAccuracyPer;

	// Token: 0x04000BF4 RID: 3060
	private TweenPosition m_TotalAccuracyAni;

	// Token: 0x04000BF5 RID: 3061
	private UISprite m_spTotalAccuracyUpDownIcon;

	// Token: 0x04000BF6 RID: 3062
	[HideInInspector]
	public int m_iTotalAccuracy;

	// Token: 0x04000BF7 RID: 3063
	[HideInInspector]
	public int m_iTotalAccuracyPer;

	// Token: 0x04000BF8 RID: 3064
	private bool m_bTotalAccuracyAniState;

	// Token: 0x04000BF9 RID: 3065
	private UILabel m_lMaxCombo;

	// Token: 0x04000BFA RID: 3066
	private UILabel m_lMaxComboBefore;

	// Token: 0x04000BFB RID: 3067
	private TweenPosition m_MaxComboAni;

	// Token: 0x04000BFC RID: 3068
	private UISprite m_spMaxComboUpDownIcon;

	// Token: 0x04000BFD RID: 3069
	[HideInInspector]
	public int m_iMaxComboBefore;

	// Token: 0x04000BFE RID: 3070
	[HideInInspector]
	public int m_iMaxCombo;

	// Token: 0x04000BFF RID: 3071
	private bool m_bMaxComboAniState;

	// Token: 0x04000C00 RID: 3072
	private bool m_bAniEndState;
}
