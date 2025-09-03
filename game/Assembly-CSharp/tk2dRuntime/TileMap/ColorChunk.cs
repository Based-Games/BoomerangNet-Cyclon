using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200028A RID: 650
	[Serializable]
	public class ColorChunk
	{
		// Token: 0x060012B8 RID: 4792 RVA: 0x0000FF49 File Offset: 0x0000E149
		public ColorChunk()
		{
			this.colors = new Color32[0];
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0000FF5D File Offset: 0x0000E15D
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x0000FF65 File Offset: 0x0000E165
		public bool Dirty { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x0000FF6E File Offset: 0x0000E16E
		public bool Empty
		{
			get
			{
				return this.colors.Length == 0;
			}
		}

		// Token: 0x04001496 RID: 5270
		public Color32[] colors;
	}
}
