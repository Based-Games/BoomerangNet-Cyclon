using System;
using UnityEngine;

// Token: 0x020002AE RID: 686
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUICamera")]
public class tk2dUICamera : MonoBehaviour
{
	// Token: 0x0600140A RID: 5130 RVA: 0x000114B0 File Offset: 0x0000F6B0
	public void AssignRaycastLayerMask(LayerMask mask)
	{
		this.raycastLayerMask = mask;
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x0600140B RID: 5131 RVA: 0x000114B9 File Offset: 0x0000F6B9
	public LayerMask FilteredMask
	{
		get
		{
			return this.raycastLayerMask & base.camera.cullingMask;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x0600140C RID: 5132 RVA: 0x000114D7 File Offset: 0x0000F6D7
	public Camera HostCamera
	{
		get
		{
			return base.camera;
		}
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000114DF File Offset: 0x0000F6DF
	private void OnEnable()
	{
		if (base.camera == null)
		{
			Debug.LogError("tk2dUICamera should only be attached to a camera.");
			base.enabled = false;
			return;
		}
		tk2dUIManager.RegisterCamera(this);
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x0001150A File Offset: 0x0000F70A
	private void OnDisable()
	{
		tk2dUIManager.UnregisterCamera(this);
	}

	// Token: 0x0400157F RID: 5503
	[SerializeField]
	private LayerMask raycastLayerMask = -1;
}
