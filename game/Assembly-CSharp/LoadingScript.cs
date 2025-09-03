using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class LoadingScript : MonoBehaviour
{
	// Token: 0x06000783 RID: 1923 RVA: 0x00008D1A File Offset: 0x00006F1A
	private void Awake()
	{
		Logger.Log("LoadingScript", "Initialized LoadingScript", new object[0]);
		if (Singleton<GameManager>.instance.inAttract())
		{
			this.CompleteTime = 3f;
			Singleton<GameManager>.instance.InitNetData();
		}
		this.SetObject();
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00008D58 File Offset: 0x00006F58
	private void Start()
	{
		this.SetLoading();
		this.FadeIn();
		base.StartCoroutine(this.LoadAsyncSong());
		base.StartCoroutine(this.LoadAsyncPt());
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00038FCC File Offset: 0x000371CC
	private void SetObject()
	{
		GameObject gameObject = base.transform.FindChild("Ui").gameObject;
		this.m_oControlLoading = gameObject.transform.FindChild("PanelLoading").gameObject;
		this.m_tLoading = this.m_oControlLoading.transform.FindChild("Loading").GetComponent<UITexture>();
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00008D80 File Offset: 0x00006F80
	private IEnumerator LoadAsyncSong()
	{
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		string text = "Song/" + currentDisc.Name + ".";
		string text2 = Path.GetFullPath("../Data/") + text;
		WWW www = new WWW("file:///" + text2 + (File.Exists(text2 + "ogg") ? "ogg" : "wav"));
		while (!www.isDone)
		{
		}
		AudioClip audioClip = www.audioClip;
		Singleton<SoundSourceManager>.instance.SetBgm(audioClip, false);
		Logger.Log("LoadingScript", "Loaded: " + text, new object[0]);
		yield break;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00008D88 File Offset: 0x00006F88
	private IEnumerator LoadAsyncPt()
	{
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		int num;
		switch (Singleton<SongManager>.instance.Mode)
		{
		case GAMEMODE.HAUSMIX:
			num = (int)Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage].PtType;
			break;
		case GAMEMODE.RAVEUP:
			num = (int)Singleton<SongManager>.instance.RaveUpSelectSong[GameData.Stage].PtType;
			break;
		case GAMEMODE.MISSION:
			num = (int)Singleton<SongManager>.instance.Mission.Pattern[GameData.Stage];
			break;
		default:
			throw new NotImplementedException();
		}
		string text = string.Format("{0}_{1}", currentDisc.Name, num);
		string text2 = "Pt/" + text + ".";
		string text3 = Path.GetFullPath("../Data/") + text2;
		if (File.Exists(text3 + "unity3d"))
		{
			AssetBundle assetBundle = new WWW("file:///" + text3 + "unity3d").assetBundle;
			string text4 = (assetBundle.Load(text, typeof(TextAsset)) as TextAsset).text;
			this.DelegateComplete(text4);
			this.m_bCompleteLoad = true;
			assetBundle.Unload(false);
			Logger.Log("LoadingScript", "Loaded: " + text2 + " (UNITY3D)", new object[0]);
			yield break;
		}
		using (StreamReader streamReader = new StreamReader(text3 + "xml"))
		{
			string text5 = streamReader.ReadToEnd();
			this.DelegateComplete(text5);
		}
		this.m_bCompleteLoad = true;
		Logger.Log("LoadingScript", "Loaded: " + text2 + " (XML)", new object[0]);
		yield break;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00008D97 File Offset: 0x00006F97
	private void SetManager(GameObject oManager)
	{
		this.m_oGameManager = oManager;
		this.m_sGameManager = this.m_oGameManager.GetComponent<GameManagerScript>();
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00008DB1 File Offset: 0x00006FB1
	private void DelegateComplete(string strText)
	{
		this.m_strPt = strText;
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0003902C File Offset: 0x0003722C
	private void Update()
	{
		if (this.m_bCompleteLoad)
		{
			this.m_fTime += Time.deltaTime;
			if (this.CompleteTime < this.m_fTime)
			{
				this.m_sGameManager.SendMessage("Resume");
				this.m_sGameManager.CompletePt(this.m_strPt);
				base.enabled = false;
			}
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00008DBA File Offset: 0x00006FBA
	public void CloseLoad()
	{
		this.m_tLoading.gameObject.SetActive(false);
		this.m_oControlLoading.transform.FindChild("RaveUpStage").gameObject.SetActive(false);
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0003908C File Offset: 0x0003728C
	private void SetLoading()
	{
		GameObject gameObject = this.m_oControlLoading.transform.Find("RaveUpStage").gameObject;
		GameObject gameObject2 = this.m_oControlLoading.transform.Find("Loading").gameObject;
		gameObject2.SetActive(true);
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			gameObject.SetActive(false);
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_1280, null, gameObject2.GetComponent<UITexture>(), null, null);
			return;
		}
		if (Singleton<SongManager>.instance.Mode != GAMEMODE.RAVEUP)
		{
			if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
			{
				gameObject.SetActive(false);
				MissionPackData missionPack = Singleton<SongManager>.instance.GetMissionPack(Singleton<SongManager>.instance.Mission.iPackId);
				Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.MISSIONPACK_1280, null, gameObject2.GetComponent<UITexture>(), null, missionPack);
			}
			return;
		}
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.ALBUM_1280, null, gameObject2.GetComponent<UITexture>(), null, null);
		if (GameData.Stage == 0)
		{
			base.Invoke("SetRaveUpStage", 0.0001f);
			return;
		}
		this.SetRaveUpStage();
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00039180 File Offset: 0x00037380
	private void SetRaveUpStage()
	{
		AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
		RaveUpStage raveUpAlbumHiddenStage = Singleton<SongManager>.instance.GetRaveUpAlbumHiddenStage(currentAlbum.Id);
		Singleton<SongManager>.instance.RaveUpSelectSong[3] = raveUpAlbumHiddenStage;
		GameObject gameObject = this.m_oControlLoading.transform.FindChild("RaveUpStage").gameObject;
		gameObject.SetActive(true);
		for (int i = 0; i < 4; i++)
		{
			string text = "StageSlot" + i.ToString();
			GameObject gameObject2 = gameObject.transform.FindChild(text).gameObject;
			if (GameData.Stage == i)
			{
				gameObject2.GetComponent<UISprite>().spriteName = "SelectStageSlot";
				gameObject2.GetComponent<UISprite>().MakePixelPerfect();
				gameObject2.transform.FindChild("Mark").gameObject.SetActive(true);
				gameObject2.transform.FindChild("txtStage").GetComponent<UISprite>().spriteName = "txtSelectStage";
				gameObject2.transform.FindChild("txtStage").GetComponent<UISprite>().MakePixelPerfect();
			}
			else
			{
				gameObject2.GetComponent<UISprite>().spriteName = "StageSlot";
				gameObject2.GetComponent<UISprite>().MakePixelPerfect();
				gameObject2.transform.FindChild("Mark").gameObject.SetActive(false);
				gameObject2.transform.FindChild("txtStage").GetComponent<UISprite>().spriteName = "txtStage";
				gameObject2.transform.FindChild("txtStage").GetComponent<UISprite>().MakePixelPerfect();
			}
			gameObject2.transform.FindChild("Disc").gameObject.SetActive(true);
			RaveUpStage raveUpStage = Singleton<SongManager>.instance.RaveUpSelectSong[i];
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage.iSong);
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, discInfo, gameObject2.transform.FindChild("Disc").GetComponent<UITexture>(), null, null);
			if (i == 3 && GameData.Stage < 3)
			{
				gameObject2.transform.FindChild("Disc").gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00008DED File Offset: 0x00006FED
	private void FadeIn()
	{
		this.m_tLoading.gameObject.GetComponent<TweenAlpha>().enabled = false;
	}

	// Token: 0x04000606 RID: 1542
	private GameObject m_oGameManager;

	// Token: 0x04000607 RID: 1543
	private GameManagerScript m_sGameManager;

	// Token: 0x04000608 RID: 1544
	private GameObject m_oControlLoading;

	// Token: 0x04000609 RID: 1545
	private float m_fTime;

	// Token: 0x0400060A RID: 1546
	private bool m_bCompleteLoad;

	// Token: 0x0400060B RID: 1547
	private float CompleteTime = 4f;

	// Token: 0x0400060C RID: 1548
	private string m_strPt = string.Empty;

	// Token: 0x0400060D RID: 1549
	private UITexture m_tLoading;

	// Token: 0x0400060E RID: 1550
	public float fadeInTime = 0.5f;

	// Token: 0x0400060F RID: 1551
	public float targetAlpha = 1f;

	// Token: 0x04000610 RID: 1552
	private Color originalColor;
}
