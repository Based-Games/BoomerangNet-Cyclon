using System;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class ResultManager : MonoBehaviour
{
	// Token: 0x06000E0C RID: 3596 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
	private void Awake()
	{
		this.m_ResultPlayResult = base.transform.FindChild("Panel_PlayResult").GetComponent<ResultPlayResult>();
		this.m_HouseMixRankManager = base.transform.FindChild("Panel_Rank").GetComponent<HouseMixRankManager>();
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00065088 File Offset: 0x00063288
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_result", true);
		base.Invoke("StageSound", 1.125f);
		base.Invoke("GetResultRank", Time.deltaTime);
		this.StageNumImage.spriteName = "stage_" + (GameData.Stage + 1).ToString();
		this.StageNumImage.MakePixelPerfect();
		this.StageNumImage.transform.localScale = new Vector3(2f, 2f, 1f);
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_OUTGAME);
		Singleton<GameManager>.instance.SetSceneControl(base.gameObject);
		if (!Singleton<GameManager>.instance.ONNETWORK)
		{
			Singleton<GameManager>.instance.CreatePopUp(POPUPTYPE.NETWORK_NOTSAVEDATA);
		}
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Viewing stage results", null);
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0000B3C4 File Offset: 0x000095C4
	private void SetPause(bool bEnabled)
	{
		base.enabled = !bEnabled;
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0000B2B4 File Offset: 0x000094B4
	private void StageSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_STAGECLEARED, false);
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x0006515C File Offset: 0x0006335C
	private void GetResultRank()
	{
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			this.CallBackFail();
			return;
		}
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		WWWHausMixResult wwwhausMixResult = new WWWHausMixResult();
		wwwhausMixResult.MusicId = currentDisc.ServerID;
		wwwhausMixResult.CallBack = new WWWObject.CompleteCallBack(this.CallBackComplete);
		wwwhausMixResult.CallBackFail = new WWWObject.CompleteCallBack(this.CallBackFail);
		wwwhausMixResult.Score = Singleton<GameManager>.instance.ResultData.SCORE;
		Singleton<WWWManager>.instance.AddQueue(wwwhausMixResult);
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x0000C518 File Offset: 0x0000A718
	private void CallBackComplete()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0000C518 File Offset: 0x0000A718
	private void CallBackFail()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x00063718 File Offset: 0x00061918
	private void OnDestroy()
	{
		for (int i = 0; i < 75; i++)
		{
			Singleton<SoundSourceManager>.instance.Stop((SOUNDINDEX)i);
		}
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x04000EE4 RID: 3812
	public UISprite StageNumImage;

	// Token: 0x04000EE5 RID: 3813
	private ResultPlayResult m_ResultPlayResult;

	// Token: 0x04000EE6 RID: 3814
	private HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x020001E5 RID: 485
	public enum StepKind_e
	{
		// Token: 0x04000EE8 RID: 3816
		Step1,
		// Token: 0x04000EE9 RID: 3817
		Step2,
		// Token: 0x04000EEA RID: 3818
		Step3,
		// Token: 0x04000EEB RID: 3819
		Step4,
		// Token: 0x04000EEC RID: 3820
		Step5,
		// Token: 0x04000EED RID: 3821
		Step6,
		// Token: 0x04000EEE RID: 3822
		Step7,
		// Token: 0x04000EEF RID: 3823
		Step8,
		// Token: 0x04000EF0 RID: 3824
		Step9,
		// Token: 0x04000EF1 RID: 3825
		Step10,
		// Token: 0x04000EF2 RID: 3826
		Step11,
		// Token: 0x04000EF3 RID: 3827
		Step12,
		// Token: 0x04000EF4 RID: 3828
		Step13,
		// Token: 0x04000EF5 RID: 3829
		Step14,
		// Token: 0x04000EF6 RID: 3830
		Step15,
		// Token: 0x04000EF7 RID: 3831
		Step16,
		// Token: 0x04000EF8 RID: 3832
		Step17,
		// Token: 0x04000EF9 RID: 3833
		Step18,
		// Token: 0x04000EFA RID: 3834
		Max,
		// Token: 0x04000EFB RID: 3835
		None
	}
}
