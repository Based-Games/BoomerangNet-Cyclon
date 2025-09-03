using System;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class HouseMixEqualizer : MonoBehaviour
{
	// Token: 0x06000D15 RID: 3349 RVA: 0x0005AE00 File Offset: 0x00059000
	private void Awake()
	{
		this.Eq = new Transform[this.Grid.transform.childCount];
		this.EqSprite = new UISprite[this.Grid.transform.childCount];
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			this.Eq[i] = this.Grid.transform.GetChild(i);
			this.EqSprite[i] = this.Grid.transform.GetChild(i).GetComponent<UISprite>();
		}
		for (int j = 0; j < this.Eq.Length; j++)
		{
			this.Eq[j].transform.localPosition = new Vector3((float)(j * 11), 0f, 0f);
			this.Eq[j].transform.GetComponent<TweenHeight>().duration = this.m_UpdateTime * 2f;
			this.Eq[j].transform.GetComponent<TweenHeight>().style = UITweener.Style.Once;
		}
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x0005AF18 File Offset: 0x00059118
	private void Start()
	{
		this.maxSample = this.m_MaxByte;
		this.sampleCount = this.Grid.transform.childCount;
		this.barWidth = 10f;
		this.smoothFall = 1f;
		this.spectrum = new float[this.maxSample];
		this.spectrum2 = new float[this.maxSample];
		this.indexPerFreq = 22050f / (float)this.maxSample;
		this.barValue = new float[this.sampleCount];
		this.barValue2 = new float[this.sampleCount];
		this.OldBarValue = new float[this.sampleCount];
		for (int i = 0; i < this.sampleCount + 1; i++)
		{
			this.barValue[0] = 0f;
		}
		this.m_AS = Singleton<SoundSourceManager>.instance.getNowBGM();
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x0005AFFC File Offset: 0x000591FC
	private float[] AdjustSpectrumData(float[] org, float intSample, int numSample, float maxValue, float scale)
	{
		float num = (float)org.Length;
		float[] array = new float[numSample];
		for (int i = 0; i < numSample; i++)
		{
			array[i] = 0f;
		}
		float num2 = 0f;
		int num3 = 8;
		int num4 = 1;
		int num5 = 0;
		int num6 = 0;
		while ((float)num6 < num && num5 < numSample)
		{
			if (num6 % num3 == 0)
			{
				num2 = 0f;
				num4 = 1;
			}
			num2 += org[num6];
			if ((float)num6 * this.indexPerFreq < (float)num3)
			{
				num4++;
			}
			else
			{
				float num7 = (float)(1 + num4 / 100);
				num2 = num2 * num7 / (float)num4;
				array[num5] = Mathf.Max(-maxValue, Mathf.Log(num2) * scale);
				num3 = Mathf.CeilToInt((float)num3 * intSample);
				num5++;
			}
			num6++;
		}
		return array;
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x0005B0D0 File Offset: 0x000592D0
	private void UpdateEq()
	{
		float num = (float)this.MaxValue;
		float barScale = this.m_barScale;
		this.m_AS = Singleton<SoundSourceManager>.instance.getNowBGM();
		if (this.m_AS.clip == null)
		{
			return;
		}
		if (this.ClipName != this.m_AS.clip.name)
		{
			this.StopFrame = 0f;
			for (int i = 0; i < this.sampleCount; i++)
			{
				this.OldBarValue[i] = 0f;
				this.Eq[i].transform.GetComponent<TweenHeight>().from = (int)this.Eq[i].transform.GetComponent<UISprite>().localSize.y;
				this.Eq[i].transform.GetComponent<TweenHeight>().to = (int)this.OldBarValue[i];
				this.Eq[i].transform.GetComponent<TweenHeight>().duration = 0.5f;
				this.Eq[i].transform.GetComponent<TweenHeight>().ResetToBeginning();
				this.Eq[i].transform.GetComponent<TweenHeight>().Play(true);
			}
		}
		else
		{
			this.StopFrame += Time.deltaTime;
			if (this.StopFrame > 0.5f)
			{
				this.m_AS.GetSpectrumData(this.spectrum, 0, FFTWindow.BlackmanHarris);
				this.m_AS.GetSpectrumData(this.spectrum2, 1, FFTWindow.BlackmanHarris);
				this.barValue = this.AdjustSpectrumData(this.spectrum, 1.65f, this.sampleCount, num, barScale);
				this.barValue2 = this.AdjustSpectrumData(this.spectrum2, 1.65f, this.sampleCount, num, barScale);
				for (int j = 0; j < this.sampleCount; j++)
				{
					this.Eq[j].transform.GetComponent<TweenHeight>().from = (int)this.Eq[j].transform.GetComponent<UISprite>().localSize.y;
					this.Eq[j].transform.GetComponent<TweenHeight>().duration = this.m_UpdateTime * 2f;
					if ((int)this.OldBarValue[j] <= (int)(num + (this.barValue[j] + this.barValue2[j]) / 2f))
					{
						this.OldBarValue[j] = (float)((int)(num + (this.barValue[j] + this.barValue2[j]) / 2f));
						this.Eq[j].transform.GetComponent<TweenHeight>().to = (int)this.OldBarValue[j];
					}
					else
					{
						this.OldBarValue[j] -= this.OldBarValue[j] * 0.5f;
						this.Eq[j].transform.GetComponent<TweenHeight>().to = (int)this.OldBarValue[j];
					}
					this.Eq[j].transform.GetComponent<TweenHeight>().ResetToBeginning();
					this.Eq[j].transform.GetComponent<TweenHeight>().Play(true);
					this.barValue[j] = Mathf.Min(this.barValue[j] - this.smoothFall, 0f);
				}
			}
		}
		this.ClipName = this.m_AS.clip.name;
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x0000BC09 File Offset: 0x00009E09
	private void Update()
	{
		this.m_Frame += Time.deltaTime;
		if (this.m_Frame > this.m_UpdateTime)
		{
			this.m_Frame = 0f;
			this.UpdateEq();
		}
	}

	// Token: 0x04000CFD RID: 3325
	public int MaxValue;

	// Token: 0x04000CFE RID: 3326
	public Transform Grid;

	// Token: 0x04000CFF RID: 3327
	public Transform[] Eq;

	// Token: 0x04000D00 RID: 3328
	public UISprite[] EqSprite;

	// Token: 0x04000D01 RID: 3329
	private AudioSource m_AS;

	// Token: 0x04000D02 RID: 3330
	private float[] SpectrumData;

	// Token: 0x04000D03 RID: 3331
	private float[] EqualizerObjValue;

	// Token: 0x04000D04 RID: 3332
	private float m_Frame;

	// Token: 0x04000D05 RID: 3333
	public float m_UpdateTime;

	// Token: 0x04000D06 RID: 3334
	public float m_barScale;

	// Token: 0x04000D07 RID: 3335
	public int m_MaxByte;

	// Token: 0x04000D08 RID: 3336
	private int maxSample;

	// Token: 0x04000D09 RID: 3337
	private int sampleCount;

	// Token: 0x04000D0A RID: 3338
	private float barWidth;

	// Token: 0x04000D0B RID: 3339
	private float smoothFall;

	// Token: 0x04000D0C RID: 3340
	private float[] spectrum;

	// Token: 0x04000D0D RID: 3341
	private float[] spectrum2;

	// Token: 0x04000D0E RID: 3342
	private float[] barValue;

	// Token: 0x04000D0F RID: 3343
	private float[] barValue2;

	// Token: 0x04000D10 RID: 3344
	private float[] OldBarValue;

	// Token: 0x04000D11 RID: 3345
	private float indexPerFreq;

	// Token: 0x04000D12 RID: 3346
	private string ClipName = " ";

	// Token: 0x04000D13 RID: 3347
	private float StopFrame;
}
