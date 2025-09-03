using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class HouseMixGraph_Level : MonoBehaviour
{
	// Token: 0x06000D30 RID: 3376 RVA: 0x00003648 File Offset: 0x00001848
	private void Awake()
	{
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x0005C2A8 File Offset: 0x0005A4A8
	private void setObj()
	{
		Transform transform = base.transform.FindChild("text");
		Transform transform2 = base.transform.FindChild("View");
		this.m_lisScore = transform.FindChild("Score").GetComponent<UILabel>();
		this.m_tRankObj = transform.FindChild("Ranks");
		this.m_lisRank = this.m_tRankObj.FindChild("Rank").GetComponent<UILabel>();
		this.m_lisRank_plus_1 = this.m_tRankObj.FindChild("Rank_Plus_1").GetComponent<UILabel>();
		this.m_lisRank_plus_2 = this.m_tRankObj.FindChild("Rank_Plus_2").GetComponent<UILabel>();
		this.m_gNoneScore = transform.FindChild("NoneScore").gameObject;
		this.m_LineAni = transform2.FindChild("line").GetComponent<TweenRotation>();
		this.m_LightAni = transform2.FindChild("Light").GetComponent<TweenAlpha>();
		this.m_MarkAni = transform2.FindChild("mark_ani").GetComponent<TweenAlpha>();
		this.m_fFirstPos = this.m_tRankObj.transform.localPosition.x;
		this.m_gNoneScore.SetActive(false);
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x0005C3D4 File Offset: 0x0005A5D4
	private void InitAni()
	{
		if (this.m_LightAni == null)
		{
			this.setObj();
		}
		this.m_LightAni.ResetToBeginning();
		this.m_LineAni.ResetToBeginning();
		this.m_MarkAni.ResetToBeginning();
		this.m_HouseMixGraphPanelAni[0].PanelAni.ResetToBeginning();
		this.m_HouseMixGraphPanelAni[1].PanelAni.ResetToBeginning();
		this.m_LightAni.enabled = false;
		this.m_MarkAni.enabled = false;
		this.m_LineAni.enabled = false;
		this.m_LightAni.GetComponent<UISprite>().alpha = this.m_LightAni.from;
		this.m_MarkAni.GetComponent<UISprite>().alpha = this.m_MarkAni.from;
		this.m_LineAni.transform.localEulerAngles = new Vector3(0f, 0f, this.m_LineAni.from.z);
		for (int i = 0; i < this.m_HouseMixGraphPanelAni.Length; i++)
		{
			this.m_HouseMixGraphPanelAni[i].initAni();
		}
		this.m_fFrameTime = this.m_LineAni.duration * this.m_f_Value;
		this.m_isStep = HouseMixGraph_Level.GraphAniStep_e.Line;
		this.m_bisStart = false;
		this.m_fFrame = 0f;
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x0005C520 File Offset: 0x0005A720
	public void StartAni()
	{
		this.InitAni();
		this.m_fFrameTime = this.m_LineAni.duration * this.m_f_Value;
		this.m_isStep = HouseMixGraph_Level.GraphAniStep_e.Line;
		this.m_bisStart = true;
		this.m_fFrame = 0f;
		this.m_LineAni.Play(true);
		this.m_gNoneScore.SetActive(false);
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0005C57C File Offset: 0x0005A77C
	public void TextSetting(bool select)
	{
		if (!select)
		{
			this.InitAni();
		}
		this.m_lisRank.gameObject.SetActive(true);
		this.m_lisRank_plus_1.gameObject.SetActive(true);
		this.m_lisRank_plus_2.gameObject.SetActive(true);
		this.m_lisScore.transform.localScale = new Vector3(1f, 1.1f, 1f);
		if (this.m_iScoreValue == -1)
		{
			this.m_lisScore.text = string.Empty;
			this.m_gNoneScore.SetActive(true);
		}
		else
		{
			this.m_lisScore.text = this.m_iScoreValue.ToString();
		}
		this.m_tRankObj.transform.localPosition = new Vector3((float)(this.m_lisScore.text.Length * -9) + this.m_fFirstPos, this.m_tRankObj.transform.localPosition.y, this.m_tRankObj.transform.localPosition.z);
		Vector4 vector = new Vector4(30f, 255f, 0f, 255f);
		Vector4 vector2 = new Vector4(255f, 255f, 255f, 255f);
		Vector4 vector3 = new Vector4(83f, 187f, 254f, 255f);
		Vector4 vector4 = new Vector4(15f, 255f, 134f, 255f);
		Vector4 vector5 = new Vector4(251f, 112f, 50f, 255f);
		Vector4 vector6 = new Vector4(236f, 2f, 2f, 255f);
		Vector4 vector7 = new Vector4(234f, 39f, 241f, 255f);
		Vector4 vector8 = new Vector4(129f, 129f, 129f, 255f);
		Vector4 vector9 = new Vector4(175f, 175f, 175f, 255f);
		int iRankIndex = this.m_iRankIndex;
		switch (iRankIndex + 1)
		{
		case 0:
			this.m_lisRank.gameObject.SetActive(false);
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 1:
			this.m_lisRank.text = "F";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector2);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 2:
			this.m_lisRank.text = "D";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector3);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 3:
			this.m_lisRank.text = "C";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector4);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 4:
			this.m_lisRank.text = "B";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector5);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 5:
			this.m_lisRank.text = "A";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector6);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 6:
			this.m_lisRank.text = "A";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector6);
				this.m_lisRank_plus_1.color = this.TransColor(vector6);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
				this.m_lisRank_plus_1.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 7:
			this.m_lisRank.text = "A";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector6);
				this.m_lisRank_plus_1.color = this.TransColor(vector6);
				this.m_lisRank_plus_2.color = this.TransColor(vector6);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
				this.m_lisRank_plus_1.color = this.TransColor(vector9);
				this.m_lisRank_plus_2.color = this.TransColor(vector9);
			}
			break;
		case 8:
			this.m_lisRank.text = "S";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector7);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_1.gameObject.SetActive(false);
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 9:
			this.m_lisRank.text = "S";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector7);
				this.m_lisRank_plus_1.color = this.TransColor(vector7);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
				this.m_lisRank_plus_1.color = this.TransColor(vector9);
			}
			this.m_lisRank_plus_2.gameObject.SetActive(false);
			break;
		case 10:
			this.m_lisRank.text = "S";
			if (select)
			{
				this.m_lisScore.color = this.TransColor(vector);
				this.m_lisRank.color = this.TransColor(vector7);
				this.m_lisRank_plus_1.color = this.TransColor(vector7);
				this.m_lisRank_plus_2.color = this.TransColor(vector7);
			}
			else
			{
				this.m_lisScore.color = this.TransColor(vector8);
				this.m_lisRank.color = this.TransColor(vector9);
				this.m_lisRank_plus_1.color = this.TransColor(vector9);
				this.m_lisRank_plus_2.color = this.TransColor(vector9);
			}
			break;
		}
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0005CDFC File Offset: 0x0005AFFC
	private Color TransColor(Vector4 color)
	{
		Color color2 = new Color(color.x / 255f, color.y / 255f, color.z / 255f, color.w / 255f);
		return color2;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0005CE48 File Offset: 0x0005B048
	private void Update()
	{
		if (!this.m_bisStart)
		{
			return;
		}
		this.m_fFrame += Time.deltaTime;
		if (this.m_fFrame >= this.m_fFrameTime)
		{
			this.m_fFrame = 0f;
			this.m_isStep++;
			switch (this.m_isStep)
			{
			case HouseMixGraph_Level.GraphAniStep_e.Line:
				this.m_LineAni.Play(true);
				this.m_fFrameTime = this.m_LineAni.duration * this.m_f_Value;
				break;
			case HouseMixGraph_Level.GraphAniStep_e.Light:
				this.m_LightAni.Play(true);
				this.m_fFrameTime = this.m_LightAni.duration * this.m_f_Value;
				break;
			case HouseMixGraph_Level.GraphAniStep_e.Mark:
				this.m_MarkAni.Play(true);
				this.m_fFrameTime = this.m_MarkAni.duration * this.m_f_Value;
				break;
			case HouseMixGraph_Level.GraphAniStep_e.PanelAni_1:
				this.m_HouseMixGraphPanelAni[0].AniStart();
				this.m_fFrameTime = this.m_HouseMixGraphPanelAni[0].PanelAni.duration * this.m_f_Value;
				break;
			case HouseMixGraph_Level.GraphAniStep_e.panelAni_2:
				this.m_HouseMixGraphPanelAni[1].AniStart();
				this.m_fFrameTime = this.m_HouseMixGraphPanelAni[1].PanelAni.duration * this.m_f_Value;
				break;
			case HouseMixGraph_Level.GraphAniStep_e.LabelColorAni:
				this.TextSetting(true);
				break;
			default:
				this.m_bisStart = false;
				break;
			}
		}
	}

	// Token: 0x04000D42 RID: 3394
	public HouseMixGraphPanelAni[] m_HouseMixGraphPanelAni;

	// Token: 0x04000D43 RID: 3395
	private TweenAlpha m_MarkAni;

	// Token: 0x04000D44 RID: 3396
	private TweenAlpha m_LightAni;

	// Token: 0x04000D45 RID: 3397
	private TweenRotation m_LineAni;

	// Token: 0x04000D46 RID: 3398
	private UILabel m_lisScore;

	// Token: 0x04000D47 RID: 3399
	[HideInInspector]
	public int m_iRankIndex;

	// Token: 0x04000D48 RID: 3400
	[HideInInspector]
	public int m_iScoreValue;

	// Token: 0x04000D49 RID: 3401
	private Transform m_tRankObj;

	// Token: 0x04000D4A RID: 3402
	private UILabel m_lisRank;

	// Token: 0x04000D4B RID: 3403
	private UILabel m_lisRank_plus_1;

	// Token: 0x04000D4C RID: 3404
	private UILabel m_lisRank_plus_2;

	// Token: 0x04000D4D RID: 3405
	private GameObject m_gNoneScore;

	// Token: 0x04000D4E RID: 3406
	private float m_fFrame;

	// Token: 0x04000D4F RID: 3407
	private float m_fFrameTime;

	// Token: 0x04000D50 RID: 3408
	private HouseMixGraph_Level.GraphAniStep_e m_isStep;

	// Token: 0x04000D51 RID: 3409
	private bool m_bisStart;

	// Token: 0x04000D52 RID: 3410
	private float m_f_Value = 0.5f;

	// Token: 0x04000D53 RID: 3411
	private float m_fFirstPos;

	// Token: 0x020001C1 RID: 449
	public enum GraphAniStep_e
	{
		// Token: 0x04000D55 RID: 3413
		Line,
		// Token: 0x04000D56 RID: 3414
		Light,
		// Token: 0x04000D57 RID: 3415
		Mark,
		// Token: 0x04000D58 RID: 3416
		PanelAni_1,
		// Token: 0x04000D59 RID: 3417
		panelAni_2,
		// Token: 0x04000D5A RID: 3418
		LabelColorAni
	}
}
