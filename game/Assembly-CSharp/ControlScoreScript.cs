using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class ControlScoreScript : MonoBehaviour
{
	// Token: 0x060008C0 RID: 2240 RVA: 0x00042C6C File Offset: 0x00040E6C
	public void SetObject()
	{
		this.m_rData = Singleton<GameManager>.instance.ResultData;
		this.m_tScore = base.transform.FindChild("txtScore").GetComponent<UILabel>();
		this.m_tBestScore = base.transform.FindChild("txtBestScore").GetComponent<UILabel>();
		this.m_oViewStage = base.transform.FindChild("ViewStage").gameObject;
		this.m_oViewSongInfo = base.transform.FindChild("ViewSong").gameObject;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00042CF8 File Offset: 0x00040EF8
	private void SetBestScore()
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			this.m_rData.BESTSCORE = userData.BestScore;
			return;
		}
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			USERDATA userData2 = Singleton<GameManager>.instance.UserData;
			if (userData2.BestScore > this.m_rData.BESTSCORE)
			{
				this.m_rData.BESTSCORE = userData2.BestScore;
				return;
			}
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			USERDATA userData3 = Singleton<GameManager>.instance.UserData;
			if (userData3.BestScore > this.m_rData.BESTSCORE)
			{
				this.m_rData.BESTSCORE = userData3.BestScore;
			}
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00042DA8 File Offset: 0x00040FA8
	private void Start()
	{
		this.SetBestScore();
		this.ViewScore();
		UISprite component = this.m_oViewStage.transform.FindChild("ViewStage").GetComponent<UISprite>();
		UISprite component2 = this.m_oViewStage.transform.FindChild("ViewMode").GetComponent<UISprite>();
		PTLEVEL ptlevel = PTLEVEL.EZ;
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			component2.spriteName = "HausMix";
			string[] array = new string[] { "HausStage1", "HausStage2", "HausStage3" };
			component.spriteName = array[GameData.Stage];
			ptlevel = Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage].PtType;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			component2.spriteName = "RaveUp";
			string[] array2 = new string[] { "RaveStage1", "RaveStage2", "RaveStage3", "RaveStage4" };
			component.spriteName = array2[GameData.Stage];
			ptlevel = Singleton<SongManager>.instance.RaveUpSelectSong[GameData.Stage].PtType;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			component2.spriteName = "MissionTitle";
			string[] array3 = new string[] { "ClubStage1", "ClubStage2", "ClubStage3" };
			component.spriteName = array3[GameData.Stage];
			component.transform.localPosition = new Vector3(130f, 2.115662f, 0f);
			ptlevel = Singleton<SongManager>.instance.Mission.Pattern[GameData.Stage];
		}
		component.MakePixelPerfect();
		component2.MakePixelPerfect();
		Hashtable hashtable = new Hashtable();
		hashtable["position"] = new Vector3(-980f, -298f, 0f);
		hashtable["time"] = 1f;
		hashtable["islocal"] = true;
		hashtable["looptype"] = iTween.LoopType.pingPong;
		hashtable["delay"] = 5f;
		iTween.MoveTo(this.m_oViewStage, hashtable);
		hashtable = new Hashtable();
		hashtable["position"] = new Vector3(-525f, -298f, 0f);
		hashtable["time"] = 1f;
		hashtable["islocal"] = true;
		hashtable["looptype"] = iTween.LoopType.pingPong;
		hashtable["delay"] = 5f;
		iTween.MoveTo(this.m_oViewSongInfo, hashtable);
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		bool flag = GameData.isContainHangul(currentDisc.FullName);
		this.m_oViewSongInfo.transform.FindChild("txtTitle").GetComponent<UILabel>().text = currentDisc.FullName;
		this.m_oViewSongInfo.transform.FindChild("txtTitleKr").GetComponent<UILabel>().text = currentDisc.FullName;
		if (flag)
		{
			this.m_oViewSongInfo.transform.FindChild("txtTitle").gameObject.SetActive(false);
		}
		else
		{
			this.m_oViewSongInfo.transform.FindChild("txtTitleKr").gameObject.SetActive(false);
		}
		this.m_oViewSongInfo.transform.FindChild("txtCompose").GetComponent<UILabel>().text = currentDisc.Artist;
		this.m_oViewSongInfo.transform.FindChild("PtType").GetComponent<UISprite>().spriteName = GameData.PTTYPE_SPRITENAME[ptlevel];
		this.m_oViewSongInfo.transform.FindChild("PtType").GetComponent<UISprite>().MakePixelPerfect();
		this.m_oViewSongInfo.transform.FindChild("PtType").GetComponent<UISprite>().transform.localScale = Vector3.one * 0.7f;
		string text = currentDisc.Name.ToString();
		if (GameData.E_VERSION == VERSION.EN && (text.Contains("timeline") || text.Contains("morning")))
		{
			text += "_en";
		}
		string text2 = "SongDiscMini/" + text + ".";
		if (File.Exists(Path.GetFullPath("../Data/") + text2 + "unity3d"))
		{
			base.StartCoroutine(this.LoadDiscImage(text, text2 + "unity3d", this.m_oViewSongInfo.transform.FindChild("Texture").GetComponent<UITexture>()));
			return;
		}
		base.StartCoroutine(this.LoadDiscImagePng(text, text2 + "png", this.m_oViewSongInfo.transform.FindChild("Texture").GetComponent<UITexture>()));
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x000095E1 File Offset: 0x000077E1
	private IEnumerator LoadDiscImage(string strDiscName, string strFileName, UITexture uTexture)
	{
		AssetBundle assetBundle = new WWW("file:///" + Path.GetFullPath("../Data/") + strFileName).assetBundle;
		Texture texture = assetBundle.Load(strDiscName, typeof(Texture)) as Texture;
		uTexture.mainTexture = texture;
		assetBundle.Unload(false);
		yield break;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00043270 File Offset: 0x00041470
	public void ViewScore()
	{
		if (this.m_rData == null)
		{
			return;
		}
		int score = this.m_rData.SCORE;
		int bestscore = this.m_rData.BESTSCORE;
		this.m_tScore.text = string.Format("{0:0000000}", score);
		this.m_tBestScore.text = string.Format("{0:0000000}", bestscore);
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x000095FE File Offset: 0x000077FE
	private IEnumerator LoadDiscImagePng(string strDiscName, string strFileName, UITexture uTexture)
	{
		WWW www = new WWW("file:///" + Path.GetFullPath("../Data/") + strFileName);
		while (!www.isDone)
		{
		}
		uTexture.mainTexture = www.texture;
		yield break;
	}

	// Token: 0x04000726 RID: 1830
	private GameObject m_oViewStage;

	// Token: 0x04000727 RID: 1831
	private GameObject m_oViewSongInfo;

	// Token: 0x04000728 RID: 1832
	private UILabel m_tScore;

	// Token: 0x04000729 RID: 1833
	private UILabel m_tBestScore;

	// Token: 0x0400072A RID: 1834
	private RESULTDATA m_rData;
}
