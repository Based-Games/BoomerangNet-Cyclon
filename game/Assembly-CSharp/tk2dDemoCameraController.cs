using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002CF RID: 719
[AddComponentMenu("2D Toolkit/Demo/tk2dDemoCameraController")]
public class tk2dDemoCameraController : MonoBehaviour
{
	// Token: 0x06001518 RID: 5400 RVA: 0x00011FC5 File Offset: 0x000101C5
	private void Start()
	{
		this.listTopPos = this.listItems.localPosition;
		this.listBottomPos = this.listTopPos - this.endOfListItems.localPosition;
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x0008CCE4 File Offset: 0x0008AEE4
	private IEnumerator MoveListTo(Vector3 from, Vector3 to)
	{
		this.transitioning = true;
		float time = 0.5f;
		for (float t = 0f; t < time; t += Time.deltaTime)
		{
			float nt = Mathf.Clamp01(t / time);
			nt = Mathf.SmoothStep(0f, 1f, nt);
			this.listItems.localPosition = Vector3.Lerp(from, to, nt);
			yield return 0;
		}
		this.listItems.localPosition = to;
		this.transitioning = false;
		yield break;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x0008CD1C File Offset: 0x0008AF1C
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !this.transitioning && !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
		{
			if (this.listAtTop)
			{
				base.StartCoroutine(this.MoveListTo(this.listTopPos, this.listBottomPos));
			}
			else
			{
				base.StartCoroutine(this.MoveListTo(this.listBottomPos, this.listTopPos));
			}
			this.listAtTop = !this.listAtTop;
		}
		foreach (Transform transform in this.rotatingObjects)
		{
			transform.Rotate(UnityEngine.Random.insideUnitSphere, Time.deltaTime * 360f);
		}
	}

	// Token: 0x0400167C RID: 5756
	public Transform listItems;

	// Token: 0x0400167D RID: 5757
	public Transform endOfListItems;

	// Token: 0x0400167E RID: 5758
	private Vector3 listTopPos = Vector3.zero;

	// Token: 0x0400167F RID: 5759
	private Vector3 listBottomPos = Vector3.zero;

	// Token: 0x04001680 RID: 5760
	private bool listAtTop = true;

	// Token: 0x04001681 RID: 5761
	private bool transitioning;

	// Token: 0x04001682 RID: 5762
	public Transform[] rotatingObjects = new Transform[0];
}
