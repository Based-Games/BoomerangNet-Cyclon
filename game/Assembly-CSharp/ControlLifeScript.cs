using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class ControlLifeScript : MonoBehaviour
{
	// Token: 0x060008AA RID: 2218 RVA: 0x0000954B File Offset: 0x0000774B
	private void Awake()
	{
		this.m_rData = Singleton<GameManager>.instance.ResultData;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00042138 File Offset: 0x00040338
	private void Start()
	{
		this.m_sLife = base.transform.FindChild("LifeGage").GetComponent<UISprite>();
		this.m_sLifeNoti = base.transform.FindChild("LifeNoti").GetComponent<UISprite>();
		GameObject gameObject = base.transform.FindChild("LineContainer").gameObject;
		this.m_arrLine[0] = gameObject.transform.FindChild("StageLine1").gameObject;
		this.m_arrLine[1] = gameObject.transform.FindChild("StageLine2").gameObject;
		this.m_arrLine[2] = gameObject.transform.FindChild("StageLine3").gameObject;
		string[,] array = new string[3, 2];
		array[0, 0] = "StageLine1";
		array[0, 1] = "OffStageLine1";
		array[1, 0] = "StageLine2";
		array[1, 1] = "OffStageLine2";
		array[2, 0] = "StageLine3";
		array[2, 1] = "OffStageLine3";
		string[,] array2 = array;
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
			float num = 1f;
			for (int i = 0; i < 3; i++)
			{
				if (currentAlbum.eDifficult == DISCSET_DIFFICULT.EASY)
				{
					num = GameData.MODE_EZ_DEFICULT[i];
				}
				else if (currentAlbum.eDifficult == DISCSET_DIFFICULT.NORMAL)
				{
					num = GameData.MODE_NM_DEFICULT[i];
				}
				else if (currentAlbum.eDifficult == DISCSET_DIFFICULT.HARD)
				{
					num = GameData.MODE_HD_DEFICULT[i];
				}
				Vector3 localPosition = this.m_arrLine[i].transform.localPosition;
				localPosition.y = -179f + 350f * num;
				this.m_arrLine[i].transform.localPosition = localPosition;
				Vector3 position = this.m_arrLine[i].transform.position;
				position.x = this.GetGagePos(num).x - 0.02f;
				this.m_arrLine[i].transform.position = position;
				if (GameData.Stage == i)
				{
					this.m_arrLine[i].GetComponent<UISprite>().spriteName = array2[i, 0];
					this.m_arrLine[i].GetComponent<UISprite>().MakePixelPerfect();
				}
				else
				{
					this.m_arrLine[i].GetComponent<UISprite>().spriteName = array2[i, 1];
					this.m_arrLine[i].GetComponent<UISprite>().MakePixelPerfect();
				}
				if (i == GameData.Stage)
				{
					this.m_fRaveUpHurdleEnergy = GameData.MAXENERGY * num;
				}
			}
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x000423D8 File Offset: 0x000405D8
	private Vector3 GetGagePos(float fPer)
	{
		Vector3 vector = Vector3.zero;
		Vector3[] path = iTweenPath.GetPath("LifeGage");
		float num = 0.25f;
		if (0.75f <= fPer)
		{
			float num2 = (fPer - 0.75f) / num;
			vector = Vector3.Lerp(path[3], path[4], num2);
		}
		else if (0.5f <= fPer)
		{
			float num3 = (fPer - 0.5f) / num;
			vector = Vector3.Lerp(path[2], path[3], num3);
		}
		else if (0.25f <= fPer)
		{
			float num4 = (fPer - 0.25f) / num;
			vector = Vector3.Lerp(path[1], path[2], num4);
		}
		else
		{
			float num5 = fPer / num;
			vector = Vector3.Lerp(path[0], path[1], num5);
		}
		return vector;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0000955D File Offset: 0x0000775D
	private void SetManager(GameObject oManager)
	{
		this.m_oManager = oManager;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x000424D4 File Offset: 0x000406D4
	private void Update()
	{
		if (this.m_rData.LIFE_GAGE > this.m_fCurrentEnergy)
		{
			this.m_fCurrentEnergy += Time.deltaTime * 75f;
			if (this.m_rData.LIFE_GAGE < this.m_fCurrentEnergy)
			{
				this.m_fCurrentEnergy = this.m_rData.LIFE_GAGE;
			}
		}
		else if (this.m_rData.LIFE_GAGE < this.m_fCurrentEnergy)
		{
			this.m_fCurrentEnergy -= Time.deltaTime * 75f;
			if (this.m_rData.LIFE_GAGE > this.m_fCurrentEnergy)
			{
				this.m_fCurrentEnergy = this.m_rData.LIFE_GAGE;
			}
		}
		this.SetEnergyGage();
		if (null != this.m_oManager)
		{
			if (this.m_fCurrentEnergy < this.m_fRaveUpHurdleEnergy)
			{
				this.m_oManager.SendMessage("SetHurdleUp", false);
			}
			else
			{
				this.m_oManager.SendMessage("SetHurdleUp", true);
			}
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x000425EC File Offset: 0x000407EC
	private bool SetRefillItem()
	{
		if (GameData.REFILLITEM == PLAYFREFILLITEM.NONE)
		{
			return false;
		}
		switch (GameData.REFILLITEM)
		{
		case PLAYFREFILLITEM.REFILL_50:
			this.m_fCurrentEnergy = GameData.MAXENERGY * 0.5f;
			break;
		case PLAYFREFILLITEM.REFILL_70:
			this.m_fCurrentEnergy = GameData.MAXENERGY * 0.7f;
			break;
		case PLAYFREFILLITEM.REFILL_100:
			this.m_fCurrentEnergy = GameData.MAXENERGY * 1f;
			break;
		}
		GameData.REFILLITEM = PLAYFREFILLITEM.NONE;
		this.m_oManager.SendMessage("StartRefillAnimation");
		this.m_rData.LIFE_GAGE = this.m_fCurrentEnergy;
		return true;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00042690 File Offset: 0x00040890
	private void SetEnergyGage()
	{
		bool flag = false;
		if (0f > this.m_fCurrentEnergy)
		{
			flag = true;
			if (this.SetRefillItem())
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.m_fCurrentEnergy = 0f;
			bool flag2 = false;
			if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX && GameData.Stage == 0)
			{
				flag2 = true;
			}
			if (!GameData.NEVERDIE && !flag2)
			{
				this.m_oManager.SendMessage("GameOver");
			}
		}
		float num = this.m_fCurrentEnergy / GameData.MAXENERGY;
		float num2 = 1f * num;
		this.m_sLife.fillAmount = 0f + num2;
		this.CheckNoti();
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00009566 File Offset: 0x00007766
	private void CheckNoti()
	{
		if (0.3f > this.m_sLife.fillAmount)
		{
			this.m_sLifeNoti.gameObject.SetActive(true);
		}
		else
		{
			this.m_sLifeNoti.gameObject.SetActive(false);
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00003648 File Offset: 0x00001848
	public void UpdateGrooveColor(Color cGroove)
	{
	}

	// Token: 0x0400070D RID: 1805
	private const float HEIGHT = 350f;

	// Token: 0x0400070E RID: 1806
	private const float BOTTOM = -179f;

	// Token: 0x0400070F RID: 1807
	private const float GAGE_SPEED = 75f;

	// Token: 0x04000710 RID: 1808
	private const float GAGE_MIN = 0f;

	// Token: 0x04000711 RID: 1809
	private const float GAGE_MAX = 1f;

	// Token: 0x04000712 RID: 1810
	private GameObject m_oManager;

	// Token: 0x04000713 RID: 1811
	private UISprite m_sLife;

	// Token: 0x04000714 RID: 1812
	private UISprite m_sLifeNoti;

	// Token: 0x04000715 RID: 1813
	private float m_fCurrentEnergy = GameData.MAXENERGY;

	// Token: 0x04000716 RID: 1814
	private float m_fRaveUpHurdleEnergy;

	// Token: 0x04000717 RID: 1815
	private GameObject[] m_arrLine = new GameObject[3];

	// Token: 0x04000718 RID: 1816
	private RESULTDATA m_rData;
}
