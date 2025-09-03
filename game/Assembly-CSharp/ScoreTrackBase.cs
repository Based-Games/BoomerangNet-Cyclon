using System;

// Token: 0x020000DA RID: 218
public class ScoreTrackBase
{
	// Token: 0x06000755 RID: 1877 RVA: 0x00008B10 File Offset: 0x00006D10
	public void Init(int trackIDX)
	{
		this.m_trackIDX = trackIDX;
		this.OnInit();
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00008B1F File Offset: 0x00006D1F
	public int GetTrackIDX()
	{
		return this.m_trackIDX;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00003648 File Offset: 0x00001848
	private void OnInit()
	{
	}

	// Token: 0x040005D3 RID: 1491
	private int m_trackIDX;
}
