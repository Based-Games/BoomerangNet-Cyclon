using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000049 RID: 73
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Tween")]
public class UIPlayTween : MonoBehaviour
{
	// Token: 0x060001B3 RID: 435 RVA: 0x00004C81 File Offset: 0x00002E81
	private void Awake()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00004CB2 File Offset: 0x00002EB2
	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00004CD8 File Offset: 0x00002ED8
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00018D00 File Offset: 0x00016F00
	private void OnHover(bool isOver)
	{
		if (base.enabled && (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver)))
		{
			this.mActivated = isOver && this.trigger == Trigger.OnHover;
			this.Play(isOver);
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00004CF6 File Offset: 0x00002EF6
	private void OnDragOut()
	{
		if (base.enabled && this.mActivated)
		{
			this.mActivated = false;
			this.Play(false);
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00018D68 File Offset: 0x00016F68
	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.mActivated = isPressed && this.trigger == Trigger.OnPress;
			this.Play(isPressed);
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00004D1C File Offset: 0x00002F1C
	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00004D3B File Offset: 0x00002F3B
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00018DD0 File Offset: 0x00016FD0
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.mActivated = isSelected && this.trigger == Trigger.OnSelect;
			this.Play(isSelected);
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00018E3C File Offset: 0x0001703C
	private void OnActivate(bool isActive)
	{
		if (base.enabled && (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && isActive) || (this.trigger == Trigger.OnActivateFalse && !isActive)))
		{
			this.Play(isActive);
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00018E8C File Offset: 0x0001708C
	private void Update()
	{
		if (this.disableWhenFinished != DisableCondition.DoNotDisable && this.mTweens != null)
		{
			bool flag = true;
			bool flag2 = true;
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (uitweener.enabled)
					{
						flag = false;
						break;
					}
					if (uitweener.direction != (Direction)this.disableWhenFinished)
					{
						flag2 = false;
					}
				}
				i++;
			}
			if (flag)
			{
				if (flag2)
				{
					NGUITools.SetActive(this.tweenTarget, false);
				}
				this.mTweens = null;
			}
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00018F38 File Offset: 0x00017138
	public void Play(bool forward)
	{
		this.mActive = 0;
		GameObject gameObject = ((!(this.tweenTarget == null)) ? this.tweenTarget : base.gameObject);
		if (!NGUITools.GetActive(gameObject))
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		this.mTweens = ((!this.includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (this.mTweens.Length == 0)
		{
			if (this.disableWhenFinished != DisableCondition.DoNotDisable)
			{
				NGUITools.SetActive(this.tweenTarget, false);
			}
		}
		else
		{
			bool flag = false;
			if (this.playDirection == Direction.Reverse)
			{
				forward = !forward;
			}
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (!flag && !NGUITools.GetActive(gameObject))
					{
						flag = true;
						NGUITools.SetActive(gameObject, true);
					}
					this.mActive++;
					if (this.playDirection == Direction.Toggle)
					{
						uitweener.Toggle();
					}
					else
					{
						if (this.resetOnPlay || (this.resetIfDisabled && !uitweener.enabled))
						{
							uitweener.ResetToBeginning();
						}
						uitweener.Play(forward);
					}
					EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
				i++;
			}
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000190B0 File Offset: 0x000172B0
	private void OnFinished()
	{
		if (--this.mActive == 0)
		{
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
		}
	}

	// Token: 0x040001AA RID: 426
	public GameObject tweenTarget;

	// Token: 0x040001AB RID: 427
	public int tweenGroup;

	// Token: 0x040001AC RID: 428
	public Trigger trigger;

	// Token: 0x040001AD RID: 429
	public Direction playDirection = Direction.Forward;

	// Token: 0x040001AE RID: 430
	public bool resetOnPlay;

	// Token: 0x040001AF RID: 431
	public bool resetIfDisabled;

	// Token: 0x040001B0 RID: 432
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x040001B1 RID: 433
	public DisableCondition disableWhenFinished;

	// Token: 0x040001B2 RID: 434
	public bool includeChildren;

	// Token: 0x040001B3 RID: 435
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x040001B4 RID: 436
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x040001B5 RID: 437
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x040001B6 RID: 438
	private UITweener[] mTweens;

	// Token: 0x040001B7 RID: 439
	private bool mStarted;

	// Token: 0x040001B8 RID: 440
	private int mActive;

	// Token: 0x040001B9 RID: 441
	private bool mActivated;
}
