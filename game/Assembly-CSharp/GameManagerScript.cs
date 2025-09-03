using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class GameManagerScript : MonoBehaviour
{
	// Token: 0x0600075B RID: 1883 RVA: 0x0003771C File Offset: 0x0003591C
	private void Awake()
	{
		Logger.Log("GameManagerScript", "Starting GameManagerScript init...", new object[0]);
		GameData.INGAME_AUTO = false;
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		if (Singleton<GameManager>.instance.DEMOPLAY)
		{
			Singleton<GameManager>.instance.InitNetData();
		}
		this.SetGameState();
		base.transform.FindChild("Sprite_Blind").gameObject.SetActive(false);
		this.InitChaosSpeed();
		this.SetSpeed();
		switch (GameData.RANDEFFECTOR)
		{
		case EFFECTOR_RAND.ROTATE_LEFT:
		{
			for (int i = 0; i < GameData.MAXGUIDE.Length; i++)
			{
				Vector3[] maxguide = GameData.MAXGUIDE;
				int num = i;
				maxguide[num].z = maxguide[num].z + 90f;
			}
			break;
		}
		case EFFECTOR_RAND.ROTATE_RIGHT:
		{
			for (int j = 0; j < GameData.MAXGUIDE.Length; j++)
			{
				Vector3[] maxguide2 = GameData.MAXGUIDE;
				int num2 = j;
				maxguide2[num2].z = maxguide2[num2].z - 90f;
			}
			break;
		}
		case EFFECTOR_RAND.ROTATE_RANDOM:
		{
			float num3 = 30f * (float)UnityEngine.Random.Range(-20, 20);
			for (int k = 0; k < GameData.MAXGUIDE.Length; k++)
			{
				Vector3[] maxguide3 = GameData.MAXGUIDE;
				int num4 = k;
				maxguide3[num4].z = maxguide3[num4].z + num3;
			}
			break;
		}
		}
		Singleton<GameManager>.instance.InitResult();
		this.m_rData = Singleton<GameManager>.instance.ResultData;
		this.m_fTime = 0f;
		this.SetObject();
		this.Pause();
		this.m_sLoading.SendMessage("SetManager", base.gameObject);
		this.SetMovie();
		Logger.Log("GameManagerScript", "Initialized GameManagerScript", new object[0]);
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00008B41 File Offset: 0x00006D41
	private void SetSpeed()
	{
		if (EFFECTOR_SPEED.MAX_SPEED > GameData.SPEEDEFFECTOR)
		{
			GameData.INGAMGE_SPEED = (int)GameData.SPEEDEFFECTOR;
		}
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00008B56 File Offset: 0x00006D56
	private void Start()
	{
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_INGAME);
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00003648 File Offset: 0x00001848
	private void CallFade()
	{
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00008B63 File Offset: 0x00006D63
	private void SetItem()
	{
		this.m_sControlEffector.SetEffect();
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00008B70 File Offset: 0x00006D70
	private void SetGameState()
	{
		GameData.INGAMGE_SPEED = 1;
		if (!Singleton<GameManager>.instance.PLAYUSER)
		{
			Singleton<GameManager>.instance.DEMOPLAY = true;
		}
		if (Singleton<GameManager>.instance.DEMOPLAY)
		{
			GameData.INGAME_AUTO = true;
			GameData.SPEEDEFFECTOR = EFFECTOR_SPEED.X_2;
			return;
		}
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x000378E8 File Offset: 0x00035AE8
	private void InitChaosSpeed()
	{
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_UP || GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_DN)
		{
			GameData.INGAMGE_SPEED = 0;
		}
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.MAX_SPEED)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < 12; i++)
			{
				arrayList.Add(i);
			}
			for (int j = 0; j < 12; j++)
			{
				int num = UnityEngine.Random.Range(0, arrayList.Count);
				int num2 = (int)arrayList[num];
				GameData.CHAOS_X_RANDOM[j] = GameData.CHAOS_X[num2];
				arrayList.RemoveAt(num);
			}
		}
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00037974 File Offset: 0x00035B74
	public void CompletePt(string strContent)
	{
		this.m_bCompleteLoad = true;
		this.m_ScorePlayer.Init();
		this.m_ScorePlayer.LoadXMLData(strContent);
		this.m_sControlNote.SendMessage("SetScorePlayer", this.m_ScorePlayer);
		this.m_sControlNote.SendMessage("SetData");
		if (Singleton<GameManager>.instance.DEMOPLAY)
		{
			this.m_ScorePlayer.m_dComplete = new ScorePlayer.CompletSong(this.CompleteFade);
			return;
		}
		this.m_ScorePlayer.m_dComplete = new ScorePlayer.CompletSong(this.CompleteGame);
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00037A04 File Offset: 0x00035C04
	private void StartPlay()
	{
		this.m_bStartPlay = true;
		this.m_fTime = 0f;
		base.GetComponent<LoadingScript>().CloseLoad();
		this.m_sControlCombo.SendMessage("StartMoveExtreme");
		this.m_ScorePlayer.Play(0);
		this.m_ScorePlayer.InitKeyState();
		GameData.LONGNOTE_TIMECOUNT = this.m_ScorePlayer.TPM / 16;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00037A68 File Offset: 0x00035C68
	private void StartMovie(float fElapsedTime)
	{
		this.m_aTexture.gameObject.SetActive(true);
		this.m_aMovie.SetElapsedTime(fElapsedTime);
		this.m_aMovie._volume = 0f;
		this.m_aMovie.Play();
		if (!Singleton<GameManager>.instance.DEMOPLAY)
		{
			Singleton<DiscordRichPresenceController>.instance.SetSongPresence(this.m_ScorePlayer.m_ms);
		}
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x00008BA8 File Offset: 0x00006DA8
	private void Pause()
	{
		base.enabled = false;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00037AD0 File Offset: 0x00035CD0
	private void SetMovie()
	{
		string text = Singleton<SongManager>.instance.GetCurrentDisc().Name + ".mp4";
		this.m_aMovie._folder = "../Movie/InGame/";
		this.m_aMovie._filename = text;
		this.m_aMovie._loadOnStart = true;
		this.m_aMovie._loop = false;
		this.m_aMovie._playOnStart = false;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00037B38 File Offset: 0x00035D38
	private void SetObject()
	{
		this.m_cGame = base.transform.FindChild("Camera").GetComponent<Camera>();
		this.m_cCoolBomb = base.transform.FindChild("CoolBombCamera").GetComponent<Camera>();
		GameObject gameObject = base.transform.FindChild("Ui").gameObject;
		GameObject gameObject2 = gameObject.transform.FindChild("Control").gameObject;
		this.m_cUi = gameObject.transform.FindChild("UiCamera").GetComponent<Camera>();
		this.m_oBack = gameObject.transform.FindChild("Back").transform.FindChild("BackLine").gameObject;
		GameObject gameObject3 = gameObject2.transform.FindChild("ControlPanel").gameObject;
		this.m_oBackExtreme = gameObject3.transform.FindChild("ExtremeGage").gameObject;
		this.m_sLoading = base.GetComponent<LoadingScript>();
		this.m_sControlLife = gameObject2.transform.FindChild("ControlLife").GetComponent<ControlLifeScript>();
		this.m_sControlLife.SendMessage("SetManager", base.gameObject);
		this.m_sControlNote = base.transform.FindChild("ControlNote").GetComponent<ControlNoteScript>();
		this.m_sControlNote.SendMessage("Setobject");
		this.m_sControlCombo = gameObject2.transform.FindChild("ControlCombo").GetComponent<ControlComboScript>();
		this.m_sControlCombo.SendMessage("SetManager", base.gameObject);
		this.m_sControlFever = gameObject2.transform.FindChild("ControlFever").GetComponent<ControlFeverScript>();
		this.m_sControlFever.SetBack(this.m_oBack);
		this.m_sControlFever.SetControl(base.gameObject);
		this.m_sControlJudgment = base.GetComponent<ControlJudgmentScript>();
		this.m_sControlJudgment.SetGameManager(this);
		this.m_sControlJudgment.SetScorePlayer(this.m_ScorePlayer);
		this.m_sControlJudgment.SetObject();
		this.m_sControlJudgment.SetInput(this.m_sTouch);
		GameObject gameObject4 = gameObject2.transform.FindChild("ControlLife").gameObject;
		this.m_sLifeBack = gameObject4.transform.FindChild("Back").GetComponent<UISprite>();
		this.m_sFeverBack = this.m_sControlFever.transform.FindChild("Back").GetComponent<UISprite>();
		this.m_oControlInput = this.m_sControlNote.transform.FindChild("ControlInput").gameObject;
		this.AllInput.Clear();
		GameObject gameObject5 = (GameObject)Resources.Load("Game/input");
		for (int i = 0; i < 12; i++)
		{
			GameObject gameObject6 = (GameObject)UnityEngine.Object.Instantiate(gameObject5);
			gameObject6.transform.parent = this.m_oControlInput.transform;
			gameObject6.transform.localPosition = this.m_sControlNote.GetLinePos(i);
			gameObject6.transform.localScale = Vector3.one;
			gameObject6.name = "input" + i.ToString();
			this.AllInput.Add(gameObject6);
		}
		this.vBEATVALUE = this.vBEAT_ON - this.vBEAT_OFF;
		this.m_sControlEffector = gameObject2.transform.FindChild("Effector").GetComponent<ControlEffectorScript>();
		this.m_oControlTest = gameObject2.transform.FindChild("ControlTest").gameObject;
		this.m_tTempInfo = this.m_oControlTest.transform.FindChild("txtInfo").GetComponent<UILabel>();
		this.m_tTempInput = this.m_oControlTest.transform.FindChild("txtInput").GetComponent<UILabel>();
		if (Singleton<GameManager>.instance.DEMOPLAY)
		{
			gameObject2.transform.FindChild("DemoPlay").gameObject.SetActive(true);
		}
		else
		{
			gameObject2.transform.FindChild("DemoPlay").gameObject.SetActive(false);
		}
		this.m_aMovie = gameObject.transform.FindChild("PanelMovie").transform.FindChild("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aTexture = gameObject.transform.FindChild("PanelMovie").transform.FindChild("Texture").GetComponent<UITexture>();
		this.m_aTexture.gameObject.SetActive(false);
		this.m_ScorePlayer.CallBackStart = new ScorePlayer.CallBackStartMusic(this.StartMovie);
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00037F9C File Offset: 0x0003619C
	private void UpdateRotateBack()
	{
		int curTick = this.m_ScorePlayer.GetCurTick();
		if (!this.m_bTouchRotate)
		{
			float num = (float)(curTick - this.m_iPreTick) / (this.m_ScorePlayer.TPS * 4f);
			Vector3 localEulerAngles = this.m_oBack.transform.localEulerAngles;
			localEulerAngles.z -= 360f * num;
			this.m_fLastRot = localEulerAngles.z;
			this.m_oBack.transform.localEulerAngles = localEulerAngles;
		}
		else
		{
			float angle = Singleton<GameManager>.instance.GetAngle(Vector2.zero, new Vector2(this.m_vTouchPos.x, this.m_vTouchPos.y));
			Vector3 eulerAngles = this.m_oBack.transform.eulerAngles;
			eulerAngles.z = angle * -1f + this.m_fLastRot;
			this.m_oBack.transform.localEulerAngles = eulerAngles;
		}
		this.m_oBackExtreme.transform.localEulerAngles = this.m_oBack.transform.localEulerAngles;
		this.m_iPreTick = curTick;
		if (GameData.RANDEFFECTOR == EFFECTOR_RAND.CYCLON)
		{
			int num2 = (int)this.m_ScorePlayer.TPS * 16;
			bool flag = this.m_ScorePlayer.GetCurTick() / num2 % 2 == 0;
			float num3 = Time.deltaTime * 45f;
			Vector3 localEulerAngles2 = this.m_oControlInput.transform.localEulerAngles;
			if (!flag)
			{
				num3 *= -1f;
			}
			localEulerAngles2.z += num3;
			this.m_oControlInput.transform.localEulerAngles = localEulerAngles2;
			for (int i = 0; i < GameData.MAXGUIDE.Length; i++)
			{
				Vector3[] maxguide = GameData.MAXGUIDE;
				int num4 = i;
				maxguide[num4].z = maxguide[num4].z + num3;
			}
		}
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00038160 File Offset: 0x00036360
	private void UpdateDemoCheck()
	{
		if (Singleton<GameManager>.instance.PLAYUSER || this.m_ScorePlayer.GetCurTick() < 24000)
		{
			return;
		}
		if (Singleton<SoundSourceManager>.instance.getNowBGM().volume > 0f)
		{
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = Singleton<SoundSourceManager>.instance.getNowBGM().volume - Time.deltaTime;
			return;
		}
		this.CompleteGame();
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000381D0 File Offset: 0x000363D0
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (!this.m_bStartPlay)
		{
			float num = 1f;
			if (this.m_fTime > num && this.m_bCompleteLoad)
			{
				this.StartPlay();
			}
		}
		if (this.m_bGameOver)
		{
			if (2.5f < this.m_fTime)
			{
				this.NextScene();
			}
			return;
		}
		if (this.m_bStartPlay && !Singleton<GameManager>.instance.DEMOPLAY)
		{
			string text = ConfigManager.Instance.Get<string>("game.input", false);
			if (text.Equals("touch") || text.Equals("real_io"))
			{
				this.UpdateMulti();
			}
			else
			{
				this.UpdateCommon();
				this.UpdateInput();
			}
		}
		this.UpdateDemoCheck();
		this.UpdateRotateBack();
		this.m_ScorePlayer.Update();
		this.m_sControlNote.RenderNote();
		this.m_sControlJudgment.UpdateJudgment();
		this.UpdateBeat();
		this.m_sTouch.UpdateLast();
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00008BB1 File Offset: 0x00006DB1
	private void SetHurdleUp(bool bUp)
	{
		this.m_bRaveUpHurdle = bUp;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00008BBA File Offset: 0x00006DBA
	private void CompleteFade()
	{
		base.Invoke("CompleteGame", 5f);
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00008BCC File Offset: 0x00006DCC
	private void MoveRankingScene()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		}
		Singleton<SceneSwitcher>.instance.LoadNextScene("Ranking");
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x000382C4 File Offset: 0x000364C4
	private void CompleteGame()
	{
		if (Singleton<GameManager>.instance.DEMOPLAY)
		{
			base.Invoke("MoveRankingScene", 0f);
			return;
		}
		if (this.m_rData.ONEXTREME)
		{
			this.m_rData.InitExtremeCombo();
		}
		Singleton<GameManager>.instance.ResultData.SaveStage();
		Singleton<WWWManager>.instance.PostConfiguration();
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("HausMixResult");
			WWWPostHausStage wwwpostHausStage = new WWWPostHausStage();
			Singleton<WWWManager>.instance.AddQueue(wwwpostHausStage);
			return;
		}
		if (Singleton<SongManager>.instance.Mode != GAMEMODE.RAVEUP)
		{
			if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
			{
				GameData.Stage++;
				if (3 <= GameData.Stage)
				{
					Singleton<SceneSwitcher>.instance.LoadNextScene("ClubTourResult");
					GameData.Stage = 0;
					MissionData mission = Singleton<SongManager>.instance.Mission;
					WWWPostMissionPlayScript wwwpostMissionPlayScript = new WWWPostMissionPlayScript();
					wwwpostMissionPlayScript.bCondition = mission.AllClear;
					Singleton<WWWManager>.instance.AddQueue(wwwpostMissionPlayScript);
					return;
				}
				Singleton<SceneSwitcher>.instance.LoadNextScene("game");
			}
			return;
		}
		Singleton<GameManager>.instance.RaveUpHurdleFail = !this.m_bRaveUpHurdle;
		GameData.Stage++;
		if (4 <= GameData.Stage)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("RaveUpResult");
			GameData.Stage = 0;
			WWWPostRaveUp wwwpostRaveUp = new WWWPostRaveUp();
			Singleton<WWWManager>.instance.AddQueue(wwwpostRaveUp);
			return;
		}
		if (Singleton<GameManager>.instance.RaveUpHurdleFail)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("RaveUpResult");
			WWWPostFailRaveUp wwwpostFailRaveUp = new WWWPostFailRaveUp();
			Singleton<WWWManager>.instance.AddQueue(wwwpostFailRaveUp);
			return;
		}
		Singleton<SceneSwitcher>.instance.LoadNextScene("game");
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x00008C07 File Offset: 0x00006E07
	private void PressFever()
	{
		this.m_sControlFever.PressFever();
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x00008C14 File Offset: 0x00006E14
	private void OnStartFever()
	{
		this.m_sControlNote.SendMessage("SetFeverTps", true);
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x00008C2C File Offset: 0x00006E2C
	private void OnEndFever()
	{
		this.m_sControlNote.SendMessage("SetFeverTps", false);
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00038454 File Offset: 0x00036654
	private void OnSpeedChange(bool bUp)
	{
		if (GameData.ON_FEVER)
		{
			return;
		}
		if (EFFECTOR_SPEED.MAX_SPEED <= GameData.SPEEDEFFECTOR)
		{
			return;
		}
		if (bUp)
		{
			int num = (int)(GameData.SPEEDEFFECTOR + 1);
			if (10 <= num)
			{
				return;
			}
			GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)num;
			this.SetSpeed();
			return;
		}
		else
		{
			int num2 = GameData.SPEEDEFFECTOR - EFFECTOR_SPEED.X_1;
			if (0 > num2)
			{
				return;
			}
			GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)num2;
			this.SetSpeed();
			return;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x000384AC File Offset: 0x000366AC
	private void UpdateInput()
	{
		this.m_sTouch.Init();
		if (0 < iPhoneToMouse.instance.touchCount)
		{
			iPhoneToMouse.pos touch = iPhoneToMouse.instance.GetTouch(0);
			this.m_vTouchPos = new Vector3(touch.position.x - (float)(Screen.width / 2), touch.position.y - (float)(Screen.height / 2), 0f);
			if (!this.m_bTouchRotate)
			{
				float angle = Singleton<GameManager>.instance.GetAngle(Vector2.zero, new Vector2(this.m_vTouchPos.x, this.m_vTouchPos.y));
				this.m_fLastRot = this.m_oBack.transform.eulerAngles.z - angle * -1f;
			}
			this.m_bTouchRotate = true;
		}
		else
		{
			this.m_bTouchRotate = false;
		}
		for (int i = 0; i < iPhoneToMouse.instance.touchCount; i++)
		{
			iPhoneToMouse.pos touch2 = iPhoneToMouse.instance.GetTouch(i);
			RaycastHit[] array = Physics.RaycastAll(this.m_cGame.ScreenPointToRay(touch2.position));
			if (array.Length != 0)
			{
				RaycastHit raycastHit = array[0];
				foreach (RaycastHit raycastHit2 in array)
				{
					if (raycastHit.distance > raycastHit2.distance)
					{
						raycastHit = raycastHit2;
					}
				}
				string name = raycastHit.collider.gameObject.name;
				if (touch2.phase == TouchPhase.Began)
				{
					for (int k = 0; k < 12; k++)
					{
						string text = "input" + k.ToString();
						if (name == text)
						{
							this.m_ScorePlayer.SetKeyState(k, KEYSTATE.BEGAN);
							this.m_ScorePlayer.SetKeyFirstPos(k, raycastHit.point);
							this.m_sTouch.MultiCheckInput[k] = true;
							this.m_sTouch.SingleCheckInput[k] = true;
							this.m_sTouch.MultiInputPos[k] = new Vector3(touch2.position.x, touch2.position.y, 0f);
						}
					}
				}
				if (touch2.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Stationary)
				{
					for (int l = 0; l < 12; l++)
					{
						string text2 = "input" + l.ToString();
						if (name == text2)
						{
							this.m_sTouch.SingleCheckInput[l] = true;
							this.m_sTouch.MultiCheckInput[l] = true;
							this.m_sTouch.MultiInputPos[l] = new Vector3(touch2.position.x, touch2.position.y, 0f);
							if (this.m_ScorePlayer.GetKeyState(l) == KEYSTATE.NONE)
							{
								this.m_ScorePlayer.SetKeyState(l, KEYSTATE.BEGAN);
								this.m_ScorePlayer.SetKeyFirstPos(l, raycastHit.point);
							}
							else
							{
								this.m_ScorePlayer.SetKeyState(l, KEYSTATE.MOVE);
								this.m_ScorePlayer.SetKeyPos(l, raycastHit.point);
							}
						}
					}
				}
			}
		}
		for (int m = 0; m < 12; m++)
		{
			if (!this.m_sTouch.SingleCheckInput[m])
			{
				this.m_ScorePlayer.SetKeyState(m, KEYSTATE.NONE);
			}
		}
		for (int n = 0; n < 12; n++)
		{
			GameObject gameObject = (GameObject)this.AllInput[n];
			if (this.m_ScorePlayer.GetKeyState(n) == KEYSTATE.NONE)
			{
				gameObject.transform.FindChild("TestInput").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
			}
			else
			{
				gameObject.transform.FindChild("TestInput").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.7f);
			}
		}
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00038894 File Offset: 0x00036A94
	private void UpdateCommon()
	{
		for (int i = 0; i < iPhoneToMouse.instance.touchCount; i++)
		{
			iPhoneToMouse.pos touch = iPhoneToMouse.instance.GetTouch(i);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.m_cUi.ScreenPointToRay(touch.position), out raycastHit))
			{
				string name = raycastHit.collider.gameObject.name;
				if (touch.phase == TouchPhase.Ended)
				{
					if ("Fever" == name)
					{
						this.PressFever();
					}
					if ("Extreme" == name)
					{
						this.m_sControlCombo.StartExtremeZone();
					}
					if ("MissionBack" == name)
					{
						this.m_oControlTest.SetActive(!this.m_oControlTest.activeSelf);
					}
				}
			}
		}
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00038954 File Offset: 0x00036B54
	private void UpdateMulti()
	{
		NewMultiTouch.UpdateCount();
		TouchInfo touchInfo = null;
		for (int i = 0; i < NewMultiTouch.Count; i++)
		{
			TouchInfo touch = NewMultiTouch.GetTouch(i);
			if (touch.Point.id == 1 && i == 0)
			{
				touchInfo = touch;
			}
		}
		if (touchInfo != null)
		{
			Vector3 touchToPos = Singleton<GameManager>.instance.GetTouchToPos(touchInfo);
			this.m_vTouchPos = new Vector3(touchToPos.x - (float)(Screen.width / 2), touchToPos.y - (float)(Screen.height / 2), 0f);
			if (!this.m_bTouchRotate)
			{
				float angle = Singleton<GameManager>.instance.GetAngle(Vector2.zero, new Vector2(this.m_vTouchPos.x, this.m_vTouchPos.y));
				this.m_fLastRot = this.m_oBack.transform.eulerAngles.z - angle * -1f;
			}
			this.m_bTouchRotate = true;
		}
		else
		{
			this.m_bTouchRotate = false;
		}
		this.m_sTouch.Init();
		for (int j = 0; j < NewMultiTouch.Count; j++)
		{
			TouchInfo touch2 = NewMultiTouch.GetTouch(j);
			Vector3 touchToPos2 = Singleton<GameManager>.instance.GetTouchToPos(touch2);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.m_cUi.ScreenPointToRay(touchToPos2), out raycastHit))
			{
				string name = raycastHit.collider.gameObject.name;
				if (touch2.m_ePhase == TouchPhase.Began)
				{
					if ("Fever" == name)
					{
						this.PressFever();
					}
					if ("Extreme" == name)
					{
						this.m_sControlCombo.StartExtremeZone();
					}
				}
			}
			RaycastHit[] array = Physics.RaycastAll(this.m_cGame.ScreenPointToRay(touchToPos2));
			if (array.Length != 0)
			{
				RaycastHit raycastHit2 = array[0];
				foreach (RaycastHit raycastHit3 in array)
				{
					if (raycastHit2.distance > raycastHit3.distance)
					{
						raycastHit2 = raycastHit3;
					}
				}
				string name2 = raycastHit2.collider.gameObject.name;
				for (int l = 0; l < 12; l++)
				{
					string text = "input" + l.ToString();
					if (name2 == text)
					{
						this.m_sTouch.SingleCheckInput[l] = true;
						if (touch2.m_ePhase == TouchPhase.Began)
						{
							this.m_ScorePlayer.SetKeyState(l, KEYSTATE.BEGAN);
							this.m_ScorePlayer.SetKeyFirstPos(l, raycastHit2.point);
						}
						else if (touch2.m_ePhase == TouchPhase.Moved)
						{
							if (this.m_ScorePlayer.GetKeyState(l) == KEYSTATE.NONE)
							{
								this.m_ScorePlayer.SetKeyState(l, KEYSTATE.BEGAN);
								this.m_ScorePlayer.SetKeyFirstPos(l, raycastHit2.point);
							}
							else
							{
								this.m_ScorePlayer.SetKeyState(l, KEYSTATE.MOVE);
								this.m_ScorePlayer.SetKeyPos(l, raycastHit2.point);
							}
						}
					}
				}
				foreach (RaycastHit raycastHit4 in array)
				{
					string name3 = raycastHit4.collider.gameObject.name;
					for (int m = 0; m < 12; m++)
					{
						string text2 = "input" + m.ToString();
						if (name3 == text2)
						{
							if (ConfigManager.Instance.Get<string>("game.input", false).Equals("real_io"))
							{
								Vector3 vector = new Vector3((float)touch2.Point.x, (float)touch2.Point.y, 0f);
								this.m_sTouch.MultiCheckInput[m] = true;
								this.m_sTouch.MultiInputPos[m] = vector;
							}
							else
							{
								Vector3 vector2 = new Vector3(touchToPos2.x, touchToPos2.y, 0f);
								this.m_sTouch.MultiCheckInput[m] = true;
								this.m_sTouch.MultiInputPos[m] = vector2;
							}
						}
					}
				}
			}
		}
		for (int n = 0; n < 12; n++)
		{
			GameObject gameObject = (GameObject)this.AllInput[n];
			if (!this.m_sTouch.SingleCheckInput[n])
			{
				this.m_ScorePlayer.SetKeyState(n, KEYSTATE.NONE);
				gameObject.transform.FindChild("TestInput").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
			}
			else
			{
				gameObject.transform.FindChild("TestInput").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.7f);
			}
		}
		Singleton<GameManager>.instance.LateUpdate();
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00008C44 File Offset: 0x00006E44
	public void StartRefillAnimation()
	{
		this.m_sControlEffector.StartRefillAnimation();
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00008C51 File Offset: 0x00006E51
	public void SetChangeTps()
	{
		this.m_sControlNote.SendMessage("SetChangeTps");
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00008C63 File Offset: 0x00006E63
	public void SetSpeedChange()
	{
		if (EFFECTOR_SPEED.MAX_SPEED <= GameData.SPEEDEFFECTOR)
		{
			return;
		}
		if (EFFECTOR_SPEED.X_0_5 > GameData.SPEEDEFFECTOR)
		{
			GameData.SPEEDEFFECTOR = EFFECTOR_SPEED.X_0_5;
		}
		this.SetChangeTps();
		this.m_sControlEffector.SetEffect();
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00038DEC File Offset: 0x00036FEC
	public Vector3 GetEffectPos(int iTrack)
	{
		Vector3 position = ((GameObject)this.AllInput[iTrack]).transform.position;
		position.z -= 0.01f;
		return position;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00038E28 File Offset: 0x00037028
	private void UpdateBeat()
	{
		Color color = Color.white;
		float num = (float)this.m_ScorePlayer.GetCurTick();
		float num2 = (float)(this.m_ScorePlayer.TPM / 4);
		int num3 = (int)(num / num2);
		float num4 = num % num2;
		if (this.m_iBeat != num3)
		{
			this.m_iBeat = num3;
			color = this.vBEAT_OFF;
		}
		else if (num2 / 2f > num4)
		{
			float num5 = num4 / (num2 / 2f);
			color = this.vBEAT_ON - this.vBEATVALUE * num5;
		}
		else
		{
			color = this.vBEAT_OFF;
		}
		this.m_sLifeBack.color = color;
		this.m_sFeverBack.color = color;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00008C8E File Offset: 0x00006E8E
	private void GameOver()
	{
		if (!this.m_bGameOver)
		{
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_INGAME_GAME_OVER, false);
			this.PlayGameOver();
			Singleton<WWWManager>.instance.PostConfiguration();
			this.PostFailResult();
		}
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00038EC8 File Offset: 0x000370C8
	private void PlayGameOver()
	{
		this.m_bGameOver = true;
		this.m_fTime = 0f;
		this.m_cGame.enabled = false;
		this.m_cCoolBomb.enabled = false;
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie._filename = "GameOverTxt.mov";
		this.m_aMovie.LoadMovie(true);
		this.m_aMovie.transform.parent.GetComponent<UIPanel>().depth = 500;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00008CC5 File Offset: 0x00006EC5
	private void NextScene()
	{
		Singleton<SceneSwitcher>.instance.LoadNextScene("ThanksForPlaying");
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00038F4C File Offset: 0x0003714C
	private void PostFailResult()
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			WWWPostHausFailStage wwwpostHausFailStage = new WWWPostHausFailStage();
			wwwpostHausFailStage.CallBack = new WWWObject.CompleteCallBack(Singleton<WWWManager>.instance.CallBackPostHausFailTotalResult);
			Singleton<WWWManager>.instance.AddQueue(wwwpostHausFailStage);
			return;
		}
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			WWWPostFailRaveUp wwwpostFailRaveUp = new WWWPostFailRaveUp();
			Singleton<WWWManager>.instance.AddQueue(wwwpostFailRaveUp);
			return;
		}
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			WWWPostMissionFailScript wwwpostMissionFailScript = new WWWPostMissionFailScript();
			Singleton<WWWManager>.instance.AddQueue(wwwpostMissionFailScript);
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00008CD6 File Offset: 0x00006ED6
	private void Resume()
	{
		base.enabled = true;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00008CDF File Offset: 0x00006EDF
	private void OnDestory()
	{
		GC.Collect();
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00008CDF File Offset: 0x00006EDF
	private void OnApplicationQuit()
	{
		GC.Collect();
	}

	// Token: 0x040005D8 RID: 1496
	private const float MAXLOADTIME = 5f;

	// Token: 0x040005D9 RID: 1497
	private const float MAXDEMOTIME = 2.5f;

	// Token: 0x040005DA RID: 1498
	private ScorePlayer m_ScorePlayer = new ScorePlayer();

	// Token: 0x040005DB RID: 1499
	private Camera m_cGame;

	// Token: 0x040005DC RID: 1500
	private Camera m_cUi;

	// Token: 0x040005DD RID: 1501
	private Camera m_cCoolBomb;

	// Token: 0x040005DE RID: 1502
	private LoadingScript m_sLoading;

	// Token: 0x040005DF RID: 1503
	private ControlNoteScript m_sControlNote;

	// Token: 0x040005E0 RID: 1504
	private ControlEffectorScript m_sControlEffector;

	// Token: 0x040005E1 RID: 1505
	private ControlJudgmentScript m_sControlJudgment;

	// Token: 0x040005E2 RID: 1506
	private ControlFeverScript m_sControlFever;

	// Token: 0x040005E3 RID: 1507
	private ControlComboScript m_sControlCombo;

	// Token: 0x040005E4 RID: 1508
	private ControlLifeScript m_sControlLife;

	// Token: 0x040005E5 RID: 1509
	private GameObject m_oControlInput;

	// Token: 0x040005E6 RID: 1510
	private int m_iBeat;

	// Token: 0x040005E7 RID: 1511
	private int m_iPreTick;

	// Token: 0x040005E8 RID: 1512
	private bool m_bTouchRotate;

	// Token: 0x040005E9 RID: 1513
	private Vector3 m_vTouchPos = Vector3.zero;

	// Token: 0x040005EA RID: 1514
	private float m_fLastRot;

	// Token: 0x040005EB RID: 1515
	private UISprite m_sLifeBack;

	// Token: 0x040005EC RID: 1516
	private UISprite m_sFeverBack;

	// Token: 0x040005ED RID: 1517
	private GameObject m_oBack;

	// Token: 0x040005EE RID: 1518
	private GameObject m_oBackExtreme;

	// Token: 0x040005EF RID: 1519
	private InGameTouchInfo m_sTouch = new InGameTouchInfo();

	// Token: 0x040005F0 RID: 1520
	private ArrayList AllInput = new ArrayList();

	// Token: 0x040005F1 RID: 1521
	private Color vBEAT_ON = new Color(0.4f, 0.4f, 0.4f, 0.6f);

	// Token: 0x040005F2 RID: 1522
	private Color vBEAT_OFF = new Color(1f, 1f, 1f, 1f);

	// Token: 0x040005F3 RID: 1523
	private Color vBEATVALUE;

	// Token: 0x040005F4 RID: 1524
	private bool m_bGameOver;

	// Token: 0x040005F5 RID: 1525
	private bool m_bRaveUpHurdle = true;

	// Token: 0x040005F6 RID: 1526
	private GameObject m_oControlTest;

	// Token: 0x040005F7 RID: 1527
	private UILabel m_tTempInfo;

	// Token: 0x040005F8 RID: 1528
	private UILabel m_tTempInput;

	// Token: 0x040005F9 RID: 1529
	private float m_fTime;

	// Token: 0x040005FA RID: 1530
	public bool _Test;

	// Token: 0x040005FB RID: 1531
	public EFFECTOR_SPEED TESTSPEED;

	// Token: 0x040005FC RID: 1532
	public EFFECTOR_FADER TESTFADER;

	// Token: 0x040005FD RID: 1533
	public EFFECTOR_RAND TESTRAND;

	// Token: 0x040005FE RID: 1534
	public PLAYFNORMALITEM TESTNORMALITEM;

	// Token: 0x040005FF RID: 1535
	public PLAYFREFILLITEM TESTREFILLITEM;

	// Token: 0x04000600 RID: 1536
	public PLAYFSHIELDITEM TESTSHIELD;

	// Token: 0x04000601 RID: 1537
	private RESULTDATA m_rData;

	// Token: 0x04000602 RID: 1538
	private bool m_bCompleteLoad;

	// Token: 0x04000603 RID: 1539
	private bool m_bStartPlay;

	// Token: 0x04000604 RID: 1540
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000605 RID: 1541
	private UITexture m_aTexture;
}
