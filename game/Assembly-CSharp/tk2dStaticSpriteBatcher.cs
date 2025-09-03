using System;
using System.Collections.Generic;
using tk2dRuntime;
using UnityEngine;

// Token: 0x0200027F RID: 639
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dStaticSpriteBatcher")]
public class tk2dStaticSpriteBatcher : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x0600125A RID: 4698 RVA: 0x0000FAAD File Offset: 0x0000DCAD
	public bool CheckFlag(tk2dStaticSpriteBatcher.Flags mask)
	{
		return (this.flags & mask) != tk2dStaticSpriteBatcher.Flags.None;
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0000FABD File Offset: 0x0000DCBD
	public void SetFlag(tk2dStaticSpriteBatcher.Flags mask, bool value)
	{
		if (this.CheckFlag(mask) != value)
		{
			if (value)
			{
				this.flags |= mask;
			}
			else
			{
				this.flags &= ~mask;
			}
			this.Build();
		}
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x0000FAFA File Offset: 0x0000DCFA
	private void Awake()
	{
		this.Build();
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x0007F748 File Offset: 0x0007D948
	private bool UpgradeData()
	{
		if (this.version == tk2dStaticSpriteBatcher.CURRENT_VERSION)
		{
			return false;
		}
		if (this._scale == Vector3.zero)
		{
			this._scale = Vector3.one;
		}
		if (this.version < 2 && this.batchedSprites != null)
		{
			foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
			{
				tk2dBatchedSprite.parentId = -1;
			}
		}
		if (this.version < 3)
		{
			if (this.batchedSprites != null)
			{
				foreach (tk2dBatchedSprite tk2dBatchedSprite2 in this.batchedSprites)
				{
					if (tk2dBatchedSprite2.spriteId == -1)
					{
						tk2dBatchedSprite2.type = tk2dBatchedSprite.Type.EmptyGameObject;
					}
					else
					{
						tk2dBatchedSprite2.type = tk2dBatchedSprite.Type.Sprite;
						if (tk2dBatchedSprite2.spriteCollection == null)
						{
							tk2dBatchedSprite2.spriteCollection = this.spriteCollection;
						}
					}
				}
				this.UpdateMatrices();
			}
			this.spriteCollection = null;
		}
		this.version = tk2dStaticSpriteBatcher.CURRENT_VERSION;
		return true;
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x0000FB02 File Offset: 0x0000DD02
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
		if (this.colliderMesh)
		{
			UnityEngine.Object.Destroy(this.colliderMesh);
		}
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x0007F858 File Offset: 0x0007DA58
	public void UpdateMatrices()
	{
		bool flag = false;
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (tk2dBatchedSprite.parentId != -1)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			List<tk2dBatchedSprite> list = new List<tk2dBatchedSprite>(this.batchedSprites);
			list.Sort((tk2dBatchedSprite a, tk2dBatchedSprite b) => a.parentId.CompareTo(b.parentId));
			foreach (tk2dBatchedSprite tk2dBatchedSprite2 in list)
			{
				matrix4x.SetTRS(tk2dBatchedSprite2.position, tk2dBatchedSprite2.rotation, tk2dBatchedSprite2.localScale);
				tk2dBatchedSprite2.relativeMatrix = ((tk2dBatchedSprite2.parentId != -1) ? this.batchedSprites[tk2dBatchedSprite2.parentId].relativeMatrix : Matrix4x4.identity) * matrix4x;
			}
		}
		else
		{
			foreach (tk2dBatchedSprite tk2dBatchedSprite3 in this.batchedSprites)
			{
				tk2dBatchedSprite3.relativeMatrix.SetTRS(tk2dBatchedSprite3.position, tk2dBatchedSprite3.rotation, tk2dBatchedSprite3.localScale);
			}
		}
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x0007F9C0 File Offset: 0x0007DBC0
	public void Build()
	{
		this.UpgradeData();
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
			base.GetComponent<MeshFilter>().mesh = this.mesh;
		}
		else
		{
			this.mesh.Clear();
		}
		if (this.colliderMesh)
		{
			UnityEngine.Object.Destroy(this.colliderMesh);
			this.colliderMesh = null;
		}
		if (this.batchedSprites != null && this.batchedSprites.Length != 0)
		{
			this.SortBatchedSprites();
			this.BuildRenderMesh();
			this.BuildPhysicsMesh();
		}
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x0007FA70 File Offset: 0x0007DC70
	private void SortBatchedSprites()
	{
		List<tk2dBatchedSprite> list = new List<tk2dBatchedSprite>();
		List<tk2dBatchedSprite> list2 = new List<tk2dBatchedSprite>();
		List<tk2dBatchedSprite> list3 = new List<tk2dBatchedSprite>();
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (!tk2dBatchedSprite.IsDrawn)
			{
				list3.Add(tk2dBatchedSprite);
			}
			else
			{
				Material material = this.GetMaterial(tk2dBatchedSprite);
				if (material != null)
				{
					if (material.renderQueue == 2000)
					{
						list.Add(tk2dBatchedSprite);
					}
					else
					{
						list2.Add(tk2dBatchedSprite);
					}
				}
				else
				{
					list.Add(tk2dBatchedSprite);
				}
			}
		}
		List<tk2dBatchedSprite> list4 = new List<tk2dBatchedSprite>(list.Count + list2.Count + list3.Count);
		list4.AddRange(list);
		list4.AddRange(list2);
		list4.AddRange(list3);
		Dictionary<tk2dBatchedSprite, int> dictionary = new Dictionary<tk2dBatchedSprite, int>();
		int num = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in list4)
		{
			dictionary[tk2dBatchedSprite2] = num++;
		}
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in list4)
		{
			if (tk2dBatchedSprite3.parentId != -1)
			{
				tk2dBatchedSprite3.parentId = dictionary[this.batchedSprites[tk2dBatchedSprite3.parentId]];
			}
		}
		this.batchedSprites = list4.ToArray();
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x0007FC24 File Offset: 0x0007DE24
	private Material GetMaterial(tk2dBatchedSprite bs)
	{
		tk2dBatchedSprite.Type type = bs.type;
		if (type == tk2dBatchedSprite.Type.EmptyGameObject)
		{
			return null;
		}
		if (type != tk2dBatchedSprite.Type.TextMesh)
		{
			return bs.GetSpriteDefinition().materialInst;
		}
		return this.allTextMeshData[bs.xRefId].font.materialInst;
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x0007FC70 File Offset: 0x0007DE70
	private void BuildRenderMesh()
	{
		List<Material> list = new List<Material>();
		List<List<int>> list2 = new List<List<int>>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = this.CheckFlag(tk2dStaticSpriteBatcher.Flags.FlattenDepth);
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite.GetSpriteDefinition();
			if (spriteDefinition != null)
			{
				flag |= spriteDefinition.normals != null && spriteDefinition.normals.Length > 0;
				flag2 |= spriteDefinition.tangents != null && spriteDefinition.tangents.Length > 0;
			}
			if (tk2dBatchedSprite.type == tk2dBatchedSprite.Type.TextMesh)
			{
				tk2dTextMeshData tk2dTextMeshData = this.allTextMeshData[tk2dBatchedSprite.xRefId];
				if (tk2dTextMeshData.font != null && tk2dTextMeshData.font.inst.textureGradients)
				{
					flag3 = true;
				}
			}
		}
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		int num = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in this.batchedSprites)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition2 = tk2dBatchedSprite2.GetSpriteDefinition();
			int num2 = 0;
			int num3 = 0;
			switch (tk2dBatchedSprite2.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetSpriteGeomDesc(out num2, out num3, spriteDefinition2);
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num2, out num3, spriteDefinition2, tk2dBatchedSprite2.Dimensions);
				}
				break;
			case tk2dBatchedSprite.Type.SlicedSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetSlicedSpriteGeomDesc(out num2, out num3, spriteDefinition2, tk2dBatchedSprite2.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly));
				}
				break;
			case tk2dBatchedSprite.Type.ClippedSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetClippedSpriteGeomDesc(out num2, out num3, spriteDefinition2);
				}
				break;
			case tk2dBatchedSprite.Type.TextMesh:
			{
				tk2dTextMeshData tk2dTextMeshData2 = this.allTextMeshData[tk2dBatchedSprite2.xRefId];
				tk2dTextGeomGen.GetTextMeshGeomDesc(out num2, out num3, tk2dTextGeomGen.Data(tk2dTextMeshData2, tk2dTextMeshData2.font.inst, tk2dBatchedSprite2.FormattedText));
				break;
			}
			}
			num += num2;
			list3.Add(num2);
			list4.Add(num3);
		}
		Vector3[] array3 = ((!flag) ? null : new Vector3[num]);
		Vector4[] array4 = ((!flag2) ? null : new Vector4[num]);
		Vector3[] array5 = new Vector3[num];
		Color32[] array6 = new Color32[num];
		Vector2[] array7 = new Vector2[num];
		Vector2[] array8 = ((!flag3) ? null : new Vector2[num]);
		int num4 = 0;
		Material material = null;
		List<int> list5 = null;
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = this._scale.x;
		identity.m11 = this._scale.y;
		identity.m22 = this._scale.z;
		int num5 = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in this.batchedSprites)
		{
			if (!tk2dBatchedSprite3.IsDrawn)
			{
				break;
			}
			if (tk2dBatchedSprite3.type == tk2dBatchedSprite.Type.EmptyGameObject)
			{
				num5++;
			}
			else
			{
				tk2dSpriteDefinition spriteDefinition3 = tk2dBatchedSprite3.GetSpriteDefinition();
				int num6 = list3[num5];
				int num7 = list4[num5];
				Material material2 = this.GetMaterial(tk2dBatchedSprite3);
				if (material2 != material)
				{
					if (material != null)
					{
						list.Add(material);
						list2.Add(list5);
					}
					material = material2;
					list5 = new List<int>();
				}
				Vector3[] array10 = new Vector3[num6];
				Vector2[] array11 = new Vector2[num6];
				Vector2[] array12 = ((!flag3) ? null : new Vector2[num6]);
				Color32[] array13 = new Color32[num6];
				Vector3[] array14 = ((!flag) ? null : new Vector3[num6]);
				Vector4[] array15 = ((!flag2) ? null : new Vector4[num6]);
				int[] array16 = new int[num7];
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				switch (tk2dBatchedSprite3.type)
				{
				case tk2dBatchedSprite.Type.Sprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetSpriteGeom(array10, array11, array14, array15, 0, spriteDefinition3, Vector3.one);
						tk2dSpriteGeomGen.SetSpriteIndices(array16, 0, num4, spriteDefinition3);
					}
					break;
				case tk2dBatchedSprite.Type.TiledSprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetTiledSpriteGeom(array10, array11, 0, out zero, out zero2, spriteDefinition3, Vector3.one, tk2dBatchedSprite3.Dimensions, tk2dBatchedSprite3.anchor, tk2dBatchedSprite3.BoxColliderOffsetZ, tk2dBatchedSprite3.BoxColliderExtentZ);
						tk2dSpriteGeomGen.SetTiledSpriteIndices(array16, 0, num4, spriteDefinition3, tk2dBatchedSprite3.Dimensions);
					}
					break;
				case tk2dBatchedSprite.Type.SlicedSprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetSlicedSpriteGeom(array10, array11, 0, out zero, out zero2, spriteDefinition3, Vector3.one, tk2dBatchedSprite3.Dimensions, tk2dBatchedSprite3.SlicedSpriteBorderBottomLeft, tk2dBatchedSprite3.SlicedSpriteBorderTopRight, tk2dBatchedSprite3.anchor, tk2dBatchedSprite3.BoxColliderOffsetZ, tk2dBatchedSprite3.BoxColliderExtentZ);
						tk2dSpriteGeomGen.SetSlicedSpriteIndices(array16, 0, num4, spriteDefinition3, tk2dBatchedSprite3.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly));
					}
					break;
				case tk2dBatchedSprite.Type.ClippedSprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetClippedSpriteGeom(array10, array11, 0, out zero, out zero2, spriteDefinition3, Vector3.one, tk2dBatchedSprite3.ClippedSpriteRegionBottomLeft, tk2dBatchedSprite3.ClippedSpriteRegionTopRight, tk2dBatchedSprite3.BoxColliderOffsetZ, tk2dBatchedSprite3.BoxColliderExtentZ);
						tk2dSpriteGeomGen.SetClippedSpriteIndices(array16, 0, num4, spriteDefinition3);
					}
					break;
				case tk2dBatchedSprite.Type.TextMesh:
				{
					tk2dTextMeshData tk2dTextMeshData3 = this.allTextMeshData[tk2dBatchedSprite3.xRefId];
					tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(tk2dTextMeshData3, tk2dTextMeshData3.font.inst, tk2dBatchedSprite3.FormattedText);
					int num8 = tk2dTextGeomGen.SetTextMeshGeom(array10, array11, array12, array13, 0, geomData);
					if (!geomData.fontInst.isPacked)
					{
						Color32 color = tk2dTextMeshData3.color;
						Color32 color2 = ((!tk2dTextMeshData3.useGradient) ? tk2dTextMeshData3.color : tk2dTextMeshData3.color2);
						for (int l = 0; l < array13.Length; l++)
						{
							Color32 color3 = ((l % 4 >= 2) ? color2 : color);
							byte b = array13[l].r * color3.r / byte.MaxValue;
							byte b2 = array13[l].g * color3.g / byte.MaxValue;
							byte b3 = array13[l].b * color3.b / byte.MaxValue;
							byte b4 = array13[l].a * color3.a / byte.MaxValue;
							if (geomData.fontInst.premultipliedAlpha)
							{
								b = b * b4 / byte.MaxValue;
								b2 = b2 * b4 / byte.MaxValue;
								b3 = b3 * b4 / byte.MaxValue;
							}
							array13[l] = new Color32(b, b2, b3, b4);
						}
					}
					tk2dTextGeomGen.SetTextMeshIndices(array16, 0, num4, geomData, num8);
					break;
				}
				}
				tk2dBatchedSprite3.CachedBoundsCenter = zero;
				tk2dBatchedSprite3.CachedBoundsExtents = zero2;
				if (num6 > 0 && tk2dBatchedSprite3.type != tk2dBatchedSprite.Type.TextMesh)
				{
					bool flag5 = tk2dBatchedSprite3.spriteCollection != null && tk2dBatchedSprite3.spriteCollection.premultipliedAlpha;
					tk2dSpriteGeomGen.SetSpriteColors(array13, 0, num6, tk2dBatchedSprite3.color, flag5);
				}
				Matrix4x4 matrix4x = identity * tk2dBatchedSprite3.relativeMatrix;
				for (int m = 0; m < num6; m++)
				{
					Vector3 vector = Vector3.Scale(array10[m], tk2dBatchedSprite3.baseScale);
					vector = matrix4x.MultiplyPoint(vector);
					if (flag4)
					{
						vector.z = 0f;
					}
					array5[num4 + m] = vector;
					array7[num4 + m] = array11[m];
					if (flag3)
					{
						array8[num4 + m] = array12[m];
					}
					array6[num4 + m] = array13[m];
					if (flag)
					{
						array3[num4 + m] = tk2dBatchedSprite3.rotation * array14[m];
					}
					if (flag2)
					{
						Vector3 vector2 = new Vector3(array15[m].x, array15[m].y, array15[m].z);
						vector2 = tk2dBatchedSprite3.rotation * vector2;
						array4[num4 + m] = new Vector4(vector2.x, vector2.y, vector2.z, array15[m].w);
					}
				}
				list5.AddRange(array16);
				num4 += num6;
				num5++;
			}
		}
		if (list5 != null)
		{
			list.Add(material);
			list2.Add(list5);
		}
		if (this.mesh)
		{
			this.mesh.vertices = array5;
			this.mesh.uv = array7;
			if (flag3)
			{
				this.mesh.uv2 = array8;
			}
			this.mesh.colors32 = array6;
			if (flag)
			{
				this.mesh.normals = array3;
			}
			if (flag2)
			{
				this.mesh.tangents = array4;
			}
			this.mesh.subMeshCount = list2.Count;
			for (int n = 0; n < list2.Count; n++)
			{
				this.mesh.SetTriangles(list2[n].ToArray(), n);
			}
			this.mesh.RecalculateBounds();
		}
		base.renderer.sharedMaterials = list.ToArray();
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x00080638 File Offset: 0x0007E838
	private void BuildPhysicsMesh()
	{
		MeshCollider meshCollider = base.GetComponent<MeshCollider>();
		if (meshCollider != null)
		{
			if (base.collider != meshCollider)
			{
				return;
			}
			if (!this.CheckFlag(tk2dStaticSpriteBatcher.Flags.GenerateCollider))
			{
				UnityEngine.Object.Destroy(meshCollider);
			}
		}
		if (!this.CheckFlag(tk2dStaticSpriteBatcher.Flags.GenerateCollider))
		{
			return;
		}
		bool flag = this.CheckFlag(tk2dStaticSpriteBatcher.Flags.FlattenDepth);
		int num = 0;
		int num2 = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (!tk2dBatchedSprite.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite.GetSpriteDefinition();
			bool flag2 = false;
			bool flag3 = false;
			switch (tk2dBatchedSprite.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag2 = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag3 = true;
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag3 = tk2dBatchedSprite.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				break;
			}
			if (flag2)
			{
				num += spriteDefinition.colliderIndicesFwd.Length;
				num2 += spriteDefinition.colliderVertices.Length;
			}
			else if (flag3)
			{
				num += 36;
				num2 += 8;
			}
		}
		if (num == 0)
		{
			if (this.colliderMesh)
			{
				UnityEngine.Object.Destroy(this.colliderMesh);
			}
			return;
		}
		if (meshCollider == null)
		{
			meshCollider = base.gameObject.AddComponent<MeshCollider>();
		}
		if (this.colliderMesh == null)
		{
			this.colliderMesh = new Mesh();
			this.colliderMesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			this.colliderMesh.Clear();
		}
		int num3 = 0;
		Vector3[] array2 = new Vector3[num2];
		int num4 = 0;
		int[] array3 = new int[num];
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = this._scale.x;
		identity.m11 = this._scale.y;
		identity.m22 = this._scale.z;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in this.batchedSprites)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition2 = tk2dBatchedSprite2.GetSpriteDefinition();
			bool flag4 = false;
			bool flag5 = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			switch (tk2dBatchedSprite2.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition2 != null && spriteDefinition2.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag4 = true;
				}
				if (spriteDefinition2 != null && spriteDefinition2.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag5 = true;
					vector = spriteDefinition2.colliderVertices[0];
					vector2 = spriteDefinition2.colliderVertices[1];
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag5 = tk2dBatchedSprite2.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				if (flag5)
				{
					vector = tk2dBatchedSprite2.CachedBoundsCenter;
					vector2 = tk2dBatchedSprite2.CachedBoundsExtents;
				}
				break;
			}
			Matrix4x4 matrix4x = identity * tk2dBatchedSprite2.relativeMatrix;
			if (flag)
			{
				matrix4x.m23 = 0f;
			}
			if (flag4)
			{
				tk2dSpriteGeomGen.SetSpriteDefinitionMeshData(array2, array3, num3, num4, num3, spriteDefinition2, matrix4x, tk2dBatchedSprite2.baseScale);
				num3 += spriteDefinition2.colliderVertices.Length;
				num4 += spriteDefinition2.colliderIndicesFwd.Length;
			}
			else if (flag5)
			{
				tk2dSpriteGeomGen.SetBoxMeshData(array2, array3, num3, num4, num3, vector, vector2, matrix4x, tk2dBatchedSprite2.baseScale);
				num3 += 8;
				num4 += 36;
			}
		}
		this.colliderMesh.vertices = array2;
		this.colliderMesh.triangles = array3;
		meshCollider.sharedMesh = this.colliderMesh;
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x0000FB3A File Offset: 0x0000DD3A
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return this.spriteCollection == spriteCollection;
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x0000FAFA File Offset: 0x0000DCFA
	public void ForceBuild()
	{
		this.Build();
	}

	// Token: 0x04001451 RID: 5201
	public static int CURRENT_VERSION = 3;

	// Token: 0x04001452 RID: 5202
	public int version;

	// Token: 0x04001453 RID: 5203
	public tk2dBatchedSprite[] batchedSprites;

	// Token: 0x04001454 RID: 5204
	public tk2dTextMeshData[] allTextMeshData;

	// Token: 0x04001455 RID: 5205
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04001456 RID: 5206
	[SerializeField]
	private tk2dStaticSpriteBatcher.Flags flags = tk2dStaticSpriteBatcher.Flags.GenerateCollider;

	// Token: 0x04001457 RID: 5207
	private Mesh mesh;

	// Token: 0x04001458 RID: 5208
	private Mesh colliderMesh;

	// Token: 0x04001459 RID: 5209
	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x02000280 RID: 640
	public enum Flags
	{
		// Token: 0x0400145C RID: 5212
		None,
		// Token: 0x0400145D RID: 5213
		GenerateCollider,
		// Token: 0x0400145E RID: 5214
		FlattenDepth,
		// Token: 0x0400145F RID: 5215
		SortToCamera = 4
	}
}
