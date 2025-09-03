using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class MoveNoteScript : MonoBehaviour
{
	// Token: 0x060007DD RID: 2013 RVA: 0x0003B9B4 File Offset: 0x00039BB4
	private void Awake()
	{
		this.m_oNote = base.transform.FindChild("Note").gameObject;
		this.m_oNoteGroove = base.transform.FindChild("NoteGroove").gameObject;
		this.m_oUp = this.m_oNote.transform.FindChild("Up").gameObject;
		this.m_oNoteEnd = base.transform.FindChild("DirEnd").gameObject;
		this.m_sLine = base.transform.FindChild("Line").GetComponent<LineRenderer>();
		this.m_sTail = base.transform.FindChild("Tail").GetComponent<LineRenderer>();
		this.m_sprNote = this.m_oNote.GetComponent<SpriteRenderer>();
		this.m_sprNoteGroove = this.m_oNoteGroove.GetComponent<SpriteRenderer>();
		this.m_sprUp = this.m_oUp.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00008F19 File Offset: 0x00007119
	private void SetFail()
	{
		this.m_sprNote.sprite = this.OffSprite;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00008F2C File Offset: 0x0000712C
	private void SetControlNote(ControlNoteScript sControlNote)
	{
		this.m_sControlNote = sControlNote;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00008F35 File Offset: 0x00007135
	private void SetScorePlayer(ScorePlayer sPlayer)
	{
		this.m_ScorePlayer = sPlayer;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00008F3E File Offset: 0x0000713E
	private void SetEvent(ScoreEventBase pEvt)
	{
		this.m_sEvt = pEvt;
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0003BA9C File Offset: 0x00039C9C
	public void ClearLine()
	{
		foreach (object obj in this.m_arrLine)
		{
			Transform transform = (Transform)obj;
			if (this.m_sControlNote.NotePool.IsSpawned(transform))
			{
				this.m_sControlNote.NotePool.Despawn(transform);
			}
		}
		this.m_arrLine.Clear();
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0003BB2C File Offset: 0x00039D2C
	public void ClearRound()
	{
		foreach (object obj in this.m_arrRoundLine)
		{
			Transform transform = (Transform)obj;
			if (this.m_sControlNote.NotePool.IsSpawned(transform))
			{
				this.m_sControlNote.NotePool.Despawn(transform);
			}
		}
		this.m_arrRoundLine.Clear();
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0003BBBC File Offset: 0x00039DBC
	private void InitPos(int iTrack)
	{
		ArrayList moveInfo = this.m_sEvt.MoveInfo;
		ScoreEventBase scoreEventBase = (ScoreEventBase)moveInfo[this.m_sEvt.NextIdx];
		Vector3 zero = Vector3.zero;
		zero.z = 80f;
		base.transform.localPosition = zero;
		base.transform.localEulerAngles = GameData.MAXGUIDE[iTrack];
		this.m_sprNote.sprite = this.OnSprite;
		if ((long)scoreEventBase.Attr == 2L || (long)scoreEventBase.Attr == 6L)
		{
			Vector3 localScale = this.m_oNote.transform.localScale;
			float x = localScale.x;
			localScale.x = Mathf.Abs(x) * -1f;
			this.m_oNote.transform.localScale = localScale;
		}
		else if ((long)scoreEventBase.Attr == 3L || (long)scoreEventBase.Attr == 7L)
		{
			Vector3 localScale2 = this.m_oNote.transform.localScale;
			float x2 = localScale2.x;
			localScale2.x = Mathf.Abs(x2);
			this.m_oNote.transform.localScale = localScale2;
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0003BCF0 File Offset: 0x00039EF0
	private void RenderSameLine(float fViewValue)
	{
		if (GameData.SPEEDEFFECTOR == EFFECTOR_SPEED.CHAOS_W)
		{
			return;
		}
		ArrayList sameInfo = this.m_sEvt.SameInfo;
		if (sameInfo.Count == 0)
		{
			return;
		}
		ScoreEventBase scoreEventBase = (ScoreEventBase)sameInfo[0];
		if (scoreEventBase == null)
		{
			return;
		}
		if (this.m_sEvt.JudgmentStart != JUDGMENT_TYPE.JUDGMENT_NONE)
		{
			this.ClearLine();
			return;
		}
		if (scoreEventBase.JudgmentStart != JUDGMENT_TYPE.JUDGMENT_NONE)
		{
			this.ClearLine();
			return;
		}
		if (null == scoreEventBase.objControlNote)
		{
			return;
		}
		GameObject gameObject = base.transform.FindChild("ControlLine").gameObject;
		GameObject gameObject2 = scoreEventBase.objControlNote.transform.FindChild("ControlLine").gameObject;
		gameObject.transform.position = this.m_oNote.transform.position;
		gameObject2.transform.position = scoreEventBase.objControlNote.transform.FindChild("Note").transform.position;
		if (!this.m_sEvt.SameCheck)
		{
			this.m_sControlNote.RotNote(gameObject2, this.m_sControlNote.GetLinePos(this.m_sEvt.Track), this.m_sControlNote.GetLinePos(scoreEventBase.Track));
			this.m_sControlNote.RotNote(gameObject, this.m_sControlNote.GetLinePos(scoreEventBase.Track), this.m_sControlNote.GetLinePos(this.m_sEvt.Track));
			this.m_sEvt.SameCheck = true;
		}
		Vector3 position = gameObject.transform.FindChild("LineEnd").transform.position;
		Vector3 position2 = gameObject2.transform.FindChild("LineEnd").transform.position;
		Vector3 linePos = this.m_sControlNote.GetLinePos(this.m_sEvt.Track);
		Vector3 linePos2 = this.m_sControlNote.GetLinePos(scoreEventBase.Track);
		float num = Vector3.Distance(linePos, linePos2);
		float angle = Singleton<GameManager>.instance.GetAngle(new Vector2(linePos.x, linePos.y), new Vector2(linePos2.x, linePos2.y));
		int num2 = (int)(num / 0.4f);
		Vector3 zero = Vector3.zero;
		zero.z = -angle;
		position2.z = this.m_oNote.transform.position.z;
		position.z = position2.z;
		for (int i = 0; i < num2; i++)
		{
			float num3 = (float)i / (float)num2;
			Vector3 vector = Vector3.Lerp(position2, position, num3);
			Transform transform;
			if (num2 == this.m_arrLine.Count)
			{
				transform = (Transform)this.m_arrLine[i];
			}
			else
			{
				transform = this.m_sControlNote.NotePool.Spawn(this.LINKLINE.transform);
			}
			transform.position = vector;
			transform.localEulerAngles = zero;
			transform.transform.FindChild("LinkLine").GetComponent<SpriteRenderer>().color = this.m_cNote * 0.85f;
			if (num2 > this.m_arrLine.Count)
			{
				this.m_arrLine.Add(transform);
			}
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0003C02C File Offset: 0x0003A22C
	private void RenderMultiSameLine(float fViewValue)
	{
		ArrayList sameInfo = this.m_sEvt.SameInfo;
		foreach (object obj in sameInfo)
		{
			ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
			if (scoreEventBase == null)
			{
				return;
			}
			if (scoreEventBase.JudgmentStart != JUDGMENT_TYPE.JUDGMENT_NONE)
			{
				this.ClearRound();
				return;
			}
			GameObject objControlNote = scoreEventBase.objControlNote;
			if (null == objControlNote)
			{
				return;
			}
		}
		if (this.m_sEvt.JudgmentStart != JUDGMENT_TYPE.JUDGMENT_NONE)
		{
			this.ClearRound();
			return;
		}
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 80; i++)
		{
			float num = (float)i / 80f;
			zero.z = Mathf.Lerp(0f, 360f, num);
			Transform transform;
			if (this.m_arrRoundLine.Count == 80)
			{
				transform = (Transform)this.m_arrRoundLine[i];
			}
			else
			{
				transform = this.m_sControlNote.NotePool.Spawn(this.ROUNDLINE.transform);
			}
			this.m_sControlNote.LineCenter.transform.localEulerAngles = zero;
			Vector3 position = this.m_sControlNote.LineGuide.transform.position;
			position.z = base.transform.position.z;
			position.x = (1f - fViewValue) * position.x;
			position.y = (1f - fViewValue) * position.y;
			transform.transform.localEulerAngles = zero;
			transform.transform.FindChild("LinkLine").position = position;
			transform.transform.FindChild("LinkLine").GetComponent<SpriteRenderer>().color = this.m_cNote * 0.85f;
			if (80 > this.m_arrRoundLine.Count)
			{
				this.m_arrRoundLine.Add(transform);
			}
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0003C258 File Offset: 0x0003A458
	private void CheckDurTime(int iCurTick)
	{
		ArrayList moveInfo = this.m_sEvt.MoveInfo;
		if (iCurTick < this.m_sEvt.Tick)
		{
			this.m_sEvt.DurRate = 1f;
		}
		ScoreEventBase scoreEventBase = this.m_sEvt;
		for (int i = 0; i < moveInfo.Count; i++)
		{
			ScoreEventBase scoreEventBase2 = (ScoreEventBase)moveInfo[i];
			if (iCurTick < scoreEventBase2.Tick)
			{
				int num;
				if (i == 0)
				{
					num = this.m_sEvt.Tick;
				}
				else
				{
					scoreEventBase = (ScoreEventBase)moveInfo[i - 1];
					num = scoreEventBase.Tick;
				}
				int num2 = scoreEventBase2.Tick - num;
				Vector3 vector = GameData.MAXGUIDE[scoreEventBase.Track];
				Vector3 vector2 = GameData.MAXGUIDE[scoreEventBase2.Track];
				if (scoreEventBase2.Attr == 2 || scoreEventBase2.Attr == 6)
				{
					if (scoreEventBase.Track > scoreEventBase2.Track)
					{
						vector2 = GameData.MAXGUIDE[scoreEventBase2.Track] + new Vector3(0f, 0f, 360f);
					}
				}
				else if ((scoreEventBase2.Attr == 3 || scoreEventBase2.Attr == 7) && scoreEventBase.Track < scoreEventBase.Track)
				{
					vector2 = GameData.MAXGUIDE[scoreEventBase.Track] - new Vector3(0f, 0f, 360f);
				}
				float num3 = Mathf.Abs(vector.z - vector2.z);
				this.m_sEvt.DurRate = num3 / (float)num2 * 3f;
				if (1f > this.m_sEvt.DurRate)
				{
					this.m_sEvt.DurRate = 1f;
				}
				break;
			}
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0003C44C File Offset: 0x0003A64C
	public void SetPosition(int iCurTick, float fViewTps, Vector3 vLinePos)
	{
		this.CheckDurTime(iCurTick);
		ArrayList moveInfo = this.m_sEvt.MoveInfo;
		ScoreEventBase scoreEventBase = (ScoreEventBase)moveInfo[moveInfo.Count - 1];
		ScoreEventBase scoreEventBase2 = (ScoreEventBase)moveInfo[this.m_sEvt.NextIdx];
		Vector3 vector = Vector3.zero;
		float num = this.m_sControlNote.GetNotePos(this.m_sEvt.Tick, this.m_sEvt.Track, fViewTps).z;
		if (0f > num)
		{
			num = 0f;
		}
		vector.z = num;
		base.transform.localPosition = vector;
		int num2 = this.m_sEvt.Tick - iCurTick;
		float num3 = (float)num2 / fViewTps;
		int num4 = this.m_sEvt.Tick - this.m_sEvt.Duration * 2;
		this.m_arrRefRender.Clear();
		this.m_arrRefTailRender.Clear();
		ScoreEventBase scoreEventBase3 = this.m_sEvt;
		int num5 = 0;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = Vector3.zero;
		Vector3 vector4 = GameData.MAXGUIDE[scoreEventBase.Track];
		bool flag = false;
		if (iCurTick > scoreEventBase2.Tick)
		{
			int nextIdx = this.m_sEvt.NextIdx;
			this.m_sEvt.NextIdx++;
			if (moveInfo.Count <= this.m_sEvt.NextIdx)
			{
				this.m_sEvt.NextIdx = moveInfo.Count - 1;
			}
			if (nextIdx != this.m_sEvt.NextIdx)
			{
				scoreEventBase2 = (ScoreEventBase)moveInfo[this.m_sEvt.NextIdx];
				if ((long)scoreEventBase2.Attr == 2L || (long)scoreEventBase2.Attr == 6L)
				{
					Vector3 localScale = this.m_sprNote.transform.localScale;
					float x = localScale.x;
					localScale.x = Mathf.Abs(x) * -1f;
					this.m_sprNote.transform.localScale = localScale;
				}
				else if ((long)scoreEventBase2.Attr == 3L || (long)scoreEventBase2.Attr == 7L)
				{
					Vector3 localScale2 = this.m_sprNote.transform.localScale;
					float x2 = localScale2.x;
					localScale2.x = Mathf.Abs(x2);
					this.m_sprNote.transform.localScale = localScale2;
				}
			}
		}
		if (!this.m_sEvt.OnMark)
		{
			if (iCurTick > num4)
			{
				this.m_sEvt.OnMark = true;
				Vector3 vector5 = this.m_sControlNote.GetFrontLinePos(this.m_sEvt.Track);
				GameObject gameObject = this.m_sControlNote.NotePool.Spawn(this.MOVENOTEMARK.transform).gameObject;
				this.m_sEvt.MarkNote = gameObject;
				this.m_sEvt.MarkNote.transform.position = vector5;
				this.m_sEvt.MarkNote.GetComponent<SpriteRenderer>().color = Color.white;
				foreach (object obj in moveInfo)
				{
					ScoreEventBase scoreEventBase4 = (ScoreEventBase)obj;
					vector5 = this.m_sControlNote.GetFrontLinePos(scoreEventBase4.Track);
					GameObject gameObject2 = this.m_sControlNote.NotePool.Spawn(this.MOVENOTEMARK.transform).gameObject;
					scoreEventBase4.MarkNote = gameObject2;
					scoreEventBase4.MarkNote.transform.position = vector5;
				}
				this.m_sEvt.OnUpMark = true;
			}
		}
		else if (null != this.m_sEvt.MarkNote)
		{
			this.m_sEvt.MarkNote.transform.localScale = this.m_sControlNote.BeatScale;
		}
		for (int i = 0; i < moveInfo.Count; i++)
		{
			ScoreEventBase scoreEventBase5 = (ScoreEventBase)moveInfo[i];
			int num6 = scoreEventBase5.Tick - scoreEventBase3.Tick;
			int num7 = scoreEventBase5.Tick - this.m_sEvt.Duration * 2;
			if (!scoreEventBase5.OnMark)
			{
				if (iCurTick < scoreEventBase5.Tick && iCurTick > num7)
				{
					scoreEventBase5.OnMark = true;
					if (null != scoreEventBase5.MarkNote)
					{
						Singleton<GameManager>.instance.SetView(scoreEventBase5.MarkNote, true);
						scoreEventBase5.MarkNote.GetComponent<SpriteRenderer>().color = this.MARK_END;
					}
				}
			}
			else if (iCurTick > scoreEventBase5.Tick)
			{
				scoreEventBase5.OnMark = false;
				if (null != scoreEventBase5.MarkNote)
				{
					Singleton<GameManager>.instance.SetView(scoreEventBase5.MarkNote, false);
				}
			}
			vector2 = GameData.MAXGUIDE[scoreEventBase3.Track];
			vector3 = GameData.MAXGUIDE[scoreEventBase5.Track];
			if (scoreEventBase5.Attr == 2 || scoreEventBase5.Attr == 6)
			{
				if (scoreEventBase3.Track > scoreEventBase5.Track)
				{
					vector3 = GameData.MAXGUIDE[scoreEventBase5.Track] + new Vector3(0f, 0f, 360f);
				}
			}
			else if ((scoreEventBase5.Attr == 3 || scoreEventBase5.Attr == 7) && scoreEventBase3.Track < scoreEventBase5.Track)
			{
				vector3 = GameData.MAXGUIDE[scoreEventBase5.Track] - new Vector3(0f, 0f, 360f);
			}
			int num8 = num6 / 15 + 1;
			if (iCurTick > this.m_sEvt.Tick && JUDGMENT_TYPE.BREAK < this.m_sEvt.JudgmentStart && iCurTick > scoreEventBase5.Tick)
			{
				this.m_ScorePlayer.SetKeyState(scoreEventBase3.Track, KEYSTATE.MOVE);
				this.m_ScorePlayer.SetKeyState(scoreEventBase5.Track, KEYSTATE.MOVE);
			}
			if (!flag && scoreEventBase5.Tick > iCurTick)
			{
				float num9 = (float)(iCurTick - scoreEventBase3.Tick) / (float)num6;
				vector4 = Vector3.Lerp(vector2, vector3, num9);
				flag = true;
			}
			for (int j = 0; j < num8; j++)
			{
				float num10 = (float)j / (float)num8;
				if (num8 - 1 == j)
				{
					num10 = 1f;
				}
				Vector3 vector6 = Vector3.Lerp(vector2, vector3, num10);
				this.m_sControlNote.LineCenter.transform.localEulerAngles = vector6;
				int num11 = scoreEventBase3.Tick + j * 15;
				int num12 = num11 - iCurTick;
				if (fViewTps >= (float)num12)
				{
					Vector3 position = this.m_sControlNote.LineCenter.transform.position;
					position.z = this.m_sControlNote.GetNotePos(num11, this.m_sEvt.Track, fViewTps).z;
					if (0f > position.z)
					{
						position.z = 0f;
					}
					else
					{
						this.m_sControlNote.LineCenter.transform.position = position;
						float num13 = (float)num12 / fViewTps;
						float logTime = GameData.GetLogTime(num13);
						Vector3 position2 = this.m_sControlNote.LineGuide.transform.position;
						position2.x = (1f - logTime) * position2.x;
						position2.y = (1f - logTime) * position2.y;
						this.m_arrRefTailRender.Add(position2);
					}
				}
			}
			this.m_sControlNote.LineCenter.transform.localPosition = Vector3.zero;
			if (iCurTick < this.m_sEvt.Tick)
			{
				num6 *= 2;
			}
			for (int k = 0; k < num8; k++)
			{
				if (num4 <= iCurTick)
				{
					int num14 = iCurTick - num4;
					int num15 = num5 + k * 15;
					if (num14 >= num15)
					{
						int num16 = scoreEventBase3.Tick + k * 15;
						if (iCurTick <= num16)
						{
							float num17 = (float)k / (float)num8;
							if (moveInfo.Count - 1 == i && num8 - 1 == k)
							{
								num17 = 1f;
							}
							Vector3 vector7 = Vector3.Lerp(vector2, vector3, num17);
							this.m_sControlNote.LineCenter.transform.localEulerAngles = vector7;
							this.m_arrRefRender.Add(this.m_sControlNote.LineTail.transform.position);
						}
					}
				}
			}
			scoreEventBase3 = scoreEventBase5;
			num5 += num6;
		}
		this.m_sTail.SetVertexCount(this.m_arrRefTailRender.Count);
		for (int l = 0; l < this.m_arrRefTailRender.Count; l++)
		{
			this.m_sTail.SetPosition(l, (Vector3)this.m_arrRefTailRender[l]);
		}
		this.m_sLine.SetVertexCount(this.m_arrRefRender.Count);
		for (int m = 0; m < this.m_arrRefRender.Count; m++)
		{
			Vector3 vector8 = (Vector3)this.m_arrRefRender[m];
			vector8.z = 0.01f;
			this.m_sLine.SetPosition(m, vector8);
		}
		float num18 = GameData.GetLogTime(num3);
		if (iCurTick > this.m_sEvt.Tick)
		{
			this.m_sControlNote.LineCenter.transform.localEulerAngles = vector4;
			this.m_sEvt.objControlNote.transform.localEulerAngles = vector4;
			this.m_sEvt.MarkNote.transform.position = this.m_sControlNote.LineTail.transform.position;
			this.m_oNote.transform.position = this.m_sControlNote.LineGuide.transform.position;
		}
		else
		{
			base.transform.localEulerAngles = GameData.MAXGUIDE[this.m_sEvt.Track];
			Vector3 notePos = this.m_sControlNote.GetNotePos(iCurTick, this.m_sEvt.Track, fViewTps);
			notePos.z = this.m_sControlNote.GetNotePos(this.m_sEvt.Tick, this.m_sEvt.Track, fViewTps).z;
			if (0f > num18)
			{
				num18 = 0f;
			}
			notePos.x = (1f - num18) * notePos.x;
			notePos.y = (1f - num18) * notePos.y;
			this.m_oNote.transform.position = notePos;
		}
		this.m_sControlNote.LineCenter.transform.localEulerAngles = GameData.MAXGUIDE[scoreEventBase.Track];
		vector = this.m_sControlNote.GetNotePos(scoreEventBase.Tick, scoreEventBase.Track, fViewTps);
		if (0f > vector.z)
		{
			vector.z = 0f;
		}
		this.m_oNoteEnd.transform.position = vector;
		Color color = Color.white;
		Color color2 = Color.white;
		if (this.m_sEvt.JudgmentStart == JUDGMENT_TYPE.BREAK)
		{
			color = this.FailedMoveNote;
			color2 = this.FailedMoveNote;
		}
		if (iCurTick < scoreEventBase.Tick)
		{
			int num19 = scoreEventBase.Tick - iCurTick;
			float num20 = (float)num19 / fViewTps;
			color2 = this.m_sControlNote.DepthNoteColor(num20, color2);
			color2.a = this.m_sControlNote.GetEffectAlpha(scoreEventBase.Tick, color2.a, this.m_sEvt.Track);
			this.m_oNoteEnd.GetComponent<SpriteRenderer>().color = color2;
		}
		this.m_sprUp.color = color;
		Vector3 localEulerAngles = this.m_sprNoteGroove.transform.localEulerAngles;
		localEulerAngles.z = num3 * 1440f;
		this.m_sprNoteGroove.transform.localEulerAngles = localEulerAngles;
		this.m_sprNoteGroove.transform.localPosition = this.m_sprNote.transform.localPosition;
		color = this.m_sControlNote.DepthNoteColor(num3, color);
		color.a = this.m_sControlNote.GetEffectAlpha(this.m_sEvt.Tick, color.a, this.m_sEvt.Track);
		this.m_sprNote.color = color;
		this.m_sprNoteGroove.color = color;
		this.m_sprUp.color = color;
		this.m_cNote = color;
		this.m_sTail.SetColors(color, color2);
		float num21 = Mathf.Abs(num3);
		float num22 = num21 % 0.25f / 0.25f;
		float num23 = 0.5f + 0.5f * num22;
		this.m_sprNoteGroove.transform.localScale = Vector3.one * 0.7f * num23;
		if (this.m_sEvt.DRAW_DOUBLLINE)
		{
			this.RenderSameLine(num18);
		}
		else if (this.m_sEvt.DRAW_MULTILINE)
		{
			this.RenderMultiSameLine(num18);
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00008F47 File Offset: 0x00007147
	public Vector3 GetNotePos()
	{
		return this.m_oNote.transform.position;
	}

	// Token: 0x04000660 RID: 1632
	private const int MoveValue = 15;

	// Token: 0x04000661 RID: 1633
	private ScorePlayer m_ScorePlayer;

	// Token: 0x04000662 RID: 1634
	public GameObject MOVENOTEMARK;

	// Token: 0x04000663 RID: 1635
	private GameObject m_oNote;

	// Token: 0x04000664 RID: 1636
	private GameObject m_oNoteGroove;

	// Token: 0x04000665 RID: 1637
	private GameObject m_oUp;

	// Token: 0x04000666 RID: 1638
	private GameObject m_oNoteEnd;

	// Token: 0x04000667 RID: 1639
	private SpriteRenderer m_sprNote;

	// Token: 0x04000668 RID: 1640
	private SpriteRenderer m_sprNoteGroove;

	// Token: 0x04000669 RID: 1641
	private SpriteRenderer m_sprUp;

	// Token: 0x0400066A RID: 1642
	private LineRenderer m_sLine;

	// Token: 0x0400066B RID: 1643
	private LineRenderer m_sTail;

	// Token: 0x0400066C RID: 1644
	private ScoreEventBase m_sEvt;

	// Token: 0x0400066D RID: 1645
	private ControlNoteScript m_sControlNote;

	// Token: 0x0400066E RID: 1646
	private ArrayList m_arrRefRender = new ArrayList();

	// Token: 0x0400066F RID: 1647
	private ArrayList m_arrRefTailRender = new ArrayList();

	// Token: 0x04000670 RID: 1648
	private Color MARK_END = new Color(1f, 0.29411766f, 0.22352941f);

	// Token: 0x04000671 RID: 1649
	private Color FailedMoveNote = new Color(0.3f, 0.3f, 0.3f, 1f);

	// Token: 0x04000672 RID: 1650
	private ArrayList m_arrLine = new ArrayList();

	// Token: 0x04000673 RID: 1651
	private ArrayList m_arrRoundLine = new ArrayList();

	// Token: 0x04000674 RID: 1652
	public GameObject LINKLINE;

	// Token: 0x04000675 RID: 1653
	public GameObject ROUNDLINE;

	// Token: 0x04000676 RID: 1654
	private Color m_cNote;

	// Token: 0x04000677 RID: 1655
	public Sprite OnSprite;

	// Token: 0x04000678 RID: 1656
	public Sprite OffSprite;
}
