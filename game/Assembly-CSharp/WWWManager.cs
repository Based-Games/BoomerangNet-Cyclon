using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class WWWManager : Singleton<WWWManager>
{
	// Token: 0x06000B0D RID: 2829 RVA: 0x000507A4 File Offset: 0x0004E9A4
	private void Awake()
	{
		string text = ConfigManager.Instance.Get<string>("network.server_url", false);
		this.SERVER_URL = text;
		this.WEBSONG_URL = text;
		this.WEBSONGDATA_URL = text;
		Logger.Log("WWWManager", "Initialized service URL: " + text, new object[0]);
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0000A72C File Offset: 0x0000892C
	public void SetUrl()
	{
		if (GameData.INCOMETEST)
		{
			this.SERVER_URL = "http://ec2-54-169-98-174.ap-southeast-1.compute.amazonaws.com/cyclon/";
		}
		bool isDebugBuild = Debug.isDebugBuild;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0000A746 File Offset: 0x00008946
	public void AddQueue(WWWObject wObj)
	{
		this.m_qCallback.Enqueue(wObj);
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x000090F9 File Offset: 0x000072F9
	private bool IsContain(WWWObject wObj)
	{
		return false;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x000507F4 File Offset: 0x0004E9F4
	private void Update()
	{
		if (0 < this.m_qCallback.Count)
		{
			WWWObject wwwobject = this.m_qCallback.Dequeue();
			this.m_arrCallBack.Add(wwwobject);
			base.StartCoroutine(this.StartLoadAsync(wwwobject));
		}
		for (int i = 0; i < this.m_arrCallBack.Count; i++)
		{
			WWWObject wwwobject2 = (WWWObject)this.m_arrCallBack[i];
			wwwobject2.m_tTime += Time.deltaTime;
			if (7f < wwwobject2.m_tTime)
			{
				if (wwwobject2.CallBackFail != null)
				{
					wwwobject2.CallBackFail();
				}
				wwwobject2.CheckCancel = true;
				wwwobject2.www.Dispose();
				wwwobject2.www = null;
				this.m_arrCallBack.Remove(wwwobject2);
			}
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0000A754 File Offset: 0x00008954
	protected IEnumerator StartLoadAsync(WWWObject wwwObject)
	{
		wwwObject.StartLoad();
		yield return wwwObject.www;
		if (wwwObject != null && this.m_arrCallBack.Contains(wwwObject))
		{
			wwwObject.CompleteLoad();
			wwwObject.www.Dispose();
			wwwObject.www = null;
			this.m_arrCallBack.Remove(wwwObject);
		}
		yield break;
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x000508B8 File Offset: 0x0004EAB8
	public void Cancel()
	{
		this.m_qCallback.Clear();
		foreach (object obj in this.m_arrCallBack)
		{
			WWWObject wwwobject = (WWWObject)obj;
			wwwobject.CheckCancel = true;
			wwwobject.www.Dispose();
			wwwobject.www = null;
		}
		this.m_arrCallBack.Clear();
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00050938 File Offset: 0x0004EB38
	public void GetGameCenterPoint()
	{
		WWWGameCenterPoint wwwgameCenterPoint = new WWWGameCenterPoint();
		Singleton<WWWManager>.instance.AddQueue(wwwgameCenterPoint);
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00050958 File Offset: 0x0004EB58
	public void PostHausTotalResult()
	{
		WWWPostHausTotalResult wwwpostHausTotalResult = new WWWPostHausTotalResult();
		this.AddQueue(wwwpostHausTotalResult);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00050974 File Offset: 0x0004EB74
	public void GetNewSongEventCheck()
	{
		WWWDocumentData wwwdocumentData = new WWWDocumentData();
		wwwdocumentData.strPath = "newsongevent.txt";
		wwwdocumentData.CallReturnBack = new WWWObject.CompleteReturnCallBack(this.CallBacNewSongEvent);
		Singleton<WWWManager>.instance.AddQueue(wwwdocumentData);
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x000509B0 File Offset: 0x0004EBB0
	public void GetEmergencyCheck()
	{
		WWWDocumentData wwwdocumentData = new WWWDocumentData();
		wwwdocumentData.strPath = "Emergency.txt";
		wwwdocumentData.CallReturnBack = new WWWObject.CompleteReturnCallBack(this.CallBackEmergency);
		Singleton<WWWManager>.instance.AddQueue(wwwdocumentData);
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x000509EC File Offset: 0x0004EBEC
	public void GetNotice()
	{
		WWWNotice wwwnotice = new WWWNotice();
		wwwnotice.CallBackFail = new WWWObject.CompleteCallBack(this.CallBackNotNetwork);
		Singleton<WWWManager>.instance.AddQueue(wwwnotice);
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0000A76A File Offset: 0x0000896A
	public void CallBackNotNetwork()
	{
		if (Singleton<GameManager>.instance.ONNETWORK)
		{
			Singleton<GameManager>.instance.ONNETWORK = false;
		}
		Singleton<WWWManager>.instance.GetEmergencyCheck();
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0000A78D File Offset: 0x0000898D
	private void CallBackEmergency(string strContent)
	{
		if (strContent.Contains("0"))
		{
			GameData.CheckSystem = CHECKSYSTEM.NONE;
		}
		if (strContent.Contains("1"))
		{
			GameData.CheckSystem = CHECKSYSTEM.REGULAR;
		}
		if (strContent.Contains("2"))
		{
			GameData.CheckSystem = CHECKSYSTEM.EMERGENCY;
		}
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0000A7C8 File Offset: 0x000089C8
	private void CallBacNewSongEvent(string strContent)
	{
		if (strContent.Contains("0"))
		{
			Singleton<GameManager>.instance.isNewSongEvent = false;
		}
		if (strContent.Contains("1"))
		{
			Singleton<GameManager>.instance.isNewSongEvent = true;
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00050A1C File Offset: 0x0004EC1C
	public void CallBackPostHausFailTotalResult()
	{
		WWWPostHausFailTotalResult wwwpostHausFailTotalResult = new WWWPostHausFailTotalResult();
		this.AddQueue(wwwpostHausFailTotalResult);
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00050A38 File Offset: 0x0004EC38
	public void CallBackLogin()
	{
		WWWServerLv wwwserverLv = new WWWServerLv();
		this.AddQueue(wwwserverLv);
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence(userData.Name + " logged in!", "Waiting to start...");
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			userData.TexIcon = (Texture)Resources.Load("Common/UserIcon/usericon");
			return;
		}
		if (Singleton<SongManager>.instance.DicIconInfo.ContainsKey(userData.Icon))
		{
			userData.TexIcon = (Texture)Resources.Load("Common/UserIcon/" + Singleton<SongManager>.instance.DicIconInfo[userData.Icon].ToString());
			return;
		}
		userData.TexIcon = (Texture)Resources.Load("Common/UserIcon/1");
		if (!userData.WebIcon.Contains("null"))
		{
			WWWIcon wwwicon = new WWWIcon();
			this.AddQueue(wwwicon);
			return;
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0000A7FA File Offset: 0x000089FA
	public void LoadPreview(string strName)
	{
		base.StartCoroutine(this.LoadAsycnPreview(strName));
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0000A80A File Offset: 0x00008A0A
	private IEnumerator LoadAsycnPreview(string strName)
	{
		string text = "Preview/" + strName + ".";
		string text2 = Path.GetFullPath("../Data/") + text;
		if (File.Exists(text2 + "unity3d"))
		{
			AssetBundle assetBundle = new WWW("file:///" + text2 + "unity3d").assetBundle;
			Singleton<SoundSourceManager>.instance.SetBgm(assetBundle.Load(strName, typeof(AudioClip)) as AudioClip, false);
			assetBundle.Unload(false);
			if (this.CallBackPreview != null)
			{
				this.CallBackPreview();
			}
			yield break;
		}
		WWW www = new WWW("file:///" + text2 + (File.Exists(text2 + "ogg") ? "ogg" : "wav"));
		while (!www.isDone)
		{
		}
		AudioClip audioClip = www.audioClip;
		Singleton<SoundSourceManager>.instance.SetBgm(audioClip, false);
		if (this.CallBackPreview != null)
		{
			this.CallBackPreview();
		}
		yield break;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00050B24 File Offset: 0x0004ED24
	public void PostConfiguration()
	{
		WWWPostConfigration wwwpostConfigration = new WWWPostConfigration();
		Singleton<WWWManager>.instance.AddQueue(wwwpostConfigration);
	}

	// Token: 0x04000A87 RID: 2695
	private const string REALSERVER_URL = "https://api.playcyclon.com/cyclon/";

	// Token: 0x04000A88 RID: 2696
	private const string INCOMEENSERVER_URL = "http://ec2-54-169-98-174.ap-southeast-1.compute.amazonaws.com/cyclon/";

	// Token: 0x04000A89 RID: 2697
	private const string TESTSERVER_URL = "http://192.168.0.171:8080/cyclon/";

	// Token: 0x04000A8A RID: 2698
	public const string WEB_COMMONDATA = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/Common.csv";

	// Token: 0x04000A8B RID: 2699
	public const string WEB_DISCSTOCK = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/DiscStock.csv";

	// Token: 0x04000A8C RID: 2700
	public const string WEB_LEVELUP = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/LevelUp.csv";

	// Token: 0x04000A8D RID: 2701
	public const string WEB_HOUSESTAGE = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/HouseStage.csv";

	// Token: 0x04000A8E RID: 2702
	public const string WEB_RAVEUPALBUM = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/RaveupAlbum.csv";

	// Token: 0x04000A8F RID: 2703
	public const string WEB_RAVEUPSTAGE = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/RaveupStage.csv";

	// Token: 0x04000A90 RID: 2704
	public const string WEB_CLUBMISSION = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Scripts/Mission.csv";

	// Token: 0x04000A91 RID: 2705
	public const string WEB_EMERGENCY = "http://patch.playcyclon.com/Emergency.txt";

	// Token: 0x04000A92 RID: 2706
	public const string WEB_NEWSONGEVENT = "http://patch.playcyclon.com/newsongevent.txt";

	// Token: 0x04000A93 RID: 2707
	private const float TIMEOUT = 7f;

	// Token: 0x04000A94 RID: 2708
	public string SERVER_URL = "https://api.playcyclon.com/cyclon/";

	// Token: 0x04000A95 RID: 2709
	public string WEBSONG_URL = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/Songs/";

	// Token: 0x04000A96 RID: 2710
	public string WEBSONGDATA_URL = "https://s3-ap-northeast-1.amazonaws.com/cyclonwebdata/Test/SongData/";

	// Token: 0x04000A97 RID: 2711
	public string SECRET_KEY = "747269706c65444553";

	// Token: 0x04000A98 RID: 2712
	private ArrayList m_arrCallBack = new ArrayList();

	// Token: 0x04000A99 RID: 2713
	private Queue<WWWObject> m_qCallback = new Queue<WWWObject>();

	// Token: 0x04000A9A RID: 2714
	public string authorization = string.Empty;

	// Token: 0x04000A9B RID: 2715
	public string userid = string.Empty;

	// Token: 0x04000A9C RID: 2716
	public WWWManager.CallBackLoadPreview CallBackPreview;

	// Token: 0x0200016C RID: 364
	// (Invoke) Token: 0x06000B22 RID: 2850
	public delegate void CallBackLoadPreview();
}
