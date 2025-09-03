using System;
using UnityEngine;

// Token: 0x02000272 RID: 626
[Serializable]
public class tk2dSpriteDefinition
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x0000F607 File Offset: 0x0000D807
	public bool Valid
	{
		get
		{
			return this.name.Length != 0;
		}
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x0007CBDC File Offset: 0x0007ADDC
	public Bounds GetBounds()
	{
		return new Bounds(new Vector3(this.boundsData[0].x, this.boundsData[0].y, this.boundsData[0].z), new Vector3(this.boundsData[1].x, this.boundsData[1].y, this.boundsData[1].z));
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x0007CC60 File Offset: 0x0007AE60
	public Bounds GetUntrimmedBounds()
	{
		return new Bounds(new Vector3(this.untrimmedBoundsData[0].x, this.untrimmedBoundsData[0].y, this.untrimmedBoundsData[0].z), new Vector3(this.untrimmedBoundsData[1].x, this.untrimmedBoundsData[1].y, this.untrimmedBoundsData[1].z));
	}

	// Token: 0x040013DE RID: 5086
	public string name;

	// Token: 0x040013DF RID: 5087
	public Vector3[] boundsData;

	// Token: 0x040013E0 RID: 5088
	public Vector3[] untrimmedBoundsData;

	// Token: 0x040013E1 RID: 5089
	public Vector2 texelSize;

	// Token: 0x040013E2 RID: 5090
	public Vector3[] positions;

	// Token: 0x040013E3 RID: 5091
	public Vector3[] normals;

	// Token: 0x040013E4 RID: 5092
	public Vector4[] tangents;

	// Token: 0x040013E5 RID: 5093
	public Vector2[] uvs;

	// Token: 0x040013E6 RID: 5094
	public int[] indices = new int[] { 0, 3, 1, 2, 3, 0 };

	// Token: 0x040013E7 RID: 5095
	public Material material;

	// Token: 0x040013E8 RID: 5096
	[NonSerialized]
	public Material materialInst;

	// Token: 0x040013E9 RID: 5097
	public int materialId;

	// Token: 0x040013EA RID: 5098
	public string sourceTextureGUID;

	// Token: 0x040013EB RID: 5099
	public bool extractRegion;

	// Token: 0x040013EC RID: 5100
	public int regionX;

	// Token: 0x040013ED RID: 5101
	public int regionY;

	// Token: 0x040013EE RID: 5102
	public int regionW;

	// Token: 0x040013EF RID: 5103
	public int regionH;

	// Token: 0x040013F0 RID: 5104
	public tk2dSpriteDefinition.FlipMode flipped;

	// Token: 0x040013F1 RID: 5105
	public bool complexGeometry;

	// Token: 0x040013F2 RID: 5106
	public tk2dSpriteDefinition.ColliderType colliderType;

	// Token: 0x040013F3 RID: 5107
	public Vector3[] colliderVertices;

	// Token: 0x040013F4 RID: 5108
	public int[] colliderIndicesFwd;

	// Token: 0x040013F5 RID: 5109
	public int[] colliderIndicesBack;

	// Token: 0x040013F6 RID: 5110
	public bool colliderConvex;

	// Token: 0x040013F7 RID: 5111
	public bool colliderSmoothSphereCollisions;

	// Token: 0x040013F8 RID: 5112
	public tk2dSpriteDefinition.AttachPoint[] attachPoints = new tk2dSpriteDefinition.AttachPoint[0];

	// Token: 0x02000273 RID: 627
	public enum ColliderType
	{
		// Token: 0x040013FA RID: 5114
		Unset,
		// Token: 0x040013FB RID: 5115
		None,
		// Token: 0x040013FC RID: 5116
		Box,
		// Token: 0x040013FD RID: 5117
		Mesh
	}

	// Token: 0x02000274 RID: 628
	public enum FlipMode
	{
		// Token: 0x040013FF RID: 5119
		None,
		// Token: 0x04001400 RID: 5120
		Tk2d,
		// Token: 0x04001401 RID: 5121
		TPackerCW
	}

	// Token: 0x02000275 RID: 629
	[Serializable]
	public class AttachPoint
	{
		// Token: 0x06001202 RID: 4610 RVA: 0x0000F638 File Offset: 0x0000D838
		public void CopyFrom(tk2dSpriteDefinition.AttachPoint src)
		{
			this.name = src.name;
			this.position = src.position;
			this.angle = src.angle;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0000F65E File Offset: 0x0000D85E
		public bool CompareTo(tk2dSpriteDefinition.AttachPoint src)
		{
			return this.name == src.name && src.position == this.position && src.angle == this.angle;
		}

		// Token: 0x04001402 RID: 5122
		public string name = string.Empty;

		// Token: 0x04001403 RID: 5123
		public Vector3 position = Vector3.zero;

		// Token: 0x04001404 RID: 5124
		public float angle;
	}
}
