﻿using System;
using UnityEngine;

// Token: 0x0200027B RID: 635
public static class tk2dSpriteGeomGen
{
	// Token: 0x0600122E RID: 4654 RVA: 0x0007D6E0 File Offset: 0x0007B8E0
	public static void SetSpriteColors(Color32[] dest, int offset, int numVertices, Color c, bool premulAlpha)
	{
		if (premulAlpha)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		Color32 color = c;
		for (int i = 0; i < numVertices; i++)
		{
			dest[offset + i] = color;
		}
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x0007D75C File Offset: 0x0007B95C
	public static Vector2 GetAnchorOffset(tk2dBaseSprite.Anchor anchor, float width, float height)
	{
		Vector2 zero = Vector2.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero.x = (float)((int)(width / 2f));
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			zero.x = (float)((int)width);
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.LowerRight:
			zero.y = (float)((int)height);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero.y = (float)((int)(height / 2f));
			break;
		}
		return zero;
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x0000F8BF File Offset: 0x0000DABF
	public static void GetSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		numVertices = spriteDef.positions.Length;
		numIndices = spriteDef.indices.Length;
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x0007D824 File Offset: 0x0007BA24
	public static void SetSpriteGeom(Vector3[] pos, Vector2[] uv, Vector3[] norm, Vector4[] tang, int offset, tk2dSpriteDefinition spriteDef, Vector3 scale)
	{
		for (int i = 0; i < spriteDef.positions.Length; i++)
		{
			pos[offset + i] = Vector3.Scale(spriteDef.positions[i], scale);
		}
		for (int j = 0; j < spriteDef.uvs.Length; j++)
		{
			uv[offset + j] = spriteDef.uvs[j];
		}
		if (norm != null && spriteDef.normals != null)
		{
			for (int k = 0; k < spriteDef.normals.Length; k++)
			{
				norm[offset + k] = spriteDef.normals[k];
			}
		}
		if (tang != null && spriteDef.tangents != null)
		{
			for (int l = 0; l < spriteDef.tangents.Length; l++)
			{
				tang[offset + l] = spriteDef.tangents[l];
			}
		}
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x0007D948 File Offset: 0x0007BB48
	public static void SetSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		for (int i = 0; i < spriteDef.indices.Length; i++)
		{
			indices[offset + i] = vStart + spriteDef.indices[i];
		}
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x0000F8D5 File Offset: 0x0000DAD5
	public static void GetClippedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		if (spriteDef.positions.Length == 4)
		{
			numVertices = 4;
			numIndices = 6;
		}
		else
		{
			numVertices = 0;
			numIndices = 0;
		}
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x0007D980 File Offset: 0x0007BB80
	public static void SetClippedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 clipBottomLeft, Vector2 clipTopRight, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		if (spriteDef.positions.Length == 4)
		{
			Vector3 vector = spriteDef.untrimmedBoundsData[0] - spriteDef.untrimmedBoundsData[1] * 0.5f;
			Vector3 vector2 = spriteDef.untrimmedBoundsData[0] + spriteDef.untrimmedBoundsData[1] * 0.5f;
			float num = Mathf.Lerp(vector.x, vector2.x, clipBottomLeft.x);
			float num2 = Mathf.Lerp(vector.x, vector2.x, clipTopRight.x);
			float num3 = Mathf.Lerp(vector.y, vector2.y, clipBottomLeft.y);
			float num4 = Mathf.Lerp(vector.y, vector2.y, clipTopRight.y);
			Vector3 vector3 = spriteDef.boundsData[1];
			Vector3 vector4 = spriteDef.boundsData[0] - vector3 * 0.5f;
			float num5 = (num - vector4.x) / vector3.x;
			float num6 = (num2 - vector4.x) / vector3.x;
			float num7 = (num3 - vector4.y) / vector3.y;
			float num8 = (num4 - vector4.y) / vector3.y;
			Vector2 vector5 = new Vector2(Mathf.Clamp01(num5), Mathf.Clamp01(num7));
			Vector2 vector6 = new Vector2(Mathf.Clamp01(num6), Mathf.Clamp01(num8));
			Vector3 vector7 = spriteDef.positions[0];
			Vector3 vector8 = spriteDef.positions[3];
			Vector3 vector9 = new Vector3(Mathf.Lerp(vector7.x, vector8.x, vector5.x) * scale.x, Mathf.Lerp(vector7.y, vector8.y, vector5.y) * scale.y, vector7.z * scale.z);
			Vector3 vector10 = new Vector3(Mathf.Lerp(vector7.x, vector8.x, vector6.x) * scale.x, Mathf.Lerp(vector7.y, vector8.y, vector6.y) * scale.y, vector7.z * scale.z);
			boundsCenter.Set(vector9.x + (vector10.x - vector9.x) * 0.5f, vector9.y + (vector10.y - vector9.y) * 0.5f, colliderOffsetZ);
			boundsExtents.Set((vector10.x - vector9.x) * 0.5f, (vector10.y - vector9.y) * 0.5f, colliderExtentZ);
			pos[offset] = new Vector3(vector9.x, vector9.y, vector9.z);
			pos[offset + 1] = new Vector3(vector10.x, vector9.y, vector9.z);
			pos[offset + 2] = new Vector3(vector9.x, vector10.y, vector9.z);
			pos[offset + 3] = new Vector3(vector10.x, vector10.y, vector9.z);
			if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
			{
				Vector2 vector11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
				Vector2 vector12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector6.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector6.x));
				uv[offset] = new Vector2(vector11.x, vector11.y);
				uv[offset + 1] = new Vector2(vector11.x, vector12.y);
				uv[offset + 2] = new Vector2(vector12.x, vector11.y);
				uv[offset + 3] = new Vector2(vector12.x, vector12.y);
			}
			else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
			{
				Vector2 vector13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
				Vector2 vector14 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector6.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector6.x));
				uv[offset] = new Vector2(vector13.x, vector13.y);
				uv[offset + 2] = new Vector2(vector14.x, vector13.y);
				uv[offset + 1] = new Vector2(vector13.x, vector14.y);
				uv[offset + 3] = new Vector2(vector14.x, vector14.y);
			}
			else
			{
				Vector2 vector15 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.y));
				Vector2 vector16 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector6.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector6.y));
				uv[offset] = new Vector2(vector15.x, vector15.y);
				uv[offset + 1] = new Vector2(vector16.x, vector15.y);
				uv[offset + 2] = new Vector2(vector15.x, vector16.y);
				uv[offset + 3] = new Vector2(vector16.x, vector16.y);
			}
		}
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x0000F8F6 File Offset: 0x0000DAF6
	public static void SetClippedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		if (spriteDef.positions.Length == 4)
		{
			indices[offset] = vStart;
			indices[offset + 1] = vStart + 3;
			indices[offset + 2] = vStart + 1;
			indices[offset + 3] = vStart + 2;
			indices[offset + 4] = vStart + 3;
			indices[offset + 5] = vStart;
		}
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x0000F930 File Offset: 0x0000DB30
	public static void GetSlicedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, bool borderOnly)
	{
		if (spriteDef.positions.Length == 4)
		{
			numVertices = 16;
			numIndices = ((!borderOnly) ? 54 : 48);
		}
		else
		{
			numVertices = 0;
			numIndices = 0;
		}
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x0007E140 File Offset: 0x0007C340
	public static void SetSlicedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		if (spriteDef.positions.Length == 4)
		{
			float x = spriteDef.texelSize.x;
			float y = spriteDef.texelSize.y;
			Vector3[] positions = spriteDef.positions;
			float num = positions[1].x - positions[0].x;
			float num2 = positions[2].y - positions[0].y;
			float num3 = borderTopRight.y * num2;
			float num4 = borderBottomLeft.y * num2;
			float num5 = borderTopRight.x * num;
			float num6 = borderBottomLeft.x * num;
			float num7 = dimensions.x * x;
			float num8 = dimensions.y * y;
			float num9 = 0f;
			float num10 = 0f;
			switch (anchor)
			{
			case tk2dBaseSprite.Anchor.LowerCenter:
			case tk2dBaseSprite.Anchor.MiddleCenter:
			case tk2dBaseSprite.Anchor.UpperCenter:
				num9 = (float)(-(float)((int)(dimensions.x / 2f)));
				break;
			case tk2dBaseSprite.Anchor.LowerRight:
			case tk2dBaseSprite.Anchor.MiddleRight:
			case tk2dBaseSprite.Anchor.UpperRight:
				num9 = (float)(-(float)((int)dimensions.x));
				break;
			}
			switch (anchor)
			{
			case tk2dBaseSprite.Anchor.MiddleLeft:
			case tk2dBaseSprite.Anchor.MiddleCenter:
			case tk2dBaseSprite.Anchor.MiddleRight:
				num10 = (float)(-(float)((int)(dimensions.y / 2f)));
				break;
			case tk2dBaseSprite.Anchor.UpperLeft:
			case tk2dBaseSprite.Anchor.UpperCenter:
			case tk2dBaseSprite.Anchor.UpperRight:
				num10 = (float)(-(float)((int)dimensions.y));
				break;
			}
			num9 *= x;
			num10 *= y;
			boundsCenter.Set(scale.x * (num7 * 0.5f + num9), scale.y * (num8 * 0.5f + num10), colliderOffsetZ);
			boundsExtents.Set(scale.x * (num7 * 0.5f), scale.y * (num8 * 0.5f), colliderExtentZ);
			Vector2[] uvs = spriteDef.uvs;
			Vector2 vector = uvs[1] - uvs[0];
			Vector2 vector2 = uvs[2] - uvs[0];
			Vector3 vector3 = new Vector3(num9, num10, 0f);
			Vector3[] array = new Vector3[]
			{
				vector3,
				vector3 + new Vector3(0f, num4, 0f),
				vector3 + new Vector3(0f, num8 - num3, 0f),
				vector3 + new Vector3(0f, num8, 0f)
			};
			Vector2[] array2 = new Vector2[]
			{
				uvs[0],
				uvs[0] + vector2 * borderBottomLeft.y,
				uvs[0] + vector2 * (1f - borderTopRight.y),
				uvs[0] + vector2
			};
			for (int i = 0; i < 4; i++)
			{
				pos[offset + i * 4] = array[i];
				pos[offset + i * 4 + 1] = array[i] + new Vector3(num6, 0f, 0f);
				pos[offset + i * 4 + 2] = array[i] + new Vector3(num7 - num5, 0f, 0f);
				pos[offset + i * 4 + 3] = array[i] + new Vector3(num7, 0f, 0f);
				for (int j = 0; j < 4; j++)
				{
					pos[offset + i * 4 + j] = Vector3.Scale(pos[offset + i * 4 + j], scale);
				}
				uv[offset + i * 4] = array2[i];
				uv[offset + i * 4 + 1] = array2[i] + vector * borderBottomLeft.x;
				uv[offset + i * 4 + 2] = array2[i] + vector * (1f - borderTopRight.x);
				uv[offset + i * 4 + 3] = array2[i] + vector;
			}
		}
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x0007E698 File Offset: 0x0007C898
	public static void SetSlicedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, bool borderOnly)
	{
		if (spriteDef.positions.Length == 4)
		{
			int[] array = new int[]
			{
				0, 4, 1, 1, 4, 5, 1, 5, 2, 2,
				5, 6, 2, 6, 3, 3, 6, 7, 4, 8,
				5, 5, 8, 9, 6, 10, 7, 7, 10, 11,
				8, 12, 9, 9, 12, 13, 9, 13, 10, 10,
				13, 14, 10, 14, 11, 11, 14, 15, 5, 9,
				6, 6, 9, 10
			};
			int num = array.Length;
			if (borderOnly)
			{
				num -= 6;
			}
			for (int i = 0; i < num; i++)
			{
				indices[offset + i] = vStart + array[i];
			}
		}
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x0007E6F4 File Offset: 0x0007C8F4
	public static void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsData[1].x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsData[1].y);
		numVertices = num * num2 * 4;
		numIndices = num * num2 * 6;
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0007E768 File Offset: 0x0007C968
	public static void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsData[1].x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsData[1].y);
		Vector2 vector = new Vector2(dimensions.x * spriteDef.texelSize.x * scale.x, dimensions.y * spriteDef.texelSize.y * scale.y);
		Vector3 vector2 = Vector3.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector2.x = -(vector.x / 2f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.x = -vector.x;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector2.y = -(vector.y / 2f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.y = -vector.y;
			break;
		}
		Vector3 vector3 = vector2;
		vector2 -= Vector3.Scale(spriteDef.positions[0], scale);
		boundsCenter.Set(vector.x * 0.5f + vector3.x, vector.y * 0.5f + vector3.y, colliderOffsetZ);
		boundsExtents.Set(vector.x * 0.5f, vector.y * 0.5f, colliderExtentZ);
		int num3 = 0;
		Vector3 vector4 = Vector3.Scale(spriteDef.untrimmedBoundsData[1], scale);
		Vector3 zero = Vector3.zero;
		Vector3 vector5 = zero;
		for (int i = 0; i < num2; i++)
		{
			vector5.x = zero.x;
			for (int j = 0; j < num; j++)
			{
				float num4 = 1f;
				float num5 = 1f;
				if (Mathf.Abs(vector5.x + vector4.x) > Mathf.Abs(vector.x))
				{
					num4 = vector.x % vector4.x / vector4.x;
				}
				if (Mathf.Abs(vector5.y + vector4.y) > Mathf.Abs(vector.y))
				{
					num5 = vector.y % vector4.y / vector4.y;
				}
				Vector3 vector6 = vector5 + vector2;
				if (num4 != 1f || num5 != 1f)
				{
					Vector2 zero2 = Vector2.zero;
					Vector2 vector7 = new Vector2(num4, num5);
					Vector3 vector8 = new Vector3(Mathf.Lerp(spriteDef.positions[0].x, spriteDef.positions[3].x, zero2.x) * scale.x, Mathf.Lerp(spriteDef.positions[0].y, spriteDef.positions[3].y, zero2.y) * scale.y, spriteDef.positions[0].z * scale.z);
					Vector3 vector9 = new Vector3(Mathf.Lerp(spriteDef.positions[0].x, spriteDef.positions[3].x, vector7.x) * scale.x, Mathf.Lerp(spriteDef.positions[0].y, spriteDef.positions[3].y, vector7.y) * scale.y, spriteDef.positions[0].z * scale.z);
					pos[offset + num3] = vector6 + new Vector3(vector8.x, vector8.y, vector8.z);
					pos[offset + num3 + 1] = vector6 + new Vector3(vector9.x, vector8.y, vector8.z);
					pos[offset + num3 + 2] = vector6 + new Vector3(vector8.x, vector9.y, vector8.z);
					pos[offset + num3 + 3] = vector6 + new Vector3(vector9.x, vector9.y, vector8.z);
					if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
					{
						Vector2 vector10 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.x));
						Vector2 vector11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector7.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector7.x));
						uv[offset + num3] = new Vector2(vector10.x, vector10.y);
						uv[offset + num3 + 1] = new Vector2(vector10.x, vector11.y);
						uv[offset + num3 + 2] = new Vector2(vector11.x, vector10.y);
						uv[offset + num3 + 3] = new Vector2(vector11.x, vector11.y);
					}
					else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
					{
						Vector2 vector12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.x));
						Vector2 vector13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector7.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector7.x));
						uv[offset + num3] = new Vector2(vector12.x, vector12.y);
						uv[offset + num3 + 2] = new Vector2(vector13.x, vector12.y);
						uv[offset + num3 + 1] = new Vector2(vector12.x, vector13.y);
						uv[offset + num3 + 3] = new Vector2(vector13.x, vector13.y);
					}
					else
					{
						Vector2 vector14 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.y));
						Vector2 vector15 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector7.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector7.y));
						uv[offset + num3] = new Vector2(vector14.x, vector14.y);
						uv[offset + num3 + 1] = new Vector2(vector15.x, vector14.y);
						uv[offset + num3 + 2] = new Vector2(vector14.x, vector15.y);
						uv[offset + num3 + 3] = new Vector2(vector15.x, vector15.y);
					}
				}
				else
				{
					pos[offset + num3] = vector6 + Vector3.Scale(spriteDef.positions[0], scale);
					pos[offset + num3 + 1] = vector6 + Vector3.Scale(spriteDef.positions[1], scale);
					pos[offset + num3 + 2] = vector6 + Vector3.Scale(spriteDef.positions[2], scale);
					pos[offset + num3 + 3] = vector6 + Vector3.Scale(spriteDef.positions[3], scale);
					uv[offset + num3] = spriteDef.uvs[0];
					uv[offset + num3 + 1] = spriteDef.uvs[1];
					uv[offset + num3 + 2] = spriteDef.uvs[2];
					uv[offset + num3 + 3] = spriteDef.uvs[3];
				}
				num3 += 4;
				vector5.x += vector4.x;
			}
			vector5.y += vector4.y;
		}
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x0007F238 File Offset: 0x0007D438
	public static void SetTiledSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num;
		int num2;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, spriteDef, dimensions);
		int num3 = 0;
		for (int i = 0; i < num2; i += 6)
		{
			indices[offset + i] = vStart + spriteDef.indices[0] + num3;
			indices[offset + i + 1] = vStart + spriteDef.indices[1] + num3;
			indices[offset + i + 2] = vStart + spriteDef.indices[2] + num3;
			indices[offset + i + 3] = vStart + spriteDef.indices[3] + num3;
			indices[offset + i + 4] = vStart + spriteDef.indices[4] + num3;
			indices[offset + i + 5] = vStart + spriteDef.indices[5] + num3;
			num3 += 4;
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0007F2DC File Offset: 0x0007D4DC
	public static void SetBoxMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, Vector3 origin, Vector3 extents, Matrix4x4 mat, Vector3 baseScale)
	{
		tk2dSpriteGeomGen.boxScaleMatrix.m03 = origin.x * baseScale.x;
		tk2dSpriteGeomGen.boxScaleMatrix.m13 = origin.y * baseScale.y;
		tk2dSpriteGeomGen.boxScaleMatrix.m23 = origin.z * baseScale.z;
		tk2dSpriteGeomGen.boxScaleMatrix.m00 = extents.x * baseScale.x;
		tk2dSpriteGeomGen.boxScaleMatrix.m11 = extents.y * baseScale.y;
		tk2dSpriteGeomGen.boxScaleMatrix.m22 = extents.z * baseScale.z;
		Matrix4x4 matrix4x = mat * tk2dSpriteGeomGen.boxScaleMatrix;
		for (int i = 0; i < 8; i++)
		{
			pos[posOffset + i] = matrix4x.MultiplyPoint(tk2dSpriteGeomGen.boxUnitVertices[i]);
		}
		float num = mat.m00 * mat.m11 * mat.m22 * baseScale.x * baseScale.y * baseScale.z;
		int[] array = ((num < 0f) ? tk2dSpriteGeomGen.boxIndicesBack : tk2dSpriteGeomGen.boxIndicesFwd);
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x0007F434 File Offset: 0x0007D634
	public static void SetSpriteDefinitionMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, tk2dSpriteDefinition spriteDef, Matrix4x4 mat, Vector3 baseScale)
	{
		for (int i = 0; i < spriteDef.colliderVertices.Length; i++)
		{
			Vector3 vector = Vector3.Scale(spriteDef.colliderVertices[i], baseScale);
			vector = mat.MultiplyPoint(vector);
			pos[posOffset + i] = vector;
		}
		float num = mat.m00 * mat.m11 * mat.m22;
		int[] array = ((num < 0f) ? spriteDef.colliderIndicesBack : spriteDef.colliderIndicesFwd);
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0007F4EC File Offset: 0x0007D6EC
	public static void SetSpriteVertexNormals(Vector3[] pos, Vector3 pMin, Vector3 pMax, Vector3[] spriteDefNormals, Vector4[] spriteDefTangents, Vector3[] normals, Vector4[] tangents)
	{
		Vector3 vector = pMax - pMin;
		int num = pos.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector2 = pos[i];
			float num2 = (vector2.x - pMin.x) / vector.x;
			float num3 = (vector2.y - pMin.y) / vector.y;
			float num4 = (1f - num2) * (1f - num3);
			float num5 = num2 * (1f - num3);
			float num6 = (1f - num2) * num3;
			float num7 = num2 * num3;
			if (spriteDefNormals != null && spriteDefNormals.Length == 4 && i < normals.Length)
			{
				normals[i] = spriteDefNormals[0] * num4 + spriteDefNormals[1] * num5 + spriteDefNormals[2] * num6 + spriteDefNormals[3] * num7;
			}
			if (spriteDefTangents != null && spriteDefTangents.Length == 4 && i < tangents.Length)
			{
				tangents[i] = spriteDefTangents[0] * num4 + spriteDefTangents[1] * num5 + spriteDefTangents[2] * num6 + spriteDefTangents[3] * num7;
			}
		}
	}

	// Token: 0x0400142C RID: 5164
	private static readonly int[] boxIndicesBack = new int[]
	{
		0, 1, 2, 2, 1, 3, 6, 5, 4, 7,
		5, 6, 3, 7, 6, 2, 3, 6, 4, 5,
		1, 4, 1, 0, 6, 4, 0, 6, 0, 2,
		1, 7, 3, 5, 7, 1
	};

	// Token: 0x0400142D RID: 5165
	private static readonly int[] boxIndicesFwd = new int[]
	{
		2, 1, 0, 3, 1, 2, 4, 5, 6, 6,
		5, 7, 6, 7, 3, 6, 3, 2, 1, 5,
		4, 0, 1, 4, 0, 4, 6, 2, 0, 6,
		3, 7, 1, 1, 7, 5
	};

	// Token: 0x0400142E RID: 5166
	private static readonly Vector3[] boxUnitVertices = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x0400142F RID: 5167
	private static Matrix4x4 boxScaleMatrix = Matrix4x4.identity;
}
