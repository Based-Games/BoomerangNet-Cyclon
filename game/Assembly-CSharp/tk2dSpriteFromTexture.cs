using System;
using tk2dRuntime;
using UnityEngine;

// Token: 0x0200027A RID: 634
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteFromTexture")]
public class tk2dSpriteFromTexture : MonoBehaviour
{
	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06001225 RID: 4645 RVA: 0x0007D354 File Offset: 0x0007B554
	private tk2dBaseSprite Sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<tk2dBaseSprite>();
				if (this._sprite == null)
				{
					Debug.Log("tk2dSpriteFromTexture - Missing sprite object. Creating.");
					this._sprite = base.gameObject.AddComponent<tk2dSprite>();
				}
			}
			return this._sprite;
		}
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x0000F84A File Offset: 0x0000DA4A
	private void Awake()
	{
		this.Create(this.spriteCollectionSize, this.texture, this.anchor);
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06001227 RID: 4647 RVA: 0x0000F864 File Offset: 0x0000DA64
	public bool HasSpriteCollection
	{
		get
		{
			return this.spriteCollection != null;
		}
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x0000F872 File Offset: 0x0000DA72
	private void OnDestroy()
	{
		this.DestroyInternal();
		if (base.renderer != null)
		{
			base.renderer.material = null;
		}
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x0007D3B0 File Offset: 0x0007B5B0
	public void Create(tk2dSpriteCollectionSize spriteCollectionSize, Texture texture, tk2dBaseSprite.Anchor anchor)
	{
		this.DestroyInternal();
		if (texture != null)
		{
			this.spriteCollectionSize.CopyFrom(spriteCollectionSize);
			this.texture = texture;
			this.anchor = anchor;
			GameObject gameObject = new GameObject("tk2dSpriteFromTexture - " + texture.name);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.hideFlags = HideFlags.DontSave;
			Vector2 anchorOffset = tk2dSpriteGeomGen.GetAnchorOffset(anchor, (float)texture.width, (float)texture.height);
			this.spriteCollection = SpriteCollectionGenerator.CreateFromTexture(gameObject, texture, spriteCollectionSize, new Vector2((float)texture.width, (float)texture.height), new string[] { "unnamed" }, new Rect[]
			{
				new Rect(0f, 0f, (float)texture.width, (float)texture.height)
			}, null, new Vector2[] { anchorOffset }, new bool[1]);
			string text = "SpriteFromTexture " + texture.name;
			this.spriteCollection.spriteCollectionName = text;
			this.spriteCollection.spriteDefinitions[0].material.name = text;
			this.spriteCollection.spriteDefinitions[0].material.hideFlags = HideFlags.HideInInspector | HideFlags.DontSave;
			this.Sprite.SetSprite(this.spriteCollection, 0);
		}
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x0000F897 File Offset: 0x0000DA97
	public void Clear()
	{
		this.DestroyInternal();
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x0000F89F File Offset: 0x0000DA9F
	public void ForceBuild()
	{
		this.DestroyInternal();
		this.Create(this.spriteCollectionSize, this.texture, this.anchor);
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x0007D524 File Offset: 0x0007B724
	private void DestroyInternal()
	{
		if (this.spriteCollection != null)
		{
			if (this.spriteCollection.spriteDefinitions[0].material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.spriteCollection.spriteDefinitions[0].material);
			}
			UnityEngine.Object.DestroyImmediate(this.spriteCollection.gameObject);
			this.spriteCollection = null;
		}
	}

	// Token: 0x04001427 RID: 5159
	public Texture texture;

	// Token: 0x04001428 RID: 5160
	public tk2dSpriteCollectionSize spriteCollectionSize = new tk2dSpriteCollectionSize();

	// Token: 0x04001429 RID: 5161
	public tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.MiddleCenter;

	// Token: 0x0400142A RID: 5162
	private tk2dSpriteCollectionData spriteCollection;

	// Token: 0x0400142B RID: 5163
	private tk2dBaseSprite _sprite;
}
