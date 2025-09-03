using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/Orthographic Camera")]
[ExecuteInEditMode]
public class UIOrthoCamera : MonoBehaviour
{
	// Token: 0x060005D8 RID: 1496 RVA: 0x00007D2A File Offset: 0x00005F2A
	private void Start()
	{
		this.mCam = base.camera;
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002DB78 File Offset: 0x0002BD78
	private void Update()
	{
		float num = this.mCam.rect.yMin * (float)Screen.height;
		float num2 = this.mCam.rect.yMax * (float)Screen.height;
		float num3 = (num2 - num) * 0.5f * this.mTrans.lossyScale.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, num3))
		{
			this.mCam.orthographicSize = num3;
		}
	}

	// Token: 0x04000482 RID: 1154
	private Camera mCam;

	// Token: 0x04000483 RID: 1155
	private Transform mTrans;
}
