using System;
using System.Collections.Generic;
using tk2dRuntime;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000283 RID: 643
[AddComponentMenu("2D Toolkit/TileMap/TileMap")]
[ExecuteInEditMode]
public class tk2dTileMap : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x0600127E RID: 4734 RVA: 0x0000FC45 File Offset: 0x0000DE45
	// (set) Token: 0x0600127F RID: 4735 RVA: 0x0000FC4D File Offset: 0x0000DE4D
	public tk2dSpriteCollectionData Editor__SpriteCollection
	{
		get
		{
			return this.spriteCollection;
		}
		set
		{
			this._spriteCollectionInst = null;
			this.spriteCollection = value;
			if (this.spriteCollection != null)
			{
				this._spriteCollectionInst = this.spriteCollection.inst;
			}
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001280 RID: 4736 RVA: 0x0000FC7F File Offset: 0x0000DE7F
	public tk2dSpriteCollectionData SpriteCollectionInst
	{
		get
		{
			if (this._spriteCollectionInst == null && this.spriteCollection != null)
			{
				this._spriteCollectionInst = this.spriteCollection.inst;
			}
			return this._spriteCollectionInst;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001281 RID: 4737 RVA: 0x0000FCBA File Offset: 0x0000DEBA
	public bool AllowEdit
	{
		get
		{
			return this._inEditMode;
		}
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x00081278 File Offset: 0x0007F478
	private void Awake()
	{
		if (this.spriteCollection != null)
		{
			this._spriteCollectionInst = this.spriteCollection.inst;
		}
		bool flag = true;
		if (this.SpriteCollectionInst && this.SpriteCollectionInst.buildKey != this.spriteCollectionKey)
		{
			flag = false;
		}
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			if ((Application.isPlaying && this._inEditMode) || !flag)
			{
				this.EndEditMode();
			}
			else if (this._spriteCollectionInst != null && this.data != null && this.renderData == null)
			{
				this.Build(tk2dTileMap.BuildFlags.ForceBuild);
			}
		}
		else if (this._inEditMode)
		{
			Debug.LogError("Tilemap " + base.name + " is still in edit mode. Please fix.Building overhead will be significant.");
			this.EndEditMode();
		}
		else if (!flag)
		{
			this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		}
		else if (this._spriteCollectionInst != null && this.data != null && this.renderData == null)
		{
			this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		}
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x0000FCC2 File Offset: 0x0000DEC2
	public void Build()
	{
		this.Build(tk2dTileMap.BuildFlags.Default);
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x0000FCCB File Offset: 0x0000DECB
	public void ForceBuild()
	{
		this.Build(tk2dTileMap.BuildFlags.ForceBuild);
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x000813C8 File Offset: 0x0007F5C8
	private void ClearSpawnedInstances()
	{
		if (this.layers == null)
		{
			return;
		}
		BuilderUtil.HideTileMapPrefabs(this);
		for (int i = 0; i < this.layers.Length; i++)
		{
			Layer layer = this.layers[i];
			for (int j = 0; j < layer.spriteChannel.chunks.Length; j++)
			{
				SpriteChunk spriteChunk = layer.spriteChannel.chunks[j];
				if (!(spriteChunk.gameObject == null))
				{
					Transform transform = spriteChunk.gameObject.transform;
					List<Transform> list = new List<Transform>();
					for (int k = 0; k < transform.childCount; k++)
					{
						list.Add(transform.GetChild(k));
					}
					for (int l = 0; l < list.Count; l++)
					{
						UnityEngine.Object.DestroyImmediate(list[l].gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0000FCD4 File Offset: 0x0000DED4
	private void SetPrefabsRootActive(bool active)
	{
		if (this.prefabsRoot != null)
		{
			this.prefabsRoot.SetActive(active);
		}
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x000814BC File Offset: 0x0007F6BC
	public void Build(tk2dTileMap.BuildFlags buildFlags)
	{
		if (this.spriteCollection != null)
		{
			this._spriteCollectionInst = this.spriteCollection.inst;
		}
		if (this.data != null && this.spriteCollection != null)
		{
			if (this.data.tilePrefabs == null)
			{
				this.data.tilePrefabs = new UnityEngine.Object[this.SpriteCollectionInst.Count];
			}
			else if (this.data.tilePrefabs.Length != this.SpriteCollectionInst.Count)
			{
				Array.Resize<UnityEngine.Object>(ref this.data.tilePrefabs, this.SpriteCollectionInst.Count);
			}
			BuilderUtil.InitDataStore(this);
			if (this.SpriteCollectionInst)
			{
				this.SpriteCollectionInst.InitMaterialIds();
			}
			bool flag = (buildFlags & tk2dTileMap.BuildFlags.ForceBuild) != tk2dTileMap.BuildFlags.Default;
			if (this.SpriteCollectionInst && this.SpriteCollectionInst.buildKey != this.spriteCollectionKey)
			{
				flag = true;
			}
			Dictionary<Layer, bool> dictionary = new Dictionary<Layer, bool>();
			if (this.layers != null)
			{
				for (int i = 0; i < this.layers.Length; i++)
				{
					Layer layer = this.layers[i];
					if (layer != null && layer.gameObject != null)
					{
						dictionary[layer] = layer.gameObject.activeSelf;
					}
				}
			}
			if (flag)
			{
				this.ClearSpawnedInstances();
			}
			BuilderUtil.CreateRenderData(this, this._inEditMode, dictionary);
			RenderMeshBuilder.Build(this, this._inEditMode, flag);
			if (!this._inEditMode)
			{
				ColliderBuilder.Build(this, flag);
				BuilderUtil.SpawnPrefabs(this, flag);
			}
			foreach (Layer layer2 in this.layers)
			{
				layer2.ClearDirtyFlag();
			}
			if (this.colorChannel != null)
			{
				this.colorChannel.ClearDirtyFlag();
			}
			if (this.SpriteCollectionInst)
			{
				this.spriteCollectionKey = this.SpriteCollectionInst.buildKey;
			}
			return;
		}
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x000816D0 File Offset: 0x0007F8D0
	public bool GetTileAtPosition(Vector3 position, out int x, out int y)
	{
		float num;
		float num2;
		bool tileFracAtPosition = this.GetTileFracAtPosition(position, out num, out num2);
		x = (int)num;
		y = (int)num2;
		return tileFracAtPosition;
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x000816F4 File Offset: 0x0007F8F4
	public bool GetTileFracAtPosition(Vector3 position, out float x, out float y)
	{
		tk2dTileMapData.TileType tileType = this.data.tileType;
		if (tileType != tk2dTileMapData.TileType.Rectangular)
		{
			if (tileType == tk2dTileMapData.TileType.Isometric)
			{
				if (this.data.tileSize.x != 0f)
				{
					float num = Mathf.Atan2(this.data.tileSize.y, this.data.tileSize.x / 2f);
					Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
					x = (vector.x - this.data.tileOrigin.x) / this.data.tileSize.x;
					y = (vector.y - this.data.tileOrigin.y) / this.data.tileSize.y;
					float num2 = y * 0.5f;
					int num3 = (int)num2;
					float num4 = num2 - (float)num3;
					float num5 = x % 1f;
					x = (float)((int)x);
					y = (float)(num3 * 2);
					if (num5 > 0.5f)
					{
						if (num4 > 0.5f && Mathf.Atan2(1f - num4, (num5 - 0.5f) * 2f) < num)
						{
							y += 1f;
						}
						else if (num4 < 0.5f && Mathf.Atan2(num4, (num5 - 0.5f) * 2f) < num)
						{
							y -= 1f;
						}
					}
					else if (num5 < 0.5f)
					{
						if (num4 > 0.5f && Mathf.Atan2(num4 - 0.5f, num5 * 2f) > num)
						{
							y += 1f;
							x -= 1f;
						}
						if (num4 < 0.5f && Mathf.Atan2(num4, (0.5f - num5) * 2f) < num)
						{
							y -= 1f;
							x -= 1f;
						}
					}
					return x >= 0f && x <= (float)this.width && y >= 0f && y <= (float)this.height;
				}
			}
			x = 0f;
			y = 0f;
			return false;
		}
		Vector3 vector2 = base.transform.worldToLocalMatrix.MultiplyPoint(position);
		x = (vector2.x - this.data.tileOrigin.x) / this.data.tileSize.x;
		y = (vector2.y - this.data.tileOrigin.y) / this.data.tileSize.y;
		return x >= 0f && x <= (float)this.width && y >= 0f && y <= (float)this.height;
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x000819F0 File Offset: 0x0007FBF0
	public Vector3 GetTilePosition(int x, int y)
	{
		tk2dTileMapData.TileType tileType = this.data.tileType;
		if (tileType == tk2dTileMapData.TileType.Rectangular || tileType != tk2dTileMapData.TileType.Isometric)
		{
			Vector3 vector = new Vector3((float)x * this.data.tileSize.x + this.data.tileOrigin.x, (float)y * this.data.tileSize.y + this.data.tileOrigin.y, 0f);
			return base.transform.localToWorldMatrix.MultiplyPoint(vector);
		}
		Vector3 vector2 = new Vector3(((float)x + (((y & 1) != 0) ? 0.5f : 0f)) * this.data.tileSize.x + this.data.tileOrigin.x, (float)y * this.data.tileSize.y + this.data.tileOrigin.y, 0f);
		return base.transform.localToWorldMatrix.MultiplyPoint(vector2);
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00081B08 File Offset: 0x0007FD08
	public int GetTileIdAtPosition(Vector3 position, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return -1;
		}
		int num;
		int num2;
		if (!this.GetTileAtPosition(position, out num, out num2))
		{
			return -1;
		}
		return this.layers[layer].GetTile(num, num2);
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x0000FCF3 File Offset: 0x0000DEF3
	public TileInfo GetTileInfoForTileId(int tileId)
	{
		return this.data.GetTileInfoForSprite(tileId);
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00081B50 File Offset: 0x0007FD50
	public Color GetInterpolatedColorAtPosition(Vector3 position)
	{
		Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
		int num = (int)((vector.x - this.data.tileOrigin.x) / this.data.tileSize.x);
		int num2 = (int)((vector.y - this.data.tileOrigin.y) / this.data.tileSize.y);
		if (this.colorChannel == null || this.colorChannel.IsEmpty)
		{
			return Color.white;
		}
		if (num < 0 || num >= this.width || num2 < 0 || num2 >= this.height)
		{
			return this.colorChannel.clearColor;
		}
		int num3;
		ColorChunk colorChunk = this.colorChannel.FindChunkAndCoordinate(num, num2, out num3);
		if (colorChunk.Empty)
		{
			return this.colorChannel.clearColor;
		}
		int num4 = this.partitionSizeX + 1;
		Color color = colorChunk.colors[num3];
		Color color2 = colorChunk.colors[num3 + 1];
		Color color3 = colorChunk.colors[num3 + num4];
		Color color4 = colorChunk.colors[num3 + num4 + 1];
		float num5 = (float)num * this.data.tileSize.x + this.data.tileOrigin.x;
		float num6 = (float)num2 * this.data.tileSize.y + this.data.tileOrigin.y;
		float num7 = (vector.x - num5) / this.data.tileSize.x;
		float num8 = (vector.y - num6) / this.data.tileSize.y;
		Color color5 = Color.Lerp(color, color2, num7);
		Color color6 = Color.Lerp(color3, color4, num7);
		return Color.Lerp(color5, color6, num8);
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0000FD01 File Offset: 0x0000DF01
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return spriteCollection == this.spriteCollection || this._spriteCollectionInst == spriteCollection;
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x0000FD23 File Offset: 0x0000DF23
	public void EndEditMode()
	{
		this._inEditMode = false;
		this.SetPrefabsRootActive(true);
		this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		if (this.prefabsRoot != null)
		{
			UnityEngine.Object.DestroyImmediate(this.prefabsRoot);
			this.prefabsRoot = null;
		}
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00003648 File Offset: 0x00001848
	public void TouchMesh(Mesh mesh)
	{
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x0000FD5D File Offset: 0x0000DF5D
	public void DestroyMesh(Mesh mesh)
	{
		UnityEngine.Object.DestroyImmediate(mesh);
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x0000FD65 File Offset: 0x0000DF65
	public int GetTilePrefabsListCount()
	{
		return this.tilePrefabsList.Count;
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06001293 RID: 4755 RVA: 0x0000FD72 File Offset: 0x0000DF72
	public List<tk2dTileMap.TilemapPrefabInstance> TilePrefabsList
	{
		get
		{
			return this.tilePrefabsList;
		}
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x00081D68 File Offset: 0x0007FF68
	public void GetTilePrefabsListItem(int index, out int x, out int y, out int layer, out GameObject instance)
	{
		tk2dTileMap.TilemapPrefabInstance tilemapPrefabInstance = this.tilePrefabsList[index];
		x = tilemapPrefabInstance.x;
		y = tilemapPrefabInstance.y;
		layer = tilemapPrefabInstance.layer;
		instance = tilemapPrefabInstance.instance;
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x00081DA4 File Offset: 0x0007FFA4
	public void SetTilePrefabsList(List<int> xs, List<int> ys, List<int> layers, List<GameObject> instances)
	{
		int count = instances.Count;
		this.tilePrefabsList = new List<tk2dTileMap.TilemapPrefabInstance>(count);
		for (int i = 0; i < count; i++)
		{
			tk2dTileMap.TilemapPrefabInstance tilemapPrefabInstance = new tk2dTileMap.TilemapPrefabInstance();
			tilemapPrefabInstance.x = xs[i];
			tilemapPrefabInstance.y = ys[i];
			tilemapPrefabInstance.layer = layers[i];
			tilemapPrefabInstance.instance = instances[i];
			this.tilePrefabsList.Add(tilemapPrefabInstance);
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001296 RID: 4758 RVA: 0x0000FD7A File Offset: 0x0000DF7A
	// (set) Token: 0x06001297 RID: 4759 RVA: 0x0000FD82 File Offset: 0x0000DF82
	public Layer[] Layers
	{
		get
		{
			return this.layers;
		}
		set
		{
			this.layers = value;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06001298 RID: 4760 RVA: 0x0000FD8B File Offset: 0x0000DF8B
	// (set) Token: 0x06001299 RID: 4761 RVA: 0x0000FD93 File Offset: 0x0000DF93
	public ColorChannel ColorChannel
	{
		get
		{
			return this.colorChannel;
		}
		set
		{
			this.colorChannel = value;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x0600129A RID: 4762 RVA: 0x0000FD9C File Offset: 0x0000DF9C
	// (set) Token: 0x0600129B RID: 4763 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
	public GameObject PrefabsRoot
	{
		get
		{
			return this.prefabsRoot;
		}
		set
		{
			this.prefabsRoot = value;
		}
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0000FDAD File Offset: 0x0000DFAD
	public int GetTile(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return -1;
		}
		return this.layers[layer].GetTile(x, y);
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0000FDD5 File Offset: 0x0000DFD5
	public tk2dTileFlags GetTileFlags(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return tk2dTileFlags.None;
		}
		return this.layers[layer].GetTileFlags(x, y);
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0000FDFD File Offset: 0x0000DFFD
	public void SetTile(int x, int y, int layer, int tile)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].SetTile(x, y, tile);
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0000FE26 File Offset: 0x0000E026
	public void SetTileFlags(int x, int y, int layer, tk2dTileFlags flags)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].SetTileFlags(x, y, flags);
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0000FE4F File Offset: 0x0000E04F
	public void ClearTile(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].ClearTile(x, y);
	}

	// Token: 0x04001471 RID: 5233
	public string editorDataGUID = string.Empty;

	// Token: 0x04001472 RID: 5234
	public tk2dTileMapData data;

	// Token: 0x04001473 RID: 5235
	public GameObject renderData;

	// Token: 0x04001474 RID: 5236
	[SerializeField]
	private tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04001475 RID: 5237
	private tk2dSpriteCollectionData _spriteCollectionInst;

	// Token: 0x04001476 RID: 5238
	[SerializeField]
	private int spriteCollectionKey;

	// Token: 0x04001477 RID: 5239
	public int width = 128;

	// Token: 0x04001478 RID: 5240
	public int height = 128;

	// Token: 0x04001479 RID: 5241
	public int partitionSizeX = 32;

	// Token: 0x0400147A RID: 5242
	public int partitionSizeY = 32;

	// Token: 0x0400147B RID: 5243
	[SerializeField]
	private Layer[] layers;

	// Token: 0x0400147C RID: 5244
	[SerializeField]
	private ColorChannel colorChannel;

	// Token: 0x0400147D RID: 5245
	[SerializeField]
	private GameObject prefabsRoot;

	// Token: 0x0400147E RID: 5246
	[SerializeField]
	private List<tk2dTileMap.TilemapPrefabInstance> tilePrefabsList = new List<tk2dTileMap.TilemapPrefabInstance>();

	// Token: 0x0400147F RID: 5247
	[SerializeField]
	private bool _inEditMode;

	// Token: 0x04001480 RID: 5248
	public string serializedMeshPath;

	// Token: 0x02000284 RID: 644
	[Serializable]
	public class TilemapPrefabInstance
	{
		// Token: 0x04001481 RID: 5249
		public int x;

		// Token: 0x04001482 RID: 5250
		public int y;

		// Token: 0x04001483 RID: 5251
		public int layer;

		// Token: 0x04001484 RID: 5252
		public GameObject instance;
	}

	// Token: 0x02000285 RID: 645
	[Flags]
	public enum BuildFlags
	{
		// Token: 0x04001486 RID: 5254
		Default = 0,
		// Token: 0x04001487 RID: 5255
		EditMode = 1,
		// Token: 0x04001488 RID: 5256
		ForceBuild = 2
	}
}
