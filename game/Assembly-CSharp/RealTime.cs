using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class RealTime : MonoBehaviour
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000366 RID: 870 RVA: 0x00005CD3 File Offset: 0x00003ED3
	public static float time
	{
		get
		{
			if (RealTime.mInst == null)
			{
				RealTime.Spawn();
			}
			return RealTime.mInst.mRealTime;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000367 RID: 871 RVA: 0x00005CF4 File Offset: 0x00003EF4
	public static float deltaTime
	{
		get
		{
			if (RealTime.mInst == null)
			{
				RealTime.Spawn();
			}
			return RealTime.mInst.mRealDelta;
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00022624 File Offset: 0x00020824
	private static void Spawn()
	{
		GameObject gameObject = new GameObject("_RealTime");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		RealTime.mInst = gameObject.AddComponent<RealTime>();
		RealTime.mInst.mRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0002265C File Offset: 0x0002085C
	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		this.mRealDelta = realtimeSinceStartup - this.mRealTime;
		this.mRealTime = realtimeSinceStartup;
	}

	// Token: 0x040002C8 RID: 712
	private static RealTime mInst;

	// Token: 0x040002C9 RID: 713
	private float mRealTime;

	// Token: 0x040002CA RID: 714
	private float mRealDelta;
}
