using System;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class ClubTourMissionCell : MonoBehaviour
{
	// Token: 0x06000BCF RID: 3023 RVA: 0x00053E9C File Offset: 0x0005209C
	private void Awake()
	{
		this.m_gSelectBG = base.transform.FindChild("Sprite_Select").gameObject;
		this.m_txMissionImage = base.transform.FindChild("Texture_Mission").GetComponent<UITexture>();
		this.m_gLock = base.transform.FindChild("Lock").gameObject;
		this.m_gClearIcon = base.transform.FindChild("Sprite_ClearIcon").gameObject;
		this.m_tMssionState = base.transform.FindChild("MissionState");
		for (int i = 0; i < this.MissionNumber.Length; i++)
		{
			this.MissionNumber[i] = this.m_tMssionState.FindChild("Mission_" + (i + 1).ToString()).GetComponent<UISprite>();
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x0000AE5C File Offset: 0x0000905C
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0000AE6E File Offset: 0x0000906E
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.DragProcess();
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0000AE8B File Offset: 0x0000908B
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.ClickProcess();
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0000AEA8 File Offset: 0x000090A8
	private void UIClick()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_CLUBTOUR_TOUCHPACK, false);
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.ClickProcess();
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x0000AECD File Offset: 0x000090CD
	public void ClickProcess()
	{
		this.m_ClubTourManager.MissionSetting(this.m_MissionPackData, this.m_iNum);
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x00053F6C File Offset: 0x0005216C
	public void ClearMissionCheck()
	{
		int num = 0;
		for (int i = 0; i < this.m_MissionPackData.ArrMissionData.Count; i++)
		{
			if (((MissionData)this.m_MissionPackData.ArrMissionData[i]).Cleared)
			{
				num++;
				this.MissionNumber[i].spriteName = "ClubTour_Mission_Num_" + (i + 1).ToString() + "_Clear";
			}
			else
			{
				this.MissionNumber[i].spriteName = "ClubTour_Mission_Num_" + (i + 1).ToString();
			}
		}
		if (this.m_bisLock)
		{
			this.m_tMssionState.gameObject.SetActive(false);
			return;
		}
		if (num == 3)
		{
			this.m_gClearIcon.SetActive(true);
			return;
		}
		this.m_gClearIcon.SetActive(false);
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0000AEE6 File Offset: 0x000090E6
	public void LockCheck(int userLv)
	{
		if (this.m_MissionPackData.Lock)
		{
			this.m_gLock.SetActive(true);
			this.m_bisLock = true;
			return;
		}
		this.m_gLock.SetActive(false);
		this.m_bisLock = false;
	}

	// Token: 0x04000B7A RID: 2938
	public UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04000B7B RID: 2939
	[HideInInspector]
	public ClubTourManager m_ClubTourManager;

	// Token: 0x04000B7C RID: 2940
	[HideInInspector]
	public MissionPackData m_MissionPackData;

	// Token: 0x04000B7D RID: 2941
	[HideInInspector]
	public UITexture m_txMissionImage;

	// Token: 0x04000B7E RID: 2942
	[HideInInspector]
	public UIScroll m_UIScroll;

	// Token: 0x04000B7F RID: 2943
	[HideInInspector]
	public int m_iNum;

	// Token: 0x04000B80 RID: 2944
	[HideInInspector]
	public GameObject m_gSelectBG;

	// Token: 0x04000B81 RID: 2945
	private UISprite[] MissionNumber = new UISprite[3];

	// Token: 0x04000B82 RID: 2946
	private GameObject m_gLock;

	// Token: 0x04000B83 RID: 2947
	private bool m_bisLock;

	// Token: 0x04000B84 RID: 2948
	private GameObject m_gClearIcon;

	// Token: 0x04000B85 RID: 2949
	private Transform m_tMssionState;
}
