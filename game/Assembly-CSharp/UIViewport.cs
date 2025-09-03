using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Viewport Camera")]
public class UIViewport : MonoBehaviour
{
	// Token: 0x06000688 RID: 1672 RVA: 0x0000842F File Offset: 0x0000662F
	private void Start()
	{
		this.mCam = base.camera;
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00033870 File Offset: 0x00031A70
	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
			Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
			Rect rect = new Rect(vector.x / (float)Screen.width, vector2.y / (float)Screen.height, (vector2.x - vector.x) / (float)Screen.width, (vector.y - vector2.y) / (float)Screen.height);
			float num = this.fullSize * rect.height;
			if (rect != this.mCam.rect)
			{
				this.mCam.rect = rect;
			}
			if (this.mCam.orthographicSize != num)
			{
				this.mCam.orthographicSize = num;
			}
		}
	}

	// Token: 0x0400051C RID: 1308
	public Camera sourceCamera;

	// Token: 0x0400051D RID: 1309
	public Transform topLeft;

	// Token: 0x0400051E RID: 1310
	public Transform bottomRight;

	// Token: 0x0400051F RID: 1311
	public float fullSize = 1f;

	// Token: 0x04000520 RID: 1312
	private Camera mCam;
}
