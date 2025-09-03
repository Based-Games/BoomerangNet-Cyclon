using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001A RID: 26
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Examples/Item Database")]
public class InvDatabase : MonoBehaviour
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003D5D File Offset: 0x00001F5D
	public static InvDatabase[] list
	{
		get
		{
			if (InvDatabase.mIsDirty)
			{
				InvDatabase.mIsDirty = false;
				InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
			}
			return InvDatabase.mList;
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00003D55 File Offset: 0x00001F55
	private void OnEnable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00003D55 File Offset: 0x00001F55
	private void OnDisable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000151B8 File Offset: 0x000133B8
	private InvBaseItem GetItem(int id16)
	{
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			InvBaseItem invBaseItem = this.items[i];
			if (invBaseItem.id16 == id16)
			{
				return invBaseItem;
			}
			i++;
		}
		return null;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00015200 File Offset: 0x00013400
	private static InvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.databaseID == dbID)
			{
				return invDatabase;
			}
			i++;
		}
		return null;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00015240 File Offset: 0x00013440
	public static InvBaseItem FindByID(int id32)
	{
		InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
		return (!(database != null)) ? null : database.GetItem(id32 & 65535);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00015278 File Offset: 0x00013478
	public static InvBaseItem FindByName(string exact)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			int j = 0;
			int count = invDatabase.items.Count;
			while (j < count)
			{
				InvBaseItem invBaseItem = invDatabase.items[j];
				if (invBaseItem.name == exact)
				{
					return invBaseItem;
				}
				j++;
			}
			i++;
		}
		return null;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000152EC File Offset: 0x000134EC
	public static int FindItemID(InvBaseItem item)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.items.Contains(item))
			{
				return (invDatabase.databaseID << 16) | item.id16;
			}
			i++;
		}
		return -1;
	}

	// Token: 0x040000A0 RID: 160
	private static InvDatabase[] mList;

	// Token: 0x040000A1 RID: 161
	private static bool mIsDirty = true;

	// Token: 0x040000A2 RID: 162
	public int databaseID;

	// Token: 0x040000A3 RID: 163
	public List<InvBaseItem> items = new List<InvBaseItem>();

	// Token: 0x040000A4 RID: 164
	public UIAtlas iconAtlas;
}
