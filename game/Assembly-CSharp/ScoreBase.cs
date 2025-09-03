using System;
using System.Collections;
using System.Xml;

// Token: 0x020000D7 RID: 215
public class ScoreBase
{
	// Token: 0x06000710 RID: 1808 RVA: 0x00008881 File Offset: 0x00006A81
	public void Init()
	{
		this.m_tps = 10f;
		this.m_totalTick = 1;
		this.m_ms = 1;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0000889C File Offset: 0x00006A9C
	public ArrayList GetEvtVec(int trackIDX)
	{
		return this.m_arrAllTrackNotes[trackIDX];
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000712 RID: 1810 RVA: 0x000088A6 File Offset: 0x00006AA6
	public float TPS
	{
		get
		{
			return this.m_tps;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x000088AE File Offset: 0x00006AAE
	public int TPM
	{
		get
		{
			return this.m_tpm;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000714 RID: 1812 RVA: 0x000088B6 File Offset: 0x00006AB6
	public int TotalTick
	{
		get
		{
			return this.m_totalTick;
		}
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x000088BE File Offset: 0x00006ABE
	public ArrayList GetChangeTPSInfoVec()
	{
		return this.m_changeTPSInfoVec;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00036FF0 File Offset: 0x000351F0
	public bool LoadXMLData(string strSong)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(strSong);
		XmlElement documentElement = xmlDocument.DocumentElement;
		this.OnParsingHeaderUnderTag(documentElement);
		this.OnParsingInstrument(documentElement);
		this.OnParsingNoteAttr(documentElement);
		this.OnParsingTempo(documentElement);
		return true;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00003648 File Offset: 0x00001848
	private void OnParsingInstrument(XmlElement root)
	{
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0003702C File Offset: 0x0003522C
	protected void OnParsingHeaderUnderTag(XmlElement root)
	{
		XmlNode xmlNode = root.SelectSingleNode("header").SelectSingleNode("songinfo");
		this.m_tpm = int.Parse(xmlNode.Attributes["tpm"].Value);
		GameData.CONVERT_VALUE = this.m_tpm / 192;
		this.m_totalTick = int.Parse(xmlNode.Attributes["tick"].Value);
		this.m_ms = int.Parse(xmlNode.Attributes["ms"].Value);
		this.m_tps = float.Parse(xmlNode.Attributes["tps"].Value);
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x000370E0 File Offset: 0x000352E0
	protected void OnParsingNoteAttr(XmlElement root)
	{
		foreach (object obj in root.SelectSingleNode("note_list").SelectNodes("track"))
		{
			XmlNode xmlNode = (XmlNode)obj;
			ScoreTrackBase scoreTrackBase = new ScoreTrackBase();
			int num = int.Parse(xmlNode.Attributes["idx"].Value);
			scoreTrackBase.Init(num);
			this.m_arrTrack.Add(scoreTrackBase);
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("note");
			ArrayList arrayList = new ArrayList();
			foreach (object obj2 in xmlNodeList)
			{
				XmlNode xmlNode2 = (XmlNode)obj2;
				ScoreEventBase scoreEventBase = new ScoreEventBase();
				scoreEventBase.Init();
				int num2 = int.Parse(xmlNode2.Attributes["tick"].Value);
				scoreEventBase.Tick = num2;
				scoreEventBase.Track = num;
				if (xmlNode2.Attributes["dur"] != null)
				{
					num2 = int.Parse(xmlNode2.Attributes["dur"].Value);
					scoreEventBase.Duration = num2;
				}
				if (xmlNode2.Attributes["attr"] != null)
				{
					num2 = int.Parse(xmlNode2.Attributes["attr"].Value);
					scoreEventBase.Attr = num2;
				}
				arrayList.Add(scoreEventBase);
			}
			this.m_arrAllTrackNotes[num] = arrayList;
		}
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x000372B0 File Offset: 0x000354B0
	protected void OnParsingTempo(XmlElement root)
	{
		XmlNodeList xmlNodeList = root.SelectSingleNode("tempo").SelectNodes("tempo");
		this.m_changeTPSInfoVec.Clear();
		foreach (object obj in xmlNodeList)
		{
			XmlNode xmlNode = (XmlNode)obj;
			SChangeTPSInfo schangeTPSInfo = new SChangeTPSInfo();
			int num = int.Parse(xmlNode.Attributes["tick"].Value);
			float num2 = float.Parse(xmlNode.Attributes["tps"].Value);
			schangeTPSInfo.Set(num, num2);
			this.m_changeTPSInfoVec.Add(schangeTPSInfo);
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x000088C6 File Offset: 0x00006AC6
	public int TotalMS
	{
		get
		{
			return this.m_ms;
		}
	}

	// Token: 0x040005AF RID: 1455
	private int m_totalTick;

	// Token: 0x040005B0 RID: 1456
	private float m_tps;

	// Token: 0x040005B1 RID: 1457
	private int m_tpm;

	// Token: 0x040005B2 RID: 1458
	private ArrayList m_changeTPSInfoVec = new ArrayList();

	// Token: 0x040005B3 RID: 1459
	private ArrayList m_arrTrack = new ArrayList();

	// Token: 0x040005B4 RID: 1460
	private ArrayList[] m_arrAllTrackNotes = new ArrayList[64];

	// Token: 0x040005B5 RID: 1461
	private int m_ms;
}
