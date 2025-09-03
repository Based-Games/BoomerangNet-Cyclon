using System;
using UnityEngine;

// Token: 0x02000236 RID: 566
[Serializable]
public class tk2dCameraResolutionOverride
{
	// Token: 0x0600103F RID: 4159 RVA: 0x00074640 File Offset: 0x00072840
	public bool Match(int pixelWidth, int pixelHeight)
	{
		switch (this.matchBy)
		{
		case tk2dCameraResolutionOverride.MatchByType.Resolution:
			return pixelWidth == this.width && pixelHeight == this.height;
		case tk2dCameraResolutionOverride.MatchByType.AspectRatio:
		{
			float num = (float)pixelWidth * this.aspectRatioDenominator / this.aspectRatioNumerator;
			return Mathf.Approximately(num, (float)pixelHeight);
		}
		case tk2dCameraResolutionOverride.MatchByType.Wildcard:
			return true;
		default:
			return false;
		}
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x000746A4 File Offset: 0x000728A4
	public void Upgrade(int version)
	{
		if (version == 0)
		{
			this.matchBy = (((this.width != -1 || this.height != -1) && (this.width != 0 || this.height != 0)) ? tk2dCameraResolutionOverride.MatchByType.Resolution : tk2dCameraResolutionOverride.MatchByType.Wildcard);
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06001041 RID: 4161 RVA: 0x000746F4 File Offset: 0x000728F4
	public static tk2dCameraResolutionOverride DefaultOverride
	{
		get
		{
			return new tk2dCameraResolutionOverride
			{
				name = "Override",
				matchBy = tk2dCameraResolutionOverride.MatchByType.Wildcard,
				autoScaleMode = tk2dCameraResolutionOverride.AutoScaleMode.FitVisible,
				fitMode = tk2dCameraResolutionOverride.FitMode.Center
			};
		}
	}

	// Token: 0x040011D9 RID: 4569
	public string name;

	// Token: 0x040011DA RID: 4570
	public tk2dCameraResolutionOverride.MatchByType matchBy;

	// Token: 0x040011DB RID: 4571
	public int width;

	// Token: 0x040011DC RID: 4572
	public int height;

	// Token: 0x040011DD RID: 4573
	public float aspectRatioNumerator = 4f;

	// Token: 0x040011DE RID: 4574
	public float aspectRatioDenominator = 3f;

	// Token: 0x040011DF RID: 4575
	public float scale = 1f;

	// Token: 0x040011E0 RID: 4576
	public Vector2 offsetPixels = new Vector2(0f, 0f);

	// Token: 0x040011E1 RID: 4577
	public tk2dCameraResolutionOverride.AutoScaleMode autoScaleMode;

	// Token: 0x040011E2 RID: 4578
	public tk2dCameraResolutionOverride.FitMode fitMode;

	// Token: 0x02000237 RID: 567
	public enum MatchByType
	{
		// Token: 0x040011E4 RID: 4580
		Resolution,
		// Token: 0x040011E5 RID: 4581
		AspectRatio,
		// Token: 0x040011E6 RID: 4582
		Wildcard
	}

	// Token: 0x02000238 RID: 568
	public enum AutoScaleMode
	{
		// Token: 0x040011E8 RID: 4584
		None,
		// Token: 0x040011E9 RID: 4585
		FitWidth,
		// Token: 0x040011EA RID: 4586
		FitHeight,
		// Token: 0x040011EB RID: 4587
		FitVisible,
		// Token: 0x040011EC RID: 4588
		StretchToFit,
		// Token: 0x040011ED RID: 4589
		ClosestMultipleOfTwo,
		// Token: 0x040011EE RID: 4590
		PixelPerfect
	}

	// Token: 0x02000239 RID: 569
	public enum FitMode
	{
		// Token: 0x040011F0 RID: 4592
		Constant,
		// Token: 0x040011F1 RID: 4593
		Center
	}
}
