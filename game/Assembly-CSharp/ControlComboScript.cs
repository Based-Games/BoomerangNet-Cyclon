using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class ControlComboScript : MonoBehaviour
{
	// Token: 0x0600084C RID: 2124 RVA: 0x0003EF90 File Offset: 0x0003D190
	private void Awake()
	{
		this.m_rData = Singleton<GameManager>.instance.ResultData;
		this.m_rData.ONEXTREME = false;
		this.m_oMask = GameObject.Find("FillAlpha").gameObject;
		float extremeGage = Singleton<SongManager>.instance.GetExtremeGage(Singleton<GameManager>.instance.UserData.Level);
		GameData.EXTREME_TEMPFILL = 1f - extremeGage;
		this.m_oMask.GetComponent<UISprite>().fillAmount = GameData.EXTREME_TEMPFILL;
		this.m_fDegreeMove = (1f - GameData.EXTREME_TEMPFILL) * 360f;
		GameObject gameObject = base.transform.parent.gameObject;
		this.m_sBtnExtremeBack = gameObject.transform.FindChild("Item").transform.FindChild("Extreme").transform.FindChild("Back").GetComponent<UISprite>();
		this.m_sBtnExtremeGage = this.m_sBtnExtremeBack.transform.FindChild("Gage").GetComponent<UISprite>();
		GameObject gameObject2 = gameObject.transform.FindChild("ControlPanel").gameObject;
		this.m_sExtremeInGage = gameObject2.transform.FindChild("ExtremeInGage").GetComponent<UISprite>();
		this.m_sExtremeInGage.fillAmount = 1f - GameData.EXTREME_TEMPFILL;
		this.m_oLtMask = gameObject2.transform.FindChild("Lt").gameObject;
		this.m_oRtMask = gameObject2.transform.FindChild("Rt").gameObject;
		this.m_sExtremeNoti = base.transform.FindChild("ExtremeNoti").GetComponent<UISprite>();
		this.m_sBtnExtremeGage.gameObject.SetActive(false);
		this.m_sExtremeInGage.gameObject.SetActive(false);
		GameData.START_EXTREME_DEGREE = 0f;
		GameData.END_EXTREME_DEGREE = 360f * GameData.EXTREME_TEMPFILL;
		float num = GameData.END_EXTREME_DEGREE - 180f;
		Vector3 localEulerAngles = this.m_oMask.transform.localEulerAngles;
		localEulerAngles.z = num;
		this.m_oMask.transform.localEulerAngles = localEulerAngles;
		this.SetExtremeMask();
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00009271 File Offset: 0x00007471
	private void SetManager(GameObject oControl)
	{
		this.m_oControl = oControl;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0003F19C File Offset: 0x0003D39C
	private void SetExtremeMask()
	{
		Vector3 vector = Vector3.zero;
		vector.z = GameData.START_EXTREME_DEGREE + 180f - 0.2f;
		this.m_oLtMask.transform.localEulerAngles = vector;
		vector = Vector3.zero;
		vector.z = GameData.END_EXTREME_DEGREE + 180f - 2.5f;
		this.m_oRtMask.transform.localEulerAngles = vector;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0003F208 File Offset: 0x0003D408
	private void StartMoveExtreme()
	{
		float num = UnityEngine.Random.Range(0.5f, 3f);
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = 0;
		hashtable["to"] = 1f;
		hashtable["time"] = num;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "UpdateMoveExtreme";
		iTween.ValueTo(base.gameObject, hashtable);
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0003F290 File Offset: 0x0003D490
	private void UpdateMoveExtreme(float fExtreme)
	{
		float num = Time.deltaTime * 100f;
		Vector3 localEulerAngles = this.m_oMask.transform.localEulerAngles;
		localEulerAngles.z += num;
		this.m_oMask.transform.localEulerAngles = localEulerAngles;
		GameData.END_EXTREME_DEGREE += num;
		GameData.START_EXTREME_DEGREE += num;
		GameData.END_EXTREME_DEGREE %= 360f;
		GameData.START_EXTREME_DEGREE %= 360f;
		this.SetExtremeMask();
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0003F318 File Offset: 0x0003D518
	private void Start()
	{
		this.InitComboNum();
		this.m_arrGage[0] = base.transform.FindChild("ComboGage1").GetComponent<UISprite>();
		this.m_arrGage[1] = base.transform.FindChild("ComboGage2").GetComponent<UISprite>();
		this.m_arrGage[0].fillAmount = 0f;
		this.m_arrGage[1].fillAmount = 0f;
		this.SetDir(0f);
		this.m_sExtreme = base.transform.FindChild("ComboNum").transform.FindChild("Extreme").GetComponent<UISprite>();
		this.InitItem();
		this.SetCombo();
		this.SetComboCount();
		this.ChangeExtreme(this.m_rData.EXTREMESTATE, false);
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003F3E4 File Offset: 0x0003D5E4
	private void InitItem()
	{
		PLAYFNORMALITEM playitem = GameData.PLAYITEM;
		if (playitem != PLAYFNORMALITEM.EXTREME_X2)
		{
			if (playitem == PLAYFNORMALITEM.EXTREME_X3)
			{
				this.m_rData.EXTREMESTATE = EXTREME_STATE.SUPEREXTREME;
				return;
			}
		}
		else
		{
			this.m_rData.EXTREMESTATE = EXTREME_STATE.EXTREME;
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0003F418 File Offset: 0x0003D618
	private void Update()
	{
		if (this.m_bCompleteExtreme)
		{
			float num = 0.134f * (1f - this.m_fLastValue) + 0.746f;
			this.m_arrGage[0].fillAmount = num;
			if (this.m_fLastValue <= 0f)
			{
				this.ChangeExtreme(EXTREME_STATE.EXTREME, true);
				this.m_bCompleteExtreme = false;
				if (Singleton<GameManager>.instance.DEMOPLAY)
				{
					this.StartExtremeZone();
				}
			}
		}
		if (this.m_bCompleteSuperExtreme)
		{
			float num2 = 0.134f * (1f - this.m_fLastValue) + 0.746f;
			this.m_arrGage[0].fillAmount = 1f - num2;
			this.m_arrGage[1].fillAmount = num2;
			if (this.m_fLastValue <= 0f)
			{
				this.ChangeExtreme(EXTREME_STATE.SUPEREXTREME, true);
				this.m_bCompleteSuperExtreme = false;
			}
		}
		this.UpdateRoate();
		this.UpdateExtreme();
		if (this.m_eStartExtreme != ControlComboScript.START_EXTREME.NONE)
		{
			return;
		}
		this.UpdateGage();
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0003F4FC File Offset: 0x0003D6FC
	public void IncCombo()
	{
		this.SetPreCombo();
		RESULTDATA rData = this.m_rData;
		int num = rData.TOTAL_NOBONUSCOMBOCOUNT;
		rData.TOTAL_NOBONUSCOMBOCOUNT = num + 1;
		FEVER_STATE fever_STATE = ControlFeverScript.m_eState;
		if (ControlFeverScript.m_bInitItem)
		{
			fever_STATE = FEVER_STATE.NONE;
		}
		switch (fever_STATE + 1)
		{
		case FEVER_STATE.FEVERx2:
		{
			RESULTDATA rData2 = this.m_rData;
			num = rData2.TOTAL_COMBOCOUNT;
			rData2.TOTAL_COMBOCOUNT = num + 1;
			RESULTDATA rData3 = this.m_rData;
			num = rData3.COMBO;
			rData3.COMBO = num + 1;
			break;
		}
		case FEVER_STATE.FEVERx3:
			this.m_rData.TOTAL_COMBOCOUNT += 2;
			this.m_rData.COMBO += 2;
			break;
		case FEVER_STATE.FEVERx4:
			this.m_rData.TOTAL_COMBOCOUNT += 3;
			this.m_rData.COMBO += 3;
			break;
		case FEVER_STATE.FEVERx5:
			this.m_rData.TOTAL_COMBOCOUNT += 4;
			this.m_rData.COMBO += 4;
			break;
		case FEVER_STATE.MAX:
			this.m_rData.TOTAL_COMBOCOUNT += 5;
			this.m_rData.COMBO += 5;
			break;
		}
		if (this.m_bCompleteExtreme)
		{
			return;
		}
		if (this.m_bCompleteSuperExtreme)
		{
			return;
		}
		if (this.m_eStartExtreme != ControlComboScript.START_EXTREME.NONE)
		{
			return;
		}
		if (this.m_bStartExtreme)
		{
			return;
		}
		switch (fever_STATE + 1)
		{
		case FEVER_STATE.FEVERx2:
		{
			RESULTDATA rData4 = this.m_rData;
			num = rData4.EXTREMECOMBO;
			rData4.EXTREMECOMBO = num + 1;
			return;
		}
		case FEVER_STATE.FEVERx3:
			this.m_rData.EXTREMECOMBO += 2;
			return;
		case FEVER_STATE.FEVERx4:
			this.m_rData.EXTREMECOMBO += 3;
			return;
		case FEVER_STATE.FEVERx5:
			this.m_rData.EXTREMECOMBO += 4;
			return;
		case FEVER_STATE.MAX:
			this.m_rData.EXTREMECOMBO += 5;
			return;
		default:
			return;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x0000927A File Offset: 0x0000747A
	// (set) Token: 0x06000856 RID: 2134 RVA: 0x00009282 File Offset: 0x00007482
	public int CHAOSCOMBO { get; set; }

	// Token: 0x06000857 RID: 2135 RVA: 0x0000928B File Offset: 0x0000748B
	private void InitExtremeCombo()
	{
		this.m_rData.EXTREMECOMBO = 0;
		this.m_fLastValue = 0f;
		this.m_fTarget = 0f;
		this.ChangeExtreme(EXTREME_STATE.NONE, true);
		this.SetViewGage();
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000092BD File Offset: 0x000074BD
	private void UpdateMove(float fValue)
	{
		this.m_fLastValue = fValue;
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0003F6CC File Offset: 0x0003D8CC
	private void SetViewCount()
	{
		if (this.m_rData.COMBO == 0)
		{
			this.m_arrTarget[0] = 0;
			this.m_arrTarget[1] = 0;
			this.m_arrTarget[2] = 0;
			this.m_arrTarget[3] = 0;
			this.m_arrTarget[4] = 0;
		}
		else
		{
			this.m_arrTarget[0] = this.m_rData.COMBO / 10000;
			this.m_arrTarget[1] = this.m_rData.COMBO % 10000 / 1000;
			this.m_arrTarget[2] = this.m_rData.COMBO % 1000 / 100;
			this.m_arrTarget[3] = this.m_rData.COMBO % 100 / 10;
			this.m_arrTarget[4] = this.m_rData.COMBO % 10;
		}
		for (int i = 0; i < 5; i++)
		{
			if (this.m_arrCurrent[i] != this.m_arrTarget[i])
			{
				this.m_arrSame[i] = false;
			}
		}
		if (this.m_rData.COMBO < this.m_iPreCombo)
		{
			this.m_arrUpMove[3] = false;
		}
		else if (this.m_rData.COMBO > this.m_iPreCombo)
		{
			this.m_arrUpMove[3] = true;
		}
		if (this.m_rData.COMBO / 10 < this.m_iPreCombo / 10)
		{
			this.m_arrUpMove[2] = false;
		}
		else if (this.m_rData.COMBO / 10 > this.m_iPreCombo / 10)
		{
			this.m_arrUpMove[2] = true;
		}
		if (this.m_rData.COMBO / 100 < this.m_iPreCombo / 100)
		{
			this.m_arrUpMove[1] = false;
		}
		else if (this.m_rData.COMBO / 100 > this.m_iPreCombo / 100)
		{
			this.m_arrUpMove[1] = true;
		}
		if (this.m_rData.COMBO / 1000 < this.m_iPreCombo / 1000)
		{
			this.m_arrUpMove[0] = false;
			return;
		}
		if (this.m_rData.COMBO / 1000 > this.m_iPreCombo / 1000)
		{
			this.m_arrUpMove[0] = true;
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0003F8DC File Offset: 0x0003DADC
	private void SetComboCount()
	{
		GameObject gameObject = base.transform.FindChild("ComboCount").gameObject;
		this.AllComboNum[0] = gameObject.transform.FindChild("Combo00000").gameObject;
		this.AllComboNum[1] = gameObject.transform.FindChild("Combo0000").gameObject;
		this.AllComboNum[2] = gameObject.transform.FindChild("Combo000").gameObject;
		this.AllComboNum[3] = gameObject.transform.FindChild("Combo00").gameObject;
		this.AllComboNum[4] = gameObject.transform.FindChild("Combo0").gameObject;
		int[] array = new int[] { 8, 9, 0, 1, 2 };
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(this.ComboNum);
				UILabel component = gameObject2.GetComponent<UILabel>();
				gameObject2.transform.parent = this.AllComboNum[i].transform;
				this.AllCombo[i, j] = component;
				int num = array[j];
				component.transform.localPosition = this.AllPos[j];
				component.text = num.ToString();
				component.name = num.ToString();
				component.transform.localScale = Vector3.one;
			}
		}
		this.SetViewCount();
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0003FA54 File Offset: 0x0003DC54
	private void UpdateRoate()
	{
		for (int i = 0; i < 5; i++)
		{
			this.UpdateRotateNum(i, this.m_arrTarget[i]);
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0003FA7C File Offset: 0x0003DC7C
	private void UpdateRotateNum(int iNum, int iTargetNum)
	{
		if (this.m_arrSame[iNum])
		{
			return;
		}
		float num = this.AllPos[1].x * Time.deltaTime * this.m_fArrTime[iNum];
		float num2 = this.AllPos[1].y * Time.deltaTime * this.m_fArrTime[iNum];
		int num3;
		if (this.m_arrUpMove[iNum])
		{
			num3 = 1;
		}
		else
		{
			num3 = -1;
		}
		for (int i = 0; i < 5; i++)
		{
			UILabel uilabel = this.AllCombo[iNum, i];
			Vector3 localPosition = uilabel.transform.localPosition;
			this.AllPrePos[i] = localPosition;
			localPosition.x += num * (float)num3;
			localPosition.y += num2 * (float)num3;
			bool flag = false;
			int num4 = 0;
			if (this.m_arrUpMove[iNum])
			{
				if (this.AllPos[0].y < localPosition.y)
				{
					flag = true;
					num4 = i + 4;
					if (4 < num4)
					{
						num4 -= 5;
					}
				}
			}
			else if (this.AllPos[this.AllPos.Length - 1].y > localPosition.y)
			{
				flag = true;
				num4 = i - 4;
				if (0 > num4)
				{
					num4 += 5;
				}
			}
			if (flag)
			{
				localPosition.x -= this.AllPos[1].x * 5f * (float)num3;
				localPosition.y -= this.AllPos[1].y * 5f * (float)num3;
				int num5 = int.Parse(this.AllCombo[iNum, num4].text) + num3;
				if (9 < num5)
				{
					num5 = 0;
				}
				if (0 > num5)
				{
					num5 = 9;
				}
				uilabel.text = num5.ToString();
				uilabel.name = num5.ToString();
			}
			uilabel.transform.localPosition = localPosition;
		}
		float num6 = 0f;
		float num7 = 0f;
		for (int j = 0; j < 5; j++)
		{
			UILabel uilabel2 = this.AllCombo[iNum, j];
			Vector3 localPosition2 = uilabel2.transform.localPosition;
			if (this.m_arrUpMove[iNum])
			{
				if (0f <= localPosition2.y && this.AllPrePos[j].y < 0f)
				{
					int num8 = int.Parse(uilabel2.text);
					if (iTargetNum == num8)
					{
						num6 = localPosition2.x;
						num7 = localPosition2.y;
						this.m_arrCurrent[iNum] = iTargetNum;
						this.m_arrSame[iNum] = true;
					}
				}
			}
			else if (0f >= localPosition2.y && 0f < this.AllPrePos[j].y)
			{
				int num9 = int.Parse(uilabel2.text);
				if (iTargetNum == num9)
				{
					num6 = localPosition2.x;
					num7 = localPosition2.y;
					this.m_arrCurrent[iNum] = iTargetNum;
					this.m_arrSame[iNum] = true;
				}
			}
		}
		if (this.m_arrSame[iNum])
		{
			for (int k = 0; k < 5; k++)
			{
				UILabel uilabel3 = this.AllCombo[iNum, k];
				Vector3 localPosition3 = uilabel3.transform.localPosition;
				localPosition3.x -= num6;
				localPosition3.y -= num7;
				uilabel3.transform.localPosition = localPosition3;
			}
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0003FDD4 File Offset: 0x0003DFD4
	private void SetDir(float fFill)
	{
		if (1f < fFill)
		{
			fFill = 1f;
		}
		float num = 225f;
		float num2 = fFill * num;
		float num3 = 47f - num2;
		Vector3 localEulerAngles = this.m_oControlDir.transform.localEulerAngles;
		localEulerAngles.z = num3;
		this.m_oControlDir.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0003FE2C File Offset: 0x0003E02C
	private void SetSuperExtremeDir(float fFill)
	{
		float num = 48f;
		float num2 = (1f - fFill) * num;
		float num3 = -178f - num2;
		Vector3 localEulerAngles = this.m_oControlDir.transform.localEulerAngles;
		localEulerAngles.z = num3;
		this.m_oControlDir.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0003FE7C File Offset: 0x0003E07C
	private void SetStartExtremeDir(float fFill)
	{
		float num = 273f;
		float num2 = (1f - fFill) * num;
		float num3 = -226f + num2;
		Vector3 localEulerAngles = this.m_oControlDir.transform.localEulerAngles;
		localEulerAngles.z = num3;
		this.m_oControlDir.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0003FECC File Offset: 0x0003E0CC
	private void InitComboNum()
	{
		GameObject gameObject = base.transform.FindChild("ComboNum").gameObject;
		string[] array = new string[]
		{
			"Num0", "Num10", "Num20", "Num30", "Num40", "Num50", "Num60", "Num70", "Num80", "Num90",
			"NumMax"
		};
		this.m_oControlDir = gameObject.transform.FindChild("ControlDir").gameObject;
		for (int i = 0; i < 11; i++)
		{
			this.ArrComboNum[i] = gameObject.transform.FindChild(array[i]).GetComponent<UISprite>();
			this.ArrOnCombo[i] = false;
		}
		for (int j = 0; j < 11; j++)
		{
			this.SetComboView(j, false);
		}
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0003FFB4 File Offset: 0x0003E1B4
	private void SetComboView(int iCnt, bool bOn)
	{
		if (bOn)
		{
			this.ArrComboNum[iCnt].color = Color.white;
			this.ArrComboNum[iCnt].transform.localScale = Vector3.one * 1.5f;
			Hashtable hashtable = new Hashtable();
			hashtable["scale"] = Vector3.one;
			hashtable["time"] = 1f;
			iTween.ScaleTo(this.ArrComboNum[iCnt].gameObject, hashtable);
			return;
		}
		this.ArrComboNum[iCnt].color = new Color(0.40784314f, 0.40784314f, 0.40784314f, 0.8f);
		this.ArrComboNum[iCnt].transform.localScale = Vector3.one;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00040078 File Offset: 0x0003E278
	private void SetViewGage()
	{
		if (this.m_bCompleteExtreme)
		{
			return;
		}
		if (this.m_bCompleteSuperExtreme)
		{
			return;
		}
		this.m_fTarget = (float)this.m_rData.EXTREMECOMBO / GameData.MAXCOMBO;
		float num = 0.626f;
		float num2 = 0f;
		if (this.m_rData.EXTREMESTATE == EXTREME_STATE.NONE)
		{
			num2 = (this.m_arrGage[0].fillAmount - 0.12f) / num;
		}
		else if (this.m_rData.EXTREMESTATE == EXTREME_STATE.EXTREME)
		{
			num2 = (this.m_arrGage[1].fillAmount - 0.12f) / num;
		}
		int num3 = (int)(num2 * 10f);
		for (int i = 0; i < 11; i++)
		{
			if (num3 != i)
			{
				if (this.ArrOnCombo[i])
				{
					this.SetComboView(i, false);
					this.ArrOnCombo[i] = false;
				}
			}
			else if (!this.ArrOnCombo[i])
			{
				this.SetComboView(i, true);
				this.ArrOnCombo[i] = true;
			}
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x000092C6 File Offset: 0x000074C6
	public void SetPreCombo()
	{
		this.m_iPreCombo = this.m_rData.COMBO;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x000092D9 File Offset: 0x000074D9
	public void SetCombo()
	{
		this.SetViewCount();
		this.SetViewGage();
		this.m_fStart = this.m_fLastValue;
		this.m_fUpSpeed = 10f;
		this.m_fDnSpeed = 500f;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00040158 File Offset: 0x0003E358
	private void UpdateGage()
	{
		float fTarget = this.m_fTarget;
		if (fTarget > this.m_fStart)
		{
			this.m_fLastValue += Time.deltaTime * this.m_fGageSpeed;
			float num = (fTarget - this.m_fStart) / 2f;
			if (this.m_fLastValue < this.m_fStart + num)
			{
				this.m_fGageSpeed += Time.deltaTime * this.m_fUpSpeed;
				this.m_fUpSpeed += Time.deltaTime * 0.04f;
				this.m_fGageSpeed = Mathf.Min(this.m_fGageSpeed, 1f);
				if (this.m_fLastValue > this.m_fStart + num)
				{
					this.m_fDnSpeed = 500f;
				}
			}
			else
			{
				this.m_fGageSpeed -= Time.deltaTime * this.m_fDnSpeed;
				this.m_fDnSpeed += Time.deltaTime * 0.04f;
				this.m_fGageSpeed = Mathf.Max(this.m_fGageSpeed, 0.1f);
			}
			this.m_fLastValue = Mathf.Min(this.m_fLastValue, fTarget);
		}
		else if (fTarget < this.m_fStart)
		{
			this.m_fLastValue -= Time.deltaTime * this.m_fGageSpeed;
			float num2 = (this.m_fStart - fTarget) / 0.5f;
			if (this.m_fLastValue > this.m_fStart - num2)
			{
				this.m_fGageSpeed += Time.deltaTime * this.m_fUpSpeed;
				this.m_fUpSpeed += Time.deltaTime * 0.04f;
				this.m_fGageSpeed = Mathf.Min(this.m_fGageSpeed, 1f);
				if (this.m_fLastValue < this.m_fStart + num2)
				{
					this.m_fDnSpeed = 500f;
				}
			}
			else
			{
				this.m_fGageSpeed -= Time.deltaTime * this.m_fDnSpeed;
				this.m_fDnSpeed += Time.deltaTime * 0.04f;
				this.m_fGageSpeed = Mathf.Max(this.m_fGageSpeed, 0.1f);
			}
			this.m_fLastValue = Mathf.Max(this.m_fLastValue, fTarget);
		}
		float num3 = this.m_fLastValue * 0.626f + 0.12f;
		if (this.m_bCompleteExtreme)
		{
			this.SetDir(this.m_fLastValue);
			return;
		}
		if (this.m_bCompleteSuperExtreme)
		{
			this.SetSuperExtremeDir(this.m_fLastValue);
			return;
		}
		EXTREME_STATE extreme_STATE = this.m_rData.EXTREMESTATE + 1;
		if (extreme_STATE != EXTREME_STATE.EXTREME)
		{
			if (extreme_STATE != EXTREME_STATE.SUPEREXTREME)
			{
				return;
			}
			this.m_arrGage[0].fillAmount = 1f - num3;
			this.m_arrGage[1].fillAmount = num3;
			this.SetDir(this.m_fLastValue);
			if (this.m_fLastValue >= 1f)
			{
				this.m_bCompleteSuperExtreme = true;
				this.m_fTarget = 0f;
				return;
			}
		}
		else
		{
			this.m_arrGage[0].fillAmount = num3;
			this.SetDir(this.m_fLastValue);
			if (this.m_fLastValue >= 1f)
			{
				this.m_bCompleteExtreme = true;
				this.m_rData.EXTREMECOMBO = 0;
				this.m_fTarget = 0f;
			}
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00009309 File Offset: 0x00007509
	public void UpdateGrooveColor(Color cGroove)
	{
		this.m_sExtremeNoti.color = cGroove;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0004045C File Offset: 0x0003E65C
	private void UpdateExtreme()
	{
		if (this.m_bStartExtreme)
		{
			this.m_fTime += Time.deltaTime;
			int num = (int)this.m_rData.EXTREMESTATE;
			if (0 > num)
			{
				num = 0;
			}
			float num2 = 1f - this.m_fTime / GameData.EXTREME_TIME[num];
			this.m_sExtremeInGage.fillAmount = (1f - GameData.EXTREME_TEMPFILL) * num2;
			if (GameData.EXTREME_TIME[num] < this.m_fTime)
			{
				this.EndExtreme();
			}
			float num3 = Time.deltaTime * 100f;
			Vector3 localEulerAngles = this.m_oMask.transform.localEulerAngles;
			localEulerAngles.z += num3;
			this.m_oMask.transform.localEulerAngles = localEulerAngles;
			Vector3 localEulerAngles2 = this.m_sBtnExtremeGage.transform.localEulerAngles;
			localEulerAngles2.z += num3;
			this.m_sBtnExtremeGage.transform.localEulerAngles = localEulerAngles2;
			localEulerAngles.z += this.m_fDegreeMove;
			this.m_sExtremeInGage.transform.localEulerAngles = localEulerAngles;
			GameData.END_EXTREME_DEGREE += num3;
			GameData.START_EXTREME_DEGREE += num3;
			GameData.END_EXTREME_DEGREE %= 360f;
			GameData.START_EXTREME_DEGREE %= 360f;
			this.SetExtremeMask();
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x000405A4 File Offset: 0x0003E7A4
	public void AutoExtremeZone(EXTREME_STATE eState = EXTREME_STATE.EXTREME)
	{
		if (this.m_bStartExtreme)
		{
			return;
		}
		RESULTDATA rData = this.m_rData;
		int num = rData.EXTREMECOUNT;
		rData.EXTREMECOUNT = num + 1;
		if (this.m_rData.EXTREMESTATE == EXTREME_STATE.EXTREME)
		{
			RESULTDATA rData2 = this.m_rData;
			num = rData2.EXTREMECOUNT_x2;
			rData2.EXTREMECOUNT_x2 = num + 1;
		}
		else
		{
			RESULTDATA rData3 = this.m_rData;
			num = rData3.EXTREMECOUNT_x3;
			rData3.EXTREMECOUNT_x3 = num + 1;
		}
		this.m_bStartExtreme = true;
		this.m_sExtremeInGage.gameObject.SetActive(true);
		this.m_sBtnExtremeGage.gameObject.SetActive(true);
		this.m_sExtremeInGage.fillAmount = 1f - GameData.EXTREME_TEMPFILL;
		this.m_fTime = 0f;
		this.m_rData.ONEXTREME = true;
		this.ChangeExtreme(eState, true);
		this.m_sBtnExtremeGage.gameObject.SetActive(true);
		this.m_sBtnExtremeBack.spriteName = "MaxExtreme";
		this.m_sBtnExtremeBack.MakePixelPerfect();
		this.m_sBtnExtremeBack.GetComponent<TweenScale>().enabled = false;
		this.PlayStartGage();
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x000406AC File Offset: 0x0003E8AC
	private void PressItem()
	{
		PLAYFNORMALITEM playitem = GameData.PLAYITEM;
		if (playitem != PLAYFNORMALITEM.EXTREME_X2)
		{
			if (playitem == PLAYFNORMALITEM.EXTREME_X3)
			{
				GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
				this.m_oControl.SendMessage("SetItem");
				return;
			}
		}
		else
		{
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			this.m_oControl.SendMessage("SetItem");
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x000406F4 File Offset: 0x0003E8F4
	public void StartExtremeZone()
	{
		if (this.m_bStartExtreme || this.m_rData.EXTREMESTATE == EXTREME_STATE.NONE || this.m_bCompleteExtreme || this.m_bCompleteSuperExtreme)
		{
			return;
		}
		RESULTDATA rData = this.m_rData;
		int num = rData.EXTREMECOUNT;
		rData.EXTREMECOUNT = num + 1;
		if (this.m_rData.EXTREMESTATE == EXTREME_STATE.EXTREME)
		{
			RESULTDATA rData2 = this.m_rData;
			num = rData2.EXTREMECOUNT_x2;
			rData2.EXTREMECOUNT_x2 = num + 1;
		}
		else
		{
			RESULTDATA rData3 = this.m_rData;
			num = rData3.EXTREMECOUNT_x3;
			rData3.EXTREMECOUNT_x3 = num + 1;
		}
		this.PressItem();
		if (!Singleton<GameManager>.instance.DEMOPLAY || Singleton<GameManager>.instance.DemoSound)
		{
			if (this.m_rData.EXTREMESTATE == EXTREME_STATE.SUPEREXTREME)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_INGAME_EXTREME_2_START, false);
			}
			else
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_INGAME_EXTREME_1_START, false);
			}
		}
		this.m_bStartExtreme = true;
		this.m_rData.ONEXTREME = true;
		this.m_sExtremeInGage.gameObject.SetActive(true);
		this.m_sExtremeNoti.gameObject.SetActive(false);
		this.m_sBtnExtremeGage.gameObject.SetActive(true);
		this.m_sBtnExtremeBack.spriteName = "MaxExtreme";
		this.m_sBtnExtremeBack.MakePixelPerfect();
		this.m_sBtnExtremeBack.GetComponent<TweenScale>().enabled = false;
		this.m_sExtremeInGage.fillAmount = 1f - GameData.EXTREME_TEMPFILL;
		this.m_fTime = 0f;
		this.m_fTarget = 0f;
		this.PlayStartGage();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00040864 File Offset: 0x0003EA64
	private void UpdateGageMove(float fValue)
	{
		ControlComboScript.START_EXTREME eStartExtreme = this.m_eStartExtreme;
		if (eStartExtreme != ControlComboScript.START_EXTREME.ONE)
		{
			if (eStartExtreme == ControlComboScript.START_EXTREME.TWO)
			{
				float num = 0.76f;
				float num2 = (1f - fValue) * num;
				this.m_arrGage[1].fillAmount = 0.88f - num2;
				float num3 = 273f;
				Vector3 localEulerAngles = this.m_oControlDir.transform.localEulerAngles;
				localEulerAngles.z = -226f + num3 * (1f - fValue);
				this.m_oControlDir.transform.localEulerAngles = localEulerAngles;
				return;
			}
		}
		else
		{
			float num4 = 0.76f;
			float num5 = (1f - fValue) * num4;
			this.m_arrGage[0].fillAmount = 0.88f - num5;
			float num6 = 273f;
			Vector3 localEulerAngles2 = this.m_oControlDir.transform.localEulerAngles;
			localEulerAngles2.z = -226f + num6 * (1f - fValue);
			this.m_oControlDir.transform.localEulerAngles = localEulerAngles2;
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00009317 File Offset: 0x00007517
	private void CompleteGageMove()
	{
		if (!this.m_bStartExtreme)
		{
			this.m_eStartExtreme = ControlComboScript.START_EXTREME.NONE;
			this.InitExtremeCombo();
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00040954 File Offset: 0x0003EB54
	private void TempGageMove(float fValue)
	{
		float num = this.m_fLastGageTwo - 0.12f;
		float num2 = (1f - fValue) * num;
		this.m_arrGage[1].fillAmount = this.m_fLastGageTwo - num2;
		num = 0.88f - this.m_fLastGageOne;
		num2 = (1f - fValue) * num;
		this.m_arrGage[0].fillAmount = this.m_fLastGageOne + num2;
		float num3 = 0.626f;
		float num4 = num2 / num3;
		this.SetDir(num4);
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x000409CC File Offset: 0x0003EBCC
	private void CompleteTempGage()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = 0f;
		hashtable["to"] = 1f;
		hashtable["time"] = 0.5f;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "TempTwoMove";
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["oncomplete"] = "CompleteTempTwoGage";
		iTween.ValueTo(base.gameObject, hashtable);
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0000932E File Offset: 0x0000752E
	private void TempTwoMove(float fValue)
	{
		this.SetDir(fValue);
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00040A6C File Offset: 0x0003EC6C
	private void CompleteTempTwoGage()
	{
		this.m_fLastGageOne = 1f;
		this.m_arrGage[0].invert = true;
		Hashtable hashtable = new Hashtable();
		hashtable["from"] = 1f;
		hashtable["to"] = 0f;
		hashtable["time"] = 0.5f;
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["onupdate"] = "UpdateGageMove";
		hashtable["onupdatetarget"] = base.gameObject;
		hashtable["oncomplete"] = "CompleteGageMove";
		iTween.ValueTo(base.gameObject, hashtable);
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00040B28 File Offset: 0x0003ED28
	private void PlayStartGage()
	{
		if (this.m_rData.EXTREMESTATE == EXTREME_STATE.SUPEREXTREME)
		{
			this.m_fLastGageTwo = this.m_arrGage[1].fillAmount;
			Hashtable hashtable = new Hashtable();
			hashtable["from"] = 1f;
			hashtable["to"] = 0f;
			hashtable["time"] = 1f;
			hashtable["onupdatetarget"] = base.gameObject;
			hashtable["onupdate"] = "UpdateGageMove";
			hashtable["onupdatetarget"] = base.gameObject;
			hashtable["oncomplete"] = "CompleteGageMove";
			iTween.ValueTo(base.gameObject, hashtable);
			this.m_eStartExtreme = ControlComboScript.START_EXTREME.TWO;
			return;
		}
		this.m_fLastGageOne = this.m_arrGage[0].fillAmount;
		this.m_fLastGageTwo = this.m_arrGage[1].fillAmount;
		Hashtable hashtable2 = new Hashtable();
		hashtable2["from"] = 1f;
		hashtable2["to"] = 0f;
		hashtable2["time"] = 0.5f;
		hashtable2["onupdatetarget"] = base.gameObject;
		hashtable2["onupdate"] = "TempGageMove";
		hashtable2["onupdatetarget"] = base.gameObject;
		hashtable2["oncomplete"] = "CompleteTempGage";
		iTween.ValueTo(base.gameObject, hashtable2);
		this.m_eStartExtreme = ControlComboScript.START_EXTREME.ONE;
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00040CB4 File Offset: 0x0003EEB4
	private void ChangeExtreme(EXTREME_STATE eState, bool bSound = true)
	{
		this.m_rData.EXTREMESTATE = eState;
		switch (this.m_rData.EXTREMESTATE + 1)
		{
		case EXTREME_STATE.EXTREME:
			this.EnableExtreme(true, "2._0022_EX1White", 0f, false, false);
			this.m_sExtremeNoti.gameObject.SetActive(false);
			this.m_sBtnExtremeBack.spriteName = "BackExtreme";
			this.m_sBtnExtremeBack.MakePixelPerfect();
			this.m_sBtnExtremeGage.gameObject.SetActive(false);
			this.m_sExtremeNoti.gameObject.SetActive(false);
			return;
		case EXTREME_STATE.SUPEREXTREME:
			this.EnableExtreme(true, "2._0019_EX2", 0f, true, bSound);
			if (!this.m_rData.ONEXTREME)
			{
				this.m_sBtnExtremeBack.spriteName = "MaxExtreme2";
				this.m_sBtnExtremeBack.MakePixelPerfect();
				this.m_sBtnExtremeBack.GetComponent<TweenScale>().enabled = true;
			}
			this.m_sExtremeNoti.gameObject.SetActive(true);
			return;
		case EXTREME_STATE.MAX:
			this.EnableExtreme(true, "2._0021_S-EX", 0.88f, true, bSound);
			if (!this.m_rData.ONEXTREME)
			{
				this.m_sBtnExtremeBack.spriteName = "MaxExtreme2";
				this.m_sBtnExtremeBack.MakePixelPerfect();
				this.m_sBtnExtremeBack.GetComponent<TweenScale>().enabled = true;
			}
			this.m_sExtremeNoti.gameObject.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00040E0C File Offset: 0x0003F00C
	private void EnableExtreme(bool isEnabled, string spriteName, float fillAmount, bool hasTwoGages, bool bSound)
	{
		this.m_sExtreme.enabled = isEnabled;
		this.m_sExtreme.spriteName = spriteName;
		this.m_sExtreme.MakePixelPerfect();
		this.SetDir(0f);
		if (hasTwoGages)
		{
			this.m_arrGage[0].enabled = true;
			this.m_arrGage[1].enabled = true;
			this.m_arrGage[1].fillAmount = fillAmount;
			this.m_arrGage[0].invert = false;
		}
		else
		{
			this.m_arrGage[0].enabled = true;
			this.m_arrGage[1].enabled = false;
			this.m_arrGage[0].invert = true;
			this.m_arrGage[0].fillAmount = fillAmount;
		}
		if (bSound)
		{
			bool flag = true;
			if (Singleton<GameManager>.instance.inAttract() && !Singleton<GameManager>.instance.DemoSound)
			{
				flag = false;
			}
			if (flag)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_INGAME_EXTREME_CHARGE, false);
			}
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00040EF0 File Offset: 0x0003F0F0
	private void EndExtreme()
	{
		this.m_fTime = 0f;
		this.m_bStartExtreme = false;
		this.m_rData.ONEXTREME = false;
		this.m_sExtremeInGage.gameObject.SetActive(false);
		this.m_sBtnExtremeGage.gameObject.SetActive(false);
		this.m_rData.EXTREMECOMBO = 0;
		this.m_fLastValue = 0f;
		this.m_fTarget = 0f;
		this.m_rData.EXTREMESTATE = EXTREME_STATE.NONE;
		this.m_eStartExtreme = ControlComboScript.START_EXTREME.NONE;
		this.ChangeExtreme(EXTREME_STATE.NONE, true);
	}

	// Token: 0x040006AF RID: 1711
	private const float MOVEVALUE = 100f;

	// Token: 0x040006B0 RID: 1712
	private const float CHANGEUPSP = 10f;

	// Token: 0x040006B1 RID: 1713
	private const float CHANGEDNSP = 500f;

	// Token: 0x040006B2 RID: 1714
	private const int MAX_COMBOCNT = 11;

	// Token: 0x040006B3 RID: 1715
	private const int MAXCOUNT = 5;

	// Token: 0x040006B4 RID: 1716
	private const int MAXVIEWCOUNT = 5;

	// Token: 0x040006B5 RID: 1717
	private const float GAGESTART = 0.12f;

	// Token: 0x040006B6 RID: 1718
	private const float GAGEEND = 0.746f;

	// Token: 0x040006B7 RID: 1719
	private const float GAGEENDANI = 0.134f;

	// Token: 0x040006B8 RID: 1720
	private const float GAGEEXTREME = 0.88f;

	// Token: 0x040006B9 RID: 1721
	private const float DIR_START = 47f;

	// Token: 0x040006BA RID: 1722
	private const float DIR_END = -178f;

	// Token: 0x040006BB RID: 1723
	private const float DIR_EXTREME = -226f;

	// Token: 0x040006BC RID: 1724
	private const string SPR_MAXEXTREME = "MaxExtreme2";

	// Token: 0x040006BD RID: 1725
	private const string SPR_ONEXTREME = "MaxExtreme";

	// Token: 0x040006BE RID: 1726
	private const string SPR_BASEEXTREME = "BackExtreme";

	// Token: 0x040006BF RID: 1727
	public GameObject ComboNum;

	// Token: 0x040006C0 RID: 1728
	private UISprite[] m_arrGage = new UISprite[2];

	// Token: 0x040006C1 RID: 1729
	private GameObject m_oControlDir;

	// Token: 0x040006C2 RID: 1730
	private GameObject m_oControl;

	// Token: 0x040006C3 RID: 1731
	private UISprite m_sBtnExtremeGage;

	// Token: 0x040006C4 RID: 1732
	private UISprite m_sBtnExtremeBack;

	// Token: 0x040006C5 RID: 1733
	private UISprite m_sExtremeInGage;

	// Token: 0x040006C6 RID: 1734
	private UISprite m_sExtremeNoti;

	// Token: 0x040006C7 RID: 1735
	private GameObject m_oMask;

	// Token: 0x040006C8 RID: 1736
	private GameObject m_oLtMask;

	// Token: 0x040006C9 RID: 1737
	private GameObject m_oRtMask;

	// Token: 0x040006CA RID: 1738
	private bool m_bStartExtreme;

	// Token: 0x040006CB RID: 1739
	private float m_fTime;

	// Token: 0x040006CC RID: 1740
	private int m_iPreCombo;

	// Token: 0x040006CD RID: 1741
	private float m_fStart;

	// Token: 0x040006CE RID: 1742
	private float m_fTarget;

	// Token: 0x040006CF RID: 1743
	private float m_fLastValue;

	// Token: 0x040006D0 RID: 1744
	private float m_fGageSpeed;

	// Token: 0x040006D1 RID: 1745
	private float m_fUpSpeed;

	// Token: 0x040006D2 RID: 1746
	private float m_fDnSpeed;

	// Token: 0x040006D3 RID: 1747
	private bool[] ArrOnCombo = new bool[11];

	// Token: 0x040006D4 RID: 1748
	private UISprite[] ArrComboNum = new UISprite[11];

	// Token: 0x040006D5 RID: 1749
	private UISprite m_sExtreme;

	// Token: 0x040006D6 RID: 1750
	private GameObject[] AllComboNum = new GameObject[5];

	// Token: 0x040006D7 RID: 1751
	private UILabel[,] AllCombo = new UILabel[5, 5];

	// Token: 0x040006D8 RID: 1752
	private Vector3[] AllPos = new Vector3[]
	{
		new Vector3(26f, 100f, 0f),
		new Vector3(13f, 50f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(-13f, -50f, 0f),
		new Vector3(-26f, -100f, 0f)
	};

	// Token: 0x040006D9 RID: 1753
	private Vector3[] AllPrePos = new Vector3[]
	{
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero
	};

	// Token: 0x040006DA RID: 1754
	private float[] m_fArrTime = new float[] { 10f, 10.4f, 10.6f, 10.8f, 10.8f };

	// Token: 0x040006DB RID: 1755
	private bool[] m_arrSame = new bool[] { true, true, true, true, true };

	// Token: 0x040006DC RID: 1756
	private bool[] m_arrUpMove = new bool[] { true, true, true, true, true };

	// Token: 0x040006DD RID: 1757
	private int[] m_arrTarget = new int[5];

	// Token: 0x040006DE RID: 1758
	private int[] m_arrCurrent = new int[5];

	// Token: 0x040006DF RID: 1759
	private bool m_bCompleteExtreme;

	// Token: 0x040006E0 RID: 1760
	private bool m_bCompleteSuperExtreme;

	// Token: 0x040006E1 RID: 1761
	private ControlComboScript.START_EXTREME m_eStartExtreme;

	// Token: 0x040006E2 RID: 1762
	private float m_fLastGageOne;

	// Token: 0x040006E3 RID: 1763
	private float m_fLastGageTwo;

	// Token: 0x040006E4 RID: 1764
	private float m_fDegreeMove;

	// Token: 0x040006E5 RID: 1765
	private RESULTDATA m_rData;

	// Token: 0x020000F4 RID: 244
	public enum START_EXTREME
	{
		// Token: 0x040006E8 RID: 1768
		NONE,
		// Token: 0x040006E9 RID: 1769
		ONE,
		// Token: 0x040006EA RID: 1770
		TWO
	}
}
