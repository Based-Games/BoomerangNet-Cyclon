using System;
using UnityEngine;

// Token: 0x02000256 RID: 598
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSlicedSprite")]
public class tk2dSlicedSprite : tk2dBaseSprite
{
	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06001167 RID: 4455 RVA: 0x0000EBE7 File Offset: 0x0000CDE7
	// (set) Token: 0x06001168 RID: 4456 RVA: 0x0000EBEF File Offset: 0x0000CDEF
	public bool BorderOnly
	{
		get
		{
			return this._borderOnly;
		}
		set
		{
			if (value != this._borderOnly)
			{
				this._borderOnly = value;
				this.UpdateIndices();
			}
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06001169 RID: 4457 RVA: 0x0000EC0A File Offset: 0x0000CE0A
	// (set) Token: 0x0600116A RID: 4458 RVA: 0x0000EC12 File Offset: 0x0000CE12
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

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x0600116B RID: 4459 RVA: 0x0000EC38 File Offset: 0x0000CE38
	// (set) Token: 0x0600116C RID: 4460 RVA: 0x0000EC40 File Offset: 0x0000CE40
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

	// Token: 0x0600116D RID: 4461 RVA: 0x0007A01C File Offset: 0x0007821C
	public void SetBorder(float left, float bottom, float right, float top)
	{
		if (this.borderLeft != left || this.borderBottom != bottom || this.borderRight != right || this.borderTop != top)
		{
			this.borderLeft = left;
			this.borderBottom = bottom;
			this.borderRight = right;
			this.borderTop = top;
			this.UpdateVertices();
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x0600116E RID: 4462 RVA: 0x0000EC61 File Offset: 0x0000CE61
	// (set) Token: 0x0600116F RID: 4463 RVA: 0x0000EC69 File Offset: 0x0000CE69
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

	// Token: 0x06001170 RID: 4464 RVA: 0x0007A080 File Offset: 0x00078280
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.hideFlags = HideFlags.DontSave;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		if (this.boxCollider == null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
		}
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
		}
	}

	// Token: 0x06001171 RID: 4465 RVA: 0x0000EC84 File Offset: 0x0000CE84
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x06001172 RID: 4466 RVA: 0x0000ECA1 File Offset: 0x0000CEA1
	protected new void SetColors(Color32[] dest)
	{
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 16, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x0007A118 File Offset: 0x00078318
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float num = ((!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z);
		float num2 = ((!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f));
		tk2dSpriteGeomGen.SetSlicedSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this.dimensions, new Vector2(this.borderLeft, this.borderBottom), new Vector2(this.borderRight, this.borderTop), this.anchor, num, num2);
		if (this.meshNormals.Length > 0 || this.meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(this.meshVertices, this.meshVertices[0], this.meshVertices[15], currentSprite.normals, currentSprite.tangents, this.meshNormals, this.meshTangents);
		}
		if (currentSprite.positions.Length != 4 || currentSprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x0007A288 File Offset: 0x00078488
	private void SetIndices()
	{
		int num = ((!this._borderOnly) ? 54 : 48);
		this.meshIndices = new int[num];
		tk2dSpriteGeomGen.SetSlicedSpriteIndices(this.meshIndices, 0, 0, base.CurrentSprite, this._borderOnly);
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x0007A2D0 File Offset: 0x000784D0
	private bool NearEnough(float value, float compValue, float scale)
	{
		float num = Mathf.Abs(value - compValue);
		return Mathf.Abs(num / scale) < 0.01f;
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x0007A2F8 File Offset: 0x000784F8
	private void PermanentUpgradeLegacyMode()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float x = currentSprite.untrimmedBoundsData[0].x;
		float y = currentSprite.untrimmedBoundsData[0].y;
		float x2 = currentSprite.untrimmedBoundsData[1].x;
		float y2 = currentSprite.untrimmedBoundsData[1].y;
		if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperCenter;
		}
		else if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		}
		else if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerCenter;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperRight;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleRight;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerRight;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperLeft;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleLeft;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerLeft;
		}
		else
		{
			Debug.LogError("tk2dSlicedSprite (" + base.name + ") error - Unable to determine anchor upgrading from legacy mode. Please fix this manually.");
			this._anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		}
		float num = x2 / currentSprite.texelSize.x;
		float num2 = y2 / currentSprite.texelSize.y;
		this._dimensions.x = this._scale.x * num;
		this._dimensions.y = this._scale.y * num2;
		this._scale.Set(1f, 1f, 1f);
		this.legacyMode = false;
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x0007A5C8 File Offset: 0x000787C8
	public override void Build()
	{
		if (this.legacyMode)
		{
			this.PermanentUpgradeLegacyMode();
		}
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		this.meshUvs = new Vector2[16];
		this.meshVertices = new Vector3[16];
		this.meshColors = new Color32[16];
		this.meshNormals = new Vector3[0];
		this.meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			this.meshNormals = new Vector3[16];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			this.meshTangents = new Vector4[16];
		}
		this.SetIndices();
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
		this.mesh.triangles = this.meshIndices;
		this.mesh.RecalculateBounds();
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x0000ECBD File Offset: 0x0000CEBD
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x0000ECC5 File Offset: 0x0000CEC5
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x0000ECBD File Offset: 0x0000CEBD
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x0000ECCD File Offset: 0x0000CECD
	private void UpdateIndices()
	{
		if (this.mesh != null)
		{
			this.SetIndices();
			this.mesh.triangles = this.meshIndices;
		}
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x0007A784 File Offset: 0x00078984
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

	// Token: 0x0600117D RID: 4477 RVA: 0x0007A7D4 File Offset: 0x000789D4
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
			this.UpdateCollider();
		}
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x0007A88C File Offset: 0x00078A8C
	protected override void UpdateCollider()
	{
		if (this.CreateBoxCollider && this.boxCollider != null)
		{
			this.boxCollider.size = 2f * this.boundsExtents;
			this.boxCollider.center = this.boundsCenter;
		}
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x0000ECF7 File Offset: 0x0000CEF7
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x000791C0 File Offset: 0x000773C0
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x0000ECFF File Offset: 0x0000CEFF
	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x0007A8E4 File Offset: 0x00078AE4
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

	// Token: 0x040012DE RID: 4830
	private Mesh mesh;

	// Token: 0x040012DF RID: 4831
	private Vector2[] meshUvs;

	// Token: 0x040012E0 RID: 4832
	private Vector3[] meshVertices;

	// Token: 0x040012E1 RID: 4833
	private Color32[] meshColors;

	// Token: 0x040012E2 RID: 4834
	private Vector3[] meshNormals;

	// Token: 0x040012E3 RID: 4835
	private Vector4[] meshTangents;

	// Token: 0x040012E4 RID: 4836
	private int[] meshIndices;

	// Token: 0x040012E5 RID: 4837
	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	// Token: 0x040012E6 RID: 4838
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchor;

	// Token: 0x040012E7 RID: 4839
	[SerializeField]
	private bool _borderOnly;

	// Token: 0x040012E8 RID: 4840
	[SerializeField]
	private bool legacyMode;

	// Token: 0x040012E9 RID: 4841
	public float borderTop = 0.2f;

	// Token: 0x040012EA RID: 4842
	public float borderBottom = 0.2f;

	// Token: 0x040012EB RID: 4843
	public float borderLeft = 0.2f;

	// Token: 0x040012EC RID: 4844
	public float borderRight = 0.2f;

	// Token: 0x040012ED RID: 4845
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x040012EE RID: 4846
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x040012EF RID: 4847
	private Vector3 boundsExtents = Vector3.zero;
}
