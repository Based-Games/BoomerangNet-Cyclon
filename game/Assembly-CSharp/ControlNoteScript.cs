using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class ControlNoteScript : MonoBehaviour
{
	// Token: 0x060006F0 RID: 1776 RVA: 0x00003648 File Offset: 0x00001848
	private void Awake()
	{
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00008798 File Offset: 0x00006998
	private void Start()
	{
		this.NotePool = PoolManager.Pools["NotePool"];
		this.BEAT_VALUE = this.BEAT_ON - this.BEAT_0FF;
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00036024 File Offset: 0x00034224
	private void SetFeverTps(bool bStart)
	{
		Hashtable hashtable = new Hashtable();
		float num = 1f;
		float num2 = GameData.FEVER_CHANGE_TIME[GameData.INGAMGE_SPEED];
		FEVER_STATE eState = ControlFeverScript.m_eState;
		switch (eState + 1)
		{
		case FEVER_STATE.FEVERx4:
			num2 *= 2f;
			break;
		case FEVER_STATE.FEVERx5:
			num2 *= 3f;
			break;
		case FEVER_STATE.MAX:
			num2 *= 4f;
			break;
		}
		num2 = this.m_ScorePlayer.TPS * num2;
		if (bStart)
		{
			hashtable["from"] = this.m_fViewTps;
			hashtable["to"] = this.m_fViewTps - num2;
			hashtable["time"] = num;
			hashtable["easetype"] = iTween.EaseType.linear;
			hashtable["onupdatetarget"] = base.gameObject;
			hashtable["onupdate"] = "OnUpdateTps";
		}
		else
		{
			hashtable["from"] = this.m_fViewTps - num2;
			hashtable["to"] = this.m_fViewTps;
			hashtable["time"] = num;
			hashtable["easetype"] = iTween.EaseType.linear;
			hashtable["onupdatetarget"] = base.gameObject;
			hashtable["onupdate"] = "OnUpdateTps";
		}
		iTween.ValueTo(base.gameObject, hashtable);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x000087C6 File Offset: 0x000069C6
	private void OnUpdateTps(float fValue)
	{
		this.m_fTargetTps = fValue;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x000087CF File Offset: 0x000069CF
	private float GetViewTps(int iTrack)
	{
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.MAX_SPEED)
		{
			return this.m_ScorePlayer.TPS * GameData.CHAOS_X_RANDOM[iTrack];
		}
		return this.m_fTargetTps;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x000087F7 File Offset: 0x000069F7
	private void SetScorePlayer(ScorePlayer sPlay)
	{
		this.m_ScorePlayer = sPlay;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x000361AC File Offset: 0x000343AC
	private void SetTps()
	{
		this.m_fViewTps = this.m_ScorePlayer.TPS * GameData.ARR_TIME[GameData.INGAMGE_SPEED];
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_UP)
		{
			this.m_fViewTps = this.m_ScorePlayer.TPS * GameData.CHAOS_UP_SPEED[GameData.INGAMGE_SPEED];
		}
		else if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_DN)
		{
			this.m_fViewTps = this.m_ScorePlayer.TPS * GameData.CHAOS_DN_SPEED[GameData.INGAMGE_SPEED];
		}
		this.m_fTargetTps = this.m_fViewTps;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0003623C File Offset: 0x0003443C
	private void SetChangeTps()
	{
		if (GameData.ON_FEVER)
		{
			return;
		}
		this.m_fViewTps = this.m_ScorePlayer.TPS * GameData.ARR_TIME[GameData.INGAMGE_SPEED];
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_UP)
		{
			this.m_fViewTps = this.m_ScorePlayer.TPS * GameData.CHAOS_UP_SPEED[GameData.INGAMGE_SPEED];
		}
		else if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_DN)
		{
			this.m_fViewTps = this.m_ScorePlayer.TPS * GameData.CHAOS_DN_SPEED[GameData.INGAMGE_SPEED];
		}
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = this.m_fTargetTps;
		hashtable["to"] = this.m_fViewTps;
		hashtable["time"] = 1f;
		hashtable["easetype"] = iTween.EaseType.easeOutCubic;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "UpdateTps";
		iTween.ValueTo(base.gameObject, hashtable);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000087C6 File Offset: 0x000069C6
	private void UpdateTps(float fTps)
	{
		this.m_fTargetTps = fTps;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00036350 File Offset: 0x00034550
	private void CompleteTps()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = this.m_fTargetTps;
		hashtable["to"] = this.m_fViewTps;
		hashtable["time"] = 0.3f;
		hashtable["easetype"] = iTween.EaseType.easeOutCirc;
		hashtable["onupdate"] = "UpdateTps";
		hashtable["onupdatetarget"] = base.gameObject;
		iTween.ValueTo(base.gameObject, hashtable);
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x000363E4 File Offset: 0x000345E4
	private void SetData()
	{
		this.m_trackEvt = this.m_ScorePlayer.AllTrackEvt;
		this.SetTps();
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_W)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["from"] = GameData.CHAOS_W_MIN;
			hashtable["to"] = GameData.CHAOS_W_MAX;
			hashtable["time"] = GameData.CHAOS_W_TIME;
			hashtable["looptype"] = iTween.LoopType.pingPong;
			hashtable["onupdate"] = "UpdateChaos";
			hashtable["onupdatetarget"] = base.gameObject;
			iTween.ValueTo(base.gameObject, hashtable);
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00008800 File Offset: 0x00006A00
	private void UpdateChaos(float fValue)
	{
		this.m_fTargetTps = this.m_ScorePlayer.TPS * fValue;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00036498 File Offset: 0x00034698
	private void Setobject()
	{
		this.LineCenter = base.transform.FindChild("LineCenter").gameObject;
		this.LineTail = this.LineCenter.transform.FindChild("LineTail").gameObject;
		this.LineGuide = this.LineCenter.transform.FindChild("LineGuide").gameObject;
		this.LineCenter.transform.localPosition = Vector3.zero;
		this.LineCenter.transform.localEulerAngles = Vector3.zero;
		this.LineTail.transform.localEulerAngles = Vector3.zero;
		this.LineTail.transform.localPosition = this.TAIL_LINE;
		this.LineGuide.transform.localEulerAngles = Vector3.zero;
		this.LineGuide.transform.localPosition = this.NOTE_LINE;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00036580 File Offset: 0x00034780
	public void RenderNote()
	{
		if (this.m_ScorePlayer == null)
		{
			return;
		}
		this.m_iCurTick = this.m_ScorePlayer.GetCurTick();
		for (int i = 0; i < 12; i++)
		{
			SPlayEvtList splayEvtList = this.m_trackEvt[i];
			ArrayList evtView = splayEvtList.evtView;
			for (int j = 0; j < evtView.Count; j++)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)evtView[j];
				if (scoreEventBase != null)
				{
					if (scoreEventBase.IsMoveNote())
					{
						this.RenderMoveNote(scoreEventBase, i);
					}
					else if (scoreEventBase.IsLongNote())
					{
						this.RenderLongNote(scoreEventBase, i);
					}
					else if (scoreEventBase.IsDirNote())
					{
						this.RenderDirlNote(scoreEventBase, i);
					}
					else
					{
						this.RenderNormalNote(scoreEventBase, i);
					}
				}
			}
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00036658 File Offset: 0x00034858
	private void RenderNormalNote(ScoreEventBase pEvt, int iTrack)
	{
		float viewTps = this.GetViewTps(iTrack);
		Vector3 linePos = this.GetLinePos(iTrack);
		if (pEvt.EndView)
		{
			return;
		}
		if ((float)this.m_iCurTick > (float)pEvt.Tick + viewTps / 2f)
		{
			pEvt.EndView = true;
			return;
		}
		if (pEvt.CanRender)
		{
			int num = pEvt.Tick - this.m_iCurTick;
			if (viewTps < (float)num)
			{
				return;
			}
		}
		if (pEvt.CanRender)
		{
			pEvt.CanRender = false;
			GameObject gameObject = this.NotePool.Spawn(this.NOTE.transform).gameObject;
			pEvt.objControlNote = gameObject;
			pEvt.objControlNote.SendMessage("SetEvent", pEvt);
			pEvt.objControlNote.SendMessage("SetControlNote", this);
			pEvt.objControlNote.SendMessage("InitPos", iTrack);
		}
		NormalNoteScript component = pEvt.objControlNote.GetComponent<NormalNoteScript>();
		component.SetPosition(this.m_iCurTick, viewTps, linePos);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00036750 File Offset: 0x00034950
	private void RenderDirlNote(ScoreEventBase pEvt, int iTrack)
	{
		float viewTps = this.GetViewTps(iTrack);
		Vector3 linePos = this.GetLinePos(iTrack);
		if (pEvt.EndView)
		{
			return;
		}
		if ((float)this.m_iCurTick > (float)pEvt.Tick + viewTps / 2f)
		{
			pEvt.EndView = true;
			return;
		}
		if (pEvt.CanRender)
		{
			int num = pEvt.Tick - this.m_iCurTick;
			if (viewTps < (float)num)
			{
				return;
			}
		}
		if (pEvt.CanRender)
		{
			pEvt.CanRender = false;
			GameObject gameObject = this.NotePool.Spawn(this.DIRNOTE.transform).gameObject;
			gameObject.name = "Dir" + pEvt.Track.ToString();
			pEvt.objControlNote = gameObject;
			pEvt.objControlNote.SendMessage("SetEvent", pEvt);
			pEvt.objControlNote.SendMessage("SetControlNote", this);
			pEvt.objControlNote.SendMessage("InitPos", iTrack);
		}
		DirNoteScript component = pEvt.objControlNote.GetComponent<DirNoteScript>();
		component.SetPosition(this.m_iCurTick, viewTps, this.m_ScorePlayer.TPS * 2f, linePos);
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0003687C File Offset: 0x00034A7C
	private void RenderLongNote(ScoreEventBase pEvt, int iTrack)
	{
		float viewTps = this.GetViewTps(iTrack);
		Vector3 linePos = this.GetLinePos(iTrack);
		if (pEvt.EndView)
		{
			return;
		}
		if ((pEvt.JudgmentEnd == JUDGMENT_TYPE.JUDGMENT_NONE || pEvt.JudgmentEnd == JUDGMENT_TYPE.BREAK) && (float)this.m_iCurTick > (float)pEvt.GetEndTick() + viewTps / 2f)
		{
			pEvt.EndView = true;
			return;
		}
		if (pEvt.CanRender)
		{
			int num = pEvt.Tick - this.m_iCurTick;
			if (viewTps < (float)num)
			{
				return;
			}
		}
		if (pEvt.CanRender)
		{
			pEvt.CanRender = false;
			GameObject gameObject = this.NotePool.Spawn(this.LONGNOTE.transform).gameObject;
			pEvt.objControlNote = gameObject;
			pEvt.objControlNote.SendMessage("SetEvent", pEvt);
			pEvt.objControlNote.SendMessage("SetControlNote", this);
			pEvt.objControlNote.SendMessage("InitPos", iTrack);
		}
		LongNoteScript component = pEvt.objControlNote.GetComponent<LongNoteScript>();
		component.SetPosition(this.m_iCurTick, viewTps, linePos);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00036990 File Offset: 0x00034B90
	private void RenderMoveNote(ScoreEventBase pEvt, int iTrack)
	{
		if (pEvt.EndView)
		{
			return;
		}
		float viewTps = this.GetViewTps(iTrack);
		int num = this.m_ScorePlayer.GetJudgmentRangTime(false) / 4;
		if (this.m_iCurTick > pEvt.GetEndTick() + num)
		{
			pEvt.EndView = true;
			return;
		}
		if (pEvt.CanRender)
		{
			int num2 = pEvt.Tick - this.m_iCurTick;
			if (viewTps < (float)num2)
			{
				return;
			}
			pEvt.CanRender = false;
			GameObject gameObject = this.NotePool.Spawn(this.LONGMOVENOTE.transform).gameObject;
			pEvt.objControlNote = gameObject;
			gameObject.name = "Move" + pEvt.Track.ToString();
			pEvt.objControlNote.SendMessage("SetScorePlayer", this.m_ScorePlayer);
			pEvt.objControlNote.SendMessage("SetEvent", pEvt);
			pEvt.objControlNote.SendMessage("SetControlNote", this);
			pEvt.objControlNote.SendMessage("InitPos", iTrack);
		}
		MoveNoteScript component = pEvt.objControlNote.GetComponent<MoveNoteScript>();
		component.SetPosition(this.m_iCurTick, viewTps, Vector3.zero);
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00036AB4 File Offset: 0x00034CB4
	public Vector3 GetLinePos(int iLine)
	{
		Vector3 localEulerAngles = this.LineCenter.transform.localEulerAngles;
		this.LineCenter.transform.localEulerAngles = GameData.MAXGUIDE[iLine];
		Vector3 position = this.LineGuide.transform.position;
		this.LineCenter.transform.localEulerAngles = localEulerAngles;
		return position;
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00036B18 File Offset: 0x00034D18
	public Vector3 GetRotPos(float fRot)
	{
		Vector3 localEulerAngles = this.LineCenter.transform.localEulerAngles;
		Vector3 localEulerAngles2 = this.LineCenter.transform.localEulerAngles;
		localEulerAngles2.z = fRot;
		this.LineCenter.transform.localEulerAngles = localEulerAngles2;
		Vector3 position = this.LineGuide.transform.position;
		this.LineCenter.transform.localEulerAngles = localEulerAngles;
		return position;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00008815 File Offset: 0x00006A15
	public Vector3 GetPos(Vector3 vPos)
	{
		this.LineCenter.transform.localPosition = vPos;
		return this.LineGuide.transform.position;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00036B84 File Offset: 0x00034D84
	public Vector3 GetFrontLinePos(int iLine)
	{
		this.LineCenter.transform.localEulerAngles = GameData.MAXGUIDE[iLine];
		this.LineTail.transform.localPosition = this.TEMP_TAIL_LINE;
		Vector3 position = this.LineTail.transform.position;
		this.LineTail.transform.localPosition = this.TAIL_LINE;
		return position;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00036BF0 File Offset: 0x00034DF0
	public Vector3 GetNotePos(int iTick, int iTrack, float fViewTps)
	{
		int curTick = this.m_ScorePlayer.GetCurTick();
		Vector3 linePos = this.GetLinePos(iTrack);
		int num = iTick - curTick;
		float num2 = (float)num / fViewTps;
		Vector3 zero = Vector3.zero;
		float logTime = GameData.GetLogTime(num2);
		zero.x = (1f - logTime) * linePos.x;
		zero.y = (1f - logTime) * linePos.y;
		zero.z = logTime * 80f;
		return zero;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00036C68 File Offset: 0x00034E68
	public Color DepthNoteColor(float fValue, Color cBase)
	{
		float num = 0.5f + (1f - fValue) * 1f - 0.5f;
		return cBase * num;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00036C98 File Offset: 0x00034E98
	public void RotNote(GameObject oNote, Vector3 vStart, Vector3 vPos)
	{
		float angle = Singleton<GameManager>.instance.GetAngle(new Vector2(vStart.x, vStart.y), new Vector2(vPos.x, vPos.y));
		Vector3 eulerAngles = oNote.transform.eulerAngles;
		eulerAngles.z = 360f - angle;
		oNote.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00036CFC File Offset: 0x00034EFC
	public float GetEffectAlpha(int iTick, float fAlpha, int iTrack)
	{
		float num = fAlpha;
		float viewTps = this.GetViewTps(iTrack);
		int curTick = this.m_ScorePlayer.GetCurTick();
		switch (GameData.FADEEFFCTOR)
		{
		case EFFECTOR_FADER.FADEIN:
		{
			float num2 = (float)(iTick - curTick);
			float num3 = viewTps * GameData.FADEIN_START;
			float num4 = viewTps * GameData.FADEIN_END;
			float num5 = num3 - num4;
			if ((float)iTick - num3 >= (float)curTick)
			{
				num = 0f;
			}
			else if (num4 <= num2 && num2 < num3)
			{
				num = 1f - (num2 - num4) / num5;
			}
			break;
		}
		case EFFECTOR_FADER.FADEOUT:
		{
			float num6 = (float)(iTick - curTick);
			float num7 = viewTps * GameData.FADEOUT_START;
			float num8 = viewTps * GameData.FADEOUT_END;
			float num9 = num7 - num8;
			if (num8 <= num6 && num6 < num7)
			{
				num = (num6 - num8) / num9;
			}
			else if ((float)curTick > (float)iTick - num8)
			{
				num = 0f;
			}
			else
			{
				num = 1f;
			}
			break;
		}
		case EFFECTOR_FADER.BLINK:
		{
			float num10 = (float)this.m_ScorePlayer.TPM / GameData.BLINK_VALUE;
			int num11 = (int)((float)curTick / num10);
			float num12 = (float)curTick % num10;
			float num13 = num10 * 0.5f;
			int num14 = num11 % 4;
			if (num14 == 1 || num14 == 3)
			{
				if (num13 > num12)
				{
					num = num12 / num13;
				}
				else
				{
					num = 2f - num12 / num13;
				}
			}
			else
			{
				num = 0f;
			}
			if (num > fAlpha)
			{
				num = fAlpha;
			}
			break;
		}
		case EFFECTOR_FADER.BLANK:
			num = 0f;
			break;
		}
		return num;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00036E88 File Offset: 0x00035088
	private void UpdateBeatMark()
	{
		if (this.m_ScorePlayer == null)
		{
			return;
		}
		int curTick = this.m_ScorePlayer.GetCurTick();
		float num = (float)curTick;
		float num2 = (float)(this.m_ScorePlayer.TPM / 4);
		int num3 = (int)(num / num2);
		float num4 = num % num2;
		if (this.m_iBeat != num3)
		{
			this.m_iBeat = num3;
			this.BeatScale = this.BEAT_0FF;
		}
		else if (num2 / 2f > num4)
		{
			float num5 = num4 / (num2 / 2f);
			this.BeatScale = this.BEAT_ON - this.BEAT_VALUE * num5;
		}
		else
		{
			this.BeatScale = this.BEAT_0FF;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00036F38 File Offset: 0x00035138
	private void UpdateSpeedChange()
	{
		if (this.m_bCheckChangeSpeed)
		{
			float num = this.m_fMiddleTps - this.m_fTargetTps;
			num = Mathf.Abs(num);
			this.m_fTargetTps = (this.m_fMiddleTps + this.m_fTargetTps) / 2f;
			if (0.01f > num)
			{
				this.m_fTargetTps = this.m_fMiddleTps;
				this.m_bCheckChangeSpeed = false;
			}
			return;
		}
		if (this.m_bChangeSpeed)
		{
			float num2 = this.m_fViewTps - this.m_fTargetTps;
			float num3 = Mathf.Abs(num2);
			this.m_fTargetTps += num2 * 0.3f;
			if (0.01f > num3)
			{
				this.m_fTargetTps = this.m_fViewTps;
				this.m_bChangeSpeed = false;
			}
		}
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00008838 File Offset: 0x00006A38
	private void Update()
	{
		this.UpdateBeatMark();
		this.UpdateSpeedChange();
	}

	// Token: 0x04000594 RID: 1428
	private ScorePlayer m_ScorePlayer;

	// Token: 0x04000595 RID: 1429
	private SPlayEvtList[] m_trackEvt;

	// Token: 0x04000596 RID: 1430
	[HideInInspector]
	public GameObject LineCenter;

	// Token: 0x04000597 RID: 1431
	[HideInInspector]
	public GameObject LineTail;

	// Token: 0x04000598 RID: 1432
	[HideInInspector]
	public GameObject LineGuide;

	// Token: 0x04000599 RID: 1433
	private Vector3 NOTE_LINE = new Vector3(0f, -4.7f, 0f);

	// Token: 0x0400059A RID: 1434
	private Vector3 TAIL_LINE = new Vector3(0f, -4.03f, 0f);

	// Token: 0x0400059B RID: 1435
	private Vector3 TEMP_TAIL_LINE = new Vector3(0f, -4.02f, 0f);

	// Token: 0x0400059C RID: 1436
	[HideInInspector]
	public Vector3 BEAT_ON = new Vector3(1.5f, 1.5f, 1f);

	// Token: 0x0400059D RID: 1437
	[HideInInspector]
	public Vector3 BEAT_0FF = new Vector3(1f, 1f, 1f);

	// Token: 0x0400059E RID: 1438
	[HideInInspector]
	public Vector3 BEAT_VALUE = Vector3.zero;

	// Token: 0x0400059F RID: 1439
	[HideInInspector]
	public SpawnPool NotePool;

	// Token: 0x040005A0 RID: 1440
	public GameObject NOTE;

	// Token: 0x040005A1 RID: 1441
	public GameObject LONGNOTE;

	// Token: 0x040005A2 RID: 1442
	public GameObject DIRNOTE;

	// Token: 0x040005A3 RID: 1443
	public GameObject LONGMOVENOTE;

	// Token: 0x040005A4 RID: 1444
	public Material NOTEMAT;

	// Token: 0x040005A5 RID: 1445
	private float m_fViewTps;

	// Token: 0x040005A6 RID: 1446
	private float m_fTargetTps;

	// Token: 0x040005A7 RID: 1447
	private int m_iCurTick;

	// Token: 0x040005A8 RID: 1448
	private int m_iBeat;

	// Token: 0x040005A9 RID: 1449
	[HideInInspector]
	public Vector3 BeatScale = Vector3.zero;

	// Token: 0x040005AA RID: 1450
	private float m_fMiddleTps;

	// Token: 0x040005AB RID: 1451
	private bool m_bChangeSpeed;

	// Token: 0x040005AC RID: 1452
	private bool m_bCheckChangeSpeed;
}
