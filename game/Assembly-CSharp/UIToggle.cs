using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x0200005E RID: 94
[AddComponentMenu("NGUI/Interaction/Toggle")]
[ExecuteInEditMode]
public class UIToggle : UIWidgetContainer
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600024D RID: 589 RVA: 0x000052DB File Offset: 0x000034DB
	// (set) Token: 0x0600024E RID: 590 RVA: 0x000052E3 File Offset: 0x000034E3
	public bool value
	{
		get
		{
			return this.mIsActive;
		}
		set
		{
			if (this.group == 0 || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600024F RID: 591 RVA: 0x00005313 File Offset: 0x00003513
	// (set) Token: 0x06000250 RID: 592 RVA: 0x0000531B File Offset: 0x0000351B
	[Obsolete("Use 'value' instead")]
	public bool isChecked
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0001CCF4 File Offset: 0x0001AEF4
	public static UIToggle GetActiveToggle(int group)
	{
		for (int i = 0; i < UIToggle.list.size; i++)
		{
			UIToggle uitoggle = UIToggle.list[i];
			if (uitoggle != null && uitoggle.group == group && uitoggle.mIsActive)
			{
				return uitoggle;
			}
		}
		return null;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00005324 File Offset: 0x00003524
	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00005331 File Offset: 0x00003531
	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000533F File Offset: 0x0000353F
	private void Start()
	{
		this.mIsActive = !this.startsActive;
		this.mStarted = true;
		this.Set(this.startsActive);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00005363 File Offset: 0x00003563
	private void OnClick()
	{
		if (base.enabled)
		{
			this.value = !this.value;
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0001CD50 File Offset: 0x0001AF50
	private void Set(bool state)
	{
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.group != 0 && state)
			{
				int i = 0;
				int num = UIToggle.list.size;
				while (i < num)
				{
					UIToggle uitoggle = UIToggle.list[i];
					if (uitoggle != this && uitoggle.group == this.group)
					{
						uitoggle.Set(false);
					}
					if (UIToggle.list.size != num)
					{
						num = UIToggle.list.size;
						i = 0;
					}
					else
					{
						i++;
					}
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween)
				{
					this.activeSprite.alpha = ((!this.mIsActive) ? 0f : 1f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, (!this.mIsActive) ? 0f : 1f);
				}
			}
			UIToggle.current = this;
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
			}
			UIToggle.current = null;
			if (this.activeAnimation != null)
			{
				ActiveAnimation.Play(this.activeAnimation, (!state) ? Direction.Reverse : Direction.Forward);
			}
		}
	}

	// Token: 0x0400023D RID: 573
	public static BetterList<UIToggle> list = new BetterList<UIToggle>();

	// Token: 0x0400023E RID: 574
	public static UIToggle current;

	// Token: 0x0400023F RID: 575
	public int group;

	// Token: 0x04000240 RID: 576
	public UIWidget activeSprite;

	// Token: 0x04000241 RID: 577
	public Animation activeAnimation;

	// Token: 0x04000242 RID: 578
	public bool startsActive;

	// Token: 0x04000243 RID: 579
	public bool instantTween;

	// Token: 0x04000244 RID: 580
	public bool optionCanBeNone;

	// Token: 0x04000245 RID: 581
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x04000246 RID: 582
	[HideInInspector]
	[SerializeField]
	private Transform radioButtonRoot;

	// Token: 0x04000247 RID: 583
	[SerializeField]
	[HideInInspector]
	private bool startsChecked;

	// Token: 0x04000248 RID: 584
	[SerializeField]
	[HideInInspector]
	private UISprite checkSprite;

	// Token: 0x04000249 RID: 585
	[SerializeField]
	[HideInInspector]
	private Animation checkAnimation;

	// Token: 0x0400024A RID: 586
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x0400024B RID: 587
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnActivate";

	// Token: 0x0400024C RID: 588
	private bool mIsActive = true;

	// Token: 0x0400024D RID: 589
	private bool mStarted;
}
