using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000151 RID: 337
public class WWWMissionUserRecordScript : WWWObject
{
	// Token: 0x06000AAA RID: 2730
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netUserRecordData;
		Singleton<GameManager>.instance.UserData.BestScore = 0;
		this.m_sData.Init();
		string text = string.Format("mission/{0}/user/{1}", this.strMissionID, Singleton<GameManager>.instance.s_UserID);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AAB RID: 2731
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
		this.ParsingUserRecord(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AAC RID: 2732
	private void ParsingUserRecord(string strContent)
	{
		this.m_sData.Loading = false;
		this.m_sData.OnComplete = true;
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		if (dictionary.ContainsKey("best"))
		{
			Dictionary<string, object> dictionary2 = dictionary["best"] as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				if (dictionary2.ContainsKey("score"))
				{
					this.m_sData.Score = (int)((long)dictionary2["score"]);
					Singleton<GameManager>.instance.UserData.BestScore = this.m_sData.Score;
				}
				if (dictionary2.ContainsKey("maxCombo"))
				{
					this.m_sData.MaxCombo = (int)((long)dictionary2["maxCombo"]);
				}
				if (dictionary2.ContainsKey("accuracy"))
				{
					this.m_sData.Accuracy = (int)((long)dictionary2["accuracy"]);
				}
				if (dictionary2.ContainsKey("rankClass"))
				{
					int enumIndex = GameData.GetEnumIndex<GRADE>((string)dictionary2["rankClass"]);
					if (enumIndex != -1)
					{
						int num = (int)Enum.GetValues(typeof(GRADE)).GetValue(enumIndex);
						this.m_sData.RankClass = (GRADE)num;
					}
				}
			}
		}
		if (dictionary.ContainsKey("bestEmblem"))
		{
			Dictionary<string, object> dictionary3 = dictionary["bestEmblem"] as Dictionary<string, object>;
			if (dictionary3 != null)
			{
				if (dictionary3.ContainsKey("trophy"))
				{
					this.m_sData.strTrophy = (string)dictionary3["trophy"];
				}
				if (dictionary3.ContainsKey("allCombo"))
				{
					this.m_sData.AllCombo = (bool)dictionary3["allCombo"];
				}
				if (dictionary3.ContainsKey("perfectPlay"))
				{
					this.m_sData.PerfectPlay = (bool)dictionary3["perfectPlay"];
				}
			}
		}
	}

	// Token: 0x04000A6C RID: 2668
	public string strMissionID = "53bbb9bbe4b0c23e7553c795";

	// Token: 0x04000A6D RID: 2669
	private UserRecordData m_sData;
}
