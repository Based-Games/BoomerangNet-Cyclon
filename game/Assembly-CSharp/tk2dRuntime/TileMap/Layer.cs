using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200028C RID: 652
	[Serializable]
	public class Layer
	{
		// Token: 0x060012CE RID: 4814 RVA: 0x00010039 File Offset: 0x0000E239
		public Layer(int hash, int width, int height, int divX, int divY)
		{
			this.spriteChannel = new SpriteChannel();
			this.Init(hash, width, height, divX, divY);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000832D4 File Offset: 0x000814D4
		public void Init(int hash, int width, int height, int divX, int divY)
		{
			this.divX = divX;
			this.divY = divY;
			this.hash = hash;
			this.numColumns = (width + divX - 1) / divX;
			this.numRows = (height + divY - 1) / divY;
			this.width = width;
			this.height = height;
			this.spriteChannel.chunks = new SpriteChunk[this.numColumns * this.numRows];
			for (int i = 0; i < this.numColumns * this.numRows; i++)
			{
				this.spriteChannel.chunks[i] = new SpriteChunk();
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x00010059 File Offset: 0x0000E259
		public bool IsEmpty
		{
			get
			{
				return this.spriteChannel.chunks.Length == 0;
			}
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0001006B File Offset: 0x0000E26B
		public void Create()
		{
			this.spriteChannel.chunks = new SpriteChunk[this.numColumns * this.numRows];
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0001008A File Offset: 0x0000E28A
		public int[] GetChunkData(int x, int y)
		{
			return this.GetChunk(x, y).spriteIds;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00010099 File Offset: 0x0000E299
		public SpriteChunk GetChunk(int x, int y)
		{
			return this.spriteChannel.chunks[y * this.numColumns + x];
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00083374 File Offset: 0x00081574
		private SpriteChunk FindChunkAndCoordinate(int x, int y, out int offset)
		{
			int num = x / this.divX;
			int num2 = y / this.divY;
			SpriteChunk spriteChunk = this.spriteChannel.chunks[num2 * this.numColumns + num];
			int num3 = x - num * this.divX;
			int num4 = y - num2 * this.divY;
			offset = num4 * this.divX + num3;
			return spriteChunk;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x000833D0 File Offset: 0x000815D0
		private bool GetRawTileValue(int x, int y, ref int value)
		{
			int num;
			SpriteChunk spriteChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (spriteChunk.spriteIds == null || spriteChunk.spriteIds.Length == 0)
			{
				return false;
			}
			value = spriteChunk.spriteIds[num];
			return true;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00083410 File Offset: 0x00081610
		private void SetRawTileValue(int x, int y, int value)
		{
			int num;
			SpriteChunk spriteChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (spriteChunk != null)
			{
				this.CreateChunk(spriteChunk);
				spriteChunk.spriteIds[num] = value;
				spriteChunk.Dirty = true;
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00083448 File Offset: 0x00081648
		public int GetTile(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num) && num != -1)
			{
				return num & 16777215;
			}
			return -1;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00083478 File Offset: 0x00081678
		public tk2dTileFlags GetTileFlags(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num) && num != -1)
			{
				return (tk2dTileFlags)(num & -16777216);
			}
			return tk2dTileFlags.None;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000834A8 File Offset: 0x000816A8
		public int GetRawTile(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num))
			{
				return num;
			}
			return -1;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x000834CC File Offset: 0x000816CC
		public void SetTile(int x, int y, int tile)
		{
			tk2dTileFlags tileFlags = this.GetTileFlags(x, y);
			int num = ((tile != -1) ? (tile | (int)tileFlags) : (-1));
			this.SetRawTileValue(x, y, num);
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x000834FC File Offset: 0x000816FC
		public void SetTileFlags(int x, int y, tk2dTileFlags flags)
		{
			int tile = this.GetTile(x, y);
			if (tile != -1)
			{
				int num = tile | (int)flags;
				this.SetRawTileValue(x, y, num);
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x000100B1 File Offset: 0x0000E2B1
		public void ClearTile(int x, int y)
		{
			this.SetTile(x, y, -1);
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000100BC File Offset: 0x0000E2BC
		public void SetRawTile(int x, int y, int rawTile)
		{
			this.SetRawTileValue(x, y, rawTile);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00083528 File Offset: 0x00081728
		private void CreateChunk(SpriteChunk chunk)
		{
			if (chunk.spriteIds == null || chunk.spriteIds.Length == 0)
			{
				chunk.spriteIds = new int[this.divX * this.divY];
				for (int i = 0; i < this.divX * this.divY; i++)
				{
					chunk.spriteIds[i] = -1;
				}
			}
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0008358C File Offset: 0x0008178C
		private void Optimize(SpriteChunk chunk)
		{
			bool flag = true;
			foreach (int num in chunk.spriteIds)
			{
				if (num != -1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				chunk.spriteIds = new int[0];
			}
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x000835DC File Offset: 0x000817DC
		public void Optimize()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				this.Optimize(spriteChunk);
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00083614 File Offset: 0x00081814
		public void OptimizeIncremental()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				if (spriteChunk.Dirty)
				{
					this.Optimize(spriteChunk);
				}
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00083658 File Offset: 0x00081858
		public void ClearDirtyFlag()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				spriteChunk.Dirty = false;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00083690 File Offset: 0x00081890
		public int NumActiveChunks
		{
			get
			{
				int num = 0;
				foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
				{
					if (!spriteChunk.IsEmpty)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0400149E RID: 5278
		private const int tileMask = 16777215;

		// Token: 0x0400149F RID: 5279
		private const int flagMask = -16777216;

		// Token: 0x040014A0 RID: 5280
		public int hash;

		// Token: 0x040014A1 RID: 5281
		public SpriteChannel spriteChannel;

		// Token: 0x040014A2 RID: 5282
		public int width;

		// Token: 0x040014A3 RID: 5283
		public int height;

		// Token: 0x040014A4 RID: 5284
		public int numColumns;

		// Token: 0x040014A5 RID: 5285
		public int numRows;

		// Token: 0x040014A6 RID: 5286
		public int divX;

		// Token: 0x040014A7 RID: 5287
		public int divY;

		// Token: 0x040014A8 RID: 5288
		public GameObject gameObject;
	}
}
