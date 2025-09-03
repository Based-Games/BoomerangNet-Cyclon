using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
public class UISlider : UIProgressBar
{
	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000230 RID: 560 RVA: 0x00005084 File Offset: 0x00003284
	// (set) Token: 0x06000231 RID: 561 RVA: 0x0000508C File Offset: 0x0000328C
	[Obsolete("Use 'value' instead")]
	public float sliderValue
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

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000232 RID: 562 RVA: 0x0000515C File Offset: 0x0000335C
	// (set) Token: 0x06000233 RID: 563 RVA: 0x00003648 File Offset: 0x00001848
	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0001C44C File Offset: 0x0001A64C
	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0001C4DC File Offset: 0x0001A6DC
	protected override void OnStart()
	{
		GameObject gameObject = ((!(this.mBG != null) || !(this.mBG.collider != null)) ? base.gameObject : this.mBG.gameObject);
		UIEventListener uieventListener = UIEventListener.Get(gameObject);
		UIEventListener uieventListener2 = uieventListener;
		uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener uieventListener3 = uieventListener;
		uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && this.thumb.collider != null && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uieventListener4 = UIEventListener.Get(this.thumb.gameObject);
			UIEventListener uieventListener5 = uieventListener4;
			uieventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener uieventListener6 = uieventListener4;
			uieventListener6.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener6.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0001C61C File Offset: 0x0001A81C
	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00005164 File Offset: 0x00003364
	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0001C670 File Offset: 0x0001A870
	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (isPressed)
		{
			this.mOffset = ((!(this.mFG == null)) ? (base.value - base.ScreenToValue(UICamera.lastTouchPosition)) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000518E File Offset: 0x0000338E
	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastTouchPosition);
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0001C6E0 File Offset: 0x0001A8E0
	protected void OnKey(KeyCode key)
	{
		if (base.enabled)
		{
			float num = (((float)this.numberOfSteps <= 1f) ? 0.125f : (1f / (float)(this.numberOfSteps - 1)));
			if (base.fillDirection == UIProgressBar.FillDirection.LeftToRight || base.fillDirection == UIProgressBar.FillDirection.RightToLeft)
			{
				if (key == KeyCode.LeftArrow)
				{
					base.value = this.mValue - num;
				}
				else if (key == KeyCode.RightArrow)
				{
					base.value = this.mValue + num;
				}
			}
			else if (key == KeyCode.DownArrow)
			{
				base.value = this.mValue - num;
			}
			else if (key == KeyCode.UpArrow)
			{
				base.value = this.mValue + num;
			}
		}
	}

	// Token: 0x04000225 RID: 549
	[HideInInspector]
	[SerializeField]
	private Transform foreground;

	// Token: 0x04000226 RID: 550
	[SerializeField]
	[HideInInspector]
	private float rawValue = 1f;

	// Token: 0x04000227 RID: 551
	[HideInInspector]
	[SerializeField]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	// Token: 0x04000228 RID: 552
	[HideInInspector]
	[SerializeField]
	protected bool mInverted;

	// Token: 0x02000059 RID: 89
	private enum Direction
	{
		// Token: 0x0400022A RID: 554
		Horizontal,
		// Token: 0x0400022B RID: 555
		Vertical,
		// Token: 0x0400022C RID: 556
		Upgraded
	}
}
