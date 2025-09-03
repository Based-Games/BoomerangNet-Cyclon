using System;
using UnityEngine;

// Token: 0x020002AD RID: 685
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIAudioManager")]
public class tk2dUIAudioManager : MonoBehaviour
{
	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001405 RID: 5125 RVA: 0x0008833C File Offset: 0x0008653C
	public static tk2dUIAudioManager Instance
	{
		get
		{
			if (tk2dUIAudioManager.instance == null)
			{
				tk2dUIAudioManager.instance = UnityEngine.Object.FindObjectOfType(typeof(tk2dUIAudioManager)) as tk2dUIAudioManager;
				if (tk2dUIAudioManager.instance == null)
				{
					tk2dUIAudioManager.instance = new GameObject("tk2dUIAudioManager").AddComponent<tk2dUIAudioManager>();
				}
			}
			return tk2dUIAudioManager.instance;
		}
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x00011454 File Offset: 0x0000F654
	private void Awake()
	{
		if (tk2dUIAudioManager.instance == null)
		{
			tk2dUIAudioManager.instance = this;
		}
		else if (tk2dUIAudioManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.Setup();
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x0008839C File Offset: 0x0008659C
	private void Setup()
	{
		if (this.audioSrc == null)
		{
			this.audioSrc = base.gameObject.GetComponent<AudioSource>();
		}
		if (this.audioSrc == null)
		{
			this.audioSrc = base.gameObject.AddComponent<AudioSource>();
			this.audioSrc.playOnAwake = false;
		}
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x0001148E File Offset: 0x0000F68E
	public void Play(AudioClip clip)
	{
		this.audioSrc.PlayOneShot(clip);
	}

	// Token: 0x0400157D RID: 5501
	private static tk2dUIAudioManager instance;

	// Token: 0x0400157E RID: 5502
	private AudioSource audioSrc;
}
