using System;
using UnityEngine;

// Token: 0x0200025F RID: 607
[Serializable]
public class tk2dSpriteColliderIsland
{
	// Token: 0x060011E2 RID: 4578 RVA: 0x0000F43A File Offset: 0x0000D63A
	public bool IsValid()
	{
		if (this.connected)
		{
			return this.points.Length >= 3;
		}
		return this.points.Length >= 2;
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x0007BDB0 File Offset: 0x00079FB0
	public void CopyFrom(tk2dSpriteColliderIsland src)
	{
		this.connected = src.connected;
		this.points = new Vector2[src.points.Length];
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i] = src.points[i];
		}
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x0007BE18 File Offset: 0x0007A018
	public bool CompareTo(tk2dSpriteColliderIsland src)
	{
		if (this.connected != src.connected)
		{
			return false;
		}
		if (this.points.Length != src.points.Length)
		{
			return false;
		}
		for (int i = 0; i < this.points.Length; i++)
		{
			if (this.points[i] != src.points[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400131D RID: 4893
	public bool connected = true;

	// Token: 0x0400131E RID: 4894
	public Vector2[] points;
}
