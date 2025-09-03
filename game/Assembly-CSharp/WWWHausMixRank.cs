using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000157 RID: 343
public class WWWHausMixRank : WWWObject
{
	// Token: 0x06000ABF RID: 2751 RVA: 0x0004C49C File Offset: 0x0004A69C
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netHouseMixData;
		this.m_sData.dInfo = this.dInfo;
		this.m_sData.Loading = true;
		this.m_sData.OnComplete = false;
		Singleton<GameManager>.instance.UserData.BestScore = 0;
		string text = string.Format("music/{0}/user/{1}/bestRankings", this.MusicId, Singleton<GameManager>.instance.s_UserID);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0004C51C File Offset: 0x0004A71C
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
		this.ParsingBestRanking(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0004C57C File Offset: 0x0004A77C
	private void ParsingBestRanking(string strContent)
	{
		this.m_sData.Loading = false;
		this.m_sData.OnComplete = true;
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		this.m_sData.Init();
		if (dictionary.ContainsKey("userPatternsStatus"))
		{
			List<object> list = (List<object>)dictionary["userPatternsStatus"];
			for (int i = 0; i < list.Count; i++)
			{
				PatternScoreInfo patternScoreInfo = new PatternScoreInfo();
				Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
				if (dictionary2.ContainsKey("patternId"))
				{
					patternScoreInfo.PatternIdx = (int)((long)dictionary2["patternId"]);
				}
				if (dictionary2.ContainsKey("best"))
				{
					Dictionary<string, object> dictionary3 = dictionary2["best"] as Dictionary<string, object>;
					if (dictionary3 != null)
					{
						if (dictionary3.ContainsKey("score"))
						{
							patternScoreInfo.BestScore = (int)((long)dictionary3["score"]);
						}
						if (dictionary3.ContainsKey("noteScore"))
						{
							patternScoreInfo.NoteScore = (int)((long)dictionary3["noteScore"]);
						}
						if (dictionary3.ContainsKey("maxCombo"))
						{
							patternScoreInfo.MaxCombo = (int)((long)dictionary3["maxCombo"]);
						}
						if (dictionary3.ContainsKey("realCombo"))
						{
							patternScoreInfo.RealCombo = (int)((long)dictionary3["realCombo"]);
						}
						if (dictionary3.ContainsKey("accuracy"))
						{
							patternScoreInfo.Accuracy = (int)((long)dictionary3["accuracy"]);
						}
						if (dictionary3.ContainsKey("rankClass"))
						{
							int enumIndex = GameData.GetEnumIndex<GRADE>((string)dictionary3["rankClass"]);
							if (enumIndex != -1)
							{
								int num = (int)Enum.GetValues(typeof(GRADE)).GetValue(enumIndex);
								patternScoreInfo.RankClass = (GRADE)num;
							}
						}
					}
				}
				if (dictionary2.ContainsKey("bestEmblem"))
				{
					Dictionary<string, object> dictionary4 = dictionary2["bestEmblem"] as Dictionary<string, object>;
					if (dictionary4 != null)
					{
						if (dictionary4.ContainsKey("trophy"))
						{
							patternScoreInfo.strTrophy = (string)dictionary4["trophy"];
						}
						if (dictionary4.ContainsKey("allCombo"))
						{
							patternScoreInfo.AllCombo = (bool)dictionary4["allCombo"];
						}
						if (dictionary4.ContainsKey("perfectPlay"))
						{
							patternScoreInfo.PerfectPlay = (bool)dictionary4["perfectPlay"];
						}
					}
				}
				patternScoreInfo.ArrScores.Clear();
				if (dictionary2.ContainsKey("recently"))
				{
					List<object> list2 = (List<object>)dictionary2["recently"];
					if (list2 != null)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							Dictionary<string, object> dictionary5 = list2[j] as Dictionary<string, object>;
							if (dictionary5.ContainsKey("score"))
							{
								patternScoreInfo.ArrScores.Add((int)((long)dictionary5["score"]));
							}
						}
					}
				}
				PTLEVEL ptIdToPtLevel = Singleton<SongManager>.instance.GetPtIdToPtLevel(patternScoreInfo.PatternIdx);
				patternScoreInfo.PTTYPE = ptIdToPtLevel;
				this.m_sData.DicPatternInfo[ptIdToPtLevel] = patternScoreInfo;
			}
		}
		if (dictionary.ContainsKey("weeklyTop"))
		{
			this.m_sData.ArrLocalRank.Clear();
			List<object> list3 = (List<object>)dictionary["weeklyTop"];
			if (list3 != null)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					Dictionary<string, object> dictionary6 = list3[k] as Dictionary<string, object>;
					RankInfo rankInfo = new RankInfo();
					if (dictionary6.ContainsKey("name"))
					{
						rankInfo.Name = (string)dictionary6["name"];
					}
					if (dictionary6.ContainsKey("nation"))
					{
						rankInfo.Nation = (string)dictionary6["nation"];
					}
					if (dictionary6.ContainsKey("score"))
					{
						rankInfo.Score = (int)((long)dictionary6["score"]);
					}
					if (dictionary6.ContainsKey("ranking"))
					{
						rankInfo.Ranking = (int)((long)dictionary6["ranking"]);
					}
					if (dictionary6.ContainsKey("iconId"))
					{
						rankInfo.Icon = (int)((long)dictionary6["iconId"]);
					}
					this.m_sData.ArrLocalRank.Add(rankInfo);
				}
			}
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0004C9F0 File Offset: 0x0004ABF0
	private void ParsingBestRankingForGuest(string strContent)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		if (dictionary.ContainsKey("userPatternsStatus"))
		{
			PatternScoreInfo patternScoreInfo = new PatternScoreInfo();
			Dictionary<string, object> dictionary2 = ((List<object>)dictionary["userPatternsStatus"])[0] as Dictionary<string, object>;
			if (dictionary2.ContainsKey("patternId"))
			{
				patternScoreInfo.PatternIdx = (int)((long)dictionary2["patternId"]);
			}
			if (dictionary2.ContainsKey("best"))
			{
				Dictionary<string, object> dictionary3 = dictionary2["best"] as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					if (dictionary3.ContainsKey("score"))
					{
						patternScoreInfo.BestScore = (int)((long)dictionary3["score"]);
					}
					if (dictionary3.ContainsKey("noteScore"))
					{
						patternScoreInfo.NoteScore = (int)((long)dictionary3["noteScore"]);
					}
					if (dictionary3.ContainsKey("maxCombo"))
					{
						patternScoreInfo.MaxCombo = (int)((long)dictionary3["maxCombo"]);
					}
					if (dictionary3.ContainsKey("realCombo"))
					{
						patternScoreInfo.RealCombo = (int)((long)dictionary3["realCombo"]);
					}
					if (dictionary3.ContainsKey("accuracy"))
					{
						patternScoreInfo.Accuracy = (int)((long)dictionary3["accuracy"]);
					}
					if (dictionary3.ContainsKey("rankClass"))
					{
						int enumIndex = GameData.GetEnumIndex<GRADE>((string)dictionary3["rankClass"]);
						if (enumIndex != -1)
						{
							int num = (int)Enum.GetValues(typeof(GRADE)).GetValue(enumIndex);
							patternScoreInfo.RankClass = (GRADE)num;
						}
					}
				}
			}
			if (dictionary2.ContainsKey("bestEmblem"))
			{
				Dictionary<string, object> dictionary4 = dictionary2["bestEmblem"] as Dictionary<string, object>;
				if (dictionary4 != null)
				{
					if (dictionary4.ContainsKey("trophy"))
					{
						patternScoreInfo.strTrophy = (string)dictionary4["trophy"];
					}
					if (dictionary4.ContainsKey("allCombo"))
					{
						patternScoreInfo.AllCombo = (bool)dictionary4["allCombo"];
					}
					if (dictionary4.ContainsKey("perfectPlay"))
					{
						patternScoreInfo.PerfectPlay = (bool)dictionary4["perfectPlay"];
					}
				}
			}
			patternScoreInfo.ArrScores.Clear();
			PTLEVEL ptIdToPtLevel = Singleton<SongManager>.instance.GetPtIdToPtLevel(patternScoreInfo.PatternIdx);
			patternScoreInfo.PTTYPE = ptIdToPtLevel;
			this.m_sData.DicPatternInfo[ptIdToPtLevel] = patternScoreInfo;
		}
	}

	// Token: 0x04000A70 RID: 2672
	private HouseMixSelectData m_sData;

	// Token: 0x04000A71 RID: 2673
	public string MusicId = "535e06912c658fc297b5aa12";

	// Token: 0x04000A72 RID: 2674
	public DiscInfo dInfo;
}
