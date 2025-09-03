using System;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000251 RID: 593
[AddComponentMenu("2D Toolkit/Backend/tk2dBaseSprite")]
public abstract class tk2dBaseSprite : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001111 RID: 4369 RVA: 0x0000E868 File Offset: 0x0000CA68
	// (remove) Token: 0x06001112 RID: 4370 RVA: 0x0000E881 File Offset: 0x0000CA81
	public event Action<tk2dBaseSprite> SpriteChanged;

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001113 RID: 4371 RVA: 0x0000E89A File Offset: 0x0000CA9A
	// (set) Token: 0x06001114 RID: 4372 RVA: 0x0000E8A2 File Offset: 0x0000CAA2
	public tk2dSpriteCollectionData Collection
	{
		get
		{
			return this.collection;
		}
		set
		{
			this.collection = value;
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x0000E8BC File Offset: 0x0000CABC
	private void InitInstance()
	{
		if (this.collectionInst == null && this.collection != null)
		{
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001116 RID: 4374 RVA: 0x0000E8F1 File Offset: 0x0000CAF1
	// (set) Token: 0x06001117 RID: 4375 RVA: 0x0000E8F9 File Offset: 0x0000CAF9
	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			if (value != this._color)
			{
				this._color = value;
				this.InitInstance();
				this.UpdateColors();
			}
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06001118 RID: 4376 RVA: 0x0000E91F File Offset: 0x0000CB1F
	// (set) Token: 0x06001119 RID: 4377 RVA: 0x00077E38 File Offset: 0x00076038
	public Vector3 scale
	{
		get
		{
			return this._scale;
		}
		set
		{
			if (value != this._scale)
			{
				this._scale = value;
				this.InitInstance();
				this.UpdateVertices();
				this.UpdateCollider();
				if (this.SpriteChanged != null)
				{
					this.SpriteChanged(this);
				}
			}
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600111A RID: 4378 RVA: 0x0000E927 File Offset: 0x0000CB27
	// (set) Token: 0x0600111B RID: 4379 RVA: 0x0000E92F File Offset: 0x0000CB2F
	public int SortingOrder
	{
		get
		{
			return this.renderLayer;
		}
		set
		{
			if (this.renderLayer != value)
			{
				this.renderLayer = value;
				this.InitInstance();
				this.UpdateVertices();
			}
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x0600111C RID: 4380 RVA: 0x0000E950 File Offset: 0x0000CB50
	// (set) Token: 0x0600111D RID: 4381 RVA: 0x00077E88 File Offset: 0x00076088
	public bool FlipX
	{
		get
		{
			return this._scale.x < 0f;
		}
		set
		{
			this.scale = new Vector3(Mathf.Abs(this._scale.x) * (float)((!value) ? 1 : (-1)), this._scale.y, this._scale.z);
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x0600111E RID: 4382 RVA: 0x0000E964 File Offset: 0x0000CB64
	// (set) Token: 0x0600111F RID: 4383 RVA: 0x00077ED8 File Offset: 0x000760D8
	public bool FlipY
	{
		get
		{
			return this._scale.y < 0f;
		}
		set
		{
			this.scale = new Vector3(this._scale.x, Mathf.Abs(this._scale.y) * (float)((!value) ? 1 : (-1)), this._scale.z);
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001120 RID: 4384 RVA: 0x0000E978 File Offset: 0x0000CB78
	// (set) Token: 0x06001121 RID: 4385 RVA: 0x00077F28 File Offset: 0x00076128
	public int spriteId
	{
		get
		{
			return this._spriteId;
		}
		set
		{
			if (value != this._spriteId)
			{
				this.InitInstance();
				value = Mathf.Clamp(value, 0, this.collectionInst.spriteDefinitions.Length - 1);
				if (this._spriteId < 0 || this._spriteId >= this.collectionInst.spriteDefinitions.Length || this.GetCurrentVertexCount() != this.collectionInst.spriteDefinitions[value].positions.Length || this.collectionInst.spriteDefinitions[this._spriteId].complexGeometry != this.collectionInst.spriteDefinitions[value].complexGeometry)
				{
					this._spriteId = value;
					this.UpdateGeometry();
				}
				else
				{
					this._spriteId = value;
					this.UpdateVertices();
				}
				this.UpdateMaterial();
				this.UpdateCollider();
				if (this.SpriteChanged != null)
				{
					this.SpriteChanged(this);
				}
			}
		}
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x0000E980 File Offset: 0x0000CB80
	public void SetSprite(int newSpriteId)
	{
		this.spriteId = newSpriteId;
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x00078014 File Offset: 0x00076214
	public bool SetSprite(string spriteName)
	{
		int spriteIdByName = this.collection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			this.SetSprite(spriteIdByName);
		}
		else
		{
			Debug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x0007805C File Offset: 0x0007625C
	public void SetSprite(tk2dSpriteCollectionData newCollection, int newSpriteId)
	{
		bool flag = false;
		if (this.Collection != newCollection)
		{
			this.collection = newCollection;
			this.collectionInst = this.collection.inst;
			this._spriteId = -1;
			flag = true;
		}
		this.spriteId = newSpriteId;
		if (flag)
		{
			this.UpdateMaterial();
		}
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x000780B0 File Offset: 0x000762B0
	public bool SetSprite(tk2dSpriteCollectionData newCollection, string spriteName)
	{
		int spriteIdByName = newCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			this.SetSprite(newCollection, spriteIdByName);
		}
		else
		{
			Debug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x000780F4 File Offset: 0x000762F4
	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera != null)
		{
			if (this.Collection.version < 2)
			{
				Debug.LogError("Need to rebuild sprite collection.");
			}
			float num2 = base.transform.position.z - tk2dCamera.transform.position.z;
			float num3 = this.Collection.invOrthoSize * this.Collection.halfTargetHeight;
			num = tk2dCamera.GetSizeAtDistance(num2) * num3;
		}
		else if (Camera.main)
		{
			if (Camera.main.isOrthoGraphic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float num4 = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, num4);
			}
			num *= this.Collection.invOrthoSize;
		}
		else
		{
			Debug.LogError("Main camera not found.");
		}
		this.scale = new Vector3(Mathf.Sign(this.scale.x) * num, Mathf.Sign(this.scale.y) * num, Mathf.Sign(this.scale.z) * num);
	}

	// Token: 0x06001127 RID: 4391
	protected abstract void UpdateMaterial();

	// Token: 0x06001128 RID: 4392
	protected abstract void UpdateColors();

	// Token: 0x06001129 RID: 4393
	protected abstract void UpdateVertices();

	// Token: 0x0600112A RID: 4394
	protected abstract void UpdateGeometry();

	// Token: 0x0600112B RID: 4395
	protected abstract int GetCurrentVertexCount();

	// Token: 0x0600112C RID: 4396
	public abstract void Build();

	// Token: 0x0600112D RID: 4397 RVA: 0x0000E989 File Offset: 0x0000CB89
	public int GetSpriteIdByName(string name)
	{
		this.InitInstance();
		return this.collectionInst.GetSpriteIdByName(name);
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x0007826C File Offset: 0x0007646C
	public static T AddComponent<T>(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId) where T : tk2dBaseSprite
	{
		T t = go.AddComponent<T>();
		t._spriteId = -1;
		t.SetSprite(spriteCollection, spriteId);
		t.Build();
		return t;
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x000782AC File Offset: 0x000764AC
	public static T AddComponent<T>(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName) where T : tk2dBaseSprite
	{
		int spriteIdByName = spriteCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName == -1)
		{
			Debug.LogError(string.Format("Unable to find sprite named {0} in sprite collection {1}", spriteName, spriteCollection.spriteCollectionName));
			return (T)((object)null);
		}
		return tk2dBaseSprite.AddComponent<T>(go, spriteCollection, spriteIdByName);
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x0000E99D File Offset: 0x0000CB9D
	protected int GetNumVertices()
	{
		this.InitInstance();
		return this.collectionInst.spriteDefinitions[this.spriteId].positions.Length;
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0000E9BE File Offset: 0x0000CBBE
	protected int GetNumIndices()
	{
		this.InitInstance();
		return this.collectionInst.spriteDefinitions[this.spriteId].indices.Length;
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000782F0 File Offset: 0x000764F0
	protected void SetPositions(Vector3[] positions, Vector3[] normals, Vector4[] tangents)
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this.spriteId];
		int numVertices = this.GetNumVertices();
		for (int i = 0; i < numVertices; i++)
		{
			positions[i].x = tk2dSpriteDefinition.positions[i].x * this._scale.x;
			positions[i].y = tk2dSpriteDefinition.positions[i].y * this._scale.y;
			positions[i].z = tk2dSpriteDefinition.positions[i].z * this._scale.z;
		}
		if (normals.Length > 0)
		{
			for (int j = 0; j < numVertices; j++)
			{
				normals[j] = tk2dSpriteDefinition.normals[j];
			}
		}
		if (tangents.Length > 0)
		{
			for (int k = 0; k < numVertices; k++)
			{
				tangents[k] = tk2dSpriteDefinition.tangents[k];
			}
		}
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x0007841C File Offset: 0x0007661C
	protected void SetColors(Color32[] dest)
	{
		Color color = this._color;
		if (this.collectionInst.premultipliedAlpha)
		{
			color.r *= color.a;
			color.g *= color.a;
			color.b *= color.a;
		}
		Color32 color2 = color;
		int numVertices = this.GetNumVertices();
		for (int i = 0; i < numVertices; i++)
		{
			dest[i] = color2;
		}
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x000784AC File Offset: 0x000766AC
	public Bounds GetBounds()
	{
		this.InitInstance();
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		return new Bounds(new Vector3(tk2dSpriteDefinition.boundsData[0].x * this._scale.x, tk2dSpriteDefinition.boundsData[0].y * this._scale.y, tk2dSpriteDefinition.boundsData[0].z * this._scale.z), new Vector3(tk2dSpriteDefinition.boundsData[1].x * Mathf.Abs(this._scale.x), tk2dSpriteDefinition.boundsData[1].y * Mathf.Abs(this._scale.y), tk2dSpriteDefinition.boundsData[1].z * Mathf.Abs(this._scale.z)));
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x000785A0 File Offset: 0x000767A0
	public Bounds GetUntrimmedBounds()
	{
		this.InitInstance();
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		return new Bounds(new Vector3(tk2dSpriteDefinition.untrimmedBoundsData[0].x * this._scale.x, tk2dSpriteDefinition.untrimmedBoundsData[0].y * this._scale.y, tk2dSpriteDefinition.untrimmedBoundsData[0].z * this._scale.z), new Vector3(tk2dSpriteDefinition.untrimmedBoundsData[1].x * Mathf.Abs(this._scale.x), tk2dSpriteDefinition.untrimmedBoundsData[1].y * Mathf.Abs(this._scale.y), tk2dSpriteDefinition.untrimmedBoundsData[1].z * Mathf.Abs(this._scale.z)));
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x00078694 File Offset: 0x00076894
	public static Bounds AdjustedMeshBounds(Bounds bounds, int renderLayer)
	{
		Vector3 center = bounds.center;
		center.z = (float)(-(float)renderLayer) * 0.01f;
		bounds.center = center;
		return bounds;
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x0000E9DF File Offset: 0x0000CBDF
	public tk2dSpriteDefinition GetCurrentSpriteDef()
	{
		this.InitInstance();
		return (!(this.collectionInst == null)) ? this.collectionInst.spriteDefinitions[this._spriteId] : null;
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001138 RID: 4408 RVA: 0x0000E9DF File Offset: 0x0000CBDF
	public tk2dSpriteDefinition CurrentSprite
	{
		get
		{
			this.InitInstance();
			return (!(this.collectionInst == null)) ? this.collectionInst.spriteDefinitions[this._spriteId] : null;
		}
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x00003648 File Offset: 0x00001848
	public virtual void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x000090F9 File Offset: 0x000072F9
	protected virtual bool NeedBoxCollider()
	{
		return false;
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x000786C4 File Offset: 0x000768C4
	protected virtual void UpdateCollider()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box && this.boxCollider == null)
		{
			this.boxCollider = base.gameObject.GetComponent<BoxCollider>();
			if (this.boxCollider == null)
			{
				this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}
		if (this.boxCollider != null)
		{
			if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
			{
				this.boxCollider.center = new Vector3(tk2dSpriteDefinition.colliderVertices[0].x * this._scale.x, tk2dSpriteDefinition.colliderVertices[0].y * this._scale.y, tk2dSpriteDefinition.colliderVertices[0].z * this._scale.z);
				this.boxCollider.size = new Vector3(2f * tk2dSpriteDefinition.colliderVertices[1].x * this._scale.x, 2f * tk2dSpriteDefinition.colliderVertices[1].y * this._scale.y, 2f * tk2dSpriteDefinition.colliderVertices[1].z * this._scale.z);
			}
			else if (tk2dSpriteDefinition.colliderType != tk2dSpriteDefinition.ColliderType.Unset)
			{
				if (this.boxCollider != null)
				{
					this.boxCollider.center = new Vector3(0f, 0f, -100000f);
					this.boxCollider.size = Vector3.zero;
				}
			}
		}
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00078888 File Offset: 0x00076A88
	protected virtual void CreateCollider()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Unset)
		{
			return;
		}
		if (base.collider != null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
			this.meshCollider = base.GetComponent<MeshCollider>();
		}
		if ((this.NeedBoxCollider() || tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box) && this.meshCollider == null)
		{
			if (this.boxCollider == null)
			{
				this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}
		else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh && this.boxCollider == null)
		{
			if (this.meshCollider == null)
			{
				this.meshCollider = base.gameObject.AddComponent<MeshCollider>();
			}
			if (this.meshColliderMesh == null)
			{
				this.meshColliderMesh = new Mesh();
			}
			this.meshColliderMesh.Clear();
			this.meshColliderPositions = new Vector3[tk2dSpriteDefinition.colliderVertices.Length];
			for (int i = 0; i < this.meshColliderPositions.Length; i++)
			{
				this.meshColliderPositions[i] = new Vector3(tk2dSpriteDefinition.colliderVertices[i].x * this._scale.x, tk2dSpriteDefinition.colliderVertices[i].y * this._scale.y, tk2dSpriteDefinition.colliderVertices[i].z * this._scale.z);
			}
			this.meshColliderMesh.vertices = this.meshColliderPositions;
			float num = this._scale.x * this._scale.y * this._scale.z;
			this.meshColliderMesh.triangles = ((num < 0f) ? tk2dSpriteDefinition.colliderIndicesBack : tk2dSpriteDefinition.colliderIndicesFwd);
			this.meshCollider.sharedMesh = this.meshColliderMesh;
			this.meshCollider.convex = tk2dSpriteDefinition.colliderConvex;
			this.meshCollider.smoothSphereCollisions = tk2dSpriteDefinition.colliderSmoothSphereCollisions;
			if (base.rigidbody)
			{
				base.rigidbody.centerOfMass = Vector3.zero;
			}
		}
		else if (tk2dSpriteDefinition.colliderType != tk2dSpriteDefinition.ColliderType.None && Application.isPlaying)
		{
			Debug.LogError("Invalid mesh collider on sprite, please remove and try again.");
		}
		this.UpdateCollider();
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x0000EA10 File Offset: 0x0000CC10
	protected void Awake()
	{
		if (this.collection != null)
		{
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x0000EA34 File Offset: 0x0000CC34
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return this.Collection == spriteCollection;
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00078B08 File Offset: 0x00076D08
	public virtual void ForceBuild()
	{
		if (this.collection == null)
		{
			return;
		}
		this.collectionInst = this.collection.inst;
		if (this.spriteId < 0 || this.spriteId >= this.collectionInst.spriteDefinitions.Length)
		{
			this.spriteId = 0;
		}
		this.Build();
		if (this.SpriteChanged != null)
		{
			this.SpriteChanged(this);
		}
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x00078B80 File Offset: 0x00076D80
	public static GameObject CreateFromTexture<T>(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor) where T : tk2dBaseSprite
	{
		tk2dSpriteCollectionData tk2dSpriteCollectionData = SpriteCollectionGenerator.CreateFromTexture(texture, size, region, anchor);
		if (tk2dSpriteCollectionData == null)
		{
			return null;
		}
		GameObject gameObject = new GameObject();
		tk2dBaseSprite.AddComponent<T>(gameObject, tk2dSpriteCollectionData, 0);
		return gameObject;
	}

	// Token: 0x040012B5 RID: 4789
	[SerializeField]
	private tk2dSpriteCollectionData collection;

	// Token: 0x040012B6 RID: 4790
	protected tk2dSpriteCollectionData collectionInst;

	// Token: 0x040012B7 RID: 4791
	[SerializeField]
	protected Color _color = Color.white;

	// Token: 0x040012B8 RID: 4792
	[SerializeField]
	protected Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x040012B9 RID: 4793
	[SerializeField]
	protected int _spriteId;

	// Token: 0x040012BA RID: 4794
	public BoxCollider boxCollider;

	// Token: 0x040012BB RID: 4795
	public MeshCollider meshCollider;

	// Token: 0x040012BC RID: 4796
	public Vector3[] meshColliderPositions;

	// Token: 0x040012BD RID: 4797
	public Mesh meshColliderMesh;

	// Token: 0x040012BE RID: 4798
	[SerializeField]
	protected int renderLayer;

	// Token: 0x02000252 RID: 594
	public enum Anchor
	{
		// Token: 0x040012C1 RID: 4801
		LowerLeft,
		// Token: 0x040012C2 RID: 4802
		LowerCenter,
		// Token: 0x040012C3 RID: 4803
		LowerRight,
		// Token: 0x040012C4 RID: 4804
		MiddleLeft,
		// Token: 0x040012C5 RID: 4805
		MiddleCenter,
		// Token: 0x040012C6 RID: 4806
		MiddleRight,
		// Token: 0x040012C7 RID: 4807
		UpperLeft,
		// Token: 0x040012C8 RID: 4808
		UpperCenter,
		// Token: 0x040012C9 RID: 4809
		UpperRight
	}
}
