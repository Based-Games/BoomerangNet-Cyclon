using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	// Token: 0x0600043E RID: 1086 RVA: 0x00006615 File Offset: 0x00004815
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00025A0C File Offset: 0x00023C0C
	private void Update()
	{
		float num = ((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).magnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, num);
			if (this.mThreshold >= (this.target - this.mTrans.position).magnitude)
			{
				this.mTrans.position = this.target;
				if (this.onFinished != null)
				{
					this.onFinished(this);
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).magnitude * 0.001f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, num);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).magnitude)
			{
				this.mTrans.localPosition = this.target;
				if (this.onFinished != null)
				{
					this.onFinished(this);
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				base.enabled = false;
			}
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00025C24 File Offset: 0x00023E24
	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}

	// Token: 0x0400033E RID: 830
	public Vector3 target = Vector3.zero;

	// Token: 0x0400033F RID: 831
	public float strength = 10f;

	// Token: 0x04000340 RID: 832
	public bool worldSpace;

	// Token: 0x04000341 RID: 833
	public bool ignoreTimeScale;

	// Token: 0x04000342 RID: 834
	public GameObject eventReceiver;

	// Token: 0x04000343 RID: 835
	public string callWhenFinished;

	// Token: 0x04000344 RID: 836
	public SpringPosition.OnFinished onFinished;

	// Token: 0x04000345 RID: 837
	private Transform mTrans;

	// Token: 0x04000346 RID: 838
	private float mThreshold;

	// Token: 0x0200008C RID: 140
	// (Invoke) Token: 0x06000442 RID: 1090
	public delegate void OnFinished(SpringPosition spring);
}
