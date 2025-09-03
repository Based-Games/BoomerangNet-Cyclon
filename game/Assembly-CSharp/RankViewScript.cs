using System;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class RankViewScript : MonoBehaviour
{
	// Token: 0x060008E3 RID: 2275 RVA: 0x000438F4 File Offset: 0x00041AF4
	private void Awake()
	{
		this.m_tName = base.transform.FindChild("Name").GetComponent<UILabel>();
		this.m_tRank = base.transform.FindChild("Rank").GetComponent<UILabel>();
		this.m_tScore = base.transform.FindChild("Score").GetComponent<UILabel>();
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00009699 File Offset: 0x00007899
	public void SetName(string strName)
	{
		this.m_tName.text = strName;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000096A7 File Offset: 0x000078A7
	public void SetRank(int iRank)
	{
		this.m_tRank.text = iRank.ToString();
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000096BB File Offset: 0x000078BB
	public void SetScore(int iScore)
	{
		this.m_tScore.text = iScore.ToString();
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04000747 RID: 1863
	private UILabel m_tName;

	// Token: 0x04000748 RID: 1864
	private UILabel m_tRank;

	// Token: 0x04000749 RID: 1865
	private UILabel m_tScore;
}
