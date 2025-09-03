using System;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class ResultBeatPoint : MonoBehaviour
{
	// Token: 0x06000DF3 RID: 3571 RVA: 0x00063AE4 File Offset: 0x00061CE4
	private void Awake()
	{
		this.m_ilBeatPointValue = base.transform.FindChild("text").GetComponent<ImageFontLabel>();
		this.m_BeatPointValueAni = base.transform.FindChild("numAni1").GetComponent<TweenPosition>();
		this.m_spBeatPointPlus = base.transform.FindChild("Sprite_Plus").GetComponent<UISprite>();
		switch (this.m_ResultKind)
		{
		case ResultKind_e.AllClearResult:
			this.m_ResultAllClearPlayResult = base.transform.parent.GetComponent<ResultAllClearPlayResult>();
			break;
		case ResultKind_e.RaveUpClearResult:
			this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
			break;
		case ResultKind_e.ClubTourClearResult:
			this.m_ClubTourResult = base.transform.parent.GetComponent<ClubTourResult>();
			break;
		}
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x0000C442 File Offset: 0x0000A642
	private void Start()
	{
		this.m_BeatPointValueAni.duration = this.m_fDurationTime;
		this.m_BeatPointValueAni.to = this.m_v3TweenToPos;
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x0000C466 File Offset: 0x0000A666
	public void DefualtBeatPointSetting()
	{
		this.m_iBeatPoint = 350;
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00063BBC File Offset: 0x00061DBC
	public void BeatPointSetting()
	{
		int num = 0;
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		switch (this.m_ResultKind)
		{
		case ResultKind_e.ClearResult:
			num = resultData.BEATPOINT;
			break;
		case ResultKind_e.AllClearResult:
			num = resultData.ALLCLEAR_BEATPOINT;
			break;
		case ResultKind_e.RaveUpClearResult:
			num = (this.m_RaveUpResult.m_RaveUpPostData.m_iBeatPoint = resultData.BEATPOINT);
			break;
		case ResultKind_e.ClubTourClearResult:
			num = resultData.BEATPOINT;
			break;
		}
		this.m_iSaveBeatPointValue = (this.m_iBeatPoint = num);
		this.m_ilBeatPointValue.text = "0";
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			this.m_BeatPointValueAni.duration = this.m_fDurationTime * 0.01f;
			this.m_iSaveBeatPointValue = (this.m_iBeatPoint = 0);
		}
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x0000C473 File Offset: 0x0000A673
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x00055FC8 File Offset: 0x000541C8
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

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00063CA8 File Offset: 0x00061EA8
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		ResultManager.StepKind_e aniStep = this.m_AniStep;
		if (aniStep == ResultManager.StepKind_e.Step1)
		{
			if (this.m_iBeatPoint > 0)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_BP_COUNT, true);
			}
			this.m_BeatPointValueAni.enabled = true;
			this.PlayAni(this.m_bBeatPointAniState, this.m_BeatPointValueAni.transform.localPosition.x, this.m_iBeatPoint, 0, this.m_ilBeatPointValue.gameObject, null, false, false, false, null);
			if (this.m_BeatPointValueAni.transform.localPosition.x == this.m_v3TweenToPos.x)
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT);
				this.m_bAniEndState = true;
			}
		}
	}

	// Token: 0x04000EAB RID: 3755
	public ResultKind_e m_ResultKind;

	// Token: 0x04000EAC RID: 3756
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000EAD RID: 3757
	private ImageFontLabel m_ilBeatPointValue;

	// Token: 0x04000EAE RID: 3758
	[HideInInspector]
	public int m_iBeatPoint;

	// Token: 0x04000EAF RID: 3759
	private int m_iSaveBeatPointValue;

	// Token: 0x04000EB0 RID: 3760
	private TweenPosition m_BeatPointValueAni;

	// Token: 0x04000EB1 RID: 3761
	private bool m_bBeatPointAniState;

	// Token: 0x04000EB2 RID: 3762
	private UISprite m_spBeatPointPlus;

	// Token: 0x04000EB3 RID: 3763
	private bool m_bAniEndState;

	// Token: 0x04000EB4 RID: 3764
	private float m_fDurationTime = 0.25f;

	// Token: 0x04000EB5 RID: 3765
	private Vector3 m_v3TweenToPos = new Vector3(1f, 0f, 0f);

	// Token: 0x04000EB6 RID: 3766
	private RaveUpResult m_RaveUpResult;

	// Token: 0x04000EB7 RID: 3767
	private ResultAllClearPlayResult m_ResultAllClearPlayResult;

	// Token: 0x04000EB8 RID: 3768
	private ClubTourResult m_ClubTourResult;
}
