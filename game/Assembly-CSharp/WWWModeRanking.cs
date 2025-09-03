using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x0200015E RID: 350
public class WWWModeRanking : WWWObject
{
	// Token: 0x06000ADC RID: 2780
	public override void StartLoad()
	{
		string text = "games/bestRankings";
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000ADD RID: 2781
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
		this.ParsingData(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000ADE RID: 2782
	private void ParsingData(string strContent)
	{
		ArrayList hausModeRank = Singleton<GameManager>.instance.HausModeRank;
		ArrayList raveModeRank = Singleton<GameManager>.instance.RaveModeRank;
		hausModeRank.Clear();
		raveModeRank.Clear();
		for (int i = 1; i <= 30; i++)
		{
			RankInfo rankInfo = new RankInfo
			{
				Name = "DJ CYCLON",
				Level = 0,
				Score = 0,
				RankClass = GRADE.NON,
				Icon = 0,
				Ranking = i
			};
			for (int j = 0; j < 3; j++)
			{
				HouseStage houseStage = new HouseStage
				{
					iSong = 1,
					PtType = PTLEVEL.EZ
				};
				rankInfo.ArrHauseInfo.Add(houseStage);
			}
			hausModeRank.Add(rankInfo);
		}
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		if (dictionary.ContainsKey("stages"))
		{
			List<object> list = (List<object>)dictionary["stages"];
			if (list != null)
			{
				for (int k = 0; k < list.Count; k++)
				{
					Dictionary<string, object> dictionary2 = list[k] as Dictionary<string, object>;
					RankInfo rankInfo2 = new RankInfo();
					if (dictionary2.ContainsKey("name"))
					{
						rankInfo2.Name = (string)dictionary2["name"];
					}
					if (dictionary2.ContainsKey("nation"))
					{
						rankInfo2.Nation = (string)dictionary2["nation"];
					}
					if (dictionary2.ContainsKey("level"))
					{
						rankInfo2.Level = (int)((long)dictionary2["level"]);
					}
					if (dictionary2.ContainsKey("icon"))
					{
						rankInfo2.WebIcon = (string)dictionary2["icon"];
					}
					if (dictionary2.ContainsKey("iconId"))
					{
						rankInfo2.Icon = (int)((long)dictionary2["iconId"]);
					}
					if (dictionary2.ContainsKey("score"))
					{
						rankInfo2.Score = (int)((long)dictionary2["score"]);
					}
					if (dictionary2.ContainsKey("ranking"))
					{
						rankInfo2.Ranking = (int)((long)dictionary2["ranking"]);
					}
					if (dictionary2.ContainsKey("rankClass"))
					{
						string text = (string)dictionary2["rankClass"];
						rankInfo2.RankClass = GameData.ParseEnum<GRADE>(text);
					}
					if (dictionary2.ContainsKey("musics"))
					{
						List<object> list2 = (List<object>)dictionary2["musics"];
						if (list2 != null)
						{
							rankInfo2.ArrHauseInfo.Clear();
							for (int l = 0; l < 3; l++)
							{
								Dictionary<string, object> dictionary3 = list2[l] as Dictionary<string, object>;
								HouseStage houseStage2 = new HouseStage();
								if (dictionary3.ContainsKey("musicId"))
								{
									string text2 = (string)dictionary3["musicId"];
									DiscInfo discInfoServerKey = Singleton<SongManager>.instance.GetDiscInfoServerKey(text2);
									houseStage2.iSong = discInfoServerKey.Id;
								}
								if (dictionary3.ContainsKey("patternId"))
								{
									int num = (int)((long)dictionary3["patternId"]);
									houseStage2.PtType = Singleton<SongManager>.instance.GetPtIdToPtLevel(num);
								}
								rankInfo2.ArrHauseInfo.Add(houseStage2);
							}
						}
					}
					hausModeRank[k] = rankInfo2;
				}
			}
		}
		if (dictionary.ContainsKey("albums"))
		{
			List<object> list3 = (List<object>)dictionary["albums"];
			if (list3 != null)
			{
				for (int m = 0; m < list3.Count; m++)
				{
					Dictionary<string, object> dictionary4 = list3[m] as Dictionary<string, object>;
					RankInfo rankInfo3 = new RankInfo();
					if (dictionary4.ContainsKey("name"))
					{
						rankInfo3.Name = (string)dictionary4["name"];
					}
					if (dictionary4.ContainsKey("nation"))
					{
						rankInfo3.Nation = (string)dictionary4["nation"];
					}
					if (dictionary4.ContainsKey("level"))
					{
						rankInfo3.Level = (int)((long)dictionary4["level"]);
					}
					if (dictionary4.ContainsKey("icon"))
					{
						rankInfo3.WebIcon = (string)dictionary4["icon"];
					}
					if (dictionary4.ContainsKey("iconId"))
					{
						rankInfo3.Icon = (int)((long)dictionary4["iconId"]);
					}
					if (dictionary4.ContainsKey("score"))
					{
						rankInfo3.Score = (int)((long)dictionary4["score"]);
					}
					if (dictionary4.ContainsKey("ranking"))
					{
						rankInfo3.Ranking = (int)((long)dictionary4["ranking"]);
					}
					if (dictionary4.ContainsKey("rankClass"))
					{
						string text3 = (string)dictionary4["rankClass"];
						rankInfo3.RankClass = GameData.ParseEnum<GRADE>(text3);
					}
					if (dictionary4.ContainsKey("albumId"))
					{
						string text4 = (string)dictionary4["albumId"];
						rankInfo3.aInfo = Singleton<SongManager>.instance.GetAlbumServerKeyInfo(text4);
					}
					if (dictionary4.ContainsKey("musics"))
					{
						List<object> list4 = (List<object>)dictionary4["musics"];
						if (list4 != null)
						{
							rankInfo3.ArrRaveInfo.Clear();
							for (int n = 0; n < 4; n++)
							{
								Dictionary<string, object> dictionary5 = list4[n] as Dictionary<string, object>;
								RaveUpStage raveUpStage = new RaveUpStage();
								if (dictionary5.ContainsKey("musicId"))
								{
									string text5 = (string)dictionary5["musicId"];
									DiscInfo discInfoServerKey2 = Singleton<SongManager>.instance.GetDiscInfoServerKey(text5);
									raveUpStage.iSong = discInfoServerKey2.Id;
								}
								if (dictionary5.ContainsKey("patternId"))
								{
									int num2 = (int)((long)dictionary5["patternId"]);
									raveUpStage.PtType = Singleton<SongManager>.instance.GetPtIdToPtLevel(num2);
								}
								rankInfo3.ArrRaveInfo.Add(raveUpStage);
							}
						}
					}
					raveModeRank.Add(rankInfo3);
				}
			}
		}
	}
}
