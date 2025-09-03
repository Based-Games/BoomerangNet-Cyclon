using System;
using System.Collections.Generic;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000295 RID: 661
	public static class RenderMeshBuilder
	{
		// Token: 0x060012F8 RID: 4856 RVA: 0x0008417C File Offset: 0x0008237C
		public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, ColorChunk colorChunk, bool useColor, bool skipPrefabs, int baseX, int baseY)
		{
			List<Vector3> list = new List<Vector3>();
			List<Color> list2 = new List<Color>();
			List<Vector2> list3 = new List<Vector2>();
			int[] spriteIds = chunk.spriteIds;
			Vector3 tileSize = tileMap.data.tileSize;
			int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
			UnityEngine.Object[] tilePrefabs = tileMap.data.tilePrefabs;
			Color32 color = ((!useColor || tileMap.ColorChannel == null) ? Color.white : tileMap.ColorChannel.clearColor);
			if (colorChunk == null || colorChunk.colors.Length == 0)
			{
				useColor = false;
			}
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			int num7;
			BuilderUtil.GetLoopOrder(tileMap.data.sortMethod, tileMap.partitionSizeX, tileMap.partitionSizeY, out num2, out num3, out num4, out num5, out num6, out num7);
			float num8 = 0f;
			float num9 = 0f;
			tileMap.data.GetTileOffset(out num8, out num9);
			List<int>[] array = new List<int>[tileMap.SpriteCollectionInst.materials.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<int>();
			}
			int num10 = tileMap.partitionSizeX + 1;
			for (int num11 = num5; num11 != num6; num11 += num7)
			{
				float num12 = (float)((baseY + num11) & 1) * num8;
				for (int num13 = num2; num13 != num3; num13 += num4)
				{
					int num14 = spriteIds[num11 * tileMap.partitionSizeX + num13];
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(num14);
					bool flag = BuilderUtil.IsRawTileFlagSet(num14, tk2dTileFlags.FlipX);
					bool flag2 = BuilderUtil.IsRawTileFlagSet(num14, tk2dTileFlags.FlipY);
					bool flag3 = BuilderUtil.IsRawTileFlagSet(num14, tk2dTileFlags.Rot90);
					Vector3 vector = new Vector3(tileSize.x * ((float)num13 + num12), tileSize.y * (float)num11, 0f);
					if (tileFromRawTile >= 0 && tileFromRawTile < num)
					{
						if (!skipPrefabs || !tilePrefabs[tileFromRawTile])
						{
							tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
							int count = list.Count;
							for (int j = 0; j < tk2dSpriteDefinition.positions.Length; j++)
							{
								Vector3 vector2 = BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, tk2dSpriteDefinition.positions[j], flag, flag2, flag3);
								if (useColor)
								{
									Color color2 = colorChunk.colors[num11 * num10 + num13];
									Color color3 = colorChunk.colors[num11 * num10 + num13 + 1];
									Color color4 = colorChunk.colors[(num11 + 1) * num10 + num13];
									Color color5 = colorChunk.colors[(num11 + 1) * num10 + (num13 + 1)];
									Vector3 vector3 = vector2 - tk2dSpriteDefinition.untrimmedBoundsData[0];
									Vector3 vector4 = vector3 + tileMap.data.tileSize * 0.5f;
									float num15 = Mathf.Clamp01(vector4.x / tileMap.data.tileSize.x);
									float num16 = Mathf.Clamp01(vector4.y / tileMap.data.tileSize.y);
									Color color6 = Color.Lerp(Color.Lerp(color2, color3, num15), Color.Lerp(color4, color5, num15), num16);
									list2.Add(color6);
								}
								else
								{
									list2.Add(color);
								}
								list.Add(vector + vector2);
								list3.Add(tk2dSpriteDefinition.uvs[j]);
							}
							bool flag4 = false;
							if (flag)
							{
								flag4 = !flag4;
							}
							if (flag2)
							{
								flag4 = !flag4;
							}
							List<int> list4 = array[tk2dSpriteDefinition.materialId];
							for (int k = 0; k < tk2dSpriteDefinition.indices.Length; k++)
							{
								int num17 = ((!flag4) ? k : (tk2dSpriteDefinition.indices.Length - 1 - k));
								list4.Add(count + tk2dSpriteDefinition.indices[num17]);
							}
						}
					}
				}
			}
			if (chunk.mesh == null)
			{
				chunk.mesh = new Mesh();
			}
			chunk.mesh.vertices = list.ToArray();
			chunk.mesh.uv = list3.ToArray();
			chunk.mesh.colors = list2.ToArray();
			List<Material> list5 = new List<Material>();
			int num18 = 0;
			int num19 = 0;
			foreach (List<int> list6 in array)
			{
				if (list6.Count > 0)
				{
					list5.Add(tileMap.SpriteCollectionInst.materials[num18]);
					num19++;
				}
				num18++;
			}
			if (num19 > 0)
			{
				chunk.mesh.subMeshCount = num19;
				chunk.gameObject.renderer.materials = list5.ToArray();
				int num20 = 0;
				foreach (List<int> list7 in array)
				{
					if (list7.Count > 0)
					{
						chunk.mesh.SetTriangles(list7.ToArray(), num20);
						num20++;
					}
				}
			}
			chunk.mesh.RecalculateBounds();
			MeshFilter component = chunk.gameObject.GetComponent<MeshFilter>();
			component.sharedMesh = chunk.mesh;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x000846F0 File Offset: 0x000828F0
		public static void Build(tk2dTileMap tileMap, bool editMode, bool forceBuild)
		{
			bool flag = !editMode;
			bool flag2 = !forceBuild;
			int numLayers = tileMap.data.NumLayers;
			for (int i = 0; i < numLayers; i++)
			{
				Layer layer = tileMap.Layers[i];
				if (!layer.IsEmpty)
				{
					LayerInfo layerInfo = tileMap.data.Layers[i];
					bool flag3 = !tileMap.ColorChannel.IsEmpty && tileMap.data.Layers[i].useColor;
					for (int j = 0; j < layer.numRows; j++)
					{
						int num = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int num2 = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							ColorChunk chunk2 = tileMap.ColorChannel.GetChunk(k, j);
							bool flag4 = chunk2 != null && chunk2.Dirty;
							if (!flag2 || flag4 || chunk.Dirty)
							{
								if (chunk.mesh != null)
								{
									chunk.mesh.Clear();
								}
								if (!chunk.IsEmpty)
								{
									if (editMode || (!editMode && !layerInfo.skipMeshGeneration))
									{
										RenderMeshBuilder.BuildForChunk(tileMap, chunk, chunk2, flag3, flag, num2, num);
									}
									if (chunk.mesh != null)
									{
										tileMap.TouchMesh(chunk.mesh);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
