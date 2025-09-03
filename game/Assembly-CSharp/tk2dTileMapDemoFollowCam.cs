using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class tk2dTileMapDemoFollowCam : MonoBehaviour
{
	// Token: 0x060012FF RID: 4863 RVA: 0x00010299 File Offset: 0x0000E499
	private void Awake()
	{
		this.cam = base.GetComponent<tk2dCamera>();
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x00084898 File Offset: 0x00082A98
	private void FixedUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 vector = Vector3.MoveTowards(position, this.target.position, this.followSpeed * Time.deltaTime);
		vector.z = position.z;
		base.transform.position = vector;
		if (this.target.rigidbody != null && this.cam != null)
		{
			float magnitude = this.target.rigidbody.velocity.magnitude;
			float num = Mathf.Clamp01((magnitude - this.minZoomSpeed) / (this.maxZoomSpeed - this.minZoomSpeed));
			float num2 = Mathf.Lerp(1f, this.maxZoomFactor, num);
			this.cam.ZoomFactor = Mathf.MoveTowards(this.cam.ZoomFactor, num2, 0.2f * Time.deltaTime);
		}
	}

	// Token: 0x040014C8 RID: 5320
	private tk2dCamera cam;

	// Token: 0x040014C9 RID: 5321
	public Transform target;

	// Token: 0x040014CA RID: 5322
	public float followSpeed = 1f;

	// Token: 0x040014CB RID: 5323
	public float minZoomSpeed = 20f;

	// Token: 0x040014CC RID: 5324
	public float maxZoomSpeed = 40f;

	// Token: 0x040014CD RID: 5325
	public float maxZoomFactor = 0.6f;
}
