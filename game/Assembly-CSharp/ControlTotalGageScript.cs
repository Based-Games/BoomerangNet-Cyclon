using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class ControlTotalGageScript : MonoBehaviour
{
	// Token: 0x060008DA RID: 2266 RVA: 0x00043504 File Offset: 0x00041704
	private void Awake()
	{
		this.m_oControlGrade = base.transform.FindChild("ControlGrade").gameObject;
		for (int i = 0; i < 10; i++)
		{
			this.ArrGrade[i] = this.m_oControlGrade.transform.FindChild("Grade" + i.ToString()).GetComponent<UISprite>();
			this.ArrGrade[i].transform.localPosition = new Vector3(60f * (float)i, 0f, 0f);
			if (this.m_iGrade == i)
			{
				this.ArrGrade[i].spriteName = this.arrGoldGrade[i];
			}
			else
			{
				this.ArrGrade[i].spriteName = this.arrSilverGrade[i];
			}
			this.ArrGrade[i].MakePixelPerfect();
		}
		this.m_rData = Singleton<GameManager>.instance.ResultData;
		this.SetGrade(0, true);
		this.m_oControlBar = base.transform.FindChild("ControlBar").gameObject;
		for (int j = 0; j < 10; j++)
		{
			string text = "Container" + j.ToString();
			this.ArrBar[j] = this.m_oControlBar.transform.FindChild(text).gameObject;
			this.ArrBar[j].transform.localPosition = new Vector3((float)j * 60f, 0f, 0f);
		}
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00009659 File Offset: 0x00007859
	private void Start()
	{
		this.SetGage();
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00009661 File Offset: 0x00007861
	public void SetGage()
	{
		if (this.m_rData.TOTALNOTECOUNT == 0)
		{
			return;
		}
		this.m_fTargetRt = this.m_rData.Accuracy;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00043680 File Offset: 0x00041880
	private void UpdateGrade()
	{
		int gradetype = (int)this.m_rData.GRADETYPE;
		bool flag = false;
		if (gradetype != this.m_iGrade)
		{
			flag = true;
		}
		this.m_iGrade = gradetype;
		this.SetGrade(gradetype, flag);
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x000436B8 File Offset: 0x000418B8
	private void SetGrade(int iGrade, bool bChange)
	{
		for (int i = 0; i < 10; i++)
		{
			if (i == iGrade)
			{
				this.ArrGrade[i].spriteName = this.arrGoldGrade[i];
			}
			else
			{
				this.ArrGrade[i].spriteName = this.arrSilverGrade[i];
			}
			if (bChange)
			{
				this.ArrGrade[i].MakePixelPerfect();
				int num = iGrade - i;
				Vector3 localPosition = this.ArrGrade[i].transform.localPosition;
				localPosition.x = (float)num * 60f * -1f;
				Hashtable hashtable = new Hashtable();
				hashtable["position"] = localPosition;
				hashtable["time"] = 0.5f;
				hashtable["islocal"] = true;
				hashtable["easetype"] = iTween.EaseType.easeOutExpo;
				iTween.MoveTo(this.ArrGrade[i].gameObject, hashtable);
			}
		}
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x000437B0 File Offset: 0x000419B0
	private void UpdateBarMove()
	{
		for (int i = 0; i < 10; i++)
		{
			Vector3 localPosition = this.ArrBar[i].transform.localPosition;
			float num = 600f * this.m_fTargetRt;
			num = (float)i * 60f - num;
			localPosition.x = Mathf.Lerp(localPosition.x, num, Time.deltaTime * 5f);
			this.ArrBar[i].transform.localPosition = localPosition;
		}
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00043830 File Offset: 0x00041A30
	private void UpdateBarSize()
	{
		Vector3 position = base.gameObject.transform.position;
		foreach (UISprite uisprite in this.ArrGrade)
		{
			float num = Mathf.Abs(position.x - uisprite.transform.position.x);
			float num2 = 1f - num * 2.5f;
			if (1f < num2)
			{
				num2 = 1f;
			}
			if (0.2f > num2)
			{
				num2 = 0.2f;
			}
			Vector3 localScale = uisprite.transform.localScale;
			localScale.y = num2;
			localScale.x = num2;
			uisprite.transform.localScale = localScale;
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00009685 File Offset: 0x00007885
	private void Update()
	{
		this.UpdateBarMove();
		this.UpdateGrade();
		this.UpdateBarSize();
	}

	// Token: 0x04000739 RID: 1849
	private const float GradeWidth = 60f;

	// Token: 0x0400073A RID: 1850
	private const int MAX_BAR = 10;

	// Token: 0x0400073B RID: 1851
	private const float BAR_WIDTH = 60f;

	// Token: 0x0400073C RID: 1852
	private const float TOTAL_BAR_WIDTH = 600f;

	// Token: 0x0400073D RID: 1853
	private const float BAR_MAXMOVE = 5f;

	// Token: 0x0400073E RID: 1854
	private GameObject m_oControlGrade;

	// Token: 0x0400073F RID: 1855
	private GameObject m_oControlBar;

	// Token: 0x04000740 RID: 1856
	private int m_iGrade;

	// Token: 0x04000741 RID: 1857
	private UISprite[] ArrGrade = new UISprite[10];

	// Token: 0x04000742 RID: 1858
	private string[] arrGoldGrade = new string[] { "Goldf", "Goldd", "Goldc", "Goldb", "Golda", "GoldAp", "GoldApp", "Golds", "GoldSp", "GoldSpp" };

	// Token: 0x04000743 RID: 1859
	private string[] arrSilverGrade = new string[] { "Silverf", "Silverd", "Silverc", "Silverb", "Silvera", "SilverAp", "SilverApp", "Silvers", "SilverSp", "SilverSpp" };

	// Token: 0x04000744 RID: 1860
	private GameObject[] ArrBar = new GameObject[10];

	// Token: 0x04000745 RID: 1861
	private float m_fTargetRt;

	// Token: 0x04000746 RID: 1862
	private RESULTDATA m_rData;
}
