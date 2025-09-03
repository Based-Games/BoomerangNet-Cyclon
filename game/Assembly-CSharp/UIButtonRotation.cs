using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	// Token: 0x06000130 RID: 304 RVA: 0x000167D4 File Offset: 0x000149D4
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mRot = this.tweenTarget.localRotation;
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000442C File Offset: 0x0000262C
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00016824 File Offset: 0x00014A24
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.value = this.mRot;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00016878 File Offset: 0x00014A78
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))) : (this.mRot * Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00016910 File Offset: 0x00014B10
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000444A File Offset: 0x0000264A
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x0400010D RID: 269
	public Transform tweenTarget;

	// Token: 0x0400010E RID: 270
	public Vector3 hover = Vector3.zero;

	// Token: 0x0400010F RID: 271
	public Vector3 pressed = Vector3.zero;

	// Token: 0x04000110 RID: 272
	public float duration = 0.2f;

	// Token: 0x04000111 RID: 273
	private Quaternion mRot;

	// Token: 0x04000112 RID: 274
	private bool mStarted;
}
