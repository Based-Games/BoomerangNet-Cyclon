using System;
using UnityEngine;

// Token: 0x02000232 RID: 562
[Serializable]
public class tk2dCameraSettings
{
	// Token: 0x040011C9 RID: 4553
	public tk2dCameraSettings.ProjectionType projection;

	// Token: 0x040011CA RID: 4554
	public float orthographicSize = 10f;

	// Token: 0x040011CB RID: 4555
	public float orthographicPixelsPerMeter = 20f;

	// Token: 0x040011CC RID: 4556
	public tk2dCameraSettings.OrthographicOrigin orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.Center;

	// Token: 0x040011CD RID: 4557
	public tk2dCameraSettings.OrthographicType orthographicType;

	// Token: 0x040011CE RID: 4558
	public float fieldOfView = 60f;

	// Token: 0x040011CF RID: 4559
	public Rect rect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x02000233 RID: 563
	public enum ProjectionType
	{
		// Token: 0x040011D1 RID: 4561
		Orthographic,
		// Token: 0x040011D2 RID: 4562
		Perspective
	}

	// Token: 0x02000234 RID: 564
	public enum OrthographicType
	{
		// Token: 0x040011D4 RID: 4564
		PixelsPerMeter,
		// Token: 0x040011D5 RID: 4565
		OrthographicSize
	}

	// Token: 0x02000235 RID: 565
	public enum OrthographicOrigin
	{
		// Token: 0x040011D7 RID: 4567
		BottomLeft,
		// Token: 0x040011D8 RID: 4568
		Center
	}
}
