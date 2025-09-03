using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class LevelGiftPopup : MonoBehaviour
{
	// Token: 0x06000CD5 RID: 3285 RVA: 0x000594B4 File Offset: 0x000576B4
	private void Awake()
	{
		Transform transform = base.transform.FindChild("Popup");
		this.m_lUserLevel = transform.FindChild("Label_text_Level").GetComponent<UILabel>();
		this.m_lDiscName = transform.FindChild("Label_DiscName").GetComponent<UILabel>();
		this.m_lDiscName_Han = transform.FindChild("Label_DiscName_Han").GetComponent<UILabel>();
		this.m_lDiscArtist = transform.FindChild("Label_Artist").GetComponent<UILabel>();
		this.m_gBG = base.transform.FindChild("BG").gameObject;
		this.m_spPT = new UISprite[this.m_iMaxPt];
		this.m_txDisc = transform.FindChild("Texture_CD").GetComponent<UITexture>();
		this.m_PopupBG = transform.FindChild("Sprite_PopupBG").GetComponent<UISprite>();
		this.m_PopupInfoBG = transform.FindChild("Sprite_ItemBG").GetComponent<UISprite>();
		this.m_CDBG = this.m_txDisc.transform.FindChild("Sprite").GetComponent<UISprite>();
		for (int i = 0; i < this.m_iMaxPt; i++)
		{
			this.m_spPT[i] = transform.FindChild("PT").FindChild("Sprite_PT" + (i + 1).ToString()).GetComponent<UISprite>();
			this.m_spPT[i].gameObject.SetActive(false);
		}
		transform.transform.eulerAngles = new Vector3(90f, 0f, 0f);
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x0000B8C1 File Offset: 0x00009AC1
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x00059634 File Offset: 0x00057834
	private void Init()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_POPUP, false);
		this.ModeCheck();
		this.m_gBG.SetActive(true);
		base.Invoke("ColOpen", 3f);
		base.Invoke("Close", 5f);
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00059680 File Offset: 0x00057880
	private void ModeCheck()
	{
		if (!this.m_bClubTour)
		{
			this.m_PopupBG.spriteName = "HausMix_BG";
			this.m_PopupInfoBG.spriteName = "HausMix_BaseBG";
			this.m_CDBG.spriteName = "HausMix_CDBG";
		}
		else
		{
			this.m_PopupBG.spriteName = "ClubTour_BG";
			this.m_PopupInfoBG.spriteName = "ClubTour_BaseBG";
			this.m_CDBG.spriteName = "ClubTour_CDBG";
		}
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00059700 File Offset: 0x00057900
	public void CheckLevelGift(int lv)
	{
		this.m_lUserLevel.text = lv.ToString();
		int num = lv;
		if (num != 10)
		{
			if (num != 20)
			{
				if (num != 30)
				{
					if (num != 40)
					{
						if (num != 50)
						{
							if (num != 60)
							{
								UnityEngine.Object.DestroyObject(base.gameObject);
							}
							else
							{
								this.ImageSetting(49);
								this.setPT(0, PTLEVEL.NM);
								this.setPT(1, PTLEVEL.HD);
								this.setPT(2, PTLEVEL.PR);
							}
						}
						else
						{
							this.ImageSetting(30);
							this.setPT(0, PTLEVEL.NM);
							this.setPT(1, PTLEVEL.HD);
							this.setPT(2, PTLEVEL.PR);
						}
					}
					else
					{
						this.ImageSetting(35);
						this.setPT(0, PTLEVEL.NM);
						this.setPT(1, PTLEVEL.HD);
						this.setPT(2, PTLEVEL.PR);
					}
				}
				else
				{
					this.ImageSetting(34);
					this.setPT(0, PTLEVEL.EZ);
					this.setPT(1, PTLEVEL.NM);
					this.setPT(2, PTLEVEL.HD);
				}
			}
			else
			{
				this.ImageSetting(47);
				this.setPT(0, PTLEVEL.EZ);
				this.setPT(1, PTLEVEL.NM);
				this.setPT(2, PTLEVEL.HD);
			}
		}
		else
		{
			this.ImageSetting(3);
			this.setPT(0, PTLEVEL.EZ);
			this.setPT(1, PTLEVEL.NM);
			this.setPT(2, PTLEVEL.HD);
		}
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00059844 File Offset: 0x00057A44
	private void ImageSetting(int DiscID)
	{
		DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(DiscID);
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, discInfo, this.m_txDisc, null, null);
		bool flag = GameData.isContainHangul(discInfo.FullName);
		this.m_lDiscName_Han.gameObject.SetActive(false);
		this.m_lDiscName.gameObject.SetActive(false);
		if (flag)
		{
			this.m_lDiscName_Han.gameObject.SetActive(true);
			this.m_lDiscName_Han.text = discInfo.FullName.ToUpper();
		}
		else
		{
			this.m_lDiscName.gameObject.SetActive(true);
			this.m_lDiscName.text = discInfo.FullName.ToUpper();
		}
		this.m_lDiscArtist.text = discInfo.Artist.ToUpper();
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00059910 File Offset: 0x00057B10
	private void setPT(int index, PTLEVEL ptNum)
	{
		this.m_spPT[index].gameObject.SetActive(true);
		switch (ptNum)
		{
		case PTLEVEL.EZ:
			this.m_spPT[index].spriteName = "Dif_Ez";
			break;
		case PTLEVEL.NM:
			this.m_spPT[index].spriteName = "Dif_Nm";
			break;
		case PTLEVEL.HD:
			this.m_spPT[index].spriteName = "Dif_Hd";
			break;
		case PTLEVEL.PR:
			this.m_spPT[index].spriteName = "Dif_Pr";
			break;
		case PTLEVEL.MX:
			this.m_spPT[index].spriteName = "Dif_Mx";
			break;
		case PTLEVEL.S1:
			this.m_spPT[index].spriteName = "Dif_Hid1";
			break;
		case PTLEVEL.S2:
			this.m_spPT[index].spriteName = "Dif_Hid2";
			break;
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0000B8C9 File Offset: 0x00009AC9
	private void ColOpen()
	{
		this.m_bClose = true;
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x00003648 File Offset: 0x00001848
	private void SetInfo()
	{
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0000B8D2 File Offset: 0x00009AD2
	private void ClickClose()
	{
		if (!this.m_bClose)
		{
			return;
		}
		this.Close();
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0000B8E6 File Offset: 0x00009AE6
	private void Close()
	{
		UnityEngine.Object.DestroyObject(base.gameObject);
	}

	// Token: 0x04000CAB RID: 3243
	public bool m_bClubTour;

	// Token: 0x04000CAC RID: 3244
	private UILabel m_lUserLevel;

	// Token: 0x04000CAD RID: 3245
	private UILabel m_lDiscName;

	// Token: 0x04000CAE RID: 3246
	private UILabel m_lDiscName_Han;

	// Token: 0x04000CAF RID: 3247
	private UILabel m_lDiscArtist;

	// Token: 0x04000CB0 RID: 3248
	private UISprite[] m_spPT;

	// Token: 0x04000CB1 RID: 3249
	private UITexture m_txDisc;

	// Token: 0x04000CB2 RID: 3250
	private GameObject m_gBG;

	// Token: 0x04000CB3 RID: 3251
	private int m_iMaxPt = 3;

	// Token: 0x04000CB4 RID: 3252
	private UISprite m_PopupBG;

	// Token: 0x04000CB5 RID: 3253
	private UISprite m_PopupInfoBG;

	// Token: 0x04000CB6 RID: 3254
	private UISprite m_CDBG;

	// Token: 0x04000CB7 RID: 3255
	private bool m_bClose;
}
