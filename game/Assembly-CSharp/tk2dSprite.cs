using System;
using UnityEngine;

// Token: 0x02000257 RID: 599
[AddComponentMenu("2D Toolkit/Sprite/tk2dSprite")]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class tk2dSprite : tk2dBaseSprite
{
	// Token: 0x06001184 RID: 4484 RVA: 0x0007ABB4 File Offset: 0x00078DB4
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.hideFlags = HideFlags.DontSave;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
		}
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x0000ED0B File Offset: 0x0000CF0B
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
		if (this.meshColliderMesh)
		{
			UnityEngine.Object.Destroy(this.meshColliderMesh);
		}
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x0007AC30 File Offset: 0x00078E30
	public override void Build()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		this.meshVertices = new Vector3[tk2dSpriteDefinition.positions.Length];
		this.meshColors = new Color32[tk2dSpriteDefinition.positions.Length];
		this.meshNormals = new Vector3[0];
		this.meshTangents = new Vector4[0];
		if (tk2dSpriteDefinition.normals != null && tk2dSpriteDefinition.normals.Length > 0)
		{
			this.meshNormals = new Vector3[tk2dSpriteDefinition.normals.Length];
		}
		if (tk2dSpriteDefinition.tangents != null && tk2dSpriteDefinition.tangents.Length > 0)
		{
			this.meshTangents = new Vector4[tk2dSpriteDefinition.tangents.Length];
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		base.SetColors(this.meshColors);
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
			base.GetComponent<MeshFilter>().mesh = this.mesh;
		}
		this.mesh.Clear();
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		this.mesh.triangles = tk2dSpriteDefinition.indices;
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(base.GetBounds(), this.renderLayer);
		this.UpdateMaterial();
		this.CreateCollider();
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x0000ED43 File Offset: 0x0000CF43
	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteId);
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x0000ED4D File Offset: 0x0000CF4D
	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteName);
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x0000ED57 File Offset: 0x0000CF57
	public static GameObject CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
	{
		return tk2dBaseSprite.CreateFromTexture<tk2dSprite>(texture, size, region, anchor);
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x0000ED62 File Offset: 0x0000CF62
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x0000ED6A File Offset: 0x0000CF6A
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x0000ED72 File Offset: 0x0000CF72
	protected override void UpdateVertices()
	{
		this.UpdateVerticesImpl();
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x0007ADDC File Offset: 0x00078FDC
	protected void UpdateColorsImpl()
	{
		if (this.mesh == null || this.meshColors == null || this.meshColors.Length == 0)
		{
			return;
		}
		base.SetColors(this.meshColors);
		this.mesh.colors32 = this.meshColors;
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x0007AE30 File Offset: 0x00079030
	protected void UpdateVerticesImpl()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		if (this.mesh == null || this.meshVertices == null || this.meshVertices.Length == 0)
		{
			return;
		}
		if (tk2dSpriteDefinition.normals.Length != this.meshNormals.Length)
		{
			this.meshNormals = ((tk2dSpriteDefinition.normals == null || tk2dSpriteDefinition.normals.Length <= 0) ? new Vector3[0] : new Vector3[tk2dSpriteDefinition.normals.Length]);
		}
		if (tk2dSpriteDefinition.tangents.Length != this.meshTangents.Length)
		{
			this.meshTangents = ((tk2dSpriteDefinition.tangents == null || tk2dSpriteDefinition.tangents.Length <= 0) ? new Vector4[0] : new Vector4[tk2dSpriteDefinition.tangents.Length]);
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(base.GetBounds(), this.renderLayer);
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x0007AF8C File Offset: 0x0007918C
	protected void UpdateGeometryImpl()
	{
		if (this.mesh == null)
		{
			return;
		}
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		if (this.meshVertices == null || this.meshVertices.Length != tk2dSpriteDefinition.positions.Length)
		{
			this.meshVertices = new Vector3[tk2dSpriteDefinition.positions.Length];
			this.meshNormals = ((tk2dSpriteDefinition.normals == null || tk2dSpriteDefinition.normals.Length <= 0) ? new Vector3[0] : new Vector3[tk2dSpriteDefinition.normals.Length]);
			this.meshTangents = ((tk2dSpriteDefinition.tangents == null || tk2dSpriteDefinition.tangents.Length <= 0) ? new Vector4[0] : new Vector4[tk2dSpriteDefinition.tangents.Length]);
			this.meshColors = new Color32[tk2dSpriteDefinition.positions.Length];
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		base.SetColors(this.meshColors);
		this.mesh.Clear();
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(base.GetBounds(), this.renderLayer);
		this.mesh.triangles = tk2dSpriteDefinition.indices;
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x000791C0 File Offset: 0x000773C0
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x0000ED7A File Offset: 0x0000CF7A
	protected override int GetCurrentVertexCount()
	{
		if (this.meshVertices == null)
		{
			return 0;
		}
		return this.meshVertices.Length;
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x0000ED91 File Offset: 0x0000CF91
	public override void ForceBuild()
	{
		base.ForceBuild();
		base.GetComponent<MeshFilter>().mesh = this.mesh;
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x0007921C File Offset: 0x0007741C
	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 vector = Vector3.Scale(currentSprite.untrimmedBoundsData[0] - 0.5f * currentSprite.untrimmedBoundsData[1], this._scale);
		Vector3 vector2 = Vector3.Scale(currentSprite.untrimmedBoundsData[1], this._scale);
		Vector3 vector3 = vector2 + dMax - dMin;
		vector3.x /= currentSprite.untrimmedBoundsData[1].x;
		vector3.y /= currentSprite.untrimmedBoundsData[1].y;
		Vector3 vector4 = new Vector3((!Mathf.Approximately(this._scale.x, 0f)) ? (vector.x * vector3.x / this._scale.x) : 0f, (!Mathf.Approximately(this._scale.y, 0f)) ? (vector.y * vector3.y / this._scale.y) : 0f);
		Vector3 vector5 = vector + dMin - vector4;
		vector5.z = 0f;
		base.transform.position = base.transform.TransformPoint(vector5);
		base.scale = new Vector3(vector3.x, vector3.y, this._scale.z);
	}

	// Token: 0x040012F0 RID: 4848
	private Mesh mesh;

	// Token: 0x040012F1 RID: 4849
	private Vector3[] meshVertices;

	// Token: 0x040012F2 RID: 4850
	private Vector3[] meshNormals;

	// Token: 0x040012F3 RID: 4851
	private Vector4[] meshTangents;

	// Token: 0x040012F4 RID: 4852
	private Color32[] meshColors;
}
