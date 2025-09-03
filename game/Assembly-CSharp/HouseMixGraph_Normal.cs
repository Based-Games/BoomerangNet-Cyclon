using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class HouseMixGraph_Normal : MonoBehaviour
{
	// Token: 0x06000D39 RID: 3385 RVA: 0x0005D014 File Offset: 0x0005B214
	private void Awake()
	{
		this.GraphValue = new float[this.GraphCount];
		this.GraphObj = new UISlider[this.GraphCount];
		if (this.Grid.childCount > 0)
		{
			for (int i = 0; i < this.Grid.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.Grid.GetChild(i).gameObject);
			}
		}
		this.GraphPrefab = Resources.Load("Prefab/HouseMix/HouseMixStickGraph") as GameObject;
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0005D09C File Offset: 0x0005B29C
	public void SetGraph(ArrayList ScoreArr)
	{
		if (ScoreArr.Count == 0)
		{
			return;
		}
		if (ScoreArr.Count == 1)
		{
			ScoreArr.Add((int)ScoreArr[0]);
		}
		int num = 0;
		if (ScoreArr.Count < 5)
		{
			this.GraphCount = ScoreArr.Count;
		}
		else
		{
			this.GraphCount = 5;
		}
		for (int i = 0; i < this.GraphCount; i++)
		{
			this.GraphValue[i] = (float)((int)ScoreArr[i]);
			num += (int)ScoreArr[i];
		}
		this.MaxValue = (float)(num / ScoreArr.Count * 2);
		int num2 = this.GraphCount - 1;
		if (num2 < 0)
		{
			num2 = 0;
		}
		this.RevisionGraphCount = (this.MaxStickGraphCount - this.GraphCount) / num2;
		this.m_RevisionGraphCount = this.MaxStickGraphCount - this.GraphCount - this.RevisionGraphCount * num2;
		this.CreateGraph();
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0005D194 File Offset: 0x0005B394
	public void CreateGraph()
	{
		if (this.ObjArray.Count > 0)
		{
			for (int i = 0; i < this.ObjArray.Count; i++)
			{
				UnityEngine.Object.DestroyObject((GameObject)this.ObjArray[i]);
			}
		}
		this.ObjArray.Clear();
		for (int j = 0; j < this.GraphCount; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.GraphPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			this.GraphObj[j] = gameObject.GetComponent<UISlider>();
			this.GraphObj[j].gameObject.name = j.ToString() + "_MainGraph";
			this.GraphObj[j].transform.parent = this.Grid;
			this.GraphObj[j].transform.localScale = Vector3.one;
			this.GraphObj[j].transform.localPosition = Vector3.zero;
			this.GraphObj[j].foregroundWidget.width = (int)this.GraphWidthSize;
			this.ObjArray.Add(gameObject);
		}
		float num = 0f;
		for (int k = 0; k < this.GraphCount; k++)
		{
			this.GraphObj[k].transform.localPosition = new Vector3(num * (this.GraphWidthSize + 2f), 0f, 0f);
			this.GraphObj[k].GetComponent<HouseMixGraphStickAni>().isMainGraph = true;
			this.GraphObj[k].GetComponent<HouseMixGraphStickAni>().LimitMaxValue = this.MaxValue;
			this.GraphObj[k].GetComponent<HouseMixGraphStickAni>().m_MainVlue = this.GraphValue[k];
			this.GraphObj[k].GetComponent<HouseMixGraphStickAni>().Init();
			this.GraphObj[k].GetComponent<HouseMixGraphStickAni>().index = k;
			this.GraphObj[k].GetComponent<HouseMixGraphStickAni>().MaxIndex = this.GraphCount + 1;
			num += 1f;
			if (k < this.GraphCount - 1)
			{
				float num2 = (this.GraphValue[k + 1] - this.GraphValue[k]) / (float)(this.RevisionGraphCount + 1);
				if (k == this.GraphCount - 2)
				{
					this.RevisionGraphCount += this.m_RevisionGraphCount;
				}
				for (int l = 0; l < this.RevisionGraphCount; l++)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(this.GraphPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					UISlider component = gameObject2.GetComponent<UISlider>();
					component.gameObject.name = k.ToString() + "_MainGraph_" + l.ToString();
					component.transform.parent = this.Grid;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					component.foregroundWidget.width = (int)this.GraphWidthSize;
					component.transform.localPosition = new Vector3(num * (this.GraphWidthSize + 2f), 0f, 0f);
					this.ObjArray.Add(gameObject2);
					this.RevisionObj.Add(gameObject2);
					component.value = (this.GraphValue[k] + num2 * (float)UnityEngine.Random.Range(l, this.RevisionGraphCount)) / this.MaxValue;
					component.GetComponent<HouseMixGraphStickAni>().MinValue = this.GraphValue[k];
					component.GetComponent<HouseMixGraphStickAni>().MaxValue = this.GraphValue[k + 1];
					component.GetComponent<HouseMixGraphStickAni>().LimitMaxValue = this.MaxValue;
					component.GetComponent<HouseMixGraphStickAni>().Init();
					component.GetComponent<HouseMixGraphStickAni>().StickAniState = true;
					component.GetComponent<HouseMixGraphStickAni>().index = l + 1;
					component.GetComponent<HouseMixGraphStickAni>().MaxIndex = this.RevisionGraphCount;
					num += 1f;
				}
			}
		}
	}

	// Token: 0x04000D5B RID: 3419
	public Transform Grid;

	// Token: 0x04000D5C RID: 3420
	public int RevisionGraphCount = 5;

	// Token: 0x04000D5D RID: 3421
	private int m_RevisionGraphCount;

	// Token: 0x04000D5E RID: 3422
	private float GraphWidthSize = 6f;

	// Token: 0x04000D5F RID: 3423
	private GameObject GraphPrefab;

	// Token: 0x04000D60 RID: 3424
	private int MaxStickGraphCount = 43;

	// Token: 0x04000D61 RID: 3425
	private int GraphCount = 5;

	// Token: 0x04000D62 RID: 3426
	private float[] GraphValue;

	// Token: 0x04000D63 RID: 3427
	private UISlider[] GraphObj;

	// Token: 0x04000D64 RID: 3428
	private ArrayList ObjArray = new ArrayList();

	// Token: 0x04000D65 RID: 3429
	private ArrayList RevisionObj = new ArrayList();

	// Token: 0x04000D66 RID: 3430
	private float MaxValue = 10000000f;
}
