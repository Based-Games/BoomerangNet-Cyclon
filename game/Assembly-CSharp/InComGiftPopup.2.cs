using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class InComGiftPopup : MonoBehaviour
{
	// Token: 0x06000E5E RID: 3678 RVA: 0x00067DE8 File Offset: 0x00065FE8
	private IEnumerator PopupView()
	{
		USERDATA uData = Singleton<GameManager>.instance.UserData;
		this.m_BG.enabled = true;
		this.Popup(this.m_NowPopupKind);
		yield return new WaitForSeconds(this.m_fPopupTime);
		this.Popup(this.m_NowPopupKind);
		yield return new WaitForSeconds(this.m_fPopupTime);
		this.Popup(this.m_NowPopupKind);
		yield return new WaitForSeconds(this.m_fPopupTime);
		if (uData.INCOMGIFTPOPUPSTATE == INCOMGIFTPOPUP.BASEGIFT)
		{
			this.DestroyPopup();
		}
		this.Popup(this.m_NowPopupKind);
		yield return new WaitForSeconds(this.m_fPopupTime);
		if (uData.INCOMGIFTPOPUPSTATE == INCOMGIFTPOPUP.LV10GIFT)
		{
			this.DestroyPopup();
		}
		this.Popup(this.m_NowPopupKind);
		yield return new WaitForSeconds(this.m_fPopupTime);
		if (this.m_NowPopupKind == InComGiftPopup.popupKind_e.Max)
		{
			this.DestroyPopup();
		}
		yield break;
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00067E04 File Offset: 0x00066004
	private void Awake()
	{
		this.m_BG = base.transform.FindChild("bg").GetComponent<TweenAlpha>();
		this.m_Popups = new TweenRotation[5];
		for (int i = 0; i < this.m_Popups.Length; i++)
		{
			this.m_Popups[i] = base.transform.FindChild("Popup_" + ((InComGiftPopup.popupKind_e)i).ToString()).GetComponent<TweenRotation>();
		}
		this.m_sTimer = GameObject.Find("Timer").GetComponent<TimerScript>();
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
	private void Start()
	{
		base.Invoke("Check", Time.deltaTime);
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x00067E94 File Offset: 0x00066094
	private void Check()
	{
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
			return;
		}
		if (userData.INCOMGIFTPOPUPSTATE != INCOMGIFTPOPUP.NONE)
		{
			this.m_sTimer.PauseTimer(true);
			base.StartCoroutine("PopupView");
		}
		else
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x00067EFC File Offset: 0x000660FC
	private void DestroyPopup()
	{
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		this.m_sTimer.PauseTimer(false);
		userData.INCOMGIFTPOPUPSTATE = INCOMGIFTPOPUP.NONE;
		UnityEngine.Object.DestroyObject(base.gameObject);
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00067F34 File Offset: 0x00066134
	private void Popup(InComGiftPopup.popupKind_e kind)
	{
		if (kind > InComGiftPopup.popupKind_e.item)
		{
			this.m_Popups[kind - InComGiftPopup.popupKind_e.CD].gameObject.SetActive(false);
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_POPUP, false);
		this.m_Popups[(int)kind].transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		this.m_Popups[(int)kind].duration = 0.25f;
		this.m_Popups[(int)kind].ResetToBeginning();
		this.m_Popups[(int)kind].Play(true);
		this.m_NowPopupKind++;
	}

	// Token: 0x04000F9A RID: 3994
	private InComGiftPopup.popupKind_e m_NowPopupKind;

	// Token: 0x04000F9B RID: 3995
	private TweenRotation[] m_Popups;

	// Token: 0x04000F9C RID: 3996
	private TweenAlpha m_BG;

	// Token: 0x04000F9D RID: 3997
	private float m_fPopupTime = 1.5f;

	// Token: 0x04000F9E RID: 3998
	private TimerScript m_sTimer;

	// Token: 0x020001F2 RID: 498
	public enum popupKind_e
	{
		// Token: 0x04000FA0 RID: 4000
		item,
		// Token: 0x04000FA1 RID: 4001
		CD,
		// Token: 0x04000FA2 RID: 4002
		ICON,
		// Token: 0x04000FA3 RID: 4003
		CD_lv10,
		// Token: 0x04000FA4 RID: 4004
		CD_lv20,
		// Token: 0x04000FA5 RID: 4005
		Max
	}
}
