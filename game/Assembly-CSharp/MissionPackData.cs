using System;
using System.Collections;

// Token: 0x02000143 RID: 323
public class MissionPackData
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0004A96C File Offset: 0x00048B6C
	public bool Lock
	{
		get
		{
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			foreach (object obj in this.ArrMissionData)
			{
				MissionData missionData = (MissionData)obj;
				if (missionData.iMissionOpenLevel > userData.Level)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x040009D2 RID: 2514
	public int iPackId;

	// Token: 0x040009D3 RID: 2515
	public string strPackTitle;

	// Token: 0x040009D4 RID: 2516
	public ArrayList ArrMissionData = new ArrayList();
}
