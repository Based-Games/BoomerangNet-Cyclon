using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class RaveUpSelectAlbum : MonoBehaviour
{
	// Token: 0x06000F0A RID: 3850 RVA: 0x0006C764 File Offset: 0x0006A964
	private void Awake()
	{
		this.m_txSelectAlbum = base.transform.FindChild("AlbumTexture").GetComponent<UITexture>();
		this.m_lAlbumName = base.transform.FindChild("AlbumName").GetComponent<UILabel>();
		this.m_lAlbumKind = base.transform.FindChild("AlbumKind").GetComponent<UILabel>();
		this.m_RaveUpDiscAni = new RaveUpDiscAni[6];
		this.m_txInDiscs = new UITexture[6];
		this.m_sInDiscName = new string[6];
		for (int i = 0; i < this.m_RaveUpDiscAni.Length; i++)
		{
			this.m_RaveUpDiscAni[i] = base.transform.FindChild("Album_DiscSet").FindChild("Disc_" + (i + 1).ToString()).GetComponent<RaveUpDiscAni>();
			this.m_txInDiscs[i] = this.m_RaveUpDiscAni[i].transform.FindChild("DiscImage_ex").GetComponent<UITexture>();
		}
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a disc-set...", "Playing RAVE UP");
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0000D049 File Offset: 0x0000B249
	private void Start()
	{
		if (Singleton<SongManager>.instance.GetAlbumTotalCnt() >= 10)
		{
			this.m_bRollingState = true;
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0000D060 File Offset: 0x0000B260
	private void setSelectAlbumTransform(Transform tAlbum)
	{
		this.m_SelectDiscTrans = tAlbum;
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0000D069 File Offset: 0x0000B269
	private void setAlbumInfo(AlbumInfo ai)
	{
		this.m_AlbumInfo = ai;
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0000D072 File Offset: 0x0000B272
	public void SelectEndMove()
	{
		this.SelectBGMoveEnd = true;
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0006C868 File Offset: 0x0006AA68
	public void DiscAni()
	{
		for (int i = 0; i < this.m_RaveUpDiscAni.Length; i++)
		{
			this.m_RaveUpDiscAni[i].StartAni();
		}
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0006C898 File Offset: 0x0006AA98
	public void setSelectAlbum()
	{
		this.m_arrDiscInfo.Clear();
		this.SelectBGMoveEnd = false;
		ArrayList raveUpAlbumStage = Singleton<SongManager>.instance.GetRaveUpAlbumStage(Singleton<SongManager>.instance.SelectAlbumId);
		for (int i = 0; i < raveUpAlbumStage.Count; i++)
		{
			RaveUpStage raveUpStage = (RaveUpStage)raveUpAlbumStage[i];
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage.iSong);
			this.m_arrDiscInfo.Add(discInfo);
			this.m_RaveUpDiscAni[i].m_iPT = raveUpStage.PtType;
			this.m_RaveUpDiscAni[i].m_DiscInfo = discInfo;
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, discInfo, this.m_txInDiscs[i], null, null);
			this.m_txInDiscs[i].alpha = 0f;
			this.m_sInDiscName[i] = discInfo.Name;
		}
		this.m_lAlbumName.text = this.m_AlbumInfo.FullName;
		switch (Singleton<SongManager>.instance.GetAlbumInfo(Singleton<SongManager>.instance.SelectAlbumId).eDifficult)
		{
		case DISCSET_DIFFICULT.EASY:
			this.m_lAlbumKind.text = "EASY PERFORMANCE";
			break;
		case DISCSET_DIFFICULT.NORMAL:
			this.m_lAlbumKind.text = "NORMAL PERFORMANCE";
			break;
		case DISCSET_DIFFICULT.HARD:
			this.m_lAlbumKind.text = "HARD PERFORMANCE";
			break;
		}
		Vector3 vector = new Vector3(0f, 400f, 0f);
		this.m_SelectBG.from = this.m_SelectBG.transform.localPosition;
		if (this.m_bRollingState)
		{
			this.m_SelectBG.to = this.m_SelectDiscTrans.localPosition;
		}
		else
		{
			this.m_SelectBG.to = this.m_SelectDiscTrans.localPosition + vector;
		}
		this.m_SelectBG.ResetToBeginning();
		this.m_SelectBG.Play(true);
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.ALBUM_633, null, this.m_txSelectAlbum, this.m_AlbumInfo, null);
		base.Invoke("DiscAni", 0.025f);
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0000D07B File Offset: 0x0000B27B
	private void Update()
	{
		if (this.m_bRollingState)
		{
			if (!this.SelectBGMoveEnd)
			{
				return;
			}
			this.m_SelectBG.transform.localPosition = this.m_SelectDiscTrans.localPosition;
		}
	}

	// Token: 0x04001087 RID: 4231
	public TweenPosition m_SelectBG;

	// Token: 0x04001088 RID: 4232
	public ArrayList m_arrDiscInfo = new ArrayList();

	// Token: 0x04001089 RID: 4233
	[HideInInspector]
	public UITexture m_txSelectAlbum;

	// Token: 0x0400108A RID: 4234
	[HideInInspector]
	public UILabel m_lAlbumName;

	// Token: 0x0400108B RID: 4235
	[HideInInspector]
	public UILabel m_lAlbumKind;

	// Token: 0x0400108C RID: 4236
	[HideInInspector]
	public UITexture[] m_txInDiscs;

	// Token: 0x0400108D RID: 4237
	[HideInInspector]
	public string[] m_sInDiscName;

	// Token: 0x0400108E RID: 4238
	private RaveUpDiscAni[] m_RaveUpDiscAni;

	// Token: 0x0400108F RID: 4239
	private Transform m_SelectDiscTrans;

	// Token: 0x04001090 RID: 4240
	private bool SelectBGMoveEnd;

	// Token: 0x04001091 RID: 4241
	private AlbumInfo m_AlbumInfo;

	// Token: 0x04001092 RID: 4242
	private bool m_bRollingState;
}
