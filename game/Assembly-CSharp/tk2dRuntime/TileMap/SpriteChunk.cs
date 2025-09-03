using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000288 RID: 648
	[Serializable]
	public class SpriteChunk
	{
		// Token: 0x060012B0 RID: 4784 RVA: 0x0000FF03 File Offset: 0x0000E103
		public SpriteChunk()
		{
			this.spriteIds = new int[0];
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060012B1 RID: 4785 RVA: 0x0000FF17 File Offset: 0x0000E117
		// (set) Token: 0x060012B2 RID: 4786 RVA: 0x0000FF1F File Offset: 0x0000E11F
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = value;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0000FF28 File Offset: 0x0000E128
		public bool IsEmpty
		{
			get
			{
				return this.spriteIds.Length == 0;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x00082BDC File Offset: 0x00080DDC
		public bool HasGameData
		{
			get
			{
				return this.gameObject != null || this.mesh != null || this.meshCollider != null || this.colliderMesh != null;
			}
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00082C2C File Offset: 0x00080E2C
		public void DestroyGameData(tk2dTileMap tileMap)
		{
			if (this.mesh != null)
			{
				tileMap.DestroyMesh(this.mesh);
			}
			if (this.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(this.gameObject);
			}
			this.gameObject = null;
			this.mesh = null;
			this.DestroyColliderData(tileMap);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00082C88 File Offset: 0x00080E88
		public void DestroyColliderData(tk2dTileMap tileMap)
		{
			if (this.colliderMesh != null)
			{
				tileMap.DestroyMesh(this.colliderMesh);
			}
			if (this.meshCollider != null && this.meshCollider.sharedMesh != null && this.meshCollider.sharedMesh != this.colliderMesh)
			{
				tileMap.DestroyMesh(this.meshCollider.sharedMesh);
			}
			if (this.meshCollider != null)
			{
				UnityEngine.Object.DestroyImmediate(this.meshCollider);
			}
			this.meshCollider = null;
			this.colliderMesh = null;
		}

		// Token: 0x0400148F RID: 5263
		private bool dirty;

		// Token: 0x04001490 RID: 5264
		public int[] spriteIds;

		// Token: 0x04001491 RID: 5265
		public GameObject gameObject;

		// Token: 0x04001492 RID: 5266
		public Mesh mesh;

		// Token: 0x04001493 RID: 5267
		public MeshCollider meshCollider;

		// Token: 0x04001494 RID: 5268
		public Mesh colliderMesh;
	}
}
