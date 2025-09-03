using System;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class ResultAllClearScore : MonoBehaviour
{
	// Token: 0x06000DEB RID: 3563 RVA: 0x00063928 File Offset: 0x00061B28
	private void Awake()
	{
		this.m_Step_1_Ani = base.transform.FindChild("Step_1_Ani").GetComponent<TweenPosition>();
		this.m_ilTotalScore = base.transform.FindChild("4_totalscore").FindChild("text").GetComponent<ImageFontLabel>();
		this.m_ResultAllClearPlayResult = base.transform.parent.GetComponent<ResultAllClearPlayResult>();
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0000C3E0 File Offset: 0x0000A5E0
	private void Start()
	{
		this.m_Step_1_Ani.duration = this.m_fDurationTime;
		this.m_Step_1_Ani.to = this.m_v3TweenToPos;
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0006398C File Offset: 0x00061B8C
	public void DefaultScoreSetting()
	{
		for (int i = 0; i < this.MaxDiscCount; i++)
		{
			this.m_iTotalScore += 30000;
		}
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000639C4 File Offset: 0x00061BC4
	public void ScoreSetting()
	{
		this.m_ilTotalScore.text = "0";
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		this.m_ResultAllClearPlayResult.m_AllClearPostInfo.m_iTotalScore = resultData.SCORE;
		this.m_iTotalScore = resultData.SCORE;
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0000C404 File Offset: 0x0000A604
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00055FC8 File Offset: 0x000541C8
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

	// Token: 0x06000DF1 RID: 3569 RVA: 0x00063A10 File Offset: 0x00061C10
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		ResultManager.StepKind_e aniStep = this.m_AniStep;
		if (aniStep == ResultManager.StepKind_e.Step1)
		{
			if (this.m_iTotalScore > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_SCORE_COUNT, true);
			}
			this.m_Step_1_Ani.enabled = true;
			this.PlayAni(this.m_bTotalScoreAniState, this.m_Step_1_Ani.transform.localPosition.x, this.m_iTotalScore, 0, this.m_ilTotalScore.gameObject, null, false, false, false, null);
			if (this.m_Step_1_Ani.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
				this.m_AniStep++;
			}
		}
	}

	// Token: 0x04000EA1 RID: 3745
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000EA2 RID: 3746
	private float m_fDurationTime = 0.25f;

	// Token: 0x04000EA3 RID: 3747
	private Vector3 m_v3TweenToPos = new Vector3(1f, 0f, 0f);

	// Token: 0x04000EA4 RID: 3748
	private TweenPosition m_Step_1_Ani;

	// Token: 0x04000EA5 RID: 3749
	private ImageFontLabel m_ilTotalScore;

	// Token: 0x04000EA6 RID: 3750
	private int m_iTotalScore;

	// Token: 0x04000EA7 RID: 3751
	private bool m_bTotalScoreAniState;

	// Token: 0x04000EA8 RID: 3752
	private bool m_bAniEndState;

	// Token: 0x04000EA9 RID: 3753
	private int MaxDiscCount = 3;

	// Token: 0x04000EAA RID: 3754
	private ResultAllClearPlayResult m_ResultAllClearPlayResult;
}
