using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class ClubTourMissionBtn : MonoBehaviour
{
	// Token: 0x06000BCC RID: 3020 RVA: 0x0000ADF1 File Offset: 0x00008FF1
	private void Awake()
	{
		this.m_ClubTourMissionInfoManager = base.transform.parent.parent.GetComponent<ClubTourMissionInfoManager>();
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x0000AE0E File Offset: 0x0000900E
	public void ClickProcess()
	{
		if (this.m_bIsSelect)
		{
			return;
		}
		this.m_bIsSelect = true;
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_CLUBTOUR_TOUCHMISSION, false);
		this.m_ClubTourMissionInfoManager.MissionClick(this.m_BtnKind);
	}

	// Token: 0x04000B76 RID: 2934
	public ClubTourMissionInfoManager.MissionBtn_e m_BtnKind;

	// Token: 0x04000B77 RID: 2935
	private ClubTourMissionInfoManager m_ClubTourMissionInfoManager;

	// Token: 0x04000B78 RID: 2936
	[HideInInspector]
	public UISprite m_spBtnImage;

	// Token: 0x04000B79 RID: 2937
	[HideInInspector]
	public bool m_bIsSelect;
}
