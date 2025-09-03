using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class ClubTourRewards : MonoBehaviour
{
	// Token: 0x06000BF5 RID: 3061 RVA: 0x00054FE0 File Offset: 0x000531E0
	private void Awake()
	{
		Transform transform = base.transform.FindChild("Bp");
		this.m_lBp = transform.FindChild("Label_Bp").GetComponent<UILabel>();
		this.m_BpAni = transform.FindChild("Ani").GetComponent<TweenPosition>();
		Transform transform2 = base.transform.FindChild("Exp");
		this.m_lExp = transform2.FindChild("Label_Exp").GetComponent<UILabel>();
		this.m_ExpAni = transform2.FindChild("Ani").GetComponent<TweenPosition>();
		this.m_lBoost = base.transform.FindChild("Other").FindChild("Label_Boost_Text").GetComponent<UILabel>();
		this.m_txCD = base.transform.FindChild("Other").FindChild("Texture_DiscCD").GetComponent<UITexture>();
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0000B069 File Offset: 0x00009269
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0000B071 File Offset: 0x00009271
	private void Init()
	{
		this.m_lBp.text = "0";
		this.m_lExp.text = "0";
		this.m_bBpState = false;
		this.m_bExpState = false;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0000B0A1 File Offset: 0x000092A1
	private void RewardSetting()
	{
		this.m_iBPValue = 5000;
		this.m_iExpValue = 3000;
	}

	// Token: 0x04000BB6 RID: 2998
	private UILabel m_lBp;

	// Token: 0x04000BB7 RID: 2999
	private TweenPosition m_BpAni;

	// Token: 0x04000BB8 RID: 3000
	private int m_iBPValue;

	// Token: 0x04000BB9 RID: 3001
	private UILabel m_lExp;

	// Token: 0x04000BBA RID: 3002
	private int m_iExpValue;

	// Token: 0x04000BBB RID: 3003
	private TweenPosition m_ExpAni;

	// Token: 0x04000BBC RID: 3004
	private UILabel m_lBoost;

	// Token: 0x04000BBD RID: 3005
	private UITexture m_txCD;

	// Token: 0x04000BBE RID: 3006
	private bool m_bBpState;

	// Token: 0x04000BBF RID: 3007
	private bool m_bExpState;
}
