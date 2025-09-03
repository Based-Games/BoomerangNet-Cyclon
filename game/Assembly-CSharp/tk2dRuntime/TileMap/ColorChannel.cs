using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200028B RID: 651
	[Serializable]
	public class ColorChannel
	{
		// Token: 0x060012BC RID: 4796 RVA: 0x0000FF7B File Offset: 0x0000E17B
		public ColorChannel(int width, int height, int divX, int divY)
		{
			this.Init(width, height, divX, divY);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0000FF99 File Offset: 0x0000E199
		public ColorChannel()
		{
			this.chunks = new ColorChunk[0];
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		public void Init(int width, int height, int divX, int divY)
		{
			this.numColumns = (width + divX - 1) / divX;
			this.numRows = (height + divY - 1) / divY;
			this.chunks = new ColorChunk[0];
			this.divX = divX;
			this.divY = divY;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00082D30 File Offset: 0x00080F30
		public ColorChunk FindChunkAndCoordinate(int x, int y, out int offset)
		{
			int num = x / this.divX;
			int num2 = y / this.divY;
			num = Mathf.Clamp(num, 0, this.numColumns - 1);
			num2 = Mathf.Clamp(num2, 0, this.numRows - 1);
			int num3 = num2 * this.numColumns + num;
			ColorChunk colorChunk = this.chunks[num3];
			int num4 = x - num * this.divX;
			int num5 = y - num2 * this.divY;
			offset = num5 * (this.divX + 1) + num4;
			return colorChunk;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00082DAC File Offset: 0x00080FAC
		public Color GetColor(int x, int y)
		{
			if (this.IsEmpty)
			{
				return this.clearColor;
			}
			int num;
			ColorChunk colorChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (colorChunk.colors.Length == 0)
			{
				return this.clearColor;
			}
			return colorChunk.colors[num];
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00082E00 File Offset: 0x00081000
		private void InitChunk(ColorChunk chunk)
		{
			if (chunk.colors.Length == 0)
			{
				chunk.colors = new Color32[(this.divX + 1) * (this.divY + 1)];
				for (int i = 0; i < chunk.colors.Length; i++)
				{
					chunk.colors[i] = this.clearColor;
				}
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00082E6C File Offset: 0x0008106C
		public void SetColor(int x, int y, Color color)
		{
			if (this.IsEmpty)
			{
				this.Create();
			}
			int num = this.divX + 1;
			int num2 = Mathf.Max(x - 1, 0) / this.divX;
			int num3 = Mathf.Max(y - 1, 0) / this.divY;
			ColorChunk colorChunk = this.GetChunk(num2, num3, true);
			int num4 = x - num2 * this.divX;
			int num5 = y - num3 * this.divY;
			colorChunk.colors[num5 * num + num4] = color;
			colorChunk.Dirty = true;
			bool flag = false;
			bool flag2 = false;
			if (x != 0 && x % this.divX == 0 && num2 + 1 < this.numColumns)
			{
				flag = true;
			}
			if (y != 0 && y % this.divY == 0 && num3 + 1 < this.numRows)
			{
				flag2 = true;
			}
			if (flag)
			{
				int num6 = num2 + 1;
				colorChunk = this.GetChunk(num6, num3, true);
				num4 = x - num6 * this.divX;
				num5 = y - num3 * this.divY;
				colorChunk.colors[num5 * num + num4] = color;
				colorChunk.Dirty = true;
			}
			if (flag2)
			{
				int num7 = num3 + 1;
				colorChunk = this.GetChunk(num2, num7, true);
				num4 = x - num2 * this.divX;
				num5 = y - num7 * this.divY;
				colorChunk.colors[num5 * num + num4] = color;
				colorChunk.Dirty = true;
			}
			if (flag && flag2)
			{
				int num8 = num2 + 1;
				int num9 = num3 + 1;
				colorChunk = this.GetChunk(num8, num9, true);
				num4 = x - num8 * this.divX;
				num5 = y - num9 * this.divY;
				colorChunk.colors[num5 * num + num4] = color;
				colorChunk.Dirty = true;
			}
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0000FFF1 File Offset: 0x0000E1F1
		public ColorChunk GetChunk(int x, int y)
		{
			if (this.chunks == null || this.chunks.Length == 0)
			{
				return null;
			}
			return this.chunks[y * this.numColumns + x];
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00083054 File Offset: 0x00081254
		public ColorChunk GetChunk(int x, int y, bool init)
		{
			if (this.chunks == null || this.chunks.Length == 0)
			{
				return null;
			}
			ColorChunk colorChunk = this.chunks[y * this.numColumns + x];
			this.InitChunk(colorChunk);
			return colorChunk;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00083098 File Offset: 0x00081298
		public void ClearChunk(ColorChunk chunk)
		{
			for (int i = 0; i < chunk.colors.Length; i++)
			{
				chunk.colors[i] = this.clearColor;
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000830DC File Offset: 0x000812DC
		public void ClearDirtyFlag()
		{
			foreach (ColorChunk colorChunk in this.chunks)
			{
				colorChunk.Dirty = false;
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00083110 File Offset: 0x00081310
		public void Clear(Color color)
		{
			this.clearColor = color;
			foreach (ColorChunk colorChunk in this.chunks)
			{
				this.ClearChunk(colorChunk);
			}
			this.Optimize();
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0001001E File Offset: 0x0000E21E
		public void Delete()
		{
			this.chunks = new ColorChunk[0];
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00083150 File Offset: 0x00081350
		public void Create()
		{
			this.chunks = new ColorChunk[this.numColumns * this.numRows];
			for (int i = 0; i < this.chunks.Length; i++)
			{
				this.chunks[i] = new ColorChunk();
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0008319C File Offset: 0x0008139C
		private void Optimize(ColorChunk chunk)
		{
			bool flag = true;
			Color32 color = this.clearColor;
			foreach (Color32 color2 in chunk.colors)
			{
				if (color2.r != color.r || color2.g != color.g || color2.b != color.b || color2.a != color.a)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				chunk.colors = new Color32[0];
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0008324C File Offset: 0x0008144C
		public void Optimize()
		{
			foreach (ColorChunk colorChunk in this.chunks)
			{
				this.Optimize(colorChunk);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x0001002C File Offset: 0x0000E22C
		public bool IsEmpty
		{
			get
			{
				return this.chunks.Length == 0;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00083280 File Offset: 0x00081480
		public int NumActiveChunks
		{
			get
			{
				int num = 0;
				foreach (ColorChunk colorChunk in this.chunks)
				{
					if (colorChunk != null && colorChunk.colors != null && colorChunk.colors.Length > 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x04001498 RID: 5272
		public Color clearColor = Color.white;

		// Token: 0x04001499 RID: 5273
		public ColorChunk[] chunks;

		// Token: 0x0400149A RID: 5274
		public int numColumns;

		// Token: 0x0400149B RID: 5275
		public int numRows;

		// Token: 0x0400149C RID: 5276
		public int divX;

		// Token: 0x0400149D RID: 5277
		public int divY;
	}
}
