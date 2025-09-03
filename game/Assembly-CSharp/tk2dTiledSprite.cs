using System;
using UnityEngine;

// Token: 0x02000281 RID: 641
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dTiledSprite")]
public class tk2dTiledSprite : tk2dBaseSprite
{
	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06001269 RID: 4713 RVA: 0x0000FB8E File Offset: 0x0000DD8E
	// (set) Token: 0x0600126A RID: 4714 RVA: 0x0000FB96 File Offset: 0x0000DD96
	public Vector2 dimensions
	{
		get
		{
			return this._dimensions;
		}
		set
		{
			if (value != this._dimensions)
			{
				this._dimensions = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x0600126B RID: 4715 RVA: 0x0000FBBC File Offset: 0x0000DDBC
	// (set) Token: 0x0600126C RID: 4716 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
	public tk2dBaseSprite.Anchor anchor
	{
		get
		{
			return this._anchor;
		}
		set
		{
			if (value != this._anchor)
			{
				this._anchor = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x0600126D RID: 4717 RVA: 0x0000FBE5 File Offset: 0x0000DDE5
	// (set) Token: 0x0600126E RID: 4718 RVA: 0x0000FBED File Offset: 0x0000DDED
	public bool CreateBoxCollider
	{
		get
		{
			return this._createBoxCollider;
		}
		set
		{
			if (this._createBoxCollider != value)
			{
				this._createBoxCollider = value;
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x00080A0C File Offset: 0x0007EC0C
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
			if (this.boxCollider == null)
			{
				this.boxCollider = base.GetComponent<BoxCollider>();
			}
		}
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x0000FC08 File Offset: 0x0000DE08
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00080AA4 File Offset: 0x0007ECA4
	protected new void SetColors(Color32[] dest)
	{
		int num;
		int num2;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, base.CurrentSprite, this.dimensions);
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, num, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x00080AE0 File Offset: 0x0007ECE0
	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		int num;
		int num2;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, currentSprite, this.dimensions);
		if (this.meshUvs == null || this.meshUvs.Length != num)
		{
			this.meshUvs = new Vector2[num];
			this.meshVertices = new Vector3[num];
			this.meshColors = new Color32[num];
		}
		if (this.meshIndices == null || this.meshIndices.Length != num2)
		{
			this.meshIndices = new int[num2];
		}
		this.meshNormals = new Vector3[0];
		this.meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			this.meshNormals = new Vector3[num];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			this.meshTangents = new Vector4[num];
		}
		float num3 = ((!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z);
		float num4 = ((!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f));
		tk2dSpriteGeomGen.SetTiledSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this.dimensions, this.anchor, num3, num4);
		tk2dSpriteGeomGen.SetTiledSpriteIndices(this.meshIndices, 0, 0, currentSprite, this.dimensions);
		if (this.meshNormals.Length > 0 || this.meshTangents.Length > 0)
		{
			Vector3 vector = new Vector3(currentSprite.positions[0].x * this.dimensions.x * currentSprite.texelSize.x * base.scale.x, currentSprite.positions[0].y * this.dimensions.y * currentSprite.texelSize.y * base.scale.y);
			Vector3 vector2 = new Vector3(currentSprite.positions[3].x * this.dimensions.x * currentSprite.texelSize.x * base.scale.x, currentSprite.positions[3].y * this.dimensions.y * currentSprite.texelSize.y * base.scale.y);
			tk2dSpriteGeomGen.SetSpriteVertexNormals(this.meshVertices, vector, vector2, currentSprite.normals, currentSprite.tangents, this.meshNormals, this.meshTangents);
		}
		this.SetColors(this.meshColors);
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			this.mesh.Clear();
		}
		this.mesh.vertices = this.meshVertices;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = this.meshUvs;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.triangles = this.meshIndices;
		this.mesh.RecalculateBounds();
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x0000FC25 File Offset: 0x0000DE25
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x0000FC2D File Offset: 0x0000DE2D
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0000FC25 File Offset: 0x0000DE25
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x00080EB0 File Offset: 0x0007F0B0
	protected void UpdateColorsImpl()
	{
		if (this.meshColors == null || this.meshColors.Length == 0)
		{
			this.Build();
		}
		else
		{
			this.SetColors(this.meshColors);
			this.mesh.colors32 = this.meshColors;
		}
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x0000FC35 File Offset: 0x0000DE35
	protected void UpdateGeometryImpl()
	{
		this.Build();
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x00080F00 File Offset: 0x0007F100
	protected override void UpdateCollider()
	{
		if (this.CreateBoxCollider && this.boxCollider != null)
		{
			this.boxCollider.size = 2f * this.boundsExtents;
			this.boxCollider.center = this.boundsCenter;
		}
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x0000FC3D File Offset: 0x0000DE3D
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x000791C0 File Offset: 0x000773C0
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x0000ECFF File Offset: 0x0000CEFF
	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00080F58 File Offset: 0x0007F158
	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 vector = new Vector3(this._dimensions.x * currentSprite.texelSize.x * this._scale.x, this._dimensions.y * currentSprite.texelSize.y * this._scale.y);
		Vector3 vector2 = Vector3.zero;
		switch (this._anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			vector2.Set(0f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			vector2.Set(0.5f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			vector2.Set(1f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			vector2.Set(0f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			vector2.Set(0.5f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector2.Set(1f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			vector2.Set(0f, 1f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector2.Set(0.5f, 1f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.Set(1f, 1f, 0f);
			break;
		}
		vector2 = Vector3.Scale(vector2, vector) * -1f;
		Vector3 vector3 = vector + dMax - dMin;
		vector3.x /= currentSprite.texelSize.x * this._scale.x;
		vector3.y /= currentSprite.texelSize.y * this._scale.y;
		Vector3 vector4 = new Vector3((!Mathf.Approximately(this._dimensions.x, 0f)) ? (vector2.x * vector3.x / this._dimensions.x) : 0f, (!Mathf.Approximately(this._dimensions.y, 0f)) ? (vector2.y * vector3.y / this._dimensions.y) : 0f);
		Vector3 vector5 = vector2 + dMin - vector4;
		vector5.z = 0f;
		base.transform.position = base.transform.TransformPoint(vector5);
		this.dimensions = new Vector2(vector3.x, vector3.y);
	}

	// Token: 0x04001460 RID: 5216
	private Mesh mesh;

	// Token: 0x04001461 RID: 5217
	private Vector2[] meshUvs;

	// Token: 0x04001462 RID: 5218
	private Vector3[] meshVertices;

	// Token: 0x04001463 RID: 5219
	private Color32[] meshColors;

	// Token: 0x04001464 RID: 5220
	private Vector3[] meshNormals;

	// Token: 0x04001465 RID: 5221
	private Vector4[] meshTangents;

	// Token: 0x04001466 RID: 5222
	private int[] meshIndices;

	// Token: 0x04001467 RID: 5223
	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	// Token: 0x04001468 RID: 5224
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchor;

	// Token: 0x04001469 RID: 5225
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x0400146A RID: 5226
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x0400146B RID: 5227
	private Vector3 boundsExtents = Vector3.zero;
}
