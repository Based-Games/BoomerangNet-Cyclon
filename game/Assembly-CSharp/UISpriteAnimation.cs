using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BF RID: 191
[AddComponentMenu("NGUI/UI/Sprite Animation")]
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
public class UISpriteAnimation : MonoBehaviour
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x0000803B File Offset: 0x0000623B
	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000647 RID: 1607 RVA: 0x00008048 File Offset: 0x00006248
	// (set) Token: 0x06000648 RID: 1608 RVA: 0x00008050 File Offset: 0x00006250
	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000649 RID: 1609 RVA: 0x00008059 File Offset: 0x00006259
	// (set) Token: 0x0600064A RID: 1610 RVA: 0x00008061 File Offset: 0x00006261
	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x00008081 File Offset: 0x00006281
	// (set) Token: 0x0600064C RID: 1612 RVA: 0x00008089 File Offset: 0x00006289
	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00008092 File Offset: 0x00006292
	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0000809A File Offset: 0x0000629A
	private void Start()
	{
		this.RebuildSpriteList();
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00031DF8 File Offset: 0x0002FFF8
	private void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && (float)this.mFPS > 0f)
		{
			this.mDelta += RealTime.deltaTime;
			float num = 1f / (float)this.mFPS;
			if (num < this.mDelta)
			{
				this.mDelta = ((num <= 0f) ? 0f : (this.mDelta - num));
				if (++this.mIndex >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.loop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					this.mSprite.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00031EF4 File Offset: 0x000300F4
	private void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uispriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.mPrefix) || uispriteData.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(uispriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00031FC4 File Offset: 0x000301C4
	public void Reset()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			this.mSprite.MakePixelPerfect();
		}
	}

	// Token: 0x040004CF RID: 1231
	[HideInInspector]
	[SerializeField]
	private int mFPS = 30;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	[HideInInspector]
	private string mPrefix = string.Empty;

	// Token: 0x040004D1 RID: 1233
	[SerializeField]
	[HideInInspector]
	private bool mLoop = true;

	// Token: 0x040004D2 RID: 1234
	private UISprite mSprite;

	// Token: 0x040004D3 RID: 1235
	private float mDelta;

	// Token: 0x040004D4 RID: 1236
	private int mIndex;

	// Token: 0x040004D5 RID: 1237
	private bool mActive = true;

	// Token: 0x040004D6 RID: 1238
	private List<string> mSpriteNames = new List<string>();
}
