using System;
using UnityEngine;

// Token: 0x02000253 RID: 595
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/Sprite/tk2dClippedSprite")]
public class tk2dClippedSprite : tk2dBaseSprite
{
	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001142 RID: 4418 RVA: 0x00078C2C File Offset: 0x00076E2C
	// (set) Token: 0x06001143 RID: 4419 RVA: 0x00078C90 File Offset: 0x00076E90
	public Rect ClipRect
	{
		get
		{
			this._clipRect.Set(this._clipBottomLeft.x, this._clipBottomLeft.y, this._clipTopRight.x - this._clipBottomLeft.x, this._clipTopRight.y - this._clipBottomLeft.y);
			return this._clipRect;
		}
		set
		{
			Vector2 vector = new Vector2(value.x, value.y);
			this.clipBottomLeft = vector;
			vector.x += value.width;
			vector.y += value.height;
			this.clipTopRight = vector;
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06001144 RID: 4420 RVA: 0x0000EA42 File Offset: 0x0000CC42
	// (set) Token: 0x06001145 RID: 4421 RVA: 0x0000EA4A File Offset: 0x0000CC4A
	public Vector2 clipBottomLeft
	{
		get
		{
			return this._clipBottomLeft;
		}
		set
		{
			if (value != this._clipBottomLeft)
			{
				this._clipBottomLeft = new Vector2(value.x, value.y);
				this.Build();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001146 RID: 4422 RVA: 0x0000EA82 File Offset: 0x0000CC82
	// (set) Token: 0x06001147 RID: 4423 RVA: 0x0000EA8A File Offset: 0x0000CC8A
	public Vector2 clipTopRight
	{
		get
		{
			return this._clipTopRight;
		}
		set
		{
			if (value != this._clipTopRight)
			{
				this._clipTopRight = new Vector2(value.x, value.y);
				this.Build();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06001148 RID: 4424 RVA: 0x0000EAC2 File Offset: 0x0000CCC2
	// (set) Token: 0x06001149 RID: 4425 RVA: 0x0000EACA File Offset: 0x0000CCCA
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

	// Token: 0x0600114A RID: 4426 RVA: 0x00078CEC File Offset: 0x00076EEC
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

	// Token: 0x0600114B RID: 4427 RVA: 0x0000EAE5 File Offset: 0x0000CCE5
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x0000EB02 File Offset: 0x0000CD02
	protected new void SetColors(Color32[] dest)
	{
		if (base.CurrentSprite.positions.Length == 4)
		{
			tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 4, this._color, this.collectionInst.premultipliedAlpha);
		}
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x00078D68 File Offset: 0x00076F68
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float num = ((!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z);
		float num2 = ((!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f));
		tk2dSpriteGeomGen.SetClippedSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this._clipBottomLeft, this._clipTopRight, num, num2);
		if (this.meshNormals.Length > 0 || this.meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(this.meshVertices, this.meshVertices[0], this.meshVertices[3], currentSprite.normals, currentSprite.tangents, this.meshNormals, this.meshTangents);
		}
		if (currentSprite.positions.Length != 4 || currentSprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x00078EB4 File Offset: 0x000770B4
	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		this.meshUvs = new Vector2[4];
		this.meshVertices = new Vector3[4];
		this.meshColors = new Color32[4];
		this.meshNormals = new Vector3[0];
		this.meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			this.meshNormals = new Vector3[4];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			this.meshTangents = new Vector4[4];
		}
		this.SetGeometry(this.meshVertices, this.meshUvs);
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
		int[] array = new int[6];
		tk2dSpriteGeomGen.SetClippedSpriteIndices(array, 0, 0, base.CurrentSprite);
		this.mesh.triangles = array;
		this.mesh.RecalculateBounds();
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x0000EB30 File Offset: 0x0000CD30
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x0000EB38 File Offset: 0x0000CD38
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x0000EB30 File Offset: 0x0000CD30
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x00079064 File Offset: 0x00077264
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

	// Token: 0x06001153 RID: 4435 RVA: 0x000790B4 File Offset: 0x000772B4
	protected void UpdateGeometryImpl()
	{
		if (this.meshVertices == null || this.meshVertices.Length == 0)
		{
			this.Build();
		}
		else
		{
			this.SetGeometry(this.meshVertices, this.meshUvs);
			this.mesh.vertices = this.meshVertices;
			this.mesh.uv = this.meshUvs;
			this.mesh.normals = this.meshNormals;
			this.mesh.tangents = this.meshTangents;
			this.mesh.RecalculateBounds();
			this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		}
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x00079168 File Offset: 0x00077368
	protected override void UpdateCollider()
	{
		if (this.CreateBoxCollider && this.boxCollider != null)
		{
			this.boxCollider.size = 2f * this.boundsExtents;
			this.boxCollider.center = this.boundsCenter;
		}
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x0000EB40 File Offset: 0x0000CD40
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x000791C0 File Offset: 0x000773C0
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x0000EB48 File Offset: 0x0000CD48
	protected override int GetCurrentVertexCount()
	{
		return 4;
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x0007921C File Offset: 0x0007741C
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

	// Token: 0x040012CA RID: 4810
	private Mesh mesh;

	// Token: 0x040012CB RID: 4811
	private Vector2[] meshUvs;

	// Token: 0x040012CC RID: 4812
	private Vector3[] meshVertices;

	// Token: 0x040012CD RID: 4813
	private Color32[] meshColors;

	// Token: 0x040012CE RID: 4814
	private Vector3[] meshNormals;

	// Token: 0x040012CF RID: 4815
	private Vector4[] meshTangents;

	// Token: 0x040012D0 RID: 4816
	private int[] meshIndices;

	// Token: 0x040012D1 RID: 4817
	public Vector2 _clipBottomLeft = new Vector2(0f, 0f);

	// Token: 0x040012D2 RID: 4818
	public Vector2 _clipTopRight = new Vector2(1f, 1f);

	// Token: 0x040012D3 RID: 4819
	private Rect _clipRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040012D4 RID: 4820
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x040012D5 RID: 4821
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x040012D6 RID: 4822
	private Vector3 boundsExtents = Vector3.zero;
}
