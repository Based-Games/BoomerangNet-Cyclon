using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000160 RID: 352
public class WWWGetUserData : WWWObject
{
	// Token: 0x06000AE4 RID: 2788 RVA: 0x0004E210 File Offset: 0x0004C410
	public override void StartLoad()
	{
		string text = string.Format("users/{0}", Singleton<GameManager>.instance.s_UserID);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0004E240 File Offset: 0x0004C440
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			if (this.CallBackFail != null)
			{
				this.CallBackFail();
			}
			return;
		}
		string text = this.www.text;
		this.ParsingUserData(text);
		Singleton<GameManager>.instance.ONLOGIN = true;
		Singleton<GameManager>.instance.UserData.SetViewValue();
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0004E2B8 File Offset: 0x0004C4B8
	private void ParsingUserData(string strContent)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		userData.ArrItem.Clear();
		dictionary.ContainsKey("userId");
		if (dictionary.ContainsKey("nation"))
		{
			userData.Nation = (string)dictionary["nation"];
		}
		if (dictionary.ContainsKey("name"))
		{
			userData.Name = (string)dictionary["name"];
		}
		if (dictionary.ContainsKey("icon"))
		{
			userData.WebIcon = (string)dictionary["icon"];
		}
		if (dictionary.ContainsKey("iconId") && dictionary["iconId"] != null)
		{
			userData.Icon = (int)((long)dictionary["iconId"]);
		}
		if (dictionary.ContainsKey("setBGM"))
		{
			userData.SetBGM = (string)dictionary["setBGM"];
		}
		if (dictionary.ContainsKey("lastSong") && dictionary["lastSong"] != null)
		{
			userData.LastSongID = (int)((long)dictionary["lastSong"]);
		}
		if (dictionary.ContainsKey("lastPT") && dictionary["lastPT"] != null)
		{
			userData.lastPT = (int)((long)dictionary["lastPT"]);
		}
		if (dictionary.ContainsKey("lastScrollPosition") && dictionary["lastScrollPosition"] != null)
		{
			userData.lastScrollPosition = Convert.ToSingle(dictionary["lastScrollPosition"]);
		}
		if (dictionary.ContainsKey("club"))
		{
			userData.ClubName = (string)dictionary["club"];
		}
		if (dictionary.ContainsKey("clubIcon") && dictionary["clubIcon"] != null)
		{
			userData.ClubIcon = (int)((long)dictionary["clubIcon"]);
		}
		if (dictionary.ContainsKey("level"))
		{
			userData.Level = (int)((long)dictionary["level"]);
		}
		if (dictionary.ContainsKey("beatPoint"))
		{
			userData.BeatPoint = (int)((long)dictionary["beatPoint"]);
		}
		if (dictionary.ContainsKey("exp"))
		{
			userData.TotalExp = (int)((long)dictionary["exp"]);
		}
		if (dictionary.ContainsKey("language"))
		{
			userData.Language = (string)dictionary["language"];
		}
		if (dictionary.ContainsKey("userItems"))
		{
			List<object> list = (List<object>)dictionary["userItems"];
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					USERITEM useritem = new USERITEM();
					Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
					if (dictionary2.ContainsKey("publishedItemNo"))
					{
						useritem.iPublishedItemNo = (int)((long)dictionary2["publishedItemNo"]);
					}
					if (dictionary2.ContainsKey("type"))
					{
						int enumIndex = GameData.GetEnumIndex<ITEMTYPE>((string)dictionary2["type"]);
						if (enumIndex != -1)
						{
							int num = (int)Enum.GetValues(typeof(ITEMTYPE)).GetValue(enumIndex);
							useritem.eType = (ITEMTYPE)num;
						}
						if (useritem.eType == ITEMTYPE.gameItem)
						{
							if (dictionary2.ContainsKey("gameItemType"))
							{
								useritem.strName = (string)dictionary2["gameItemType"];
							}
							if (dictionary2.ContainsKey("value"))
							{
								useritem.value = (int)((long)dictionary2["value"]);
							}
							if (dictionary2.ContainsKey("count"))
							{
								useritem.count = (int)((long)dictionary2["count"]);
							}
							if ("refillItem" == useritem.strName)
							{
								if (useritem.value == 50)
								{
									userData.PlayRefillItem[PLAYFREFILLITEM.REFILL_50] = useritem.count;
								}
								else if (useritem.value == 70)
								{
									userData.PlayRefillItem[PLAYFREFILLITEM.REFILL_70] = useritem.count;
								}
								else if (useritem.value == 100)
								{
									userData.PlayRefillItem[PLAYFREFILLITEM.REFILL_100] = useritem.count;
								}
							}
							else if ("shieldItem" == useritem.strName)
							{
								if (useritem.value == 3)
								{
									userData.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X3] = useritem.count;
								}
								else if (useritem.value == 2)
								{
									userData.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X2] = useritem.count;
								}
								else if (useritem.value == 1)
								{
									userData.PlayShieldItem[PLAYFSHIELDITEM.SHIELD_X1] = useritem.count;
								}
							}
							else if ("extremeItem" == useritem.strName)
							{
								if (useritem.value == 3)
								{
									userData.PlayNormalItem[PLAYFNORMALITEM.EXTREME_X3] = useritem.count;
								}
								else if (useritem.value == 2)
								{
									userData.PlayNormalItem[PLAYFNORMALITEM.EXTREME_X2] = useritem.count;
								}
							}
							else if ("feverItem" == useritem.strName)
							{
								if (useritem.value == 5)
								{
									userData.PlayNormalItem[PLAYFNORMALITEM.FEVER_x5] = useritem.count;
								}
								else if (useritem.value == 4)
								{
									userData.PlayNormalItem[PLAYFNORMALITEM.FEVER_x4] = useritem.count;
								}
								else if (useritem.value == 3)
								{
									userData.PlayNormalItem[PLAYFNORMALITEM.FEVER_x3] = useritem.count;
								}
								else if (useritem.value == 2)
								{
									userData.PlayNormalItem[PLAYFNORMALITEM.FEVER_x2] = useritem.count;
								}
							}
						}
					}
					dictionary2.ContainsKey("buffType");
					if (dictionary2.ContainsKey("value"))
					{
						long num2 = (long)dictionary2["value"];
					}
					dictionary2.ContainsKey("unit");
					userData.ArrItem.Add(useritem);
				}
			}
		}
		if (dictionary.ContainsKey("clubItems"))
		{
			List<object> list2 = (List<object>)dictionary["clubItems"];
			if (list2 != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					object obj = list2[j];
				}
			}
		}
		if (dictionary.ContainsKey("globalItems"))
		{
			List<object> list3 = (List<object>)dictionary["globalItems"];
			if (list3 != null)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					object obj2 = list3[k];
				}
			}
		}
		if (dictionary.ContainsKey("musics"))
		{
			List<object> list4 = (List<object>)dictionary["musics"];
			if (list4 != null)
			{
				for (int l = 0; l < list4.Count; l++)
				{
					Dictionary<string, object> dictionary3 = list4[l] as Dictionary<string, object>;
					string text = string.Empty;
					int num3 = 0;
					int num4 = 0;
					if (dictionary3.ContainsKey("musicId"))
					{
						text = (string)dictionary3["musicId"];
					}
					if (dictionary3.ContainsKey("patternId"))
					{
						num3 = (int)((long)dictionary3["patternId"]);
					}
					if (dictionary3.ContainsKey("stage"))
					{
						num4 = (int)((long)dictionary3["stage"]);
					}
					DiscInfo discInfoServerKey = Singleton<SongManager>.instance.GetDiscInfoServerKey(text);
					if (discInfoServerKey != null)
					{
						for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
						{
							if (discInfoServerKey.ContainPt(ptlevel) && discInfoServerKey.DicPtInfo[ptlevel].iPtServerId == num3)
							{
								BonusPattern bonusPattern = new BonusPattern();
								bonusPattern.iSong = discInfoServerKey.Id;
								bonusPattern.PTTYPE = ptlevel;
								bonusPattern.iStage = num4;
								bool flag = false;
								foreach (object obj3 in userData.ArrHausPattern)
								{
									BonusPattern bonusPattern2 = (BonusPattern)obj3;
									if (bonusPattern2.iSong == bonusPattern.iSong && bonusPattern2.PTTYPE == bonusPattern.PTTYPE && bonusPattern2.iStage == bonusPattern.iStage)
									{
										flag = true;
									}
								}
								if (!flag)
								{
									userData.ArrHausPattern.Add(bonusPattern);
								}
							}
						}
					}
				}
			}
		}
		if (dictionary.ContainsKey("albums"))
		{
			List<object> list5 = (List<object>)dictionary["albums"];
			if (list5 != null)
			{
				for (int m = 0; m < list5.Count; m++)
				{
					object obj4 = list5[m];
				}
			}
		}
		userData.ArrMissions.Clear();
		if (dictionary.ContainsKey("missions"))
		{
			List<object> list6 = (List<object>)dictionary["missions"];
			if (list6 != null)
			{
				for (int n = 0; n < list6.Count; n++)
				{
					ClubGetInfo clubGetInfo = new ClubGetInfo();
					string text2 = string.Empty;
					bool flag2 = false;
					Dictionary<string, object> dictionary4 = list6[n] as Dictionary<string, object>;
					if (dictionary4.ContainsKey("missionId"))
					{
						text2 = (string)dictionary4["missionId"];
					}
					if (dictionary4.ContainsKey("cleared"))
					{
						flag2 = (bool)dictionary4["cleared"];
					}
					int serverKeyToId = Singleton<SongManager>.instance.GetServerKeyToId(text2);
					clubGetInfo.MissionId = serverKeyToId;
					clubGetInfo.Cleard = flag2;
					if (serverKeyToId != -1 && !userData.GetContainMissionInfo(serverKeyToId))
					{
						userData.ArrMissions.Add(clubGetInfo);
					}
				}
			}
		}
		if (dictionary.ContainsKey("configurations"))
		{
			Dictionary<string, object> dictionary5 = dictionary["configurations"] as Dictionary<string, object>;
			if (dictionary5 != null)
			{
				if (dictionary5.ContainsKey("KeyEFfect"))
				{
					Singleton<SoundSourceManager>.instance.EFF_NUM = (int)((long)dictionary5["KeyEFfect"]);
				}
				if (dictionary5.ContainsKey("KeyVolume") && dictionary5["KeyVolume"] != null)
				{
					string text3 = string.Empty;
					if (typeof(long) == dictionary5["KeyVolume"].GetType())
					{
						text3 = ((int)((long)dictionary5["KeyVolume"])).ToString();
					}
					else
					{
						text3 = (string)dictionary5["KeyVolume"];
					}
					Singleton<SoundSourceManager>.instance.EFF_VOLUME = float.Parse(text3);
				}
				if (dictionary5.ContainsKey("Speed"))
				{
					EFFECTOR_SPEED effector_SPEED = GameData.ParseEnum<EFFECTOR_SPEED>((string)dictionary5["Speed"]);
					if (EFFECTOR_SPEED.MAX_SPEED > effector_SPEED)
					{
						userData.UserSpeed = effector_SPEED;
					}
				}
				if (dictionary5.ContainsKey("IsUseKeySound"))
				{
					string text4 = (string)dictionary5["IsUseKeySound"];
					if (true.ToString() != text4 && Singleton<SoundSourceManager>.instance.EFF_NUM == 0)
					{
						Singleton<SoundSourceManager>.instance.EFF_NUM = 3;
						Singleton<SoundSourceManager>.instance.EFF_VOLUME = 0.55f;
					}
				}
				else
				{
					Singleton<SoundSourceManager>.instance.EFF_NUM = 3;
					Singleton<SoundSourceManager>.instance.EFF_VOLUME = 0.55f;
				}
				userData.IsUseKeySound = true;
				if (dictionary5.ContainsKey("EventSong"))
				{
					string text5 = (string)dictionary5["EventSong"];
					Singleton<SongManager>.instance.AllEventData.Clear();
					if (string.Empty != text5)
					{
						foreach (string text6 in text5.Split(new char[] { ","[0] }))
						{
							EventSongData eventSongData = new EventSongData();
							string[] array2 = text6.Split(new char[] { "_"[0] });
							eventSongData.iSongId = int.Parse(array2[0]);
							eventSongData.ePt = GameData.ParseEnum<PTLEVEL>(array2[1]);
							eventSongData.iCount = int.Parse(array2[2]);
							Singleton<SongManager>.instance.AllEventData.Add(eventSongData);
						}
					}
				}
			}
		}
		if (dictionary.ContainsKey("rewards"))
		{
			List<object> list7 = (List<object>)dictionary["rewards"];
			if (list7 != null)
			{
				for (int num6 = 0; num6 < list7.Count; num6++)
				{
					RewardInfo rewardInfo = new RewardInfo();
					Dictionary<string, object> dictionary6 = list7[num6] as Dictionary<string, object>;
					if (dictionary6.ContainsKey("triggerType"))
					{
						string text7 = (string)dictionary6["triggerType"];
						if (Enum.IsDefined(typeof(RewardTrigger), dictionary6["triggerType"]))
						{
							rewardInfo.TriggerType = (RewardTrigger)((int)Enum.Parse(typeof(RewardTrigger), text7, true));
						}
					}
					if (dictionary6.ContainsKey("triggerParams"))
					{
						Dictionary<string, object> dictionary7 = dictionary6["triggerParams"] as Dictionary<string, object>;
						if (dictionary7.ContainsKey("level"))
						{
							rewardInfo.iIncomLevel = (int)((long)dictionary7["level"]);
							if (rewardInfo.iIncomLevel == 1)
							{
								userData.INCOMGIFTPOPUPSTATE = INCOMGIFTPOPUP.BASEGIFT;
							}
							else if (rewardInfo.iIncomLevel == 10)
							{
								userData.INCOMGIFTPOPUPSTATE = INCOMGIFTPOPUP.LV10GIFT;
							}
							else if (rewardInfo.iIncomLevel == 20)
							{
								userData.INCOMGIFTPOPUPSTATE = INCOMGIFTPOPUP.LV20GIFT;
							}
						}
					}
				}
			}
		}
	}
}
