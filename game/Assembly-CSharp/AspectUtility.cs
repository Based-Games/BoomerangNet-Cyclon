using System;
using UnityEngine;

// Token: 0x02000218 RID: 536
public class AspectUtility : MonoBehaviour
{
	// Token: 0x06000F76 RID: 3958 RVA: 0x000702E0 File Offset: 0x0006E4E0
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
		if (this.cam == null)
		{
			Logger.Error("AspectUtility", "requires a Camera component on the same GameObject.", new object[0]);
			base.enabled = false;
			return;
		}
		Logger.Log("AspectUtility", "Hello", new object[0]);
		this.SetCamera();
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x00070340 File Offset: 0x0006E540
	private void SetCamera()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = this.targetAspectWidth / this.targetAspectHeight;
		if (Mathf.Approximately(num2, 0f) || float.IsNaN(num2))
		{
			Logger.Warn("AspectUtility", "Invalid target aspect ratio. Using default 16:9.", new object[0]);
			num2 = 1.7777778f;
		}
		float num3 = num / num2;
		Rect rect = new Rect(0f, 0f, 1f, 1f);
		if (num3 > 1f)
		{
			rect.width = 1f / num3;
			rect.x = (1f - rect.width) / 2f;
		}
		else
		{
			rect.height = num3;
			rect.y = (1f - rect.height) / 2f;
		}
		this.cam.rect = rect;
		if (this.bBackCamera && this.backgroundCam == null)
		{
			this.backgroundCam = new GameObject("BackgroundCam", new Type[] { typeof(Camera) }).GetComponent<Camera>();
			this.backgroundCam.depth = float.MinValue;
			this.backgroundCam.clearFlags = CameraClearFlags.Color;
			this.backgroundCam.backgroundColor = Color.black;
			this.backgroundCam.cullingMask = 0;
			this.backgroundCam.rect = new Rect(0f, 0f, 1f, 1f);
		}
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x000704B8 File Offset: 0x0006E6B8
	private void Update()
	{
		if (Screen.width != this.lastScreenWidth || Screen.height != this.lastScreenHeight)
		{
			this.lastScreenWidth = Screen.width;
			this.lastScreenHeight = Screen.height;
			this.SetCamera();
			Logger.Log("AspectUtility", "Camera Updated " + this.lastScreenWidth.ToString() + "x" + this.lastScreenHeight.ToString(), new object[0]);
		}
	}

	// Token: 0x04001145 RID: 4421
	[SerializeField]
	public bool bBackCamera = true;

	// Token: 0x04001146 RID: 4422
	[SerializeField]
	[Tooltip("Target width for the desired aspect ratio (e.g., 16 for 16:9)")]
	private float targetAspectWidth = 16f;

	// Token: 0x04001147 RID: 4423
	[SerializeField]
	[Tooltip("Target height for the desired aspect ratio (e.g., 10 for 16:10)")]
	private float targetAspectHeight = 10f;

	// Token: 0x04001148 RID: 4424
	private Camera cam;

	// Token: 0x04001149 RID: 4425
	private Camera backgroundCam;

	// Token: 0x0400114A RID: 4426
	private int lastScreenWidth;

	// Token: 0x0400114B RID: 4427
	private int lastScreenHeight;
}
