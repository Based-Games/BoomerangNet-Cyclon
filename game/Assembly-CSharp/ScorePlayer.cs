using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class ScorePlayer
{
	// Token: 0x06000806 RID: 2054 RVA: 0x0003DB78 File Offset: 0x0003BD78
	public void Init()
	{
		for (int i = 0; i < 64; i++)
		{
			this.m_trackEvt[i] = new SPlayEvtList();
		}
		this.KeyState.ALL_KEYSTATE = new KEYSTATE[12];
		this.KeyState.ALL_KEYMOVE = new Vector3[12];
		this.KeyState.ALL_KEYFIRST = new Vector3[12];
		this.m_totalTick = 0;
		this.m_ms = 0;
		this.m_befChangeTPSTick = 0;
		this.m_state = ScorePlayer.STATE.STATE_STOP;
		this.m_curTick = 0;
		this.m_startTimeMS = 0;
		this.m_curTPS = 0f;
		this.m_Tpm = 0;
		this.m_applyChangeTPSVecIDX = 0;
		this.m_pScore.Init();
		this.InitKeyState();
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0003DC2C File Offset: 0x0003BE2C
	public void ReadyToStart()
	{
		for (int i = 0; i < 64; i++)
		{
			this.m_trackEvt[i].FirstIter();
			this.m_trackEvt[i].SetPlayedEvent(false);
		}
		this.m_curTick = 0;
		this.m_applyChangeTPSVecIDX = 0;
		this.m_curTPS = this.m_pScore.TPS;
		this.m_totalTick = this.m_pScore.TotalTick;
		this.m_ms = this.m_pScore.TotalMS;
		this.m_Tpm = this.m_pScore.TPM;
		this.m_startTimeMS = this.GetTime();
		this.RefreshCurTPS(this.m_curTick, this.m_startTimeMS);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000090BD File Offset: 0x000072BD
	public void Update()
	{
		if (!this.IsPlaying())
		{
			return;
		}
		if (this.UpdateTimer())
		{
			this.UpdateAutoPlayEvent();
		}
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000090D6 File Offset: 0x000072D6
	public void Play(int Playtick)
	{
		if (this.IsPlaying())
		{
			return;
		}
		this.ReadyToStart();
		if (0 < Playtick)
		{
			this.FastForward(Playtick);
		}
		this.m_state = ScorePlayer.STATE.STATE_PLAYING;
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x0003DCD4 File Offset: 0x0003BED4
	public bool LoadXMLData(string strSong)
	{
		if (this.m_pScore == null)
		{
			Debug.Log("score null");
			return false;
		}
		this.m_pScore.Init();
		if (!this.m_pScore.LoadXMLData(strSong))
		{
			return false;
		}
		this.m_curTPS = this.m_pScore.TPS;
		this.m_totalTick = this.m_pScore.TotalTick;
		this.m_ms = this.m_pScore.TotalMS;
		this.MakeOrderedEventList();
		this.CheckMoveNote();
		this.RemoveLineNote();
		if (GameData.RANDEFFECTOR == EFFECTOR_RAND.RANDOM)
		{
			this.SetLineRand();
		}
		else if (GameData.RANDEFFECTOR == EFFECTOR_RAND.ROTATE_STEP)
		{
			this.SetTrackRand();
		}
		else if (GameData.RANDEFFECTOR == EFFECTOR_RAND.MIRROR)
		{
			this.SetMirrorRand();
		}
		return true;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0003DD84 File Offset: 0x0003BF84
	private void SetRand()
	{
		ArrayList[] array = new ArrayList[12];
		for (int i = 0; i < 12; i++)
		{
			array[i] = new ArrayList();
		}
		for (int j = 0; j < 12; j++)
		{
			ArrayList evtVec = this.m_trackEvt[j].evtVec;
			for (int k = 0; k < evtVec.Count; k++)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtVec[k];
				int num = UnityEngine.Random.Range(0, 12);
				scoreEventBase.Track = num;
				array[num].Add(scoreEventBase);
			}
		}
		for (int l = 0; l < 12; l++)
		{
			array[l].Sort(new SortEvtNoteClass());
			this.m_trackEvt[l].evtVec = array[l];
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0003DE40 File Offset: 0x0003C040
	private bool InSpecialNoteTime(ScoreEventBase sNote)
	{
		if (sNote.IsLongNote())
		{
			using (IEnumerator enumerator = this.m_arrSpecialNote.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
					if (scoreEventBase.Tick <= sNote.Tick && scoreEventBase.Tick + scoreEventBase.Duration >= sNote.Tick)
					{
						return true;
					}
					if (scoreEventBase.Tick <= sNote.GetEndTick() && scoreEventBase.Tick + scoreEventBase.Duration >= sNote.GetEndTick())
					{
						return true;
					}
				}
				return false;
			}
		}
		foreach (object obj2 in this.m_arrSpecialNote)
		{
			ScoreEventBase scoreEventBase2 = (ScoreEventBase)obj2;
			if (scoreEventBase2.Tick <= sNote.Tick && scoreEventBase2.Tick + scoreEventBase2.Duration >= sNote.Tick)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0003DF60 File Offset: 0x0003C160
	private void SetLineRand()
	{
		ArrayList arrayList = new ArrayList();
		int[] array = new int[12];
		for (int i = 0; i < 12; i++)
		{
			arrayList.Add(i);
		}
		for (int j = 0; j < 12; j++)
		{
			int num = UnityEngine.Random.Range(0, arrayList.Count);
			array[j] = (int)arrayList[num];
			arrayList.RemoveAt(num);
		}
		ArrayList[] array2 = new ArrayList[12];
		for (int k = 0; k < 12; k++)
		{
			array2[k] = new ArrayList();
		}
		for (int l = 0; l < 12; l++)
		{
			ArrayList evtVec = this.m_trackEvt[l].evtVec;
			for (int m = 0; m < evtVec.Count; m++)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtVec[m];
				int num2 = array[l];
				if (scoreEventBase.IsMoveNote())
				{
					num2 = scoreEventBase.Track;
				}
				else if (this.InSpecialNoteTime(scoreEventBase))
				{
					num2 = scoreEventBase.Track;
				}
				scoreEventBase.SetTrack(num2, false);
				array2[num2].Add(scoreEventBase);
				scoreEventBase.MULTILINE = false;
				scoreEventBase.SameInfo.Clear();
			}
		}
		for (int n = 0; n < 12; n++)
		{
			array2[n].Sort(new SortEvtNoteClass());
			this.m_trackEvt[n].evtVec = array2[n];
		}
		this.CheckSameLine();
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x0003E0CC File Offset: 0x0003C2CC
	private void SetTrackRand()
	{
		ArrayList[] array = new ArrayList[12];
		for (int i = 0; i < 12; i++)
		{
			array[i] = new ArrayList();
		}
		for (int j = 0; j < 12; j++)
		{
			ArrayList evtVec = this.m_trackEvt[j].evtVec;
			for (int k = 0; k < evtVec.Count; k++)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtVec[k];
				int num = scoreEventBase.Tick / (int)this.m_curTPS;
				int num2 = scoreEventBase.Track + num;
				num2 %= 12;
				scoreEventBase.SetTrack(num2, false);
				array[num2].Add(scoreEventBase);
			}
		}
		for (int l = 0; l < 12; l++)
		{
			array[l].Sort(new SortEvtNoteClass());
			this.m_trackEvt[l].evtVec = array[l];
		}
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
	private void SetMirrorRand()
	{
		ArrayList[] array = new ArrayList[12];
		for (int i = 0; i < 12; i++)
		{
			array[i] = new ArrayList();
		}
		int[] array2 = new int[]
		{
			10, 9, 8, 7, 6, 5, 4, 3, 2, 1,
			0, 11
		};
		for (int j = 0; j < 12; j++)
		{
			ArrayList evtVec = this.m_trackEvt[j].evtVec;
			for (int k = 0; k < evtVec.Count; k++)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtVec[k];
				int num = array2[scoreEventBase.Track];
				if (scoreEventBase.IsMoveNote())
				{
					foreach (object obj in scoreEventBase.MoveInfo)
					{
						ScoreEventBase scoreEventBase2 = (ScoreEventBase)obj;
						int num2 = array2[scoreEventBase2.Track];
						if (scoreEventBase2.Attr == 2)
						{
							scoreEventBase2.Attr = 3;
						}
						else if (scoreEventBase2.Attr == 3)
						{
							scoreEventBase2.Attr = 2;
						}
						if (scoreEventBase2.Attr == 6)
						{
							scoreEventBase2.Attr = 7;
						}
						else if (scoreEventBase2.Attr == 7)
						{
							scoreEventBase2.Attr = 6;
						}
						scoreEventBase2.SetTrack(num2, true);
					}
				}
				scoreEventBase.SetTrack(num, true);
				array[num].Add(scoreEventBase);
			}
		}
		for (int l = 0; l < 12; l++)
		{
			array[l].Sort(new SortEvtNoteClass());
			this.m_trackEvt[l].evtVec = array[l];
		}
		this.CheckSameLine();
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0003E340 File Offset: 0x0003C540
	private void RemoveLineNote()
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < 12; i++)
		{
			arrayList.Clear();
			foreach (object obj in this.m_trackEvt[i].evtVec)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
				if (scoreEventBase.IsMoveNote() && scoreEventBase.Attr != 1 && scoreEventBase.Attr != 5)
				{
					arrayList.Add(scoreEventBase);
				}
			}
			foreach (object obj2 in arrayList)
			{
				ScoreEventBase scoreEventBase2 = (ScoreEventBase)obj2;
				this.m_trackEvt[i].evtVec.Remove(scoreEventBase2);
			}
		}
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x0003E434 File Offset: 0x0003C634
	private void IsSameTimeCheck(ScoreEventBase pNote)
	{
		pNote.SameInfo.Clear();
		for (int i = 0; i < 12; i++)
		{
			if (pNote.Track != i)
			{
				int tick = pNote.Tick;
				foreach (object obj in this.m_trackEvt[i].evtVec)
				{
					ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
					if (tick == scoreEventBase.Tick)
					{
						if (!pNote.SameInfo.Contains(scoreEventBase))
						{
							pNote.MULTILINE = true;
							scoreEventBase.MULTILINE = true;
							pNote.SameInfo.Add(scoreEventBase);
						}
						if (!scoreEventBase.SameInfo.Contains(pNote))
						{
							pNote.MULTILINE = true;
							scoreEventBase.MULTILINE = true;
							scoreEventBase.SameInfo.Add(pNote);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0003E51C File Offset: 0x0003C71C
	private bool IsSameTimeSpecialNote(int uTick, int attr)
	{
		foreach (object obj in this.m_arrSpecialNote)
		{
			ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
			if (uTick == scoreEventBase.Tick && attr == scoreEventBase.Attr)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0003E588 File Offset: 0x0003C788
	private void CheckMoveNote()
	{
		int count = this.m_arrSpecialNote.Count;
		for (int i = 0; i < count; i++)
		{
			ScoreEventBase scoreEventBase = (ScoreEventBase)this.m_arrSpecialNote[i];
			int tick = scoreEventBase.Tick;
			if (scoreEventBase.Attr == 1)
			{
				ArrayList moveInfo = scoreEventBase.MoveInfo;
				for (int j = i; j < count; j++)
				{
					ScoreEventBase scoreEventBase2 = (ScoreEventBase)this.m_arrSpecialNote[j];
					if (tick <= scoreEventBase2.Tick)
					{
						if (scoreEventBase2.Attr == 1 && scoreEventBase != scoreEventBase2)
						{
							break;
						}
						if ((scoreEventBase2.Attr == 2 || scoreEventBase2.Attr == 3) && !this.IsSameTimeSpecialNote(scoreEventBase2.Tick, 1) && !moveInfo.Contains(scoreEventBase2))
						{
							moveInfo.Add(scoreEventBase2);
							scoreEventBase.Duration = scoreEventBase2.Tick - scoreEventBase.Tick;
						}
					}
				}
			}
			if (scoreEventBase.Attr == 5)
			{
				ArrayList moveInfo2 = scoreEventBase.MoveInfo;
				for (int k = i; k < count; k++)
				{
					ScoreEventBase scoreEventBase3 = (ScoreEventBase)this.m_arrSpecialNote[k];
					if (tick <= scoreEventBase3.Tick)
					{
						if (scoreEventBase3.Attr == 5 && scoreEventBase != scoreEventBase3)
						{
							break;
						}
						if ((scoreEventBase3.Attr == 6 || scoreEventBase3.Attr == 7) && !this.IsSameTimeSpecialNote(scoreEventBase3.Tick, 5) && !moveInfo2.Contains(scoreEventBase3))
						{
							moveInfo2.Add(scoreEventBase3);
							scoreEventBase.Duration = scoreEventBase3.Tick - scoreEventBase.Tick;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0003E714 File Offset: 0x0003C914
	protected void MakeOrderedEventList()
	{
		this.CopyTrackEvtFromScoreBase(0, 64);
		for (int i = 0; i < 64; i++)
		{
			this.m_trackEvt[i].FirstIter();
		}
		for (int j = 0; j < 12; j++)
		{
			foreach (object obj in this.m_trackEvt[j].evtVec)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
				if (scoreEventBase.IsMoveNote() && !this.m_arrSpecialNote.Contains(scoreEventBase))
				{
					this.m_arrSpecialNote.Add(scoreEventBase);
				}
			}
		}
		this.m_arrSpecialNote.Sort(new SortEvtNoteClass());
		this.CheckSameLine();
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0003E7DC File Offset: 0x0003C9DC
	private void CheckSameLine()
	{
		for (int i = 0; i < 12; i++)
		{
			foreach (object obj in this.m_trackEvt[i].evtVec)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
				if (!scoreEventBase.MULTILINE)
				{
					this.IsSameTimeCheck(scoreEventBase);
					if (scoreEventBase.SameInfo.Count != 0)
					{
						if (scoreEventBase.SameInfo.Count == 1)
						{
							scoreEventBase.DRAW_DOUBLLINE = true;
						}
						else if (2 <= scoreEventBase.SameInfo.Count)
						{
							scoreEventBase.DRAW_MULTILINE = true;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0003E890 File Offset: 0x0003CA90
	private void CopyTrackEvtFromScoreBase(int startTrackIDX, int endTrackIDX)
	{
		for (int i = startTrackIDX; i < endTrackIDX; i++)
		{
			this.m_trackEvt[i].ClearEvent();
			ArrayList evtVec = this.m_pScore.GetEvtVec(i);
			if (evtVec != null)
			{
				for (int j = 0; j < evtVec.Count; j++)
				{
					ScoreEventBase scoreEventBase = (ScoreEventBase)evtVec[j];
					this.m_trackEvt[i].evtVec.Add(scoreEventBase);
				}
			}
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x000090F9 File Offset: 0x000072F9
	private bool LoadKeySndFile(string szKeySndFilename)
	{
		return false;
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00003648 File Offset: 0x00001848
	private void SetMuteSound()
	{
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00003648 File Offset: 0x00001848
	private void SetBGMFile(string szBGMFile)
	{
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x000090FC File Offset: 0x000072FC
	public ArrayList GetSpecialNote()
	{
		return this.m_arrSpecialNote;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00009104 File Offset: 0x00007304
	public ScorePlayer.STATE GetState()
	{
		return this.m_state;
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0000910C File Offset: 0x0000730C
	public bool IsPlaying()
	{
		return this.GetState() == ScorePlayer.STATE.STATE_PLAYING;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00009117 File Offset: 0x00007317
	public bool IsPause()
	{
		return this.GetState() == ScorePlayer.STATE.STATE_PAUSE;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00009122 File Offset: 0x00007322
	public bool IsGameOver()
	{
		return this.GetState() == ScorePlayer.STATE.STATE_GAME_OVER;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0003E8F8 File Offset: 0x0003CAF8
	public void InitKeyState()
	{
		for (int i = 0; i < 12; i++)
		{
			this.KeyState.ALL_KEYSTATE[i] = KEYSTATE.NONE;
		}
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0000912D File Offset: 0x0000732D
	public void SetKeyState(int iLine, KEYSTATE eState)
	{
		this.KeyState.ALL_KEYSTATE[iLine] = eState;
		if (eState == KEYSTATE.NONE)
		{
			this.KeyState.ALL_KEYFIRST[iLine] = Vector3.zero;
			this.KeyState.ALL_KEYMOVE[iLine] = Vector3.zero;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0000916C File Offset: 0x0000736C
	public void SetKeyFirstPos(int iLine, Vector3 vPos)
	{
		this.KeyState.ALL_KEYFIRST[iLine] = vPos;
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00009180 File Offset: 0x00007380
	public KEYSTATE GetKeyState(int iLine)
	{
		return this.KeyState.ALL_KEYSTATE[iLine];
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0000918F File Offset: 0x0000738F
	public void SetKeyPos(int iLine, Vector3 vValue)
	{
		this.KeyState.ALL_KEYMOVE[iLine] = vValue;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x000091A3 File Offset: 0x000073A3
	public Vector3 GetKeyFirstPos(int iLine)
	{
		return this.KeyState.ALL_KEYFIRST[iLine];
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000091B6 File Offset: 0x000073B6
	public Vector3 GetKeyMovePos(int iLine)
	{
		return this.KeyState.ALL_KEYMOVE[iLine];
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00003648 File Offset: 0x00001848
	private void SetTempo(float fTempo)
	{
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x0003E920 File Offset: 0x0003CB20
	private void RefreshCurTPS(int curTick, int curTime)
	{
		ArrayList changeTPSInfoVec = this.m_pScore.GetChangeTPSInfoVec();
		for (int i = changeTPSInfoVec.Count - 1; i >= 0; i--)
		{
			SChangeTPSInfo schangeTPSInfo = (SChangeTPSInfo)changeTPSInfoVec[i];
			if (schangeTPSInfo.tick <= curTick)
			{
				this.m_curTick = curTick;
				this.m_curTPS = schangeTPSInfo.tps;
				this.m_befChangeTPSTick = schangeTPSInfo.tick;
				this.m_startTimeMS = curTime - this.TickToMS(this.GetCurTick() - this.m_befChangeTPSTick);
				this.m_applyChangeTPSVecIDX = i + 1;
				return;
			}
		}
		this.m_curTick = curTick;
		this.m_curTPS = this.m_pScore.TPS;
		this.m_befChangeTPSTick = 0;
		this.m_startTimeMS = curTime - this.TickToMS(this.GetCurTick() - this.m_befChangeTPSTick);
		this.m_applyChangeTPSVecIDX = 0;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x000091C9 File Offset: 0x000073C9
	private int GetTime()
	{
		return (int)(Time.realtimeSinceStartup * 1000f);
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000091D7 File Offset: 0x000073D7
	public int GetCurTick()
	{
		return this.m_curTick;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x000091DF File Offset: 0x000073DF
	public int GetElapsedTime()
	{
		return TypeDef.GETTIMEINTERVAL(this.GetTime(), this.m_startTimeMS);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
	private void CalcCurTick()
	{
		int time = this.GetTime();
		int num = (int)((double)TypeDef.GETTIMEINTERVAL(time, this.m_startTimeMS) * (double)this.m_curTPS) / 1000 + this.m_befChangeTPSTick;
		ArrayList changeTPSInfoVec = this.m_pScore.GetChangeTPSInfoVec();
		if (changeTPSInfoVec.Count > this.m_applyChangeTPSVecIDX && ((SChangeTPSInfo)changeTPSInfoVec[this.m_applyChangeTPSVecIDX]).tick < num)
		{
			this.RefreshCurTPS(num, time);
			return;
		}
		this.m_curTick = num;
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0003EA64 File Offset: 0x0003CC64
	public bool UpdateTimer()
	{
		int curTick = this.m_curTick;
		this.CalcCurTick();
		if (this.m_curTick != curTick)
		{
			if (this.m_curTick >= this.m_totalTick)
			{
				this.m_curTick = this.m_totalTick;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000091F2 File Offset: 0x000073F2
	private void FastForward(int playTick)
	{
		if (this.m_curTick >= playTick)
		{
			return;
		}
		this.RefreshCurTPS(playTick, this.m_curTick);
		this.UpdateTimer();
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00009212 File Offset: 0x00007412
	public int TickToMS(int tick)
	{
		return (int)((float)(tick * 1000) / this.m_curTPS);
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00009224 File Offset: 0x00007424
	public int MSToTick(int ms)
	{
		return (int)(this.m_curTPS * (float)ms / 1000f);
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0003EAA4 File Offset: 0x0003CCA4
	private void UpdateBgmEvent()
	{
		SPlayEvtList splayEvtList = this.m_trackEvt[31];
		ScoreEventBase curEvt = splayEvtList.GetCurEvt();
		if (curEvt == null)
		{
			return;
		}
		int num = this.GetCurTick() + (int)(this.m_curTPS / 32f * GameData.TEST_PLAYTIME);
		if (curEvt.Tick > num)
		{
			return;
		}
		if (curEvt.Attr == 0)
		{
			this.PlayEvent(curEvt, 31, num - curEvt.Tick);
			splayEvtList.SetPlayedEvent(true);
			splayEvtList.NextIter();
			return;
		}
		if (curEvt.Attr == 100)
		{
			if (this.m_dComplete != null)
			{
				this.m_dComplete();
				return;
			}
		}
		else if (curEvt.Attr == 99 && !this.m_bFadeStart)
		{
			if (this.CallFade != null)
			{
				this.CallFade();
			}
			this.m_bFadeStart = true;
			if (this.m_dComplete != null)
			{
				this.m_dComplete();
			}
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0003EB70 File Offset: 0x0003CD70
	private void UpdateAutoPlayEvent()
	{
		for (int i = 0; i < 12; i++)
		{
			SPlayEvtList splayEvtList = this.m_trackEvt[i];
			splayEvtList.CollectViewNote();
			ArrayList evtView = splayEvtList.evtView;
			for (int j = 0; j < evtView.Count; j++)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtView[j];
				if (scoreEventBase != null && scoreEventBase.EndView)
				{
					scoreEventBase.Clear();
				}
			}
			ScoreEventBase curEvt = splayEvtList.GetCurEvt();
			if (curEvt != null)
			{
				if (curEvt.IsMoveNote() && curEvt.Attr != 1 && curEvt.Attr != 5 && this.GetCurTick() > curEvt.Tick)
				{
					splayEvtList.NextIter();
				}
				if (curEvt.EndView)
				{
					splayEvtList.NextIter();
				}
			}
		}
		this.UpdateBgmEvent();
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0003EC2C File Offset: 0x0003CE2C
	private void PlayEvent(ScoreEventBase pEvt, int trackIDX, int elapsedTick)
	{
		if (trackIDX == 31)
		{
			float num = (float)this.TickToMS(elapsedTick);
			num *= 0.001f;
			if (this.CallBackStart != null)
			{
				this.CallBackStart(num);
			}
			if (Singleton<GameManager>.instance.inAttract() && !Singleton<GameManager>.instance.DemoSound)
			{
				return;
			}
			Singleton<SoundSourceManager>.instance.SetBgmTime(num);
			Singleton<SoundSourceManager>.instance.PlayBgm();
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00009236 File Offset: 0x00007436
	public int GetJudgmentRangTime(bool bIn)
	{
		if (bIn)
		{
			return GameData.InJudgment_Time;
		}
		return GameData.OutJudgment_Time;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0003EC94 File Offset: 0x0003CE94
	public bool IsInJudgmentRangeTick(int iCurTick, int iEvtTick)
	{
		bool flag = true;
		if (iCurTick > iEvtTick)
		{
			flag = false;
		}
		return iCurTick + this.GetJudgmentRangTime(flag) >= iEvtTick;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0003ECB8 File Offset: 0x0003CEB8
	private int GetJogMaximumTick(int iCurTick, bool bIn)
	{
		int num = this.GetJudgmentRangTime(bIn) * 2;
		if (iCurTick <= num)
		{
			return 0;
		}
		return iCurTick - num;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0003ECD8 File Offset: 0x0003CED8
	private int GetMaximumTick(int iCurTick, bool bIn)
	{
		int judgmentRangTime = this.GetJudgmentRangTime(bIn);
		if (iCurTick <= judgmentRangTime)
		{
			return 0;
		}
		return iCurTick - judgmentRangTime;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0003ECF8 File Offset: 0x0003CEF8
	public bool IsInMaximumTick(int uCurTick, int uEvtTick)
	{
		bool flag = uEvtTick > uCurTick;
		return this.GetMaximumTick(uCurTick, flag) <= uEvtTick;
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0003ED18 File Offset: 0x0003CF18
	public bool IsInJogMaximumTick(int uCurTick, int uEvtTick)
	{
		bool flag = uEvtTick > uCurTick;
		return this.GetJogMaximumTick(uCurTick, flag) <= uEvtTick;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00009246 File Offset: 0x00007446
	private float GetTempLongGuide(int dwCurTick, int dwEvtTick)
	{
		return (float)(dwEvtTick - dwCurTick) / (this.TPS * 2f);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0003ED38 File Offset: 0x0003CF38
	public uint GetJudgmentPer(int dwCurTick, int dwEvtTick)
	{
		bool flag = true;
		if (dwCurTick > dwEvtTick)
		{
			flag = false;
		}
		float num = (float)((dwCurTick <= dwEvtTick) ? (dwEvtTick - dwCurTick) : (dwCurTick - dwEvtTick));
		int num2;
		if (flag)
		{
			num2 = GameData.InJudgment_Time;
		}
		else
		{
			num2 = GameData.OutJudgment_Time;
		}
		float num3 = num / (float)num2;
		num3 = (1f - num3) * 100f;
		return (uint)num3;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0003ED80 File Offset: 0x0003CF80
	public JUDGMENT_TYPE GetJudgmentType(uint uPer, bool bIn)
	{
		if (bIn)
		{
			if ((ulong)uPer >= (ulong)((long)GameData.InJudgment_Per[4]))
			{
				return JUDGMENT_TYPE.PERFECT;
			}
			if ((ulong)uPer >= (ulong)((long)GameData.InJudgment_Per[3]))
			{
				return JUDGMENT_TYPE.GREAT;
			}
			if ((ulong)uPer >= (ulong)((long)GameData.InJudgment_Per[2]))
			{
				return JUDGMENT_TYPE.GOOD;
			}
			if ((ulong)uPer >= (ulong)((long)GameData.InJudgment_Per[1]))
			{
				return JUDGMENT_TYPE.POOR;
			}
		}
		else
		{
			if ((ulong)uPer >= (ulong)((long)GameData.OutJudgment_Per[4]))
			{
				return JUDGMENT_TYPE.PERFECT;
			}
			if ((ulong)uPer >= (ulong)((long)GameData.OutJudgment_Per[3]))
			{
				return JUDGMENT_TYPE.GREAT;
			}
			if ((ulong)uPer >= (ulong)((long)GameData.OutJudgment_Per[2]))
			{
				return JUDGMENT_TYPE.GOOD;
			}
			if ((ulong)uPer >= (ulong)((long)GameData.OutJudgment_Per[1]))
			{
				return JUDGMENT_TYPE.POOR;
			}
		}
		return JUDGMENT_TYPE.BREAK;
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x00009259 File Offset: 0x00007459
	public SPlayEvtList[] AllTrackEvt
	{
		get
		{
			return this.m_trackEvt;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x0600083D RID: 2109 RVA: 0x00009261 File Offset: 0x00007461
	public float TPS
	{
		get
		{
			return this.m_curTPS;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x0600083E RID: 2110 RVA: 0x00009269 File Offset: 0x00007469
	public int TPM
	{
		get
		{
			return this.m_Tpm;
		}
	}

	// Token: 0x0400068E RID: 1678
	public const int INS_MAX_NUM = 256;

	// Token: 0x0400068F RID: 1679
	public const int INVALID_TICK = -1;

	// Token: 0x04000690 RID: 1680
	public const int TAIL_TICK = 50;

	// Token: 0x04000691 RID: 1681
	public const int BBG_DISTANCE = 50;

	// Token: 0x04000692 RID: 1682
	public const int CHECK_UPDATE_RATIO_TIME = 5;

	// Token: 0x04000693 RID: 1683
	public const int CHECK_UPDATE_BGM_TIME = 10;

	// Token: 0x04000694 RID: 1684
	public const int RENDER_NODE_RESERVED_CNT = 200;

	// Token: 0x04000695 RID: 1685
	private ScorePlayer.STATE m_state;

	// Token: 0x04000696 RID: 1686
	private int m_curTick;

	// Token: 0x04000697 RID: 1687
	private int m_startTimeMS;

	// Token: 0x04000698 RID: 1688
	private int m_totalTick;

	// Token: 0x04000699 RID: 1689
	private float m_curTPS;

	// Token: 0x0400069A RID: 1690
	private int m_Tpm;

	// Token: 0x0400069B RID: 1691
	private int m_befChangeTPSTick;

	// Token: 0x0400069C RID: 1692
	private int m_applyChangeTPSVecIDX;

	// Token: 0x0400069D RID: 1693
	private int m_befChangeTPSTime;

	// Token: 0x0400069E RID: 1694
	private ScoreBase m_pScore = new ScoreBase();

	// Token: 0x0400069F RID: 1695
	private SPlayEvtList[] m_trackEvt = new SPlayEvtList[64];

	// Token: 0x040006A0 RID: 1696
	private ArrayList m_arrSpecialNote = new ArrayList();

	// Token: 0x040006A1 RID: 1697
	private bool m_isGameEnd;

	// Token: 0x040006A2 RID: 1698
	private int m_befTick;

	// Token: 0x040006A3 RID: 1699
	public SKeyProcess KeyState = new SKeyProcess();

	// Token: 0x040006A4 RID: 1700
	public ScorePlayer.CompletSong m_dComplete;

	// Token: 0x040006A5 RID: 1701
	public bool m_bFadeStart;

	// Token: 0x040006A6 RID: 1702
	public ScorePlayer.CallBackStartMusic CallBackStart;

	// Token: 0x040006A7 RID: 1703
	public ScorePlayer.CallBackFade CallFade;

	// Token: 0x040006A8 RID: 1704
	public int m_ms;

	// Token: 0x020000EF RID: 239
	public enum STATE
	{
		// Token: 0x040006AA RID: 1706
		STATE_STOP,
		// Token: 0x040006AB RID: 1707
		STATE_PLAYING,
		// Token: 0x040006AC RID: 1708
		STATE_PAUSE,
		// Token: 0x040006AD RID: 1709
		STATE_GAME_OVER,
		// Token: 0x040006AE RID: 1710
		STATE_MAX_NUM
	}

	// Token: 0x020000F0 RID: 240
	// (Invoke) Token: 0x06000840 RID: 2112
	public delegate void CompletSong();

	// Token: 0x020000F1 RID: 241
	// (Invoke) Token: 0x06000844 RID: 2116
	public delegate void CallBackStartMusic(float fTime);

	// Token: 0x020000F2 RID: 242
	// (Invoke) Token: 0x06000848 RID: 2120
	public delegate void CallBackFade();
}
