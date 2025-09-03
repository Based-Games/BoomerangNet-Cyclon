using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x0600013E RID: 318 RVA: 0x000044C5 File Offset: 0x000026C5
	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000044CD File Offset: 0x000026CD
	private void OnEnable()
	{
		this.Recenter();
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000044D5 File Offset: 0x000026D5
	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x000044E8 File Offset: 0x000026E8
	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00016B6C File Offset: 0x00014D6C
	public void Recenter()
	{
		if (this.mDrag == null)
		{
			this.mDrag = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mDrag == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.enabled = false;
				return;
			}
			this.mDrag.onDragFinished = new UIScrollView.OnDragFinished(this.OnDragFinished);
			if (this.mDrag.horizontalScrollBar != null)
			{
				this.mDrag.horizontalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
			if (this.mDrag.verticalScrollBar != null)
			{
				this.mDrag.verticalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
		}
		if (this.mDrag.panel == null)
		{
			return;
		}
		Vector3[] worldCorners = this.mDrag.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = vector - this.mDrag.currentMomentum * (this.mDrag.momentumAmount * 0.1f);
		this.mDrag.currentMomentum = Vector3.zero;
		float num = float.MaxValue;
		Transform transform = null;
		Transform transform2 = base.transform;
		int num2 = 0;
		int i = 0;
		int childCount = transform2.childCount;
		while (i < childCount)
		{
			Transform child = transform2.GetChild(i);
			float num3 = Vector3.SqrMagnitude(child.position - vector2);
			if (num3 < num)
			{
				num = num3;
				transform = child;
				num2 = i;
			}
			i++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == transform2.GetChild(num2))
		{
			Vector2 totalDelta = UICamera.currentTouch.totalDelta;
			if (totalDelta.x > this.nextPageThreshold)
			{
				if (num2 > 0)
				{
					transform = transform2.GetChild(num2 - 1);
				}
			}
			else if (totalDelta.x < -this.nextPageThreshold && num2 < transform2.childCount - 1)
			{
				transform = transform2.GetChild(num2 + 1);
			}
		}
		this.CenterOn(transform, vector);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00016E10 File Offset: 0x00015010
	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mDrag != null && this.mDrag.panel != null)
		{
			Transform cachedTransform = this.mDrag.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 vector = cachedTransform.InverseTransformPoint(target.position);
			Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 vector3 = vector - vector2;
			if (!this.mDrag.canMoveHorizontally)
			{
				vector3.x = 0f;
			}
			if (!this.mDrag.canMoveVertically)
			{
				vector3.y = 0f;
			}
			vector3.z = 0f;
			SpringPanel.Begin(this.mDrag.panel.cachedGameObject, cachedTransform.localPosition - vector3, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00016F0C File Offset: 0x0001510C
	public void CenterOn(Transform target)
	{
		if (this.mDrag != null && this.mDrag.panel != null)
		{
			Vector3[] worldCorners = this.mDrag.panel.worldCorners;
			Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, vector);
		}
	}

	// Token: 0x04000119 RID: 281
	public float springStrength = 8f;

	// Token: 0x0400011A RID: 282
	public float nextPageThreshold;

	// Token: 0x0400011B RID: 283
	public SpringPanel.OnFinished onFinished;

	// Token: 0x0400011C RID: 284
	private UIScrollView mDrag;

	// Token: 0x0400011D RID: 285
	private GameObject mCenteredObject;
}
