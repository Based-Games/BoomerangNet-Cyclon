using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200029C RID: 668
[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownMenu")]
public class tk2dUIDropDownMenu : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600131F RID: 4895 RVA: 0x00010531 File Offset: 0x0000E731
	// (remove) Token: 0x06001320 RID: 4896 RVA: 0x0001054A File Offset: 0x0000E74A
	public event Action OnSelectedItemChange;

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06001321 RID: 4897 RVA: 0x00010563 File Offset: 0x0000E763
	// (set) Token: 0x06001322 RID: 4898 RVA: 0x0001056B File Offset: 0x0000E76B
	public List<string> ItemList
	{
		get
		{
			return this.itemList;
		}
		set
		{
			this.itemList = value;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001323 RID: 4899 RVA: 0x00010574 File Offset: 0x0000E774
	// (set) Token: 0x06001324 RID: 4900 RVA: 0x0001057C File Offset: 0x0000E77C
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = Mathf.Clamp(value, 0, this.ItemList.Count - 1);
			this.SetSelectedItem();
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001325 RID: 4901 RVA: 0x0001059E File Offset: 0x0000E79E
	public string SelectedItem
	{
		get
		{
			if (this.index >= 0 && this.index < this.itemList.Count)
			{
				return this.itemList[this.index];
			}
			return string.Empty;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001326 RID: 4902 RVA: 0x000105D9 File Offset: 0x0000E7D9
	// (set) Token: 0x06001327 RID: 4903 RVA: 0x000105F9 File Offset: 0x0000E7F9
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.dropDownButton != null)
			{
				return this.dropDownButton.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.dropDownButton != null && this.dropDownButton.sendMessageTarget != value)
			{
				this.dropDownButton.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001328 RID: 4904 RVA: 0x0001062E File Offset: 0x0000E82E
	// (set) Token: 0x06001329 RID: 4905 RVA: 0x00010636 File Offset: 0x0000E836
	public tk2dUILayout MenuLayoutItem
	{
		get
		{
			return this.menuLayoutItem;
		}
		set
		{
			this.menuLayoutItem = value;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x0600132A RID: 4906 RVA: 0x0001063F File Offset: 0x0000E83F
	// (set) Token: 0x0600132B RID: 4907 RVA: 0x00010647 File Offset: 0x0000E847
	public tk2dUILayout TemplateLayoutItem
	{
		get
		{
			return this.templateLayoutItem;
		}
		set
		{
			this.templateLayoutItem = value;
		}
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x00084E60 File Offset: 0x00083060
	private void Awake()
	{
		foreach (string text in this.startingItemList)
		{
			this.itemList.Add(text);
		}
		this.index = this.startingIndex;
		this.dropDownItemTemplate.gameObject.SetActive(false);
		this.UpdateList();
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00010650 File Offset: 0x0000E850
	private void OnEnable()
	{
		this.dropDownButton.OnDown += this.ExpandButtonPressed;
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x00010669 File Offset: 0x0000E869
	private void OnDisable()
	{
		this.dropDownButton.OnDown -= this.ExpandButtonPressed;
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x00084EBC File Offset: 0x000830BC
	public void UpdateList()
	{
		if (this.dropDownItems.Count > this.ItemList.Count)
		{
			for (int i = this.ItemList.Count; i < this.dropDownItems.Count; i++)
			{
				this.dropDownItems[i].gameObject.SetActive(false);
			}
		}
		while (this.dropDownItems.Count < this.ItemList.Count)
		{
			this.dropDownItems.Add(this.CreateAnotherDropDownItem());
		}
		for (int j = 0; j < this.ItemList.Count; j++)
		{
			tk2dUIDropDownItem tk2dUIDropDownItem = this.dropDownItems[j];
			Vector3 localPosition = tk2dUIDropDownItem.transform.localPosition;
			if (this.menuLayoutItem != null && this.templateLayoutItem != null)
			{
				localPosition.y = this.menuLayoutItem.bMin.y - (float)j * (this.templateLayoutItem.bMax.y - this.templateLayoutItem.bMin.y);
			}
			else
			{
				localPosition.y = -this.height - (float)j * tk2dUIDropDownItem.height;
			}
			tk2dUIDropDownItem.transform.localPosition = localPosition;
			if (tk2dUIDropDownItem.label != null)
			{
				tk2dUIDropDownItem.LabelText = this.itemList[j];
			}
			tk2dUIDropDownItem.Index = j;
		}
		this.SetSelectedItem();
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x00085040 File Offset: 0x00083240
	public void SetSelectedItem()
	{
		if (this.index < 0 || this.index >= this.ItemList.Count)
		{
			this.index = 0;
		}
		if (this.index >= 0 && this.index < this.ItemList.Count)
		{
			this.selectedTextMesh.text = this.ItemList[this.index];
			this.selectedTextMesh.Commit();
		}
		else
		{
			this.selectedTextMesh.text = string.Empty;
			this.selectedTextMesh.Commit();
		}
		if (this.OnSelectedItemChange != null)
		{
			this.OnSelectedItemChange();
		}
		if (this.SendMessageTarget != null && this.SendMessageOnSelectedItemChangeMethodName.Length > 0)
		{
			this.SendMessageTarget.SendMessage(this.SendMessageOnSelectedItemChangeMethodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x0008512C File Offset: 0x0008332C
	private tk2dUIDropDownItem CreateAnotherDropDownItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.dropDownItemTemplate.gameObject) as GameObject;
		gameObject.name = "DropDownItem";
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = this.dropDownItemTemplate.transform.localPosition;
		gameObject.transform.localRotation = this.dropDownItemTemplate.transform.localRotation;
		gameObject.transform.localScale = this.dropDownItemTemplate.transform.localScale;
		tk2dUIDropDownItem component = gameObject.GetComponent<tk2dUIDropDownItem>();
		component.OnItemSelected += this.ItemSelected;
		tk2dUIUpDownHoverButton component2 = gameObject.GetComponent<tk2dUIUpDownHoverButton>();
		component.upDownHoverBtn = component2;
		component2.OnToggleOver += this.DropDownItemHoverBtnToggle;
		return component;
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x00010682 File Offset: 0x0000E882
	private void ItemSelected(tk2dUIDropDownItem item)
	{
		if (this.isExpanded)
		{
			this.CollapseList();
		}
		this.Index = item.Index;
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x000106A1 File Offset: 0x0000E8A1
	private void ExpandButtonPressed()
	{
		if (this.isExpanded)
		{
			this.CollapseList();
		}
		else
		{
			this.ExpandList();
		}
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x000851F8 File Offset: 0x000833F8
	private void ExpandList()
	{
		this.isExpanded = true;
		int num = Mathf.Min(this.ItemList.Count, this.dropDownItems.Count);
		for (int i = 0; i < num; i++)
		{
			this.dropDownItems[i].gameObject.SetActive(true);
		}
		tk2dUIDropDownItem tk2dUIDropDownItem = this.dropDownItems[this.index];
		if (tk2dUIDropDownItem.upDownHoverBtn != null)
		{
			tk2dUIDropDownItem.upDownHoverBtn.IsOver = true;
		}
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x00085280 File Offset: 0x00083480
	private void CollapseList()
	{
		this.isExpanded = false;
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x000852E8 File Offset: 0x000834E8
	private void DropDownItemHoverBtnToggle(tk2dUIUpDownHoverButton upDownHoverButton)
	{
		if (upDownHoverButton.IsOver)
		{
			foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
			{
				if (tk2dUIDropDownItem.upDownHoverBtn != upDownHoverButton && tk2dUIDropDownItem.upDownHoverBtn != null)
				{
					tk2dUIDropDownItem.upDownHoverBtn.IsOver = false;
				}
			}
		}
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x00085374 File Offset: 0x00083574
	private void OnDestroy()
	{
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.OnItemSelected -= this.ItemSelected;
			if (tk2dUIDropDownItem.upDownHoverBtn != null)
			{
				tk2dUIDropDownItem.upDownHoverBtn.OnToggleOver -= this.DropDownItemHoverBtnToggle;
			}
		}
	}

	// Token: 0x040014E2 RID: 5346
	public tk2dUIItem dropDownButton;

	// Token: 0x040014E3 RID: 5347
	public tk2dTextMesh selectedTextMesh;

	// Token: 0x040014E4 RID: 5348
	[HideInInspector]
	public float height;

	// Token: 0x040014E5 RID: 5349
	public tk2dUIDropDownItem dropDownItemTemplate;

	// Token: 0x040014E6 RID: 5350
	[SerializeField]
	private string[] startingItemList;

	// Token: 0x040014E7 RID: 5351
	[SerializeField]
	private int startingIndex;

	// Token: 0x040014E8 RID: 5352
	private List<string> itemList = new List<string>();

	// Token: 0x040014E9 RID: 5353
	public string SendMessageOnSelectedItemChangeMethodName = string.Empty;

	// Token: 0x040014EA RID: 5354
	private int index;

	// Token: 0x040014EB RID: 5355
	private List<tk2dUIDropDownItem> dropDownItems = new List<tk2dUIDropDownItem>();

	// Token: 0x040014EC RID: 5356
	private bool isExpanded;

	// Token: 0x040014ED RID: 5357
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout menuLayoutItem;

	// Token: 0x040014EE RID: 5358
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout templateLayoutItem;
}
