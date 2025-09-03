using System;
using UnityEngine;

// Token: 0x020002D2 RID: 722
[AddComponentMenu("2D Toolkit/Demo/tk2dDemoRuntimeSpriteController")]
public class tk2dDemoRuntimeSpriteController : MonoBehaviour
{
	// Token: 0x06001524 RID: 5412 RVA: 0x00012011 File Offset: 0x00010211
	private void Start()
	{
		if (this.destroyOnStart != null)
		{
			UnityEngine.Object.Destroy(this.destroyOnStart);
		}
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x0008CEF8 File Offset: 0x0008B0F8
	private void DestroyData()
	{
		if (this.spriteInstance != null)
		{
			UnityEngine.Object.Destroy(this.spriteInstance.gameObject);
		}
		if (this.spriteCollectionInstance != null)
		{
			UnityEngine.Object.Destroy(this.spriteCollectionInstance.gameObject);
		}
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x0008CF48 File Offset: 0x0008B148
	private void DoDemoTexturePacker(tk2dSpriteCollectionSize spriteCollectionSize)
	{
		if (GUILayout.Button("Import", new GUILayoutOption[0]))
		{
			this.DestroyData();
			this.spriteCollectionInstance = tk2dSpriteCollectionData.CreateFromTexturePacker(spriteCollectionSize, this.texturePackerExportFile.text, this.texturePackerTexture);
			this.spriteInstance = new GameObject("sprite")
			{
				transform = 
				{
					localPosition = new Vector3(-1f, 0f, 0f)
				}
			}.AddComponent<tk2dSprite>();
			this.spriteInstance.SetSprite(this.spriteCollectionInstance, "sun");
			tk2dSprite tk2dSprite = new GameObject("sprite2")
			{
				transform = 
				{
					parent = this.spriteInstance.transform,
					localPosition = new Vector3(2f, 0f, 0f)
				}
			}.AddComponent<tk2dSprite>();
			tk2dSprite.SetSprite(this.spriteCollectionInstance, "2dtoolkit_logo");
			tk2dSprite = new GameObject("sprite3")
			{
				transform = 
				{
					parent = this.spriteInstance.transform,
					localPosition = new Vector3(1f, 1f, 0f)
				}
			}.AddComponent<tk2dSprite>();
			tk2dSprite.SetSprite(this.spriteCollectionInstance, "button_up");
			tk2dSprite = new GameObject("sprite4")
			{
				transform = 
				{
					parent = this.spriteInstance.transform,
					localPosition = new Vector3(1f, -1f, 0f)
				}
			}.AddComponent<tk2dSprite>();
			tk2dSprite.SetSprite(this.spriteCollectionInstance, "Rock");
		}
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x0008D0E8 File Offset: 0x0008B2E8
	private void DoDemoRuntimeSpriteCollection(tk2dSpriteCollectionSize spriteCollectionSize)
	{
		if (GUILayout.Button("Use Full Texture", new GUILayoutOption[0]))
		{
			this.DestroyData();
			Rect rect = new Rect(0f, 0f, (float)this.runtimeTexture.width, (float)this.runtimeTexture.height);
			Vector2 vector = new Vector2(rect.width / 2f, rect.height / 2f);
			GameObject gameObject = tk2dSprite.CreateFromTexture(this.runtimeTexture, spriteCollectionSize, rect, vector);
			this.spriteInstance = gameObject.GetComponent<tk2dSprite>();
			this.spriteCollectionInstance = this.spriteInstance.Collection;
		}
		if (GUILayout.Button("Extract Region)", new GUILayoutOption[0]))
		{
			this.DestroyData();
			Rect rect2 = new Rect(79f, 243f, 215f, 200f);
			Vector2 vector2 = new Vector2(rect2.width / 2f, rect2.height / 2f);
			GameObject gameObject2 = tk2dSprite.CreateFromTexture(this.runtimeTexture, spriteCollectionSize, rect2, vector2);
			this.spriteInstance = gameObject2.GetComponent<tk2dSprite>();
			this.spriteCollectionInstance = this.spriteInstance.Collection;
		}
		if (GUILayout.Button("Extract multiple Sprites", new GUILayoutOption[0]))
		{
			this.DestroyData();
			string[] array = new string[] { "Extracted region", "Another region", "Full sprite" };
			Rect[] array2 = new Rect[]
			{
				new Rect(79f, 243f, 215f, 200f),
				new Rect(256f, 0f, 64f, 64f),
				new Rect(0f, 0f, (float)this.runtimeTexture.width, (float)this.runtimeTexture.height)
			};
			Vector2[] array3 = new Vector2[]
			{
				new Vector2(array2[0].width / 2f, array2[0].height / 2f),
				new Vector2(0f, array2[1].height),
				new Vector2(0f, array2[1].height)
			};
			this.spriteCollectionInstance = tk2dSpriteCollectionData.CreateFromTexture(this.runtimeTexture, spriteCollectionSize, array, array2, array3);
			this.spriteInstance = new GameObject("sprite")
			{
				transform = 
				{
					localPosition = new Vector3(-1f, 0f, 0f)
				}
			}.AddComponent<tk2dSprite>();
			this.spriteInstance.SetSprite(this.spriteCollectionInstance, 0);
			tk2dSprite tk2dSprite = new GameObject("sprite2")
			{
				transform = 
				{
					parent = this.spriteInstance.transform,
					localPosition = new Vector3(2f, 0f, 0f)
				}
			}.AddComponent<tk2dSprite>();
			tk2dSprite.SetSprite(this.spriteCollectionInstance, "Another region");
		}
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x0008D414 File Offset: 0x0008B614
	private void OnGUI()
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = tk2dSpriteCollectionSize.Explicit(5f, 640f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical("box", new GUILayoutOption[0]);
		GUILayout.Label("Runtime Sprite Collection", new GUILayoutOption[0]);
		this.DoDemoRuntimeSpriteCollection(tk2dSpriteCollectionSize);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box", new GUILayoutOption[0]);
		GUILayout.Label("Texture Packer Import", new GUILayoutOption[0]);
		this.DoDemoTexturePacker(tk2dSpriteCollectionSize);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x0400168D RID: 5773
	public Texture2D runtimeTexture;

	// Token: 0x0400168E RID: 5774
	public Texture2D texturePackerTexture;

	// Token: 0x0400168F RID: 5775
	public TextAsset texturePackerExportFile;

	// Token: 0x04001690 RID: 5776
	public GameObject destroyOnStart;

	// Token: 0x04001691 RID: 5777
	private tk2dBaseSprite spriteInstance;

	// Token: 0x04001692 RID: 5778
	private tk2dSpriteCollectionData spriteCollectionInstance;
}
