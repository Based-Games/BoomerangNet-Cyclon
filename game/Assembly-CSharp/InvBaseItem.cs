using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000018 RID: 24
[Serializable]
public class InvBaseItem
{
	// Token: 0x0400008B RID: 139
	public int id16;

	// Token: 0x0400008C RID: 140
	public string name;

	// Token: 0x0400008D RID: 141
	public string description;

	// Token: 0x0400008E RID: 142
	public InvBaseItem.Slot slot;

	// Token: 0x0400008F RID: 143
	public int minItemLevel = 1;

	// Token: 0x04000090 RID: 144
	public int maxItemLevel = 50;

	// Token: 0x04000091 RID: 145
	public List<InvStat> stats = new List<InvStat>();

	// Token: 0x04000092 RID: 146
	public GameObject attachment;

	// Token: 0x04000093 RID: 147
	public Color color = Color.white;

	// Token: 0x04000094 RID: 148
	public UIAtlas iconAtlas;

	// Token: 0x04000095 RID: 149
	public string iconName = string.Empty;

	// Token: 0x02000019 RID: 25
	public enum Slot
	{
		// Token: 0x04000097 RID: 151
		None,
		// Token: 0x04000098 RID: 152
		Weapon,
		// Token: 0x04000099 RID: 153
		Shield,
		// Token: 0x0400009A RID: 154
		Body,
		// Token: 0x0400009B RID: 155
		Shoulders,
		// Token: 0x0400009C RID: 156
		Bracers,
		// Token: 0x0400009D RID: 157
		Boots,
		// Token: 0x0400009E RID: 158
		Trinket,
		// Token: 0x0400009F RID: 159
		_LastDoNotUse
	}
}
