using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class EventManager : MonoBehaviour
{
	// Token: 0x06000CCF RID: 3279 RVA: 0x0000B87A File Offset: 0x00009A7A
	private void Awake()
	{
		this.m_Position = base.transform.FindChild("EventText");
		this.m_lEvent = this.m_Position.transform.FindChild("Label_Event").GetComponent<UILabel>();
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00059244 File Offset: 0x00057444
	private void Start()
	{
		this.m_lEvent.text = string.Empty;
		this.getEventText();
		this.m_v3StartPos = this.m_Position.transform.localPosition;
		this.m_iMaxIndex = this.m_arrEventText.Count;
		this.m_iEventIndex = 0;
		this.m_fBaseSize = (float)(this.m_lEvent.fontSize / 20);
		if (this.m_bClubTourEvent)
		{
			this.m_fSpeed = 125f;
		}
		else
		{
			this.m_fSpaceValue *= this.m_fBaseSize;
		}
		this.setEventText();
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x000592D8 File Offset: 0x000574D8
	private void getEventText()
	{
		ArrayList arrNotice = Singleton<GameManager>.instance.ArrNotice;
		for (int i = 0; i < arrNotice.Count; i++)
		{
			NoticeInfo noticeInfo = (NoticeInfo)Singleton<GameManager>.instance.ArrNotice[i];
			if (noticeInfo.content2 != null)
			{
				string text = noticeInfo.title + " | " + noticeInfo.content2;
				this.m_arrEventText.Add(text);
			}
		}
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00059344 File Offset: 0x00057544
	private void setEventText()
	{
		if (this.m_arrEventText.Count <= 0)
		{
			this.m_bSetting = true;
			return;
		}
		this.m_sText = (string)this.m_arrEventText[this.m_iEventIndex];
		this.m_lEvent.text = this.m_sText;
		int length = this.m_sText.Length;
		this.m_Position.transform.localPosition = this.m_v3StartPos;
		float num = this.m_fSpaceValue * -(float)length;
		if (this.m_bClubTourEvent)
		{
			if (num > -410f)
			{
				num = -410f + num;
			}
		}
		else if (num > -820f)
		{
			num = -820f + num;
		}
		this.m_v3TargetPos = new Vector3(num, 0f, 0f);
		this.m_bSetting = false;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00059408 File Offset: 0x00057608
	private void FixedUpdate()
	{
		if (this.m_bSetting)
		{
			return;
		}
		if (this.m_v3TargetPos.x >= this.m_Position.transform.localPosition.x)
		{
			this.m_bSetting = true;
			if (this.m_iEventIndex >= this.m_iMaxIndex)
			{
				this.m_iEventIndex = 0;
			}
			this.setEventText();
			this.m_iEventIndex++;
		}
		this.m_Position.transform.localPosition = new Vector3(this.m_Position.transform.localPosition.x + Time.deltaTime * -this.m_fSpeed, 0f, 0f);
	}

	// Token: 0x04000C9E RID: 3230
	public bool m_bClubTourEvent;

	// Token: 0x04000C9F RID: 3231
	private Transform m_Position;

	// Token: 0x04000CA0 RID: 3232
	private Vector3 m_v3StartPos;

	// Token: 0x04000CA1 RID: 3233
	private UILabel m_lEvent;

	// Token: 0x04000CA2 RID: 3234
	private float m_fSpaceValue = 13f;

	// Token: 0x04000CA3 RID: 3235
	private ArrayList m_arrEventText = new ArrayList();

	// Token: 0x04000CA4 RID: 3236
	private string m_sText;

	// Token: 0x04000CA5 RID: 3237
	private Vector3 m_v3TargetPos;

	// Token: 0x04000CA6 RID: 3238
	private int m_iEventIndex;

	// Token: 0x04000CA7 RID: 3239
	private int m_iMaxIndex;

	// Token: 0x04000CA8 RID: 3240
	private float m_fBaseSize;

	// Token: 0x04000CA9 RID: 3241
	private bool m_bSetting;

	// Token: 0x04000CAA RID: 3242
	private float m_fSpeed = 250f;
}
