using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MiniJSON;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class SoundSourceManager : Singleton<SoundSourceManager>
{
	// Token: 0x06000A79 RID: 2681
	[DllImport("SndProxy")]
	private static extern int fnSndProxyInit(IntPtr hwnd);

	// Token: 0x06000A7A RID: 2682
	[DllImport("SndProxy")]
	private static extern int fnSndProxyLoadSample(string lpszWave);

	// Token: 0x06000A7B RID: 2683
	[DllImport("SndProxy")]
	private static extern int fnSndProxyPlaySample(string lpszWave, float fMainVolume, float fVariableRate, bool bLoop);

	// Token: 0x06000A7C RID: 2684
	[DllImport("SndProxy")]
	private static extern int fnSndProxyLoadSampleBuffer(string lpszWave, byte[] pBuffer, int iLength);

	// Token: 0x06000A7D RID: 2685
	[DllImport("SndProxy")]
	private static extern float fnSndProxyGetCurrentTime(string lpszWave);

	// Token: 0x06000A7E RID: 2686
	[DllImport("SndProxy")]
	private static extern void fnSndProxySetCurrentTime(string lpszWave, float fTime);

	// Token: 0x06000A7F RID: 2687
	[DllImport("SndProxy")]
	private static extern int fnSndProxyStopSample(string lpszWave);

	// Token: 0x06000A80 RID: 2688
	[DllImport("SndProxy")]
	private static extern int fnSndProxyStopAllSample();

	// Token: 0x06000A81 RID: 2689
	[DllImport("SndProxy")]
	private static extern int fnSndProxyDestroy();

	// Token: 0x06000A82 RID: 2690 RVA: 0x0000A3D1 File Offset: 0x000085D1
	private void Awake()
	{
		if (base.gameObject.GetComponent<AudioListener>() == null)
		{
			base.gameObject.AddComponent<AudioListener>();
		}
		this.LoadSound();
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0004AF14 File Offset: 0x00049114
	public void InitKeySound()
	{
		List<object> list = (List<object>)(Json.Deserialize(Singleton<GameManager>.instance.ReadSystemJSONFile("touchLibrary")) as Dictionary<string, object>)["keysounds"];
		SoundSourceManager.fnSndProxyInit(IntPtr.Zero);
		for (int i = 1; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			string text = "Touch/" + dictionary["fileName"].ToString();
			byte[] array = Singleton<SongManager>.instance.LoadUnPackerFile(text);
			if (SoundSourceManager.fnSndProxyLoadSampleBuffer(dictionary["fileName"].ToString(), array, array.Length) != 0)
			{
				Logger.Error("SoundSourceManager", "Error Load fnSndProxyLoadSampleBuffer" + dictionary["fileName"].ToString(), new object[0]);
			}
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0000A3F8 File Offset: 0x000085F8
	public void LoadMainSound(string strName, byte[] pBuffer, int iLength)
	{
		SoundSourceManager.fnSndProxyLoadSampleBuffer(strName, pBuffer, iLength);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0000A403 File Offset: 0x00008603
	public void SetMainSoundTime(string strName, float fTime)
	{
		SoundSourceManager.fnSndProxySetCurrentTime(strName, fTime);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0000A40C File Offset: 0x0000860C
	public float GetMainSoundTime(string strName)
	{
		return SoundSourceManager.fnSndProxyGetCurrentTime(strName);
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0000A414 File Offset: 0x00008614
	public void PlayMainSound(string strName)
	{
		SoundSourceManager.fnSndProxyPlaySample(strName, 1f, (float)this.EFF_VARIABLE, false);
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0004AFE8 File Offset: 0x000491E8
	public void LoadSound()
	{
		string[] array = new string[]
		{
			"Sound/Sfx/UIKeySound/1_powerset/1_trig_powerset", "Sound/Sfx/UIKeySound/1_powerset/2_shot_powerset", "Sound/Sfx/UIKeySound/1_powerset/3_zap_powerset", "Sound/Sfx/UIKeySound/1_powerset/4_snap_powerset", "Sound/Sfx/UIKeySound/2_midiumset/1_trig_midiumset", "Sound/Sfx/UIKeySound/2_midiumset/2_shot_midiumset", "Sound/Sfx/UIKeySound/2_midiumset/3_zap_midiumset", "Sound/Sfx/UIKeySound/2_midiumset/4_snap_midiumset", "Sound/Sfx/UIKeySound/3_lowerset/1_trig_lowerset", "Sound/Sfx/UIKeySound/3_lowerset/2_shot_lowerset",
			"Sound/Sfx/UIKeySound/3_lowerset/3_zap_lowerset", "Sound/Sfx/UIKeySound/3_lowerset/4_snap_lowerset", "Sound/Sfx/Game/_extreme_1start", "Sound/Sfx/Game/_extreme_2start", "Sound/Sfx/Game/_extreme_charge", "Sound/Sfx/Game/_extreme_in", "Sound/Sfx/Game/_fever_charge", "Sound/Sfx/Game/_fever_start", "Sound/Sfx/Game/_game_over", "Sound/Sfx/ModeSelect/int_haus",
			"Sound/Sfx/ModeSelect/int_raveup", "Sound/Sfx/ModeSelect/touch_mode", "Sound/Sfx/ModeSelect/int_club", "Sound/Sfx/ModeSelect/touch_haus", "Sound/Sfx/ModeSelect/touch_raveup", "Sound/Sfx/ModeSelect/touch_club", "Sound/Sfx/ModeSelect/touch_gp", "Sound/Sfx/Login/login_button", "Sound/Sfx/Login/login_fail", "Sound/Sfx/Login/login_success",
			"Sound/Sfx/RaveUp/album_next", "Sound/Sfx/RaveUp/album_sel", "Sound/Sfx/RaveUp/album_spread", "Sound/Sfx/RaveUp/disc_mount", "Sound/Sfx/RaveUp/disc_unmount", "Sound/Sfx/RaveUp/start_noready", "Sound/Sfx/RaveUp/hidden_mount", "Sound/Sfx/Title/coin_finish", "Sound/Sfx/Title/coin_in", "Sound/Sfx/Title/common_start",
			"Sound/Sfx/Title/int_title", "Sound/Sfx/Title/start_alright", "Sound/Sfx/SongSelect/effect_mount", "Sound/Sfx/SongSelect/effect_scroll", "Sound/Sfx/SongSelect/patt_sel", "Sound/Sfx/SongSelect/sort_decrease", "Sound/Sfx/SongSelect/sort_increase", "Sound/Sfx/SongSelect/touch_song", "Sound/Sfx/Result/a", "Sound/Sfx/Result/accuracy_count",
			"Sound/Sfx/Result/b_d", "Sound/Sfx/Result/bp_count", "Sound/Sfx/Result/exp_bar", "Sound/Sfx/Result/f", "Sound/Sfx/Result/level_up", "Sound/Sfx/Result/s", "Sound/Sfx/Result/score_count", "Sound/Sfx/Result/stage_cleared", "Sound/Sfx/Result/trophy", "Sound/Sfx/Result/popup",
			"Sound/Sfx/Result/record_maxcombo", "Sound/Sfx/Result/record_new", "Sound/Sfx/Result/record_perfectplay", "Sound/Sfx/Result/record_top", "Sound/Sfx/Result/record_combo_all", "Sound/Sfx/Common/common_back", "Sound/Sfx/Common/common_start", "Sound/Sfx/Common/common_timer", "Sound/Sfx/Narration/vce_sel_album", "Sound/Sfx/Narration/vce_sel_mission",
			"Sound/Sfx/Narration/vce_sel_music", "Sound/Sfx/Narration/vce_title", "Sound/Sfx/ClubTour/disc_spread", "Sound/Sfx/ClubTour/touch_mission", "Sound/Sfx/ClubTour/touch_pack"
		};
		string[] array2 = new string[]
		{
			"Sound/Bgm/bgm_albumsel", "Sound/Bgm/bgm_discsel", "Sound/Bgm/bgm_ending", "Sound/Bgm/bgm_login", "Sound/Bgm/bgm_modesel", "Sound/Bgm/bgm_raveresult", "Sound/Bgm/bgm_result", "Sound/Bgm/bgm_title", "Sound/Bgm/bgm_totalresult", "Sound/Bgm/bgm_missionsel",
			"Sound/Bgm/bgm_clubresult", "Sound/Bgm/tuto_fix", "Sound/Bgm/ranking_bgm"
		};
		for (int i = 0; i < 75; i++)
		{
			AudioClip audioClip = (AudioClip)Resources.Load(array[i], typeof(AudioClip));
			this.audioSources[i] = base.gameObject.AddComponent("AudioSource") as AudioSource;
			this.audioSources[i].volume = 1f;
			this.audioSources[i].Stop();
			this.audioSources[i].clip = audioClip;
			this.audioSources[i].loop = false;
			this.audioSources[i].playOnAwake = false;
		}
		for (int j = 0; j < 13; j++)
		{
			this.BgmClips[j] = (AudioClip)Resources.Load(array2[j], typeof(AudioClip));
		}
		this.BgmAudio = base.gameObject.AddComponent("AudioSource") as AudioSource;
		this.BgmAudio.volume = 1f;
		this.BgmAudio.loop = false;
		this.BgmAudio.playOnAwake = false;
		this.BgmAudio.Stop();
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0000A42A File Offset: 0x0000862A
	public void SetEffSound(int iIdx, AudioClip aEffect)
	{
		this.EffSource[iIdx].clip = aEffect;
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0000A43A File Offset: 0x0000863A
	public AudioSource getAudioSource(SOUNDINDEX eIndex)
	{
		return this.audioSources[(int)eIndex];
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0000A444 File Offset: 0x00008644
	public void Play(SOUNDINDEX eIndex, bool bLoop = false)
	{
		this.audioSources[(int)eIndex].loop = bLoop;
		this.audioSources[(int)eIndex].Play();
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0000A461 File Offset: 0x00008661
	public void Stop(SOUNDINDEX eIndex)
	{
		this.audioSources[(int)eIndex].Stop();
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0000A470 File Offset: 0x00008670
	public void Pause(SOUNDINDEX eIndex)
	{
		this.audioSources[(int)eIndex].Pause();
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0000A47F File Offset: 0x0000867F
	public void SetBgm(AudioClip aBgm, bool Loop = false)
	{
		if (this.m_bLock)
		{
			return;
		}
		this.BgmAudio.clip = aBgm;
		this.BgmAudio.loop = Loop;
		this.BgmAudio.panLevel = 0f;
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0000A4B2 File Offset: 0x000086B2
	public AudioSource getNowBGM()
	{
		return this.BgmAudio;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0000A4BA File Offset: 0x000086BA
	public void SetBgmTime(float fTime)
	{
		this.BgmAudio.time = fTime;
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0000A4C8 File Offset: 0x000086C8
	public void PlayBgm()
	{
		this.BgmAudio.Play();
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0000A4D5 File Offset: 0x000086D5
	public float GetBgmElapsedTime()
	{
		return this.BgmAudio.time;
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0000A4E2 File Offset: 0x000086E2
	public void LoadLockBgm(bool bLock)
	{
		this.m_bLock = bLock;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0000A4EB File Offset: 0x000086EB
	public void PlayBgm(BGMINDEX eBgm, bool Loop = false)
	{
		this.BgmAudio.clip = this.BgmClips[(int)eBgm];
		this.BgmAudio.loop = Loop;
		this.BgmAudio.Play();
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0000A517 File Offset: 0x00008717
	public void StopBgm()
	{
		this.BgmAudio.Stop();
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0004B41C File Offset: 0x0004961C
	public void SetRandKeyIndex()
	{
		int num = UnityEngine.Random.Range(0, 20);
		this.EFF_NUM = num;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0004B43C File Offset: 0x0004963C
	public void Effect(int iGroup)
	{
		this.iHunGroup = iGroup;
		if (this.EFF_NUM != -1)
		{
			if (Singleton<GameManager>.instance.inAttract() && !Singleton<GameManager>.instance.DemoSound)
			{
				return;
			}
			SoundSourceManager.fnSndProxyStopSample(this.EFF_NUM.ToString() + ".ogg");
			SoundSourceManager.fnSndProxyPlaySample(this.EFF_NUM.ToString() + ".ogg", this.EFF_VOLUME, (float)this.EFF_VARIABLE, false);
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0004B4B8 File Offset: 0x000496B8
	public void InitBGM()
	{
		Dictionary<string, object> dictionary = Json.Deserialize(Singleton<GameManager>.instance.ReadSystemJSONFile("BGMLibrary")) as Dictionary<string, object>;
		foreach (string text in dictionary.Keys)
		{
			List<SoundSourceManager.BgmClip> list = new List<SoundSourceManager.BgmClip>();
			foreach (object obj in ((dictionary[text] as Dictionary<string, object>)["sounds"] as List<object>))
			{
				string text2 = (string)obj;
				string text3 = string.Concat(new string[] { "System/Sound/bgm/", text, "/", text2, ".wav" });
				WWW www = new WWW("file:///" + Path.GetFullPath("../Data/") + text3);
				while (!www.isDone)
				{
				}
				AudioClip audioClip = www.GetAudioClip(false);
				SoundSourceManager.BgmClip bgmClip = new SoundSourceManager.BgmClip
				{
					Name = text2,
					Clip = audioClip
				};
				list.Add(bgmClip);
			}
			this.namedBgmClips[text] = list;
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0004B614 File Offset: 0x00049814
	public AudioClip GetBgmClip(string key, string name)
	{
		SoundSourceManager.BgmClip bgmClip = this.namedBgmClips[key].Find((SoundSourceManager.BgmClip b) => b.Name == name);
		if (bgmClip == null)
		{
			bgmClip = this.namedBgmClips["cyclon"].Find((SoundSourceManager.BgmClip b) => b.Name == name);
		}
		return bgmClip.Clip;
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0004B678 File Offset: 0x00049878
	public void PlayNamedBgm(string BGMname, bool Loop = false)
	{
		string setBGM = Singleton<GameManager>.instance.UserData.SetBGM;
		this.BgmAudio.clip = this.GetBgmClip(setBGM, BGMname);
		this.BgmAudio.loop = Loop;
		this.BgmAudio.Play();
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0004B6C0 File Offset: 0x000498C0
	public void PlayRandomBgm(string BGMName, bool Loop = false)
	{
		string[] array = new string[] { "cyclon", "xonic", "technika3", "technika2", "technika1", "technika_tune", "extreme" };
		int num = UnityEngine.Random.Range(0, array.Length);
		string text = array[num];
		this.BgmAudio.clip = this.GetBgmClip(text, BGMName);
		this.BgmAudio.loop = Loop;
		this.BgmAudio.Play();
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0000A524 File Offset: 0x00008724
	public void PlaySystemBgm(string BGMname, bool Loop = false)
	{
		this.BgmAudio.clip = this.GetBgmClip("system", BGMname);
		this.BgmAudio.loop = Loop;
		this.BgmAudio.Play();
	}

	// Token: 0x04000A01 RID: 2561
	public AudioSource[] audioSources = new AudioSource[75];

	// Token: 0x04000A02 RID: 2562
	private AudioClip[] BgmClips = new AudioClip[13];

	// Token: 0x04000A03 RID: 2563
	public AudioSource BgmAudio = new AudioSource();

	// Token: 0x04000A04 RID: 2564
	public AudioSource[] EffSource = new AudioSource[5];

	// Token: 0x04000A05 RID: 2565
	public int iHunGroup;

	// Token: 0x04000A06 RID: 2566
	public int EFF_NUM;

	// Token: 0x04000A07 RID: 2567
	public float EFF_VOLUME = 1f;

	// Token: 0x04000A08 RID: 2568
	public int EFF_VARIABLE = 20;

	// Token: 0x04000A09 RID: 2569
	public bool m_bLock;

	// Token: 0x04000A0A RID: 2570
	private Dictionary<string, List<SoundSourceManager.BgmClip>> namedBgmClips = new Dictionary<string, List<SoundSourceManager.BgmClip>>();

	// Token: 0x0200014C RID: 332
	private class BgmClip
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0000A554 File Offset: 0x00008754
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x0000A55C File Offset: 0x0000875C
		public string Name { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0000A565 File Offset: 0x00008765
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0000A56D File Offset: 0x0000876D
		public AudioClip Clip { get; set; }
	}
}
