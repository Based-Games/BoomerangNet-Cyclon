using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class RaveUpDiscAni : MonoBehaviour
{
	// Token: 0x06000EE1 RID: 3809 RVA: 0x0006B030 File Offset: 0x00069230
	private void Awake()
	{
		this.m_txMainTexture = base.transform.FindChild("DiscImage").GetComponent<UITexture>();
		this.m_txExTexture = base.transform.FindChild("DiscImage_ex").GetComponent<UITexture>();
		this.m_spMainPt = base.transform.FindChild("Pt").GetComponent<UISprite>();
		this.m_spExPt = base.transform.FindChild("Pt_ex").GetComponent<UISprite>();
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0006B0AC File Offset: 0x000692AC
	public void StartAni()
	{
		float num = 0.05f;
		base.CancelInvoke("AniSound");
		base.CancelInvoke("MainTextureSetting");
		this.Ani();
		base.Invoke("MainTextureSetting", num + this.m_fDuration * this.m_fDiscindex + this.m_fDuration * 0.5f);
		base.Invoke("AniSound", num + this.m_fDuration + 0.2f);
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0000CF2E File Offset: 0x0000B12E
	private void AniSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_ALBUM_SPREAD, false);
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0006B11C File Offset: 0x0006931C
	private void Ani()
	{
		base.GetComponent<TweenRotation>().duration = this.m_fDuration;
		base.GetComponent<TweenRotation>().delay = this.m_fDuration * this.m_fDiscindex;
		base.GetComponent<TweenRotation>().from = Vector3.zero;
		base.GetComponent<TweenRotation>().ResetToBeginning();
		base.GetComponent<TweenRotation>().Play(true);
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0006B17C File Offset: 0x0006937C
	private void MainTextureSetting()
	{
		this.m_spMainPt.gameObject.SetActive(true);
		this.m_txMainTexture.mainTexture = this.m_txExTexture.mainTexture;
		this.m_spMainPt.spriteName = "level_" + this.m_iPT.ToString().ToLower() + "_sm";
	}

	// Token: 0x0400104D RID: 4173
	[HideInInspector]
	public PTLEVEL m_iPT;

	// Token: 0x0400104E RID: 4174
	public float m_fDiscindex;

	// Token: 0x0400104F RID: 4175
	private UITexture m_txMainTexture;

	// Token: 0x04001050 RID: 4176
	private UITexture m_txExTexture;

	// Token: 0x04001051 RID: 4177
	private UISprite m_spMainPt;

	// Token: 0x04001052 RID: 4178
	private UISprite m_spExPt;

	// Token: 0x04001053 RID: 4179
	[HideInInspector]
	public DiscInfo m_DiscInfo;

	// Token: 0x04001054 RID: 4180
	private float m_fDuration = 0.15f;
}
