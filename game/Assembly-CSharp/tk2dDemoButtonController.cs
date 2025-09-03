using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
[AddComponentMenu("2D Toolkit/Demo/tk2dDemoButtonController")]
public class tk2dDemoButtonController : MonoBehaviour
{
	// Token: 0x06001513 RID: 5395 RVA: 0x00011F4F File Offset: 0x0001014F
	private void Update()
	{
		base.transform.Rotate(Vector3.up, this.spinSpeed * Time.deltaTime);
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x00011F6D File Offset: 0x0001016D
	private void SpinLeft()
	{
		this.spinSpeed = 4f;
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x00011F7A File Offset: 0x0001017A
	private void SpinRight()
	{
		this.spinSpeed = -4f;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x00011F87 File Offset: 0x00010187
	private void StopSpinning()
	{
		this.spinSpeed = 0f;
	}

	// Token: 0x0400167B RID: 5755
	private float spinSpeed;
}
