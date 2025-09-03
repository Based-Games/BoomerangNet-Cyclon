using System;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class ResultNewRecord : MonoBehaviour
{
	// Token: 0x06000E1A RID: 3610 RVA: 0x00065618 File Offset: 0x00063818
	private void Awake()
	{
		this.m_NewRecord = base.transform.FindChild("Sprite_NewRecord").GetComponent<TweenAlpha>();
		switch (this.m_ResultKind)
		{
		case ResultKind_e.RaveUpClearResult:
			this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
			break;
		}
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x0000C535 File Offset: 0x0000A735
	private void Start()
	{
		this.m_NewRecord.GetComponent<UISprite>().alpha = 0f;
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00065684 File Offset: 0x00063884
	public void StartAni()
	{
		if (Singleton<GameManager>.instance.ResultData.IsAllComboPlay() || Singleton<GameManager>.instance.ResultData.IsPerfectPlay())
		{
			base.Invoke("StepSetting", 1.5f);
		}
		else
		{
			base.Invoke("StepSetting", Time.deltaTime);
		}
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0000C54C File Offset: 0x0000A74C
	private void StepSetting()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000656E0 File Offset: 0x000638E0
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[resultData.PTTYPE];
		ResultManager.StepKind_e aniStep = this.m_AniStep;
		if (aniStep == ResultManager.StepKind_e.Step1)
		{
			switch (this.m_ResultKind)
			{
			case ResultKind_e.ClearResult:
				if (patternScoreInfo.BestScore < resultData.SCORE)
				{
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_NEWRECORD, false);
					this.m_NewRecord.enabled = true;
				}
				break;
			case ResultKind_e.RaveUpClearResult:
				if (patternScoreInfo.BestScore < resultData.SCORE)
				{
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_NEWRECORD, false);
					this.m_NewRecord.enabled = true;
				}
				break;
			case ResultKind_e.ClubTourClearResult:
				if (patternScoreInfo.BestScore < resultData.SCORE)
				{
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_NEWRECORD, false);
					this.m_NewRecord.enabled = true;
				}
				break;
			}
			this.m_bAniEndState = true;
		}
	}

	// Token: 0x04000F08 RID: 3848
	public ResultKind_e m_ResultKind;

	// Token: 0x04000F09 RID: 3849
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000F0A RID: 3850
	private TweenAlpha m_NewRecord;

	// Token: 0x04000F0B RID: 3851
	private bool m_bAniEndState;

	// Token: 0x04000F0C RID: 3852
	private RaveUpResult m_RaveUpResult;
}
