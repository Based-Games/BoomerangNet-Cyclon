using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000290 RID: 656
	[Serializable]
	public class LayerInfo
	{
		// Token: 0x060012EF RID: 4847 RVA: 0x000100FF File Offset: 0x0000E2FF
		public LayerInfo()
		{
			this.unityLayer = 0;
			this.useColor = true;
			this.generateCollider = true;
			this.skipMeshGeneration = false;
		}

		// Token: 0x040014AB RID: 5291
		public string name;

		// Token: 0x040014AC RID: 5292
		public int hash;

		// Token: 0x040014AD RID: 5293
		public bool useColor;

		// Token: 0x040014AE RID: 5294
		public bool generateCollider;

		// Token: 0x040014AF RID: 5295
		public float z = 0.1f;

		// Token: 0x040014B0 RID: 5296
		public int unityLayer;

		// Token: 0x040014B1 RID: 5297
		public bool skipMeshGeneration;

		// Token: 0x040014B2 RID: 5298
		public PhysicMaterial physicMaterial;
	}
}
