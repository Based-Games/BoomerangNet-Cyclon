using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class ClubTourSpeedEff : MonoBehaviour
{
	// Token: 0x06000BFA RID: 3066 RVA: 0x000550B4 File Offset: 0x000532B4
	private void Awake()
	{
		this.m_UIScrollManager = base.transform.FindChild("ScrollView").GetComponent<UIScroll>();
		this.m_EffGrid = this.m_UIScrollManager.transform.FindChild("Grid").transform;
		this.m_spDownEff = this.m_gDownEff.transform.FindChild("Eff").GetComponent<UISprite>();
		this.m_ScrollTweenPos = this.m_UIScrollManager.GetComponent<TweenPosition>();
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0000B0CC File Offset: 0x000092CC
	private void Start()
	{
		this.EffPositionSetting();
		this.Setting();
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00055130 File Offset: 0x00053330
	private void Setting()
	{
		if (GameData.Stage < 3)
		{
			if (GameData.Stage == 0)
			{
				this.m_iCenterIndex = (int)Singleton<GameManager>.instance.UserData.UserSpeed;
			}
			string text = string.Empty;
			this.m_UIScrollManager.transform.localPosition = new Vector3(0f, -this.m_v2CellSize.y * (float)this.m_iCenterIndex, 0f);
			text = this.m_EffGrid.GetChild(this.m_iCenterIndex).gameObject.name;
			GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)this.m_iCenterIndex;
			for (int i = 0; i < this.m_EffGrid.childCount; i++)
			{
				(this.m_EffGrid.GetChild(i).collider as BoxCollider).center = Vector3.zero;
				if (this.m_EffGrid.GetChild(i).GetComponent<ClubTourCenterPosCheck>().m_iIndex == this.m_iCenterIndex)
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
			int num = this.m_iCenterIndex - 1;
			if (num < 0)
			{
				num = this.m_EffGrid.childCount - 1;
			}
			this.m_gDownEff.SetActive(true);
			this.m_spDownEff.spriteName = this.m_EffGrid.GetChild(num).gameObject.name;
			this.m_spDownEff.MakePixelPerfect();
			this.m_spDownEff.transform.localScale = Vector3.one * 2f;
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00055314 File Offset: 0x00053514
	private void EffPositionSetting()
	{
		this.m_ieffCount = this.m_EffGrid.childCount;
		this.m_iUpEffCount = (this.m_ieffCount - 1) / 2;
		this.m_iDownEffCount = this.m_ieffCount - 1 - this.m_iUpEffCount;
		int num = this.m_iUpEffCount;
		for (int i = 0; i < this.m_EffGrid.childCount; i++)
		{
			if (i <= this.m_iDownEffCount)
			{
				this.m_EffGrid.GetChild(i).localPosition = new Vector3(0f, (float)i * this.m_v2CellSize.y, 0f);
			}
			else
			{
				this.m_EffGrid.GetChild(i).localPosition = new Vector3(0f, (float)num * -this.m_v2CellSize.y, 0f);
				num--;
			}
		}
		this.m_UIScrollManager.CellsSetting(0);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0000B0DA File Offset: 0x000092DA
	public void SetEndCenter()
	{
		this.m_bEndCenter = true;
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x000553F8 File Offset: 0x000535F8
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

	// Token: 0x06000C00 RID: 3072 RVA: 0x0000B0E3 File Offset: 0x000092E3
	private void DragProcess()
	{
		base.CancelInvoke("DragEndProcess");
		base.CancelInvoke("ColliderReset");
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x0000B0FB File Offset: 0x000092FB
	private void HideInit()
	{
		this.m_bisShow = false;
		this.m_bisPress = false;
		this.m_fPressFrame = 0f;
		this.m_bPanelAniStart = false;
		base.Invoke("ColliderReset", 1f);
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x0000B12D File Offset: 0x0000932D
	public void SelectEffect(int selectIndex)
	{
		this.m_iCenterIndex = selectIndex;
		GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)this.m_iCenterIndex;
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000554A8 File Offset: 0x000536A8
	private void ColliderReset()
	{
		this.m_spIcon.gameObject.SetActive(true);
		string text = string.Empty;
		int num = 0;
		for (int i = 0; i < this.m_EffGrid.childCount; i++)
		{
			BoxCollider boxCollider = this.m_EffGrid.GetChild(i).collider as BoxCollider;
			boxCollider.center = Vector3.zero;
			if (this.m_EffGrid.GetChild(i).GetComponent<ClubTourCenterPosCheck>().m_iIndex == this.m_iCenterIndex)
			{
				this.m_EffGrid.GetChild(i).collider.enabled = true;
				text = this.m_EffGrid.GetChild(i).name;
				num = i;
				this.IconImageSetting(this.m_EffGrid.GetChild(i), "_select");
			}
			else
			{
				this.m_EffGrid.GetChild(i).collider.enabled = false;
				this.IconImageSetting(this.m_EffGrid.GetChild(i), string.Empty);
			}
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_EFFECT_MOUNT, false);
		GameData.SPEEDEFFECTOR = (EFFECTOR_SPEED)this.m_iCenterIndex;
		this.m_spIcon.spriteName = text;
		if (text != "NONE")
		{
			this.m_spIcon.spriteName = text + "_select";
		}
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
		int num2 = num - 1;
		if (num2 < 0)
		{
			num2 = this.m_EffGrid.childCount - 1;
		}
		this.m_gDownEff.SetActive(true);
		this.m_spDownEff.spriteName = this.m_EffGrid.GetChild(num2).gameObject.name;
		this.m_spDownEff.MakePixelPerfect();
		this.m_spDownEff.transform.localScale = Vector3.one * 2f;
		base.GetComponent<TweenAlpha>().duration = 0.5f;
		base.GetComponent<TweenAlpha>().Play(false);
		this.m_bClickSelectUse = false;
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x0000B141 File Offset: 0x00009341
	private void DragEndProcess()
	{
		this.HideInit();
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x0000B149 File Offset: 0x00009349
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

	// Token: 0x06000C06 RID: 3078 RVA: 0x000556B0 File Offset: 0x000538B0
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
			this.m_gDownEff.SetActive(false);
			base.Invoke("DragEndProcess", 1f);
		}
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x0005576C File Offset: 0x0005396C
	public void EffClickSelect(Vector3 TargetPos)
	{
		if (!this.m_bClickSelectUse)
		{
			return;
		}
		base.CancelInvoke("DragEndProcess");
		this.m_ScrollTweenPos.enabled = false;
		this.m_ScrollTweenPos.transform.localPosition = new Vector3(0f, this.m_ScrollTweenPos.transform.localPosition.y - (this.m_ScrollTweenPos.transform.localPosition.y + TargetPos.y), 0f);
		this.HideInit();
		base.CancelInvoke("ColliderReset");
		this.ColliderReset();
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x0005580C File Offset: 0x00053A0C
	private void IconImageSetting(Transform item, string strSelect = "")
	{
		string name = item.name;
		if (name != "NONE")
		{
			item.GetComponent<ClubTourCenterPosCheck>().m_spIcon.spriteName = name + strSelect;
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x0000B169 File Offset: 0x00009369
	private void Update()
	{
		this.ViewEffecter();
	}

	// Token: 0x04000BC0 RID: 3008
	public UISprite m_spIcon;

	// Token: 0x04000BC1 RID: 3009
	public TweenPosition m_TweenPanelAniObj;

	// Token: 0x04000BC2 RID: 3010
	public GameObject m_gDownEff;

	// Token: 0x04000BC3 RID: 3011
	public Vector2 m_v2CellSize;

	// Token: 0x04000BC4 RID: 3012
	[HideInInspector]
	public bool m_bisShow;

	// Token: 0x04000BC5 RID: 3013
	[HideInInspector]
	public int m_iCenterIndex;

	// Token: 0x04000BC6 RID: 3014
	private Transform m_EffGrid;

	// Token: 0x04000BC7 RID: 3015
	private UIScroll m_UIScrollManager;

	// Token: 0x04000BC8 RID: 3016
	private TweenPosition m_ScrollTweenPos;

	// Token: 0x04000BC9 RID: 3017
	private UISprite m_spDownEff;

	// Token: 0x04000BCA RID: 3018
	private int m_ieffCount;

	// Token: 0x04000BCB RID: 3019
	private int m_iUpEffCount;

	// Token: 0x04000BCC RID: 3020
	private int m_iDownEffCount;

	// Token: 0x04000BCD RID: 3021
	private float m_fPressFrame;

	// Token: 0x04000BCE RID: 3022
	private float m_fPressTime = 0.01f;

	// Token: 0x04000BCF RID: 3023
	private bool m_bisPress;

	// Token: 0x04000BD0 RID: 3024
	private bool m_bPanelAniStart;

	// Token: 0x04000BD1 RID: 3025
	private bool m_bEndCenter;

	// Token: 0x04000BD2 RID: 3026
	[HideInInspector]
	public bool m_bClickSelectUse;
}
