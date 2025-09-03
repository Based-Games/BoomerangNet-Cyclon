using System;
using UnityEngine;

// Token: 0x02000254 RID: 596
[AddComponentMenu("2D Toolkit/Deprecated/Extra/tk2dPixelPerfectHelper")]
public class tk2dPixelPerfectHelper : MonoBehaviour
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x0600115B RID: 4443 RVA: 0x000793B4 File Offset: 0x000775B4
	public static tk2dPixelPerfectHelper inst
	{
		get
		{
			if (tk2dPixelPerfectHelper._inst == null)
			{
				tk2dPixelPerfectHelper._inst = UnityEngine.Object.FindObjectOfType(typeof(tk2dPixelPerfectHelper)) as tk2dPixelPerfectHelper;
				if (tk2dPixelPerfectHelper._inst == null)
				{
					return null;
				}
				tk2dPixelPerfectHelper.inst.Setup();
			}
			return tk2dPixelPerfectHelper._inst;
		}
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x0000EB74 File Offset: 0x0000CD74
	private void Awake()
	{
		this.Setup();
		tk2dPixelPerfectHelper._inst = this;
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x0007940C File Offset: 0x0007760C
	public virtual void Setup()
	{
		float num = (float)this.collectionTargetHeight / this.targetResolutionHeight;
		if (base.camera != null)
		{
			this.cam = base.camera;
		}
		if (this.cam == null)
		{
			this.cam = Camera.main;
		}
		if (this.cam.isOrthoGraphic)
		{
			this.scaleK = num * this.cam.orthographicSize / this.collectionOrthoSize;
			this.scaleD = 0f;
		}
		else
		{
			float num2 = num * Mathf.Tan(0.017453292f * this.cam.fieldOfView * 0.5f) / this.collectionOrthoSize;
			this.scaleK = num2 * -this.cam.transform.position.z;
			this.scaleD = num2;
		}
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x0000EB82 File Offset: 0x0000CD82
	public static float CalculateScaleForPerspectiveCamera(float fov, float zdist)
	{
		return Mathf.Abs(Mathf.Tan(0.017453292f * fov * 0.5f) * zdist);
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x0000EB9D File Offset: 0x0000CD9D
	public bool CameraIsOrtho
	{
		get
		{
			return this.cam.isOrthoGraphic;
		}
	}

	// Token: 0x040012D7 RID: 4823
	private static tk2dPixelPerfectHelper _inst;

	// Token: 0x040012D8 RID: 4824
	[NonSerialized]
	public Camera cam;

	// Token: 0x040012D9 RID: 4825
	public int collectionTargetHeight = 640;

	// Token: 0x040012DA RID: 4826
	public float collectionOrthoSize = 1f;

	// Token: 0x040012DB RID: 4827
	public float targetResolutionHeight = 640f;

	// Token: 0x040012DC RID: 4828
	[NonSerialized]
	public float scaleD;

	// Token: 0x040012DD RID: 4829
	[NonSerialized]
	public float scaleK;
}
