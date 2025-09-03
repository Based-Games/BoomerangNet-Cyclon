using System;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class RaveUpSelectAni : MonoBehaviour
{
	// Token: 0x06000F13 RID: 3859 RVA: 0x0006CA94 File Offset: 0x0006AC94
	private void Awake()
	{
		this.m_selects = new TweenPosition[4];
		for (int i = 0; i < 4; i++)
		{
			this.m_selects[i] = base.transform.FindChild("Select_" + (i + 1).ToString()).GetComponent<TweenPosition>();
		}
		this.m_v3FixPos = new Vector3[4];
		for (int j = 0; j < this.m_v3FixPos.Length; j++)
		{
			this.m_v3FixPos[j] = this.m_selects[j].transform.localPosition;
		}
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0006CB34 File Offset: 0x0006AD34
	public void SpacePosSetting(int isIndex)
	{
		this.m_bSpaceAni = true;
		for (int i = 0; i < this.m_selects.Length; i++)
		{
			if (i < isIndex)
			{
				Vector3 vector = this.m_v3FixPos[i];
				this.m_selects[i].from = this.m_selects[i].transform.localPosition;
				this.m_selects[i].to = new Vector3(vector.x - this.m_fMoveSpaceValue, vector.y, vector.z);
				this.m_selects[i].ResetToBeginning();
				this.m_selects[i].Play(true);
			}
			else if (i == isIndex)
			{
				Vector3 vector2 = this.m_v3FixPos[i];
				this.m_selects[i].from = this.m_selects[i].transform.localPosition;
				this.m_selects[i].to = vector2;
				this.m_selects[i].ResetToBeginning();
				this.m_selects[i].Play(true);
			}
			else
			{
				Vector3 vector3 = this.m_v3FixPos[i];
				this.m_selects[i].from = this.m_selects[i].transform.localPosition;
				this.m_selects[i].to = new Vector3(vector3.x + this.m_fMoveSpaceValue, vector3.y, vector3.z);
				this.m_selects[i].ResetToBeginning();
				this.m_selects[i].Play(true);
			}
		}
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0006CCC8 File Offset: 0x0006AEC8
	public void NoSpacePosSetting()
	{
		for (int i = 0; i < this.m_selects.Length; i++)
		{
			Vector3 vector = this.m_v3FixPos[i];
			this.m_selects[i].from = this.m_selects[i].transform.localPosition;
			this.m_selects[i].to = vector;
			this.m_selects[i].ResetToBeginning();
			this.m_selects[i].Play(true);
		}
	}

	// Token: 0x04001093 RID: 4243
	[HideInInspector]
	public TweenPosition[] m_selects;

	// Token: 0x04001094 RID: 4244
	[HideInInspector]
	public Vector3[] m_v3FixPos = new Vector3[4];

	// Token: 0x04001095 RID: 4245
	private bool m_bSpaceAni;

	// Token: 0x04001096 RID: 4246
	private bool m_bNoSpaceAni;

	// Token: 0x04001097 RID: 4247
	private float m_fMoveSpaceValue = 40f;
}
