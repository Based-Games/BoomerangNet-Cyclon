using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class HouseMixGraphStickAni : MonoBehaviour
{
	// Token: 0x06000D2D RID: 3373 RVA: 0x0005C0EC File Offset: 0x0005A2EC
	public void Init()
	{
		this.m_Frame = 0f;
		this.m_StickAni.duration = this.m_FrameTime;
		this.m_StickAni.transform.localPosition = Vector3.zero;
		this.m_StickAni.from = Vector3.zero;
		this.m_StickAni.ResetToBeginning();
		this.m_Slider.value = 0f;
		if (this.isMainGraph)
		{
			this.m_Slider.value = this.m_MainVlue / this.LimitMaxValue;
		}
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x0005C178 File Offset: 0x0005A378
	private void Update()
	{
		if (!this.StickAniState)
		{
			return;
		}
		this.m_Frame += Time.deltaTime;
		if (this.m_Frame >= this.m_FrameTime)
		{
			if (!this.isMainGraph)
			{
				this.m_Frame = 0f;
				this.m_StickAni.from = this.m_StickAni.transform.localPosition;
				float num = this.MinValue + (this.MaxValue - this.MinValue) / (float)this.MaxIndex * (float)(this.index - 1);
				float num2 = this.MinValue + (this.MaxValue - this.MinValue) / (float)this.MaxIndex * (float)this.index;
				this.m_StickAni.to = new Vector3(UnityEngine.Random.Range(num - num / 2.5f, num2), 0f, 0f);
				this.m_StickAni.ResetToBeginning();
				this.m_StickAni.Play(true);
			}
			else
			{
				this.StickAniState = false;
			}
		}
		this.m_Slider.value = this.m_StickAni.transform.localPosition.x / this.LimitMaxValue;
	}

	// Token: 0x04000D36 RID: 3382
	public UISlider m_Slider;

	// Token: 0x04000D37 RID: 3383
	public TweenPosition m_StickAni;

	// Token: 0x04000D38 RID: 3384
	[HideInInspector]
	public float LimitMaxValue;

	// Token: 0x04000D39 RID: 3385
	[HideInInspector]
	public float MinValue;

	// Token: 0x04000D3A RID: 3386
	[HideInInspector]
	public float MaxValue;

	// Token: 0x04000D3B RID: 3387
	private float m_Frame;

	// Token: 0x04000D3C RID: 3388
	private float m_FrameTime = 0.1f;

	// Token: 0x04000D3D RID: 3389
	[HideInInspector]
	public bool StickAniState;

	// Token: 0x04000D3E RID: 3390
	[HideInInspector]
	public bool isMainGraph;

	// Token: 0x04000D3F RID: 3391
	[HideInInspector]
	public float m_MainVlue;

	// Token: 0x04000D40 RID: 3392
	[HideInInspector]
	public int index;

	// Token: 0x04000D41 RID: 3393
	[HideInInspector]
	public int MaxIndex;
}
