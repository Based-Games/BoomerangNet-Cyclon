using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
[AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : UISlider
{
	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000207 RID: 519 RVA: 0x00005084 File Offset: 0x00003284
	// (set) Token: 0x06000208 RID: 520 RVA: 0x0000508C File Offset: 0x0000328C
	[Obsolete("Use 'value' instead")]
	public float scrollValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000209 RID: 521 RVA: 0x00005095 File Offset: 0x00003295
	// (set) Token: 0x0600020A RID: 522 RVA: 0x0001AB00 File Offset: 0x00018D00
	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mSize != num)
			{
				this.mSize = num;
				this.mIsDirty = true;
				if (this.onChange != null)
				{
					UIProgressBar.current = this;
					EventDelegate.Execute(this.onChange);
					UIProgressBar.current = null;
				}
				if (!Application.isPlaying)
				{
					this.ForceUpdate();
				}
			}
		}
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0001AB60 File Offset: 0x00018D60
	protected override void Upgrade()
	{
		if (this.mDir != UIScrollBar.Direction.Upgraded)
		{
			this.mValue = this.mScroll;
			if (this.mDir == UIScrollBar.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop);
			}
			this.mDir = UIScrollBar.Direction.Upgraded;
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0001ABCC File Offset: 0x00018DCC
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mFG != null && this.mFG.collider != null && this.mFG.gameObject != base.gameObject)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.mFG.gameObject);
			UIEventListener uieventListener2 = uieventListener;
			uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(base.OnPressForeground));
			UIEventListener uieventListener3 = uieventListener;
			uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(base.OnDragForeground));
			this.mFG.autoResizeBoxCollider = true;
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0001AC84 File Offset: 0x00018E84
	protected override float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return base.LocalToValue(localPos);
		}
		float num = Mathf.Clamp01(this.mSize) * 0.5f;
		float num2 = num;
		float num3 = 1f - num;
		Vector3[] localCorners = this.mFG.localCorners;
		if (base.isHorizontal)
		{
			num2 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num2);
			num3 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num3);
			return (!base.isInverted) ? ((localPos.x - num2) / (num3 - num2)) : ((num3 - localPos.x) / (num3 - num2));
		}
		num2 = Mathf.Lerp(localCorners[0].y, localCorners[1].y, num2);
		num3 = Mathf.Lerp(localCorners[3].y, localCorners[2].y, num3);
		return (!base.isInverted) ? ((localPos.y - num2) / (num3 - num2)) : ((num3 - localPos.y) / (num3 - num2));
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0001ADB0 File Offset: 0x00018FB0
	public override void ForceUpdate()
	{
		if (this.mFG != null)
		{
			this.mIsDirty = false;
			float num = Mathf.Clamp01(this.mSize) * 0.5f;
			float num2 = Mathf.Lerp(num, 1f - num, base.value);
			float num3 = num2 - num;
			float num4 = num2 + num;
			if (base.isHorizontal)
			{
				this.mFG.drawRegion = ((!base.isInverted) ? new Vector4(num3, 0f, num4, 1f) : new Vector4(1f - num4, 0f, 1f - num3, 1f));
			}
			else
			{
				this.mFG.drawRegion = ((!base.isInverted) ? new Vector4(0f, num3, 1f, num4) : new Vector4(0f, 1f - num4, 1f, 1f - num3));
			}
			if (this.thumb != null)
			{
				Vector4 drawingDimensions = this.mFG.drawingDimensions;
				Vector3 vector = new Vector3(Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, 0.5f), Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, 0.5f));
				base.SetThumbPosition(this.mFG.cachedTransform.TransformPoint(vector));
			}
		}
		else
		{
			base.ForceUpdate();
		}
	}

	// Token: 0x040001F4 RID: 500
	[SerializeField]
	[HideInInspector]
	protected float mSize = 1f;

	// Token: 0x040001F5 RID: 501
	[HideInInspector]
	[SerializeField]
	private float mScroll;

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	[HideInInspector]
	private UIScrollBar.Direction mDir = UIScrollBar.Direction.Upgraded;

	// Token: 0x02000052 RID: 82
	private enum Direction
	{
		// Token: 0x040001F8 RID: 504
		Horizontal,
		// Token: 0x040001F9 RID: 505
		Vertical,
		// Token: 0x040001FA RID: 506
		Upgraded
	}
}
