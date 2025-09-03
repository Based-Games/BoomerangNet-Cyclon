using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class RaveUpCD : MonoBehaviour
{
	// Token: 0x06000ECF RID: 3791 RVA: 0x0000CDB4 File Offset: 0x0000AFB4
	private void Awake()
	{
		this.m_RaveUpDisc = base.transform.parent.parent.GetComponent<RaveUpDisc>();
		this.m_tHome = base.transform.parent;
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0000CDE2 File Offset: 0x0000AFE2
	private void Start()
	{
		this.m_ts = base.GetComponent<TweenScale>();
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0006A32C File Offset: 0x0006852C
	private void Update()
	{
		if (!this.m_bMoveState)
		{
			return;
		}
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		float num = iPhoneToMouse.instance.GetTouch(0).position.x / vector.x;
		float num2 = iPhoneToMouse.instance.GetTouch(0).position.y / vector.y;
		Vector3 vector2 = base.transform.parent.localPosition + base.transform.parent.parent.localPosition;
		base.transform.localPosition = new Vector3(num * 2560f - 1280f - vector2.x, num2 * 1536f - 768f - vector2.y, 0f);
		this.m_fFrame += Time.deltaTime;
		this.m_fFrame = 0f;
		this.SelectPanelCheck();
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0006A42C File Offset: 0x0006862C
	private void SelectPanelAttachCheck()
	{
		if (this.m_bisAttach)
		{
			return;
		}
		if (this.m_TargetObject == null)
		{
			this.GoHome();
			this.m_SelectObj[0].m_RaveUpSelectAni.NoSpacePosSetting();
			return;
		}
		this.m_TargetObject.m_RaveUpSelectAni.NoSpacePosSetting();
		if (!this.m_btx96)
		{
			this.m_btx96 = true;
			this.m_btx500 = false;
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, this.m_dInfo, base.GetComponent<UITexture>(), null, null);
		}
		if (!this.m_TargetObject.m_bisHaveCD)
		{
			if (this.m_tAttachObj == null)
			{
				this.m_TargetObject.Attach(base.gameObject);
				base.transform.parent = this.m_TargetObject.m_tAttachObj;
				this.m_tAttachObj = this.m_TargetObject.m_tAttachObj;
				if (this.m_ts.to.x != this.m_ts.transform.localScale.x)
				{
					this.m_ts.from = base.transform.localScale;
					this.m_ts.to = Vector3.one;
					this.m_ts.ResetToBeginning();
					this.m_ts.Play(true);
				}
				this.m_bisAttach = true;
			}
			else
			{
				this.m_TargetObject.Attach(base.gameObject);
				base.transform.parent = this.m_TargetObject.m_tAttachObj;
				this.m_tAttachObj = this.m_TargetObject.m_tAttachObj;
				if (this.m_ts.to.x != this.m_ts.transform.localScale.x)
				{
					this.m_ts.from = base.transform.localScale;
					this.m_ts.to = Vector3.one;
					this.m_ts.ResetToBeginning();
					this.m_ts.Play(true);
				}
				this.m_bisAttach = true;
			}
		}
		else if (this.m_tAttachObj == null)
		{
			this.m_TargetObject.m_gcd.GetComponent<RaveUpCD>().m_bisAttach = false;
			this.m_TargetObject.m_gcd.GetComponent<RaveUpCD>().GoHome();
			this.m_TargetObject.Attach(base.gameObject);
			base.transform.parent = this.m_TargetObject.m_tAttachObj;
			this.m_tAttachObj = this.m_TargetObject.m_tAttachObj;
			if (this.m_ts.to.x != this.m_ts.transform.localScale.x)
			{
				this.m_ts.from = base.transform.localScale;
				this.m_ts.to = Vector3.one;
				this.m_ts.ResetToBeginning();
				this.m_ts.Play(true);
			}
			this.m_bisAttach = true;
		}
		else
		{
			this.m_tAttachObj.parent.GetComponent<RaveUpSelect>().Attach(this.m_TargetObject.m_gcd);
			this.m_TargetObject.m_gcd.transform.parent = this.m_tAttachObj;
			this.m_TargetObject.m_gcd.transform.localScale = Vector3.one;
			this.m_TargetObject.m_gcd.GetComponent<RaveUpCD>().m_tAttachObj = this.m_tAttachObj;
			this.m_TargetObject.m_gcd.GetComponent<RaveUpCD>().JustTweenAni();
			this.m_TargetObject.Attach(base.gameObject);
			base.transform.parent = this.m_TargetObject.m_tAttachObj;
			this.m_tAttachObj = this.m_TargetObject.m_tAttachObj;
			if (this.m_ts.to.x != this.m_ts.transform.localScale.x)
			{
				this.m_ts.from = base.transform.localScale;
				this.m_ts.to = Vector3.one;
				this.m_ts.ResetToBeginning();
				this.m_ts.Play(true);
			}
			this.m_bisAttach = true;
		}
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0006A848 File Offset: 0x00068A48
	private void SelectPanelCheck()
	{
		this.m_TargetObject = null;
		int num = 100;
		float num2 = 0f;
		if (!this.m_btx500)
		{
			this.m_btx500 = true;
			this.m_btx96 = false;
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_500, this.m_dInfo, base.GetComponent<UITexture>(), null, null);
		}
		for (int i = 0; i < this.m_tAttachDiscObj.Length; i++)
		{
			int num3 = (int)(Vector3.Distance(base.transform.position, this.m_tAttachDiscObj[i].transform.position) * 100f);
			if (this.m_SelectObj[i].m_bisHaveCD)
			{
				this.m_SelectObj[i].transform.localScale = Vector3.one;
			}
			if (num > num3)
			{
				num = num3;
				if (num < this.m_iAttachDistValue)
				{
					num2 += 1f;
					num = num3;
					this.m_SelectObj[i].SelectReady();
					this.m_TargetObject = this.m_SelectObj[i];
					this.m_ts.from = base.transform.localScale;
					this.m_ts.to = new Vector3(1.25f, 1.25f, 1f);
					this.m_ts.ResetToBeginning();
					this.m_ts.Play(true);
					this.m_iSelectIndex = i;
					for (int j = 0; j < i; j++)
					{
						if (!this.m_SelectObj[j].m_bisHaveCD)
						{
							this.m_SelectObj[j].NoneReady();
						}
						else
						{
							this.m_SelectObj[j].transform.localScale = Vector3.one;
						}
					}
				}
				else
				{
					if (!this.m_SelectObj[i].m_bisHaveCD)
					{
						this.m_SelectObj[i].NoneReady();
					}
					else
					{
						this.m_SelectObj[i].m_gReadyBG.SetActive(false);
					}
					this.m_SelectObj[0].m_RaveUpSelectAni.NoSpacePosSetting();
				}
			}
			else if (!this.m_SelectObj[i].m_bisHaveCD)
			{
				this.m_SelectObj[i].NoneReady();
			}
			else
			{
				this.m_SelectObj[i].m_gReadyBG.SetActive(false);
			}
		}
		if (num2 <= 0f)
		{
			this.m_SelectObj[0].m_RaveUpSelectAni.NoSpacePosSetting();
			this.m_ts.from = base.transform.localScale;
			this.m_ts.to = new Vector3(1.8f, 1.8f, 1f);
			this.m_ts.ResetToBeginning();
			this.m_ts.Play(true);
		}
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0006AAE0 File Offset: 0x00068CE0
	public void TweenAni()
	{
		this.m_ts.enabled = false;
		this.SelectPanelAttachCheck();
		if (!this.m_bisAttach)
		{
			base.transform.parent = this.m_tHome;
		}
		base.GetComponent<TweenPosition>().from = base.transform.localPosition;
		base.GetComponent<TweenPosition>().to = Vector3.zero;
		base.GetComponent<TweenPosition>().ResetToBeginning();
		base.GetComponent<TweenPosition>().Play(true);
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
	public void JustTweenAni()
	{
		base.GetComponent<TweenPosition>().from = base.transform.localPosition;
		base.GetComponent<TweenPosition>().to = Vector3.zero;
		base.GetComponent<TweenPosition>().ResetToBeginning();
		base.GetComponent<TweenPosition>().Play(true);
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0006AB58 File Offset: 0x00068D58
	public void GoHome()
	{
		if (!this.m_btx500)
		{
			this.m_btx500 = true;
			this.m_btx96 = false;
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_500, this.m_dInfo, base.GetComponent<UITexture>(), null, null);
		}
		base.transform.parent = this.m_tHome;
		base.transform.localScale = Vector3.one * 2f;
		this.m_bMoveState = false;
		base.GetComponent<TweenPosition>().from = base.transform.localPosition;
		base.GetComponent<TweenPosition>().to = Vector3.zero;
		base.GetComponent<TweenPosition>().ResetToBeginning();
		base.GetComponent<TweenPosition>().Play(true);
	}

	// Token: 0x04001028 RID: 4136
	public int m_iIndex;

	// Token: 0x04001029 RID: 4137
	public RaveUpSelect[] m_SelectObj;

	// Token: 0x0400102A RID: 4138
	public Transform[] m_tAttachDiscObj;

	// Token: 0x0400102B RID: 4139
	[HideInInspector]
	public RaveUpDisc m_RaveUpDisc;

	// Token: 0x0400102C RID: 4140
	[HideInInspector]
	public Transform m_tHome;

	// Token: 0x0400102D RID: 4141
	[HideInInspector]
	public int m_iLevel;

	// Token: 0x0400102E RID: 4142
	[HideInInspector]
	public Transform m_tAttachObj;

	// Token: 0x0400102F RID: 4143
	[HideInInspector]
	public bool m_bMoveState;

	// Token: 0x04001030 RID: 4144
	[HideInInspector]
	public Texture m_tx500;

	// Token: 0x04001031 RID: 4145
	[HideInInspector]
	public Texture m_tx96;

	// Token: 0x04001032 RID: 4146
	[HideInInspector]
	public float m_fNearDist = 100f;

	// Token: 0x04001033 RID: 4147
	[HideInInspector]
	public RaveUpSelect m_TargetObject;

	// Token: 0x04001034 RID: 4148
	[HideInInspector]
	public bool m_bisAttach;

	// Token: 0x04001035 RID: 4149
	[HideInInspector]
	public string m_sDiscName;

	// Token: 0x04001036 RID: 4150
	[HideInInspector]
	public DiscInfo m_dInfo;

	// Token: 0x04001037 RID: 4151
	private Vector3 m_v3FromPos = Vector3.zero;

	// Token: 0x04001038 RID: 4152
	private int m_iAttachDistValue = 25;

	// Token: 0x04001039 RID: 4153
	private TweenScale m_ts;

	// Token: 0x0400103A RID: 4154
	private float m_fDragSpeed = 2.15f;

	// Token: 0x0400103B RID: 4155
	private bool m_btx500;

	// Token: 0x0400103C RID: 4156
	private bool m_btx96;

	// Token: 0x0400103D RID: 4157
	private int m_iSelectIndex;

	// Token: 0x0400103E RID: 4158
	private float m_fFrame;

	// Token: 0x0400103F RID: 4159
	private float m_fTime = 0.25f;
}
