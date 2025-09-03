using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MiniJSON;

// Token: 0x02000163 RID: 355
public class WWWNotice : WWWObject
{
	// Token: 0x06000AF0 RID: 2800
	public override void StartLoad()
	{
		string text = "games/notices";
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AF1 RID: 2801
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			if (Singleton<GameManager>.instance.ONNETWORK)
			{
				Singleton<GameManager>.instance.ONNETWORK = false;
				Singleton<WWWManager>.instance.GetEmergencyCheck();
			}
			if (this.CallBackFail != null)
			{
				this.CallBackFail();
			}
			return;
		}
		if (!Singleton<GameManager>.instance.ONNETWORK)
		{
			Singleton<GameManager>.instance.ONNETWORK = true;
		}
		this.ParsingData(this.www.text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AF2 RID: 2802
	private void ParsingData(string strTxt)
	{
		ArrayList arrNotice = Singleton<GameManager>.instance.ArrNotice;
		arrNotice.Clear();
		Dictionary<string, object> dictionary = Json.Deserialize(strTxt) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		if (dictionary.ContainsKey("needUpdate"))
		{
			Singleton<GameManager>.instance.UPDATE = (bool)dictionary["needUpdate"];
		}
		else
		{
			Singleton<GameManager>.instance.UPDATE = false;
		}
		if (dictionary.ContainsKey("demoSongs"))
		{
			List<SongManager.DemoSong> list = new List<SongManager.DemoSong>();
			List<object> list2 = (List<object>)dictionary["demoSongs"];
			if (list2 != null)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					Dictionary<string, object> dictionary3 = list2[i] as Dictionary<string, object>;
					int num = (int)((long)dictionary3["id"]);
					int num2 = (int)((long)dictionary3["songId"]);
					int num3 = (int)((long)dictionary3["pt"]);
					list.Add(new SongManager.DemoSong
					{
						id = num,
						songId = num2,
						pt = num3
					});
				}
				Singleton<SongManager>.instance.demoSongs = list;
			}
		}
		if (dictionary.ContainsKey("notices"))
		{
			List<object> list3 = (List<object>)dictionary["notices"];
			if (list3 != null)
			{
				for (int j = 0; j < list3.Count; j++)
				{
					Dictionary<string, object> dictionary2 = list3[j] as Dictionary<string, object>;
					NoticeInfo noticeInfo = new NoticeInfo();
					string empty = string.Empty;
					if (dictionary2.ContainsKey("noticeNo"))
					{
						noticeInfo.NoticeNum = (int)((long)dictionary2["noticeNo"]);
					}
					if (dictionary2.ContainsKey("nation"))
					{
						noticeInfo.nation = (string)dictionary2["nation"];
					}
					if (dictionary2.ContainsKey("title"))
					{
						noticeInfo.title = (string)dictionary2["title"];
					}
					if (dictionary2.ContainsKey("noticeType"))
					{
						string text = (string)dictionary2["noticeType"];
						if ("notice" == text)
						{
							noticeInfo.eType = NOTICETYPE.NOTICE;
						}
						else if ("event" == text)
						{
							noticeInfo.eType = NOTICETYPE.EVENT;
						}
					}
					if (dictionary2.ContainsKey("contentType"))
					{
						string text2 = (string)dictionary2["contentType"];
						if ("text" == text2)
						{
							noticeInfo.eContent = NOTICECONTENT.TEXT;
						}
						else if ("image" == text2)
						{
							noticeInfo.eContent = NOTICECONTENT.IMAGE;
						}
					}
					if (dictionary2.ContainsKey("content"))
					{
						noticeInfo.content = (string)dictionary2["content"];
						noticeInfo.eSize = NOTICESIZE.SMALL;
						if (noticeInfo.content != null && 4 < Regex.Matches(noticeInfo.content, "\n").Count)
						{
							noticeInfo.eSize = NOTICESIZE.BIG;
						}
					}
					if (dictionary2.ContainsKey("content2"))
					{
						noticeInfo.content2 = (string)dictionary2["content2"];
					}
					if (dictionary2.ContainsKey("image"))
					{
						noticeInfo.imagepath = (string)dictionary2["image"];
					}
					if (0 < noticeInfo.NoticeNum && noticeInfo.NoticeNum / 1000 == 1)
					{
						arrNotice.Add(noticeInfo);
					}
				}
			}
		}
	}
}
