using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	// Token: 0x06000129 RID: 297 RVA: 0x00016638 File Offset: 0x00014838
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mPos = this.tweenTarget.localPosition;
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000043C0 File Offset: 0x000025C0
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00016688 File Offset: 0x00014888
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.enabled = false;
			}
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000166DC File Offset: 0x000148DC
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mPos : (this.mPos + this.hover)) : (this.mPos + this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0001676C File Offset: 0x0001496C
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mPos : (this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x000043DE File Offset: 0x000025DE
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04000107 RID: 263
	public Transform tweenTarget;

	// Token: 0x04000108 RID: 264
	public Vector3 hover = Vector3.zero;

	// Token: 0x04000109 RID: 265
	public Vector3 pressed = new Vector3(2f, -2f);

	// Token: 0x0400010A RID: 266
	public float duration = 0.2f;

	// Token: 0x0400010B RID: 267
	private Vector3 mPos;

	// Token: 0x0400010C RID: 268
	private bool mStarted;
}
