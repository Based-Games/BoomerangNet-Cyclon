using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class ClubTourResultManager : MonoBehaviour
{
	// Token: 0x06000C27 RID: 3111 RVA: 0x0000B27C File Offset: 0x0000947C
	private void Awake()
	{
		this.m_RaveUpResult = base.transform.FindChild("Panel_PlayResult").GetComponent<ClubTourResult>();
		this.m_HouseMixRankManager = base.transform.FindChild("Panel_Rank").GetComponent<HouseMixRankManager>();
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0005651C File Offset: 0x0005471C
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_clubtour_result", true);
		base.Invoke("GetResultRank", Time.deltaTime);
		base.Invoke("StageSound", 1f);
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Viewing mission results", null);
		MissionData mission = Singleton<SongManager>.instance.Mission;
		for (int i = 0; i < mission.ArrMissionType.Length; i++)
		{
			if (!mission.AllClear)
			{
				for (int j = 0; j < this.titles.Length; j++)
				{
					this.titles[j].spriteName = "ClubTourResult_missionfailed";
					this.titles[j].MakePixelPerfect();
					this.titles[j].transform.localScale = Vector3.one * 2f;
				}
				return;
			}
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0000B2B4 File Offset: 0x000094B4
	private void StageSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_STAGECLEARED, false);
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x000565E4 File Offset: 0x000547E4
	private void GetResultRank()
	{
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			this.CallBackFail();
			return;
		}
		WWWMissionRankScript wwwmissionRankScript = new WWWMissionRankScript();
		wwwmissionRankScript.CallBack = new WWWObject.CompleteCallBack(this.CallBackComplete);
		wwwmissionRankScript.CallBackFail = new WWWObject.CompleteCallBack(this.CallBackFail);
		wwwmissionRankScript.Score = Singleton<GameManager>.instance.ResultData.SCORE;
		Singleton<WWWManager>.instance.AddQueue(wwwmissionRankScript);
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0000B2C3 File Offset: 0x000094C3
	private void CallBackComplete()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x0000B2C3 File Offset: 0x000094C3
	private void CallBackFail()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x0000B2D0 File Offset: 0x000094D0
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Alpha9))
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("ClubTourResult");
		}
	}

	// Token: 0x04000C01 RID: 3073
	private ClubTourResult m_RaveUpResult;

	// Token: 0x04000C02 RID: 3074
	private HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x04000C03 RID: 3075
	public UISprite[] titles;

	// Token: 0x02000197 RID: 407
	public enum ClubTourResultStepKind_e
	{
		// Token: 0x04000C05 RID: 3077
		Step1,
		// Token: 0x04000C06 RID: 3078
		Step2,
		// Token: 0x04000C07 RID: 3079
		Step3,
		// Token: 0x04000C08 RID: 3080
		Step4,
		// Token: 0x04000C09 RID: 3081
		Step5,
		// Token: 0x04000C0A RID: 3082
		Step6,
		// Token: 0x04000C0B RID: 3083
		Step7,
		// Token: 0x04000C0C RID: 3084
		Step8,
		// Token: 0x04000C0D RID: 3085
		Step9,
		// Token: 0x04000C0E RID: 3086
		Step10,
		// Token: 0x04000C0F RID: 3087
		Step11,
		// Token: 0x04000C10 RID: 3088
		Step12,
		// Token: 0x04000C11 RID: 3089
		Step13,
		// Token: 0x04000C12 RID: 3090
		Step14,
		// Token: 0x04000C13 RID: 3091
		Step15,
		// Token: 0x04000C14 RID: 3092
		Step16,
		// Token: 0x04000C15 RID: 3093
		Step17,
		// Token: 0x04000C16 RID: 3094
		Step18,
		// Token: 0x04000C17 RID: 3095
		Max,
		// Token: 0x04000C18 RID: 3096
		None
	}
}
