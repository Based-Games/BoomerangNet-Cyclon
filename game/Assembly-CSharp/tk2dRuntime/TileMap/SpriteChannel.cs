using System;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000289 RID: 649
	[Serializable]
	public class SpriteChannel
	{
		// Token: 0x060012B7 RID: 4791 RVA: 0x0000FF35 File Offset: 0x0000E135
		public SpriteChannel()
		{
			this.chunks = new SpriteChunk[0];
		}

		// Token: 0x04001495 RID: 5269
		public SpriteChunk[] chunks;
	}
}
