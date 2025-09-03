using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025E RID: 606
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAttachPoint")]
[ExecuteInEditMode]
public class tk2dSpriteAttachPoint : MonoBehaviour
{
	// Token: 0x060011DC RID: 4572 RVA: 0x0000F39B File Offset: 0x0000D59B
	private void Awake()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<tk2dBaseSprite>();
			if (this.sprite != null)
			{
				this.HandleSpriteChanged(this.sprite);
			}
		}
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x0000F3D7 File Offset: 0x0000D5D7
	private void OnEnable()
	{
		if (this.sprite != null)
		{
			this.sprite.SpriteChanged += this.HandleSpriteChanged;
		}
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x0000F401 File Offset: 0x0000D601
	private void OnDisable()
	{
		if (this.sprite != null)
		{
			this.sprite.SpriteChanged -= this.HandleSpriteChanged;
		}
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x0007BB34 File Offset: 0x00079D34
	private void UpdateAttachPointTransform(tk2dSpriteDefinition.AttachPoint attachPoint, Transform t)
	{
		t.localPosition = Vector3.Scale(attachPoint.position, this.sprite.scale);
		t.localScale = this.sprite.scale;
		float num = Mathf.Sign(this.sprite.scale.x) * Mathf.Sign(this.sprite.scale.y);
		t.localEulerAngles = new Vector3(0f, 0f, attachPoint.angle * num);
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x0007BBC0 File Offset: 0x00079DC0
	private void HandleSpriteChanged(tk2dBaseSprite spr)
	{
		tk2dSpriteDefinition currentSprite = spr.CurrentSprite;
		int num = Mathf.Max(currentSprite.attachPoints.Length, this.attachPoints.Count);
		if (num > tk2dSpriteAttachPoint.attachPointUpdated.Length)
		{
			tk2dSpriteAttachPoint.attachPointUpdated = new bool[num];
		}
		foreach (tk2dSpriteDefinition.AttachPoint attachPoint in currentSprite.attachPoints)
		{
			bool flag = false;
			int num2 = 0;
			foreach (Transform transform in this.attachPoints)
			{
				if (transform != null && transform.name == attachPoint.name)
				{
					tk2dSpriteAttachPoint.attachPointUpdated[num2] = true;
					this.UpdateAttachPointTransform(attachPoint, transform);
					flag = true;
				}
				num2++;
			}
			if (!flag)
			{
				GameObject gameObject = new GameObject(attachPoint.name);
				Transform transform2 = gameObject.transform;
				transform2.parent = base.transform;
				this.UpdateAttachPointTransform(attachPoint, transform2);
				tk2dSpriteAttachPoint.attachPointUpdated[this.attachPoints.Count] = true;
				this.attachPoints.Add(transform2);
			}
		}
		if (this.deactivateUnusedAttachPoints)
		{
			for (int j = 0; j < this.attachPoints.Count; j++)
			{
				if (this.attachPoints[j] != null)
				{
					GameObject gameObject2 = this.attachPoints[j].gameObject;
					if (tk2dSpriteAttachPoint.attachPointUpdated[j] && !gameObject2.activeSelf)
					{
						gameObject2.SetActive(true);
					}
					else if (!tk2dSpriteAttachPoint.attachPointUpdated[j] && gameObject2.activeSelf)
					{
						gameObject2.SetActive(false);
					}
				}
				tk2dSpriteAttachPoint.attachPointUpdated[j] = false;
			}
		}
	}

	// Token: 0x04001319 RID: 4889
	private tk2dBaseSprite sprite;

	// Token: 0x0400131A RID: 4890
	public List<Transform> attachPoints = new List<Transform>();

	// Token: 0x0400131B RID: 4891
	private static bool[] attachPointUpdated = new bool[32];

	// Token: 0x0400131C RID: 4892
	public bool deactivateUnusedAttachPoints;
}
