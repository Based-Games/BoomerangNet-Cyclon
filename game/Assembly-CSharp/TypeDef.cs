using System;

// Token: 0x020000DB RID: 219
public class TypeDef
{
	// Token: 0x06000759 RID: 1881 RVA: 0x00008B27 File Offset: 0x00006D27
	public static int GETTIMEINTERVAL(int iCur, int iPrev)
	{
		return (iPrev > iCur) ? ((int)((long)iCur + (long)((ulong)(-1)) - (long)iPrev)) : (iCur - iPrev);
	}

	// Token: 0x040005D4 RID: 1492
	public const int TRACK_MAX_NUM = 64;

	// Token: 0x040005D5 RID: 1493
	public const float VOL_DEF_NUM = 1f;

	// Token: 0x040005D6 RID: 1494
	public const float VEL_DEF_NUM = 1f;

	// Token: 0x040005D7 RID: 1495
	public const int DUR_DEF_NUM = 0;
}
