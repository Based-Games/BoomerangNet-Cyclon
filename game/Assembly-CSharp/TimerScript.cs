using System;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class TimerScript : MonoBehaviour
{
	// Token: 0x0600094D RID: 2381 RVA: 0x00044960 File Offset: 0x00042B60
	private void Awake()
	{
		this.m_tTimer = base.transform.FindChild("TimeNum").GetComponent<UILabel>();
		this.m_sTimerBg = base.transform.FindChild("BG").GetComponent<UISprite>();
		this.m_bOnTimer = false;
		this.m_tTimer.transform.localPosition = new Vector3(-12.58f, 4.29f, 0f);
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00009AA7 File Offset: 0x00007CA7
	private void Update()
	{
		this.UpdateTimer();
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00009AAF File Offset: 0x00007CAF
	public void StartTimer(int iTime, int LimitTime = 10)
	{
		this.m_iLimitTime = LimitTime;
		this.m_iTime = iTime;
		this.m_bOnTimer = true;
		this.m_tTimer.text = string.Format("{0:00}", this.m_iTime);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00009AE6 File Offset: 0x00007CE6
	public void StopTimer()
	{
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		this.m_bOnTimer = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x00009B07 File Offset: 0x00007D07
	public void PauseTimer(bool bPause)
	{
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		this.m_bOnTimer = !bPause;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x000449D0 File Offset: 0x00042BD0
	private void UpdateTimer()
	{
		if (!this.m_bOnTimer)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		this.m_fBGAniFrame += deltaTime;
		int num = (int)(45f * (this.m_fBGAniFrame / 0.8f));
		if (num >= 45)
		{
			this.m_fBGAniFrame = 0f;
			num = 0;
			this.TimeNumber();
		}
		this.m_tTimer.transform.localScale = new Vector3(1.2f - 0.25f * ((float)num / 44f), 1.2f - 0.25f * ((float)num / 44f), 1f);
		this.m_sTimerBg.spriteName = "time_000" + num.ToString();
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00044A84 File Offset: 0x00042C84
	private void TimeNumber()
	{
		if (!Singleton<GameManager>.instance.MenuTimer)
		{
			if (this.m_iTime - 1 >= 0)
			{
				this.m_iTime--;
			}
			else
			{
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
				this.m_iTime = 0;
				if (this.m_bOnTimer && this.CallBackTimeover != null)
				{
					this.CallBackTimeover();
				}
				this.m_bOnTimer = false;
			}
		}
		if (this.m_iTime <= this.m_iLimitTime)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_COMMON_TIMER, false);
		}
		this.m_tTimer.text = string.Format("{0:00}", this.m_iTime);
	}

	// Token: 0x04000778 RID: 1912
	private const float TIME_FRAME = 0.8f;

	// Token: 0x04000779 RID: 1913
	private const float TIMENUM_SCALE = 0.25f;

	// Token: 0x0400077A RID: 1914
	private UILabel m_tTimer;

	// Token: 0x0400077B RID: 1915
	private UISprite m_sTimerBg;

	// Token: 0x0400077C RID: 1916
	private int m_iTime = 20;

	// Token: 0x0400077D RID: 1917
	private bool m_bOnTimer;

	// Token: 0x0400077E RID: 1918
	public TimerScript.CompleteTimeOver CallBackTimeover;

	// Token: 0x0400077F RID: 1919
	private float m_fBGAniFrame;

	// Token: 0x04000780 RID: 1920
	private int m_iLimitTime = 10;

	// Token: 0x02000106 RID: 262
	// (Invoke) Token: 0x06000956 RID: 2390
	public delegate void CompleteTimeOver();
}
