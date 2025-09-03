using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007A RID: 122
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000375 RID: 885 RVA: 0x00005DBA File Offset: 0x00003FBA
	[Obsolete("Use UIDrawCall.activeList")]
	public static BetterList<UIDrawCall> list
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000376 RID: 886 RVA: 0x00005DBA File Offset: 0x00003FBA
	public static BetterList<UIDrawCall> activeList
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000377 RID: 887 RVA: 0x00005DC1 File Offset: 0x00003FC1
	public static BetterList<UIDrawCall> inactiveList
	{
		get
		{
			return UIDrawCall.mInactiveList;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000378 RID: 888 RVA: 0x00005DC8 File Offset: 0x00003FC8
	// (set) Token: 0x06000379 RID: 889 RVA: 0x00005DD0 File Offset: 0x00003FD0
	public bool isDirty
	{
		get
		{
			return this.mDirty;
		}
		set
		{
			this.mDirty = value;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600037A RID: 890 RVA: 0x00005DD9 File Offset: 0x00003FD9
	// (set) Token: 0x0600037B RID: 891 RVA: 0x00005DE1 File Offset: 0x00003FE1
	public int renderQueue
	{
		get
		{
			return this.mRenderQueue;
		}
		set
		{
			if (this.mRenderQueue != value)
			{
				this.mRenderQueue = value;
				if (this.mDynamicMat != null)
				{
					this.mDynamicMat.renderQueue = value;
				}
			}
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00005E13 File Offset: 0x00004013
	public int finalRenderQueue
	{
		get
		{
			return (!(this.mDynamicMat != null)) ? this.mRenderQueue : this.mDynamicMat.renderQueue;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600037D RID: 893 RVA: 0x00005E3C File Offset: 0x0000403C
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600037E RID: 894 RVA: 0x00005E61 File Offset: 0x00004061
	// (set) Token: 0x0600037F RID: 895 RVA: 0x00005E69 File Offset: 0x00004069
	public Material baseMaterial
	{
		get
		{
			return this.mMaterial;
		}
		set
		{
			if (this.mMaterial != value)
			{
				this.mMaterial = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000380 RID: 896 RVA: 0x00005E8A File Offset: 0x0000408A
	public Material dynamicMaterial
	{
		get
		{
			return this.mDynamicMat;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000381 RID: 897 RVA: 0x00005E92 File Offset: 0x00004092
	// (set) Token: 0x06000382 RID: 898 RVA: 0x00005E9A File Offset: 0x0000409A
	public Texture mainTexture
	{
		get
		{
			return this.mTexture;
		}
		set
		{
			this.mTexture = value;
			if (this.mDynamicMat != null)
			{
				this.mDynamicMat.mainTexture = value;
			}
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000383 RID: 899 RVA: 0x00005EC0 File Offset: 0x000040C0
	// (set) Token: 0x06000384 RID: 900 RVA: 0x00005EC8 File Offset: 0x000040C8
	public Shader shader
	{
		get
		{
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000385 RID: 901 RVA: 0x00005EE9 File Offset: 0x000040E9
	public int triangles
	{
		get
		{
			return (!(this.mMesh != null)) ? 0 : this.mTriangles;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000386 RID: 902 RVA: 0x00005F08 File Offset: 0x00004108
	public bool isClipped
	{
		get
		{
			return this.mClipping != UIDrawCall.Clipping.None;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000387 RID: 903 RVA: 0x00005F16 File Offset: 0x00004116
	// (set) Token: 0x06000388 RID: 904 RVA: 0x00005F1E File Offset: 0x0000411E
	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mClipping = value;
				this.mReset = true;
			}
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000389 RID: 905 RVA: 0x00005F3A File Offset: 0x0000413A
	// (set) Token: 0x0600038A RID: 906 RVA: 0x00005F42 File Offset: 0x00004142
	public Vector4 clipRange
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			this.mClipRange = value;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600038B RID: 907 RVA: 0x00005F4B File Offset: 0x0000414B
	// (set) Token: 0x0600038C RID: 908 RVA: 0x00005F53 File Offset: 0x00004153
	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoft;
		}
		set
		{
			this.mClipSoft = value;
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00022824 File Offset: 0x00020A24
	private void CreateMaterial()
	{
		string text = ((!(this.mShader != null)) ? ((!(this.mMaterial != null)) ? "Unlit/Transparent Colored" : this.mMaterial.shader.name) : this.mShader.name);
		text = text.Replace("GUI/Text Shader", "Unlit/Text");
		text = text.Replace(" (AlphaClip)", string.Empty);
		text = text.Replace(" (SoftClip)", string.Empty);
		Shader shader;
		if (this.mClipping == UIDrawCall.Clipping.SoftClip)
		{
			shader = Shader.Find(text + " (SoftClip)");
		}
		else if (this.mClipping == UIDrawCall.Clipping.AlphaClip)
		{
			shader = Shader.Find(text + " (AlphaClip)");
		}
		else
		{
			shader = ((!(this.mShader != null)) ? Shader.Find(text) : this.mShader);
		}
		if (this.mMaterial != null)
		{
			this.mDynamicMat = new Material(this.mMaterial);
			this.mDynamicMat.hideFlags = HideFlags.DontSave;
			this.mDynamicMat.CopyPropertiesFromMaterial(this.mMaterial);
			if (shader != null)
			{
				this.mDynamicMat.shader = shader;
			}
			else if (this.mClipping != UIDrawCall.Clipping.None)
			{
				Debug.LogError(text + " doesn't have a clipped shader version for " + this.mClipping);
				this.mClipping = UIDrawCall.Clipping.None;
			}
		}
		else
		{
			this.mDynamicMat = new Material(shader);
			this.mDynamicMat.hideFlags = HideFlags.DontSave;
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x000229C0 File Offset: 0x00020BC0
	private Material RebuildMaterial()
	{
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.CreateMaterial();
		this.mDynamicMat.renderQueue = this.mRenderQueue;
		this.mLastClip = this.mClipping;
		if (this.mTexture != null)
		{
			this.mDynamicMat.mainTexture = this.mTexture;
		}
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[] { this.mDynamicMat };
		}
		return this.mDynamicMat;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00022A50 File Offset: 0x00020C50
	private void UpdateMaterials()
	{
		if (this.mRebuildMat || this.mDynamicMat == null || this.mClipping != this.mLastClip)
		{
			this.RebuildMaterial();
			this.mRebuildMat = false;
		}
		else if (this.mRenderer.sharedMaterial != this.mDynamicMat)
		{
			this.mRenderer.sharedMaterials = new Material[] { this.mDynamicMat };
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00022AD4 File Offset: 0x00020CD4
	public void Set(BetterList<Vector3> verts, BetterList<Vector3> norms, BetterList<Vector4> tans, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		int size = verts.size;
		if (size > 0 && size == uvs.size && size == cols.size && size % 4 == 0)
		{
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (verts.size < 65000)
			{
				int num = (size >> 1) * 3;
				bool flag = this.mIndices == null || this.mIndices.Length != num;
				if (this.mMesh == null)
				{
					this.mMesh = new Mesh();
					this.mMesh.hideFlags = HideFlags.DontSave;
					this.mMesh.name = ((!(this.mMaterial != null)) ? "Mesh" : this.mMaterial.name);
					this.mMesh.MarkDynamic();
					flag = true;
				}
				bool flag2 = uvs.buffer.Length != verts.buffer.Length || cols.buffer.Length != verts.buffer.Length || (norms != null && norms.buffer.Length != verts.buffer.Length) || (tans != null && tans.buffer.Length != verts.buffer.Length);
				this.mTriangles = verts.size >> 1;
				if (flag2 || verts.buffer.Length > 65000)
				{
					if (flag2 || this.mMesh.vertexCount != verts.size)
					{
						this.mMesh.Clear();
						flag = true;
					}
					this.mMesh.vertices = verts.ToArray();
					this.mMesh.uv = uvs.ToArray();
					this.mMesh.colors32 = cols.ToArray();
					if (norms != null)
					{
						this.mMesh.normals = norms.ToArray();
					}
					if (tans != null)
					{
						this.mMesh.tangents = tans.ToArray();
					}
				}
				else
				{
					if (this.mMesh.vertexCount != verts.buffer.Length)
					{
						this.mMesh.Clear();
						flag = true;
					}
					this.mMesh.vertices = verts.buffer;
					this.mMesh.uv = uvs.buffer;
					this.mMesh.colors32 = cols.buffer;
					if (norms != null)
					{
						this.mMesh.normals = norms.buffer;
					}
					if (tans != null)
					{
						this.mMesh.tangents = tans.buffer;
					}
				}
				if (flag)
				{
					this.mIndices = this.GenerateCachedIndexBuffer(size, num);
					this.mMesh.triangles = this.mIndices;
				}
				if (!this.alwaysOnScreen)
				{
					this.mMesh.RecalculateBounds();
				}
				this.mFilter.mesh = this.mMesh;
			}
			else
			{
				this.mTriangles = 0;
				if (this.mFilter.mesh != null)
				{
					this.mFilter.mesh.Clear();
				}
				Debug.LogError("Too many vertices on one panel: " + verts.size);
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			this.UpdateMaterials();
		}
		else
		{
			if (this.mFilter.mesh != null)
			{
				this.mFilter.mesh.Clear();
			}
			Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00022EB8 File Offset: 0x000210B8
	private int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
	{
		int i = 0;
		int count = UIDrawCall.mCache.Count;
		while (i < count)
		{
			int[] array = UIDrawCall.mCache[i];
			if (array != null && array.Length == indexCount)
			{
				return array;
			}
			i++;
		}
		int[] array2 = new int[indexCount];
		int num = 0;
		for (int j = 0; j < vertexCount; j += 4)
		{
			array2[num++] = j;
			array2[num++] = j + 1;
			array2[num++] = j + 2;
			array2[num++] = j + 2;
			array2[num++] = j + 3;
			array2[num++] = j;
		}
		if (UIDrawCall.mCache.Count > 10)
		{
			UIDrawCall.mCache.RemoveAt(0);
		}
		UIDrawCall.mCache.Add(array2);
		return array2;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00022F94 File Offset: 0x00021194
	private void OnWillRenderObject()
	{
		if (this.mReset)
		{
			this.mReset = false;
			this.UpdateMaterials();
		}
		if (this.mDynamicMat != null && this.isClipped && this.mClipping != UIDrawCall.Clipping.ConstrainButDontClip)
		{
			this.mDynamicMat.mainTextureOffset = new Vector2(-this.mClipRange.x / this.mClipRange.z, -this.mClipRange.y / this.mClipRange.w);
			this.mDynamicMat.mainTextureScale = new Vector2(1f / this.mClipRange.z, 1f / this.mClipRange.w);
			Vector2 vector = new Vector2(1000f, 1000f);
			if (this.mClipSoft.x > 0f)
			{
				vector.x = this.mClipRange.z / this.mClipSoft.x;
			}
			if (this.mClipSoft.y > 0f)
			{
				vector.y = this.mClipRange.w / this.mClipSoft.y;
			}
			this.mDynamicMat.SetVector("_ClipSharpness", vector);
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00005F5C File Offset: 0x0000415C
	private void OnEnable()
	{
		this.mRebuildMat = true;
		UIDrawCall.mActiveList.Add(this);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x000230E0 File Offset: 0x000212E0
	private void OnDisable()
	{
		UIDrawCall.mActiveList.Remove(this);
		UIDrawCall.mInactiveList.Add(this);
		this.depthStart = int.MaxValue;
		this.depthEnd = int.MinValue;
		this.panel = null;
		this.manager = null;
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.mDynamicMat = null;
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00005F70 File Offset: 0x00004170
	private void OnDestroy()
	{
		UIDrawCall.mInactiveList.Remove(this);
		NGUITools.DestroyImmediate(this.mMesh);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0002313C File Offset: 0x0002133C
	public static UIDrawCall Create(int index, UIPanel panel, Material mat, Texture tex, Shader shader)
	{
		if (index < UIDrawCall.mActiveList.size)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList.buffer[index];
			if (uidrawCall != null && uidrawCall.manager == panel && uidrawCall.baseMaterial == mat && uidrawCall.mainTexture == tex && uidrawCall.shader == shader)
			{
				return uidrawCall;
			}
		}
		int i = UIDrawCall.mActiveList.size;
		while (i > index)
		{
			NGUITools.SetActive(UIDrawCall.mActiveList[--i].gameObject, false);
		}
		return UIDrawCall.Create(null, panel, mat, tex, shader);
	}

	// Token: 0x06000397 RID: 919 RVA: 0x000231F8 File Offset: 0x000213F8
	private static UIDrawCall Create(string name, UIPanel pan, Material mat, Texture tex, Shader shader)
	{
		UIDrawCall uidrawCall = UIDrawCall.Create(name);
		uidrawCall.gameObject.layer = pan.cachedGameObject.layer;
		uidrawCall.baseMaterial = mat;
		uidrawCall.mainTexture = tex;
		uidrawCall.shader = shader;
		int num = 0;
		for (int i = 0; i < UIDrawCall.mActiveList.size - 1; i++)
		{
			UIDrawCall uidrawCall2 = UIDrawCall.mActiveList.buffer[i];
			if (num < uidrawCall2.mRenderQueue)
			{
				num = uidrawCall2.mRenderQueue;
			}
		}
		if (pan.renderQueue == UIPanel.RenderQueue.Automatic)
		{
			if (num == 0)
			{
				uidrawCall.renderQueue = ((!(mat != null)) ? 3000 : mat.renderQueue);
			}
			else
			{
				uidrawCall.renderQueue = num + 1;
			}
		}
		else if (pan.renderQueue == UIPanel.RenderQueue.StartAt)
		{
			uidrawCall.renderQueue = Mathf.Max(num + 1, pan.startingRenderQueue);
		}
		else
		{
			uidrawCall.renderQueue = pan.startingRenderQueue;
		}
		uidrawCall.manager = pan;
		return uidrawCall;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x000232F8 File Offset: 0x000214F8
	private static UIDrawCall Create(string name)
	{
		if (UIDrawCall.mInactiveList.size > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList.Pop();
			if (name != null)
			{
				uidrawCall.name = name;
			}
			NGUITools.SetActive(uidrawCall.gameObject, true);
			return uidrawCall;
		}
		GameObject gameObject = new GameObject(name);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject.AddComponent<UIDrawCall>();
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00023350 File Offset: 0x00021550
	public static void ClearAll()
	{
		bool isPlaying = Application.isPlaying;
		int i = UIDrawCall.mActiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[--i];
			if (isPlaying)
			{
				NGUITools.SetActive(uidrawCall.gameObject, false);
			}
			else
			{
				NGUITools.DestroyImmediate(uidrawCall.gameObject);
			}
		}
		UIDrawCall.mActiveList.Clear();
	}

	// Token: 0x0600039A RID: 922 RVA: 0x000233B8 File Offset: 0x000215B8
	public static void ReleaseAll()
	{
		UIDrawCall.ClearAll();
		int i = UIDrawCall.mInactiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList[--i];
			NGUITools.DestroyImmediate(uidrawCall.gameObject);
		}
		UIDrawCall.mInactiveList.Clear();
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00023408 File Offset: 0x00021608
	public static void SetDirty()
	{
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			UIDrawCall.mActiveList[i].isDirty = true;
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00023444 File Offset: 0x00021644
	public static void SetDirty(UIPanel panel)
	{
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[i];
			if (uidrawCall.manager == panel)
			{
				uidrawCall.isDirty = true;
			}
		}
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00023490 File Offset: 0x00021690
	public static int Count(UIPanel panel)
	{
		int num = 0;
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			if (UIDrawCall.mActiveList[i].manager == panel)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000234DC File Offset: 0x000216DC
	public static void Destroy(UIPanel panel)
	{
		int i = UIDrawCall.mActiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[--i];
			if (uidrawCall.manager == panel)
			{
				UIDrawCall.Destroy(uidrawCall);
			}
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x00005F89 File Offset: 0x00004189
	public static void Destroy(UIDrawCall dc)
	{
		if (Application.isPlaying)
		{
			NGUITools.SetActive(dc.gameObject, false);
		}
		else
		{
			NGUITools.DestroyImmediate(dc.gameObject);
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00023528 File Offset: 0x00021728
	public static void Update(UIPanel panel)
	{
		Transform cachedTransform = panel.cachedTransform;
		Vector4 zero = Vector4.zero;
		bool usedForUI = panel.usedForUI;
		if (panel.clipping != UIDrawCall.Clipping.None)
		{
			Vector4 finalClipRegion = panel.finalClipRegion;
			zero = new Vector4(finalClipRegion.x, finalClipRegion.y, finalClipRegion.z * 0.5f, finalClipRegion.w * 0.5f);
		}
		if (zero.z == 0f)
		{
			zero.z = (float)Screen.width * 0.5f;
		}
		if (zero.w == 0f)
		{
			zero.w = (float)Screen.height * 0.5f;
		}
		if (panel.halfPixelOffset)
		{
			zero.x -= 0.5f;
			zero.y += 0.5f;
		}
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[i];
			if (uidrawCall.manager == panel)
			{
				Transform cachedTransform2 = uidrawCall.cachedTransform;
				if (usedForUI)
				{
					Vector4 vector = zero;
					Transform parent = panel.cachedTransform.parent;
					Vector3 vector2 = panel.cachedTransform.localPosition;
					if (parent != null)
					{
						float num = Mathf.Round(vector2.x);
						float num2 = Mathf.Round(vector2.y);
						vector.x += vector2.x - num;
						vector.y += vector2.y - num2;
						vector2.x = num;
						vector2.y = num2;
						vector2 = parent.TransformPoint(vector2);
					}
					cachedTransform2.position = vector2 + panel.drawCallOffset;
					uidrawCall.clipRange = vector;
				}
				else
				{
					cachedTransform2.position = panel.cachedTransform.position;
					uidrawCall.clipRange = zero;
				}
				cachedTransform2.rotation = cachedTransform.rotation;
				cachedTransform2.localScale = cachedTransform.lossyScale;
				uidrawCall.clipping = panel.clipping;
				uidrawCall.clipSoftness = panel.clipSoftness;
			}
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00023754 File Offset: 0x00021954
	public static void UpdateLayer(UIPanel panel)
	{
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[i];
			if (uidrawCall.manager == panel)
			{
				uidrawCall.gameObject.layer = panel.cachedGameObject.layer;
			}
		}
	}

	// Token: 0x040002D2 RID: 722
	private const int maxIndexBufferCache = 10;

	// Token: 0x040002D3 RID: 723
	private static BetterList<UIDrawCall> mActiveList = new BetterList<UIDrawCall>();

	// Token: 0x040002D4 RID: 724
	private static BetterList<UIDrawCall> mInactiveList = new BetterList<UIDrawCall>();

	// Token: 0x040002D5 RID: 725
	[HideInInspector]
	[NonSerialized]
	public int depthStart = int.MaxValue;

	// Token: 0x040002D6 RID: 726
	[HideInInspector]
	[NonSerialized]
	public int depthEnd = int.MinValue;

	// Token: 0x040002D7 RID: 727
	[HideInInspector]
	[NonSerialized]
	public UIPanel manager;

	// Token: 0x040002D8 RID: 728
	[HideInInspector]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x040002D9 RID: 729
	[HideInInspector]
	[NonSerialized]
	public bool alwaysOnScreen;

	// Token: 0x040002DA RID: 730
	private Material mMaterial;

	// Token: 0x040002DB RID: 731
	private Texture mTexture;

	// Token: 0x040002DC RID: 732
	private Shader mShader;

	// Token: 0x040002DD RID: 733
	private UIDrawCall.Clipping mClipping;

	// Token: 0x040002DE RID: 734
	private Vector4 mClipRange;

	// Token: 0x040002DF RID: 735
	private Vector2 mClipSoft;

	// Token: 0x040002E0 RID: 736
	private Transform mTrans;

	// Token: 0x040002E1 RID: 737
	private Mesh mMesh;

	// Token: 0x040002E2 RID: 738
	private MeshFilter mFilter;

	// Token: 0x040002E3 RID: 739
	private MeshRenderer mRenderer;

	// Token: 0x040002E4 RID: 740
	private Material mDynamicMat;

	// Token: 0x040002E5 RID: 741
	private int[] mIndices;

	// Token: 0x040002E6 RID: 742
	private bool mRebuildMat = true;

	// Token: 0x040002E7 RID: 743
	private bool mDirty;

	// Token: 0x040002E8 RID: 744
	private bool mReset = true;

	// Token: 0x040002E9 RID: 745
	private int mRenderQueue = 3000;

	// Token: 0x040002EA RID: 746
	private UIDrawCall.Clipping mLastClip;

	// Token: 0x040002EB RID: 747
	private int mTriangles;

	// Token: 0x040002EC RID: 748
	private static List<int[]> mCache = new List<int[]>(10);

	// Token: 0x0200007B RID: 123
	public enum Clipping
	{
		// Token: 0x040002EE RID: 750
		None,
		// Token: 0x040002EF RID: 751
		AlphaClip = 2,
		// Token: 0x040002F0 RID: 752
		SoftClip,
		// Token: 0x040002F1 RID: 753
		ConstrainButDontClip
	}
}
