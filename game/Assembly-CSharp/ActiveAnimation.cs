using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000062 RID: 98
[RequireComponent(typeof(Animation))]
[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : MonoBehaviour
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000260 RID: 608 RVA: 0x0001D198 File Offset: 0x0001B398
	public bool isPlaying
	{
		get
		{
			if (this.mAnim == null)
			{
				return false;
			}
			foreach (object obj in this.mAnim)
			{
				AnimationState animationState = (AnimationState)obj;
				if (this.mAnim.IsPlaying(animationState.name))
				{
					if (this.mLastDirection == Direction.Forward)
					{
						if (animationState.time < animationState.length)
						{
							return true;
						}
					}
					else
					{
						if (this.mLastDirection != Direction.Reverse)
						{
							return true;
						}
						if (animationState.time > 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0001D280 File Offset: 0x0001B480
	public void Reset()
	{
		if (this.mAnim != null)
		{
			foreach (object obj in this.mAnim)
			{
				AnimationState animationState = (AnimationState)obj;
				if (this.mLastDirection == Direction.Reverse)
				{
					animationState.time = animationState.length;
				}
				else if (this.mLastDirection == Direction.Forward)
				{
					animationState.time = 0f;
				}
			}
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000053A7 File Offset: 0x000035A7
	private void Start()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0001D324 File Offset: 0x0001B524
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (deltaTime == 0f)
		{
			return;
		}
		if (this.mAnim != null)
		{
			bool flag = false;
			foreach (object obj in this.mAnim)
			{
				AnimationState animationState = (AnimationState)obj;
				if (this.mAnim.IsPlaying(animationState.name))
				{
					float num = animationState.speed * deltaTime;
					animationState.time += num;
					if (num < 0f)
					{
						if (animationState.time > 0f)
						{
							flag = true;
						}
						else
						{
							animationState.time = 0f;
						}
					}
					else if (animationState.time < animationState.length)
					{
						flag = true;
					}
					else
					{
						animationState.time = animationState.length;
					}
				}
			}
			this.mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
			if (this.mNotify)
			{
				this.mNotify = false;
				ActiveAnimation.current = this;
				EventDelegate.Execute(this.onFinished);
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
				}
				ActiveAnimation.current = null;
				if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
				{
					NGUITools.SetActive(base.gameObject, false);
				}
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0001D4DC File Offset: 0x0001B6DC
	private void Play(string clipName, Direction playDirection)
	{
		if (this.mAnim != null)
		{
			base.enabled = true;
			this.mAnim.enabled = false;
			if (playDirection == Direction.Toggle)
			{
				playDirection = ((this.mLastDirection == Direction.Forward) ? Direction.Reverse : Direction.Forward);
			}
			bool flag = string.IsNullOrEmpty(clipName);
			if (flag)
			{
				if (!this.mAnim.isPlaying)
				{
					this.mAnim.Play();
				}
			}
			else if (!this.mAnim.IsPlaying(clipName))
			{
				this.mAnim.Play(clipName);
			}
			foreach (object obj in this.mAnim)
			{
				AnimationState animationState = (AnimationState)obj;
				if (string.IsNullOrEmpty(clipName) || animationState.name == clipName)
				{
					float num = Mathf.Abs(animationState.speed);
					animationState.speed = num * (float)playDirection;
					if (playDirection == Direction.Reverse && animationState.time == 0f)
					{
						animationState.time = animationState.length;
					}
					else if (playDirection == Direction.Forward && animationState.time == animationState.length)
					{
						animationState.time = 0f;
					}
				}
			}
			this.mLastDirection = playDirection;
			this.mNotify = true;
			this.mAnim.Sample();
		}
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0001D65C File Offset: 0x0001B85C
	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].Refresh();
				i++;
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		return activeAnimation;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x000053D8 File Offset: 0x000035D8
	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x000053E4 File Offset: 0x000035E4
	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	// Token: 0x04000256 RID: 598
	public static ActiveAnimation current;

	// Token: 0x04000257 RID: 599
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04000258 RID: 600
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x04000259 RID: 601
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x0400025A RID: 602
	private Animation mAnim;

	// Token: 0x0400025B RID: 603
	private Direction mLastDirection;

	// Token: 0x0400025C RID: 604
	private Direction mDisableDirection;

	// Token: 0x0400025D RID: 605
	private bool mNotify;
}
