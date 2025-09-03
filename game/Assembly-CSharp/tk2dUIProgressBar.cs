using System;
using UnityEngine;

// Token: 0x0200029F RID: 671
[AddComponentMenu("2D Toolkit/UI/tk2dUIProgressBar")]
public class tk2dUIProgressBar : MonoBehaviour
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06001350 RID: 4944 RVA: 0x0001096C File Offset: 0x0000EB6C
	// (remove) Token: 0x06001351 RID: 4945 RVA: 0x00010985 File Offset: 0x0000EB85
	public event Action OnProgressComplete;

	// Token: 0x06001352 RID: 4946 RVA: 0x0001099E File Offset: 0x0000EB9E
	private void Start()
	{
		this.InitializeSlicedSpriteDimensions();
		this.Value = this.percent;
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001353 RID: 4947 RVA: 0x000109B2 File Offset: 0x0000EBB2
	// (set) Token: 0x06001354 RID: 4948 RVA: 0x000854FC File Offset: 0x000836FC
	public float Value
	{
		get
		{
			return this.percent;
		}
		set
		{
			this.percent = Mathf.Clamp(value, 0f, 1f);
			if (Application.isPlaying)
			{
				if (this.clippedSpriteBar != null)
				{
					this.clippedSpriteBar.clipTopRight = new Vector2(this.Value, 1f);
				}
				else if (this.scalableBar != null)
				{
					this.scalableBar.localScale = new Vector3(this.Value, this.scalableBar.localScale.y, this.scalableBar.localScale.z);
				}
				else if (this.slicedSpriteBar != null)
				{
					this.InitializeSlicedSpriteDimensions();
					float num = Mathf.Lerp(this.emptySlicedSpriteDimensions.x, this.fullSlicedSpriteDimensions.x, this.Value);
					this.currentDimensions.Set(num, this.fullSlicedSpriteDimensions.y);
					this.slicedSpriteBar.dimensions = this.currentDimensions;
				}
				if (!this.isProgressComplete && this.Value == 1f)
				{
					this.isProgressComplete = true;
					if (this.OnProgressComplete != null)
					{
						this.OnProgressComplete();
					}
					if (this.sendMessageTarget != null && this.SendMessageOnProgressCompleteMethodName.Length > 0)
					{
						this.sendMessageTarget.SendMessage(this.SendMessageOnProgressCompleteMethodName, this, SendMessageOptions.RequireReceiver);
					}
				}
				else if (this.isProgressComplete && this.Value < 1f)
				{
					this.isProgressComplete = false;
				}
			}
		}
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x000856A4 File Offset: 0x000838A4
	private void InitializeSlicedSpriteDimensions()
	{
		if (!this.initializedSlicedSpriteDimensions)
		{
			if (this.slicedSpriteBar != null)
			{
				tk2dSpriteDefinition currentSprite = this.slicedSpriteBar.CurrentSprite;
				Vector3 vector = currentSprite.boundsData[1];
				this.fullSlicedSpriteDimensions = this.slicedSpriteBar.dimensions;
				this.emptySlicedSpriteDimensions.Set((this.slicedSpriteBar.borderLeft + this.slicedSpriteBar.borderRight) * vector.x / currentSprite.texelSize.x, this.fullSlicedSpriteDimensions.y);
			}
			this.initializedSlicedSpriteDimensions = true;
		}
	}

	// Token: 0x040014FA RID: 5370
	public Transform scalableBar;

	// Token: 0x040014FB RID: 5371
	public tk2dClippedSprite clippedSpriteBar;

	// Token: 0x040014FC RID: 5372
	public tk2dSlicedSprite slicedSpriteBar;

	// Token: 0x040014FD RID: 5373
	private bool initializedSlicedSpriteDimensions;

	// Token: 0x040014FE RID: 5374
	private Vector2 emptySlicedSpriteDimensions = Vector2.zero;

	// Token: 0x040014FF RID: 5375
	private Vector2 fullSlicedSpriteDimensions = Vector2.zero;

	// Token: 0x04001500 RID: 5376
	private Vector2 currentDimensions = Vector2.zero;

	// Token: 0x04001501 RID: 5377
	[SerializeField]
	private float percent;

	// Token: 0x04001502 RID: 5378
	private bool isProgressComplete;

	// Token: 0x04001503 RID: 5379
	public GameObject sendMessageTarget;

	// Token: 0x04001504 RID: 5380
	public string SendMessageOnProgressCompleteMethodName = string.Empty;
}
