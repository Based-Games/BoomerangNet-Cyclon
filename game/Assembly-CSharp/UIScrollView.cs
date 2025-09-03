using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
[AddComponentMenu("NGUI/Interaction/Scroll View")]
[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
public class UIScrollView : MonoBehaviour
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000210 RID: 528 RVA: 0x0000509D File Offset: 0x0000329D
	public UIPanel panel
	{
		get
		{
			return this.mPanel;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000211 RID: 529 RVA: 0x000050A5 File Offset: 0x000032A5
	public Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000212 RID: 530 RVA: 0x000050D6 File Offset: 0x000032D6
	public bool canMoveHorizontally
	{
		get
		{
			return this.movement == UIScrollView.Movement.Horizontal || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.x != 0f);
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000213 RID: 531 RVA: 0x0001AFC4 File Offset: 0x000191C4
	public bool canMoveVertically
	{
		get
		{
			return this.movement == UIScrollView.Movement.Vertical || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.y != 0f);
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000214 RID: 532 RVA: 0x0001B010 File Offset: 0x00019210
	public virtual bool shouldMoveHorizontally
	{
		get
		{
			float num = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.x * 2f;
			}
			return num > this.mPanel.width;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000215 RID: 533 RVA: 0x0001B070 File Offset: 0x00019270
	public virtual bool shouldMoveVertically
	{
		get
		{
			float num = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.y * 2f;
			}
			return num > this.mPanel.height;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000216 RID: 534 RVA: 0x0001B0D0 File Offset: 0x000192D0
	protected virtual bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 finalClipRegion = this.mPanel.finalClipRegion;
			Bounds bounds = this.bounds;
			float num = ((finalClipRegion.z != 0f) ? (finalClipRegion.z * 0.5f) : ((float)Screen.width));
			float num2 = ((finalClipRegion.w != 0f) ? (finalClipRegion.w * 0.5f) : ((float)Screen.height));
			if (this.canMoveHorizontally)
			{
				if (bounds.min.x < finalClipRegion.x - num)
				{
					return true;
				}
				if (bounds.max.x > finalClipRegion.x + num)
				{
					return true;
				}
			}
			if (this.canMoveVertically)
			{
				if (bounds.min.y < finalClipRegion.y - num2)
				{
					return true;
				}
				if (bounds.max.y > finalClipRegion.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000217 RID: 535 RVA: 0x00005116 File Offset: 0x00003316
	// (set) Token: 0x06000218 RID: 536 RVA: 0x0000511E File Offset: 0x0000331E
	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
			this.mShouldMove = true;
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0001B208 File Offset: 0x00019408
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
		if (this.mPanel.clipping == UIDrawCall.Clipping.None)
		{
			this.mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		}
		if (this.movement != UIScrollView.Movement.Custom && this.scale.sqrMagnitude > 0.001f)
		{
			if (this.scale.x == 1f && this.scale.y == 0f)
			{
				this.movement = UIScrollView.Movement.Horizontal;
			}
			else if (this.scale.x == 0f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Vertical;
			}
			else if (this.scale.x == 1f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Unrestricted;
			}
			else
			{
				this.movement = UIScrollView.Movement.Custom;
				this.customMovement.x = this.scale.x;
				this.customMovement.y = this.scale.y;
			}
			this.scale = Vector3.zero;
		}
		if (Application.isPlaying)
		{
			UIPanel uipanel = this.mPanel;
			uipanel.onChange = (UIPanel.OnChangeDelegate)Delegate.Combine(uipanel.onChange, new UIPanel.OnChangeDelegate(this.OnPanelChange));
		}
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0001B37C File Offset: 0x0001957C
	private void OnDestroy()
	{
		if (Application.isPlaying && this.mPanel != null)
		{
			UIPanel uipanel = this.mPanel;
			uipanel.onChange = (UIPanel.OnChangeDelegate)Delegate.Remove(uipanel.onChange, new UIPanel.OnChangeDelegate(this.OnPanelChange));
		}
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000512E File Offset: 0x0000332E
	private void OnPanelChange()
	{
		this.UpdateScrollbars(true);
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0001B3CC File Offset: 0x000195CC
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.UpdateScrollbars(true);
			if (this.horizontalScrollBar != null)
			{
				EventDelegate.Add(this.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnHorizontalBar));
				this.horizontalScrollBar.alpha = ((this.showScrollBars != UIScrollView.ShowCondition.Always && !this.shouldMoveHorizontally) ? 0f : 1f);
			}
			if (this.verticalScrollBar != null)
			{
				EventDelegate.Add(this.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnVerticalBar));
				this.verticalScrollBar.alpha = ((this.showScrollBars != UIScrollView.ShowCondition.Always && !this.shouldMoveVertically) ? 0f : 1f);
			}
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00005137 File Offset: 0x00003337
	public bool RestrictWithinBounds(bool instant)
	{
		return this.RestrictWithinBounds(instant, true, true);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0001B4A4 File Offset: 0x000196A4
	public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
	{
		Bounds bounds = this.bounds;
		Vector3 vector = this.mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (!horizontal)
		{
			vector.x = 0f;
		}
		if (!vertical)
		{
			vector.y = 0f;
		}
		if (vector.magnitude > 1f)
		{
			if (!instant && this.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
			{
				Vector3 vector2 = this.mTrans.localPosition + vector;
				vector2.x = Mathf.Round(vector2.x);
				vector2.y = Mathf.Round(vector2.y);
				SpringPanel.Begin(this.mPanel.gameObject, vector2, 13f);
			}
			else
			{
				this.MoveRelative(vector);
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0001B598 File Offset: 0x00019798
	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0001B5C0 File Offset: 0x000197C0
	public virtual void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this.mShouldMove = this.shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (this.horizontalScrollBar != null && vector2.x > vector.x)
			{
				Vector4 finalClipRegion = this.mPanel.finalClipRegion;
				float num = finalClipRegion.z * 0.5f;
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num -= this.mPanel.clipSoftness.x;
				}
				float num2 = finalClipRegion.x - num - bounds.min.x;
				float num3 = bounds.max.x - num - finalClipRegion.x;
				float num4 = vector2.x - vector.x;
				num2 = Mathf.Clamp01(num2 / num4);
				num3 = Mathf.Clamp01(num3 / num4);
				float num5 = num2 + num3;
				this.mIgnoreCallbacks = true;
				this.horizontalScrollBar.barSize = 1f - num5;
				this.horizontalScrollBar.value = ((num5 <= 0.001f) ? 0f : (num2 / num5));
				this.mIgnoreCallbacks = false;
			}
			if (this.verticalScrollBar != null && vector2.y > vector.y)
			{
				Vector4 finalClipRegion2 = this.mPanel.finalClipRegion;
				float num6 = finalClipRegion2.w * 0.5f;
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num6 -= this.mPanel.clipSoftness.y;
				}
				float num7 = finalClipRegion2.y - num6 - vector.y;
				float num8 = vector2.y - num6 - finalClipRegion2.y;
				float num9 = vector2.y - vector.y;
				num7 = Mathf.Clamp01(num7 / num9);
				num8 = Mathf.Clamp01(num8 / num9);
				float num10 = num7 + num8;
				this.mIgnoreCallbacks = true;
				this.verticalScrollBar.barSize = 1f - num10;
				this.verticalScrollBar.value = ((num10 <= 0.001f) ? 0f : (1f - num7 / num10));
				this.mIgnoreCallbacks = false;
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
		}
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0001B878 File Offset: 0x00019A78
	public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		this.DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
		{
			return;
		}
		Vector4 finalClipRegion = this.mPanel.finalClipRegion;
		finalClipRegion.x = Mathf.Round(finalClipRegion.x);
		finalClipRegion.y = Mathf.Round(finalClipRegion.y);
		finalClipRegion.z = Mathf.Round(finalClipRegion.z);
		finalClipRegion.w = Mathf.Round(finalClipRegion.w);
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= this.mPanel.clipSoftness.x;
			num4 += this.mPanel.clipSoftness.x;
			num5 -= this.mPanel.clipSoftness.y;
			num6 += this.mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		num7 = Mathf.Round(num7);
		num8 = Mathf.Round(num8);
		if (!updateScrollbars)
		{
			Vector3 localPosition = this.mTrans.localPosition;
			if (this.canMoveHorizontally)
			{
				localPosition.x += finalClipRegion.x - num7;
			}
			if (this.canMoveVertically)
			{
				localPosition.y += finalClipRegion.y - num8;
			}
			this.mTrans.localPosition = localPosition;
		}
		if (this.canMoveHorizontally)
		{
			finalClipRegion.x = num7;
		}
		if (this.canMoveVertically)
		{
			finalClipRegion.y = num8;
		}
		Vector4 baseClipRegion = this.mPanel.baseClipRegion;
		this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
		if (updateScrollbars)
		{
			this.UpdateScrollbars(false);
		}
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0001BB18 File Offset: 0x00019D18
	[ContextMenu("Reset Clipping Position")]
	public void ResetPosition()
	{
		this.mCalculatedBounds = false;
		this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, false);
		this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0001BB68 File Offset: 0x00019D68
	private void OnHorizontalBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			float num = ((!(this.horizontalScrollBar != null)) ? 0f : this.horizontalScrollBar.value);
			float num2 = ((!(this.verticalScrollBar != null)) ? 0f : this.verticalScrollBar.value);
			this.SetDragAmount(num, num2, false);
		}
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0001BB68 File Offset: 0x00019D68
	private void OnVerticalBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			float num = ((!(this.horizontalScrollBar != null)) ? 0f : this.horizontalScrollBar.value);
			float num2 = ((!(this.verticalScrollBar != null)) ? 0f : this.verticalScrollBar.value);
			this.SetDragAmount(num, num2, false);
		}
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0001BBD8 File Offset: 0x00019DD8
	public virtual void MoveRelative(Vector3 relative)
	{
		this.mTrans.localPosition += relative;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= relative.x;
		clipOffset.y -= relative.y;
		this.mPanel.clipOffset = clipOffset;
		this.UpdateScrollbars(false);
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0001BC48 File Offset: 0x00019E48
	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 vector = this.mTrans.InverseTransformPoint(absolute);
		Vector3 vector2 = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(vector - vector2);
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0001BC80 File Offset: 0x00019E80
	public void Press(bool pressed)
	{
		if (this.smoothDragStart && pressed)
		{
			this.mDragStarted = false;
			this.mDragStartOffset = Vector2.zero;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (!pressed && this.mDragID == UICamera.currentTouchID)
			{
				this.mDragID = -10;
			}
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastHit.point;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
				Vector2 clipOffset = this.mPanel.clipOffset;
				clipOffset.x = Mathf.Round(clipOffset.x);
				clipOffset.y = Mathf.Round(clipOffset.y);
				this.mPanel.clipOffset = clipOffset;
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
			}
			else
			{
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
				}
				if ((!this.smoothDragStart || this.mDragStarted) && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
			}
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0001BE50 File Offset: 0x0001A050
	public void Drag()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mShouldMove)
		{
			if (this.mDragID == -10)
			{
				this.mDragID = UICamera.currentTouchID;
			}
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			if (this.smoothDragStart && !this.mDragStarted)
			{
				this.mDragStarted = true;
				this.mDragStartOffset = UICamera.currentTouch.totalDelta;
			}
			Ray ray = ((!this.smoothDragStart) ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - this.mDragStartOffset));
			float num = 0f;
			if (this.mPlane.Raycast(ray, out num))
			{
				Vector3 point = ray.GetPoint(num);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.mTrans.InverseTransformDirection(vector);
					if (this.movement == UIScrollView.Movement.Horizontal)
					{
						vector.y = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Vertical)
					{
						vector.x = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Unrestricted)
					{
						vector.z = 0f;
					}
					else
					{
						vector.Scale(this.customMovement);
					}
					vector = this.mTrans.TransformDirection(vector);
				}
				this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				if (!this.iOSDragEmulation)
				{
					this.MoveAbsolute(vector);
				}
				else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 1f)
				{
					this.MoveAbsolute(vector * 0.5f);
					this.mMomentum *= 0.5f;
				}
				else
				{
					this.MoveAbsolute(vector);
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(true, this.canMoveHorizontally, this.canMoveVertically);
				}
			}
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0001C114 File Offset: 0x0001A314
	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.scrollWheelFactor != 0f)
		{
			this.DisableSpring();
			this.mShouldMove = this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0001C194 File Offset: 0x0001A394
	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		if (this.showScrollBars != UIScrollView.ShowCondition.Always)
		{
			bool flag = false;
			bool flag2 = false;
			if (this.showScrollBars != UIScrollView.ShowCondition.WhenDragging || this.mDragID != -10 || this.mMomentum.magnitude > 0.01f)
			{
				flag = this.shouldMoveVertically;
				flag2 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float num = this.verticalScrollBar.alpha;
				num += ((!flag) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num = Mathf.Clamp01(num);
				if (this.verticalScrollBar.alpha != num)
				{
					this.verticalScrollBar.alpha = num;
				}
			}
			if (this.horizontalScrollBar)
			{
				float num2 = this.horizontalScrollBar.alpha;
				num2 += ((!flag2) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num2 = Mathf.Clamp01(num2);
				if (this.horizontalScrollBar.alpha != num2)
				{
					this.horizontalScrollBar.alpha = num2;
				}
			}
		}
		if (this.mShouldMove && !this.mPressed)
		{
			if (this.movement == UIScrollView.Movement.Horizontal || this.movement == UIScrollView.Movement.Unrestricted)
			{
				this.mMomentum.x = this.mMomentum.x - this.mScroll * 0.05f;
			}
			else if (this.movement == UIScrollView.Movement.Vertical)
			{
				this.mMomentum.y = this.mMomentum.y - this.mScroll * 0.05f;
			}
			else
			{
				this.mMomentum -= this.customMovement * (this.mScroll * 0.05f);
			}
			if (this.mMomentum.magnitude > 0.0001f)
			{
				this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
				Vector3 vector = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.MoveAbsolute(vector);
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
				}
				if (this.mMomentum.magnitude < 0.0001f && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
				return;
			}
			this.mScroll = 0f;
			this.mMomentum = Vector3.zero;
		}
		else
		{
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	// Token: 0x040001FB RID: 507
	public UIScrollView.Movement movement;

	// Token: 0x040001FC RID: 508
	public UIScrollView.DragEffect dragEffect = UIScrollView.DragEffect.MomentumAndSpring;

	// Token: 0x040001FD RID: 509
	public bool restrictWithinPanel = true;

	// Token: 0x040001FE RID: 510
	public bool disableDragIfFits;

	// Token: 0x040001FF RID: 511
	public bool smoothDragStart = true;

	// Token: 0x04000200 RID: 512
	public bool iOSDragEmulation = true;

	// Token: 0x04000201 RID: 513
	public float scrollWheelFactor = 0.25f;

	// Token: 0x04000202 RID: 514
	public float momentumAmount = 35f;

	// Token: 0x04000203 RID: 515
	public UIScrollBar horizontalScrollBar;

	// Token: 0x04000204 RID: 516
	public UIScrollBar verticalScrollBar;

	// Token: 0x04000205 RID: 517
	public UIScrollView.ShowCondition showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;

	// Token: 0x04000206 RID: 518
	public Vector2 customMovement = new Vector2(1f, 0f);

	// Token: 0x04000207 RID: 519
	public Vector2 relativePositionOnReset = Vector2.zero;

	// Token: 0x04000208 RID: 520
	public UIScrollView.OnDragFinished onDragFinished;

	// Token: 0x04000209 RID: 521
	[HideInInspector]
	[SerializeField]
	private Vector3 scale = new Vector3(1f, 0f, 0f);

	// Token: 0x0400020A RID: 522
	private Transform mTrans;

	// Token: 0x0400020B RID: 523
	private UIPanel mPanel;

	// Token: 0x0400020C RID: 524
	private Plane mPlane;

	// Token: 0x0400020D RID: 525
	private Vector3 mLastPos;

	// Token: 0x0400020E RID: 526
	private bool mPressed;

	// Token: 0x0400020F RID: 527
	private Vector3 mMomentum = Vector3.zero;

	// Token: 0x04000210 RID: 528
	private float mScroll;

	// Token: 0x04000211 RID: 529
	private Bounds mBounds;

	// Token: 0x04000212 RID: 530
	private bool mCalculatedBounds;

	// Token: 0x04000213 RID: 531
	private bool mShouldMove;

	// Token: 0x04000214 RID: 532
	private bool mIgnoreCallbacks;

	// Token: 0x04000215 RID: 533
	private int mDragID = -10;

	// Token: 0x04000216 RID: 534
	private Vector2 mDragStartOffset = Vector2.zero;

	// Token: 0x04000217 RID: 535
	private bool mDragStarted;

	// Token: 0x02000054 RID: 84
	public enum Movement
	{
		// Token: 0x04000219 RID: 537
		Horizontal,
		// Token: 0x0400021A RID: 538
		Vertical,
		// Token: 0x0400021B RID: 539
		Unrestricted,
		// Token: 0x0400021C RID: 540
		Custom
	}

	// Token: 0x02000055 RID: 85
	public enum DragEffect
	{
		// Token: 0x0400021E RID: 542
		None,
		// Token: 0x0400021F RID: 543
		Momentum,
		// Token: 0x04000220 RID: 544
		MomentumAndSpring
	}

	// Token: 0x02000056 RID: 86
	public enum ShowCondition
	{
		// Token: 0x04000222 RID: 546
		Always,
		// Token: 0x04000223 RID: 547
		OnlyIfNeeded,
		// Token: 0x04000224 RID: 548
		WhenDragging
	}

	// Token: 0x02000057 RID: 87
	// (Invoke) Token: 0x0600022C RID: 556
	public delegate void OnDragFinished();
}
