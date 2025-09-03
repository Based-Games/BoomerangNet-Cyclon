using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class RaveUpDisc : MonoBehaviour
{
	// Token: 0x06000ED8 RID: 3800 RVA: 0x0006AC08 File Offset: 0x00068E08
	private void Awake()
	{
		this.m_txDiscImage = base.transform.FindChild("Texture_Disc").GetComponent<UITexture>();
		this.m_tStarGrid = base.transform.FindChild("StarGrid");
		this.m_tcdHome = base.transform.FindChild("CDHome");
		this.m_gCD = this.m_tcdHome.FindChild("CD").gameObject;
		this.m_spDiscLevel = base.transform.FindChild("DiscLevel").GetComponent<UISprite>();
		this.m_gSelectDiscBG = base.transform.FindChild("DiscSelectBG").gameObject;
		this.m_StarPrefab = Resources.Load("Prefab/DiscDifficultStar") as GameObject;
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0000CE42 File Offset: 0x0000B042
	private void Start()
	{
		this.m_txDiscImage.alpha = 1f;
		this.m_gSelectDiscBG.SetActive(false);
		this.m_RaveUpCD = this.m_gCD.GetComponent<RaveUpCD>();
		this.m_gCD.SetActive(false);
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0006ACC4 File Offset: 0x00068EC4
	public void StarSetting(int Level)
	{
		this.m_RaveUpCD.m_iLevel = Level;
		for (int i = 0; i < this.m_tStarGrid.transform.childCount; i++)
		{
			UnityEngine.Object.DestroyObject(this.m_tStarGrid.transform.GetChild(i).gameObject);
		}
		if (this.m_StarPrefab != null)
		{
			int num = 0;
			for (int j = 0; j < 12; j++)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_StarPrefab);
				gameObject.transform.parent = this.m_tStarGrid.transform;
				gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
				string text = string.Empty;
				if (j >= 5 && j < 10)
				{
					text = "_1";
				}
				else if (j >= 10 && j < 13)
				{
					text = "_2";
				}
				gameObject.transform.localPosition = new Vector3(29f * (float)j + (float)(num * 10), 0f, 0f);
				if (Level <= j)
				{
					gameObject.GetComponent<StarSetting>().setStar(false, string.Empty);
				}
				else
				{
					gameObject.GetComponent<StarSetting>().setStar(true, text);
				}
			}
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0000CE7D File Offset: 0x0000B07D
	public void PlayDiscPreviewAudio()
	{
		Singleton<WWWManager>.instance.CallBackPreview = new WWWManager.CallBackLoadPreview(this.CompletePreview);
		Singleton<WWWManager>.instance.LoadPreview(this.m_sName);
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0000CEA5 File Offset: 0x0000B0A5
	private void CompletePreview()
	{
		if (this.m_bPreviewPlay)
		{
			Singleton<SoundSourceManager>.instance.PlayBgm();
			Singleton<SoundSourceManager>.instance.getNowBGM().loop = true;
			Singleton<SoundSourceManager>.instance.getNowBGM().time = 0f;
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0006AE28 File Offset: 0x00069028
	private void PressProcess()
	{
		this.m_fMainBGMTime = Singleton<SoundSourceManager>.instance.getNowBGM().time;
		Singleton<SoundSourceManager>.instance.StopBgm();
		this.m_bPreviewPlay = true;
		this.PlayDiscPreviewAudio();
		this.m_gSelectDiscBG.SetActive(true);
		this.m_txDiscImage.alpha = 0.4f;
		if (this.m_RaveUpCD.m_bisAttach)
		{
			return;
		}
		this.m_RaveUpCD.m_tAttachObj = null;
		this.m_RaveUpCD.m_bMoveState = true;
		this.m_gCD.GetComponent<UIPanel>().depth = 401;
		this.m_gCD.GetComponent<TweenPosition>().enabled = false;
		this.m_gCD.SetActive(true);
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		float num = iPhoneToMouse.instance.GetTouch(0).position.x / vector.x;
		float num2 = iPhoneToMouse.instance.GetTouch(0).position.y / vector.y;
		Vector3 vector2 = this.m_gCD.transform.parent.localPosition + this.m_gCD.transform.parent.parent.localPosition;
		this.m_gCD.transform.localPosition = new Vector3(num * 2560f - 1280f - vector2.x, num2 * 1536f - 768f - vector2.y, 0f);
		this.m_gCD.transform.localScale = Vector3.one;
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0006AFC0 File Offset: 0x000691C0
	private void DragEndProcess()
	{
		this.m_bPreviewPlay = false;
		Singleton<SoundSourceManager>.instance.StopBgm();
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_discsel", true);
		Singleton<SoundSourceManager>.instance.getNowBGM().time = this.m_fMainBGMTime;
		this.m_RaveUpCD.m_bMoveState = false;
		this.m_RaveUpCD.TweenAni();
		this.m_gCD.GetComponent<UIPanel>().depth = 400;
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
	public void HideCD()
	{
		if (!this.m_RaveUpCD.m_bisAttach)
		{
			this.m_gSelectDiscBG.SetActive(false);
			this.m_gCD.SetActive(false);
			this.m_txDiscImage.alpha = 1f;
			return;
		}
	}

	// Token: 0x04001040 RID: 4160
	[HideInInspector]
	public UITexture m_txDiscImage;

	// Token: 0x04001041 RID: 4161
	[HideInInspector]
	public Transform m_tStarGrid;

	// Token: 0x04001042 RID: 4162
	[HideInInspector]
	public UISprite m_spDiscLevel;

	// Token: 0x04001043 RID: 4163
	[HideInInspector]
	public GameObject m_gSelectDiscBG;

	// Token: 0x04001044 RID: 4164
	[HideInInspector]
	public GameObject m_gCD;

	// Token: 0x04001045 RID: 4165
	[HideInInspector]
	public Transform m_tcdHome;

	// Token: 0x04001046 RID: 4166
	[HideInInspector]
	public string m_sName;

	// Token: 0x04001047 RID: 4167
	private GameObject m_StarPrefab;

	// Token: 0x04001048 RID: 4168
	private RaveUpCD m_RaveUpCD;

	// Token: 0x04001049 RID: 4169
	private bool m_bMoveState;

	// Token: 0x0400104A RID: 4170
	private AudioSource m_DiscSelectBgm = new AudioSource();

	// Token: 0x0400104B RID: 4171
	private bool m_bPreviewPlay;

	// Token: 0x0400104C RID: 4172
	private float m_fMainBGMTime;
}
