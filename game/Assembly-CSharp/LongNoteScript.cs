using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class LongNoteScript : MonoBehaviour
{
	// Token: 0x060007D0 RID: 2000 RVA: 0x0003AE54 File Offset: 0x00039054
	private void Awake()
	{
		this.m_arrLine.Clear();
		this.m_oNote = base.transform.FindChild("Note").gameObject;
		this.m_oNoteGroove = base.transform.FindChild("NoteGroove").gameObject;
		this.m_sprNote = this.m_oNote.GetComponent<SpriteRenderer>();
		this.m_sprNoteGroove = this.m_oNoteGroove.GetComponent<SpriteRenderer>();
		this.m_sLine = base.transform.FindChild("Line").GetComponent<LineRenderer>();
		this.m_sEnd = base.transform.FindChild("End").GetComponent<LineRenderer>();
		this.m_sLine.SetVertexCount(2);
		this.m_sEnd.SetVertexCount(2);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00008EE2 File Offset: 0x000070E2
	private void SetControlNote(ControlNoteScript sControlNote)
	{
		this.m_sControlNote = sControlNote;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00008EEB File Offset: 0x000070EB
	private void SetEvent(ScoreEventBase pEvt)
	{
		this.m_sEvt = pEvt;
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00008EF4 File Offset: 0x000070F4
	private void SetFail()
	{
		this.m_sprNote.sprite = this.OffSprite;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0003AF14 File Offset: 0x00039114
	private void InitPos(int iTrack)
	{
		Vector3 zero = Vector3.zero;
		zero.z = 80f;
		base.transform.localPosition = zero;
		base.transform.localEulerAngles = GameData.MAXGUIDE[iTrack];
		this.m_sprNote.sprite = this.OnSprite;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0003AF6C File Offset: 0x0003916C
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

	// Token: 0x060007D6 RID: 2006 RVA: 0x0003AFFC File Offset: 0x000391FC
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0003B08C File Offset: 0x0003928C
	public void SetPosition(int iCurTick, float fViewTps, Vector3 vLinePos)
	{
		int num = this.m_sEvt.Tick - iCurTick;
		float num2 = (float)num / fViewTps;
		int num3 = this.m_sEvt.GetEndTick() - iCurTick;
		float num4 = (float)num3 / fViewTps;
		Vector3 vector = vLinePos;
		vector.z = 80f;
		float logTime = GameData.GetLogTime(num2);
		float logTime2 = GameData.GetLogTime(num4);
		float num5 = Mathf.Abs(logTime);
		float num6 = num5 % 0.25f / 0.25f;
		float num7 = 0.5f + 0.5f * num6;
		this.m_sprNoteGroove.transform.localScale = Vector3.one * num7 * 35f;
		if (this.m_sEvt.JudgmentEnd == JUDGMENT_TYPE.JUDGMENT_NONE || this.m_sEvt.JudgmentEnd == JUDGMENT_TYPE.BREAK)
		{
			float num8 = logTime;
			if (0f > num8)
			{
				num8 = 0f;
			}
			vector.x = (1f - num8) * vLinePos.x;
			vector.y = (1f - num8) * vLinePos.y;
			vector.z = logTime * 80f;
			if (JUDGMENT_TYPE.BREAK < this.m_sEvt.JudgmentStart)
			{
				vector.z = 0f;
			}
		}
		else
		{
			vector = vLinePos;
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vector, 0.5f);
		this.m_sLine.SetPosition(0, vector);
		vector.x = (1f - logTime2) * vLinePos.x;
		vector.y = (1f - logTime2) * vLinePos.y;
		vector.z = logTime2 * 80f;
		Vector3 localEulerAngles = this.m_sprNoteGroove.transform.localEulerAngles;
		localEulerAngles.z = logTime * 1440f;
		this.m_sprNoteGroove.transform.localEulerAngles = localEulerAngles;
		Color color = this.m_sControlNote.DepthNoteColor(logTime, Color.white);
		color.a = this.m_sControlNote.GetEffectAlpha(this.m_sEvt.Tick, color.a, this.m_sEvt.Track);
		this.m_sprNote.color = color;
		this.m_sprNoteGroove.color = color;
		this.m_sLine.SetPosition(1, vector);
		this.m_sEnd.SetPosition(0, vector);
		this.m_sEnd.SetPosition(1, vector + new Vector3(0f, 0f, 3f));
		Color color2 = Color.white * (1f - logTime);
		Color color3 = Color.white * (1f - logTime2);
		color2.a = color.a;
		color3.a = this.m_sControlNote.GetEffectAlpha(this.m_sEvt.Tick + this.m_sEvt.Duration, color3.a, this.m_sEvt.Track);
		this.m_cNote = color;
		this.m_sLine.SetColors(color2, color3);
		this.m_sEnd.SetColors(color3, color3);
		if (this.m_sEvt.DRAW_DOUBLLINE)
		{
			this.RenderSameLine(logTime);
		}
		else if (this.m_sEvt.DRAW_MULTILINE)
		{
			this.RenderMultiSameLine(logTime);
		}
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00008F07 File Offset: 0x00007107
	public Vector3 GetNotePos()
	{
		return this.m_oNote.transform.position;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0003B3D4 File Offset: 0x000395D4
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

	// Token: 0x060007DA RID: 2010 RVA: 0x0003B710 File Offset: 0x00039910
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

	// Token: 0x060007DB RID: 2011 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04000651 RID: 1617
	private GameObject m_oNote;

	// Token: 0x04000652 RID: 1618
	private GameObject m_oNoteGroove;

	// Token: 0x04000653 RID: 1619
	private SpriteRenderer m_sprNote;

	// Token: 0x04000654 RID: 1620
	private SpriteRenderer m_sprNoteGroove;

	// Token: 0x04000655 RID: 1621
	private LineRenderer m_sEnd;

	// Token: 0x04000656 RID: 1622
	private LineRenderer m_sLine;

	// Token: 0x04000657 RID: 1623
	private ScoreEventBase m_sEvt;

	// Token: 0x04000658 RID: 1624
	private ControlNoteScript m_sControlNote;

	// Token: 0x04000659 RID: 1625
	private ArrayList m_arrLine = new ArrayList();

	// Token: 0x0400065A RID: 1626
	private ArrayList m_arrRoundLine = new ArrayList();

	// Token: 0x0400065B RID: 1627
	public GameObject LINKLINE;

	// Token: 0x0400065C RID: 1628
	public GameObject ROUNDLINE;

	// Token: 0x0400065D RID: 1629
	private Color m_cNote;

	// Token: 0x0400065E RID: 1630
	public Sprite OnSprite;

	// Token: 0x0400065F RID: 1631
	public Sprite OffSprite;
}
