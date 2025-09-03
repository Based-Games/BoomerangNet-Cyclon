using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class tk2dUIDemo5Controller : tk2dUIBaseDemoController
{
	// Token: 0x060014E9 RID: 5353 RVA: 0x0008BEB8 File Offset: 0x0008A0B8
	private void CustomizeListObject(Transform contentRoot)
	{
		string[] array = new string[] { "Ba", "Po", "Re", "Zu", "Meh", "Ra'", "B'k", "Adam", "Ben", "George" };
		string[] array2 = new string[]
		{
			"Hoopler", "Hysleria", "Yeinydd", "Nekmit", "Novanoid", "Toog1t", "Yboiveth", "Resaix", "Voquev", "Yimello",
			"Oleald", "Digikiki", "Nocobot", "Morath", "Toximble", "Rodrup", "Chillaid", "Brewtine", "Surogou", "Winooze",
			"Hendassa", "Ekcle", "Noelind", "Animepolis", "Tupress", "Jeren", "Yoffa", "Acaer"
		};
		string text = array[UnityEngine.Random.Range(0, array.Length)] + " " + array2[UnityEngine.Random.Range(0, array2.Length)];
		Color color = new Color32((byte)UnityEngine.Random.Range(192, 255), (byte)UnityEngine.Random.Range(192, 255), (byte)UnityEngine.Random.Range(192, 255), byte.MaxValue);
		contentRoot.Find("Name").GetComponent<tk2dTextMesh>().text = text;
		contentRoot.Find("HP").GetComponent<tk2dTextMesh>().text = "HP: " + UnityEngine.Random.Range(100, 512).ToString();
		contentRoot.Find("MP").GetComponent<tk2dTextMesh>().text = "MP: " + (UnityEngine.Random.Range(2, 40) * 10).ToString();
		contentRoot.Find("Portrait").GetComponent<tk2dBaseSprite>().color = color;
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x0008C110 File Offset: 0x0008A310
	private void Start()
	{
		this.prefabItem.transform.parent = null;
		base.DoSetActive(this.prefabItem.transform, false);
		float num = 0f;
		float x = (this.prefabItem.GetMaxBounds() - this.prefabItem.GetMinBounds()).x;
		for (int i = 0; i < 10; i++)
		{
			tk2dUILayout tk2dUILayout = UnityEngine.Object.Instantiate(this.prefabItem) as tk2dUILayout;
			tk2dUILayout.transform.parent = this.manualScrollableArea.contentContainer.transform;
			tk2dUILayout.transform.localPosition = new Vector3(num, 0f, 0f);
			base.DoSetActive(tk2dUILayout.transform, true);
			this.CustomizeListObject(tk2dUILayout.transform);
			num += x;
		}
		this.lastListItem.transform.localPosition = new Vector3(num, this.lastListItem.transform.localPosition.y, 0f);
		num += (this.lastListItem.GetMaxBounds() - this.lastListItem.GetMinBounds()).x;
		this.manualScrollableArea.ContentLength = num;
		for (int j = 0; j < 10; j++)
		{
			tk2dUILayout tk2dUILayout2 = UnityEngine.Object.Instantiate(this.prefabItem) as tk2dUILayout;
			this.autoScrollableArea.ContentLayoutContainer.AddLayoutAtIndex(tk2dUILayout2, tk2dUILayoutItem.FixedSizeLayoutItem(), this.autoScrollableArea.ContentLayoutContainer.ItemCount - 1);
			base.DoSetActive(tk2dUILayout2.transform, true);
			this.CustomizeListObject(tk2dUILayout2.transform);
		}
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x0008C2B8 File Offset: 0x0008A4B8
	private IEnumerator AddSomeItemsManual()
	{
		float x = this.lastListItem.transform.localPosition.x;
		float w = (this.prefabItem.GetMaxBounds() - this.prefabItem.GetMinBounds()).x;
		int numToAdd = UnityEngine.Random.Range(1, 5);
		for (int i = 0; i < numToAdd; i++)
		{
			tk2dUILayout layout = UnityEngine.Object.Instantiate(this.prefabItem) as tk2dUILayout;
			layout.transform.parent = this.manualScrollableArea.contentContainer.transform;
			layout.transform.localPosition = new Vector3(x, 0f, 0f);
			base.DoSetActive(layout.transform, true);
			this.CustomizeListObject(layout.transform);
			x += w;
			this.lastListItem.transform.localPosition = new Vector3(x, this.lastListItem.transform.localPosition.y, 0f);
			this.manualScrollableArea.ContentLength = x + (this.lastListItem.GetMaxBounds() - this.lastListItem.GetMinBounds()).x;
			yield return new WaitForSeconds(0.2f);
		}
		yield break;
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x0008C2D4 File Offset: 0x0008A4D4
	private IEnumerator AddSomeItemsAuto()
	{
		int numToAdd = UnityEngine.Random.Range(1, 5);
		for (int i = 0; i < numToAdd; i++)
		{
			tk2dUILayout layout = UnityEngine.Object.Instantiate(this.prefabItem) as tk2dUILayout;
			this.autoScrollableArea.ContentLayoutContainer.AddLayoutAtIndex(layout, tk2dUILayoutItem.FixedSizeLayoutItem(), this.autoScrollableArea.ContentLayoutContainer.ItemCount - 1);
			base.DoSetActive(layout.transform, true);
			this.CustomizeListObject(layout.transform);
			yield return new WaitForSeconds(0.2f);
		}
		yield break;
	}

	// Token: 0x0400164F RID: 5711
	public tk2dUILayout prefabItem;

	// Token: 0x04001650 RID: 5712
	public tk2dUIScrollableArea manualScrollableArea;

	// Token: 0x04001651 RID: 5713
	public tk2dUILayout lastListItem;

	// Token: 0x04001652 RID: 5714
	public tk2dUIScrollableArea autoScrollableArea;
}
