using System;
using System.Runtime.InteropServices;
using DiscordRpc;

// Token: 0x02000439 RID: 1081
public class DiscordRichPresenceController : Singleton<DiscordRichPresenceController>
{
	// Token: 0x06001531 RID: 5425 RVA: 0x00003648 File Offset: 0x00001848
	private void Awake()
	{
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x0008D5CC File Offset: 0x0008B7CC
	public void InitializeDiscordRPC()
	{
		EventHandlers eventHandlers = default(EventHandlers);
		DiscordRichPresenceController.Discord_Initialize("1079499078578221268", ref eventHandlers, true, null);
		this.SetInitPresence();
		Logger.Log("DiscordRichPresenceController", "Initialized RPC", new object[0]);
	}

	// Token: 0x06001533 RID: 5427
	[DllImport("DiscordRPC")]
	private static extern void Discord_Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

	// Token: 0x06001534 RID: 5428
	[DllImport("DiscordRPC")]
	private static extern void Discord_UpdatePresence(ref RichPresence presence);

	// Token: 0x06001535 RID: 5429 RVA: 0x0008D60C File Offset: 0x0008B80C
	public void UpdateDiscordPresence(string state, string details)
	{
		RichPresence richPresence = default(RichPresence);
		richPresence.largeImageKey = "circlon";
		richPresence.state = state;
		richPresence.details = details;
		DiscordRichPresenceController.Discord_UpdatePresence(ref richPresence);
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x0008D644 File Offset: 0x0008B844
	public void SetInitPresence()
	{
		RichPresence richPresence = default(RichPresence);
		richPresence.largeImageKey = "circlon";
		richPresence.state = "In attract loop";
		richPresence.details = "Idle";
		DiscordRichPresenceController.Discord_UpdatePresence(ref richPresence);
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x0008D684 File Offset: 0x0008B884
	public void SetSongPresence(int songLength)
	{
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		int num = 3;
		string text;
		int num2;
		switch (Singleton<SongManager>.instance.Mode)
		{
		case GAMEMODE.HAUSMIX:
			text = "HAUS MIX";
			num2 = (int)Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage].PtType;
			break;
		case GAMEMODE.RAVEUP:
			text = "RAVE UP";
			num2 = (int)Singleton<SongManager>.instance.RaveUpSelectSong[GameData.Stage].PtType;
			num = 4;
			break;
		case GAMEMODE.MISSION:
			text = "CLUB TOUR";
			num2 = (int)Singleton<SongManager>.instance.Mission.Pattern[GameData.Stage];
			break;
		default:
			throw new NotImplementedException();
		}
		string text2 = "EZ";
		switch (num2)
		{
		case 1:
			text2 = "EZ";
			break;
		case 2:
			text2 = "NM";
			break;
		case 3:
			text2 = "HD";
			break;
		case 4:
			text2 = "PR";
			break;
		case 5:
			text2 = "MX";
			break;
		case 6:
			text2 = "S1";
			break;
		case 7:
			text2 = "S2";
			break;
		}
		RichPresence richPresence = default(RichPresence);
		richPresence.state = "Playing " + text;
		richPresence.details = string.Concat(new string[] { currentDisc.FullName, " | ", currentDisc.Artist, " | ", text2 });
		richPresence.partyId = currentDisc.ServerID;
		richPresence.partySize = GameData.Stage + 1;
		richPresence.partyMax = num;
		richPresence.largeImageKey = "https://beathaus.co.kr/assetstore/discs/" + currentDisc.ServerID + ".png";
		DateTimeOffset dateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		richPresence.startTimestamp = (long)(utcNow - dateTimeOffset).TotalMilliseconds;
		richPresence.endTimestamp = richPresence.startTimestamp + (long)songLength;
		DiscordRichPresenceController.Discord_UpdatePresence(ref richPresence);
	}

	// Token: 0x04001BDD RID: 7133
	private const string DiscordApplicationID = "1079499078578221268";
}
