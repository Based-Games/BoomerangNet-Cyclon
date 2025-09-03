using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public struct tk2dUITouch
{
	// Token: 0x06001485 RID: 5253 RVA: 0x00011C31 File Offset: 0x0000FE31
	public tk2dUITouch(TouchPhase _phase, int _fingerId, Vector2 _position, Vector2 _deltaPosition, float _deltaTime)
	{
		this.phase = _phase;
		this.fingerId = _fingerId;
		this.position = _position;
		this.deltaPosition = _deltaPosition;
		this.deltaTime = _deltaTime;
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x0008AD0C File Offset: 0x00088F0C
	public tk2dUITouch(Touch touch)
	{
		this.phase = touch.phase;
		this.fingerId = touch.fingerId;
		this.position = touch.position;
		this.deltaPosition = this.deltaPosition;
		this.deltaTime = this.deltaTime;
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001487 RID: 5255 RVA: 0x00011C58 File Offset: 0x0000FE58
	// (set) Token: 0x06001488 RID: 5256 RVA: 0x00011C60 File Offset: 0x0000FE60
	public TouchPhase phase { get; private set; }

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001489 RID: 5257 RVA: 0x00011C69 File Offset: 0x0000FE69
	// (set) Token: 0x0600148A RID: 5258 RVA: 0x00011C71 File Offset: 0x0000FE71
	public int fingerId { get; private set; }

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x0600148B RID: 5259 RVA: 0x00011C7A File Offset: 0x0000FE7A
	// (set) Token: 0x0600148C RID: 5260 RVA: 0x00011C82 File Offset: 0x0000FE82
	public Vector2 position { get; private set; }

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x0600148D RID: 5261 RVA: 0x00011C8B File Offset: 0x0000FE8B
	// (set) Token: 0x0600148E RID: 5262 RVA: 0x00011C93 File Offset: 0x0000FE93
	public Vector2 deltaPosition { get; private set; }

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x0600148F RID: 5263 RVA: 0x00011C9C File Offset: 0x0000FE9C
	// (set) Token: 0x06001490 RID: 5264 RVA: 0x00011CA4 File Offset: 0x0000FEA4
	public float deltaTime { get; private set; }

	// Token: 0x06001491 RID: 5265 RVA: 0x0008AD58 File Offset: 0x00088F58
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			this.phase.ToString(),
			",",
			this.fingerId,
			",",
			this.position,
			",",
			this.deltaPosition,
			",",
			this.deltaTime
		});
	}

	// Token: 0x040015DF RID: 5599
	public const int MOUSE_POINTER_FINGER_ID = 9999;
}
