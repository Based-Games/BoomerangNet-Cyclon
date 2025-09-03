using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001B RID: 27
[Serializable]
public class InvGameItem
{
	// Token: 0x060000D9 RID: 217 RVA: 0x00003D7E File Offset: 0x00001F7E
	public InvGameItem(int id)
	{
		this.mBaseItemID = id;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00003D9B File Offset: 0x00001F9B
	public InvGameItem(int id, InvBaseItem bi)
	{
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000DB RID: 219 RVA: 0x00003DBF File Offset: 0x00001FBF
	public int baseItemID
	{
		get
		{
			return this.mBaseItemID;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000DC RID: 220 RVA: 0x00003DC7 File Offset: 0x00001FC7
	public InvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000DD RID: 221 RVA: 0x00003DEB File Offset: 0x00001FEB
	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000DE RID: 222 RVA: 0x00015340 File Offset: 0x00013540
	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				num = 0f;
				break;
			case InvGameItem.Quality.Cursed:
				num = -1f;
				break;
			case InvGameItem.Quality.Damaged:
				num = 0.25f;
				break;
			case InvGameItem.Quality.Worn:
				num = 0.9f;
				break;
			case InvGameItem.Quality.Sturdy:
				num = 1f;
				break;
			case InvGameItem.Quality.Polished:
				num = 1.1f;
				break;
			case InvGameItem.Quality.Improved:
				num = 1.25f;
				break;
			case InvGameItem.Quality.Crafted:
				num = 1.5f;
				break;
			case InvGameItem.Quality.Superior:
				num = 1.75f;
				break;
			case InvGameItem.Quality.Enchanted:
				num = 2f;
				break;
			case InvGameItem.Quality.Epic:
				num = 2.5f;
				break;
			case InvGameItem.Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000DF RID: 223 RVA: 0x0001543C File Offset: 0x0001363C
	public Color color
	{
		get
		{
			Color color = Color.white;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				color = new Color(0.4f, 0.2f, 0.2f);
				break;
			case InvGameItem.Quality.Cursed:
				color = Color.red;
				break;
			case InvGameItem.Quality.Damaged:
				color = new Color(0.4f, 0.4f, 0.4f);
				break;
			case InvGameItem.Quality.Worn:
				color = new Color(0.7f, 0.7f, 0.7f);
				break;
			case InvGameItem.Quality.Sturdy:
				color = new Color(1f, 1f, 1f);
				break;
			case InvGameItem.Quality.Polished:
				color = NGUIMath.HexToColor(3774856959U);
				break;
			case InvGameItem.Quality.Improved:
				color = NGUIMath.HexToColor(2480359935U);
				break;
			case InvGameItem.Quality.Crafted:
				color = NGUIMath.HexToColor(1325334783U);
				break;
			case InvGameItem.Quality.Superior:
				color = NGUIMath.HexToColor(12255231U);
				break;
			case InvGameItem.Quality.Enchanted:
				color = NGUIMath.HexToColor(1937178111U);
				break;
			case InvGameItem.Quality.Epic:
				color = NGUIMath.HexToColor(2516647935U);
				break;
			case InvGameItem.Quality.Legendary:
				color = NGUIMath.HexToColor(4287627519U);
				break;
			}
			return color;
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0001557C File Offset: 0x0001377C
	public List<InvStat> CalculateStats()
	{
		List<InvStat> list = new List<InvStat>();
		if (this.baseItem != null)
		{
			float statMultiplier = this.statMultiplier;
			List<InvStat> stats = this.baseItem.stats;
			int i = 0;
			int count = stats.Count;
			while (i < count)
			{
				InvStat invStat = stats[i];
				int num = Mathf.RoundToInt(statMultiplier * (float)invStat.amount);
				if (num != 0)
				{
					bool flag = false;
					int j = 0;
					int count2 = list.Count;
					while (j < count2)
					{
						InvStat invStat2 = list[j];
						if (invStat2.id == invStat.id && invStat2.modifier == invStat.modifier)
						{
							invStat2.amount += num;
							flag = true;
							break;
						}
						j++;
					}
					if (!flag)
					{
						list.Add(new InvStat
						{
							id = invStat.id,
							amount = num,
							modifier = invStat.modifier
						});
					}
				}
				i++;
			}
			list.Sort(new Comparison<InvStat>(InvStat.CompareArmor));
		}
		return list;
	}

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	private int mBaseItemID;

	// Token: 0x040000A6 RID: 166
	public InvGameItem.Quality quality = InvGameItem.Quality.Sturdy;

	// Token: 0x040000A7 RID: 167
	public int itemLevel = 1;

	// Token: 0x040000A8 RID: 168
	private InvBaseItem mBaseItem;

	// Token: 0x0200001C RID: 28
	public enum Quality
	{
		// Token: 0x040000AA RID: 170
		Broken,
		// Token: 0x040000AB RID: 171
		Cursed,
		// Token: 0x040000AC RID: 172
		Damaged,
		// Token: 0x040000AD RID: 173
		Worn,
		// Token: 0x040000AE RID: 174
		Sturdy,
		// Token: 0x040000AF RID: 175
		Polished,
		// Token: 0x040000B0 RID: 176
		Improved,
		// Token: 0x040000B1 RID: 177
		Crafted,
		// Token: 0x040000B2 RID: 178
		Superior,
		// Token: 0x040000B3 RID: 179
		Enchanted,
		// Token: 0x040000B4 RID: 180
		Epic,
		// Token: 0x040000B5 RID: 181
		Legendary,
		// Token: 0x040000B6 RID: 182
		_LastDoNotUse
	}
}
