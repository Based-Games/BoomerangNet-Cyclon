using System;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class RaveUpAlbumDiscInfo : MonoBehaviour
{
	// Token: 0x06000F19 RID: 3865 RVA: 0x0006CD48 File Offset: 0x0006AF48
	private void Awake()
	{
		this.m_lAlbumName = base.transform.FindChild("Label_albumName").GetComponent<UILabel>();
		this.m_lAlbumName_Han = base.transform.FindChild("Label_albumName_Han").GetComponent<UILabel>();
		this.m_lAlbumKind = base.transform.FindChild("Label_albumKind").GetComponent<UILabel>();
		this.m_txAlbumTexture = base.transform.FindChild("Album_Texture").GetComponent<UITexture>();
		this.m_txDiscAlbumTexutre = new UITexture[this.m_iDiscMaxCount];
		this.m_spDiscLevel = new UISprite[this.m_iDiscMaxCount];
		for (int i = 0; i < this.m_iDiscMaxCount; i++)
		{
			Transform transform = base.transform.FindChild("disc_" + (i + 1).ToString());
			this.m_txDiscAlbumTexutre[i] = transform.FindChild("disc_Texture").GetComponent<UITexture>();
			this.m_spDiscLevel[i] = transform.FindChild("Sprite_Level").GetComponent<UISprite>();
		}
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x0006CE4C File Offset: 0x0006B04C
	public void DefaultDiscInfoSetting()
	{
		string text = "EASY PERFORMANCE";
		bool flag = false;
		this.m_lAlbumName.gameObject.SetActive(false);
		this.m_lAlbumName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lAlbumName.gameObject.SetActive(true);
			this.m_lAlbumName.text = "Default Name";
		}
		else
		{
			this.m_lAlbumName_Han.gameObject.SetActive(true);
			this.m_lAlbumName_Han.text = "Default Name";
		}
		this.m_lAlbumKind.text = text;
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0006CEDC File Offset: 0x0006B0DC
	public void DiscInfoSetting()
	{
		AlbumInfo albumInfo = Singleton<SongManager>.instance.GetAlbumInfo(Singleton<SongManager>.instance.SelectAlbumId);
		string fullName = albumInfo.FullName;
		string text = "EASY PERFORMANCE";
		switch (albumInfo.eDifficult)
		{
		case DISCSET_DIFFICULT.EASY:
			text = "EASY PERFORMANCE";
			break;
		case DISCSET_DIFFICULT.NORMAL:
			text = "NORMAL PERFORMANCE";
			break;
		case DISCSET_DIFFICULT.HARD:
			text = "HARD PERFORMANCE";
			break;
		}
		this.m_lAlbumKind.text = text;
		bool flag = GameData.isContainHangul(fullName);
		this.m_lAlbumName.gameObject.SetActive(false);
		this.m_lAlbumName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lAlbumName.gameObject.SetActive(true);
			this.m_lAlbumName.text = fullName;
		}
		else
		{
			this.m_lAlbumName_Han.gameObject.SetActive(true);
			this.m_lAlbumName_Han.text = fullName;
		}
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.ALBUM_633, null, this.m_txAlbumTexture, albumInfo, null);
		for (int i = 0; i < this.m_iDiscMaxCount - 1; i++)
		{
			RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(i);
			this.m_spDiscLevel[i].spriteName = "level_" + stageResult.PTTYPE.ToString().ToLower() + "_sm";
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, stageResult.DISCINFO, this.m_txDiscAlbumTexutre[i], null, null);
		}
		if (!Singleton<GameManager>.instance.RaveUpHurdleFail)
		{
			this.m_spDiscLevel[3].alpha = 0.8f;
			RESULTDATA stageResult2 = Singleton<GameManager>.instance.GetStageResult(3);
			this.m_spDiscLevel[3].spriteName = "level_" + stageResult2.PTTYPE.ToString().ToLower() + "_sm";
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, stageResult2.DISCINFO, this.m_txDiscAlbumTexutre[3], null, null);
		}
	}

	// Token: 0x04001099 RID: 4249
	private UILabel m_lAlbumName;

	// Token: 0x0400109A RID: 4250
	private UILabel m_lAlbumName_Han;

	// Token: 0x0400109B RID: 4251
	private UILabel m_lAlbumKind;

	// Token: 0x0400109C RID: 4252
	private UITexture m_txAlbumTexture;

	// Token: 0x0400109D RID: 4253
	private UITexture[] m_txDiscAlbumTexutre;

	// Token: 0x0400109E RID: 4254
	private UISprite[] m_spDiscLevel;

	// Token: 0x0400109F RID: 4255
	private int m_iDiscMaxCount = 4;
}
