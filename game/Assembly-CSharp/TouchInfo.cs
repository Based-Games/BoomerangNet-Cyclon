using System;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class TouchInfo
{
	// Token: 0x0400062E RID: 1582
	public PULSEIR_POINT Point = default(PULSEIR_POINT);

	// Token: 0x0400062F RID: 1583
	public int m_iPreType;

	// Token: 0x04000630 RID: 1584
	public TouchPhase m_ePhase = TouchPhase.Ended;
}
