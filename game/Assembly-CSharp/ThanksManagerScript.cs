using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class ThanksManagerScript : MonoBehaviour
{
	// Token: 0x06000CAF RID: 3247 RVA: 0x00058C60 File Offset: 0x00056E60
	private void Awake()
	{
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_OUTGAME);
		this.m_aMovie = base.transform.FindChild("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie._folder = "../Data/System/";
		this.m_aMovie._filename = "thanks.mp4";
		this.m_Blind = base.transform.FindChild("Sprite_Blind").GetComponent<TweenAlpha>();
		this.m_utTxtCard = base.transform.FindChild("PanelTxt").GetComponent<UIPanel>();
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Thanks for playing!", "Game Over");
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00058D00 File Offset: 0x00056F00
	private void Start()
	{
		CardManager.eject();
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_ending", false);
		base.Invoke("CompleteLoad", 9f);
		base.Invoke("TouchOn", 4f);
		if (Singleton<GameManager>.instance.ONLOGIN)
		{
			this.FadeInTxt();
		}
		this.m_fTxtTime = 0f;
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00058D60 File Offset: 0x00056F60
	private void FadeInTxt()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = 1f;
		hashtable["to"] = 1280f;
		hashtable["time"] = 1f;
		hashtable["delay"] = 0.5f;
		hashtable["easetype"] = iTween.EaseType.easeOutCubic;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "UpdateFade";
		iTween.ValueTo(base.gameObject, hashtable);
		this.m_utTxtCard.baseClipRegion = new Vector4(0f, 0f, 1f, 768f);
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00058E2C File Offset: 0x0005702C
	private void FadeOutTxt()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = 1280f;
		hashtable["to"] = 1f;
		hashtable["time"] = 0.7f;
		hashtable["easetype"] = iTween.EaseType.easeOutCubic;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "UpdateFade";
		iTween.ValueTo(base.gameObject, hashtable);
		Hashtable hashtable2 = new Hashtable();
		hashtable2["from"] = 1f;
		hashtable2["to"] = 0f;
		hashtable2["time"] = 0.7f;
		hashtable2["easetype"] = iTween.EaseType.easeOutCubic;
		hashtable2["onupdatetarget"] = base.gameObject;
		hashtable2["onupdate"] = "UpdateFadeColor";
		iTween.ValueTo(base.gameObject, hashtable2);
		this.m_utTxtCard.transform.FindChild("txtCard").GetComponent<TweenAlpha>().enabled = false;
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0000B732 File Offset: 0x00009932
	private void UpdateFade(float fValue)
	{
		this.m_utTxtCard.baseClipRegion = new Vector4(0f, 0f, fValue, 768f);
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x00058F60 File Offset: 0x00057160
	private void UpdateFadeColor(float fColor)
	{
		this.m_utTxtCard.transform.FindChild("txtCard").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, fColor);
		this.m_utTxtCard.transform.FindChild("Back").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, fColor);
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0000B754 File Offset: 0x00009954
	private void TouchOn()
	{
		this.m_bTouchState = true;
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x0000B75D File Offset: 0x0000995D
	private void CompleteLoad()
	{
		base.Invoke("NextScene", 1f);
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x0000B76F File Offset: 0x0000996F
	private void NextScene()
	{
		this.m_aMovie.Pause();
		Singleton<SceneSwitcher>.instance.LoadNextScene("CopyRight");
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x00058FD8 File Offset: 0x000571D8
	private void Update()
	{
		this.m_fTxtTime += Time.deltaTime;
		if (Singleton<GameManager>.instance.ONLOGIN && !this.m_bFadeOut && 5f < this.m_fTxtTime)
		{
			this.FadeOutTxt();
			this.m_bFadeOut = true;
		}
		base.CancelInvoke("CompleteLoad");
		if (Input.GetMouseButtonDown(0))
		{
			this.CompleteLoad();
		}
		if (this.m_aMovie.IsFinishedPlaying)
		{
			this.CompleteLoad();
		}
	}

	// Token: 0x04000C8B RID: 3211
	private const float MAX_TIME = 5f;

	// Token: 0x04000C8C RID: 3212
	private const float FADEOUT_TIME = 0.7f;

	// Token: 0x04000C8D RID: 3213
	private bool m_bTouchState;

	// Token: 0x04000C8E RID: 3214
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000C8F RID: 3215
	private TweenAlpha m_Blind;

	// Token: 0x04000C90 RID: 3216
	private UIPanel m_utTxtCard;

	// Token: 0x04000C91 RID: 3217
	private bool m_bFadeOut;

	// Token: 0x04000C92 RID: 3218
	private float m_fTxtTime;
}
