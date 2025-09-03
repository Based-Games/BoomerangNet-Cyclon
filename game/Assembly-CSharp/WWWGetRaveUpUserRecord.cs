using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000168 RID: 360
public class WWWGetRaveUpUserRecord : WWWObject
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x0004FE84 File Offset: 0x0004E084
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netUserRecordData;
		Singleton<GameManager>.instance.UserData.BestScore = 0;
		string text = string.Format("album/{0}/user/{1}/bestRanking", this.strAlbumId, Singleton<GameManager>.instance.s_UserID);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0004FEDC File Offset: 0x0004E0DC
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.CallBackFail != null)
		{
			this.CallBackFail();
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

	// Token: 0x06000B05 RID: 2821 RVA: 0x0004FF4C File Offset: 0x0004E14C
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

	// Token: 0x04000A85 RID: 2693
	public string strAlbumId = "535e06912c658fc297b5aa24";

	// Token: 0x04000A86 RID: 2694
	private UserRecordData m_sData;
}
