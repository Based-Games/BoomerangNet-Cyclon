using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[AddComponentMenu("AVPro Windows Media/Mesh Apply")]
public class AVProWindowsMediaMeshApply : MonoBehaviour
{
	// Token: 0x0600002E RID: 46 RVA: 0x000036FA File Offset: 0x000018FA
	private void Start()
	{
		if (this._movie != null && this._movie.OutputTexture != null)
		{
			this.ApplyMapping(this._movie.OutputTexture);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000036FA File Offset: 0x000018FA
	private void Update()
	{
		if (this._movie != null && this._movie.OutputTexture != null)
		{
			this.ApplyMapping(this._movie.OutputTexture);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00013A88 File Offset: 0x00011C88
	private void ApplyMapping(Texture texture)
	{
		if (this._mesh != null)
		{
			foreach (Material material in this._mesh.materials)
			{
				material.mainTexture = texture;
			}
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003734 File Offset: 0x00001934
	public void OnDisable()
	{
		this.ApplyMapping(null);
	}

	// Token: 0x04000035 RID: 53
	public MeshRenderer _mesh;

	// Token: 0x04000036 RID: 54
	public AVProWindowsMediaMovie _movie;
}
