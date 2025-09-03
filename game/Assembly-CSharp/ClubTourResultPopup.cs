using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class ClubTourResultPopup : MonoBehaviour
{
	// Token: 0x06000C37 RID: 3127 RVA: 0x00056DF8 File Offset: 0x00054FF8
	private void Awake()
	{
		this.m_tPopup = base.transform.FindChild("popup");
		this.m_lBeatPoint = this.m_tPopup.FindChild("BeatPoint").FindChild("Label").GetComponent<UILabel>();
		this.m_lExp = this.m_tPopup.FindChild("Exp").FindChild("Label").GetComponent<UILabel>();
		this.m_tPopup_Song = base.transform.FindChild("popup_Song");
		this.m_txCDImage = this.m_tPopup_Song.FindChild("Texture_CD").GetComponent<UITexture>();
		this.m_spPT = this.m_txCDImage.transform.FindChild("Sprite_PT").GetComponent<UISprite>();
		this.m_lSongName = this.m_tPopup_Song.FindChild("Songtxt").GetComponent<UILabel>();
		this.m_lSongName_Han = this.m_tPopup_Song.FindChild("Songtxt_Han").GetComponent<UILabel>();
		this.m_lItemMsg = base.transform.FindChild("popup_Song").FindChild("hantxt").GetComponent<UILabel>();
		this.m_lSongArtist = base.transform.FindChild("popup_Song").FindChild("Song_Artist").GetComponent<UILabel>();
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x00056F38 File Offset: 0x00055138
	private void init()
	{
		this.m_MissionData = Singleton<SongManager>.instance.Mission;
		this.m_lBeatPoint.text = "+ " + this.m_MissionData.iRewardBeatPoint.ToString();
		this.m_lExp.text = "+ " + this.m_MissionData.iRewardExp.ToString();
		this.m_bUseSongPopup = false;
		if (this.m_MissionData.iRewardSong != 0)
		{
			this.m_lItemMsg.text = "You have unlocked a new chart!";
			this.m_bUseSongPopup = true;
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(this.m_MissionData.iRewardSong);
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, discInfo, this.m_txCDImage, null, null);
			string fullName = discInfo.FullName;
			bool flag = GameData.isContainHangul(fullName);
			this.m_lSongName_Han.gameObject.SetActive(false);
			this.m_lSongName.gameObject.SetActive(false);
			if (flag)
			{
				this.m_lSongName_Han.gameObject.SetActive(true);
				this.m_lSongName_Han.text = fullName.ToUpper();
			}
			else
			{
				this.m_lSongName.gameObject.SetActive(true);
				this.m_lSongName.text = fullName.ToUpper();
			}
			this.m_lSongArtist.text = discInfo.Artist;
			this.m_spPT.spriteName = "level_" + this.m_MissionData.RewardPattern[0].ToString().ToLower() + "_sm";
		}
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x000570BC File Offset: 0x000552BC
	public void CheckSongPopup()
	{
		this.init();
		if (this.m_bUseSongPopup)
		{
			if (!Singleton<SongManager>.instance.Mission.Cleared)
			{
				base.Invoke("SongPopupOpen", 3f);
				base.Invoke("On", 5f);
			}
			else
			{
				base.Invoke("On", 2f);
			}
		}
		else
		{
			base.Invoke("On", 2f);
		}
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0000B2F9 File Offset: 0x000094F9
	public void SongPopupOpen()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_POPUP, false);
		this.m_tPopup.gameObject.SetActive(false);
		this.m_tPopup_Song.GetComponent<TweenRotation>().enabled = true;
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0000B32A File Offset: 0x0000952A
	private void On()
	{
		this.m_bCloseState = true;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0000B333 File Offset: 0x00009533
	private void Close()
	{
		if (!this.m_bCloseState)
		{
			return;
		}
		UnityEngine.Object.DestroyObject(base.gameObject);
	}

	// Token: 0x04000C2C RID: 3116
	private UILabel m_lBeatPoint;

	// Token: 0x04000C2D RID: 3117
	private UILabel m_lExp;

	// Token: 0x04000C2E RID: 3118
	private Transform m_tPopup;

	// Token: 0x04000C2F RID: 3119
	private Transform m_tPopup_Song;

	// Token: 0x04000C30 RID: 3120
	private UITexture m_txCDImage;

	// Token: 0x04000C31 RID: 3121
	private UISprite m_spPT;

	// Token: 0x04000C32 RID: 3122
	private UILabel m_lSongName;

	// Token: 0x04000C33 RID: 3123
	private UILabel m_lSongName_Han;

	// Token: 0x04000C34 RID: 3124
	private UILabel m_lItemMsg;

	// Token: 0x04000C35 RID: 3125
	private bool m_bUseSongPopup;

	// Token: 0x04000C36 RID: 3126
	private UILabel m_lSongArtist;

	// Token: 0x04000C37 RID: 3127
	private bool m_bCloseState;

	// Token: 0x04000C38 RID: 3128
	private MissionData m_MissionData;
}
