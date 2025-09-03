using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B1 RID: 689
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUILayout")]
public class tk2dUILayout : MonoBehaviour
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06001441 RID: 5185 RVA: 0x0001188A File Offset: 0x0000FA8A
	// (remove) Token: 0x06001442 RID: 5186 RVA: 0x000118A3 File Offset: 0x0000FAA3
	public event Action<Vector3, Vector3> OnReshape;

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06001443 RID: 5187 RVA: 0x000118BC File Offset: 0x0000FABC
	public int ItemCount
	{
		get
		{
			return this.layoutItems.Count;
		}
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x00088A14 File Offset: 0x00086C14
	private void Reset()
	{
		if (base.collider != null)
		{
			BoxCollider boxCollider = base.collider as BoxCollider;
			if (boxCollider != null)
			{
				Bounds bounds = boxCollider.bounds;
				Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
				Vector3 position = base.transform.position;
				this.Reshape(worldToLocalMatrix.MultiplyPoint(bounds.min) - this.bMin, worldToLocalMatrix.MultiplyPoint(bounds.max) - this.bMax, true);
				Vector3 vector = worldToLocalMatrix.MultiplyVector(base.transform.position - position);
				Transform transform = base.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					Vector3 vector2 = child.localPosition - vector;
					child.localPosition = vector2;
				}
				boxCollider.center -= vector;
				this.autoResizeCollider = true;
			}
		}
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x00088B20 File Offset: 0x00086D20
	public virtual void Reshape(Vector3 dMin, Vector3 dMax, bool updateChildren)
	{
		foreach (tk2dUILayoutItem tk2dUILayoutItem in this.layoutItems)
		{
			tk2dUILayoutItem.oldPos = tk2dUILayoutItem.gameObj.transform.position;
		}
		this.bMin += dMin;
		this.bMax += dMax;
		Vector3 vector = new Vector3(this.bMin.x, this.bMax.y);
		base.transform.position += base.transform.localToWorldMatrix.MultiplyVector(vector);
		this.bMin -= vector;
		this.bMax -= vector;
		if (this.autoResizeCollider)
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.center += (dMin + dMax) / 2f - vector;
				component.size += dMax - dMin;
			}
		}
		foreach (tk2dUILayoutItem tk2dUILayoutItem2 in this.layoutItems)
		{
			Vector3 vector2 = base.transform.worldToLocalMatrix.MultiplyVector(tk2dUILayoutItem2.gameObj.transform.position - tk2dUILayoutItem2.oldPos);
			Vector3 vector3 = -vector2;
			Vector3 vector4 = -vector2;
			if (updateChildren)
			{
				vector3.x += ((!tk2dUILayoutItem2.snapLeft) ? ((!tk2dUILayoutItem2.snapRight) ? 0f : dMax.x) : dMin.x);
				vector3.y += ((!tk2dUILayoutItem2.snapBottom) ? ((!tk2dUILayoutItem2.snapTop) ? 0f : dMax.y) : dMin.y);
				vector4.x += ((!tk2dUILayoutItem2.snapRight) ? ((!tk2dUILayoutItem2.snapLeft) ? 0f : dMin.x) : dMax.x);
				vector4.y += ((!tk2dUILayoutItem2.snapTop) ? ((!tk2dUILayoutItem2.snapBottom) ? 0f : dMin.y) : dMax.y);
			}
			if (tk2dUILayoutItem2.sprite != null || tk2dUILayoutItem2.UIMask != null || tk2dUILayoutItem2.layout != null)
			{
				Matrix4x4 matrix4x = base.transform.localToWorldMatrix * tk2dUILayoutItem2.gameObj.transform.worldToLocalMatrix;
				vector3 = matrix4x.MultiplyVector(vector3);
				vector4 = matrix4x.MultiplyVector(vector4);
			}
			if (tk2dUILayoutItem2.sprite != null)
			{
				tk2dUILayoutItem2.sprite.ReshapeBounds(vector3, vector4);
			}
			else if (tk2dUILayoutItem2.UIMask != null)
			{
				tk2dUILayoutItem2.UIMask.ReshapeBounds(vector3, vector4);
			}
			else if (tk2dUILayoutItem2.layout != null)
			{
				tk2dUILayoutItem2.layout.Reshape(vector3, vector4, true);
			}
			else
			{
				Vector3 vector5 = vector3;
				if (tk2dUILayoutItem2.snapLeft && tk2dUILayoutItem2.snapRight)
				{
					vector5.x = 0.5f * (vector3.x + vector4.x);
				}
				if (tk2dUILayoutItem2.snapTop && tk2dUILayoutItem2.snapBottom)
				{
					vector5.y = 0.5f * (vector3.y + vector4.y);
				}
				tk2dUILayoutItem2.gameObj.transform.position += vector5;
			}
		}
		if (this.OnReshape != null)
		{
			this.OnReshape(dMin, dMax);
		}
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x00088FB4 File Offset: 0x000871B4
	public void SetBounds(Vector3 pMin, Vector3 pMax)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		this.Reshape(worldToLocalMatrix.MultiplyPoint(pMin) - this.bMin, worldToLocalMatrix.MultiplyPoint(pMax) - this.bMax, true);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x00088FFC File Offset: 0x000871FC
	public Vector3 GetMinBounds()
	{
		return base.transform.localToWorldMatrix.MultiplyPoint(this.bMin);
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x00089024 File Offset: 0x00087224
	public Vector3 GetMaxBounds()
	{
		return base.transform.localToWorldMatrix.MultiplyPoint(this.bMax);
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000118C9 File Offset: 0x0000FAC9
	public void Refresh()
	{
		this.Reshape(Vector3.zero, Vector3.zero, true);
	}

	// Token: 0x040015A8 RID: 5544
	public Vector3 bMin = new Vector3(0f, -1f, 0f);

	// Token: 0x040015A9 RID: 5545
	public Vector3 bMax = new Vector3(1f, 0f, 0f);

	// Token: 0x040015AA RID: 5546
	public List<tk2dUILayoutItem> layoutItems = new List<tk2dUILayoutItem>();

	// Token: 0x040015AB RID: 5547
	public bool autoResizeCollider;
}
