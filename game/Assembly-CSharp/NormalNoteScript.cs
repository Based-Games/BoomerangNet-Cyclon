using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class NormalNoteScript : MonoBehaviour
{
	// Token: 0x060007EC RID: 2028 RVA: 0x0003D194 File Offset: 0x0003B394
	private void Awake()
	{
		this.m_arrLine.Clear();
		this.m_arrRoundLine.Clear();
		this.m_oNote = base.transform.FindChild("Note").gameObject;
		this.m_oNoteGroove = base.transform.FindChild("NoteGroove").gameObject;
		this.m_sprNote = this.m_oNote.GetComponent<SpriteRenderer>();
		this.m_sprNoteGroove = this.m_oNoteGroove.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00008F77 File Offset: 0x00007177
	private void SetControlNote(ControlNoteScript sControlNote)
	{
		this.m_sControlNote = sControlNote;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00008F80 File Offset: 0x00007180
	private void SetEvent(ScoreEventBase pEvt)
	{
		this.m_sEvt = pEvt;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00008F89 File Offset: 0x00007189
	private void SetFail()
	{
		this.m_sprNote.sprite = this.OffSprite;
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0003D210 File Offset: 0x0003B410
	private void InitPos(int iTrack)
	{
		Vector3 zero = Vector3.zero;
		zero.z = 80f;
		base.transform.localPosition = zero;
		base.transform.localEulerAngles = GameData.MAXGUIDE[iTrack];
		this.m_sprNote.sprite = this.OnSprite;
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0003D268 File Offset: 0x0003B468
	public void SetPosition(int iCurTick, float fViewTps, Vector3 vLinePos)
	{
		int num = this.m_sEvt.Tick - iCurTick;
		float num2 = (float)num / fViewTps;
		float logTime = GameData.GetLogTime(num2);
		float num3 = Mathf.Abs(logTime);
		float num4 = num3 % 0.25f / 0.25f;
		float num5 = 0.5f + 0.5f * num4;
		this.m_sprNoteGroove.transform.localScale = Vector3.one * num5 * 35f;
		Vector3 vector = vLinePos;
		vector.z = 80f;
		float num6 = logTime;
		if (0f > num6)
		{
			num6 = 0f;
		}
		vector.x = (1f - num6) * vLinePos.x;
		vector.y = (1f - num6) * vLinePos.y;
		vector.z = logTime * 80f;
		Vector3 localPosition = base.transform.localPosition;
		vector.x = Mathf.Lerp(localPosition.x, vector.x, 0.5f);
		vector.y = Mathf.Lerp(localPosition.y, vector.y, 0.5f);
		vector.z = Mathf.Lerp(localPosition.z, vector.z, 0.5f);
		base.transform.localPosition = vector;
		Vector3 localEulerAngles = this.m_sprNoteGroove.transform.localEulerAngles;
		localEulerAngles.z = logTime * 1440f;
		this.m_sprNoteGroove.transform.localEulerAngles = localEulerAngles;
		Color color = this.m_sControlNote.DepthNoteColor(logTime, Color.white);
		color.a = this.m_sControlNote.GetEffectAlpha(this.m_sEvt.Tick, color.a, this.m_sEvt.Track);
		this.m_sprNote.color = color;
		this.m_sprNoteGroove.color = color;
		this.m_cNote = color;
		if (this.m_sEvt.DRAW_DOUBLLINE)
		{
			this.RenderSameLine(logTime);
		}
		else if (this.m_sEvt.DRAW_MULTILINE)
		{
			this.RenderMultiSameLine(logTime);
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0003D480 File Offset: 0x0003B680
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
		Vector3 position = gameObject2.transform.FindChild("LineEnd").transform.position;
		Vector3 position2 = gameObject.transform.FindChild("LineEnd").transform.position;
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

	// Token: 0x060007F3 RID: 2035 RVA: 0x0003D7BC File Offset: 0x0003B9BC
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

	// Token: 0x060007F4 RID: 2036 RVA: 0x00008F9C File Offset: 0x0000719C
	public Vector3 GetNotePos()
	{
		return this.m_oNote.transform.position;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
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

	// Token: 0x060007F6 RID: 2038 RVA: 0x0003DA78 File Offset: 0x0003BC78
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

	// Token: 0x060007F7 RID: 2039 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04000679 RID: 1657
	private GameObject m_oNote;

	// Token: 0x0400067A RID: 1658
	private GameObject m_oNoteGroove;

	// Token: 0x0400067B RID: 1659
	private SpriteRenderer m_sprNote;

	// Token: 0x0400067C RID: 1660
	private SpriteRenderer m_sprNoteGroove;

	// Token: 0x0400067D RID: 1661
	private ScoreEventBase m_sEvt;

	// Token: 0x0400067E RID: 1662
	private ControlNoteScript m_sControlNote;

	// Token: 0x0400067F RID: 1663
	private ArrayList m_arrLine = new ArrayList();

	// Token: 0x04000680 RID: 1664
	private ArrayList m_arrRoundLine = new ArrayList();

	// Token: 0x04000681 RID: 1665
	public GameObject LINKLINE;

	// Token: 0x04000682 RID: 1666
	public GameObject ROUNDLINE;

	// Token: 0x04000683 RID: 1667
	private Color m_cNote;

	// Token: 0x04000684 RID: 1668
	public Sprite OnSprite;

	// Token: 0x04000685 RID: 1669
	public Sprite OffSprite;
}
