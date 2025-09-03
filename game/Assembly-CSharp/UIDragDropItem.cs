using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	// Token: 0x06000150 RID: 336 RVA: 0x00004604 File Offset: 0x00002804
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mCollider = base.collider;
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000462A File Offset: 0x0000282A
	private void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			this.mPressTime = RealTime.time;
		}
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0001701C File Offset: 0x0001521C
	private void OnDragStart()
	{
		if (!base.enabled || this.mTouchID != -2147483648)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressTime + 1f > RealTime.time)
			{
				return;
			}
		}
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = false;
		}
		if (this.mCollider != null)
		{
			this.mCollider.enabled = false;
		}
		this.mTouchID = UICamera.currentTouchID;
		this.mParent = this.mTrans.parent;
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = this.mTrans.localPosition;
		localPosition.z = 0f;
		this.mTrans.localPosition = localPosition;
		NGUITools.MarkParentAsChanged(base.gameObject);
		this.OnDragDropStart();
		this.m_DragState = true;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000463D File Offset: 0x0000283D
	private void OnDrag(Vector2 delta)
	{
		if (!base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x000171CC File Offset: 0x000153CC
	private void OnDragEnd()
	{
		if (!base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.mTouchID = int.MinValue;
		if (this.mCollider != null)
		{
			this.mCollider.enabled = true;
		}
		this.OnDragDropRelease(UICamera.hoveredObject);
		this.mParent = this.mTrans.parent;
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = true;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		this.OnDragDropEnd();
		this.m_DragState = false;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00004677 File Offset: 0x00002877
	protected virtual void OnDragDropStart()
	{
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x000046B3 File Offset: 0x000028B3
	protected virtual void OnDragDropMove(Vector3 delta)
	{
		this.mTrans.localPosition += delta;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00017290 File Offset: 0x00015490
	protected virtual void OnDragDropRelease(GameObject surface)
	{
		UIDragDropContainer uidragDropContainer = ((!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface));
		if (uidragDropContainer != null)
		{
			this.mTrans.parent = ((!(uidragDropContainer.reparentTarget != null)) ? uidragDropContainer.transform : uidragDropContainer.reparentTarget);
			Vector3 localPosition = this.mTrans.localPosition;
			localPosition.z = 0f;
			this.mTrans.localPosition = localPosition;
		}
		else
		{
			this.mTrans.parent = this.mParent;
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00004677 File Offset: 0x00002877
	protected virtual void OnDragDropEnd()
	{
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	// Token: 0x04000122 RID: 290
	public UIDragDropItem.Restriction restriction;

	// Token: 0x04000123 RID: 291
	public bool m_DragState;

	// Token: 0x04000124 RID: 292
	protected Transform mTrans;

	// Token: 0x04000125 RID: 293
	public Transform mParent;

	// Token: 0x04000126 RID: 294
	protected Collider mCollider;

	// Token: 0x04000127 RID: 295
	protected UIRoot mRoot;

	// Token: 0x04000128 RID: 296
	protected UIGrid mGrid;

	// Token: 0x04000129 RID: 297
	protected UITable mTable;

	// Token: 0x0400012A RID: 298
	protected int mTouchID = int.MinValue;

	// Token: 0x0400012B RID: 299
	protected float mPressTime;

	// Token: 0x0400012C RID: 300
	protected UIDragScrollView mDragScrollView;

	// Token: 0x02000037 RID: 55
	public enum Restriction
	{
		// Token: 0x0400012E RID: 302
		None,
		// Token: 0x0400012F RID: 303
		Horizontal,
		// Token: 0x04000130 RID: 304
		Vertical,
		// Token: 0x04000131 RID: 305
		PressAndHold
	}
}
