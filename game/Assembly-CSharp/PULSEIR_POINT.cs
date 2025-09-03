using System;
using System.Runtime.InteropServices;

// Token: 0x020000E4 RID: 228
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PULSEIR_POINT
{
	// Token: 0x04000628 RID: 1576
	public int id;

	// Token: 0x04000629 RID: 1577
	public int type;

	// Token: 0x0400062A RID: 1578
	public int x;

	// Token: 0x0400062B RID: 1579
	public int y;

	// Token: 0x0400062C RID: 1580
	public int w;

	// Token: 0x0400062D RID: 1581
	public int h;
}
