using System;

// Token: 0x02000258 RID: 600
[Serializable]
public class tk2dSpriteAnimationFrame
{
	// Token: 0x06001195 RID: 4501 RVA: 0x0000EDBD File Offset: 0x0000CFBD
	public void CopyFrom(tk2dSpriteAnimationFrame source)
	{
		this.CopyFrom(source, true);
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0000EDC7 File Offset: 0x0000CFC7
	public void CopyTriggerFrom(tk2dSpriteAnimationFrame source)
	{
		this.triggerEvent = source.triggerEvent;
		this.eventInfo = source.eventInfo;
		this.eventInt = source.eventInt;
		this.eventFloat = source.eventFloat;
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x0000EDF9 File Offset: 0x0000CFF9
	public void ClearTrigger()
	{
		this.triggerEvent = false;
		this.eventInt = 0;
		this.eventFloat = 0f;
		this.eventInfo = string.Empty;
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x0000EE1F File Offset: 0x0000D01F
	public void CopyFrom(tk2dSpriteAnimationFrame source, bool full)
	{
		this.spriteCollection = source.spriteCollection;
		this.spriteId = source.spriteId;
		if (full)
		{
			this.CopyTriggerFrom(source);
		}
	}

	// Token: 0x040012F5 RID: 4853
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x040012F6 RID: 4854
	public int spriteId;

	// Token: 0x040012F7 RID: 4855
	public bool triggerEvent;

	// Token: 0x040012F8 RID: 4856
	public string eventInfo = string.Empty;

	// Token: 0x040012F9 RID: 4857
	public int eventInt;

	// Token: 0x040012FA RID: 4858
	public float eventFloat;
}
