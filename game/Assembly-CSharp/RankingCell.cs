using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class RankingCell : MonoBehaviour
{
	// Token: 0x06000EAB RID: 3755 RVA: 0x000696F0 File Offset: 0x000678F0
	private void Awake()
	{
		Transform transform = base.transform;
		Transform transform2 = transform.Find("UserInfo");
		Transform transform3 = transform.Find("Class");
		this.m_tHausMix = transform.Find("DiscInfo_HausMix");
		this.m_tRaveup = transform.Find("DiscInfo_RavuUp");
		this.m_gRankPlus = new GameObject[this.m_iPluseCount];
		this.m_txHausDisc = new UITexture[this.m_iHausDiscCount];
		this.m_spHausDiscPT = new UISprite[this.m_iHausDiscCount];
		this.m_txRaveupDisc = new UITexture[this.m_iRaveupDiscCount];
		this.m_spRaveupDiscPT = new UISprite[this.m_iRaveupDiscCount];
		this.m_ilRankingNum = transform.Find("RankingNum/ImageFont_Num").GetComponent<ImageFontLabel>();
		this.m_spUserPic = transform2.Find("UserImage").GetComponent<UISprite>();
		this.m_lUserName = transform2.Find("Label_Name").GetComponent<UILabel>();
		this.m_ilUserLevel = transform2.Find("ImageFont_UserLv").GetComponent<ImageFontLabel>();
		this.m_ilScore = transform.Find("Score/ImageFont_Score").GetComponent<ImageFontLabel>();
		this.m_spRank = transform3.Find("Sprite_Rank").GetComponent<UISprite>();
		for (int i = 0; i < this.m_iPluseCount; i++)
		{
			this.m_gRankPlus[i] = transform3.Find(string.Format("Sprite_RankPlus_{0}", i + 1)).gameObject;
		}
		for (int j = 0; j < this.m_iHausDiscCount; j++)
		{
			Transform transform4 = this.m_tHausMix.Find(string.Format("Disc_{0}", j + 1));
			this.m_txHausDisc[j] = transform4.Find("Texture_Disc").GetComponent<UITexture>();
			this.m_spHausDiscPT[j] = transform4.Find("Sprite_PT").GetComponent<UISprite>();
		}
		this.m_txAlbum = this.m_tRaveup.Find("Texture_Album").GetComponent<UITexture>();
		for (int k = 0; k < this.m_iRaveupDiscCount; k++)
		{
			Transform transform5 = this.m_tRaveup.Find(string.Format("Disc_{0}", k + 1));
			this.m_txRaveupDisc[k] = transform5.Find("Texture_Disc").GetComponent<UITexture>();
			this.m_spRaveupDiscPT[k] = transform5.Find("Sprite_PT").GetComponent<UISprite>();
		}
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0000CB86 File Offset: 0x0000AD86
	public void setUserIcon(int icon)
	{
		this.m_spUserPic.spriteName = icon.ToString();
		this.m_spUserPic.MakePixelPerfect();
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x0006993C File Offset: 0x00067B3C
	public void Setting(bool isRaveUp, int rankingNum, string userName, int level, int score, int @class, ArrayList arrDiscs, int albumID = -1)
	{
		this.m_tRaveup.gameObject.SetActive(isRaveUp);
		this.m_tHausMix.gameObject.SetActive(!isRaveUp);
		this.m_iRankingNum = rankingNum;
		this.m_ilRankingNum.text = rankingNum.ToString();
		this.m_lUserName.text = userName;
		this.m_ilUserLevel.text = level.ToString();
		this.m_ilScore.text = score.ToString();
		if (albumID != -1)
		{
			ArrayList allRaveUpAlbum = Singleton<SongManager>.instance.AllRaveUpAlbum;
			albumID = Mathf.Max(albumID, 1) - 1;
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.ALBUM_633, null, this.m_txAlbum, (AlbumInfo)allRaveUpAlbum[albumID], null);
		}
		for (int i = 0; i < 0; i++)
		{
			if (isRaveUp)
			{
				RaveUpStage raveUpStage = (RaveUpStage)arrDiscs[i];
				UITexture uitexture = (isRaveUp ? this.m_txRaveupDisc[i] : this.m_txHausDisc[i]);
				Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, Singleton<SongManager>.instance.GetDiscInfo(raveUpStage.iSong), uitexture, null, null);
				this.PtSetting(isRaveUp, i, raveUpStage.PtType);
			}
			else
			{
				HouseStage houseStage = (HouseStage)arrDiscs[i];
				UITexture uitexture2 = (isRaveUp ? this.m_txRaveupDisc[i] : this.m_txHausDisc[i]);
				Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, Singleton<SongManager>.instance.GetDiscInfo(houseStage.iSong), uitexture2, null, null);
				this.PtSetting(isRaveUp, i, houseStage.PtType);
			}
		}
		this.RankSetting(@class);
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00069AB8 File Offset: 0x00067CB8
	private void RankSetting(int rank)
	{
		int num = 0;
		string text = "Rank_" + rank.ToString();
		if (rank >= 4 && rank <= 6)
		{
			text = "Rank_" + 4.ToString();
			num = rank - 4;
		}
		else if (rank >= 7 && rank <= 9)
		{
			text = "Rank_" + 7.ToString();
			num = rank - 7;
		}
		this.m_spRank.spriteName = text;
		this.m_spRank.MakePixelPerfect();
		if (this.m_gRankPlus != null)
		{
			for (int i = 0; i < this.m_gRankPlus.Length; i++)
			{
				this.m_gRankPlus[i].gameObject.SetActive(i < num);
			}
		}
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00069B64 File Offset: 0x00067D64
	private void PtSetting(bool isRaveUp, int index, PTLEVEL kind)
	{
		if (isRaveUp)
		{
			this.m_spRaveupDiscPT[index].spriteName = "level_" + kind.ToString().ToLower() + "_sm";
			return;
		}
		this.m_spHausDiscPT[index].spriteName = "level_" + kind.ToString().ToLower() + "_sm";
	}

	// Token: 0x04000FF5 RID: 4085
	[HideInInspector]
	public int m_iRankingNum;

	// Token: 0x04000FF6 RID: 4086
	private ImageFontLabel m_ilRankingNum;

	// Token: 0x04000FF7 RID: 4087
	private UISprite m_spUserPic;

	// Token: 0x04000FF8 RID: 4088
	private UILabel m_lUserName;

	// Token: 0x04000FF9 RID: 4089
	private ImageFontLabel m_ilUserLevel;

	// Token: 0x04000FFA RID: 4090
	private UISprite m_spRank;

	// Token: 0x04000FFB RID: 4091
	private GameObject[] m_gRankPlus;

	// Token: 0x04000FFC RID: 4092
	private ImageFontLabel m_ilScore;

	// Token: 0x04000FFD RID: 4093
	private Transform m_tHausMix;

	// Token: 0x04000FFE RID: 4094
	private UITexture[] m_txHausDisc;

	// Token: 0x04000FFF RID: 4095
	private UISprite[] m_spHausDiscPT;

	// Token: 0x04001000 RID: 4096
	private Transform m_tRaveup;

	// Token: 0x04001001 RID: 4097
	private UITexture m_txAlbum;

	// Token: 0x04001002 RID: 4098
	private UITexture[] m_txRaveupDisc;

	// Token: 0x04001003 RID: 4099
	private UISprite[] m_spRaveupDiscPT;

	// Token: 0x04001004 RID: 4100
	private int m_iPluseCount = 2;

	// Token: 0x04001005 RID: 4101
	private int m_iHausDiscCount = 3;

	// Token: 0x04001006 RID: 4102
	private int m_iRaveupDiscCount = 4;
}
