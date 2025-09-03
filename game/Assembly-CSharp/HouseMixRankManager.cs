using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class HouseMixRankManager : MonoBehaviour
{
	// Token: 0x06000D6E RID: 3438 RVA: 0x0005F798 File Offset: 0x0005D998
	private void Awake()
	{
		this.m_gRankCellPrefab = Resources.Load("Prefab/HouseMix/HouseMixRankCell") as GameObject;
		this.m_UIPanel = base.transform.FindChild("Rank").GetComponent<UIPanel>();
		this.m_ProgressBar = base.transform.FindChild("ProgressBar").GetComponent<UISlider>();
		this.m_gWorldBtn = base.transform.FindChild("btn").FindChild("btn_world").gameObject;
		this.m_gLocalBtn = base.transform.FindChild("btn").FindChild("btn_local").gameObject;
		this.m_spInfoTabBtn = base.transform.FindChild("btn").FindChild("btn_Info").GetComponent<UISprite>();
		this.m_WorldAni = this.m_UIPanel.transform.FindChild("world").GetComponent<TweenPosition>();
		this.m_LocalAni = this.m_UIPanel.transform.FindChild("local").GetComponent<TweenPosition>();
		this.m_tLocalDefaultGrid = this.m_UIPanel.transform.FindChild("localDefault").FindChild("DefaultGrid");
		this.m_WorldUIScroll = this.m_WorldAni.transform.FindChild("UIScroll").GetComponent<UIScroll>();
		this.m_LocalUIScroll = this.m_LocalAni.transform.FindChild("UIScroll").GetComponent<UIScroll>();
		this.m_tWorldGrid = this.m_WorldUIScroll.transform.FindChild("WorldGrid");
		this.m_tLocalGrid = this.m_LocalUIScroll.transform.FindChild("LocalGrid");
		this.m_gRankLoading = base.transform.FindChild("UI").FindChild("Loading").gameObject;
		this.m_lLoadingLabel = this.m_gRankLoading.transform.FindChild("Label_num").GetComponent<UILabel>();
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x0000BEE5 File Offset: 0x0000A0E5
	private IEnumerator RankCreate()
	{
		this.m_LocalUIScroll.transform.localPosition = Vector3.zero;
		int i = 0;
		i = 0;
		while (i < this.m_arrLocalRank.Count)
		{
			if (i == 20)
			{
				this.m_tLocalGrid.transform.localPosition = this.m_v3OriginGridPos;
			}
			RankInfo rankInfo = (RankInfo)this.m_arrLocalRank[i];
			GameObject gameObject;
			if (this.m_arrLocalRankObj.Count > i)
			{
				gameObject = (GameObject)this.m_arrLocalRankObj[i];
				goto IL_FC;
			}
			if (this.m_arrLocalDefault.Count > i)
			{
				gameObject = (GameObject)this.m_arrLocalDefault[i];
				this.m_arrLocalRankObj.Add(gameObject);
				goto IL_FC;
			}
			IL_E5:
			int num = i;
			i = num + 1;
			continue;
			IL_FC:
			gameObject.transform.parent = this.m_tLocalGrid;
			gameObject.transform.localScale = Vector3.one;
			float num2 = this.m_UIPanel.GetViewSize().x * 0.5f + this.m_v3CellSize.y * 0.5f;
			gameObject.transform.localPosition = new Vector3(0f, num2 + (float)i * -this.m_v3CellSize.y, 0f);
			gameObject.name = "LocalRankCell" + i.ToString();
			gameObject.gameObject.collider.enabled = this.m_bColEnable;
			HouseMixRankCell component = gameObject.GetComponent<HouseMixRankCell>();
			component.m_RankInfo = rankInfo;
			component.m_UIScroll = this.m_LocalUIScroll;
			if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
			{
				component.m_spRankingBG.spriteName = "ClubTourResult_rankCellBG_info_off";
			}
			else
			{
				component.m_spRankingBG.spriteName = "rankCellBG_info_off";
			}
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			component.m_spUserIcon.spriteName = rankInfo.Icon.ToString();
			component.m_spUserIcon.MakePixelPerfect();
			if (rankInfo.Name == userData.Name)
			{
				if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
				{
					component.m_spRankingBG.spriteName = "ClubTourResult_rankMySelect_info_off";
				}
				else
				{
					component.m_spRankingBG.spriteName = "rankMySelect_info_off";
				}
				component.SetRankNum((i + 1).ToString(), ImageFontLabel.FontKind_e.RankingFont_Select);
				component.m_ilUserName.text = rankInfo.Name;
			}
			else
			{
				component.SetRankNum((i + 1).ToString(), ImageFontLabel.FontKind_e.RankingFont_None);
				component.m_ilUserName.text = rankInfo.Name;
			}
			component.m_HouseMixRankManager = base.GetComponent<HouseMixRankManager>();
			component.m_bisWorldRankCell = false;
			this.m_lLoadingLabel.text = ((int)((float)i / (float)this.m_arrLocalRank.Count * 100f)).ToString() + "%";
			if (i == this.m_arrLocalRank.Count - 1)
			{
				if (this.m_arrLocalRankObj.Count > this.m_arrLocalRank.Count)
				{
					int num3 = i;
					while (i < this.m_arrLocalRankObj.Count)
					{
						GameObject gameObject2 = (GameObject)this.m_arrLocalRankObj[num3];
						gameObject2.transform.parent = this.m_tLocalDefaultGrid;
						gameObject2.transform.localPosition = Vector3.zero;
						num3++;
					}
				}
				this.m_gRankLoading.SetActive(false);
				this.m_LocalUIScroll.CellsSetting(0);
				this.m_tLocalGrid.transform.localPosition = this.m_v3OriginGridPos;
				base.StopCoroutine("RankCreate");
				yield break;
			}
			yield return null;
			goto IL_E5;
		}
		yield break;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
	private void Start()
	{
		this.CreateDefaultRankCell();
		this.LocalRankBtnClick();
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x0005F980 File Offset: 0x0005DB80
	private void ClickScoreInfo()
	{
		if (this.m_isScoreInfoViewState)
		{
			this.m_isScoreInfoViewState = false;
			if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_DECREASE, false);
				if (this.RankingCellSetting("off", "ClubTour_Ranking_InfoBtn_on", this.m_v3OriginSize, this.m_v3OriginGridPos) > 90)
				{
					this.m_LocalUIScroll.SetSmoothPosMove();
					return;
				}
			}
			else
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_DECREASE, false);
				if (this.RankingCellSetting("off", "Ranking_InfoBtn_on", this.m_v3OriginSize, this.m_v3OriginGridPos) > 90)
				{
					this.m_LocalUIScroll.SetSmoothPosMove();
				}
			}
			return;
		}
		this.m_isScoreInfoViewState = true;
		if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_INCREASE, false);
			this.RankingCellSetting("on", "ClubTour_Ranking_InfoBtn_off", this.m_v3InfoSize, this.m_v3InfoGridPos);
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_SORT_INCREASE, false);
		this.RankingCellSetting("on", "Ranking_InfoBtn_off", this.m_v3InfoSize, this.m_v3InfoGridPos);
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0005FA80 File Offset: 0x0005DC80
	private int RankingCellSetting(string strCellBG, string strBtn, Vector3 TargetSize, Vector3 TargetGridPos)
	{
		this.m_LocalUIScroll.GetComponent<TweenPosition>().enabled = false;
		this.m_spInfoTabBtn.spriteName = strBtn;
		Vector3 localPosition = this.m_LocalUIScroll.m_ProgressBar.foregroundWidget.transform.localPosition;
		this.m_LocalUIScroll.m_ProgressBar.foregroundWidget.transform.localPosition = this.m_v3ProgressBarOriginPos;
		TweenAlpha component = this.m_LocalUIScroll.m_ProgressBar.foregroundWidget.GetComponent<TweenAlpha>();
		component.ResetToBeginning();
		component.Play(true);
		int iCenterIndex = this.m_LocalUIScroll.m_iCenterIndex;
		float num = (float)iCenterIndex * TargetSize.y;
		this.m_LocalUIScroll.transform.localPosition = new Vector3(this.m_LocalUIScroll.transform.localPosition.x, num, this.m_LocalUIScroll.transform.localPosition.z);
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		for (int i = 0; i < this.m_arrLocalRankObj.Count; i++)
		{
			RankInfo rankInfo = (RankInfo)this.m_arrLocalRank[i];
			HouseMixRankCell component2 = ((GameObject)this.m_arrLocalRankObj[i]).GetComponent<HouseMixRankCell>();
			if (strCellBG == "on")
			{
				component2.InfoMode(true);
			}
			else if (strCellBG == "off")
			{
				component2.InfoMode(false);
			}
			string spriteName = component2.m_spRankingBG.spriteName;
			if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
			{
				component2.m_spRankingBG.spriteName = "ClubTourResult_rankCellBG_info_" + strCellBG;
			}
			else
			{
				component2.m_spRankingBG.spriteName = "rankCellBG_info_" + strCellBG;
			}
			if (rankInfo.Name == userData.Name)
			{
				if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
				{
					component2.m_spRankingBG.spriteName = "ClubTourResult_rankMySelect_info_" + strCellBG;
				}
				else
				{
					component2.m_spRankingBG.spriteName = "rankMySelect_info_" + strCellBG;
				}
			}
			component2.m_spRankingBG.MakePixelPerfect();
			component2.m_spRankingBG.transform.localScale = Vector3.one * 2f;
			UIScroll localUIScroll = this.m_LocalUIScroll;
			this.m_v3CellSize = TargetSize;
			localUIScroll.CellSize = TargetSize;
			this.m_tLocalGrid.transform.localPosition = TargetGridPos;
			float num2 = this.m_UIPanel.GetViewSize().x * 0.5f + this.m_v3CellSize.y * 0.5f;
			component2.transform.localPosition = new Vector3(0f, num2 + (float)i * -this.m_v3CellSize.y, 0f);
			this.m_LocalUIScroll.CellsSetting(0);
		}
		this.m_LocalUIScroll.m_ProgressBar.foregroundWidget.transform.localPosition = new Vector3(this.m_v3ProgressBarOriginPos.x, this.m_v3ProgressBarOriginPos.y - localPosition.y, this.m_v3ProgressBarOriginPos.z);
		return iCenterIndex;
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x0005FD6C File Offset: 0x0005DF6C
	private void CreateDefaultRankCell()
	{
		for (int i = 0; i < 120; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.m_gRankCellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = this.m_tLocalDefaultGrid;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = "LocalRankCell" + i.ToString();
			if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
			{
				gameObject.GetComponent<HouseMixRankCell>().m_spRankingBG.spriteName = "ClubTourResult_rankCellBG_info_off";
			}
			this.m_arrLocalDefault.Add(gameObject);
		}
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x0005FE1C File Offset: 0x0005E01C
	private void WorldRankBtnClick()
	{
		if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
		{
			this.m_gLocalBtn.GetComponent<UISprite>().spriteName = "ClubTourResult_Ranking_local_off";
		}
		else
		{
			this.m_gLocalBtn.GetComponent<UISprite>().spriteName = "Ranking_local_off";
		}
		this.m_gLocalBtn.GetComponent<UISprite>().MakePixelPerfect();
		this.m_gLocalBtn.GetComponent<UISprite>().depth = 6;
		this.m_gLocalBtn.transform.localScale = Vector3.one * 2f;
		this.m_gWorldBtn.GetComponent<UISprite>().spriteName = "Ranking_world_on";
		this.m_gWorldBtn.GetComponent<UISprite>().MakePixelPerfect();
		this.m_gWorldBtn.GetComponent<UISprite>().depth = 7;
		this.m_gWorldBtn.transform.localScale = Vector3.one * 2f;
		Vector3 localPosition = this.m_WorldAni.transform.localPosition;
		Vector3 localPosition2 = this.m_LocalAni.transform.localPosition;
		this.m_WorldAni.from = new Vector3(localPosition.x, this.m_WorldAni.from.y, this.m_WorldAni.from.z);
		this.m_WorldAni.to = new Vector3(localPosition.x, this.m_WorldAni.to.y, this.m_WorldAni.to.z);
		this.m_WorldAni.Play(true);
		this.m_LocalAni.from = new Vector3(localPosition2.x, this.m_LocalAni.from.y, this.m_LocalAni.from.z);
		this.m_LocalAni.to = new Vector3(localPosition2.x, this.m_LocalAni.to.y, this.m_LocalAni.to.z);
		this.m_LocalAni.Play(false);
		this.m_WorldUIScroll.gameObject.SetActive(true);
		this.m_WorldUIScroll.m_ProgressBar = this.m_ProgressBar;
		this.m_LocalUIScroll.m_ProgressBar = null;
		this.m_bhideLocal = true;
		this.m_bHideWorld = false;
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00060044 File Offset: 0x0005E244
	private void LocalRankBtnClick()
	{
		if (this.m_RankingKind == HouseMixRankManager.RankingKind_e.ClubTourResult)
		{
			this.m_gLocalBtn.GetComponent<UISprite>().spriteName = "ClubTourResult_Ranking_local_on";
		}
		else
		{
			this.m_gLocalBtn.GetComponent<UISprite>().spriteName = "Ranking_local_on";
		}
		this.m_gLocalBtn.GetComponent<UISprite>().MakePixelPerfect();
		this.m_gLocalBtn.GetComponent<UISprite>().depth = 7;
		this.m_gLocalBtn.transform.localScale = Vector3.one * 2f;
		this.m_gWorldBtn.GetComponent<UISprite>().MakePixelPerfect();
		this.m_gWorldBtn.GetComponent<UISprite>().depth = 6;
		this.m_gWorldBtn.transform.localScale = Vector3.one * 2f;
		Vector3 localPosition = this.m_WorldAni.transform.localPosition;
		Vector3 localPosition2 = this.m_LocalAni.transform.localPosition;
		this.m_WorldAni.from = new Vector3(localPosition.x, this.m_WorldAni.from.y, this.m_WorldAni.from.z);
		this.m_WorldAni.to = new Vector3(localPosition.x, this.m_WorldAni.to.y, this.m_WorldAni.to.z);
		this.m_WorldAni.Play(false);
		this.m_LocalAni.from = new Vector3(localPosition2.x, this.m_LocalAni.from.y, this.m_LocalAni.from.z);
		this.m_LocalAni.to = new Vector3(localPosition2.x, this.m_LocalAni.to.y, this.m_LocalAni.to.z);
		this.m_LocalAni.Play(true);
		this.m_LocalUIScroll.gameObject.SetActive(true);
		this.m_WorldUIScroll.m_ProgressBar = null;
		this.m_LocalUIScroll.m_ProgressBar = this.m_ProgressBar;
		this.m_bhideLocal = false;
		this.m_bHideWorld = true;
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x00060254 File Offset: 0x0005E454
	private void CreateWorldRankCell()
	{
		for (int i = 0; i < this.m_iWorldRankCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.m_gRankCellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = this.m_tWorldGrid;
			gameObject.transform.localScale = Vector3.one;
			float num = this.m_UIPanel.GetViewSize().x * 0.5f + this.m_v3CellSize.y * 0.5f;
			gameObject.transform.localPosition = new Vector3(0f, num + (float)i * -this.m_v3CellSize.y, 0f);
			gameObject.name = "WorldRankCell" + i.ToString();
			gameObject.GetComponent<HouseMixRankCell>().m_UIScroll = this.m_WorldUIScroll;
			gameObject.GetComponent<HouseMixRankCell>().m_spRankingBG.spriteName = "rankCellBG_info_off";
			if (i == this.m_iMyRankNum)
			{
				gameObject.GetComponent<HouseMixRankCell>().m_spRankingBG.spriteName = "rankMySelect_info_off";
				gameObject.GetComponent<HouseMixRankCell>().SetRankNum((i + 1).ToString(), ImageFontLabel.FontKind_e.RankingFont_Select);
				gameObject.GetComponent<HouseMixRankCell>().m_ilUserName.text = "WorldTestUser" + (i + 1).ToString();
			}
			else
			{
				gameObject.GetComponent<HouseMixRankCell>().SetRankNum((i + 1).ToString(), ImageFontLabel.FontKind_e.RankingFont_None);
				gameObject.GetComponent<HouseMixRankCell>().m_ilUserName.text = "WorldTestUser" + (i + 1).ToString();
			}
			gameObject.GetComponent<HouseMixRankCell>().m_HouseMixRankManager = base.GetComponent<HouseMixRankManager>();
			gameObject.GetComponent<HouseMixRankCell>().m_bisWorldRankCell = true;
		}
		this.m_WorldUIScroll.CellsSetting(0);
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x00060408 File Offset: 0x0005E608
	private void Reset()
	{
		if (this.m_arrLocalRankObj.Count > this.m_arrLocalRank.Count)
		{
			for (int i = 0; i < this.m_arrLocalRankObj.Count; i++)
			{
				GameObject gameObject = (GameObject)this.m_arrLocalRankObj[i];
				gameObject.transform.parent = this.m_tLocalDefaultGrid;
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x00060474 File Offset: 0x0005E674
	public void CreateLocalRankCell()
	{
		base.StopCoroutine("RankCreate");
		this.Reset();
		this.m_arrLocalRankObj.Clear();
		this.m_LocalUIScroll.transform.localPosition = Vector3.zero;
		this.m_LocalUIScroll.m_ProgressBar.foregroundWidget.transform.localPosition = new Vector3(-11.5f, 408.5f, 0f);
		switch (this.m_RankingKind)
		{
		case HouseMixRankManager.RankingKind_e.HausMix:
			if (Singleton<GameManager>.instance.netHouseMixData.ArrLocalRank.Count <= 0)
			{
				Singleton<GameManager>.instance.netHouseMixData.RankingInit();
			}
			this.m_arrLocalRank = Singleton<GameManager>.instance.netHouseMixData.ArrLocalRank;
			if (this.m_arrLocalRank.Count > 11)
			{
				this.m_bColEnable = true;
			}
			else
			{
				this.m_bColEnable = false;
			}
			break;
		case HouseMixRankManager.RankingKind_e.HausMixResult:
			if (Singleton<GameManager>.instance.netHouMixResultData.ArrRanking.Count <= 0)
			{
				Singleton<GameManager>.instance.netHouMixResultData.Init();
			}
			this.m_arrLocalRank = Singleton<GameManager>.instance.netHouMixResultData.ArrRanking;
			if (this.m_arrLocalRank.Count > 23)
			{
				this.m_bColEnable = true;
			}
			else
			{
				this.m_bColEnable = false;
			}
			break;
		case HouseMixRankManager.RankingKind_e.HausMixAllClearResult:
			if (Singleton<GameManager>.instance.netHouMixTotalResultData.ArrRanking.Count <= 0)
			{
				Singleton<GameManager>.instance.netHouMixTotalResultData.Init();
			}
			this.m_arrLocalRank = Singleton<GameManager>.instance.netHouMixTotalResultData.ArrRanking;
			if (this.m_arrLocalRank.Count > 23)
			{
				this.m_bColEnable = true;
			}
			else
			{
				this.m_bColEnable = false;
			}
			break;
		case HouseMixRankManager.RankingKind_e.Raveup:
			if (Singleton<GameManager>.instance.netRaveUpRankData.ArrLocalRank.Count <= 0)
			{
				Singleton<GameManager>.instance.netRaveUpRankData.Init();
			}
			this.m_arrLocalRank = Singleton<GameManager>.instance.netRaveUpRankData.ArrLocalRank;
			if (this.m_arrLocalRank.Count > 23)
			{
				this.m_bColEnable = true;
			}
			else
			{
				this.m_bColEnable = false;
			}
			break;
		case HouseMixRankManager.RankingKind_e.RaveupResult:
			if (Singleton<GameManager>.instance.netRaveUpResultData.ArrRanking.Count <= 0)
			{
				Singleton<GameManager>.instance.netRaveUpResultData.Init();
			}
			this.m_arrLocalRank = Singleton<GameManager>.instance.netRaveUpResultData.ArrRanking;
			if (this.m_arrLocalRank.Count > 23)
			{
				this.m_bColEnable = true;
			}
			else
			{
				this.m_bColEnable = false;
			}
			break;
		case HouseMixRankManager.RankingKind_e.ClubTourResult:
			if (Singleton<GameManager>.instance.netRaveUpResultData.ArrRanking.Count <= 0)
			{
				Singleton<GameManager>.instance.netRaveUpResultData.Init();
			}
			this.m_arrLocalRank = Singleton<GameManager>.instance.netRaveUpResultData.ArrRanking;
			if (this.m_arrLocalRank.Count > 23)
			{
				this.m_bColEnable = true;
			}
			else
			{
				this.m_bColEnable = false;
			}
			break;
		}
		if (this.m_arrLocalRank.Count <= 0)
		{
			this.m_gRankLoading.SetActive(false);
			return;
		}
		this.m_LocalUIScroll.CellsSetting(0);
		this.m_gRankLoading.SetActive(true);
		base.StartCoroutine("RankCreate");
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x00060788 File Offset: 0x0005E988
	private void WorldAutoScrollMove()
	{
		if (!this.m_bWorldAutoScroll)
		{
			return;
		}
		if (this.m_UIPanel.GetViewSize().y > this.m_v3CellSize.y * (float)this.m_iWorldRankCount)
		{
			return;
		}
		Vector3 localPosition = this.m_WorldUIScroll.transform.localPosition;
		this.m_WorldUIScroll.transform.localPosition = new Vector3(localPosition.x, localPosition.y + this.m_fDirValue * (this.m_fAutoScrollSpeed * Time.deltaTime), localPosition.z);
		if (localPosition.y < 0f)
		{
			this.m_fDirValue = 1f;
			return;
		}
		if (localPosition.y > this.m_v3CellSize.y * (float)this.m_iWorldRankCount - this.m_UIPanel.GetViewSize().y)
		{
			this.m_fDirValue = -1f;
		}
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x00060860 File Offset: 0x0005EA60
	private void LocalAutoScrollMove()
	{
		if (!this.m_bLocalAutoScroll)
		{
			return;
		}
		if (this.m_UIPanel.GetViewSize().y > this.m_v3CellSize.y * (float)this.m_iLocalRankCount)
		{
			return;
		}
		Vector3 localPosition = this.m_LocalUIScroll.transform.localPosition;
		this.m_LocalUIScroll.transform.localPosition = new Vector3(localPosition.x, localPosition.y + this.m_fDirValue * (this.m_fAutoScrollSpeed * Time.deltaTime), localPosition.z);
		if (localPosition.y < 0f)
		{
			this.m_fDirValue = 1f;
			return;
		}
		if (localPosition.y > this.m_v3CellSize.y * (float)this.m_iLocalRankCount - this.m_UIPanel.GetViewSize().y)
		{
			this.m_fDirValue = -1f;
		}
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x00060938 File Offset: 0x0005EB38
	private void CheckHideWorld()
	{
		if (!this.m_bHideWorld)
		{
			return;
		}
		if (this.m_WorldAni.transform.localPosition.y == this.m_WorldAni.from.y && this.m_WorldUIScroll.gameObject.activeSelf)
		{
			this.m_WorldUIScroll.gameObject.SetActive(false);
			this.m_bWorldAutoScroll = false;
			this.m_bLocalAutoScroll = true;
		}
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000609A8 File Offset: 0x0005EBA8
	private void CheckHideLocal()
	{
		if (!this.m_bhideLocal)
		{
			return;
		}
		if (this.m_LocalAni.transform.localPosition.y == this.m_LocalAni.from.y && this.m_LocalUIScroll.gameObject.activeSelf)
		{
			this.m_LocalUIScroll.gameObject.SetActive(false);
			this.m_bWorldAutoScroll = true;
			this.m_bLocalAutoScroll = false;
		}
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04000DAF RID: 3503
	public HouseMixRankManager.RankingKind_e m_RankingKind;

	// Token: 0x04000DB0 RID: 3504
	public Vector3 m_v3CellSize;

	// Token: 0x04000DB1 RID: 3505
	private Vector3 m_v3OriginSize = new Vector3(380f, 55f, 1f);

	// Token: 0x04000DB2 RID: 3506
	private Vector3 m_v3InfoSize = new Vector3(380f, 100f, 1f);

	// Token: 0x04000DB3 RID: 3507
	private Vector3 m_v3OriginGridPos = new Vector3(-58f, -45f, 0f);

	// Token: 0x04000DB4 RID: 3508
	private Vector3 m_v3InfoGridPos = new Vector3(-58f, -68f, 0f);

	// Token: 0x04000DB5 RID: 3509
	private GameObject m_gWorldBtn;

	// Token: 0x04000DB6 RID: 3510
	private GameObject m_gLocalBtn;

	// Token: 0x04000DB7 RID: 3511
	private UISprite m_spInfoTabBtn;

	// Token: 0x04000DB8 RID: 3512
	private TweenPosition m_WorldAni;

	// Token: 0x04000DB9 RID: 3513
	private TweenPosition m_LocalAni;

	// Token: 0x04000DBA RID: 3514
	private Transform m_tLocalDefaultGrid;

	// Token: 0x04000DBB RID: 3515
	private UIScroll m_WorldUIScroll;

	// Token: 0x04000DBC RID: 3516
	private UIScroll m_LocalUIScroll;

	// Token: 0x04000DBD RID: 3517
	private Transform m_tWorldGrid;

	// Token: 0x04000DBE RID: 3518
	private Transform m_tLocalGrid;

	// Token: 0x04000DBF RID: 3519
	private GameObject m_gRankLoading;

	// Token: 0x04000DC0 RID: 3520
	private UILabel m_lLoadingLabel;

	// Token: 0x04000DC1 RID: 3521
	private UIPanel m_UIPanel;

	// Token: 0x04000DC2 RID: 3522
	private UIProgressBar m_ProgressBar;

	// Token: 0x04000DC3 RID: 3523
	private GameObject m_gRankCellPrefab;

	// Token: 0x04000DC4 RID: 3524
	private ArrayList m_arrLocalDefault = new ArrayList();

	// Token: 0x04000DC5 RID: 3525
	private ArrayList m_arrLocalRankObj = new ArrayList();

	// Token: 0x04000DC6 RID: 3526
	private int m_iWorldRankCount = 50;

	// Token: 0x04000DC7 RID: 3527
	private int m_iLocalRankCount = 50;

	// Token: 0x04000DC8 RID: 3528
	private int m_iMyRankNum = 7;

	// Token: 0x04000DC9 RID: 3529
	private float m_fDirValue = 1f;

	// Token: 0x04000DCA RID: 3530
	private float m_fAutoScrollSpeed = 70f;

	// Token: 0x04000DCB RID: 3531
	private bool m_bHideWorld;

	// Token: 0x04000DCC RID: 3532
	private bool m_bhideLocal;

	// Token: 0x04000DCD RID: 3533
	private ArrayList m_arrLocalRank = new ArrayList();

	// Token: 0x04000DCE RID: 3534
	private bool m_bColEnable = true;

	// Token: 0x04000DCF RID: 3535
	private ArrayList m_arrDefaultUserName = new ArrayList();

	// Token: 0x04000DD0 RID: 3536
	private ArrayList m_arrDefaultUserRanking = new ArrayList();

	// Token: 0x04000DD1 RID: 3537
	private bool m_isScoreInfoViewState;

	// Token: 0x04000DD2 RID: 3538
	private Vector3 m_v3ProgressBarOriginPos = new Vector3(-11.5f, 408.5f, 0f);

	// Token: 0x04000DD3 RID: 3539
	[HideInInspector]
	public bool m_bWorldAutoScroll;

	// Token: 0x04000DD4 RID: 3540
	[HideInInspector]
	public bool m_bLocalAutoScroll;

	// Token: 0x020001CC RID: 460
	public enum RankingKind_e
	{
		// Token: 0x04000DD6 RID: 3542
		HausMix,
		// Token: 0x04000DD7 RID: 3543
		HausMixResult,
		// Token: 0x04000DD8 RID: 3544
		HausMixAllClearResult,
		// Token: 0x04000DD9 RID: 3545
		Raveup,
		// Token: 0x04000DDA RID: 3546
		RaveupResult,
		// Token: 0x04000DDB RID: 3547
		ClubTourResult
	}
}
