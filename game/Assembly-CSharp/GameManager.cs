using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using IoThread;
using MiniJSON;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class GameManager : Singleton<GameManager>
{
	// Token: 0x06000969 RID: 2409
	public ArrayList getHausModeRank()
	{
		WWWModeRanking wwwmodeRanking = new WWWModeRanking();
		Singleton<WWWManager>.instance.AddQueue(wwwmodeRanking);
		return this.HausModeRank;
	}

	// Token: 0x0600096A RID: 2410
	public ArrayList getRaveModeRank()
	{
		this.RaveModeRank.Clear();
		for (int i = 1; i <= 30; i++)
		{
			RankInfo rankInfo = new RankInfo
			{
				Name = "DJ CYCLON",
				Level = 0,
				Score = 0,
				RankClass = GRADE.NON,
				Icon = 0,
				Ranking = i
			};
			rankInfo.aInfo = new AlbumInfo();
			rankInfo.aInfo.Id = 0;
			for (int j = 0; j < 4; j++)
			{
				RaveUpStage raveUpStage = new RaveUpStage();
				raveUpStage.iSong = 1;
				raveUpStage.PtType = PTLEVEL.EZ;
				rankInfo.ArrRaveInfo.Add(raveUpStage);
			}
			this.RaveModeRank.Add(rankInfo);
		}
		return this.RaveModeRank;
	}

	// Token: 0x0600096B RID: 2411
	private void Awake()
	{
		new GameObject("Bootstrap").AddComponent<Bootstrap>();
		if (ConfigManager.Instance.Get<bool>("game.freeplay", false))
		{
			GameData.E_PLAYTYPE = PLAYTYPE.FREEPLAY;
		}
		this.LoadSetInfo();
		this.m_oInfo = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load("Common/RootInfo"));
		this.m_oInfo.transform.parent = base.transform;
		this.m_oInfo.transform.localEulerAngles = Vector3.zero;
		this.m_cCamera = this.m_oInfo.transform.FindChild("Camera").GetComponent<Camera>();
		this.m_oFront = this.m_oInfo.transform.FindChild("FrontPanel").gameObject;
		this.m_oControlLtInfo = this.m_oFront.transform.FindChild("LtInfo").gameObject;
		this.m_oControlPopUp = this.m_oFront.transform.FindChild("ControlPopUp").gameObject;
		this.m_oControlPopUp.SetActive(false);
		this.m_tCredists = this.m_oFront.transform.FindChild("txtCredits").GetComponent<UILabel>();
		this.m_sIdCard = this.m_oControlLtInfo.transform.FindChild("idcard").GetComponent<UISprite>();
		this.m_sNetWork = this.m_oControlLtInfo.transform.FindChild("net").GetComponent<UISprite>();
		this.m_sCheck = this.m_oControlLtInfo.transform.FindChild("Check").GetComponent<UISprite>();
		this.m_sPoint = this.m_oControlLtInfo.transform.FindChild("Point").GetComponent<UISprite>();
		this.m_sPoint.gameObject.SetActive(false);
		this.m_oControlTestView = this.m_oFront.transform.FindChild("TestView").gameObject;
		this.m_oControlTestView.SetActive(false);
		this.TOUCH = (GameObject)Resources.Load("Common/Touch");
		this.ONLOGIN = false;
		this.UPDATE = false;
		this.ONCHECK = false;
		this.ONNETWORK = false;
		Application.targetFrameRate = 60;
		for (int i = 0; i < 4; i++)
		{
			this.TotalResult[i].Init();
			this.TotalResult[i].SetDiscInfo();
		}
		this.ContinueData.Init();
		this.ContinueData.SetDiscInfo();
		this.ResultData = this.TotalResult[0];
		this.InitNetData();
		UICamera.onCustomInput = (UICamera.OnCustomInput)Delegate.Combine(UICamera.onCustomInput, new UICamera.OnCustomInput(this.PressNguiTouch));
		this.SetStandAlone();
		this.SetCredits();
		base.StartCoroutine(this.PingGoogle());
		WWWModeRanking wwwmodeRanking = new WWWModeRanking();
		Singleton<WWWManager>.instance.AddQueue(wwwmodeRanking);
		Singleton<DiscordRichPresenceController>.instance.InitializeDiscordRPC();
	}

	// Token: 0x0600096C RID: 2412
	private void Start()
	{
	}

	// Token: 0x0600096D RID: 2413
	private IEnumerator PingGoogle()
	{
		for (;;)
		{
			WWWNotice wwwnotice = new WWWNotice();
			wwwnotice.CallBackFail = new WWWObject.CompleteCallBack(Singleton<WWWManager>.instance.CallBackNotNetwork);
			Singleton<WWWManager>.instance.AddQueue(wwwnotice);
			float num = 60f;
			if (!this.ONNETWORK)
			{
				num = 1f;
				if (Singleton<GameManager>.instance.inAttract())
				{
					this.CreatePopUp(POPUPTYPE.NETWORK_NOTCONNECT);
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_START_NOREADY, false);
					Singleton<SceneSwitcher>.instance.LoadNextScene("CopyRight");
				}
			}
			else if (this.inAttract())
			{
				this.ClosePopUp();
			}
			yield return new WaitForSeconds(num);
		}
		yield break;
	}

	// Token: 0x0600096E RID: 2414
	private void SetStandAlone()
	{
		GameData.FLIP = true;
		string text = ConfigManager.Instance.Get<string>("game.input", false);
		if (text.Equals("real_io") || text.Equals("touch"))
		{
			Screen.lockCursor = false;
			Screen.showCursor = false;
		}
		NewMultiTouch.OnStart();
		iPhoneToMouse.instance.Arcade = true;
		CardManager.Init();
		base.StartCoroutine(this.LoadComplete());
		IoRun.OpenSerial();
		IoRun.CallBackBonus = new IoRun.Complete(this.CallBackBonus);
		IoRun.CallBackCoin = new IoRun.Complete(this.CallBackCoin);
		IoRun.CallBackManagemet = new IoRun.Complete(this.CallBackManagement);
	}

	// Token: 0x0600096F RID: 2415
	public void SetCredits()
	{
		string text = string.Format("CREDIT(S) {0} ({1}/{2})", Singleton<GameManager>.instance.CREDIT.ToString(), Singleton<GameManager>.instance.COIN.ToString(), Singleton<GameManager>.instance.PRICE.ToString());
		if (GameData.E_PLAYTYPE == PLAYTYPE.FREEPLAY)
		{
			text = PLAYTYPE.FREEPLAY.ToString();
		}
		this.m_tCredists.text = text;
	}

	// Token: 0x06000970 RID: 2416
	private IEnumerator LoadComplete()
	{
		while (!NewMultiTouch.OnisDllStart())
		{
			yield return new WaitForSeconds(2f);
		}
		this.CheckTouchFirmware();
		yield break;
	}

	// Token: 0x06000971 RID: 2417
	private void CheckTouchFirmware()
	{
		int num4 = 0;
		int num2 = 0;
		int num3 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (NewMultiTouch.OnDeviceInfo(num4, stringBuilder, stringBuilder2, out num2, out num3) == 1)
		{
			stringBuilder.ToString().Contains("TSR220A");
		}
	}

	// Token: 0x06000972 RID: 2418
	public void CreateTouch(Vector3 vPos)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.TOUCH);
		vPos.z = 0f;
		gameObject.transform.position = vPos;
		gameObject.transform.localScale = this.m_oInfo.transform.localScale;
		UnityEngine.Object.Destroy(gameObject, 2f);
	}

	// Token: 0x06000973 RID: 2419
	public Vector3 GetTouchToPos(TouchInfo pPoint)
	{
		if (ConfigManager.Instance.Get<string>("game.input", false).Equals("real_io"))
		{
			Vector3 zero = Vector3.zero;
			zero.x = (float)pPoint.Point.x / NewMultiTouch.MONITOR_WIDTH;
			zero.y = (float)pPoint.Point.y / NewMultiTouch.MONITOR_HEIGHT;
			zero.x *= (float)Screen.width;
			zero.y *= (float)Screen.height;
			if (GameData.FLIP)
			{
				zero.x = (float)Screen.width - zero.x;
			}
			zero.z = 0f;
			return zero;
		}
		Vector3 zero2 = Vector3.zero;
		zero2.x = (float)(pPoint.Point.x + (Screen.width - 1280));
		zero2.y = (float)(pPoint.Point.y + (Screen.height - 720));
		if (GameData.FLIP)
		{
			zero2.x = (float)Screen.width - zero2.x;
		}
		return zero2;
	}

	// Token: 0x06000974 RID: 2420
	public void InitNetData()
	{
		this.UserData.Init();
		this.netHouseMixData.Init();
		this.netHouMixResultData.Init();
		this.netHouMixTotalResultData.Init();
		this.netUserRecordData.Init();
		this.netRaveUpRankData.Init();
		this.netRaveUpResultData.Init();
		this.RewardState = MISSIONREWARD_STATE.NONE;
		if (this.inAttract())
		{
			CardManager.eject();
			this.ONLOGIN = false;
			this.PLAYUSER = false;
			Singleton<SongManager>.instance.Mode = GAMEMODE.HAUSMIX;
			Singleton<DiscordRichPresenceController>.instance.SetInitPresence();
		}
		Singleton<SongManager>.instance.LoadHouseStage();
	}

	// Token: 0x06000975 RID: 2421
	private void Update()
	{
		this.PressBonus();
		this.PressCoin();
		this.PressManagement();
		this.UpdateInput();
		this.UpdateKeyBoard();
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000976 RID: 2422
	public int COIN
	{
		get
		{
			return this.m_iCoin;
		}
	}

	// Token: 0x06000977 RID: 2423
	public void IncCoin()
	{
		this.m_iCoin++;
		int totalcoins = this.TOTALCOINS;
		this.TOTALCOINS = totalcoins + 1;
		if (this.PRICE <= this.m_iCoin)
		{
			this.m_iCoin -= this.PRICE;
			this.CREDIT += this.CREDITSET;
			this.TOTALCREDITS += this.CREDITSET;
		}
		this.SetCredits();
		if (this.HaveCredit() && this.inAttract() && this.ONNETWORK)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_COIN_FINISH, false);
			Singleton<SceneSwitcher>.instance.LoadNextScene("Title");
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_COIN_IN, false);
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000978 RID: 2424
	// (set) Token: 0x06000979 RID: 2425
	public int POINT
	{
		get
		{
			return this.m_iPoint;
		}
		set
		{
			this.m_iPoint = value;
		}
	}

	// Token: 0x0600097A RID: 2426
	public bool CanPlayCheck()
	{
		if (this.ONNETWORK)
		{
			if (this.POINT <= 0)
			{
				Singleton<GameManager>.instance.CreatePopUp(POPUPTYPE.REFILL_POINT);
				Singleton<WWWManager>.instance.GetGameCenterPoint();
				return false;
			}
			return true;
		}
		else
		{
			if (GameData.CheckSystem != CHECKSYSTEM.NONE)
			{
				Singleton<GameManager>.instance.CreatePopUp(POPUPTYPE.CHECK_SYSTEM);
				Singleton<WWWManager>.instance.GetEmergencyCheck();
				return false;
			}
			Singleton<GameManager>.instance.CreatePopUp(POPUPTYPE.NETWORK_NOTCONNECT);
			return false;
		}
	}

	// Token: 0x0600097B RID: 2427
	public void IncService()
	{
		int num = this.CREDIT;
		this.CREDIT = num + 1;
		num = this.TOTALSERVICES;
		this.TOTALSERVICES = num + 1;
		this.SetCredits();
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x0600097C RID: 2428
	// (set) Token: 0x0600097D RID: 2429
	public bool DEMOPLAY
	{
		get
		{
			return this.m_bDemo;
		}
		set
		{
			this.m_bDemo = value;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x0600097E RID: 2430
	public bool HaveCoin
	{
		get
		{
			return GameData.E_PLAYTYPE == PLAYTYPE.FREEPLAY || 0 < this.CREDIT || 0 < this.COIN;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x0600097F RID: 2431
	// (set) Token: 0x06000980 RID: 2432
	public int CREDIT
	{
		get
		{
			return this.m_iCredit;
		}
		set
		{
			this.m_iCredit = value;
			if (0 > this.m_iCredit)
			{
				this.m_iCredit = 0;
			}
			this.SetCredits();
		}
	}

	// Token: 0x06000981 RID: 2433
	private void LoadSetInfo()
	{
		GameData.INCOMETEST = ConfigManager.Instance.Get<bool>("cyclon.inCom", false);
		base.StartCoroutine(this.LoadUrl());
	}

	// Token: 0x06000982 RID: 2434
	private IEnumerator LoadUrl()
	{
		yield return null;
		Singleton<WWWManager>.instance.SetUrl();
		yield break;
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000983 RID: 2435
	// (set) Token: 0x06000984 RID: 2436
	public int CREDITSET
	{
		get
		{
			return ConfigManager.Instance.Get<int>("game.credit_set", false);
		}
		set
		{
			if (1 > value)
			{
				value = 1;
			}
			ConfigManager.Instance.Set("game.credit_set", value);
			this.SetCredits();
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000985 RID: 2437
	// (set) Token: 0x06000986 RID: 2438
	public int TOTALCREDITS
	{
		get
		{
			return ConfigManager.Instance.Get<int>("game.total_credits", false);
		}
		set
		{
			ConfigManager.Instance.Set("game.total_credits", value);
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000987 RID: 2439
	// (set) Token: 0x06000988 RID: 2440
	public int TOTALCOINS
	{
		get
		{
			return ConfigManager.Instance.Get<int>("game.total_coins", false);
		}
		set
		{
			ConfigManager.Instance.Set("game.total_coins", value);
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000989 RID: 2441
	// (set) Token: 0x0600098A RID: 2442
	public int TOTALSERVICES
	{
		get
		{
			return ConfigManager.Instance.Get<int>("game.service_credits", false);
		}
		set
		{
			ConfigManager.Instance.Set("game.service_credits", value);
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x0600098B RID: 2443
	// (set) Token: 0x0600098C RID: 2444
	public bool DemoSound
	{
		get
		{
			return ConfigManager.Instance.Get<bool>("game.demo_sound", false);
		}
		set
		{
			ConfigManager.Instance.Set("game.demo_sound", value);
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x0600098D RID: 2445
	// (set) Token: 0x0600098E RID: 2446
	public int PRICE
	{
		get
		{
			return ConfigManager.Instance.Get<int>("game.price", false);
		}
		set
		{
			ConfigManager.Instance.Set("game.price", value);
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x0600098F RID: 2447
	// (set) Token: 0x06000990 RID: 2448
	public bool ONLOGIN
	{
		get
		{
			return this.m_bOnLogin;
		}
		set
		{
			this.m_bOnLogin = value;
			if (this.m_bOnLogin)
			{
				this.m_sIdCard.spriteName = "IdCard";
			}
			else
			{
				this.m_sIdCard.spriteName = "OffIdCard";
			}
			this.m_sIdCard.MakePixelPerfect();
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000991 RID: 2449
	// (set) Token: 0x06000992 RID: 2450
	public bool ONNETWORK
	{
		get
		{
			return this.m_bOnNetWork;
		}
		set
		{
			if (value)
			{
				if (!this.m_bOnNetWork && (this.m_eCurPopUp == POPUPTYPE.CHECK_SYSTEM || this.m_eCurPopUp == POPUPTYPE.NETWORK_NOTCONNECT || this.m_eCurPopUp == POPUPTYPE.NETWORK_NOTLOGIN || this.m_eCurPopUp == POPUPTYPE.NETWORK_NOTSAVEDATA) && this.inAttract())
				{
					this.ClosePopUp();
				}
				this.m_sNetWork.spriteName = "Network";
				this.m_sNetWork.MakePixelPerfect();
			}
			else
			{
				this.m_sNetWork.spriteName = "OffNetWork";
				this.m_sNetWork.MakePixelPerfect();
			}
			this.m_bOnNetWork = value;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000993 RID: 2451
	// (set) Token: 0x06000994 RID: 2452
	public bool ONCHECK
	{
		get
		{
			return this.m_bOnCheck;
		}
		set
		{
			this.m_bOnCheck = value;
			if (this.m_bOnCheck)
			{
				this.m_sCheck.gameObject.SetActive(true);
				this.m_sCheck.spriteName = "Check";
				return;
			}
			this.m_sCheck.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000995 RID: 2453
	private void CallBackBonus()
	{
		GameData.m_bOnPressBonus = true;
	}

	// Token: 0x06000996 RID: 2454
	private void CallBackCoin()
	{
		GameData.m_bOnPressCoin = true;
	}

	// Token: 0x06000997 RID: 2455
	private void CallBackManagement()
	{
		GameData.m_bOnPressManagement = true;
	}

	// Token: 0x06000998 RID: 2456
	public void PressBonus()
	{
		if (GameData.m_bOnPressBonus)
		{
			this.m_iService++;
			this.IncService();
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_COIN_IN, false);
			GameData.m_bOnPressBonus = false;
			this.SetBookkeepingCoin();
		}
	}

	// Token: 0x06000999 RID: 2457
	private void SetBookkeepingCoin()
	{
		string[] stringArray = PlayerPrefsX.GetStringArray("BOOKKEEPING", string.Empty, 0);
		string text = DateTime.Now.ToBinary().ToString();
		string[] array;
		if (stringArray != null)
		{
			int num = stringArray.Length;
			array = new string[num + 1];
			for (int i = 0; i < stringArray.Length; i++)
			{
				array[i] = stringArray[i];
			}
			array[num] = text;
		}
		else
		{
			array = new string[] { text };
		}
		PlayerPrefsX.SetStringArray("BOOKKEEPING", array);
	}

	// Token: 0x0600099A RID: 2458
	public void PressCoin()
	{
		if (GameData.m_bOnPressCoin)
		{
			this.IncCoin();
			GameData.m_bOnPressCoin = false;
			this.SetBookkeepingCoin();
		}
	}

	// Token: 0x0600099B RID: 2459
	public void PressManagement()
	{
		if (GameData.m_bOnPressManagement)
		{
			if ("MainManagement" != Application.loadedLevelName)
			{
				Singleton<WWWManager>.instance.Cancel();
				Singleton<SceneSwitcher>.instance.LoadNextScene("MainManagement");
			}
			GameData.m_bOnPressManagement = false;
		}
	}

	// Token: 0x0600099C RID: 2460
	public void ActivieLed(LEDSTATE eState)
	{
		byte[] array = new byte[0];
		switch (eState)
		{
		case LEDSTATE.WING_NOCOIN:
			array = new byte[] { 195, 90, 165, 64 };
			break;
		case LEDSTATE.WING_OUTGAME:
			array = new byte[] { 195, 90, 165, 65 };
			break;
		case LEDSTATE.WING_INGAME:
			array = new byte[] { 195, 90, 165, 66 };
			break;
		case LEDSTATE.WING_ONPOWER:
			array = new byte[] { 195, 90, 165, 67 };
			break;
		case LEDSTATE.ONLINE_ON:
			array = new byte[] { 195, 90, 165, 49 };
			break;
		case LEDSTATE.ONLINE_Off:
			array = new byte[] { 195, 90, 165, 50 };
			break;
		case LEDSTATE.CARD_OFF:
			array = new byte[] { 195, 90, 165, 104 };
			break;
		case LEDSTATE.CARD_ON:
			array = new byte[] { 195, 90, 165, 105 };
			break;
		}
		IoRun.AddBuffer(array);
	}

	// Token: 0x0600099D RID: 2461
	public void DestroyObject(GameObject obj)
	{
		UnityEngine.Object.Destroy(obj);
	}

	// Token: 0x0600099E RID: 2462
	public void SetView(GameObject objControl, bool bOn)
	{
		foreach (Transform transform in objControl.GetComponentsInChildren<Transform>())
		{
			if (transform.gameObject.renderer)
			{
				transform.gameObject.renderer.enabled = bOn;
			}
			if (transform.gameObject.collider)
			{
				transform.gameObject.collider.enabled = bOn;
			}
		}
	}

	// Token: 0x0600099F RID: 2463
	public float GetAngle(Vector2 lineOneEnd, Vector2 lineTwoEnd)
	{
		float num6 = lineOneEnd[0] - lineTwoEnd[0];
		float num2 = lineOneEnd[1] - lineTwoEnd[1];
		float num3 = lineTwoEnd[0] - lineTwoEnd[0];
		float num4 = lineTwoEnd[1] - lineTwoEnd[1];
		float num7 = Mathf.Atan2(num6, num2);
		float num5 = Mathf.Atan2(num3, num4);
		return (num7 - num5) * 180f / 3.1415927f;
	}

	// Token: 0x060009A0 RID: 2464
	public void InitResult()
	{
		int stage = GameData.Stage;
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			if (stage == 0)
			{
				for (int i = 0; i < 3; i++)
				{
					this.TotalResult[i].Init();
				}
			}
			HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[stage];
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(houseStage.iSong);
			this.ResultData = this.TotalResult[stage];
			this.ResultData.PTTYPE = houseStage.PtType;
			this.ResultData.DISCINFO = discInfo;
			return;
		}
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			if (stage == 0)
			{
				for (int j = 0; j < 4; j++)
				{
					this.TotalResult[j].Init();
					RaveUpStage raveUpStage = Singleton<SongManager>.instance.RaveUpSelectSong[j];
					DiscInfo discInfo2 = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage.iSong);
					this.TotalResult[j].PTTYPE = raveUpStage.PtType;
					this.TotalResult[j].DISCINFO = discInfo2;
				}
				this.ContinueData.Init();
				this.ResultData = this.ContinueData;
			}
			RaveUpStage raveUpStage2 = Singleton<SongManager>.instance.RaveUpSelectSong[stage];
			DiscInfo discInfo3 = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage2.iSong);
			this.ResultData.PTTYPE = raveUpStage2.PtType;
			this.ResultData.DISCINFO = discInfo3;
			return;
		}
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			if (stage == 0)
			{
				for (int k = 0; k < 3; k++)
				{
					this.TotalResult[k].Init();
					RaveUpStage raveUpStage3 = Singleton<SongManager>.instance.RaveUpSelectSong[k];
					DiscInfo discInfo4 = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage3.iSong);
					this.TotalResult[k].PTTYPE = raveUpStage3.PtType;
					this.TotalResult[k].DISCINFO = discInfo4;
				}
				this.ContinueData.Init();
				this.ResultData = this.ContinueData;
			}
			MissionData mission = Singleton<SongManager>.instance.Mission;
			DiscInfo discInfo5 = Singleton<SongManager>.instance.GetDiscInfo(mission.Song[stage]);
			this.ResultData.PTTYPE = mission.Pattern[stage];
			this.ResultData.DISCINFO = discInfo5;
		}
	}

	// Token: 0x060009A1 RID: 2465
	public RESULTDATA GetStageResult(int iStage)
	{
		return this.TotalResult[iStage];
	}

	// Token: 0x060009A2 RID: 2466
	public void LoadEyeCatch(EYECATCHTYPE eType, DiscInfo dInfo, UITexture uTexture, AlbumInfo aInfo = null, MissionPackData mInfo = null)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		switch (eType)
		{
		case EYECATCHTYPE.DISC_1280:
			dInfo = Singleton<SongManager>.instance.GetCurrentDisc();
			text2 = dInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "SongEyeCatch/" + text2 + ".";
			break;
		case EYECATCHTYPE.DISC_145:
			text2 = dInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "SongDiscSmall/" + text2 + ".";
			break;
		case EYECATCHTYPE.SONG_500:
			text2 = dInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "SongDiscBig/" + text2 + ".";
			break;
		case EYECATCHTYPE.CD_96:
			text2 = dInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "SongCdSmall/" + text2 + ".";
			break;
		case EYECATCHTYPE.CD_500:
			text2 = dInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "SongCdBig/" + text2 + ".";
			break;
		case EYECATCHTYPE.ALBUM_633:
			text2 = aInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "AlbumMiddle/" + text2 + ".";
			break;
		case EYECATCHTYPE.ALBUM_1280:
			aInfo = Singleton<SongManager>.instance.GetCurrentAlbum();
			text2 = aInfo.Name;
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "AlbumBig/" + text2 + ".";
			break;
		case EYECATCHTYPE.MISSIONPACK_287:
			text2 = mInfo.iPackId.ToString();
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "MissionSmall/" + text2 + ".";
			break;
		case EYECATCHTYPE.MISSIONPACK_1280:
			text2 = mInfo.iPackId.ToString();
			if (GameData.E_VERSION == VERSION.EN && (text2.Contains("timeline") || text2.Contains("morning")))
			{
				text2 += "_en";
			}
			text = "MissionBig/" + text2 + ".";
			break;
		}
		if (null != uTexture)
		{
			uTexture.color = Color.white;
			if (File.Exists(Path.GetFullPath("../Data/") + text + "unity3d"))
			{
				base.StartCoroutine(this.LoadDiscImage(text2, text + "unity3d", uTexture));
			}
			else
			{
				base.StartCoroutine(this.LoadDiscImagePng(text2, text + "png", uTexture));
			}
			base.StartCoroutine(this.FadeImage(uTexture));
		}
	}

	// Token: 0x060009A3 RID: 2467
	private IEnumerator LoadDiscImage(string strDiscName, string strFileName, UITexture uTexture)
	{
		AssetBundle assetBundle = new WWW("file:///" + Path.GetFullPath("../Data/") + strFileName).assetBundle;
		Texture texture = assetBundle.Load(strDiscName, typeof(Texture)) as Texture;
		uTexture.mainTexture = texture;
		assetBundle.Unload(false);
		yield break;
	}

	// Token: 0x060009A4 RID: 2468
	private void CompleteTexture(Texture tTexture)
	{
		GameObject gameObject = this.m_oFront.transform.FindChild("Loading").gameObject;
		gameObject.GetComponent<UITexture>().mainTexture = tTexture;
		gameObject.GetComponent<UITexture>().color = Color.white;
	}

	// Token: 0x060009A5 RID: 2469
	public void SetSceneControl(GameObject oContol)
	{
		this.m_oSceneControl = oContol;
	}

	// Token: 0x060009A6 RID: 2470
	public void CreatePopUp(POPUPTYPE eType)
	{
		this.m_oControlPopUp.SetActive(true);
		this.m_eCurPopUp = eType;
		string language = Singleton<GameManager>.instance.UserData.Language;
		string text = "BoomerangNet V2에 기계가 아직 등록되지 않았습니다!\n도움이 필요하시면 Discord를 이용해주세요.\nhttps://discord.gg/EnbKc2nYwC";
		string text2 = "로그인을 할 수 없습니다. 기계가 오프라인 상태입니다.";
		string text3 = "네트워크가 현재 요청을 처리할 수 없습니다.\n도움이 필요하시면 Discord를 이용해주세요.\nhttps://discord.gg/EnbKc2nYwC";
		string text4 = "BoomerangNet V2에 연결할 수 없습니다!\n인터넷 연결을 확인해주세요.";
		string text5 = "시스템이 시작 중입니다. 잠시 기다려주세요.";
		UILabel component = this.m_oControlPopUp.transform.FindChild("Label").GetComponent<UILabel>();
		switch (eType)
		{
		case POPUPTYPE.REFILL_POINT:
			component.text = ((language == "KR") ? text : "Machine not yet registered to BoomerangNet V2!\nPlease use our Discord for help.\nhttps://discord.gg/EnbKc2nYwC");
			break;
		case POPUPTYPE.NETWORK_NOTLOGIN:
			component.text = ((language == "KR") ? text2 : "Login is not currently available,\nas the machine is offline.");
			break;
		case POPUPTYPE.NETWORK_NOTSAVEDATA:
			component.text = ((language == "KR") ? text3 : "The network was unable to process your request at this time.\nSee our discord for help.\nhttps://discord.gg/EnbKc2nYwC");
			break;
		case POPUPTYPE.NETWORK_NOTCONNECT:
			component.text = ((language == "KR") ? text4 : "Cannot connect to BoomerangNet V2!\nPlease check your internet.");
			break;
		case POPUPTYPE.CHECK_SYSTEM:
			component.text = ((language == "KR") ? text5 : "System is starting. Please wait.");
			break;
		}
		UIInputManager uiinputManager = UnityEngine.Object.FindObjectOfType(typeof(UIInputManager)) as UIInputManager;
		if (null != uiinputManager)
		{
			uiinputManager.enabled = false;
		}
	}

	// Token: 0x060009A7 RID: 2471
	public void ClosePopUp()
	{
		if ((this.m_eCurPopUp == POPUPTYPE.UPDATE_POPUP && this.UPDATE && this.inAttract()) || (this.m_eCurPopUp == POPUPTYPE.NETWORK_NOTCONNECT && !this.ONNETWORK))
		{
			return;
		}
		this.m_eCurPopUp = POPUPTYPE.NONE;
		if (null != this.m_oControlPopUp)
		{
			this.m_oControlPopUp.SetActive(false);
		}
		UIInputManager uiinputManager = UnityEngine.Object.FindObjectOfType(typeof(UIInputManager)) as UIInputManager;
		if (null != uiinputManager)
		{
			uiinputManager.enabled = true;
		}
	}

	// Token: 0x060009A8 RID: 2472
	private void UpdateInput()
	{
		if (Application.loadedLevelName == "game")
		{
			return;
		}
		if (0 < iPhoneToMouse.instance.touchCount)
		{
			iPhoneToMouse.pos touch = iPhoneToMouse.instance.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				Vector3 screenToWorld = this.GetScreenToWorld(Vector3.zero, new Vector2(touch.position.x, touch.position.y));
				this.CreateTouch(screenToWorld);
			}
			if (touch.phase == TouchPhase.Began && this.m_eCurPopUp != POPUPTYPE.NONE)
			{
				this.ClosePopUp();
			}
		}
	}

	// Token: 0x060009A9 RID: 2473
	private void PressNguiTouch()
	{
		if (0 < iPhoneToMouse.instance.touchCount)
		{
			iPhoneToMouse.pos touch = iPhoneToMouse.instance.GetTouch(0);
			RaycastHit raycastHit;
			if (touch.phase == TouchPhase.Began && UICamera.Raycast(touch.position, out raycastHit) && null != UICamera.hoveredObject)
			{
				UICamera.Notify(UICamera.hoveredObject, "OnPress", true);
				UICamera.currentTouch = new UICamera.MouseOrTouch
				{
					pos = touch.position,
					delta = Vector2.one,
					current = UICamera.hoveredObject
				};
			}
			RaycastHit raycastHit2;
			if (touch.phase == TouchPhase.Moved && UICamera.Raycast(touch.position, out raycastHit2) && UICamera.currentTouch != null)
			{
				UICamera.MouseOrTouch mouseOrTouch = new UICamera.MouseOrTouch();
				mouseOrTouch.pos = touch.position;
				mouseOrTouch.delta = UICamera.lastTouchPosition - touch.position;
				mouseOrTouch.current = UICamera.hoveredObject;
				if (UICamera.currentTouch.current == mouseOrTouch.current && null != UICamera.hoveredObject)
				{
					UICamera.Notify(UICamera.hoveredObject, "OnDrag", mouseOrTouch.delta);
				}
				UICamera.currentTouch = mouseOrTouch;
			}
			UICamera.lastTouchPosition = touch.position;
		}
	}

	// Token: 0x060009AA RID: 2474
	public Vector3 GetScreenToWorld(Vector3 targetWorld, Vector2 screenPos)
	{
		if (this.m_cCamera == null)
		{
			return Vector3.zero;
		}
		Plane plane = new Plane(this.m_cCamera.transform.forward, this.m_cCamera.transform.position);
		float distanceToPoint = plane.GetDistanceToPoint(targetWorld);
		return this.m_cCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceToPoint));
	}

	// Token: 0x060009AB RID: 2475
	private void UpdateKeyBoard()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("MainManagement");
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			this.MenuTimer = !this.MenuTimer;
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			this.IncCoin();
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_COIN_IN, false);
			this.SetBookkeepingCoin();
		}
		if (Input.GetKeyDown(KeyCode.F3) && !Singleton<GameManager>.instance.ONLOGIN)
		{
			GameData.INGAME_AUTO = !GameData.INGAME_AUTO;
		}
		if (Input.GetKeyDown(KeyCode.D) && this.inAttract())
		{
			Singleton<GameManager>.instance.DEMOPLAY = true;
			Singleton<SceneSwitcher>.instance.LoadNextScene("game");
		}
		if (Input.GetKeyDown(KeyCode.R) && this.inAttract())
		{
			if (Singleton<GameManager>.instance.DemoSound)
			{
				Singleton<SoundSourceManager>.instance.StopBgm();
				Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
			}
			this.InitNetData();
			Singleton<SceneSwitcher>.instance.LoadNextScene("Ranking");
		}
		if (Input.GetKeyDown(KeyCode.Mouse0) && this.inAttract() && this.HaveCredit() && GameData.S_CURSCENE != "Title")
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("Title");
		}
		if (Input.GetKeyDown(KeyCode.Return) && this.inAttract() && this.HaveCredit())
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_START_TITLE, false);
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SceneSwitcher>.instance.LoadNextScene("warning");
			this.DEMOPLAY = false;
		}
	}

	// Token: 0x060009AC RID: 2476
	public void LateUpdate()
	{
		this.m_arrPreTouch.Clear();
		for (int i = 0; i < this.m_arrTouch.Count; i++)
		{
			int num = (int)this.m_arrTouch[i];
			this.m_arrPreTouch.Add(num);
		}
	}

	// Token: 0x060009AD RID: 2477
	public void UpdateTouch()
	{
		this.m_arrTouch.Clear();
		for (int i = 0; i < NewMultiTouch.Count; i++)
		{
			TouchInfo touch = NewMultiTouch.GetTouch(i);
			touch.m_ePhase = TouchPhase.Began;
			int id = touch.Point.id;
			if (this.m_arrPreTouch.Contains(id))
			{
				touch.m_ePhase = TouchPhase.Moved;
			}
			this.m_arrTouch.Add(id);
		}
	}

	// Token: 0x060009AE RID: 2478
	private void OnApplicationQuit()
	{
		if (this.serialPort1 != null)
		{
			this.serialPort1.Close();
		}
	}

	// Token: 0x060009AF RID: 2479
	private IEnumerator LoadDiscImagePng(string strDiscName, string strFileName, UITexture uTexture)
	{
		WWW www = new WWW("file:///" + Path.GetFullPath("../Data/") + strFileName);
		while (!www.isDone)
		{
		}
		uTexture.mainTexture = www.texture;
		yield break;
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060009B0 RID: 2480
	// (set) Token: 0x060009B1 RID: 2481
	public bool MenuTimer
	{
		get
		{
			return ConfigManager.Instance.Get<bool>("game.event_mode", false);
		}
		set
		{
			ConfigManager.Instance.Set("game.event_mode", value);
		}
	}

	// Token: 0x060009B2 RID: 2482
	public string ReadSystemJSONFile(string fileName)
	{
		return File.ReadAllText("../Data/System/JSON/" + fileName + ".json");
	}

	// Token: 0x060009B3 RID: 2483
	public object GetIniValue(string Key)
	{
		return (Json.Deserialize(this.ReadSystemJSONFile("config")) as Dictionary<string, object>)[Key];
	}

	// Token: 0x060009B4 RID: 2484
	public void WriteSystemJSONFile(string fileName, string newData)
	{
		File.WriteAllText("../Data/System/JSON/" + fileName + ".json", newData);
	}

	// Token: 0x060009B5 RID: 2485
	public void SetIniValue(string Key, object Value)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(Singleton<GameManager>.instance.ReadSystemJSONFile("config")) as Dictionary<string, object>;
		dictionary[Key] = Value;
		Singleton<GameManager>.instance.WriteSystemJSONFile("config", Json.Serialize(dictionary));
	}

	// Token: 0x060009B6 RID: 2486
	private IEnumerator FadeImage(UITexture uTexture)
	{
		if (Singleton<GameManager>.instance.DEMOPLAY)
		{
			Color originalColor = uTexture.color;
			uTexture.color = Color.black;
			for (float t = 0f; t < 1f; t += Time.deltaTime / 0.7f)
			{
				uTexture.color = Color.Lerp(Color.black, originalColor, t);
				yield return null;
			}
			uTexture.color = originalColor;
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
			originalColor = default(Color);
		}
		yield break;
	}

	// Token: 0x060009B7 RID: 2487
	public IEnumerator SetMark(string strScene)
	{
		this.m_oControlLtInfo.SetActive(true);
		this.m_tCredists.gameObject.SetActive(true);
		if ("game" == strScene && !this.DEMOPLAY)
		{
			this.m_oControlLtInfo.SetActive(false);
		}
		if ("MainManagement" == strScene)
		{
			this.m_oControlLtInfo.SetActive(false);
			this.m_tCredists.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x060009B8 RID: 2488
	public void CreateCustomPopUp(string popupContent)
	{
		this.m_oControlPopUp.SetActive(true);
		this.m_eCurPopUp = POPUPTYPE.EVENT_POPUP;
		this.m_oControlPopUp.transform.FindChild("Label").GetComponent<UILabel>().text = popupContent;
		UIInputManager uiinputManager = UnityEngine.Object.FindObjectOfType(typeof(UIInputManager)) as UIInputManager;
		if (null != uiinputManager)
		{
			uiinputManager.enabled = false;
		}
	}

	// Token: 0x060009B9 RID: 2489
	public void UpdatePopUp()
	{
		this.m_oControlPopUp.SetActive(this.inAttract());
		this.m_eCurPopUp = POPUPTYPE.UPDATE_POPUP;
		string text = "";
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		if (userData.Language == "EN")
		{
			text = "New update available!\nPlease restart to install (｡\u02c3 ᵕ \u02c2 )";
		}
		else if (userData.Language == "KR")
		{
			text = "새로운 업데이트가 있습니다!\n설치를 위해 재부팅해주세요 (｡◕\u203f\u203f◕｡)";
		}
		this.m_oControlPopUp.transform.FindChild("Label").GetComponent<UILabel>().text = text;
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060009BA RID: 2490
	// (set) Token: 0x060009BB RID: 2491
	public bool UPDATE
	{
		get
		{
			return this.b_update;
		}
		set
		{
			this.b_update = value;
			if (this.b_update)
			{
				this.m_sCheck.gameObject.SetActive(true);
				this.m_sCheck.MakePixelPerfect();
				this.m_sCheck.spriteName = "Check";
				this.UpdatePopUp();
				return;
			}
			if (this.inAttract())
			{
				this.ClosePopUp();
			}
			this.m_sCheck.gameObject.SetActive(false);
		}
	}

	// Token: 0x060009BC RID: 2492
	public bool inAttract()
	{
		string[] array = new string[] { "CopyRight", "Ci", "Title", "Ranking", "Tutorial" };
		if (this.DEMOPLAY && Application.loadedLevelName == "game")
		{
			return true;
		}
		foreach (string text in array)
		{
			if (Application.loadedLevelName == text)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060009BD RID: 2493
	public bool HaveCredit()
	{
		return GameData.E_PLAYTYPE == PLAYTYPE.FREEPLAY || 0 < Singleton<GameManager>.instance.CREDIT;
	}

	// Token: 0x060009BE RID: 2494
	public void SaveData(string strCurScene)
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			if ("HausMixResult" == strCurScene)
			{
				this.UserData.TotalExp += this.ResultData.EXP;
				this.UserData.BeatPoint += this.ResultData.BEATPOINT;
			}
			if ("HausMixAllClearResult" == strCurScene)
			{
				this.UserData.SetViewValue();
				this.ResultData = this.ContinueData;
				this.ResultData.SaveTotalResult();
				this.UserData.TotalExp += this.ResultData.ALLCLEAR_EXP;
				this.UserData.BeatPoint += this.ResultData.ALLCLEAR_BEATPOINT;
				Singleton<WWWManager>.instance.PostHausTotalResult();
				return;
			}
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			if ("RaveUpResult" == strCurScene)
			{
				this.UserData.SetViewValue();
				this.ResultData.SaveTotalResult();
				this.UserData.TotalExp += this.ResultData.EXP;
				this.UserData.BeatPoint += this.ResultData.BEATPOINT;
				return;
			}
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION && "ClubTourResult" == strCurScene)
		{
			this.UserData.SetViewValue();
			this.ResultData.SaveTotalResult();
			this.UserData.TotalExp += this.ResultData.EXP;
			this.UserData.BeatPoint += this.ResultData.BEATPOINT;
		}
	}

	// Token: 0x0400092E RID: 2350
	private const int MAX_IO_SENDBUF = 1024;

	// Token: 0x0400092F RID: 2351
	private const string VERSIONLOCAL = "Local";

	// Token: 0x04000930 RID: 2352
	private const string VERSIONINCOM = "InCom";

	// Token: 0x04000931 RID: 2353
	private SerialPort serialPort1 = new SerialPort();

	// Token: 0x04000932 RID: 2354
	private byte[] m_sendBuf = new byte[1024];

	// Token: 0x04000933 RID: 2355
	public RESULTDATA ResultData;

	// Token: 0x04000934 RID: 2356
	public RESULTDATA ContinueData = new RESULTDATA();

	// Token: 0x04000935 RID: 2357
	public RESULTDATA[] TotalResult = new RESULTDATA[]
	{
		new RESULTDATA(),
		new RESULTDATA(),
		new RESULTDATA(),
		new RESULTDATA()
	};

	// Token: 0x04000936 RID: 2358
	public USERDATA UserData = new USERDATA();

	// Token: 0x04000937 RID: 2359
	private Camera m_cCamera;

	// Token: 0x04000938 RID: 2360
	private GameObject m_oInfo;

	// Token: 0x04000939 RID: 2361
	private GameObject m_oFront;

	// Token: 0x0400093A RID: 2362
	private GameObject m_oControlTestView;

	// Token: 0x0400093B RID: 2363
	private GameObject m_oControlLtInfo;

	// Token: 0x0400093C RID: 2364
	private GameObject m_oControlPopUp;

	// Token: 0x0400093D RID: 2365
	private POPUPTYPE m_eCurPopUp;

	// Token: 0x0400093E RID: 2366
	private GameObject m_oSceneControl;

	// Token: 0x0400093F RID: 2367
	private UILabel m_tCredists;

	// Token: 0x04000940 RID: 2368
	private UISprite m_sIdCard;

	// Token: 0x04000941 RID: 2369
	private UISprite m_sNetWork;

	// Token: 0x04000942 RID: 2370
	private UISprite m_sCheck;

	// Token: 0x04000943 RID: 2371
	private GameObject TOUCH;

	// Token: 0x04000944 RID: 2372
	public string authorization = "1673ce7c1f2fef4eaf1ac7ff80110ba00d0a3df8afbf40ba9402179a0292d1c92e153cd0ab003a9cbcb0b85a55b421508cc9f05fe1527530ca014f96554f56da";

	// Token: 0x04000945 RID: 2373
	public string s_UserID = "9UC5PIWMO7Z9";

	// Token: 0x04000946 RID: 2374
	public ArrayList ArrNotice = new ArrayList();

	// Token: 0x04000947 RID: 2375
	public ArrayList ArrItemInfo = new ArrayList();

	// Token: 0x04000948 RID: 2376
	public ArrayList ArrAddPatternInfo = new ArrayList();

	// Token: 0x04000949 RID: 2377
	public HouseMixSelectData netHouseMixData = new HouseMixSelectData();

	// Token: 0x0400094A RID: 2378
	public HouseMixResultData netHouMixResultData = new HouseMixResultData();

	// Token: 0x0400094B RID: 2379
	public HouseMixTotalResultData netHouMixTotalResultData = new HouseMixTotalResultData();

	// Token: 0x0400094C RID: 2380
	public UserRecordData netUserRecordData = new UserRecordData();

	// Token: 0x0400094D RID: 2381
	public ArrayList HausModeRank = new ArrayList();

	// Token: 0x0400094E RID: 2382
	public ArrayList RaveModeRank = new ArrayList();

	// Token: 0x0400094F RID: 2383
	public RaveUpRankData netRaveUpRankData = new RaveUpRankData();

	// Token: 0x04000950 RID: 2384
	public RaveUpResultRank netRaveUpResultData = new RaveUpResultRank();

	// Token: 0x04000951 RID: 2385
	private ArrayList m_arrPreTouch = new ArrayList();

	// Token: 0x04000952 RID: 2386
	private ArrayList m_arrTouch = new ArrayList();

	// Token: 0x04000953 RID: 2387
	public bool isAllSongMode;

	// Token: 0x04000954 RID: 2388
	public bool isNewSongEvent;

	// Token: 0x04000955 RID: 2389
	private bool m_bDemo;

	// Token: 0x04000956 RID: 2390
	private int m_iCoin;

	// Token: 0x04000957 RID: 2391
	private int m_iCredit;

	// Token: 0x04000958 RID: 2392
	private int m_iService;

	// Token: 0x04000959 RID: 2393
	private int m_iPoint;

	// Token: 0x0400095A RID: 2394
	private bool m_bOnLogin;

	// Token: 0x0400095B RID: 2395
	private bool m_bOnNetWork = true;

	// Token: 0x0400095C RID: 2396
	private bool m_bOnCheck;

	// Token: 0x0400095D RID: 2397
	public bool RaveUpHurdleFail;

	// Token: 0x0400095E RID: 2398
	public bool NoCardUser;

	// Token: 0x0400095F RID: 2399
	public bool PLAYUSER = true;

	// Token: 0x04000960 RID: 2400
	public MISSIONREWARD_STATE RewardState;

	// Token: 0x04000961 RID: 2401
	public int DEMOPLAYNUM;

	// Token: 0x04000962 RID: 2402
	public int CoinCount;

	// Token: 0x04000963 RID: 2403
	private bool b_update;

	// Token: 0x04000964 RID: 2404
	private UISprite m_sPoint;

	// Token: 0x04000965 RID: 2405
	public bool cardSuccess;

	// Token: 0x04000966 RID: 2406
	public bool READER_ACTIVE;

	// Token: 0x04000967 RID: 2407
	public bool IO_ACTIVE;
}
