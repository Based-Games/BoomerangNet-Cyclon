using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class ClubTourResultMissionInfo : MonoBehaviour
{
	// Token: 0x06000C2F RID: 3119 RVA: 0x00056650 File Offset: 0x00054850
	private void Awake()
	{
		this.m_lAlbumName = base.transform.FindChild("Label_albumName").GetComponent<UILabel>();
		this.m_lAlbumName_Han = base.transform.FindChild("Label_albumName_Han").GetComponent<UILabel>();
		this.m_lAlbumKind = base.transform.FindChild("Label_albumKind").GetComponent<UILabel>();
		this.m_txMissionPackTexture = base.transform.FindChild("Album_Texture").GetComponent<UITexture>();
		this.m_txMissionDiscTexutre = new UITexture[this.m_iDiscMaxCount];
		this.m_spDiscLevel = new UISprite[this.m_iDiscMaxCount];
		for (int i = 0; i < this.m_iDiscMaxCount; i++)
		{
			Transform transform = base.transform.FindChild("disc_" + (i + 1).ToString());
			this.m_txMissionDiscTexutre[i] = transform.FindChild("disc_Texture").GetComponent<UITexture>();
			this.m_spDiscLevel[i] = transform.FindChild("Sprite_Level").GetComponent<UISprite>();
		}
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00056754 File Offset: 0x00054954
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
		MissionData mission = Singleton<SongManager>.instance.Mission;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x000567F0 File Offset: 0x000549F0
	public void DiscInfoSetting()
	{
		MissionData mission = Singleton<SongManager>.instance.Mission;
		string strMissionTitle = mission.strMissionTitle;
		bool flag = GameData.isContainHangul(strMissionTitle);
		this.m_lAlbumName.gameObject.SetActive(false);
		this.m_lAlbumName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lAlbumName.gameObject.SetActive(true);
			this.m_lAlbumName.text = strMissionTitle;
		}
		else
		{
			this.m_lAlbumName_Han.gameObject.SetActive(true);
			this.m_lAlbumName_Han.text = strMissionTitle;
		}
		int iPackId = Singleton<SongManager>.instance.Mission.iPackId;
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.MISSIONPACK_287, null, this.m_txMissionPackTexture, null, Singleton<SongManager>.instance.GetMissionPack(iPackId));
		for (int i = 0; i < this.m_iDiscMaxCount; i++)
		{
			RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(i);
			this.m_spDiscLevel[i].spriteName = "level_" + stageResult.PTTYPE.ToString().ToLower() + "_sm";
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, stageResult.DISCINFO, this.m_txMissionDiscTexutre[i], null, null);
		}
	}

	// Token: 0x04000C19 RID: 3097
	private UILabel m_lAlbumName;

	// Token: 0x04000C1A RID: 3098
	private UILabel m_lAlbumName_Han;

	// Token: 0x04000C1B RID: 3099
	private UILabel m_lAlbumKind;

	// Token: 0x04000C1C RID: 3100
	private UITexture m_txMissionPackTexture;

	// Token: 0x04000C1D RID: 3101
	private UITexture[] m_txMissionDiscTexutre;

	// Token: 0x04000C1E RID: 3102
	private UISprite[] m_spDiscLevel;

	// Token: 0x04000C1F RID: 3103
	private int m_iDiscMaxCount = 3;
}
