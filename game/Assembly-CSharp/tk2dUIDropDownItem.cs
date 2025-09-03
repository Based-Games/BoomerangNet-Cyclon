using System;
using UnityEngine;

// Token: 0x0200029B RID: 667
[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownItem")]
public class tk2dUIDropDownItem : tk2dUIBaseItemControl
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001315 RID: 4885 RVA: 0x00010434 File Offset: 0x0000E634
	// (remove) Token: 0x06001316 RID: 4886 RVA: 0x0001044D File Offset: 0x0000E64D
	public event Action<tk2dUIDropDownItem> OnItemSelected;

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06001317 RID: 4887 RVA: 0x00010466 File Offset: 0x0000E666
	// (set) Token: 0x06001318 RID: 4888 RVA: 0x0001046E File Offset: 0x0000E66E
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06001319 RID: 4889 RVA: 0x00010477 File Offset: 0x0000E677
	// (set) Token: 0x0600131A RID: 4890 RVA: 0x00010484 File Offset: 0x0000E684
	public string LabelText
	{
		get
		{
			return this.label.text;
		}
		set
		{
			this.label.text = value;
			this.label.Commit();
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x0001049D File Offset: 0x0000E69D
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ItemSelected;
		}
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x000104C6 File Offset: 0x0000E6C6
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ItemSelected;
		}
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x000104EF File Offset: 0x0000E6EF
	private void ItemSelected()
	{
		if (this.OnItemSelected != null)
		{
			this.OnItemSelected(this);
		}
	}

	// Token: 0x040014DD RID: 5341
	public tk2dTextMesh label;

	// Token: 0x040014DE RID: 5342
	public float height;

	// Token: 0x040014DF RID: 5343
	public tk2dUIUpDownHoverButton upDownHoverBtn;

	// Token: 0x040014E0 RID: 5344
	private int index;
}
