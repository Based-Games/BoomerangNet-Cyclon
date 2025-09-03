using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class ResultAllClearManager : MonoBehaviour
{
	// Token: 0x06000DD3 RID: 3539 RVA: 0x0000C260 File Offset: 0x0000A460
	private void Awake()
	{
		this.m_ResultAllClearPlayResult = base.transform.FindChild("Panel_PlayResult").GetComponent<ResultAllClearPlayResult>();
		this.m_HouseMixRankManager = base.transform.FindChild("Panel_Rank").GetComponent<HouseMixRankManager>();
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0006365C File Offset: 0x0006185C
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_total_result", true);
		base.Invoke("GetRanking", Time.deltaTime);
		base.Invoke("StageSound", 1f);
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Viewing game results", null);
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0000B2B4 File Offset: 0x000094B4
	private void StageSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_STAGECLEARED, false);
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x000636AC File Offset: 0x000618AC
	private void GetRanking()
	{
		if (!Singleton<GameManager>.instance.ONNETWORK)
		{
			this.FailCallBack();
			return;
		}
		WWWHausMixTotalResult wwwhausMixTotalResult = new WWWHausMixTotalResult();
		wwwhausMixTotalResult.Score = Singleton<GameManager>.instance.ResultData.SCORE;
		wwwhausMixTotalResult.CallBack = new WWWObject.CompleteCallBack(this.CompleteCallBack);
		wwwhausMixTotalResult.CallBackFail = new WWWObject.CompleteCallBack(this.FailCallBack);
		Singleton<WWWManager>.instance.AddQueue(wwwhausMixTotalResult);
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0000C298 File Offset: 0x0000A498
	private void CompleteCallBack()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0000C298 File Offset: 0x0000A498
	private void FailCallBack()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x00063718 File Offset: 0x00061918
	private void OnDestroy()
	{
		for (int i = 0; i < 75; i++)
		{
			Singleton<SoundSourceManager>.instance.Stop((SOUNDINDEX)i);
		}
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x04000E74 RID: 3700
	private ResultAllClearPlayResult m_ResultAllClearPlayResult;

	// Token: 0x04000E75 RID: 3701
	private HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x020001DD RID: 477
	public enum AllClearStepKind_e
	{
		// Token: 0x04000E77 RID: 3703
		Step1,
		// Token: 0x04000E78 RID: 3704
		Step2,
		// Token: 0x04000E79 RID: 3705
		Step3,
		// Token: 0x04000E7A RID: 3706
		Step4,
		// Token: 0x04000E7B RID: 3707
		Step5,
		// Token: 0x04000E7C RID: 3708
		Step6,
		// Token: 0x04000E7D RID: 3709
		Step7,
		// Token: 0x04000E7E RID: 3710
		Step8,
		// Token: 0x04000E7F RID: 3711
		Step9,
		// Token: 0x04000E80 RID: 3712
		Step10,
		// Token: 0x04000E81 RID: 3713
		Step11,
		// Token: 0x04000E82 RID: 3714
		Step12,
		// Token: 0x04000E83 RID: 3715
		Step13,
		// Token: 0x04000E84 RID: 3716
		Step14,
		// Token: 0x04000E85 RID: 3717
		Step15,
		// Token: 0x04000E86 RID: 3718
		Step16,
		// Token: 0x04000E87 RID: 3719
		Step17,
		// Token: 0x04000E88 RID: 3720
		Step18,
		// Token: 0x04000E89 RID: 3721
		Max,
		// Token: 0x04000E8A RID: 3722
		None
	}
}
