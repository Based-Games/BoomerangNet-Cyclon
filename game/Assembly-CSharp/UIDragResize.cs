using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	// Token: 0x0600016A RID: 362 RVA: 0x00017AC8 File Offset: 0x00015CC8
	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float num;
			if (this.mPlane.Raycast(currentRay, out num))
			{
				this.mRayPos = currentRay.GetPoint(num);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00017B88 File Offset: 0x00015D88
	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float num;
			if (this.mPlane.Raycast(currentRay, out num))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 vector = currentRay.GetPoint(num) - this.mRayPos;
				cachedTransform.position += vector;
				Vector3 vector2 = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
				cachedTransform.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector2.x, vector2.y, this.minWidth, this.minHeight);
			}
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00004766 File Offset: 0x00002966
	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	// Token: 0x04000149 RID: 329
	public UIWidget target;

	// Token: 0x0400014A RID: 330
	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	// Token: 0x0400014B RID: 331
	public int minWidth = 100;

	// Token: 0x0400014C RID: 332
	public int minHeight = 100;

	// Token: 0x0400014D RID: 333
	private Plane mPlane;

	// Token: 0x0400014E RID: 334
	private Vector3 mRayPos;

	// Token: 0x0400014F RID: 335
	private Vector3 mLocalPos;

	// Token: 0x04000150 RID: 336
	private int mWidth;

	// Token: 0x04000151 RID: 337
	private int mHeight;

	// Token: 0x04000152 RID: 338
	private bool mDragging;
}
