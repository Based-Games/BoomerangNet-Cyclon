using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class DirNoteScript : MonoBehaviour
{
	// Token: 0x060007C1 RID: 1985 RVA: 0x00039DD4 File Offset: 0x00037FD4
	private void Awake()
	{
		this.m_oNote = base.transform.FindChild("Note").gameObject;
		this.m_oNoteGroove = base.transform.FindChild("NoteGroove").gameObject;
		this.m_oGuide = base.transform.FindChild("Guide").gameObject;
		this.m_oGuide2 = base.transform.FindChild("Guide2").gameObject;
		this.m_sprNote = this.m_oNote.GetComponent<SpriteRenderer>();
		this.m_sprNoteGroove = this.m_oNoteGroove.GetComponent<SpriteRenderer>();
		this.m_sprGuide = this.m_oGuide.transform.FindChild("Guide").GetComponent<SpriteRenderer>();
		this.m_sprGuide2 = this.m_oGuide2.transform.FindChild("Guide").GetComponent<SpriteRenderer>();
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00008E8D File Offset: 0x0000708D
	private void SetControlNote(ControlNoteScript sControlNote)
	{
		this.m_sControlNote = sControlNote;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00008E96 File Offset: 0x00007096
	private void SetEvent(ScoreEventBase pEvt)
	{
		this.m_sEvt = pEvt;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00008E9F File Offset: 0x0000709F
	private void SetFail()
	{
		this.m_sprNote.sprite = this.OffSprite;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00039EB0 File Offset: 0x000380B0
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

	// Token: 0x060007C6 RID: 1990 RVA: 0x00039F40 File Offset: 0x00038140
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

	// Token: 0x060007C7 RID: 1991 RVA: 0x00039FD0 File Offset: 0x000381D0
	private void InitPos(int iTrack)
	{
		Vector3 zero = Vector3.zero;
		zero.z = 80f;
		base.transform.localPosition = zero;
		base.transform.localEulerAngles = GameData.MAXGUIDE[iTrack];
		this.m_sprNote.sprite = this.OnSprite;
		if (this.m_sEvt.IsDirUpNote())
		{
			Vector3 vector = this.m_oNote.transform.localScale;
			float num = vector.x;
			vector.x = Mathf.Abs(num);
			this.m_oNote.transform.localScale = vector;
			vector = this.m_oGuide.transform.FindChild("Guide").transform.localScale;
			num = vector.x;
			vector.x = Mathf.Abs(num);
			this.m_oGuide.transform.FindChild("Guide").transform.localScale = vector;
			vector = this.m_oGuide2.transform.FindChild("Guide").transform.localScale;
			num = vector.x;
			vector.x = Mathf.Abs(num);
			this.m_oGuide2.transform.FindChild("Guide").transform.localScale = vector;
		}
		else if (this.m_sEvt.IsDirDnNote())
		{
			Vector3 vector2 = this.m_oNote.transform.localScale;
			float num2 = vector2.x;
			vector2.x = Mathf.Abs(num2) * -1f;
			this.m_oNote.transform.localScale = vector2;
			vector2 = this.m_oGuide.transform.FindChild("Guide").transform.localScale;
			num2 = vector2.x;
			vector2.x = Mathf.Abs(num2) * -1f;
			this.m_oGuide.transform.FindChild("Guide").transform.localScale = vector2;
			vector2 = this.m_oGuide2.transform.FindChild("Guide").transform.localScale;
			num2 = vector2.x;
			vector2.x = Mathf.Abs(num2) * -1f;
			this.m_oGuide2.transform.FindChild("Guide").transform.localScale = vector2;
		}
		this.SetGuide();
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0003A22C File Offset: 0x0003842C
	private void SetGuide()
	{
		int track = this.m_sEvt.Track;
		int num = track;
		bool flag = false;
		bool flag2 = false;
		float num2 = 20f;
		float num3 = 20f;
		if (this.m_sEvt.IsDirDnNote())
		{
			num = track - 1;
			int num4 = track + 1;
			if (0 > num)
			{
				num += 12;
				flag = true;
			}
			if (12 <= num4)
			{
				num4 -= 12;
				flag2 = true;
			}
		}
		else if (this.m_sEvt.IsDirUpNote())
		{
			num = track + 1;
			int num4 = track - 1;
			num2 *= -1f;
			num3 *= -1f;
			if (12 <= num)
			{
				num -= 12;
				flag = true;
			}
			if (0 > num4)
			{
				num4 += 12;
				flag = true;
			}
		}
		this.m_vGuide1_Start = GameData.MAXGUIDE[track];
		this.m_vGuide1_End = GameData.MAXGUIDE[num];
		this.m_vGuide2_Start = GameData.MAXGUIDE[track] + new Vector3(0f, 0f, num2);
		this.m_vGuide2_End = GameData.MAXGUIDE[num];
		if (flag)
		{
			if (this.m_sEvt.IsDirDnNote())
			{
				this.m_vGuide1_End.z = this.m_vGuide1_End.z - 360f;
				this.m_vGuide2_End.z = this.m_vGuide2_End.z - 360f;
				if (-360f > this.m_vGuide1_End.z)
				{
					this.m_vGuide1_End.z = this.m_vGuide1_End.z + 360f;
				}
				if (-360f > this.m_vGuide2_End.z)
				{
					this.m_vGuide2_End.z = this.m_vGuide2_End.z + 360f;
				}
			}
			else
			{
				this.m_vGuide1_Start.z = this.m_vGuide1_Start.z - 360f;
				this.m_vGuide2_Start.z = this.m_vGuide2_Start.z - 360f;
				if (-360f > this.m_vGuide1_Start.z)
				{
					this.m_vGuide1_Start.z = this.m_vGuide1_Start.z + 360f;
				}
				if (-360f > this.m_vGuide2_Start.z)
				{
					this.m_vGuide2_Start.z = this.m_vGuide2_Start.z + 360f;
				}
			}
		}
		if (flag2 && this.m_sEvt.IsDirDnNote())
		{
			this.m_vGuide2_Start.z = this.m_vGuide2_Start.z + 360f;
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0003A4A4 File Offset: 0x000386A4
	public void SetPosition(int iCurTick, float fViewTps, float TPS, Vector3 vLinePos)
	{
		int num = this.m_sEvt.Tick - iCurTick;
		float num2 = (float)num / fViewTps;
		float logTime = GameData.GetLogTime(num2);
		float num3 = Mathf.Abs(logTime);
		float num4 = num3 % 0.25f / 0.25f;
		float num5 = 0.5f + 0.5f * num4;
		this.m_sprNoteGroove.transform.localScale = Vector3.one * num5 * 0.7f;
		Vector3 localEulerAngles = this.m_sprNoteGroove.transform.localEulerAngles;
		localEulerAngles.z = logTime * 1440f;
		this.m_sprNoteGroove.transform.localEulerAngles = localEulerAngles;
		Color color = this.m_sControlNote.DepthNoteColor(logTime, Color.white);
		color.a = this.m_sControlNote.GetEffectAlpha(this.m_sEvt.Tick, color.a, this.m_sEvt.Track);
		this.m_sprNote.color = color;
		this.m_sprNoteGroove.color = color;
		this.m_cNote = color;
		float num6 = logTime;
		if (0f > num6)
		{
			num6 = 0f;
		}
		Vector3 zero = Vector3.zero;
		zero.z = logTime * 80f;
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, zero, 0.5f);
		zero.x = (1f - num6) * vLinePos.x;
		zero.y = (1f - num6) * vLinePos.y;
		this.m_oNote.transform.position = zero;
		this.m_oNoteGroove.transform.position = zero;
		float num7 = (float)(iCurTick % (int)TPS) / TPS;
		this.RenderGuideLine(num7, num6, color);
		if (this.m_sEvt.DRAW_DOUBLLINE)
		{
			this.RenderSameLine(logTime);
		}
		else if (this.m_sEvt.DRAW_MULTILINE)
		{
			this.RenderMultiSameLine(num6);
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0003A698 File Offset: 0x00038898
	private void RenderSameLine(float fSideView)
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

	// Token: 0x060007CB RID: 1995 RVA: 0x0003A9D4 File Offset: 0x00038BD4
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

	// Token: 0x060007CC RID: 1996 RVA: 0x0003AC00 File Offset: 0x00038E00
	private void RenderGuideLine(float fBeat, float fSideView, Color cNote)
	{
		float num = 1f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = fBeat * 6f % num;
		bool flag;
		if (0.5f > num4)
		{
			num3 = num4 * 2f;
			flag = true;
		}
		else
		{
			flag = false;
		}
		bool flag2;
		if (0.7f > num4)
		{
			flag2 = false;
		}
		else
		{
			float num5 = num4 - 0.6f;
			num2 = num5 * 2.5f;
			flag2 = true;
		}
		float num6 = this.m_vGuide1_End.z - this.m_vGuide1_Start.z;
		float num7 = (this.m_vGuide1_End.z - this.m_vGuide1_Start.z) * 0.9f;
		float num8 = num6 * 0.65f;
		float num9 = Mathf.Lerp(num8, num7, num2);
		float num10 = num6 * 0.35f;
		float num11 = num6 * 0.45f;
		float num12 = Mathf.Lerp(num10, num11, num3);
		Vector3 zero = Vector3.zero;
		zero.z = num9;
		this.m_oGuide.transform.localEulerAngles = zero;
		zero.z = num12;
		this.m_oGuide2.transform.localEulerAngles = zero;
		Color white = Color.white;
		if (!flag)
		{
			white.a = 0f;
			this.m_sprGuide2.color = white;
		}
		else
		{
			white.a = cNote.a;
			this.m_sprGuide2.color = white;
		}
		if (flag2)
		{
			white.a = cNote.a;
			this.m_sprGuide.color = white;
		}
		else
		{
			float num13 = num4 / 0.5f;
			if (num13 > cNote.a)
			{
				num13 = cNote.a;
			}
			white.a = num13;
			this.m_sprGuide.color = white;
		}
		float num14 = -4.8f;
		if (this.m_sEvt.IsDirUpNote())
		{
			num14 = -5.1f;
		}
		else if (this.m_sEvt.IsDirDnNote())
		{
			num14 = -4.5f;
		}
		this.m_sprGuide.transform.localPosition = new Vector3(0f, num14 * (1f - fSideView), 0f);
		this.m_sprGuide2.transform.localPosition = new Vector3(0f, num14 * (1f - fSideView), 0f);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00008EB2 File Offset: 0x000070B2
	public Vector3 GetNotePos()
	{
		return this.m_oNote.transform.position;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x0400063C RID: 1596
	private GameObject m_oNote;

	// Token: 0x0400063D RID: 1597
	private GameObject m_oNoteGroove;

	// Token: 0x0400063E RID: 1598
	private GameObject m_oGuide;

	// Token: 0x0400063F RID: 1599
	private GameObject m_oGuide2;

	// Token: 0x04000640 RID: 1600
	private SpriteRenderer m_sprNote;

	// Token: 0x04000641 RID: 1601
	private SpriteRenderer m_sprNoteGroove;

	// Token: 0x04000642 RID: 1602
	private SpriteRenderer m_sprGuide;

	// Token: 0x04000643 RID: 1603
	private SpriteRenderer m_sprGuide2;

	// Token: 0x04000644 RID: 1604
	private ScoreEventBase m_sEvt;

	// Token: 0x04000645 RID: 1605
	private ControlNoteScript m_sControlNote;

	// Token: 0x04000646 RID: 1606
	private ArrayList m_arrLine = new ArrayList();

	// Token: 0x04000647 RID: 1607
	private ArrayList m_arrRoundLine = new ArrayList();

	// Token: 0x04000648 RID: 1608
	public GameObject LINKLINE;

	// Token: 0x04000649 RID: 1609
	public GameObject ROUNDLINE;

	// Token: 0x0400064A RID: 1610
	private Color m_cNote;

	// Token: 0x0400064B RID: 1611
	public Sprite OnSprite;

	// Token: 0x0400064C RID: 1612
	public Sprite OffSprite;

	// Token: 0x0400064D RID: 1613
	public Vector3 m_vGuide1_Start = Vector3.zero;

	// Token: 0x0400064E RID: 1614
	public Vector3 m_vGuide1_End = Vector3.zero;

	// Token: 0x0400064F RID: 1615
	public Vector3 m_vGuide2_Start = Vector3.zero;

	// Token: 0x04000650 RID: 1616
	public Vector3 m_vGuide2_End = Vector3.zero;
}
