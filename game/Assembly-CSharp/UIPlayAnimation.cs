using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000046 RID: 70
[AddComponentMenu("NGUI/Interaction/Play Animation")]
[ExecuteInEditMode]
public class UIPlayAnimation : MonoBehaviour
{
	// Token: 0x060001A2 RID: 418 RVA: 0x00004B3E File Offset: 0x00002D3E
	private void Awake()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00004B6F File Offset: 0x00002D6F
	private void Start()
	{
		this.mStarted = true;
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<Animation>();
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00004B95 File Offset: 0x00002D95
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x000189A8 File Offset: 0x00016BA8
	private void OnHover(bool isOver)
	{
		if (base.enabled && (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver)))
		{
			this.mActivated = isOver && this.trigger == Trigger.OnHover;
			this.Play(isOver);
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00004BB3 File Offset: 0x00002DB3
	private void OnDragOut()
	{
		if (base.enabled && this.mActivated)
		{
			this.mActivated = false;
			this.Play(false);
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00018A10 File Offset: 0x00016C10
	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.mActivated = isPressed && this.trigger == Trigger.OnPress;
			this.Play(isPressed);
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00004BD9 File Offset: 0x00002DD9
	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00004BF8 File Offset: 0x00002DF8
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00018A78 File Offset: 0x00016C78
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.mActivated = isSelected && this.trigger == Trigger.OnSelect;
			this.Play(isSelected);
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00018AE4 File Offset: 0x00016CE4
	private void OnActivate(bool isActive)
	{
		if (base.enabled && (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && isActive) || (this.trigger == Trigger.OnActivateFalse && !isActive)))
		{
			this.Play(isActive);
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00018B34 File Offset: 0x00016D34
	public void Play(bool forward)
	{
		if (this.target)
		{
			if (this.clearSelection && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			int num = (int)(-(int)this.playDirection);
			Direction direction = (Direction)((!forward) ? num : ((int)this.playDirection));
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
			if (activeAnimation != null)
			{
				if (this.resetOnPlay)
				{
					activeAnimation.Reset();
				}
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
			}
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00018C04 File Offset: 0x00016E04
	private void OnFinished()
	{
		EventDelegate.Execute(this.onFinished);
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
		}
		this.eventReceiver = null;
	}

	// Token: 0x04000193 RID: 403
	public Animation target;

	// Token: 0x04000194 RID: 404
	public string clipName;

	// Token: 0x04000195 RID: 405
	public Trigger trigger;

	// Token: 0x04000196 RID: 406
	public Direction playDirection = Direction.Forward;

	// Token: 0x04000197 RID: 407
	public bool resetOnPlay;

	// Token: 0x04000198 RID: 408
	public bool clearSelection;

	// Token: 0x04000199 RID: 409
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x0400019A RID: 410
	public DisableCondition disableWhenFinished;

	// Token: 0x0400019B RID: 411
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x0400019C RID: 412
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x0400019D RID: 413
	[SerializeField]
	[HideInInspector]
	private string callWhenFinished;

	// Token: 0x0400019E RID: 414
	private bool mStarted;

	// Token: 0x0400019F RID: 415
	private bool mActivated;
}
