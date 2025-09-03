using System;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class ControlFeverScript : MonoBehaviour
{
	// Token: 0x06000894 RID: 2196 RVA: 0x00009462 File Offset: 0x00007662
	private void Awake()
	{
		this.m_rData = Singleton<GameManager>.instance.ResultData;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000417D0 File Offset: 0x0003F9D0
	private void InitItem()
	{
		switch (GameData.PLAYITEM)
		{
		case PLAYFNORMALITEM.FEVER_x2:
			this.m_rData.FEVER_GAGE = 100f;
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx2;
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			ControlFeverScript.m_bInitItem = true;
			break;
		case PLAYFNORMALITEM.FEVER_x3:
			this.m_rData.FEVER_GAGE = 100f;
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx3;
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			ControlFeverScript.m_bInitItem = true;
			break;
		case PLAYFNORMALITEM.FEVER_x4:
			this.m_rData.FEVER_GAGE = 100f;
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx4;
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			ControlFeverScript.m_bInitItem = true;
			break;
		case PLAYFNORMALITEM.FEVER_x5:
			this.m_rData.FEVER_GAGE = 100f;
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx5;
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			ControlFeverScript.m_bInitItem = true;
			break;
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0004189C File Offset: 0x0003FA9C
	private void Start()
	{
		ControlFeverScript.m_eState = FEVER_STATE.NONE;
		ControlFeverScript.m_bInitItem = false;
		this.InitItem();
		GameData.ON_FEVER = false;
		this.m_sFeverGage = base.transform.FindChild("FeverGage").GetComponent<UISprite>();
		this.m_sFeverNum = base.transform.FindChild("NumFever").GetComponent<UISprite>();
		this.m_sFeverFront = base.transform.FindChild("Front").GetComponent<UISprite>();
		this.m_sFeverNoti = base.transform.FindChild("FeverNoti").GetComponent<UISprite>();
		GameObject gameObject = base.transform.parent.gameObject;
		this.m_sBtnFeverBack = gameObject.transform.FindChild("Item").transform.FindChild("Fever").transform.FindChild("Back").GetComponent<UISprite>();
		this.m_sBtnFeverBack.GetComponent<TweenScale>().enabled = false;
		this.m_sBtnFeverGage = this.m_sBtnFeverBack.transform.FindChild("Gage").GetComponent<UISprite>();
		this.m_sBtnFeverGage.gameObject.SetActive(false);
		this.m_sFeverInGage = base.transform.FindChild("InGage").GetComponent<UISprite>();
		this.m_sFeverInGage.fillAmount = 0f;
		this.m_sFeverInGage.gameObject.SetActive(false);
		this.m_sFeverFront.gameObject.SetActive(false);
		this.m_bMaxFever = this.IsMaxFever();
		this.SetFeverGage();
		this.SetFeverNumSprite();
		if (ControlFeverScript.m_bInitItem)
		{
			this.m_sFeverNum.enabled = false;
		}
		if (this.m_bMaxFever)
		{
			this.m_bOnSound = true;
		}
		this.SetMaxFever(this.m_bMaxFever);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00041A54 File Offset: 0x0003FC54
	private void SetMaxFever(bool bMaxFever)
	{
		if (bMaxFever)
		{
			if (!this.m_OnMaxFever)
			{
				this.m_OnMaxFever = true;
				this.m_sFeverFront.gameObject.SetActive(true);
				if (!GameData.ON_FEVER)
				{
					this.m_sBtnFeverBack.spriteName = "MaxFever";
					this.m_sBtnFeverBack.MakePixelPerfect();
					this.m_sBtnFeverBack.GetComponent<TweenScale>().enabled = true;
				}
				this.m_sFeverNoti.gameObject.SetActive(true);
				GameData.ITEM_FEVER = true;
				if (!this.m_bOnSound)
				{
					bool flag = true;
					if (Singleton<GameManager>.instance.DEMOPLAY)
					{
						if (!Singleton<GameManager>.instance.DemoSound)
						{
							flag = false;
						}
						if (!GameData.ON_FEVER)
						{
							this.PressFever();
						}
					}
					if (flag)
					{
						Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_INGAME_FEVER_CHARGE, false);
					}
				}
				else
				{
					this.m_bOnSound = false;
				}
			}
		}
		else
		{
			this.m_OnMaxFever = false;
			this.m_sFeverFront.gameObject.SetActive(false);
			this.m_sFeverNoti.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00009474 File Offset: 0x00007674
	public void SetControl(GameObject oControl)
	{
		this.m_oControl = oControl;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0000947D File Offset: 0x0000767D
	public void SetBack(GameObject oBack)
	{
		this.m_oBack = oBack;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00009486 File Offset: 0x00007686
	public void MinusFeverTime()
	{
		this.m_fFeverTime += 2f;
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0000949A File Offset: 0x0000769A
	public void SetFever(float fFever, bool bNotSound = false)
	{
		this.m_rData.FEVER_GAGE = fFever;
		this.m_bOnSound = bNotSound;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000094AF File Offset: 0x000076AF
	private bool IsMaxFever()
	{
		return 100f <= this.m_rData.FEVER_GAGE;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00041B64 File Offset: 0x0003FD64
	public void AddFever(float fFever)
	{
		this.m_rData.FEVER_GAGE += fFever;
		if (100f < this.m_rData.FEVER_GAGE)
		{
			this.m_rData.FEVER_GAGE = 100f;
		}
		else if (0f > this.m_rData.FEVER_GAGE)
		{
			this.m_rData.FEVER_GAGE = 0f;
		}
		this.m_bMaxFever = this.IsMaxFever();
		this.SetMaxFever(this.m_bMaxFever);
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x000094C9 File Offset: 0x000076C9
	public void PressFever()
	{
		if (GameData.ITEM_FEVER && this.m_OnMaxFever)
		{
			this.StartFever();
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x000094E6 File Offset: 0x000076E6
	public void AutoFever(FEVER_STATE eState = FEVER_STATE.FEVERx2)
	{
		this.m_rData.FEVER_GAGE = 100f;
		this.m_OnMaxFever = true;
		ControlFeverScript.m_eState = eState;
		this.StartFever();
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00041BEC File Offset: 0x0003FDEC
	private void SetFeverNumSprite()
	{
		this.m_sFeverNum.enabled = true;
		FEVER_STATE eState = ControlFeverScript.m_eState;
		switch (eState + 1)
		{
		case FEVER_STATE.FEVERx2:
			this.m_sFeverNum.enabled = false;
			break;
		case FEVER_STATE.FEVERx3:
			this.m_sFeverNum.spriteName = "X-2";
			break;
		case FEVER_STATE.FEVERx4:
			this.m_sFeverNum.spriteName = "X-3";
			break;
		case FEVER_STATE.FEVERx5:
			this.m_sFeverNum.spriteName = "X-4";
			break;
		case FEVER_STATE.MAX:
			this.m_sFeverNum.spriteName = "X-5";
			break;
		}
		this.m_sFeverNum.MakePixelPerfect();
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00041C9C File Offset: 0x0003FE9C
	private void PressItem()
	{
		switch (GameData.PLAYITEM)
		{
		case PLAYFNORMALITEM.FEVER_x2:
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			this.m_oControl.SendMessage("SetItem");
			break;
		case PLAYFNORMALITEM.FEVER_x3:
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			this.m_oControl.SendMessage("SetItem");
			break;
		case PLAYFNORMALITEM.FEVER_x4:
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			this.m_oControl.SendMessage("SetItem");
			break;
		case PLAYFNORMALITEM.FEVER_x5:
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			this.m_oControl.SendMessage("SetItem");
			break;
		}
		ControlFeverScript.m_bInitItem = false;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00041D3C File Offset: 0x0003FF3C
	private void StartFever()
	{
		this.PressItem();
		this.m_rData.FEVERCOUNT++;
		FEVER_STATE eState = ControlFeverScript.m_eState;
		switch (eState + 1)
		{
		case FEVER_STATE.FEVERx2:
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx2;
			this.m_rData.FEVERCOUNT_x2++;
			break;
		case FEVER_STATE.FEVERx3:
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx3;
			this.m_rData.FEVERCOUNT_x3++;
			break;
		case FEVER_STATE.FEVERx4:
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx4;
			this.m_rData.FEVERCOUNT_x4++;
			break;
		case FEVER_STATE.FEVERx5:
			ControlFeverScript.m_eState = FEVER_STATE.FEVERx5;
			this.m_rData.FEVERCOUNT_x5++;
			break;
		}
		this.SetFeverNumSprite();
		this.m_sFeverNum.transform.localScale = Vector3.one * 2f;
		iTween.ScaleTo(this.m_sFeverNum.gameObject, Vector3.one, 0.3f);
		bool flag = true;
		if (Singleton<GameManager>.instance.DEMOPLAY && !Singleton<GameManager>.instance.DemoSound)
		{
			flag = false;
		}
		if (flag)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_INGAME_FEVER_START, false);
		}
		GameData.ON_FEVER = true;
		this.m_bMaxFever = false;
		this.m_fFeverTime = 0f;
		this.m_rData.FEVER_GAGE = 0f;
		if (ControlFeverScript.m_eState != FEVER_STATE.NONE)
		{
			this.m_oBack.GetComponent<UISprite>().spriteName = "BackFever";
		}
		this.m_sFeverInGage.gameObject.SetActive(true);
		this.m_sFeverNoti.gameObject.SetActive(false);
		this.m_sBtnFeverGage.gameObject.SetActive(true);
		this.m_sBtnFeverBack.GetComponent<TweenScale>().enabled = false;
		this.m_sBtnFeverBack.transform.localScale = Vector3.one;
		this.m_sBtnFeverBack.spriteName = "OnFever";
		this.m_sBtnFeverBack.MakePixelPerfect();
		GameData.ITEM_FEVER = false;
		this.m_oControl.SendMessage("OnStartFever");
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0000950B File Offset: 0x0000770B
	private void UpdateColor(Color cValue)
	{
		this.m_oBack.GetComponent<UISprite>().color = cValue;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00041F44 File Offset: 0x00040144
	private void EndFever()
	{
		ControlFeverScript.m_eState = FEVER_STATE.NONE;
		GameData.ON_FEVER = false;
		this.m_OnMaxFever = false;
		this.m_fFeverTime = 0f;
		this.SetFeverNumSprite();
		this.m_sFeverInGage.fillAmount = 0f;
		this.m_sBtnFeverGage.gameObject.SetActive(false);
		this.m_sFeverInGage.gameObject.SetActive(false);
		this.m_sBtnFeverBack.GetComponent<TweenScale>().enabled = false;
		this.m_sBtnFeverBack.transform.localScale = Vector3.one;
		this.m_sBtnFeverBack.spriteName = "BackFever";
		this.m_sBtnFeverBack.MakePixelPerfect();
		this.m_oBack.GetComponent<UISprite>().spriteName = "BackLine";
		this.m_oControl.SendMessage("OnEndFever");
		this.SetMaxFever(this.m_bMaxFever);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0004201C File Offset: 0x0004021C
	private void UpdateOnFever()
	{
		if (!GameData.ON_FEVER)
		{
			return;
		}
		this.m_fFeverTime += Time.deltaTime;
		if (GameData.FEVER_TIME[(int)ControlFeverScript.m_eState] < this.m_fFeverTime)
		{
			if (Singleton<GameManager>.instance.DEMOPLAY && this.m_OnMaxFever)
			{
				this.PressFever();
				return;
			}
			this.EndFever();
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0000951E File Offset: 0x0000771E
	public void UpdateGrooveColor(Color cGroove)
	{
		this.m_sFeverNoti.color = cGroove;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00042084 File Offset: 0x00040284
	private void Update()
	{
		this.UpdateOnFever();
		if (GameData.ON_FEVER)
		{
			float num = this.m_fFeverTime / GameData.FEVER_TIME[(int)ControlFeverScript.m_eState];
			num = 1f - num;
			Vector3 localEulerAngles = this.m_sBtnFeverGage.transform.localEulerAngles;
			localEulerAngles.z -= Time.deltaTime * 100f;
			this.m_sBtnFeverGage.transform.localEulerAngles = localEulerAngles;
			this.m_sFeverInGage.fillAmount = num;
		}
		this.SetFeverGage();
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0004210C File Offset: 0x0004030C
	private void SetFeverGage()
	{
		float num = this.m_rData.FEVER_GAGE / 100f;
		this.m_sFeverGage.fillAmount = num;
	}

	// Token: 0x040006F6 RID: 1782
	private const float COLOR_TIME = 0.5f;

	// Token: 0x040006F7 RID: 1783
	private const float FEVERGAGE_MOVE = 25f;

	// Token: 0x040006F8 RID: 1784
	private const float FEVERGAGE_DNMOVE = 50f;

	// Token: 0x040006F9 RID: 1785
	private const string SPR_ONFEVER = "OnFever";

	// Token: 0x040006FA RID: 1786
	private const string SPR_MAXFEVER = "MaxFever";

	// Token: 0x040006FB RID: 1787
	private const string SPR_BASE = "BackFever";

	// Token: 0x040006FC RID: 1788
	private GameObject m_oControl;

	// Token: 0x040006FD RID: 1789
	private GameObject m_oBack;

	// Token: 0x040006FE RID: 1790
	public static FEVER_STATE m_eState = FEVER_STATE.NONE;

	// Token: 0x040006FF RID: 1791
	private UISprite m_sFeverNum;

	// Token: 0x04000700 RID: 1792
	private UISprite m_sFeverFront;

	// Token: 0x04000701 RID: 1793
	private UISprite m_sFeverGage;

	// Token: 0x04000702 RID: 1794
	private UISprite m_sFeverInGage;

	// Token: 0x04000703 RID: 1795
	private UISprite m_sBtnFeverGage;

	// Token: 0x04000704 RID: 1796
	private UISprite m_sBtnFeverBack;

	// Token: 0x04000705 RID: 1797
	private UISprite m_sFeverNoti;

	// Token: 0x04000706 RID: 1798
	private bool m_OnMaxFever;

	// Token: 0x04000707 RID: 1799
	private float m_fFeverTime;

	// Token: 0x04000708 RID: 1800
	private int m_iFeverBonus;

	// Token: 0x04000709 RID: 1801
	private bool m_bOnSound;

	// Token: 0x0400070A RID: 1802
	private bool m_bMaxFever;

	// Token: 0x0400070B RID: 1803
	private RESULTDATA m_rData;

	// Token: 0x0400070C RID: 1804
	public static bool m_bInitItem;
}
