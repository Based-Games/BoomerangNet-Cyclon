using System;

// Token: 0x020000D6 RID: 214
public class SChangeTPSInfo
{
	// Token: 0x0600070E RID: 1806 RVA: 0x00008846 File Offset: 0x00006A46
	public void Set(int _tick, float _tps)
	{
		this.tick = _tick;
		this.tps = _tps;
	}

	// Token: 0x040005AD RID: 1453
	public int tick;

	// Token: 0x040005AE RID: 1454
	public float tps;
}
