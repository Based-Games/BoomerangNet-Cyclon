using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class ControlJudgmentScript : MonoBehaviour
{
	// Token: 0x06000692 RID: 1682 RVA: 0x000084DE File Offset: 0x000066DE
	public void SetInput(InGameTouchInfo sTouch)
	{
		this.m_sTouch = sTouch;
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00033DF0 File Offset: 0x00031FF0
	private void Start()
	{
		this.m_cCamera = base.transform.FindChild("Camera").GetComponent<Camera>();
		this.m_rData = Singleton<GameManager>.instance.ResultData;
		PTLEVEL ptlevel = PTLEVEL.EZ;
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			ptlevel = Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage].PtType;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			ptlevel = Singleton<SongManager>.instance.RaveUpSelectSong[GameData.Stage].PtType;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			ptlevel = Singleton<SongManager>.instance.Mission.Pattern[GameData.Stage];
		}
		this.PerfectAdd = GameData.PerfectAdd;
		if (currentDisc.DicPtInfo.ContainsKey(ptlevel))
		{
			if (ptlevel == PTLEVEL.S1 || ptlevel == PTLEVEL.S2)
			{
				this.PerfectAdd = GameData.PerfectS1S2Add;
			}
			else
			{
				DiscInfo.PtInfo ptInfo = currentDisc.DicPtInfo[ptlevel];
				if (5 >= ptInfo.iDif)
				{
					this.PerfectAdd = GameData.Perfect5LvAdd;
				}
				else if (10 <= ptInfo.iDif)
				{
					this.PerfectAdd = GameData.Perfect10LvAdd;
				}
			}
		}
		this.m_iGroup = Singleton<SongManager>.instance.GetCurrentDisc().GroupSet;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x000084E7 File Offset: 0x000066E7
	private void Update()
	{
		this.UpdateJudgmentColor();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x000084EF File Offset: 0x000066EF
	private void UpdateJudgmentColor()
	{
		if (this.m_iJudgmentPer == 0)
		{
			this.m_sNumPerfect.color = this.JUDGMENT_RAINBOW;
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00033F1C File Offset: 0x0003211C
	public void SetObject()
	{
		GameObject gameObject = base.transform.FindChild("Ui").gameObject;
		GameObject gameObject2 = gameObject.transform.FindChild("Control").gameObject;
		GameObject gameObject3 = gameObject2.transform.FindChild("ControlJudgment").gameObject;
		this.m_sNumJudgment = gameObject3.transform.FindChild("NumScore").GetComponent<UISprite>();
		this.m_sNumPerfect = this.m_sNumJudgment.transform.FindChild("NumPerfect").GetComponent<UISprite>();
		this.m_sViewJudgment = gameObject3.transform.FindChild("ViewJudgment").GetComponent<UISprite>();
		this.m_sBackGage = gameObject.transform.FindChild("Back").transform.FindChild("BackHole").GetComponent<UISprite>();
		this.m_sBackLine = gameObject.transform.FindChild("Back").transform.FindChild("BackLine").GetComponent<UISprite>();
		this.m_sControlScore = gameObject2.transform.FindChild("InfoStage").GetComponent<ControlScoreScript>();
		this.m_sControlScore.SetObject();
		this.m_sControlCombo = gameObject2.transform.FindChild("ControlCombo").GetComponent<ControlComboScript>();
		this.m_sControlCoolBomb = base.transform.FindChild("ControlCoolBomb").GetComponent<ControlCoolBombScript>();
		this.m_sControlCoolBomb.SetGameManager(this.m_sGameManager);
		this.m_sControlFever = gameObject2.transform.FindChild("ControlFever").GetComponent<ControlFeverScript>();
		this.m_sControlTotalGage = gameObject2.transform.FindChild("ControlTotalGage").GetComponent<ControlTotalGageScript>();
		this.m_sControlEffector = gameObject2.transform.FindChild("Effector").GetComponent<ControlEffectorScript>();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0000850A File Offset: 0x0000670A
	public void SetGameManager(GameManagerScript sManager)
	{
		this.m_sGameManager = sManager;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00008513 File Offset: 0x00006713
	public void SetScorePlayer(ScorePlayer sPlayer)
	{
		this.m_ScorePlayer = sPlayer;
		this.m_trackEvt = this.m_ScorePlayer.AllTrackEvt;
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0000852D File Offset: 0x0000672D
	public void SetTrack(SPlayEvtList[] arrTrack)
	{
		this.m_trackEvt = arrTrack;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x000340D4 File Offset: 0x000322D4
	public void UpdateJudgment()
	{
		this.m_iCurTick = this.m_ScorePlayer.GetCurTick();
		for (int i = 0; i < 12; i++)
		{
			ArrayList evtView = this.m_trackEvt[i].evtView;
			int j = 0;
			while (j < evtView.Count)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtView[j];
				if (scoreEventBase != null && scoreEventBase.IsNoneState())
				{
					if (!this.m_ScorePlayer.IsInJudgmentRangeTick(this.m_iCurTick, scoreEventBase.Tick))
					{
						break;
					}
					if (scoreEventBase.IsMoveNote())
					{
						this.JudgmentMoveNote(scoreEventBase);
						break;
					}
					if (scoreEventBase.IsLongNote())
					{
						this.JudgmentLongNote(scoreEventBase);
						break;
					}
					if (scoreEventBase.IsDirNote())
					{
						if (this.JudgmentDirNote(scoreEventBase))
						{
							break;
						}
						break;
					}
					else
					{
						if (this.JudgmentNormalNote(scoreEventBase))
						{
							break;
						}
						break;
					}
				}
				else
				{
					j++;
				}
			}
		}
		this.UpdateViewJudgment();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000341A0 File Offset: 0x000323A0
	private void UpdateViewJudgment()
	{
		if (0 > this.m_iJudgmentPer)
		{
			return;
		}
		Vector3 localScale = this.m_sNumJudgment.transform.localScale;
		this.m_sNumJudgment.transform.localScale = Vector3.MoveTowards(localScale, Vector3.one, Time.deltaTime * 80f);
		Vector3 vector = GameData.ViewJudgmentScale - Vector3.one;
		float num = (localScale.x - 1f) / vector.x;
		Vector3[] arr_JUDGMENTVIEW_START = this.ARR_JUDGMENTVIEW_START;
		int iJudgmentPer = this.m_iJudgmentPer;
		Vector3 vector2 = this.ARR_JUDGMENTVIEW_END[this.m_iJudgmentPer];
		float num2 = (arr_JUDGMENTVIEW_START[iJudgmentPer].x - vector2.x) * num;
		Vector3 vector3 = vector2;
		vector3.x += num2;
		this.m_sViewJudgment.transform.localPosition = vector3;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00008536 File Offset: 0x00006736
	private bool GetOnExtreme(ScoreEventBase pEvt)
	{
		return this.m_rData.ONEXTREME && null != pEvt.objControlNote && GameData.OnExtremeAngle(pEvt.objControlNote.transform.localEulerAngles.z);
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0003426C File Offset: 0x0003246C
	private void PlayNoteSound(ScoreEventBase pEvt)
	{
		if (pEvt.objControlNote)
		{
			if (pEvt.MULTILINE)
			{
				ArrayList sameInfo = pEvt.SameInfo;
				if (pEvt.NOT_SOUND)
				{
					return;
				}
				foreach (object obj in sameInfo)
				{
					ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
					if (scoreEventBase.NOT_SOUND)
					{
						return;
					}
					scoreEventBase.NOT_SOUND = true;
				}
				Singleton<SoundSourceManager>.instance.Effect(this.m_iGroup);
				return;
			}
			else
			{
				if (pEvt.NOT_SOUND)
				{
					return;
				}
				Singleton<SoundSourceManager>.instance.Effect(this.m_iGroup);
				pEvt.NOT_SOUND = true;
			}
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0000856F File Offset: 0x0000676F
	private bool SetShieldItem()
	{
		if (GameData.SHIELDITEM != PLAYFSHIELDITEM.NONE)
		{
			GameData.SHIELDITEM--;
			this.m_sControlEffector.StartShieldAnimation();
			return true;
		}
		return false;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00008593 File Offset: 0x00006793
	private IEnumerator SuccessNormalNote(ScoreEventBase pEvt)
	{
		this.PlayNoteSound(pEvt);
		int iPreCurTick = this.m_iCurTick;
		Vector3 vPos = pEvt.objControlNote.transform.FindChild("Note").transform.position;
		Vector3 vRot = pEvt.objControlNote.transform.localEulerAngles;
		bool bExtreme = this.GetOnExtreme(pEvt);
		yield return null;
		this.Notify_Success(pEvt, bExtreme, iPreCurTick);
		this.m_sControlCoolBomb.EffectNormalCoolBomb(pEvt, bExtreme, vPos, vRot);
		pEvt.EndView = true;
		yield break;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00034324 File Offset: 0x00032524
	private bool JudgmentNormalNote(ScoreEventBase pEvt)
	{
		int num = this.m_iCurTick - this.m_ScorePlayer.MSToTick(5);
		if (!this.m_ScorePlayer.IsInMaximumTick(num, pEvt.Tick))
		{
			if (!this.SetShieldItem())
			{
				this.Notify_Failed(pEvt);
				return true;
			}
			pEvt.AutoCheck = true;
		}
		if ((GameData.INGAME_AUTO || pEvt.AutoCheck) && num > pEvt.Tick)
		{
			base.StartCoroutine(this.SuccessNormalNote(pEvt));
			return true;
		}
		if (this.m_ScorePlayer.GetKeyState(pEvt.Track) == KEYSTATE.BEGAN)
		{
			base.StartCoroutine(this.SuccessNormalNote(pEvt));
			return true;
		}
		return false;
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x000085A9 File Offset: 0x000067A9
	private IEnumerator SuccessAutoDirNote(ScoreEventBase pEvt, Vector3 vPos)
	{
		this.PlayNoteSound(pEvt);
		int iPreCurTick = this.m_iCurTick;
		bool bExtreme = this.GetOnExtreme(pEvt);
		yield return null;
		this.Notify_Success(pEvt, bExtreme, iPreCurTick);
		this.m_sControlCoolBomb.EffectSwingCoolBomb(pEvt, vPos, bExtreme, pEvt.Attr == 11);
		pEvt.EndView = true;
		yield break;
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x000085C6 File Offset: 0x000067C6
	private IEnumerator SuccessDirNote(ScoreEventBase pEvt, int iCurTick, bool bIn)
	{
		this.PlayNoteSound(pEvt);
		bool bExtreme = this.GetOnExtreme(pEvt);
		if (this.m_rData.ONEXTREME && GameData.OnExtremeAngle(pEvt.objControlNote.transform.localEulerAngles.z))
		{
			bExtreme = true;
		}
		Vector3 vPos = pEvt.objControlNote.GetComponent<DirNoteScript>().GetNotePos();
		yield return null;
		pEvt.SetJudgment(pEvt.JudgmentStart);
		this.NotiJudgment(pEvt, 0U, pEvt.Track, bExtreme);
		uint judgmentPer = this.m_ScorePlayer.GetJudgmentPer(iCurTick, pEvt.Tick);
		this.ViewJudgment(judgmentPer, bIn);
		this.m_sControlCoolBomb.EffectSwingCoolBomb(pEvt, vPos, bExtreme, pEvt.Attr == 11);
		pEvt.EndView = true;
		yield break;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x000343C0 File Offset: 0x000325C0
	private bool JudgmentDirNote(ScoreEventBase pEvt)
	{
		int num = this.m_iCurTick - this.m_ScorePlayer.MSToTick(5);
		bool flag = true;
		if (num > pEvt.Tick)
		{
			flag = false;
		}
		if (!this.m_ScorePlayer.IsInMaximumTick(num, pEvt.Tick))
		{
			if (!this.SetShieldItem())
			{
				this.Notify_Failed(pEvt);
				return true;
			}
			pEvt.AutoCheck = true;
		}
		if ((GameData.INGAME_AUTO || pEvt.AutoCheck) && num > pEvt.Tick)
		{
			Vector3 notePos = pEvt.objControlNote.GetComponent<DirNoteScript>().GetNotePos();
			base.StartCoroutine(this.SuccessAutoDirNote(pEvt, notePos));
			return true;
		}
		if (pEvt.JudgmentStart == JUDGMENT_TYPE.JUDGMENT_NONE)
		{
			if (this.m_ScorePlayer.GetKeyState(pEvt.Track) == KEYSTATE.BEGAN)
			{
				uint judgmentPer = this.m_ScorePlayer.GetJudgmentPer(num, pEvt.Tick);
				JUDGMENT_TYPE judgmentType = this.m_ScorePlayer.GetJudgmentType(judgmentPer, flag);
				if (judgmentType != JUDGMENT_TYPE.BREAK)
				{
					pEvt.JudgmentStart = judgmentType;
					return false;
				}
				this.Notify_Failed(pEvt);
				return true;
			}
		}
		else if (this.m_ScorePlayer.GetKeyState(pEvt.Track) == KEYSTATE.MOVE)
		{
			if (ConfigManager.Instance.Get<string>("game.input", false).Equals("real_io"))
			{
				Vector3 keyFirstPos = this.m_ScorePlayer.GetKeyFirstPos(pEvt.Track);
				float angle = Singleton<GameManager>.instance.GetAngle(Vector3.zero, new Vector3(keyFirstPos.x, keyFirstPos.y, 0f));
				Vector3 keyMovePos = this.m_ScorePlayer.GetKeyMovePos(pEvt.Track);
				float angle2 = Singleton<GameManager>.instance.GetAngle(Vector3.zero, new Vector3(keyMovePos.x, keyMovePos.y, 0f));
				bool flag2 = false;
				if (pEvt.IsDirDnNote())
				{
					if (angle + GameData.DIRMOVE < angle2)
					{
						flag2 = true;
					}
				}
				else if (angle - GameData.DIRMOVE > angle2)
				{
					flag2 = true;
				}
				if (flag2)
				{
					base.StartCoroutine(this.SuccessDirNote(pEvt, num, flag));
					return true;
				}
			}
			else
			{
				Vector3 keyFirstPos2 = this.m_ScorePlayer.GetKeyFirstPos(pEvt.Track);
				float num2 = keyFirstPos2.x / (float)Screen.width * 1280f;
				float num3 = keyFirstPos2.y / (float)Screen.height * 720f;
				float angle3 = Singleton<GameManager>.instance.GetAngle(Vector3.zero, new Vector3(num2, num3, 0f));
				Vector3 keyMovePos2 = this.m_ScorePlayer.GetKeyMovePos(pEvt.Track);
				float num4 = keyMovePos2.x / (float)Screen.width * 1280f;
				float num5 = keyMovePos2.y / (float)Screen.height * 720f;
				float angle4 = Singleton<GameManager>.instance.GetAngle(Vector3.zero, new Vector3(num4, num5, 0f));
				bool flag3 = false;
				if (pEvt.IsDirDnNote())
				{
					if (angle3 + GameData.DIRMOVE < angle4)
					{
						flag3 = true;
					}
				}
				else if (angle3 - GameData.DIRMOVE > angle4)
				{
					flag3 = true;
				}
				if (flag3)
				{
					base.StartCoroutine(this.SuccessDirNote(pEvt, num, flag));
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000085EA File Offset: 0x000067EA
	private IEnumerator SuccessLongNote(ScoreEventBase pEvt, int iCurTick, bool bIn)
	{
		this.PlayNoteSound(pEvt);
		bool bExtreme = this.GetOnExtreme(pEvt);
		int iTrack = pEvt.Track;
		yield return null;
		uint judgmentPer = this.m_ScorePlayer.GetJudgmentPer(iCurTick, pEvt.Tick);
		JUDGMENT_TYPE judgmentType = this.m_ScorePlayer.GetJudgmentType(judgmentPer, bIn);
		pEvt.JudgmentStart = judgmentType;
		this.ViewJudgment(judgmentPer, bIn);
		pEvt.iComboCount++;
		this.IncCombo(bExtreme);
		this.LongEffectJudgment();
		this.m_sControlFever.AddFever(GameData.FeverAddGage[4]);
		if (pEvt.JudgmentEnd != JUDGMENT_TYPE.BREAK)
		{
			Vector3 localEulerAngles = pEvt.objControlNote.transform.localEulerAngles;
			this.m_sControlCoolBomb.EffectLongLineCoolBomb(iTrack, localEulerAngles, bExtreme);
		}
		yield break;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000346BC File Offset: 0x000328BC
	private bool JudgmentLongNote(ScoreEventBase pEvt)
	{
		int num = this.m_iCurTick - this.m_ScorePlayer.MSToTick(5);
		int track = pEvt.Track;
		bool flag = true;
		if (num > pEvt.Tick)
		{
			flag = false;
		}
		if (pEvt.JudgmentStart == JUDGMENT_TYPE.JUDGMENT_NONE)
		{
			if (!this.m_ScorePlayer.IsInMaximumTick(num, pEvt.Tick))
			{
				if (!this.SetShieldItem())
				{
					this.Notify_Failed(pEvt);
					return true;
				}
				pEvt.AutoCheck = true;
			}
			if (this.m_ScorePlayer.GetKeyState(track) == KEYSTATE.BEGAN)
			{
				base.StartCoroutine(this.SuccessLongNote(pEvt, num, flag));
				return false;
			}
			if ((GameData.INGAME_AUTO || pEvt.AutoCheck) && num > pEvt.Tick)
			{
				base.StartCoroutine(this.SuccessLongNote(pEvt, num, flag));
				return false;
			}
		}
		else
		{
			bool onExtreme = this.GetOnExtreme(pEvt);
			if (pEvt.GetEndTick() <= num)
			{
				pEvt.SetJudgment(pEvt.JudgmentStart);
				this.NotiJudgment(pEvt, 0U, pEvt.Track, onExtreme);
				pEvt.EndView = true;
				this.m_sControlCoolBomb.StopLongLineCoolBomb(track);
				this.m_sControlCoolBomb.EffectLongEndCoolBomb(track, onExtreme);
				return true;
			}
			int num2 = pEvt.Tick + pEvt.iComboCount * GameData.LONGNOTE_TIMECOUNT;
			if (num > num2)
			{
				pEvt.iComboCount++;
				this.IncCombo(onExtreme);
				this.ViewJudgment(100U, flag);
				this.LongEffectJudgment();
				this.m_sControlFever.AddFever(GameData.FeverAddGage[4]);
				Vector3 notePos = pEvt.objControlNote.GetComponent<LongNoteScript>().GetNotePos();
				Vector3 localEulerAngles = pEvt.objControlNote.transform.localEulerAngles;
				this.m_sControlCoolBomb.MoveLongCoolBomb(pEvt.Track, notePos, onExtreme, localEulerAngles);
			}
			if (GameData.INGAME_AUTO || pEvt.AutoCheck)
			{
				return false;
			}
			int num3 = GameData.CONVERT_VALUE * GameData.MINAUTOLONGNOTE;
			int num4 = GameData.CONVERT_VALUE * GameData.AUTOLONGNOTE;
			int num5 = pEvt.Tick + num3;
			int num6 = pEvt.GetEndTick() - num4;
			if (num5 > num6)
			{
				num6 = num5;
			}
			if (num6 < this.m_iCurTick)
			{
				return false;
			}
			if (this.m_ScorePlayer.GetKeyState(track) == KEYSTATE.NONE)
			{
				pEvt.bLongFail = true;
				pEvt.iTickFail = num;
				this.Notify_Failed(pEvt);
				this.m_sControlCoolBomb.StopLongLineCoolBomb(pEvt.Track);
				this.m_sControlCoolBomb.EffectLongEndCoolBomb(track, onExtreme);
			}
		}
		return false;
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0000860E File Offset: 0x0000680E
	private IEnumerator SuccessMoveNote(ScoreEventBase pEvt, int iCurTick, bool bIn)
	{
		this.PlayNoteSound(pEvt);
		bool bExtreme = this.GetOnExtreme(pEvt);
		int iTrack = pEvt.Track;
		yield return null;
		uint judgmentPer = this.m_ScorePlayer.GetJudgmentPer(iCurTick, pEvt.Tick);
		JUDGMENT_TYPE judgmentType = this.m_ScorePlayer.GetJudgmentType(judgmentPer, bIn);
		pEvt.JudgmentStart = judgmentType;
		this.ViewJudgment(judgmentPer, bIn);
		this.m_sTouch.AddInterVal(this.m_sTouch.MultiInputPos[iTrack], pEvt.Tick * iTrack);
		pEvt.iComboCount++;
		this.IncCombo(bExtreme);
		if (pEvt.JudgmentEnd != JUDGMENT_TYPE.BREAK && (pEvt.Attr == 1 || pEvt.Attr == 5))
		{
			Vector3 localEulerAngles = pEvt.objControlNote.transform.localEulerAngles;
			this.m_sControlCoolBomb.EffectJogLineCoolBomb(pEvt.Track, localEulerAngles, bExtreme);
		}
		yield break;
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000348E8 File Offset: 0x00032AE8
	private bool JudgmentMoveNote(ScoreEventBase pEvt)
	{
		if (pEvt == null)
		{
			return false;
		}
		int num = this.m_iCurTick - this.m_ScorePlayer.MSToTick(5);
		int track = pEvt.Track;
		bool flag = true;
		if (num > pEvt.Tick)
		{
			flag = false;
		}
		if (pEvt.JudgmentStart == JUDGMENT_TYPE.JUDGMENT_NONE)
		{
			if (!this.m_ScorePlayer.IsInMaximumTick(num, pEvt.Tick))
			{
				if (!this.SetShieldItem())
				{
					this.Notify_Failed(pEvt);
					return true;
				}
				pEvt.AutoCheck = true;
			}
			if ((GameData.INGAME_AUTO || pEvt.AutoCheck) && num > pEvt.Tick)
			{
				base.StartCoroutine(this.SuccessMoveNote(pEvt, num, flag));
				return false;
			}
			if (this.m_ScorePlayer.GetKeyState(track) == KEYSTATE.BEGAN)
			{
				base.StartCoroutine(this.SuccessMoveNote(pEvt, num, flag));
				return false;
			}
		}
		else
		{
			bool flag2 = this.GetOnExtreme(pEvt);
			if (pEvt.GetEndTick() > num)
			{
				if (null != pEvt.objControlNote)
				{
					if (pEvt.JudgmentStart != JUDGMENT_TYPE.BREAK)
					{
						if (this.m_rData.ONEXTREME && GameData.OnExtremeAngle(pEvt.objControlNote.transform.localEulerAngles.z))
						{
							flag2 = true;
						}
						bool flag3 = false;
						bool flag4 = false;
						Vector3 vector = pEvt.objControlNote.GetComponent<MoveNoteScript>().GetNotePos();
						for (int i = 0; i < 12; i++)
						{
							if (this.m_sTouch.MultiCheckInput[i])
							{
								Vector3 vector2 = this.m_sTouch.MultiInputPos[i];
								Vector3 vector3 = this.m_cCamera.WorldToScreenPoint(vector);
								Vector3 vector4 = vector2;
								float num2 = GameData.JogJudgmentDistance * pEvt.DurRate;
								if (ConfigManager.Instance.Get<string>("game.input", false).Equals("real_io"))
								{
									if (GameData.FLIP)
									{
										vector4.x = (float)Screen.width - vector4.x;
									}
								}
								else
								{
									num2 = num2 / 1280f * (float)Screen.width;
								}
								float num3 = Vector3.Distance(vector4, vector3);
								if (num2 > num3)
								{
									this.m_sTouch.strInfo = string.Format("dis:{0}, judDis:{1} Rate:{2}", num3, GameData.JogJudgmentDistance, pEvt.DurRate);
									flag3 = true;
									if (this.m_sTouch.MultiCheckInput[i])
									{
										this.m_ScorePlayer.SetKeyState(i, KEYSTATE.MOVE);
									}
								}
								if (flag3)
								{
									if (this.m_sTouch.ContainInterValKey(pEvt.Tick * track))
									{
										if (this.m_sTouch.IsSamePos(vector2, pEvt.Tick * track))
										{
											flag4 = true;
											flag3 = false;
										}
									}
									else
									{
										this.m_sTouch.AddInterVal(vector2, pEvt.Tick * track);
									}
								}
							}
						}
						if (!flag3 && flag4)
						{
							this.m_sTouch.strInfo = "SameFail";
						}
						if (GameData.INGAME_AUTO || pEvt.AutoCheck)
						{
							flag3 = true;
						}
						int num4 = GameData.CONVERT_VALUE * GameData.MINAUTOLONGNOTE;
						int num5 = GameData.CONVERT_VALUE * GameData.AUTOLONGNOTE;
						int num6 = pEvt.Tick + num4;
						int num7 = pEvt.GetEndTick() - num5;
						if (num6 > num7)
						{
							num7 = num6;
						}
						if (num7 < this.m_iCurTick && !flag4)
						{
							flag3 = true;
						}
						if (!flag3 && this.SetShieldItem())
						{
							pEvt.AutoCheck = true;
						}
						if (pEvt.AutoCheck)
						{
							flag3 = true;
						}
						if (flag3)
						{
							Vector3 localEulerAngles = pEvt.objControlNote.transform.localEulerAngles;
							this.m_sControlCoolBomb.MoveJogCoolBomb(pEvt.Track, vector, flag2, localEulerAngles);
							int num8 = pEvt.Tick + pEvt.iComboCount * GameData.LONGNOTE_TIMECOUNT;
							if (num > num8)
							{
								pEvt.iComboCount++;
								this.m_sControlFever.AddFever(GameData.FeverAddGage[4]);
								this.IncCombo(flag2);
								this.ViewJudgment(100U, flag);
								this.LongEffectJudgment();
							}
						}
						else
						{
							if (pEvt.JudgmentStart == JUDGMENT_TYPE.JUDGMENT_NONE)
							{
								vector = pEvt.objControlNote.GetComponent<MoveNoteScript>().GetNotePos();
								this.m_sControlCoolBomb.EffectJogEndCoolBomb(pEvt.Track, vector, flag2);
							}
							pEvt.JudgmentStart = JUDGMENT_TYPE.BREAK;
							this.m_sControlCoolBomb.StopJogCoolBomb(pEvt.Track);
							pEvt.SetJudgment(pEvt.JudgmentStart);
							this.NotiJudgment(pEvt, 0U, pEvt.Track, flag2);
							this.Notify_Failed(pEvt);
						}
					}
					ArrayList moveInfo = pEvt.MoveInfo;
					for (int j = 0; j < moveInfo.Count; j++)
					{
						ScoreEventBase scoreEventBase = (ScoreEventBase)moveInfo[j];
						if (scoreEventBase.IsNoneState() && num > scoreEventBase.Tick)
						{
							scoreEventBase.SetJudgment(JUDGMENT_TYPE.POOR);
						}
					}
				}
			}
			else
			{
				this.m_sControlCoolBomb.StopJogCoolBomb(pEvt.Track);
				Vector3 notePos = pEvt.objControlNote.GetComponent<MoveNoteScript>().GetNotePos();
				if (pEvt.JudgmentStart != JUDGMENT_TYPE.BREAK)
				{
					this.m_sControlCoolBomb.EffectJogEndCoolBomb(pEvt.Track, notePos, flag2);
				}
				if (null != pEvt.objControlNote)
				{
					if (pEvt.JudgmentEnd == JUDGMENT_TYPE.JUDGMENT_NONE)
					{
						pEvt.SetJudgment(pEvt.JudgmentStart);
						this.NotiJudgment(pEvt, 0U, pEvt.Track, flag2);
					}
					pEvt.EndView = true;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00034DB8 File Offset: 0x00032FB8
	public void Notify_Success(ScoreEventBase pEvt, bool bExtreme, int iPreCurTick)
	{
		int num = iPreCurTick - this.m_ScorePlayer.MSToTick(5);
		bool flag = true;
		if (num > pEvt.Tick)
		{
			flag = false;
		}
		if (pEvt.IsNoneState())
		{
			uint judgmentPer = this.m_ScorePlayer.GetJudgmentPer(num, pEvt.Tick);
			JUDGMENT_TYPE judgmentType = this.m_ScorePlayer.GetJudgmentType(judgmentPer, flag);
			pEvt.SetJudgment(judgmentType);
			this.NotiJudgment(pEvt, 0U, pEvt.Track, bExtreme);
			this.ViewJudgment(judgmentPer, flag);
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00034E28 File Offset: 0x00033028
	public void Notify_Failed(ScoreEventBase pEvt)
	{
		if (null != pEvt.objControlNote)
		{
			pEvt.objControlNote.SendMessage("SetFail");
		}
		pEvt.SetJudgment(JUDGMENT_TYPE.BREAK);
		this.NotiJudgment(pEvt, 0U, pEvt.Track, false);
		this.ViewJudgment(0U, false);
		this.NotyFailedView();
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00034E78 File Offset: 0x00033078
	public void NotyFailedView()
	{
		this.m_sBackGage.color = Color.red;
		this.m_sBackLine.color = Color.red;
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = Color.red;
		hashtable["to"] = Color.white;
		hashtable["time"] = 0.3f;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "UpdateColor";
		iTween.ValueTo(this.m_sBackGage.gameObject, hashtable);
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00008632 File Offset: 0x00006832
	private void UpdateColor(Color cBack)
	{
		this.m_sBackGage.color = cBack;
		this.m_sBackLine.color = cBack;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00034F1C File Offset: 0x0003311C
	public void ForceNotify_Success(ScoreEventBase pEvt, bool bExtreme)
	{
		JUDGMENT_TYPE judgment_TYPE = JUDGMENT_TYPE.POOR;
		pEvt.SetJudgment(judgment_TYPE);
		this.NotiJudgment(pEvt, 0U, pEvt.Track, bExtreme);
		this.ViewJudgment(100U, true);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00034F4C File Offset: 0x0003314C
	private void LongEffectJudgment()
	{
		this.m_sViewJudgment.spriteName = "txtSuperb";
		this.m_sViewJudgment.MakePixelPerfect();
		Vector3 vector = this.ARR_JUDGMENTVIEW_START[0];
		this.m_sViewJudgment.transform.localPosition = vector;
		this.m_iJudgmentPer = 0;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0000864C File Offset: 0x0000684C
	private void FailCombo()
	{
		this.m_rData.COMBO = 0;
		this.m_rData.EXTREMECOMBO = 0;
		this.m_sControlCombo.CHAOSCOMBO = 0;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00034F9C File Offset: 0x0003319C
	public void NotiJudgment(ScoreEventBase pEvt, uint uTick, int iTrack, bool bExtreme)
	{
		int num = this.m_rData.SCORE;
		this.m_sControlCombo.SetPreCombo();
		int num2 = GameData.Judgment_Score[(int)pEvt.JudgmentEnd];
		if (bExtreme && this.m_rData.EXTREMESTATE != EXTREME_STATE.NONE)
		{
			float num3 = (float)num2 * GameData.EXTREME_BONUSRATE[(int)this.m_rData.EXTREMESTATE];
			this.m_rData.EXTREMEBONUS += (int)num3;
		}
		num += num2;
		if (pEvt.JudgmentEnd != JUDGMENT_TYPE.BREAK)
		{
			this.IncCombo(false);
			if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_UP || GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_DN)
			{
				ControlComboScript sControlCombo = this.m_sControlCombo;
				int chaoscombo = sControlCombo.CHAOSCOMBO;
				sControlCombo.CHAOSCOMBO = chaoscombo + 1;
			}
		}
		switch (pEvt.JudgmentEnd)
		{
		case JUDGMENT_TYPE.BREAK:
			this.FailCombo();
			this.m_rData.LIFE_GAGE -= GameData.BreakeMinus;
			break;
		case JUDGMENT_TYPE.POOR:
			this.m_rData.LIFE_GAGE -= GameData.PoorMinus;
			break;
		case JUDGMENT_TYPE.PERFECT:
			this.m_rData.LIFE_GAGE += this.PerfectAdd;
			if (GameData.MAXENERGY <= this.m_rData.LIFE_GAGE)
			{
				this.m_rData.LIFE_GAGE = GameData.MAXENERGY;
			}
			break;
		}
		if (pEvt.JudgmentEnd != JUDGMENT_TYPE.BREAK)
		{
			this.m_sControlFever.AddFever(GameData.FeverAddGage[(int)pEvt.JudgmentEnd]);
		}
		else if (ControlFeverScript.m_eState == FEVER_STATE.NONE)
		{
			this.m_sControlFever.AddFever(GameData.FeverAddGage[(int)pEvt.JudgmentEnd]);
		}
		else
		{
			this.m_sControlFever.MinusFeverTime();
		}
		this.SetEffectChaosSpeed();
		this.m_rData.SCORE = num;
		this.m_rData.SetJudgment(pEvt.JudgmentEnd);
		this.m_sControlTotalGage.SetGage();
		this.m_sControlScore.ViewScore();
		this.m_sControlCombo.SetCombo();
		this.SetEffectChaosSpeed();
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00035170 File Offset: 0x00033370
	public void SetEffectChaosSpeed()
	{
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_UP)
		{
			if (this.m_sControlCombo.CHAOSCOMBO % 20 == 0)
			{
				int num = this.m_sControlCombo.CHAOSCOMBO / 20;
				if (GameData.CHAOS_UP_SPEED.Length <= num)
				{
					num = 0;
					this.m_sControlCombo.CHAOSCOMBO = 0;
				}
				if (GameData.INGAMGE_SPEED != num)
				{
					GameData.INGAMGE_SPEED = num;
					this.m_sGameManager.SetChangeTps();
					return;
				}
			}
		}
		else if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_DN && this.m_sControlCombo.CHAOSCOMBO % 20 == 0)
		{
			int num2 = this.m_sControlCombo.CHAOSCOMBO / 20;
			if (GameData.CHAOS_DN_SPEED.Length <= num2)
			{
				num2 = 0;
				this.m_sControlCombo.CHAOSCOMBO = 0;
			}
			if (GameData.INGAMGE_SPEED != num2)
			{
				GameData.INGAMGE_SPEED = num2;
				this.m_sGameManager.SetChangeTps();
			}
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00008672 File Offset: 0x00006872
	public void IncCombo(bool bExtreme = false)
	{
		this.m_sControlCombo.IncCombo();
		this.m_sControlCombo.SetCombo();
		if (bExtreme && this.m_rData.EXTREMESTATE != EXTREME_STATE.NONE)
		{
			this.m_rData.EXTREMEBONUS += GameData.MAXCOMBO_SCORE;
		}
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00035234 File Offset: 0x00033434
	public void ViewJudgment(uint uPer, bool bIn)
	{
		string[] array = new string[]
		{
			"Score100", "Score99", "Score95", "Score90", "Score80", "Score70", "Score60", "Score50", "Score40", "Score30",
			"Score20", "Score10", "Score1", "Score0"
		};
		string[] array2 = new string[]
		{
			"txtSuperb", "txtPerfect", "txtPerfect", "txtGreat", "txtGreat", "txtGood", "txtGood", "txtGood", "txtGood", "txtPoor",
			"txtPoor", "txtPoor", "txtPoor", "txtBreak"
		};
		int num = 0;
		if (bIn)
		{
			for (int i = 0; i < 14; i++)
			{
				if (uPer >= (uint)GameData.InJudgment_View[i])
				{
					num = i;
					break;
				}
			}
		}
		else
		{
			for (int j = 0; j < 14; j++)
			{
				if (uPer >= (uint)GameData.OutJudgment_View[j])
				{
					num = j;
					break;
				}
			}
		}
		this.m_iJudgmentPer = num;
		this.m_sNumJudgment.color = Color.white;
		this.m_sViewJudgment.color = Color.white;
		this.m_sNumJudgment.spriteName = array[num];
		this.m_sNumJudgment.MakePixelPerfect();
		this.m_sNumJudgment.transform.localScale = GameData.ViewJudgmentScale;
		if (num == 0)
		{
			this.m_sNumPerfect.enabled = true;
		}
		else
		{
			this.m_sNumPerfect.enabled = false;
		}
		Vector3 vector = this.ARR_JUDGMENTVIEW_START[num];
		this.m_sViewJudgment.transform.localPosition = vector;
		this.m_sViewJudgment.spriteName = array2[num];
		this.m_sViewJudgment.MakePixelPerfect();
	}

	// Token: 0x04000529 RID: 1321
	private const int NOT_LIFEADD_LEVEL = 10;

	// Token: 0x0400052A RID: 1322
	private const int LIFEADD_LEVEL5 = 5;

	// Token: 0x0400052B RID: 1323
	private Camera m_cCamera;

	// Token: 0x0400052C RID: 1324
	private ScorePlayer m_ScorePlayer;

	// Token: 0x0400052D RID: 1325
	private SPlayEvtList[] m_trackEvt;

	// Token: 0x0400052E RID: 1326
	private GameManagerScript m_sGameManager;

	// Token: 0x0400052F RID: 1327
	private ControlScoreScript m_sControlScore;

	// Token: 0x04000530 RID: 1328
	private ControlComboScript m_sControlCombo;

	// Token: 0x04000531 RID: 1329
	private ControlCoolBombScript m_sControlCoolBomb;

	// Token: 0x04000532 RID: 1330
	private ControlFeverScript m_sControlFever;

	// Token: 0x04000533 RID: 1331
	private ControlTotalGageScript m_sControlTotalGage;

	// Token: 0x04000534 RID: 1332
	private ControlEffectorScript m_sControlEffector;

	// Token: 0x04000535 RID: 1333
	private UISprite m_sBackGage;

	// Token: 0x04000536 RID: 1334
	private UISprite m_sBackLine;

	// Token: 0x04000537 RID: 1335
	private UISprite m_sNumJudgment;

	// Token: 0x04000538 RID: 1336
	private UISprite m_sNumPerfect;

	// Token: 0x04000539 RID: 1337
	private UISprite m_sViewJudgment;

	// Token: 0x0400053A RID: 1338
	private InGameTouchInfo m_sTouch;

	// Token: 0x0400053B RID: 1339
	private int m_iCurTick;

	// Token: 0x0400053C RID: 1340
	public Color JUDGMENT_RAINBOW = Color.white;

	// Token: 0x0400053D RID: 1341
	public float fTestJudgment;

	// Token: 0x0400053E RID: 1342
	public float fTestDistance;

	// Token: 0x0400053F RID: 1343
	public Vector3 vNotePos = Vector3.zero;

	// Token: 0x04000540 RID: 1344
	private Vector3[] ARR_JUDGMENTVIEW_START = new Vector3[]
	{
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 65f, 0f),
		new Vector3(155f, 50f, 0f),
		new Vector3(155f, 50f, 0f)
	};

	// Token: 0x04000541 RID: 1345
	private Vector3[] ARR_JUDGMENTVIEW_END = new Vector3[]
	{
		new Vector3(74f, 50f, 0f),
		new Vector3(74f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(64f, 50f, 0f),
		new Vector3(16f, 50f, 0f),
		new Vector3(30f, 50f, 0f)
	};

	// Token: 0x04000542 RID: 1346
	private int m_iJudgmentPer = -1;

	// Token: 0x04000543 RID: 1347
	private float PerfectAdd = 0.1f;

	// Token: 0x04000544 RID: 1348
	private RESULTDATA m_rData;

	// Token: 0x04000545 RID: 1349
	private int m_iGroup = 1;
}
