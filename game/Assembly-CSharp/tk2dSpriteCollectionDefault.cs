using System;
using UnityEngine;

// Token: 0x02000268 RID: 616
[Serializable]
public class tk2dSpriteCollectionDefault
{
	// Token: 0x04001370 RID: 4976
	public bool additive;

	// Token: 0x04001371 RID: 4977
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04001372 RID: 4978
	public tk2dSpriteCollectionDefinition.Anchor anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;

	// Token: 0x04001373 RID: 4979
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04001374 RID: 4980
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;
}
