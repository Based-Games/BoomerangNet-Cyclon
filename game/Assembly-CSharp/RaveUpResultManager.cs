using System;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class RaveUpResultManager : MonoBehaviour
{
	// Token: 0x06000F37 RID: 3895 RVA: 0x0000D1CB File Offset: 0x0000B3CB
	private void Awake()
	{
		this.m_RaveUpResult = base.transform.FindChild("Panel_PlayResult").GetComponent<RaveUpResult>();
		this.m_HouseMixRankManager = base.transform.FindChild("Panel_Rank").GetComponent<HouseMixRankManager>();
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x0006D8C8 File Offset: 0x0006BAC8
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_raveup_result", true);
		base.Invoke("GetResultRank", Time.deltaTime);
		base.Invoke("StageSound", 1f);
		if (Singleton<GameManager>.instance.RaveUpHurdleFail)
		{
			for (int i = 0; i < this.titles.Length; i++)
			{
				this.titles[i].spriteName = "stagefailed";
				this.titles[i].MakePixelPerfect();
				this.titles[i].transform.localScale = Vector3.one * 2f;
			}
		}
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x0000B2B4 File Offset: 0x000094B4
	private void StageSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_STAGECLEARED, false);
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x0006D964 File Offset: 0x0006BB64
	private void GetResultRank()
	{
		AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
		WWWGetRaveUpResultNearRanking wwwgetRaveUpResultNearRanking = new WWWGetRaveUpResultNearRanking();
		wwwgetRaveUpResultNearRanking.strAlbumId = currentAlbum.AlbumServerId;
		wwwgetRaveUpResultNearRanking.CallBack = new WWWObject.CompleteCallBack(this.CallBackComplete);
		wwwgetRaveUpResultNearRanking.CallBackFail = new WWWObject.CompleteCallBack(this.CallBackFail);
		wwwgetRaveUpResultNearRanking.Score = Singleton<GameManager>.instance.ResultData.SCORE;
		Singleton<WWWManager>.instance.AddQueue(wwwgetRaveUpResultNearRanking);
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0000D203 File Offset: 0x0000B403
	private void CallBackComplete()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x0000D203 File Offset: 0x0000B403
	private void CallBackFail()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00063718 File Offset: 0x00061918
	private void OnDestroy()
	{
		for (int i = 0; i < 75; i++)
		{
			Singleton<SoundSourceManager>.instance.Stop((SOUNDINDEX)i);
		}
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x040010D4 RID: 4308
	private RaveUpResult m_RaveUpResult;

	// Token: 0x040010D5 RID: 4309
	private HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x040010D6 RID: 4310
	public UISprite[] titles;

	// Token: 0x0200020E RID: 526
	public enum RaveUpResultStepKind_e
	{
		// Token: 0x040010D8 RID: 4312
		Step1,
		// Token: 0x040010D9 RID: 4313
		Step2,
		// Token: 0x040010DA RID: 4314
		Step3,
		// Token: 0x040010DB RID: 4315
		Step4,
		// Token: 0x040010DC RID: 4316
		Step5,
		// Token: 0x040010DD RID: 4317
		Step6,
		// Token: 0x040010DE RID: 4318
		Step7,
		// Token: 0x040010DF RID: 4319
		Step8,
		// Token: 0x040010E0 RID: 4320
		Step9,
		// Token: 0x040010E1 RID: 4321
		Step10,
		// Token: 0x040010E2 RID: 4322
		Step11,
		// Token: 0x040010E3 RID: 4323
		Step12,
		// Token: 0x040010E4 RID: 4324
		Step13,
		// Token: 0x040010E5 RID: 4325
		Step14,
		// Token: 0x040010E6 RID: 4326
		Step15,
		// Token: 0x040010E7 RID: 4327
		Step16,
		// Token: 0x040010E8 RID: 4328
		Step17,
		// Token: 0x040010E9 RID: 4329
		Step18,
		// Token: 0x040010EA RID: 4330
		Max,
		// Token: 0x040010EB RID: 4331
		None
	}
}
