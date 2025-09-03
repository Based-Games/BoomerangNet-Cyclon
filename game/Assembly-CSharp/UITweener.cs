using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000098 RID: 152
public abstract class UITweener : MonoBehaviour
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060004BC RID: 1212 RVA: 0x000265A0 File Offset: 0x000247A0
	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration));
			}
			return this.mAmountPerDelta;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060004BD RID: 1213 RVA: 0x00006CD2 File Offset: 0x00004ED2
	// (set) Token: 0x060004BE RID: 1214 RVA: 0x00006CDA File Offset: 0x00004EDA
	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x00006CE8 File Offset: 0x00004EE8
	public Direction direction
	{
		get
		{
			return (this.mAmountPerDelta >= 0f) ? Direction.Forward : Direction.Reverse;
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00006D01 File Offset: 0x00004F01
	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00006D1A File Offset: 0x00004F1A
	protected virtual void Start()
	{
		this.Update();
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00026604 File Offset: 0x00024804
	private void Update()
	{
		float num = ((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
		float num2 = ((!this.ignoreTimeScale) ? Time.time : RealTime.time);
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = num2 + this.delay;
		}
		if (num2 < this.mStartTime)
		{
			return;
		}
		this.mFactor += this.amountPerDelta * num;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			UITweener.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
			}
			UITweener.current = null;
			if ((this.mFactor == 1f && this.mAmountPerDelta > 0f) || (this.mFactor == 0f && this.mAmountPerDelta < 0f))
			{
				base.enabled = false;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00006D22 File Offset: 0x00004F22
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00026840 File Offset: 0x00024A40
	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.5707964f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.5707964f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.2831855f) / 6.2831855f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		this.OnUpdate((this.animationCurve == null) ? num : this.animationCurve.Evaluate(num), isFinished);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00026994 File Offset: 0x00024B94
	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00006D2B File Offset: 0x00004F2B
	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00006D2B File Offset: 0x00004F2B
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00006D34 File Offset: 0x00004F34
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00006D3D File Offset: 0x00004F3D
	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00006D70 File Offset: 0x00004F70
	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = ((this.mAmountPerDelta >= 0f) ? 0f : 1f);
		this.Sample(this.mFactor, false);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00006DAB File Offset: 0x00004FAB
	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	// Token: 0x060004CC RID: 1228
	protected abstract void OnUpdate(float factor, bool isFinished);

	// Token: 0x060004CD RID: 1229 RVA: 0x00026A2C File Offset: 0x00024C2C
	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		t.mStarted = false;
		t.duration = duration;
		t.mFactor = 0f;
		t.mAmountPerDelta = Mathf.Abs(t.mAmountPerDelta);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.enabled = true;
		if (duration <= 0f)
		{
			t.Sample(1f, true);
			t.enabled = false;
		}
		return t;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00003648 File Offset: 0x00001848
	public virtual void SetStartToCurrentValue()
	{
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00003648 File Offset: 0x00001848
	public virtual void SetEndToCurrentValue()
	{
	}

	// Token: 0x04000375 RID: 885
	public static UITweener current;

	// Token: 0x04000376 RID: 886
	[HideInInspector]
	public UITweener.Method method;

	// Token: 0x04000377 RID: 887
	[HideInInspector]
	public UITweener.Style style;

	// Token: 0x04000378 RID: 888
	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04000379 RID: 889
	[HideInInspector]
	public bool ignoreTimeScale = true;

	// Token: 0x0400037A RID: 890
	[HideInInspector]
	public float delay;

	// Token: 0x0400037B RID: 891
	[HideInInspector]
	public float duration = 1f;

	// Token: 0x0400037C RID: 892
	[HideInInspector]
	public bool steeperCurves;

	// Token: 0x0400037D RID: 893
	[HideInInspector]
	public int tweenGroup;

	// Token: 0x0400037E RID: 894
	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x0400037F RID: 895
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x04000380 RID: 896
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x04000381 RID: 897
	private bool mStarted;

	// Token: 0x04000382 RID: 898
	private float mStartTime;

	// Token: 0x04000383 RID: 899
	private float mDuration;

	// Token: 0x04000384 RID: 900
	private float mAmountPerDelta = 1000f;

	// Token: 0x04000385 RID: 901
	private float mFactor;

	// Token: 0x02000099 RID: 153
	public enum Method
	{
		// Token: 0x04000387 RID: 903
		Linear,
		// Token: 0x04000388 RID: 904
		EaseIn,
		// Token: 0x04000389 RID: 905
		EaseOut,
		// Token: 0x0400038A RID: 906
		EaseInOut,
		// Token: 0x0400038B RID: 907
		BounceIn,
		// Token: 0x0400038C RID: 908
		BounceOut
	}

	// Token: 0x0200009A RID: 154
	public enum Style
	{
		// Token: 0x0400038E RID: 910
		Once,
		// Token: 0x0400038F RID: 911
		Loop,
		// Token: 0x04000390 RID: 912
		PingPong
	}
}
