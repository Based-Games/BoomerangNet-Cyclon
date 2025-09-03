using System;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class MainManagermentScript : MonoBehaviour
{
	// Token: 0x06000E6B RID: 3691 RVA: 0x0000C7DB File Offset: 0x0000A9DB
	private void Start()
	{
		this.SetObject();
		this.ClearPanel();
		this.m_oMain.SetActive(true);
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("In test menu", null);
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x000681A4 File Offset: 0x000663A4
	private void SetObject()
	{
		GameObject gameObject = base.transform.FindChild("Anchor").gameObject;
		this.m_oMain = gameObject.transform.FindChild("Panel").gameObject;
		this.m_oMonitor = gameObject.transform.FindChild("PanelMonitor").gameObject;
		this.m_oTouch = gameObject.transform.FindChild("PanelTouch").gameObject;
		this.m_oIO = gameObject.transform.FindChild("PanelIO").gameObject;
		this.m_oSound = gameObject.transform.FindChild("PanelSound").gameObject;
		this.m_oGameSetup = gameObject.transform.FindChild("PanelGameSetup").gameObject;
		this.m_oBookkeepings = gameObject.transform.FindChild("PanelBookkeepings").gameObject;
		this.m_oDataClear = gameObject.transform.FindChild("PanelDataClear").gameObject;
		this.m_cCamera = base.transform.FindChild("Camera").GetComponent<Camera>();
		string text = GameData.MACHINE_ID.ToString();
		string text2 = GameData.XYCLON_VERSION.ToString();
		string text3 = (Singleton<GameManager>.instance.READER_ACTIVE ? "CONNECTED" : "NOT FOUND");
		string text4 = (Singleton<GameManager>.instance.IO_ACTIVE ? "CONNECTED" : "NOT FOUND");
		string text5 = string.Concat(new string[] { text, " | VER: ", text2, "\nREADER: ", text3, " | IO: ", text4 });
		Transform transform = this.m_oMain.transform.Find("MachineId");
		float num = 2f;
		transform.localScale = new Vector3(num, num, 1f);
		transform.GetComponent<UILabel>().text = text5;
		this.ViewDemoSound();
		this.ExtraSettings();
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x00003648 File Offset: 0x00001848
	private void OnBonusIo()
	{
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00068384 File Offset: 0x00066584
	private void ClearPanel()
	{
		this.m_oMain.SetActive(false);
		this.m_oMonitor.SetActive(false);
		this.m_oTouch.SetActive(false);
		this.m_oIO.SetActive(false);
		this.m_oSound.SetActive(false);
		this.m_oGameSetup.SetActive(false);
		this.m_oBookkeepings.SetActive(false);
		this.m_oDataClear.SetActive(false);
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x0000C805 File Offset: 0x0000AA05
	private void PressDemoSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		Singleton<GameManager>.instance.DemoSound = !Singleton<GameManager>.instance.DemoSound;
		this.ViewDemoSound();
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x000683F4 File Offset: 0x000665F4
	private void ViewDemoSound()
	{
		UILabel component = this.m_oGameSetup.transform.FindChild("txtDemoSound").transform.FindChild("txtContent").GetComponent<UILabel>();
		if (Singleton<GameManager>.instance.DemoSound)
		{
			component.text = "ON";
			return;
		}
		component.text = "OFF";
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x0000C831 File Offset: 0x0000AA31
	private void PressMonitor()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oMonitor.SetActive(true);
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x0000C852 File Offset: 0x0000AA52
	private void PressTouch()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oTouch.SetActive(true);
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0000C873 File Offset: 0x0000AA73
	private void PressIO()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oIO.SetActive(true);
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x0000C894 File Offset: 0x0000AA94
	private void PressSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oSound.SetActive(true);
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0000C8B5 File Offset: 0x0000AAB5
	private void PressGameSetup()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oGameSetup.SetActive(true);
		this.SetGameSetup();
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0000C8DC File Offset: 0x0000AADC
	private void PressBookeeping()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oBookkeepings.SetActive(true);
		this.SetBookeeping();
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0000C903 File Offset: 0x0000AB03
	private void PressDataClear()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oDataClear.SetActive(true);
		this.SetDataClear();
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0000C92A File Offset: 0x0000AB2A
	private void PressDataExit()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		Singleton<SceneSwitcher>.instance.LoadNextScene("CopyRight");
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0000C948 File Offset: 0x0000AB48
	private void PressMonitorPanel()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oMain.SetActive(true);
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0000C969 File Offset: 0x0000AB69
	private void PressExitTouchPanel()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		this.ClearPanel();
		this.m_oMain.SetActive(true);
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00068450 File Offset: 0x00066650
	private void PressTestSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		bool flag = ConfigManager.Instance.Get<bool>("misc.touch_login", false);
		flag = !flag;
		ConfigManager.Instance.Set("misc.touch_login", flag);
		this.ExtraSettings();
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0000C994 File Offset: 0x0000AB94
	private void PressGameType()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		if (GameData.E_PLAYTYPE == PLAYTYPE.NORMAL)
		{
			GameData.E_PLAYTYPE = PLAYTYPE.FREEPLAY;
		}
		else
		{
			GameData.E_PLAYTYPE = PLAYTYPE.NORMAL;
		}
		Singleton<GameManager>.instance.SetCredits();
		this.SetGameSetup();
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0006849C File Offset: 0x0006669C
	private void SetGameSetup()
	{
		string text = string.Format("{0} CREDITS(s) = {1} COIN(s)", Singleton<GameManager>.instance.CREDITSET, Singleton<GameManager>.instance.PRICE);
		this.m_oGameSetup.transform.FindChild("txtGameCost").gameObject.transform.FindChild("txtContent").GetComponent<UILabel>().text = text;
		this.m_oGameSetup.transform.FindChild("txtCreditSetting").gameObject.transform.FindChild("txtContent").GetComponent<UILabel>().text = Singleton<GameManager>.instance.CREDITSET.ToString();
		this.m_oGameSetup.transform.FindChild("txtCoinSetting").gameObject.transform.FindChild("txtContent").GetComponent<UILabel>().text = Singleton<GameManager>.instance.PRICE.ToString();
		UILabel component = this.m_oGameSetup.transform.FindChild("txtGameType").gameObject.transform.FindChild("Btn").transform.FindChild("txtContent").GetComponent<UILabel>();
		if (Convert.ToBoolean(GameData.E_PLAYTYPE))
		{
			component.text = PLAYTYPE.FREEPLAY.ToString();
		}
		else
		{
			component.text = PLAYTYPE.NORMAL.ToString();
		}
		bool flag = ConfigManager.Instance.Get<bool>("game.freeplay", false);
		flag = Convert.ToBoolean(GameData.E_PLAYTYPE);
		ConfigManager.Instance.Set("game.freeplay", flag);
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00068644 File Offset: 0x00066844
	private void SetBookeeping()
	{
		string text = string.Format("{0:000000000}", Singleton<GameManager>.instance.TOTALCREDITS);
		this.m_oBookkeepings.transform.FindChild("txtTotalCredits").gameObject.transform.FindChild("txtCount").GetComponent<UILabel>().text = text;
		string text2 = string.Format("{0:000000000}", Singleton<GameManager>.instance.TOTALCOINS);
		this.m_oBookkeepings.transform.FindChild("txtTotalCoins").gameObject.transform.FindChild("txtCount").GetComponent<UILabel>().text = text2;
		string text3 = string.Format("{0:000000000}", Singleton<GameManager>.instance.TOTALSERVICES);
		this.m_oBookkeepings.transform.FindChild("txtTotalService").gameObject.transform.FindChild("txtCount").GetComponent<UILabel>().text = text3;
		this.ViewBookeeping();
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00068744 File Offset: 0x00066944
	private string GetBookKeeping(int iSlot)
	{
		string[] stringArray = PlayerPrefsX.GetStringArray("BOOKKEEPING", string.Empty, 0);
		if (stringArray == null)
		{
			return string.Empty;
		}
		for (int i = 0; i < stringArray.Length; i++)
		{
			DateTime dateTime = DateTime.FromBinary(Convert.ToInt64(stringArray[i]));
			if (i == iSlot)
			{
				return dateTime.ToString();
			}
		}
		return string.Empty;
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00068798 File Offset: 0x00066998
	private void ViewBookeeping()
	{
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = this.m_oBookkeepings.transform.FindChild("BookkeepSlot" + i.ToString()).gameObject;
			string bookKeeping = this.GetBookKeeping(i);
			gameObject.transform.FindChild("txtDate").GetComponent<UILabel>().text = bookKeeping;
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x000687FC File Offset: 0x000669FC
	private void PressCreditDn()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		GameManager instance = Singleton<GameManager>.instance;
		int creditset = instance.CREDITSET;
		instance.CREDITSET = creditset - 1;
		this.SetGameSetup();
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00068830 File Offset: 0x00066A30
	private void PressCreditUp()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		GameManager instance = Singleton<GameManager>.instance;
		int creditset = instance.CREDITSET;
		instance.CREDITSET = creditset + 1;
		this.SetGameSetup();
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x00068864 File Offset: 0x00066A64
	private void PressCoinDn()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		GameManager instance = Singleton<GameManager>.instance;
		int price = instance.PRICE;
		instance.PRICE = price - 1;
		this.SetGameSetup();
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x00068898 File Offset: 0x00066A98
	private void PressCoinUp()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		GameManager instance = Singleton<GameManager>.instance;
		int price = instance.PRICE;
		instance.PRICE = price + 1;
		this.SetGameSetup();
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
	private void SetDataClear()
	{
		this.ViewConfirm(false);
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0000C9D1 File Offset: 0x0000ABD1
	private void ViewConfirm(bool bOn)
	{
		this.m_oDataClear.transform.FindChild("txtConfirm").gameObject.SetActive(bOn);
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
	private void PressAllClear()
	{
		this.ViewConfirm(true);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
	private void PressBookkeepingClear()
	{
		this.ViewConfirm(true);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
	private void PressRankingDataClear()
	{
		this.ViewConfirm(true);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
	private void PressSetDefaultOption()
	{
		this.ViewConfirm(true);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0000CA09 File Offset: 0x0000AC09
	private void PressConfirmYes()
	{
		this.ViewConfirm(false);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		if (this.m_eClear == MainManagermentScript.DATACLEAR_OPTION.BOOKEEPING)
		{
			Singleton<GameManager>.instance.TOTALCOINS = 0;
			Singleton<GameManager>.instance.TOTALCREDITS = 0;
			Singleton<GameManager>.instance.TOTALSERVICES = 0;
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0000CA48 File Offset: 0x0000AC48
	private void PressConfirmNo()
	{
		this.ViewConfirm(false);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x000688CC File Offset: 0x00066ACC
	private void ExtraSettings()
	{
		this.m_oMain.transform.FindChild("BtnSound").transform.FindChild("Label").GetComponent<UILabel>().text = "EXTRA SETTINGS";
		this.m_oSound.transform.FindChild("Title").GetComponent<UILabel>().text = "EXTRA SETTINGS";
		UILabel component = this.m_oSound.transform.FindChild("txtMusicTest").GetComponent<UILabel>();
		if (ConfigManager.Instance.Get<bool>("misc.touch_login", false))
		{
			component.text = "Touch Login: ON";
			return;
		}
		component.text = "Touch Login: OFF";
	}

	// Token: 0x04000FAA RID: 4010
	private Camera m_cCamera;

	// Token: 0x04000FAB RID: 4011
	private GameObject m_oMain;

	// Token: 0x04000FAC RID: 4012
	private GameObject m_oMonitor;

	// Token: 0x04000FAD RID: 4013
	private GameObject m_oTouch;

	// Token: 0x04000FAE RID: 4014
	private GameObject m_oIO;

	// Token: 0x04000FAF RID: 4015
	private GameObject m_oSound;

	// Token: 0x04000FB0 RID: 4016
	private GameObject m_oGameSetup;

	// Token: 0x04000FB1 RID: 4017
	private GameObject m_oBookkeepings;

	// Token: 0x04000FB2 RID: 4018
	private GameObject m_oDataClear;

	// Token: 0x04000FB3 RID: 4019
	private MainManagermentScript.DATACLEAR_OPTION m_eClear;

	// Token: 0x020001F5 RID: 501
	public enum DATACLEAR_OPTION
	{
		// Token: 0x04000FB5 RID: 4021
		BOOKEEPING,
		// Token: 0x04000FB6 RID: 4022
		RANKING,
		// Token: 0x04000FB7 RID: 4023
		DEFAULTOPTION,
		// Token: 0x04000FB8 RID: 4024
		ALLCLEAR
	}

	// Token: 0x020001F6 RID: 502
	public enum MANAGER_STATE
	{
		// Token: 0x04000FBA RID: 4026
		MONITOR,
		// Token: 0x04000FBB RID: 4027
		TOUCH,
		// Token: 0x04000FBC RID: 4028
		IO,
		// Token: 0x04000FBD RID: 4029
		SOUND,
		// Token: 0x04000FBE RID: 4030
		GAMESETUP,
		// Token: 0x04000FBF RID: 4031
		BOOKEEPING,
		// Token: 0x04000FC0 RID: 4032
		DATACLEAR,
		// Token: 0x04000FC1 RID: 4033
		MAIN
	}
}
