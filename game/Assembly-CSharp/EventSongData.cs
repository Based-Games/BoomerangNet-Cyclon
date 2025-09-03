using System;

// Token: 0x0200014A RID: 330
public class EventSongData
{
	// Token: 0x06000A77 RID: 2679 RVA: 0x0004AE58 File Offset: 0x00049058
	public string GetEventData()
	{
		return string.Concat(new string[]
		{
			this.iSongId.ToString(),
			"_",
			this.ePt.ToString(),
			"_",
			this.iCount.ToString()
		});
	}

	// Token: 0x040009FE RID: 2558
	public int iSongId;

	// Token: 0x040009FF RID: 2559
	public PTLEVEL ePt = PTLEVEL.EZ;

	// Token: 0x04000A00 RID: 2560
	public int iCount;
}
