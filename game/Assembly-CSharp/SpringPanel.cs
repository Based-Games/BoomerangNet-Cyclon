using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	// Token: 0x0600036B RID: 875 RVA: 0x00005D33 File Offset: 0x00003F33
	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00005D59 File Offset: 0x00003F59
	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00022684 File Offset: 0x00020884
	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		if (this.mThreshold == 0f)
		{
			this.mThreshold = (this.target - this.mTrans.localPosition).magnitude * 0.005f;
			this.mThreshold = Mathf.Max(this.mThreshold, 1E-05f);
		}
		bool flag = false;
		Vector3 localPosition = this.mTrans.localPosition;
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
		if (this.mThreshold >= Vector3.Magnitude(vector - this.target))
		{
			vector = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector;
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			this.onFinished();
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x000227D4 File Offset: 0x000209D4
	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.mThreshold = 0f;
		springPanel.enabled = true;
		return springPanel;
	}

	// Token: 0x040002CB RID: 715
	public Vector3 target = Vector3.zero;

	// Token: 0x040002CC RID: 716
	public float strength = 10f;

	// Token: 0x040002CD RID: 717
	public SpringPanel.OnFinished onFinished;

	// Token: 0x040002CE RID: 718
	private UIPanel mPanel;

	// Token: 0x040002CF RID: 719
	private Transform mTrans;

	// Token: 0x040002D0 RID: 720
	private float mThreshold;

	// Token: 0x040002D1 RID: 721
	private UIScrollView mDrag;

	// Token: 0x02000079 RID: 121
	// (Invoke) Token: 0x06000370 RID: 880
	public delegate void OnFinished();
}
