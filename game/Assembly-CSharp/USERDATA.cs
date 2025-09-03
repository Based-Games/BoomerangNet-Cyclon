using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class USERDATA
{
	// Token: 0x06000B42 RID: 2882 RVA: 0x00050EE4 File Offset: 0x0004F0E4
	public USERDATA()
	{
		this.Init();
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0000A8B4 File Offset: 0x00008AB4
	// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0000A8BC File Offset: 0x00008ABC
	public string Name { get; set; }

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0000A8C5 File Offset: 0x00008AC5
	// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0000A8CD File Offset: 0x00008ACD
	public string Nation { get; set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0000A8D6 File Offset: 0x00008AD6
	// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0000A8DE File Offset: 0x00008ADE
	public int Level { get; set; }

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0000A8E7 File Offset: 0x00008AE7
	// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0000A8EF File Offset: 0x00008AEF
	public int Icon { get; set; }

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0000A8F8 File Offset: 0x00008AF8
	// (set) Token: 0x06000B4C RID: 2892 RVA: 0x0000A900 File Offset: 0x00008B00
	public int TotalExp { get; set; }

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0000A909 File Offset: 0x00008B09
	// (set) Token: 0x06000B4E RID: 2894 RVA: 0x0000A911 File Offset: 0x00008B11
	public int BeatPoint { get; set; }

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0000A91A File Offset: 0x00008B1A
	// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0000A922 File Offset: 0x00008B22
	public int ClubIcon { get; set; }

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0000A92B File Offset: 0x00008B2B
	// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0000A933 File Offset: 0x00008B33
	public string ClubName { get; set; }

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0000A93C File Offset: 0x00008B3C
	// (set) Token: 0x06000B54 RID: 2900 RVA: 0x0000A944 File Offset: 0x00008B44
	public bool IsUseKeySound { get; set; }

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0000A94D File Offset: 0x00008B4D
	// (set) Token: 0x06000B56 RID: 2902 RVA: 0x0000A955 File Offset: 0x00008B55
	public int UserSelectEffect { get; set; }

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0000A95E File Offset: 0x00008B5E
	// (set) Token: 0x06000B58 RID: 2904 RVA: 0x0000A966 File Offset: 0x00008B66
	public float UserEffectVolume { get; set; }

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0000A96F File Offset: 0x00008B6F
	// (set) Token: 0x06000B5A RID: 2906 RVA: 0x0000A977 File Offset: 0x00008B77
	public int ViewPreTotalExp { get; set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0000A980 File Offset: 0x00008B80
	// (set) Token: 0x06000B5C RID: 2908 RVA: 0x0000A988 File Offset: 0x00008B88
	public int ViewPreTotalBeatPoint { get; set; }

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0000A991 File Offset: 0x00008B91
	// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0000A999 File Offset: 0x00008B99
	public int BestScore
	{
		get
		{
			return this.m_iBestScore;
		}
		set
		{
			this.m_iBestScore = value;
			if (0 > this.m_iBestScore)
			{
				this.m_iBestScore = 0;
			}
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00050F90 File Offset: 0x0004F190
	public void Init()
	{
		this.Icon = 0;
		this.Name = "GUEST DJ";
		this.UserSpeed = EFFECTOR_SPEED.X_1;
		this.UserEffectVolume = 1f;
		this.UserSelectEffect = -1;
		this.IsUseKeySound = false;
		Singleton<SoundSourceManager>.instance.EFF_NUM = 1;
		Singleton<SoundSourceManager>.instance.EFF_VOLUME = 1f;
		this.Nation = "kr";
		this.Level = 0;
		this.lastPT = 0;
		this.lastScrollPosition = 0f;
		this.TotalExp = 0;
		this.ViewPreTotalExp = 0;
		this.ViewPreTotalBeatPoint = 0;
		this.BeatPoint = 0;
		this.WebIcon = string.Empty;
		this.BestScore = 0;
		this.ClubIcon = 11;
		this.ClubName = "NO CARD!";
		this.LoggedIn = false;
		this.Banned = false;
		this.Admin = false;
		this.SetBGM = "cyclon";
		this.LastSongID = 23;
		this.Language = GameData.LANGUAGE;
		this.InitItem();
		this.ArrHausPattern.Clear();
		this.ArrRaveDiscSet.Clear();
		this.ArrItem.Clear();
		this.ArrMissions.Clear();
		this.ArrRewards.Clear();
		this.INCOMGIFTPOPUPSTATE = INCOMGIFTPOPUP.NONE;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x000510C8 File Offset: 0x0004F2C8
	private void InitItem()
	{
		this.PlayNormalItem.Clear();
		this.PlayRefillItem.Clear();
		this.PlayShieldItem.Clear();
		this.EffectorSpeedItem.Clear();
		this.EffectorFaderItem.Clear();
		this.EffectorRandItem.Clear();
		this.PlayNormalItem.Add(PLAYFNORMALITEM.NONE, 1);
		this.PlayNormalItem.Add(PLAYFNORMALITEM.FEVER_x2, 0);
		this.PlayNormalItem.Add(PLAYFNORMALITEM.FEVER_x3, 0);
		this.PlayNormalItem.Add(PLAYFNORMALITEM.FEVER_x4, 0);
		this.PlayNormalItem.Add(PLAYFNORMALITEM.FEVER_x5, 0);
		this.PlayNormalItem.Add(PLAYFNORMALITEM.EXTREME_X2, 0);
		this.PlayNormalItem.Add(PLAYFNORMALITEM.EXTREME_X3, 0);
		this.PlayRefillItem.Add(PLAYFREFILLITEM.NONE, 1);
		this.PlayRefillItem.Add(PLAYFREFILLITEM.REFILL_50, 0);
		this.PlayRefillItem.Add(PLAYFREFILLITEM.REFILL_70, 0);
		this.PlayRefillItem.Add(PLAYFREFILLITEM.REFILL_100, 0);
		this.PlayShieldItem.Add(PLAYFSHIELDITEM.NONE, 1);
		this.PlayShieldItem.Add(PLAYFSHIELDITEM.SHIELD_X1, 0);
		this.PlayShieldItem.Add(PLAYFSHIELDITEM.SHIELD_X2, 0);
		this.PlayShieldItem.Add(PLAYFSHIELDITEM.SHIELD_X3, 0);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_0_5, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_1, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_1_5, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_2, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_2_5, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_3, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_3_5, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_4, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_5, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.X_6, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.MAX_SPEED, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.CHAOS_W, true);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.CHAOS_UP, false);
		this.EffectorSpeedItem.Add(EFFECTOR_SPEED.CHAOS_DN, false);
		this.EffectorFaderItem.Add(EFFECTOR_FADER.NONE, true);
		this.EffectorFaderItem.Add(EFFECTOR_FADER.FADEIN, true);
		this.EffectorFaderItem.Add(EFFECTOR_FADER.FADEOUT, true);
		this.EffectorFaderItem.Add(EFFECTOR_FADER.BLINK, true);
		this.EffectorFaderItem.Add(EFFECTOR_FADER.BLANK, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.NONE, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.MIRROR, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.RANDOM, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.ROTATE_LEFT, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.ROTATE_RIGHT, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.ROTATE_RANDOM, true);
		this.EffectorRandItem.Add(EFFECTOR_RAND.ROTATE_STEP, false);
		this.EffectorRandItem.Add(EFFECTOR_RAND.CYCLON, true);
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0000A9B2 File Offset: 0x00008BB2
	public void SetViewValue()
	{
		this.ViewPreTotalExp = this.TotalExp;
		this.ViewPreTotalBeatPoint = this.BeatPoint;
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00051340 File Offset: 0x0004F540
	public int ViewExp
	{
		get
		{
			int viewPreTotalExp = this.ViewPreTotalExp;
			int expToLv = Singleton<SongManager>.instance.GetExpToLv(viewPreTotalExp);
			int lvMaxExp = Singleton<SongManager>.instance.GetLvMaxExp(expToLv - 1);
			return viewPreTotalExp - lvMaxExp;
		}
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00051374 File Offset: 0x0004F574
	public bool GetContainMissionInfo(int iMissionId)
	{
		foreach (object obj in this.ArrMissions)
		{
			ClubGetInfo clubGetInfo = (ClubGetInfo)obj;
			if (clubGetInfo != null && clubGetInfo.MissionId == iMissionId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x000513DC File Offset: 0x0004F5DC
	public bool GetClearedMission(int iMissionId)
	{
		foreach (object obj in this.ArrMissions)
		{
			ClubGetInfo clubGetInfo = (ClubGetInfo)obj;
			if (clubGetInfo.MissionId == iMissionId)
			{
				return clubGetInfo.Cleard;
			}
		}
		return false;
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00051444 File Offset: 0x0004F644
	public int[] GetUserUseItem()
	{
		this.CheckUseItem();
		List<int> list = new List<int>();
		foreach (object obj in this.ArrItem)
		{
			USERITEM useritem = (USERITEM)obj;
			if (useritem.bUseItem)
			{
				list.Add(useritem.iPublishedItemNo);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x000514BC File Offset: 0x0004F6BC
	private void CheckUseItem()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = 0;
		for (int i = 0; i < this.ArrItem.Count; i++)
		{
			USERITEM useritem = (USERITEM)this.ArrItem[i];
			if (useritem.eType == ITEMTYPE.gameItem)
			{
				if ("refillItem" == useritem.strName)
				{
					if (useritem.value == 50)
					{
						if (useritem.count > this.PlayRefillItem[PLAYFREFILLITEM.REFILL_50])
						{
							num = useritem.count - this.PlayRefillItem[PLAYFREFILLITEM.REFILL_50];
						}
					}
					else if (useritem.value == 70)
					{
						if (useritem.count > this.PlayRefillItem[PLAYFREFILLITEM.REFILL_70])
						{
							num2 = useritem.count - this.PlayRefillItem[PLAYFREFILLITEM.REFILL_70];
						}
					}
					else if (useritem.value == 100 && useritem.count > this.PlayRefillItem[PLAYFREFILLITEM.REFILL_100])
					{
						num3 = useritem.count - this.PlayRefillItem[PLAYFREFILLITEM.REFILL_100];
					}
				}
				else if ("shieldItem" == useritem.strName)
				{
					if (useritem.value == 3)
					{
						if (useritem.count > this.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X3])
						{
							num4 = useritem.count - this.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X3];
						}
					}
					else if (useritem.value == 2)
					{
						if (useritem.count > this.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X2])
						{
							num5 = useritem.count - this.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X2];
						}
					}
					else if (useritem.value == 1 && useritem.count > this.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X1])
					{
						num6 = useritem.count - this.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X1];
					}
				}
				else if ("extremeItem" == useritem.strName)
				{
					if (useritem.value == 3)
					{
						if (useritem.count > this.PlayNormalItem[PLAYFNORMALITEM.EXTREME_X3])
						{
							num7 = useritem.count - this.PlayNormalItem[PLAYFNORMALITEM.EXTREME_X3];
						}
					}
					else if (useritem.value == 2 && useritem.count > this.PlayNormalItem[PLAYFNORMALITEM.EXTREME_X2])
					{
						num8 = useritem.count - this.PlayNormalItem[PLAYFNORMALITEM.EXTREME_X3];
					}
				}
				else if ("feverItem" == useritem.strName)
				{
					if (useritem.value == 5)
					{
						if (useritem.count > this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x5])
						{
							num9 = useritem.count - this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x5];
						}
					}
					else if (useritem.value == 4)
					{
						if (useritem.count > this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x4])
						{
							num10 = useritem.count - this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x4];
						}
					}
					else if (useritem.value == 3)
					{
						if (useritem.count > this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x3])
						{
							num11 = useritem.count - this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x3];
						}
					}
					else if (useritem.value == 2 && useritem.count > this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x2])
					{
						num12 = useritem.count - this.PlayNormalItem[PLAYFNORMALITEM.FEVER_x2];
					}
				}
			}
		}
		for (int j = 0; j < this.ArrItem.Count; j++)
		{
			USERITEM useritem2 = (USERITEM)this.ArrItem[j];
			if (useritem2.eType == ITEMTYPE.gameItem)
			{
				if ("refillItem" == useritem2.strName)
				{
					if (useritem2.value == 50)
					{
						if (0 < num)
						{
							useritem2.bUseItem = true;
							num--;
						}
					}
					else if (useritem2.value == 70)
					{
						if (0 < num2)
						{
							useritem2.bUseItem = true;
							num2--;
						}
					}
					else if (useritem2.value == 100 && 0 < num3)
					{
						useritem2.bUseItem = true;
						num3--;
					}
				}
				else if ("shieldItem" == useritem2.strName)
				{
					if (useritem2.value == 3)
					{
						if (0 < num4)
						{
							useritem2.bUseItem = true;
							num4--;
						}
					}
					else if (useritem2.value == 2)
					{
						if (0 < num5)
						{
							useritem2.bUseItem = true;
							num5--;
						}
					}
					else if (useritem2.value == 1 && 0 < num6)
					{
						useritem2.bUseItem = true;
						num6--;
					}
				}
				else if ("extremeItem" == useritem2.strName)
				{
					if (useritem2.value == 3)
					{
						if (0 < num7)
						{
							useritem2.bUseItem = true;
							num7--;
						}
					}
					else if (useritem2.value == 2 && 0 < num8)
					{
						useritem2.bUseItem = true;
						num8--;
					}
				}
				else if ("feverItem" == useritem2.strName)
				{
					if (useritem2.value == 5)
					{
						if (0 < num9)
						{
							useritem2.bUseItem = true;
							num9--;
						}
					}
					else if (useritem2.value == 4)
					{
						if (0 < num10)
						{
							useritem2.bUseItem = true;
							num10--;
						}
					}
					else if (useritem2.value == 3)
					{
						if (0 < num11)
						{
							useritem2.bUseItem = true;
							num11--;
						}
					}
					else if (useritem2.value == 2 && 0 < num12)
					{
						useritem2.bUseItem = true;
						num12--;
					}
				}
			}
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0000A9CC File Offset: 0x00008BCC
	// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0000A9D4 File Offset: 0x00008BD4
	public bool LoggedIn { get; set; }

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0000A9DD File Offset: 0x00008BDD
	// (set) Token: 0x06000B6A RID: 2922 RVA: 0x0000A9E5 File Offset: 0x00008BE5
	public bool Admin { get; set; }

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0000A9EE File Offset: 0x00008BEE
	// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0000A9F6 File Offset: 0x00008BF6
	public bool Banned { get; set; }

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0000A9FF File Offset: 0x00008BFF
	// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0000AA07 File Offset: 0x00008C07
	public int LastSongID { get; set; }

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0000AA10 File Offset: 0x00008C10
	// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0000AA18 File Offset: 0x00008C18
	public string SetBGM { get; set; }

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0000AA21 File Offset: 0x00008C21
	// (set) Token: 0x06000B72 RID: 2930 RVA: 0x0000AA29 File Offset: 0x00008C29
	public int lastPT { get; set; }

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0000AA32 File Offset: 0x00008C32
	// (set) Token: 0x06000B74 RID: 2932 RVA: 0x0000AA3A File Offset: 0x00008C3A
	public float lastScrollPosition { get; set; }

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0000AA43 File Offset: 0x00008C43
	// (set) Token: 0x06000B76 RID: 2934 RVA: 0x0000AA4B File Offset: 0x00008C4B
	public string Language { get; set; }

	// Token: 0x04000AB2 RID: 2738
	public string WebIcon = string.Empty;

	// Token: 0x04000AB3 RID: 2739
	public EFFECTOR_SPEED UserSpeed = EFFECTOR_SPEED.X_1;

	// Token: 0x04000AB4 RID: 2740
	public Texture TexIcon;

	// Token: 0x04000AB5 RID: 2741
	public ArrayList ArrHausPattern = new ArrayList();

	// Token: 0x04000AB6 RID: 2742
	public ArrayList ArrRaveDiscSet = new ArrayList();

	// Token: 0x04000AB7 RID: 2743
	public ArrayList ArrItem = new ArrayList();

	// Token: 0x04000AB8 RID: 2744
	public ArrayList ArrMissions = new ArrayList();

	// Token: 0x04000AB9 RID: 2745
	public ArrayList ArrRewards = new ArrayList();

	// Token: 0x04000ABA RID: 2746
	public Dictionary<PLAYFNORMALITEM, int> PlayNormalItem = new Dictionary<PLAYFNORMALITEM, int>();

	// Token: 0x04000ABB RID: 2747
	public Dictionary<PLAYFREFILLITEM, int> PlayRefillItem = new Dictionary<PLAYFREFILLITEM, int>();

	// Token: 0x04000ABC RID: 2748
	public Dictionary<PLAYFSHIELDITEM, int> PlayShieldItem = new Dictionary<PLAYFSHIELDITEM, int>();

	// Token: 0x04000ABD RID: 2749
	public Dictionary<EFFECTOR_SPEED, bool> EffectorSpeedItem = new Dictionary<EFFECTOR_SPEED, bool>();

	// Token: 0x04000ABE RID: 2750
	public Dictionary<EFFECTOR_FADER, bool> EffectorFaderItem = new Dictionary<EFFECTOR_FADER, bool>();

	// Token: 0x04000ABF RID: 2751
	public Dictionary<EFFECTOR_RAND, bool> EffectorRandItem = new Dictionary<EFFECTOR_RAND, bool>();

	// Token: 0x04000AC0 RID: 2752
	public INCOMGIFTPOPUP INCOMGIFTPOPUPSTATE = INCOMGIFTPOPUP.NONE;

	// Token: 0x04000AC1 RID: 2753
	private int m_iBestScore;
}
