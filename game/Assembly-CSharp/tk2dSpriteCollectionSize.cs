using System;

// Token: 0x02000278 RID: 632
[Serializable]
public class tk2dSpriteCollectionSize
{
	// Token: 0x0600121A RID: 4634 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
	public static tk2dSpriteCollectionSize Explicit(float orthoSize, float targetHeight)
	{
		return tk2dSpriteCollectionSize.ForResolution(orthoSize, targetHeight, targetHeight);
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x0007D1BC File Offset: 0x0007B3BC
	public static tk2dSpriteCollectionSize PixelsPerMeter(float pixelsPerMeter)
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.PixelsPerMeter,
			pixelsPerMeter = pixelsPerMeter
		};
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x0007D1E0 File Offset: 0x0007B3E0
	public static tk2dSpriteCollectionSize ForResolution(float orthoSize, float width, float height)
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.Explicit,
			orthoSize = orthoSize,
			width = width,
			height = height
		};
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x0007D210 File Offset: 0x0007B410
	public static tk2dSpriteCollectionSize ForTk2dCamera()
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.PixelsPerMeter,
			pixelsPerMeter = 1f
		};
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x0007D238 File Offset: 0x0007B438
	public static tk2dSpriteCollectionSize ForTk2dCamera(tk2dCamera camera)
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dCameraSettings cameraSettings = camera.SettingsRoot.CameraSettings;
		if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Orthographic)
		{
			tk2dCameraSettings.OrthographicType orthographicType = cameraSettings.orthographicType;
			if (orthographicType != tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				if (orthographicType == tk2dCameraSettings.OrthographicType.OrthographicSize)
				{
					tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.Explicit;
					tk2dSpriteCollectionSize.height = (float)camera.nativeResolutionHeight;
					tk2dSpriteCollectionSize.orthoSize = cameraSettings.orthographicSize;
				}
			}
			else
			{
				tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
				tk2dSpriteCollectionSize.pixelsPerMeter = cameraSettings.orthographicPixelsPerMeter;
			}
		}
		else if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			tk2dSpriteCollectionSize.pixelsPerMeter = 20f;
		}
		return tk2dSpriteCollectionSize;
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x0000F7B2 File Offset: 0x0000D9B2
	public static tk2dSpriteCollectionSize Default()
	{
		return tk2dSpriteCollectionSize.PixelsPerMeter(20f);
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x0000F7BE File Offset: 0x0000D9BE
	public void CopyFromLegacy(bool useTk2dCamera, float orthoSize, float targetHeight)
	{
		if (useTk2dCamera)
		{
			this.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			this.pixelsPerMeter = 1f;
		}
		else
		{
			this.type = tk2dSpriteCollectionSize.Type.Explicit;
			this.height = targetHeight;
			this.orthoSize = orthoSize;
		}
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x0000F7F2 File Offset: 0x0000D9F2
	public void CopyFrom(tk2dSpriteCollectionSize source)
	{
		this.type = source.type;
		this.width = source.width;
		this.height = source.height;
		this.orthoSize = source.orthoSize;
		this.pixelsPerMeter = source.pixelsPerMeter;
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001222 RID: 4642 RVA: 0x0007D2DC File Offset: 0x0007B4DC
	public float OrthoSize
	{
		get
		{
			tk2dSpriteCollectionSize.Type type = this.type;
			if (type == tk2dSpriteCollectionSize.Type.Explicit)
			{
				return this.orthoSize;
			}
			if (type != tk2dSpriteCollectionSize.Type.PixelsPerMeter)
			{
				return this.orthoSize;
			}
			return 0.5f;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001223 RID: 4643 RVA: 0x0007D318 File Offset: 0x0007B518
	public float TargetHeight
	{
		get
		{
			tk2dSpriteCollectionSize.Type type = this.type;
			if (type == tk2dSpriteCollectionSize.Type.Explicit)
			{
				return this.height;
			}
			if (type != tk2dSpriteCollectionSize.Type.PixelsPerMeter)
			{
				return this.height;
			}
			return this.pixelsPerMeter;
		}
	}

	// Token: 0x0400141F RID: 5151
	public tk2dSpriteCollectionSize.Type type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;

	// Token: 0x04001420 RID: 5152
	public float orthoSize = 10f;

	// Token: 0x04001421 RID: 5153
	public float pixelsPerMeter = 20f;

	// Token: 0x04001422 RID: 5154
	public float width = 960f;

	// Token: 0x04001423 RID: 5155
	public float height = 640f;

	// Token: 0x02000279 RID: 633
	public enum Type
	{
		// Token: 0x04001425 RID: 5157
		Explicit,
		// Token: 0x04001426 RID: 5158
		PixelsPerMeter
	}
}
