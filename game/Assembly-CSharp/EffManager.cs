using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class EffManager : MonoBehaviour
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x000599FC File Offset: 0x00057BFC
	private void Awake()
	{
		this.m_UIScrollManager = base.transform.FindChild("ScrollView").GetComponent<UIScroll>();
		this.m_EffGrid = this.m_UIScrollManager.transform.FindChild("Grid").transform;
		this.m_ScrollTweenPos = this.m_UIScrollManager.GetComponent<TweenPosition>();
		this.EffItemPrefab = Resources.Load("Prefab/EffectCell") as GameObject;
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0000B171 File Offset: 0x00009371
	private void Start()
	{
		base.Invoke("Init", Time.deltaTime);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0000B906 File Offset: 0x00009B06
	private void Init()
	{
		this.CreatEffItem();
		this.EffPositionSetting();
		this.Setting();
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00059A6C File Offset: 0x00057C6C
	private void CreateEffItemObj(int i, EffManager.EffKind kind, int Count = 0)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.EffItemPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.transform.parent = this.m_EffGrid;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		HouseMixCenterPosCheck component = gameObject.GetComponent<HouseMixCenterPosCheck>();
		component.m_UIScroll = this.m_UIScrollManager;
		component.setEffManager(base.GetComponent<EffManager>());
		component.m_iIndex = i;
		switch (kind)
		{
		case EffManager.EffKind.SPEED:
			gameObject.name = ((EFFECTOR_SPEED)i).ToString();
			component.setImage(((EFFECTOR_SPEED)i).ToString());
			break;
		case EffManager.EffKind.FADER:
			gameObject.name = ((EFFECTOR_FADER)i).ToString();
			component.setImage(((EFFECTOR_FADER)i).ToString());
			break;
		case EffManager.EffKind.RAND:
			gameObject.name = ((EFFECTOR_RAND)i).ToString();
			component.setImage(((EFFECTOR_RAND)i).ToString());
			break;
		case EffManager.EffKind.NORMALITEM:
			gameObject.name = ((PLAYFNORMALITEM)i).ToString();
			component.setImage(((PLAYFNORMALITEM)i).ToString());
			component.setCount(Count);
			break;
		case EffManager.EffKind.REFILLITEM:
			gameObject.name = ((PLAYFREFILLITEM)i).ToString();
			component.setImage(((PLAYFREFILLITEM)i).ToString());
			component.setCount(Count);
			break;
		case EffManager.EffKind.SHILDITEM:
			gameObject.name = ((PLAYFSHIELDITEM)i).ToString();
			component.setImage(((PLAYFSHIELDITEM)i).ToString());
			component.setCount(Count);
			break;
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00059C10 File Offset: 0x00057E10
	private void CreatEffItem()
	{
		switch (this.m_isKind)
		{
		case EffManager.EffKind.SPEED:
		{
			for (EFFECTOR_SPEED effector_SPEED = EFFECTOR_SPEED.X_0_5; effector_SPEED < EFFECTOR_SPEED.MAX; effector_SPEED++)
			{
				if (Singleton<GameManager>.instance.UserData.EffectorSpeedItem[effector_SPEED])
				{
					this.CreateEffItemObj((int)effector_SPEED, this.m_isKind, 0);
				}
			}
			break;
		}
		case EffManager.EffKind.FADER:
		{
			for (EFFECTOR_FADER effector_FADER = EFFECTOR_FADER.NONE; effector_FADER < EFFECTOR_FADER.MAX; effector_FADER++)
			{
				if (Singleton<GameManager>.instance.UserData.EffectorFaderItem[effector_FADER])
				{
					this.CreateEffItemObj((int)effector_FADER, this.m_isKind, 0);
				}
			}
			break;
		}
		case EffManager.EffKind.RAND:
		{
			for (EFFECTOR_RAND effector_RAND = EFFECTOR_RAND.NONE; effector_RAND < EFFECTOR_RAND.MAX; effector_RAND++)
			{
				if (Singleton<GameManager>.instance.UserData.EffectorRandItem[effector_RAND])
				{
					this.CreateEffItemObj((int)effector_RAND, this.m_isKind, 0);
				}
			}
			break;
		}
		case EffManager.EffKind.NORMALITEM:
		{
			for (PLAYFNORMALITEM playfnormalitem = PLAYFNORMALITEM.NONE; playfnormalitem < PLAYFNORMALITEM.MAX; playfnormalitem++)
			{
				this.CreateEffItemObj((int)playfnormalitem, this.m_isKind, 99);
			}
			break;
		}
		case EffManager.EffKind.REFILLITEM:
		{
			for (PLAYFREFILLITEM playfrefillitem = PLAYFREFILLITEM.NONE; playfrefillitem < PLAYFREFILLITEM.MAX; playfrefillitem++)
			{
				this.CreateEffItemObj((int)playfrefillitem, this.m_isKind, 99);
			}
			break;
		}
		case EffManager.EffKind.SHILDITEM:
		{
			for (PLAYFSHIELDITEM playfshielditem = PLAYFSHIELDITEM.NONE; playfshielditem < PLAYFSHIELDITEM.MAX; playfshielditem++)
			{
				this.CreateEffItemObj((int)playfshielditem, this.m_isKind, 99);
			}
			break;
		}
		}
		this.setPanelSize();
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x00059D54 File Offset: 0x00057F54
	private void setPanelSize()
	{
		UIPanel component = base.GetComponent<UIPanel>();
		int childCount = this.m_EffGrid.childCount;
		float num = 0f;
		float num2 = this.m_v2CellSize.y * 0.75f * (float)childCount + component.clipSoftness.y;
		if (num2 < 200f)
		{
			num2 = 200f;
		}
		else if (num2 > 800f)
		{
			num2 = 800f;
		}
		if (childCount == 2)
		{
			this.m_UIScrollManager.m_ScrollOption = UIScroll.ScrollOption_e.NormalCenter;
			this.m_UIScrollManager.CellsSetting(0);
			num2 = this.m_v2CellSize.y * 0.75f * 3f + component.clipSoftness.y;
		}
		else if (childCount > 2)
		{
			this.m_UIScrollManager.m_ScrollOption = UIScroll.ScrollOption_e.RollingAndCenter;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		component.baseClipRegion = new Vector4(0f, num, 200f, num2);
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x00059E50 File Offset: 0x00058050
	private void Setting()
	{
		if (GameData.Stage < 3)
		{
			if (GameData.Stage == 0)
			{
				this.m_iCenterIndex = (int)Singleton<GameManager>.instance.UserData.UserSpeed;
			}
			else
			{
				this.m_iCenterIndex = (int)GameData.SPEEDEFFECTOR;
			}
			string text = "NONE";
			switch (this.m_isKind)
			{
			case EffManager.EffKind.SPEED:
				this.m_UIScrollManager.transform.localPosition = new Vector3(0f, -this.m_v2CellSize.y * (float)this.m_iCenterIndex, 0f);
				text = this.m_EffGrid.GetChild(this.m_iCenterIndex).gameObject.name;
				GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)this.m_iCenterIndex;
				break;
			case EffManager.EffKind.FADER:
				this.m_iCenterIndex = 0;
				this.m_UIScrollManager.transform.localPosition = Vector3.zero;
				text = this.m_EffGrid.GetChild(this.m_iCenterIndex).gameObject.name;
				GameData.FADEEFFCTOR = (EFFECTOR_FADER)this.m_iCenterIndex;
				break;
			case EffManager.EffKind.RAND:
				this.m_iCenterIndex = 0;
				this.m_UIScrollManager.transform.localPosition = Vector3.zero;
				text = this.m_EffGrid.GetChild(this.m_iCenterIndex).gameObject.name;
				GameData.RANDEFFECTOR = (EFFECTOR_RAND)this.m_iCenterIndex;
				break;
			case EffManager.EffKind.NORMALITEM:
				this.m_iCenterIndex = -1;
				this.m_UIScrollManager.transform.localPosition = Vector3.zero;
				text = this.m_EffGrid.GetChild(0).gameObject.name;
				GameData.PLAYITEM = (PLAYFNORMALITEM)this.m_iCenterIndex;
				break;
			case EffManager.EffKind.REFILLITEM:
				this.m_iCenterIndex = -1;
				this.m_UIScrollManager.transform.localPosition = Vector3.zero;
				text = this.m_EffGrid.GetChild(0).gameObject.name;
				GameData.REFILLITEM = (PLAYFREFILLITEM)this.m_iCenterIndex;
				break;
			case EffManager.EffKind.SHILDITEM:
				this.m_iCenterIndex = -1;
				this.m_UIScrollManager.transform.localPosition = Vector3.zero;
				text = this.m_EffGrid.GetChild(0).gameObject.name;
				GameData.SHIELDITEM = (PLAYFSHIELDITEM)this.m_iCenterIndex;
				break;
			}
			for (int i = 0; i < this.m_EffGrid.childCount; i++)
			{
				(this.m_EffGrid.GetChild(i).collider as BoxCollider).center = Vector3.zero;
				if (this.m_EffGrid.GetChild(i).GetComponent<HouseMixCenterPosCheck>().m_iIndex == this.m_iCenterIndex)
				{
					this.m_EffGrid.GetChild(i).collider.enabled = true;
				}
				else
				{
					this.m_EffGrid.GetChild(i).collider.enabled = false;
				}
			}
			this.m_spIcon.spriteName = text;
			if (text != "NONE")
			{
				this.m_spIcon.spriteName = text + "_select";
			}
			this.m_spIcon.MakePixelPerfect();
			this.m_spIcon.transform.localScale = Vector3.one * 2f;
		}
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0005A14C File Offset: 0x0005834C
	private void EffPositionSetting()
	{
		for (int i = 0; i < this.m_EffGrid.childCount; i++)
		{
			if (this.m_EffGrid.GetChild(i).gameObject.activeSelf)
			{
				this.m_ieffCount++;
			}
		}
		this.m_iUpEffCount = (this.m_ieffCount - 1) / 2;
		this.m_iDownEffCount = this.m_ieffCount - 1 - this.m_iUpEffCount;
		int num = this.m_iUpEffCount;
		for (int j = 0; j < this.m_EffGrid.childCount; j++)
		{
			if (j <= this.m_iDownEffCount)
			{
				this.m_EffGrid.GetChild(j).localPosition = new Vector3(0f, (float)j * this.m_v2CellSize.y, 0f);
			}
			else
			{
				this.m_EffGrid.GetChild(j).localPosition = new Vector3(0f, (float)num * -this.m_v2CellSize.y, 0f);
				num--;
			}
		}
		this.m_UIScrollManager.CellsSetting(0);
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0000B91A File Offset: 0x00009B1A
	public void SetEndCenter()
	{
		this.m_bEndCenter = true;
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x0005A264 File Offset: 0x00058464
	private void PressProcess()
	{
		if (this.m_bisShow)
		{
			return;
		}
		this.m_bisShow = false;
		this.m_bisPress = true;
		this.m_fPressFrame = 0f;
		this.m_bPanelAniStart = false;
		this.m_bEndCenter = false;
		for (int i = 0; i < this.m_EffGrid.childCount; i++)
		{
			this.m_EffGrid.GetChild(i).collider.enabled = true;
			BoxCollider boxCollider = this.m_EffGrid.GetChild(i).collider as BoxCollider;
			boxCollider.center = new Vector3(0f, 0f, -20f);
		}
		base.CancelInvoke("ColliderReset");
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0000B0E3 File Offset: 0x000092E3
	private void DragProcess()
	{
		base.CancelInvoke("DragEndProcess");
		base.CancelInvoke("ColliderReset");
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0000B923 File Offset: 0x00009B23
	private void HideInit()
	{
		this.m_bisShow = false;
		this.m_bisPress = false;
		this.m_fPressFrame = 0f;
		this.m_bPanelAniStart = false;
		base.CancelInvoke("ColliderReset");
		base.Invoke("ColliderReset", 1f);
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0005A314 File Offset: 0x00058514
	public void SelectEffect(int selectIndex)
	{
		this.m_iCenterIndex = selectIndex;
		switch (this.m_isKind)
		{
		case EffManager.EffKind.SPEED:
			GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.FADER:
			GameData.FADEEFFCTOR = (EFFECTOR_FADER)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.RAND:
			GameData.RANDEFFECTOR = (EFFECTOR_RAND)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.NORMALITEM:
			GameData.PLAYITEM = (PLAYFNORMALITEM)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.REFILLITEM:
			GameData.REFILLITEM = (PLAYFREFILLITEM)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.SHILDITEM:
			GameData.SHIELDITEM = (PLAYFSHIELDITEM)this.m_iCenterIndex;
			break;
		}
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0005A3B4 File Offset: 0x000585B4
	private void ColliderReset()
	{
		this.m_spIcon.gameObject.SetActive(true);
		string text = "NONE";
		for (int i = 0; i < this.m_EffGrid.childCount; i++)
		{
			BoxCollider boxCollider = this.m_EffGrid.GetChild(i).collider as BoxCollider;
			boxCollider.center = Vector3.zero;
			if (this.m_EffGrid.GetChild(i).GetComponent<HouseMixCenterPosCheck>().m_iIndex == this.m_iCenterIndex)
			{
				this.m_EffGrid.GetChild(i).collider.enabled = true;
				text = this.m_EffGrid.GetChild(i).name;
				this.IconImageSetting(this.m_EffGrid.GetChild(i), "_select");
			}
			else
			{
				this.m_EffGrid.GetChild(i).collider.enabled = false;
				this.IconImageSetting(this.m_EffGrid.GetChild(i), string.Empty);
			}
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_EFFECT_MOUNT, false);
		switch (this.m_isKind)
		{
		case EffManager.EffKind.SPEED:
			GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.FADER:
			GameData.FADEEFFCTOR = (EFFECTOR_FADER)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.RAND:
			GameData.RANDEFFECTOR = (EFFECTOR_RAND)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.NORMALITEM:
			GameData.PLAYITEM = (PLAYFNORMALITEM)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.REFILLITEM:
			GameData.REFILLITEM = (PLAYFREFILLITEM)this.m_iCenterIndex;
			break;
		case EffManager.EffKind.SHILDITEM:
			GameData.SHIELDITEM = (PLAYFSHIELDITEM)this.m_iCenterIndex;
			break;
		}
		this.m_spIcon.spriteName = text;
		if (text != "NONE")
		{
			this.m_spIcon.spriteName = text + "_select";
		}
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
		base.GetComponent<TweenAlpha>().duration = 0.5f;
		base.GetComponent<TweenAlpha>().Play(false);
		this.m_bClickSelectUse = false;
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0000B960 File Offset: 0x00009B60
	private void DragEndProcess()
	{
		this.HideInit();
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0000B968 File Offset: 0x00009B68
	private void HideEffecter()
	{
		if (!this.m_bEndCenter)
		{
			return;
		}
		if (!this.m_bisShow)
		{
			return;
		}
		this.HideInit();
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x0005A5C0 File Offset: 0x000587C0
	private void ViewEffecter()
	{
		if (!this.m_bisPress)
		{
			return;
		}
		this.m_fPressFrame += Time.deltaTime;
		if (this.m_fPressFrame >= this.m_fPressTime)
		{
			this.m_fPressFrame = 0f;
			this.m_spIcon.gameObject.SetActive(false);
			this.m_bisPress = false;
			this.m_bPanelAniStart = true;
			this.m_TweenPanelAniObj.ResetToBeginning();
			this.m_TweenPanelAniObj.enabled = true;
			base.GetComponent<TweenAlpha>().Play(true);
			base.GetComponent<TweenAlpha>().duration = 0.15f;
			this.m_bisShow = true;
		}
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0005A660 File Offset: 0x00058860
	public void EffClickSelect(Transform tTarget)
	{
		if (!this.m_bClickSelectUse)
		{
			return;
		}
		float y = tTarget.transform.localPosition.y;
		base.CancelInvoke("DragEndProcess");
		this.m_ScrollTweenPos.enabled = false;
		this.m_ScrollTweenPos.transform.localPosition = new Vector3(0f, this.m_ScrollTweenPos.transform.localPosition.y - (this.m_ScrollTweenPos.transform.localPosition.y + y), 0f);
		this.HideInit();
		base.CancelInvoke("ColliderReset");
		this.ColliderReset();
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0005A710 File Offset: 0x00058910
	private void IconImageSetting(Transform item, string strSelect = "")
	{
		string name = item.name;
		if (name != "NONE")
		{
			item.GetComponent<HouseMixCenterPosCheck>().m_spIcon.spriteName = name + strSelect;
		}
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x0000B988 File Offset: 0x00009B88
	private void Update()
	{
		this.ViewEffecter();
	}

	// Token: 0x04000CB8 RID: 3256
	public EffManager.EffKind m_isKind;

	// Token: 0x04000CB9 RID: 3257
	public UISprite m_spIcon;

	// Token: 0x04000CBA RID: 3258
	public TweenPosition m_TweenPanelAniObj;

	// Token: 0x04000CBB RID: 3259
	public Vector2 m_v2CellSize;

	// Token: 0x04000CBC RID: 3260
	[HideInInspector]
	public bool m_bisShow;

	// Token: 0x04000CBD RID: 3261
	[HideInInspector]
	public int m_iCenterIndex;

	// Token: 0x04000CBE RID: 3262
	private Transform m_EffGrid;

	// Token: 0x04000CBF RID: 3263
	private UIScroll m_UIScrollManager;

	// Token: 0x04000CC0 RID: 3264
	private TweenPosition m_ScrollTweenPos;

	// Token: 0x04000CC1 RID: 3265
	private int m_ieffCount;

	// Token: 0x04000CC2 RID: 3266
	private int m_iUpEffCount;

	// Token: 0x04000CC3 RID: 3267
	private int m_iDownEffCount;

	// Token: 0x04000CC4 RID: 3268
	private float m_fPressFrame;

	// Token: 0x04000CC5 RID: 3269
	private float m_fPressTime = 0.01f;

	// Token: 0x04000CC6 RID: 3270
	private bool m_bisPress;

	// Token: 0x04000CC7 RID: 3271
	private bool m_bPanelAniStart;

	// Token: 0x04000CC8 RID: 3272
	private bool m_bEndCenter;

	// Token: 0x04000CC9 RID: 3273
	private GameObject EffItemPrefab;

	// Token: 0x04000CCA RID: 3274
	[HideInInspector]
	public bool m_bClickSelectUse;

	// Token: 0x020001B4 RID: 436
	public enum EffKind
	{
		// Token: 0x04000CCC RID: 3276
		SPEED,
		// Token: 0x04000CCD RID: 3277
		FADER,
		// Token: 0x04000CCE RID: 3278
		RAND,
		// Token: 0x04000CCF RID: 3279
		NORMALITEM,
		// Token: 0x04000CD0 RID: 3280
		REFILLITEM,
		// Token: 0x04000CD1 RID: 3281
		SHILDITEM,
		// Token: 0x04000CD2 RID: 3282
		MAX
	}

	// Token: 0x020001B5 RID: 437
	public enum ItemKind
	{

	}
}
