using System;
using System.Collections;
using System.Collections.Generic;
using WyrmTale;

// Token: 0x02000164 RID: 356
public class WWWPostConfigration : WWWObject
{
	// Token: 0x06000AF4 RID: 2804
	public override void StartLoad()
	{
		string text2 = string.Format("users/{0}/configurations", Singleton<GameManager>.instance.s_UserID);
		this.www = WWWRequest.Create(text2, this.GeneratePayload());
	}

	// Token: 0x06000AF5 RID: 2805
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
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x0600164E RID: 5710
	private string GeneratePayload()
	{
		JSON json = new JSON();
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		if (EFFECTOR_SPEED.MAX_SPEED > GameData.SPEEDEFFECTOR)
		{
			userData.UserSpeed = GameData.SPEEDEFFECTOR;
		}
		json["KeyEFfect"] = Singleton<SoundSourceManager>.instance.EFF_NUM;
		json["KeyVolume"] = Singleton<SoundSourceManager>.instance.EFF_VOLUME.ToString();
		json["IsUseKeySound"] = userData.IsUseKeySound.ToString();
		json["Speed"] = userData.UserSpeed;
		string text = string.Empty;
		ArrayList allEventData = Singleton<SongManager>.instance.AllEventData;
		for (int i = 0; i < allEventData.Count; i++)
		{
			EventSongData eventSongData = (EventSongData)allEventData[i];
			if (i != 0)
			{
				text += ",";
			}
			text += eventSongData.GetEventData();
		}
		json["EventSong"] = text;
		Dictionary<PTLEVEL, int> dictionary = new Dictionary<PTLEVEL, int>
		{
			{
				PTLEVEL.EZ,
				0
			},
			{
				PTLEVEL.NM,
				1
			},
			{
				PTLEVEL.HD,
				2
			},
			{
				PTLEVEL.PR,
				3
			},
			{
				PTLEVEL.MX,
				4
			},
			{
				PTLEVEL.S1,
				5
			},
			{
				PTLEVEL.S2,
				6
			}
		};
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			json["lastSong"] = HouseMixManager.LastDiscID;
			json["lastPT"] = dictionary[HouseMixManager.LastPtType];
			json["lastScrollPosition"] = HouseMixManager.LastScrollPosition;
		}
		return json.serialized;
	}
}
