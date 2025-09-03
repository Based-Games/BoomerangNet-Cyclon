using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000029 RID: 41
[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000103 RID: 259 RVA: 0x00015F40 File Offset: 0x00014140
	// (set) Token: 0x06000104 RID: 260 RVA: 0x00015F78 File Offset: 0x00014178
	public bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider collider = base.collider;
			return collider && collider.enabled;
		}
		set
		{
			Collider collider = base.collider;
			if (collider != null)
			{
				collider.enabled = value;
			}
			else
			{
				base.enabled = value;
			}
			this.UpdateColor(value, false);
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00015FB4 File Offset: 0x000141B4
	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mStarted)
			{
				if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
				{
					this.OnHover(UICamera.selectedObject == base.gameObject);
				}
				else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
				{
					this.OnHover(UICamera.hoveredObject == base.gameObject);
				}
				else
				{
					this.UpdateColor(true, false);
				}
			}
		}
		else
		{
			this.UpdateColor(false, true);
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00004042 File Offset: 0x00002242
	protected override void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			base.OnHover(isOver);
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00004056 File Offset: 0x00002256
	protected override void OnPress(bool isPressed)
	{
		if (this.isEnabled)
		{
			base.OnPress(isPressed);
		}
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0000406A File Offset: 0x0000226A
	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOver();
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000040A2 File Offset: 0x000022A2
	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOut();
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000040DA File Offset: 0x000022DA
	protected override void OnSelect(bool isSelected)
	{
		if (this.isEnabled)
		{
			base.OnSelect(isSelected);
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x000040EE File Offset: 0x000022EE
	private void OnClick()
	{
		if (this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00016038 File Offset: 0x00014238
	public void UpdateColor(bool shouldBeEnabled, bool immediate)
	{
		if (this.tweenTarget)
		{
			if (!this.mStarted)
			{
				this.mStarted = true;
				base.Init();
			}
			Color color = ((!shouldBeEnabled) ? this.disabledColor : base.defaultColor);
			TweenColor tweenColor = TweenColor.Begin(this.tweenTarget, 0.15f, color);
			if (tweenColor != null && immediate)
			{
				tweenColor.value = color;
				tweenColor.enabled = false;
			}
		}
	}

	// Token: 0x040000E8 RID: 232
	public static UIButton current;

	// Token: 0x040000E9 RID: 233
	public Color disabledColor = Color.grey;

	// Token: 0x040000EA RID: 234
	public bool dragHighlight;

	// Token: 0x040000EB RID: 235
	public List<EventDelegate> onClick = new List<EventDelegate>();
}
