using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000165 RID: 357
public class WWWServerLv : WWWObject
{
	// Token: 0x06000AF7 RID: 2807 RVA: 0x0000A6AB File Offset: 0x000088AB
	public override void StartLoad()
	{
		this.www = WWWRequest.Create("baseData/userLevelTable", null);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0004F928 File Offset: 0x0004DB28
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
			Logger.Warn("WWWServerLv", "Request failed: {0}", new object[] { this.www.error });
			return;
		}
		string text = this.www.text;
		this.ParsingResult(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0004F9A8 File Offset: 0x0004DBA8
	private void ParsingResult(string strContent)
	{
		Singleton<SongManager>.instance.AllLevel.Clear();
		List<object> list = Json.Deserialize(strContent) as List<object>;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				LevelData levelData = new LevelData();
				if (dictionary.ContainsKey("level"))
				{
					levelData.Level = (int)((long)dictionary["level"]);
				}
				if (dictionary.ContainsKey("levelUpExp"))
				{
					levelData.Exp = (int)((long)dictionary["levelUpExp"]);
				}
				if (dictionary.ContainsKey("maxExp"))
				{
					levelData.MaxExp = (int)((long)dictionary["maxExp"]);
				}
				Singleton<SongManager>.instance.AllLevel.Add(levelData);
			}
		}
		if (0 < Singleton<SongManager>.instance.AllLevel.Count)
		{
			Singleton<SongManager>.instance.AllLevel.Sort(new SortLv());
		}
	}
}
