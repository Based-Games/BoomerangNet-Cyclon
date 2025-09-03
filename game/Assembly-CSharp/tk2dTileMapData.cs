using System;
using System.Collections.Generic;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class tk2dTileMapData : ScriptableObject
{
	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060012F2 RID: 4850 RVA: 0x0001016C File Offset: 0x0000E36C
	public int NumLayers
	{
		get
		{
			if (this.tileMapLayers == null || this.tileMapLayers.Count == 0)
			{
				this.InitLayers();
			}
			return this.tileMapLayers.Count;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0001019A File Offset: 0x0000E39A
	public LayerInfo[] Layers
	{
		get
		{
			if (this.tileMapLayers == null || this.tileMapLayers.Count == 0)
			{
				this.InitLayers();
			}
			return this.tileMapLayers.ToArray();
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x000101C8 File Offset: 0x0000E3C8
	public TileInfo GetTileInfoForSprite(int tileId)
	{
		if (this.tileInfo == null || tileId < 0 || tileId >= this.tileInfo.Length)
		{
			return null;
		}
		return this.tileInfo[tileId];
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00084054 File Offset: 0x00082254
	public TileInfo[] GetOrCreateTileInfo(int numTiles)
	{
		bool flag = false;
		if (this.tileInfo == null)
		{
			this.tileInfo = new TileInfo[numTiles];
			flag = true;
		}
		else if (this.tileInfo.Length != numTiles)
		{
			Array.Resize<TileInfo>(ref this.tileInfo, numTiles);
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < this.tileInfo.Length; i++)
			{
				if (this.tileInfo[i] == null)
				{
					this.tileInfo[i] = new TileInfo();
				}
			}
		}
		return this.tileInfo;
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x000840DC File Offset: 0x000822DC
	public void GetTileOffset(out float x, out float y)
	{
		tk2dTileMapData.TileType tileType = this.tileType;
		if (tileType != tk2dTileMapData.TileType.Rectangular)
		{
			if (tileType == tk2dTileMapData.TileType.Isometric)
			{
				x = 0.5f;
				y = 0f;
				return;
			}
		}
		x = 0f;
		y = 0f;
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x00084128 File Offset: 0x00082328
	private void InitLayers()
	{
		this.tileMapLayers = new List<LayerInfo>();
		LayerInfo layerInfo = new LayerInfo();
		layerInfo = new LayerInfo();
		layerInfo.name = "Layer 0";
		layerInfo.hash = 1892887448;
		layerInfo.z = 0f;
		this.tileMapLayers.Add(layerInfo);
	}

	// Token: 0x040014B7 RID: 5303
	public Vector3 tileSize;

	// Token: 0x040014B8 RID: 5304
	public Vector3 tileOrigin;

	// Token: 0x040014B9 RID: 5305
	public tk2dTileMapData.TileType tileType;

	// Token: 0x040014BA RID: 5306
	public tk2dTileMapData.SortMethod sortMethod;

	// Token: 0x040014BB RID: 5307
	public bool layersFixedZ;

	// Token: 0x040014BC RID: 5308
	public UnityEngine.Object[] tilePrefabs = new UnityEngine.Object[0];

	// Token: 0x040014BD RID: 5309
	[SerializeField]
	private TileInfo[] tileInfo = new TileInfo[0];

	// Token: 0x040014BE RID: 5310
	[SerializeField]
	public List<LayerInfo> tileMapLayers = new List<LayerInfo>();

	// Token: 0x02000293 RID: 659
	public enum SortMethod
	{
		// Token: 0x040014C0 RID: 5312
		BottomLeft,
		// Token: 0x040014C1 RID: 5313
		TopLeft,
		// Token: 0x040014C2 RID: 5314
		BottomRight,
		// Token: 0x040014C3 RID: 5315
		TopRight
	}

	// Token: 0x02000294 RID: 660
	public enum TileType
	{
		// Token: 0x040014C5 RID: 5317
		Rectangular,
		// Token: 0x040014C6 RID: 5318
		Isometric
	}
}
