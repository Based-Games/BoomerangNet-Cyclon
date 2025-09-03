using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class CardManagerScript : MonoBehaviour
{
	// Token: 0x06000C45 RID: 3141 RVA: 0x00057910 File Offset: 0x00055B10
	private void Awake()
	{
		this.SetObject();
		this.SetAllDeActive();
		Singleton<GameManager>.instance.InitNetData();
		this.userBGM = Singleton<GameManager>.instance.UserData.SetBGM;
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_beathaus", true);
		Singleton<SoundSourceManager>.instance.getNowBGM().time = 0f;
		this.CreateNotice();
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Logging in...", null);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00057984 File Offset: 0x00055B84
	private void Start()
	{
		Singleton<GameManager>.instance.ONLOGIN = false;
		Singleton<GameManager>.instance.PLAYUSER = true;
		CardManager.eject();
		this.ChangeState(CardManagerScript.CARDSTATE.INSERT);
		base.StartCoroutine(this.RefreshReaders());
		base.StartCoroutine(this.UpdateKeyBoard());
		Singleton<GameManager>.instance.InitNetData();
		this.m_sTimer.StartTimer(30, 10);
		this.m_sTimer.CallBackTimeover = new TimerScript.CompleteTimeOver(this.TimeOver);
		Singleton<GameManager>.instance.SetSceneControl(base.gameObject);
		if (!Singleton<GameManager>.instance.ONNETWORK)
		{
			Singleton<GameManager>.instance.CreatePopUp(POPUPTYPE.NETWORK_NOTLOGIN);
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0000B3C4 File Offset: 0x000095C4
	private void SetPause(bool bEnabled)
	{
		base.enabled = !bEnabled;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00057A28 File Offset: 0x00055C28
	private void SetObject()
	{
		this.m_cCamera = base.transform.FindChild("Camera").GetComponent<Camera>();
		this.m_pLogin = base.transform.FindChild("PanelLogin").gameObject;
		this.m_pLoading = base.transform.FindChild("PanelLoading").gameObject;
		this.m_pSuccess = base.transform.FindChild("PanelSuccess").gameObject;
		this.m_pFailed = base.transform.FindChild("PanelFailed").gameObject;
		this.m_sTimer = base.transform.FindChild("Timer").GetComponent<TimerScript>();
		GameObject gameObject = base.transform.FindChild("PanelCommon").gameObject.transform.FindChild("PanelEvent").gameObject;
		this.m_oControlScroll = gameObject.transform.FindChild("ScrollGrid").gameObject;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00057B1C File Offset: 0x00055D1C
	private void CreateNotice()
	{
		ArrayList arrNotice = Singleton<GameManager>.instance.ArrNotice;
		int count = arrNotice.Count;
		float num = 0f;
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.NOTICESLOT);
			gameObject.transform.parent = this.m_oControlScroll.transform;
			float num2 = 150f;
			gameObject.transform.localScale = Vector3.one;
			GameObject gameObject2 = gameObject.transform.FindChild("Mark").gameObject;
			UISprite component = gameObject.transform.FindChild("Back").GetComponent<UISprite>();
			UILabel component2 = gameObject.transform.FindChild("Label").GetComponent<UILabel>();
			UITexture component3 = gameObject.transform.FindChild("Texture").GetComponent<UITexture>();
			if (i < arrNotice.Count)
			{
				NoticeInfo noticeInfo = (NoticeInfo)arrNotice[i];
				gameObject2.SetActive(false);
				if (noticeInfo.eType == NOTICETYPE.EVENT)
				{
					component.spriteName = "Event";
					if (noticeInfo.eSize == NOTICESIZE.BIG)
					{
						num2 = 300f;
						component.spriteName = "EventX2";
					}
				}
				else if (noticeInfo.eType == NOTICETYPE.NOTICE)
				{
					component.spriteName = "Notice";
					if (noticeInfo.eSize == NOTICESIZE.BIG)
					{
						num2 = 300f;
						component.spriteName = "NoticeX2";
					}
				}
				component.MakePixelPerfect();
				if (noticeInfo.eContent == NOTICECONTENT.IMAGE)
				{
					WWWTexture wwwtexture = new WWWTexture();
					wwwtexture.VIEWTEXTURE = component3;
					wwwtexture.strPath = noticeInfo.imagepath;
					Singleton<WWWManager>.instance.AddQueue(wwwtexture);
				}
				else if (noticeInfo.eContent == NOTICECONTENT.TEXT)
				{
					component2.text = "\n" + noticeInfo.title;
					UILabel uilabel = component2;
					uilabel.text = uilabel.text + "\n\n" + noticeInfo.content;
				}
			}
			else
			{
				gameObject2.SetActive(true);
				component3.gameObject.SetActive(false);
				gameObject2.transform.localPosition = new Vector3(0f, num2 * -0.5f, 0f);
				component2.text = string.Empty;
			}
			gameObject.transform.localPosition = new Vector3(0f, num, 0f);
			num -= num2;
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00057D54 File Offset: 0x00055F54
	private void SetAllDeActive()
	{
		if (null != this.m_pLogin)
		{
			this.m_pLogin.SetActive(false);
		}
		if (null != this.m_pLoading)
		{
			this.m_pLoading.SetActive(false);
		}
		if (null != this.m_pSuccess)
		{
			this.m_pSuccess.SetActive(false);
		}
		if (null != this.m_pFailed)
		{
			this.m_pFailed.SetActive(false);
		}
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0000B3D0 File Offset: 0x000095D0
	private void TimeOver()
	{
		this.m_bTimeOver = true;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0000B3D9 File Offset: 0x000095D9
	private void CompleLogIn()
	{
		this.ChangeState(CardManagerScript.CARDSTATE.COMPLETE);
		Singleton<WWWManager>.instance.CallBackLogin();
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0000B3EC File Offset: 0x000095EC
	private void WaitNextStep()
	{
		this.ChangeState(CardManagerScript.CARDSTATE.COMPLETE);
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00057DCC File Offset: 0x00055FCC
	private void CompleteFrontLogin()
	{
		if (!Singleton<GameManager>.instance.cardSuccess)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_FAIL, false);
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			if (userData.Language == "EN")
			{
				Singleton<GameManager>.instance.CreateCustomPopUp("Your card isn't registered!\n Please register at https://beathaus.co.kr/user/register\nYour card's ID: " + this.readableCard(this.cardID) + "\nDo not enter any spaces.");
			}
			else if (userData.Language == "KR")
			{
				Singleton<GameManager>.instance.CreateCustomPopUp("카드가 등록되어 있지 않습니다!\nhttps://beathaus.co.kr/user/register 에서 등록해주세요\n카드 ID: " + this.readableCard(this.cardID) + "\n공백을 입력하지 마세요.");
			}
			Singleton<GameManager>.instance.ONLOGIN = false;
			Singleton<GameManager>.instance.PLAYUSER = true;
			this.m_bCompleteLoad = false;
			CardManager.eject();
			this.ChangeState(CardManagerScript.CARDSTATE.INSERT);
			this.m_sTimer.StartTimer(30, 10);
			this.m_sTimer.CallBackTimeover = new TimerScript.CompleteTimeOver(this.TimeOver);
			return;
		}
		WWWGetUserData wwwgetUserData = new WWWGetUserData();
		wwwgetUserData.CallBack = new WWWObject.CompleteCallBack(this.CompleLogIn);
		Singleton<WWWManager>.instance.AddQueue(wwwgetUserData);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0000B3F5 File Offset: 0x000095F5
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		this.UpdateTimeOver();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00057EE4 File Offset: 0x000560E4
	private void UpdateTimeOver()
	{
		if (this.m_bTimeOver)
		{
			if (this.m_eCard == CardManagerScript.CARDSTATE.CONNECTION)
			{
				this.m_fOverTime += Time.deltaTime;
				if (5f < this.m_fOverTime)
				{
					this.ChangeState(CardManagerScript.CARDSTATE.FAILED);
					return;
				}
			}
			else if (this.m_eCard == CardManagerScript.CARDSTATE.COMPLETE)
			{
				this.PressStart();
				return;
			}
			this.BackToDemo();
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0000B40F File Offset: 0x0000960F
	private void NextScene()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		Singleton<SceneSwitcher>.instance.LoadNextScene("ModeSelect");
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00057F40 File Offset: 0x00056140
	private void UpdataCard()
	{
		if (!CardManagerR1.m_bInit)
		{
			return;
		}
		if (!CardManager.isInsertCard())
		{
			return;
		}
		this.ChangeState(CardManagerScript.CARDSTATE.CONNECTION);
		this.m_fTime = 0f;
		if (CardManager.loadBase())
		{
			this.cardID = CardManagerR1.m_strId;
			this.Login();
			this.m_bCompleteLoad = true;
			return;
		}
		this.ChangeState(CardManagerScript.CARDSTATE.FAILED);
		CardManager.eject();
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00057F9C File Offset: 0x0005619C
	private void ChangeState(CardManagerScript.CARDSTATE eState)
	{
		if (this.m_eCard == eState)
		{
			return;
		}
		this.m_eCard = eState;
		this.SetAllDeActive();
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		switch (eState)
		{
		case CardManagerScript.CARDSTATE.INSERT:
			if (this.m_pLogin)
			{
				this.m_pLogin.SetActive(true);
				if (userData.Language == "EN")
				{
					this.m_pLogin.transform.FindChild("txtInsertCard").GetComponent<UISprite>().spriteName = "txtIdInsertEn";
					this.m_pLogin.transform.FindChild("txtNoIdCard").GetComponent<UISprite>().spriteName = "txtNoCardEn";
				}
				else if (userData.Language == "KR")
				{
					this.m_pLogin.transform.FindChild("txtInsertCard").GetComponent<UISprite>().spriteName = "txtIdInsert";
					this.m_pLogin.transform.FindChild("txtNoIdCard").GetComponent<UISprite>().spriteName = "txtNoCard";
				}
				this.m_pLogin.transform.FindChild("txtInsertCard").GetComponent<UISprite>().MakePixelPerfect();
				this.m_pLogin.transform.FindChild("txtNoIdCard").GetComponent<UISprite>().MakePixelPerfect();
				return;
			}
			break;
		case CardManagerScript.CARDSTATE.CONNECTION:
			if (this.m_pLoading)
			{
				this.m_pLoading.SetActive(true);
				if (userData.Language == "EN")
				{
					this.m_pLoading.transform.FindChild("txtLoading").GetComponent<UISprite>().spriteName = "txtLoadingEn";
					return;
				}
				if (userData.Language == "KR")
				{
					this.m_pLoading.transform.FindChild("txtLoading").GetComponent<UISprite>().spriteName = "txtLoading";
				}
				return;
			}
			break;
		case CardManagerScript.CARDSTATE.FAILED:
			if (this.m_pFailed)
			{
				this.m_pFailed.SetActive(true);
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_FAIL, false);
				if (userData.Language == "EN")
				{
					this.m_pFailed.transform.FindChild("txtFail").GetComponent<UISprite>().spriteName = "txtFailEn";
					return;
				}
				if (userData.Language == "KR")
				{
					this.m_pFailed.transform.FindChild("txtFail").GetComponent<UISprite>().spriteName = "txtFail";
				}
				return;
			}
			break;
		case CardManagerScript.CARDSTATE.COMPLETE:
			if (this.m_pSuccess)
			{
				base.StartCoroutine(this.switchBGM());
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_SUCCESS, false);
				this.m_pSuccess.SetActive(true);
				if (userData.Language == "EN")
				{
					this.m_pSuccess.transform.FindChild("txtSuccess").GetComponent<UISprite>().spriteName = "txtSuccesEn";
					return;
				}
				if (userData.Language == "KR")
				{
					this.m_pSuccess.transform.FindChild("txtSuccess").GetComponent<UISprite>().spriteName = "txtSucces";
				}
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000582BC File Offset: 0x000564BC
	public void PressStart()
	{
		GameManager instance = Singleton<GameManager>.instance;
		int credit = instance.CREDIT;
		instance.CREDIT = credit - 1;
		Singleton<GameManager>.instance.isAllSongMode = true;
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
		Singleton<SceneSwitcher>.instance.LoadNextScene("ModeSelect");
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x0000B42D File Offset: 0x0000962D
	public void PressBack()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_COMMON_BACK, false);
		this.ChangeState(CardManagerScript.CARDSTATE.INSERT);
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00058304 File Offset: 0x00056504
	public void PressNoIdCard()
	{
		Singleton<GameManager>.instance.isAllSongMode = true;
		GameManager instance = Singleton<GameManager>.instance;
		int credit = instance.CREDIT;
		instance.CREDIT = credit - 1;
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_COMMON_BACK, false);
		Singleton<SceneSwitcher>.instance.LoadNextScene("ModeSelect");
		WWWNoCardLogIn wwwnoCardLogIn = new WWWNoCardLogIn();
		wwwnoCardLogIn.CallBack = new WWWObject.CompleteCallBack(Singleton<WWWManager>.instance.CallBackLogin);
		Singleton<WWWManager>.instance.AddQueue(wwwnoCardLogIn);
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Logged in as Guest", "Waiting to start...");
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0000B443 File Offset: 0x00009643
	private void TestKeyLogin()
	{
		this.ChangeState(CardManagerScript.CARDSTATE.CONNECTION);
		this.m_fTime = 0f;
		this.cardID = ConfigManager.Instance.Get<string>("network.local_card", true);
		this.Login();
		this.m_bCompleteLoad = true;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0000B47A File Offset: 0x0000967A
	private IEnumerator switchBGM()
	{
		if (this.userBGM != Singleton<GameManager>.instance.UserData.SetBGM)
		{
			while (Singleton<SoundSourceManager>.instance.getNowBGM().volume > 0f)
			{
				Singleton<SoundSourceManager>.instance.getNowBGM().volume = Singleton<SoundSourceManager>.instance.getNowBGM().volume - Time.deltaTime;
			}
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SoundSourceManager>.instance.SetBgmTime(0f);
			Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_beathaus", true);
			while (Singleton<SoundSourceManager>.instance.getNowBGM().volume < 1f)
			{
				Singleton<SoundSourceManager>.instance.getNowBGM().volume = Singleton<SoundSourceManager>.instance.getNowBGM().volume + Time.deltaTime;
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00058388 File Offset: 0x00056588
	public void Login()
	{
		WWWLogIn wwwlogIn = new WWWLogIn();
		wwwlogIn.CardId = this.cardID;
		wwwlogIn.CallBack = new WWWObject.CompleteCallBack(this.CompleteFrontLogin);
		Singleton<WWWManager>.instance.AddQueue(wwwlogIn);
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x000583C4 File Offset: 0x000565C4
	private string readableCard(string input)
	{
		string text = "";
		for (int i = 0; i < input.Length; i += 4)
		{
			int num = Math.Min(4, input.Length - i);
			string text2 = input.Substring(i, num);
			text += text2;
			text += " ";
		}
		return text;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0000B489 File Offset: 0x00009689
	private IEnumerator RefreshReaders()
	{
		while (!this.m_bCompleteLoad)
		{
			this.UpdataCard();
			yield return new WaitForSeconds(0f);
		}
		yield break;
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0000B498 File Offset: 0x00009698
	private IEnumerator UpdateKeyBoard()
	{
		for (;;)
		{
			if (this.m_eCard == CardManagerScript.CARDSTATE.INSERT || this.m_eCard == CardManagerScript.CARDSTATE.FAILED)
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					this.TestKeyLogin();
				}
				if (ConfigManager.Instance.Get<bool>("misc.touch_login", false) && Input.GetKeyDown(KeyCode.Mouse0))
				{
					this.TestKeyLogin();
				}
			}
			yield return new WaitForSeconds(0f);
		}
		yield break;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0000B4A7 File Offset: 0x000096A7
	public void BackToDemo()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_UNMOUNT, false);
		Singleton<SceneSwitcher>.instance.LoadNextScene("CopyRight");
	}

	// Token: 0x04000C46 RID: 3142
	private const int CNT_BASESLOT = 4;

	// Token: 0x04000C47 RID: 3143
	private const float MAX_LOGOUTTIME = 5f;

	// Token: 0x04000C48 RID: 3144
	private const float FIRST_HEIGHT = 282f;

	// Token: 0x04000C49 RID: 3145
	private const float NOTICEHEIGHT = 150f;

	// Token: 0x04000C4A RID: 3146
	private const float DOUBLEHEIGHT = 300f;

	// Token: 0x04000C4B RID: 3147
	private Camera m_cCamera;

	// Token: 0x04000C4C RID: 3148
	private bool m_bCompleteLoad;

	// Token: 0x04000C4D RID: 3149
	private float m_fTime;

	// Token: 0x04000C4E RID: 3150
	private GameObject m_pLogin;

	// Token: 0x04000C4F RID: 3151
	private GameObject m_pLoading;

	// Token: 0x04000C50 RID: 3152
	private GameObject m_pSuccess;

	// Token: 0x04000C51 RID: 3153
	private GameObject m_pFailed;

	// Token: 0x04000C52 RID: 3154
	private GameObject m_oControlScroll;

	// Token: 0x04000C53 RID: 3155
	private bool m_bTimeOver;

	// Token: 0x04000C54 RID: 3156
	private CardManagerScript.CARDSTATE m_eCard;

	// Token: 0x04000C55 RID: 3157
	private float m_fOverTime;

	// Token: 0x04000C56 RID: 3158
	private Color ColorEventLable = new Color(0.49411765f, 0.7647059f, 0.99607843f);

	// Token: 0x04000C57 RID: 3159
	private Color ColorNoticeLable = new Color(1f, 1f, 1f);

	// Token: 0x04000C58 RID: 3160
	public GameObject NOTICESLOT;

	// Token: 0x04000C59 RID: 3161
	private TimerScript m_sTimer;

	// Token: 0x04000C5A RID: 3162
	private string userBGM;

	// Token: 0x04000C5B RID: 3163
	private string cardID;

	// Token: 0x0200019D RID: 413
	private enum CARDSTATE
	{
		// Token: 0x04000C5D RID: 3165
		NONE,
		// Token: 0x04000C5E RID: 3166
		INSERT,
		// Token: 0x04000C5F RID: 3167
		CONNECTION,
		// Token: 0x04000C60 RID: 3168
		FAILED,
		// Token: 0x04000C61 RID: 3169
		COMPLETE
	}
}
