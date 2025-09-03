using System;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public static class tk2dUITime
{
	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001482 RID: 5250 RVA: 0x00011C13 File Offset: 0x0000FE13
	public static float deltaTime
	{
		get
		{
			return tk2dUITime._deltaTime;
		}
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x00011C1A File Offset: 0x0000FE1A
	public static void Init()
	{
		tk2dUITime.lastRealTime = (double)Time.realtimeSinceStartup;
		tk2dUITime._deltaTime = Time.maximumDeltaTime;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x0008ACB4 File Offset: 0x00088EB4
	public static void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (Time.timeScale < 0.001f)
		{
			tk2dUITime._deltaTime = Mathf.Min(0.06666667f, (float)((double)realtimeSinceStartup - tk2dUITime.lastRealTime));
		}
		else
		{
			tk2dUITime._deltaTime = Time.deltaTime / Time.timeScale;
		}
		tk2dUITime.lastRealTime = (double)realtimeSinceStartup;
	}

	// Token: 0x040015DD RID: 5597
	private static double lastRealTime;

	// Token: 0x040015DE RID: 5598
	private static float _deltaTime = 0.016666668f;
}
