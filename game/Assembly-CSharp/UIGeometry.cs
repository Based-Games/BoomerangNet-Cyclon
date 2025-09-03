using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class UIGeometry
{
	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060003CC RID: 972 RVA: 0x00006137 File Offset: 0x00004337
	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060003CD RID: 973 RVA: 0x00006147 File Offset: 0x00004347
	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00006180 File Offset: 0x00004380
	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	// Token: 0x060003CF RID: 975 RVA: 0x000237D8 File Offset: 0x000219D8
	public void ApplyTransform(Matrix4x4 widgetToPanel)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int size = this.verts.size;
			while (i < size)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
			Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
			this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x000238A4 File Offset: 0x00021AA4
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					v.Add(this.mRtpVerts.buffer[j]);
					u.Add(this.uvs.buffer[j]);
					c.Add(this.cols.buffer[j]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
				}
			}
		}
	}

	// Token: 0x040002FE RID: 766
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	// Token: 0x040002FF RID: 767
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	// Token: 0x04000300 RID: 768
	public BetterList<Color32> cols = new BetterList<Color32>();

	// Token: 0x04000301 RID: 769
	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	// Token: 0x04000302 RID: 770
	private Vector3 mRtpNormal;

	// Token: 0x04000303 RID: 771
	private Vector4 mRtpTan;
}
