using System;
using System.Text;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000241 RID: 577
[AddComponentMenu("2D Toolkit/Text/tk2dTextMesh")]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dTextMesh : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06001060 RID: 4192 RVA: 0x0000DE76 File Offset: 0x0000C076
	public string FormattedText
	{
		get
		{
			return this._formattedText;
		}
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x00075DFC File Offset: 0x00073FFC
	private void UpgradeData()
	{
		if (this.data.version != 1)
		{
			this.data.font = this._font;
			this.data.text = this._text;
			this.data.color = this._color;
			this.data.color2 = this._color2;
			this.data.useGradient = this._useGradient;
			this.data.textureGradient = this._textureGradient;
			this.data.anchor = this._anchor;
			this.data.scale = this._scale;
			this.data.kerning = this._kerning;
			this.data.maxChars = this._maxChars;
			this.data.inlineStyling = this._inlineStyling;
			this.data.formatting = this._formatting;
			this.data.wordWrapWidth = this._wordWrapWidth;
			this.data.spacing = this.spacing;
			this.data.lineSpacing = this.lineSpacing;
		}
		this.data.version = 1;
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x00075F28 File Offset: 0x00074128
	private static int GetInlineStyleCommandLength(int cmdSymbol)
	{
		int num = 0;
		if (cmdSymbol != 67)
		{
			if (cmdSymbol != 71)
			{
				if (cmdSymbol != 99)
				{
					if (cmdSymbol == 103)
					{
						num = 9;
					}
				}
				else
				{
					num = 5;
				}
			}
			else
			{
				num = 17;
			}
		}
		else
		{
			num = 9;
		}
		return num;
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x00075F80 File Offset: 0x00074180
	public string FormatText(string unformattedString)
	{
		string empty = string.Empty;
		this.FormatText(ref empty, unformattedString);
		return empty;
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x0000DE7E File Offset: 0x0000C07E
	private void FormatText()
	{
		this.FormatText(ref this._formattedText, this.data.text);
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x00075FA0 File Offset: 0x000741A0
	private void FormatText(ref string _targetString, string _source)
	{
		if (!this.formatting || this.wordWrapWidth == 0 || this._fontInst.texelSize == Vector2.zero)
		{
			_targetString = _source;
			return;
		}
		float num = this._fontInst.texelSize.x * (float)this.wordWrapWidth;
		StringBuilder stringBuilder = new StringBuilder(_source.Length);
		float num2 = 0f;
		float num3 = 0f;
		int num4 = -1;
		int num5 = -1;
		bool flag = false;
		for (int i = 0; i < _source.Length; i++)
		{
			char c = _source[i];
			bool flag2 = c == '^';
			tk2dFontChar tk2dFontChar;
			if (this._fontInst.useDictionary)
			{
				if (!this._fontInst.charDict.ContainsKey((int)c))
				{
					c = '\0';
				}
				tk2dFontChar = this._fontInst.charDict[(int)c];
			}
			else
			{
				if ((int)c >= this._fontInst.chars.Length)
				{
					c = '\0';
				}
				tk2dFontChar = this._fontInst.chars[(int)c];
			}
			if (flag2)
			{
				c = '^';
			}
			if (flag)
			{
				flag = false;
			}
			else
			{
				if (this.data.inlineStyling && c == '^' && i + 1 < _source.Length)
				{
					if (_source[i + 1] != '^')
					{
						int inlineStyleCommandLength = tk2dTextMesh.GetInlineStyleCommandLength((int)_source[i + 1]);
						int num6 = 1 + inlineStyleCommandLength;
						for (int j = 0; j < num6; j++)
						{
							if (i + j < _source.Length)
							{
								stringBuilder.Append(_source[i + j]);
							}
						}
						i += num6 - 1;
						goto IL_2E4;
					}
					flag = true;
					stringBuilder.Append('^');
				}
				if (c == '\n')
				{
					num2 = 0f;
					num3 = 0f;
					num4 = stringBuilder.Length;
					num5 = i;
				}
				else if (c == ' ')
				{
					num2 += (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
					num3 = num2;
					num4 = stringBuilder.Length;
					num5 = i;
				}
				else if (num2 + tk2dFontChar.p1.x * this.data.scale.x > num)
				{
					if (num3 > 0f)
					{
						num3 = 0f;
						num2 = 0f;
						stringBuilder.Remove(num4 + 1, stringBuilder.Length - num4 - 1);
						stringBuilder.Append('\n');
						i = num5;
						goto IL_2E4;
					}
					stringBuilder.Append('\n');
					num2 = (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
				}
				else
				{
					num2 += (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
				}
				stringBuilder.Append(c);
			}
			IL_2E4:;
		}
		_targetString = stringBuilder.ToString();
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0000DE97 File Offset: 0x0000C097
	private void SetNeedUpdate(tk2dTextMesh.UpdateFlags uf)
	{
		if (this.updateFlags == tk2dTextMesh.UpdateFlags.UpdateNone)
		{
			this.updateFlags |= uf;
			tk2dUpdateManager.QueueCommit(this);
		}
		else
		{
			this.updateFlags |= uf;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x0000DECB File Offset: 0x0000C0CB
	// (set) Token: 0x06001068 RID: 4200 RVA: 0x0000DEDE File Offset: 0x0000C0DE
	public tk2dFontData font
	{
		get
		{
			this.UpgradeData();
			return this.data.font;
		}
		set
		{
			this.UpgradeData();
			this.data.font = value;
			this._fontInst = this.data.font.inst;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			this.UpdateMaterial();
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x0000DF15 File Offset: 0x0000C115
	// (set) Token: 0x0600106A RID: 4202 RVA: 0x0000DF28 File Offset: 0x0000C128
	public bool formatting
	{
		get
		{
			this.UpgradeData();
			return this.data.formatting;
		}
		set
		{
			this.UpgradeData();
			if (this.data.formatting != value)
			{
				this.data.formatting = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x0600106B RID: 4203 RVA: 0x0000DF54 File Offset: 0x0000C154
	// (set) Token: 0x0600106C RID: 4204 RVA: 0x0000DF67 File Offset: 0x0000C167
	public int wordWrapWidth
	{
		get
		{
			this.UpgradeData();
			return this.data.wordWrapWidth;
		}
		set
		{
			this.UpgradeData();
			if (this.data.wordWrapWidth != value)
			{
				this.data.wordWrapWidth = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x0600106D RID: 4205 RVA: 0x0000DF93 File Offset: 0x0000C193
	// (set) Token: 0x0600106E RID: 4206 RVA: 0x0000DFA6 File Offset: 0x0000C1A6
	public string text
	{
		get
		{
			this.UpgradeData();
			return this.data.text;
		}
		set
		{
			this.UpgradeData();
			this.data.text = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x0000DFC1 File Offset: 0x0000C1C1
	// (set) Token: 0x06001070 RID: 4208 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
	public Color color
	{
		get
		{
			this.UpgradeData();
			return this.data.color;
		}
		set
		{
			this.UpgradeData();
			this.data.color = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0000DFEF File Offset: 0x0000C1EF
	// (set) Token: 0x06001072 RID: 4210 RVA: 0x0000E002 File Offset: 0x0000C202
	public Color color2
	{
		get
		{
			this.UpgradeData();
			return this.data.color2;
		}
		set
		{
			this.UpgradeData();
			this.data.color2 = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x0000E01D File Offset: 0x0000C21D
	// (set) Token: 0x06001074 RID: 4212 RVA: 0x0000E030 File Offset: 0x0000C230
	public bool useGradient
	{
		get
		{
			this.UpgradeData();
			return this.data.useGradient;
		}
		set
		{
			this.UpgradeData();
			this.data.useGradient = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001075 RID: 4213 RVA: 0x0000E04B File Offset: 0x0000C24B
	// (set) Token: 0x06001076 RID: 4214 RVA: 0x0000E05E File Offset: 0x0000C25E
	public TextAnchor anchor
	{
		get
		{
			this.UpgradeData();
			return this.data.anchor;
		}
		set
		{
			this.UpgradeData();
			this.data.anchor = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001077 RID: 4215 RVA: 0x0000E079 File Offset: 0x0000C279
	// (set) Token: 0x06001078 RID: 4216 RVA: 0x0000E08C File Offset: 0x0000C28C
	public Vector3 scale
	{
		get
		{
			this.UpgradeData();
			return this.data.scale;
		}
		set
		{
			this.UpgradeData();
			this.data.scale = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06001079 RID: 4217 RVA: 0x0000E0A7 File Offset: 0x0000C2A7
	// (set) Token: 0x0600107A RID: 4218 RVA: 0x0000E0BA File Offset: 0x0000C2BA
	public bool kerning
	{
		get
		{
			this.UpgradeData();
			return this.data.kerning;
		}
		set
		{
			this.UpgradeData();
			this.data.kerning = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x0600107B RID: 4219 RVA: 0x0000E0D5 File Offset: 0x0000C2D5
	// (set) Token: 0x0600107C RID: 4220 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
	public int maxChars
	{
		get
		{
			this.UpgradeData();
			return this.data.maxChars;
		}
		set
		{
			this.UpgradeData();
			this.data.maxChars = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateBuffers);
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x0600107D RID: 4221 RVA: 0x0000E103 File Offset: 0x0000C303
	// (set) Token: 0x0600107E RID: 4222 RVA: 0x0000E116 File Offset: 0x0000C316
	public int textureGradient
	{
		get
		{
			this.UpgradeData();
			return this.data.textureGradient;
		}
		set
		{
			this.UpgradeData();
			this.data.textureGradient = value % this.font.gradientCount;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x0600107F RID: 4223 RVA: 0x0000E13D File Offset: 0x0000C33D
	// (set) Token: 0x06001080 RID: 4224 RVA: 0x0000E150 File Offset: 0x0000C350
	public bool inlineStyling
	{
		get
		{
			this.UpgradeData();
			return this.data.inlineStyling;
		}
		set
		{
			this.UpgradeData();
			this.data.inlineStyling = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06001081 RID: 4225 RVA: 0x0000E16B File Offset: 0x0000C36B
	// (set) Token: 0x06001082 RID: 4226 RVA: 0x0000E17E File Offset: 0x0000C37E
	public float Spacing
	{
		get
		{
			this.UpgradeData();
			return this.data.spacing;
		}
		set
		{
			this.UpgradeData();
			if (this.data.spacing != value)
			{
				this.data.spacing = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06001083 RID: 4227 RVA: 0x0000E1AA File Offset: 0x0000C3AA
	// (set) Token: 0x06001084 RID: 4228 RVA: 0x0000E1BD File Offset: 0x0000C3BD
	public float LineSpacing
	{
		get
		{
			this.UpgradeData();
			return this.data.lineSpacing;
		}
		set
		{
			this.UpgradeData();
			if (this.data.lineSpacing != value)
			{
				this.data.lineSpacing = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06001085 RID: 4229 RVA: 0x0000E1E9 File Offset: 0x0000C3E9
	// (set) Token: 0x06001086 RID: 4230 RVA: 0x0000E1F6 File Offset: 0x0000C3F6
	public int SortingOrder
	{
		get
		{
			return this.data.renderLayer;
		}
		set
		{
			if (this.data.renderLayer != value)
			{
				this.data.renderLayer = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x0000E21C File Offset: 0x0000C41C
	private void InitInstance()
	{
		if (this._fontInst == null && this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
		}
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x000762AC File Offset: 0x000744AC
	private void Awake()
	{
		this.UpgradeData();
		if (this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateBuffers;
		if (this.data.font != null)
		{
			this.Init();
			this.UpdateMaterial();
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x0007631C File Offset: 0x0007451C
	protected void OnDestroy()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		if (this.meshFilter != null)
		{
			this.mesh = this.meshFilter.sharedMesh;
		}
		if (this.mesh)
		{
			UnityEngine.Object.DestroyImmediate(this.mesh, true);
			this.meshFilter.mesh = null;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x0600108A RID: 4234 RVA: 0x0000E25B File Offset: 0x0000C45B
	private bool useInlineStyling
	{
		get
		{
			return this.inlineStyling && this._fontInst.textureGradients;
		}
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x00076390 File Offset: 0x00074590
	public int NumDrawnCharacters()
	{
		int num = this.NumTotalCharacters();
		if (num > this.data.maxChars)
		{
			num = this.data.maxChars;
		}
		return num;
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x000763C4 File Offset: 0x000745C4
	public int NumTotalCharacters()
	{
		this.InitInstance();
		if ((this.updateFlags & (tk2dTextMesh.UpdateFlags.UpdateText | tk2dTextMesh.UpdateFlags.UpdateBuffers)) != tk2dTextMesh.UpdateFlags.UpdateNone)
		{
			this.FormatText();
		}
		int num = 0;
		for (int i = 0; i < this._formattedText.Length; i++)
		{
			int num2 = (int)this._formattedText[i];
			bool flag = num2 == 94;
			if (this._fontInst.useDictionary)
			{
				if (!this._fontInst.charDict.ContainsKey(num2))
				{
					num2 = 0;
				}
			}
			else if (num2 >= this._fontInst.chars.Length)
			{
				num2 = 0;
			}
			if (flag)
			{
				num2 = 94;
			}
			if (num2 != 10)
			{
				if (this.data.inlineStyling && num2 == 94 && i + 1 < this._formattedText.Length)
				{
					if (this._formattedText[i + 1] != '^')
					{
						i += tk2dTextMesh.GetInlineStyleCommandLength((int)this._formattedText[i + 1]);
						goto IL_F5;
					}
					i++;
				}
				num++;
			}
			IL_F5:;
		}
		return num;
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x0000E276 File Offset: 0x0000C476
	[Obsolete("Use GetEstimatedMeshBoundsForString().size instead")]
	public Vector2 GetMeshDimensionsForString(string str)
	{
		return tk2dTextGeomGen.GetMeshDimensionsForString(str, tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText));
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x000764DC File Offset: 0x000746DC
	public Bounds GetEstimatedMeshBoundsForString(string str)
	{
		tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
		Vector2 meshDimensionsForString = tk2dTextGeomGen.GetMeshDimensionsForString(this.FormatText(str), geomData);
		float yanchorForHeight = tk2dTextGeomGen.GetYAnchorForHeight(meshDimensionsForString.y, geomData);
		float xanchorForWidth = tk2dTextGeomGen.GetXAnchorForWidth(meshDimensionsForString.x, geomData);
		float num = (this._fontInst.lineHeight + this.data.lineSpacing) * this.data.scale.y;
		return new Bounds(new Vector3(xanchorForWidth + meshDimensionsForString.x * 0.5f, yanchorForHeight + meshDimensionsForString.y * 0.5f + num, 0f), Vector3.Scale(meshDimensionsForString, new Vector3(1f, -1f, 1f)));
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x0000E295 File Offset: 0x0000C495
	public void Init(bool force)
	{
		if (force)
		{
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateBuffers);
		}
		this.Init();
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x000765A4 File Offset: 0x000747A4
	public void Init()
	{
		if (this._fontInst && ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateBuffers) != tk2dTextMesh.UpdateFlags.UpdateNone || this.mesh == null))
		{
			this._fontInst.InitDictionary();
			this.FormatText();
			tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
			int num;
			int num2;
			tk2dTextGeomGen.GetTextMeshGeomDesc(out num, out num2, geomData);
			this.vertices = new Vector3[num];
			this.uvs = new Vector2[num];
			this.colors = new Color32[num];
			this.untintedColors = new Color32[num];
			if (this._fontInst.textureGradients)
			{
				this.uv2 = new Vector2[num];
			}
			int[] array = new int[num2];
			int num3 = tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData);
			if (!this._fontInst.isPacked)
			{
				Color32 color = this.data.color;
				Color32 color2 = ((!this.data.useGradient) ? this.data.color : this.data.color2);
				for (int i = 0; i < num; i++)
				{
					Color32 color3 = ((i % 4 >= 2) ? color2 : color);
					byte b = this.untintedColors[i].r * color3.r / byte.MaxValue;
					byte b2 = this.untintedColors[i].g * color3.g / byte.MaxValue;
					byte b3 = this.untintedColors[i].b * color3.b / byte.MaxValue;
					byte b4 = this.untintedColors[i].a * color3.a / byte.MaxValue;
					if (this._fontInst.premultipliedAlpha)
					{
						b = b * b4 / byte.MaxValue;
						b2 = b2 * b4 / byte.MaxValue;
						b3 = b3 * b4 / byte.MaxValue;
					}
					this.colors[i] = new Color32(b, b2, b3, b4);
				}
			}
			else
			{
				this.colors = this.untintedColors;
			}
			tk2dTextGeomGen.SetTextMeshIndices(array, 0, 0, geomData, num3);
			if (this.mesh == null)
			{
				if (this.meshFilter == null)
				{
					this.meshFilter = base.GetComponent<MeshFilter>();
				}
				this.mesh = new Mesh();
				this.mesh.hideFlags = HideFlags.DontSave;
				this.meshFilter.mesh = this.mesh;
			}
			else
			{
				this.mesh.Clear();
			}
			this.mesh.vertices = this.vertices;
			this.mesh.uv = this.uvs;
			if (this.font.textureGradients)
			{
				this.mesh.uv2 = this.uv2;
			}
			this.mesh.triangles = array;
			this.mesh.colors32 = this.colors;
			this.mesh.RecalculateBounds();
			this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.data.renderLayer);
			this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
		}
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x0000E2AA File Offset: 0x0000C4AA
	public void Commit()
	{
		tk2dUpdateManager.FlushQueues();
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x00076904 File Offset: 0x00074B04
	public void DoNotUse__CommitInternal()
	{
		this.InitInstance();
		if (this._fontInst == null)
		{
			return;
		}
		this._fontInst.InitDictionary();
		if ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateBuffers) != tk2dTextMesh.UpdateFlags.UpdateNone || this.mesh == null)
		{
			this.Init();
		}
		else
		{
			if ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateText) != tk2dTextMesh.UpdateFlags.UpdateNone)
			{
				this.FormatText();
				tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
				int num = tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData);
				for (int i = num; i < this.data.maxChars; i++)
				{
					this.vertices[i * 4] = (this.vertices[i * 4 + 1] = (this.vertices[i * 4 + 2] = (this.vertices[i * 4 + 3] = Vector3.zero)));
				}
				this.mesh.vertices = this.vertices;
				this.mesh.uv = this.uvs;
				if (this._fontInst.textureGradients)
				{
					this.mesh.uv2 = this.uv2;
				}
				if (this._fontInst.isPacked)
				{
					this.colors = this.untintedColors;
					this.mesh.colors32 = this.colors;
				}
				if (this.data.inlineStyling)
				{
					this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
				}
				this.mesh.RecalculateBounds();
				this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.data.renderLayer);
			}
			if (!this.font.isPacked && (this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateColors) != tk2dTextMesh.UpdateFlags.UpdateNone)
			{
				Color32 color = this.data.color;
				Color32 color2 = ((!this.data.useGradient) ? this.data.color : this.data.color2);
				for (int j = 0; j < this.colors.Length; j++)
				{
					Color32 color3 = ((j % 4 >= 2) ? color2 : color);
					byte b = this.untintedColors[j].r * color3.r / byte.MaxValue;
					byte b2 = this.untintedColors[j].g * color3.g / byte.MaxValue;
					byte b3 = this.untintedColors[j].b * color3.b / byte.MaxValue;
					byte b4 = this.untintedColors[j].a * color3.a / byte.MaxValue;
					if (this._fontInst.premultipliedAlpha)
					{
						b = b * b4 / byte.MaxValue;
						b2 = b2 * b4 / byte.MaxValue;
						b3 = b3 * b4 / byte.MaxValue;
					}
					this.colors[j] = new Color32(b, b2, b3, b4);
				}
				this.mesh.colors32 = this.colors;
			}
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x00076C70 File Offset: 0x00074E70
	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera != null)
		{
			if (this._fontInst.version < 1)
			{
				Debug.LogError("Need to rebuild font.");
			}
			float num2 = base.transform.position.z - tk2dCamera.transform.position.z;
			float num3 = this._fontInst.invOrthoSize * this._fontInst.halfTargetHeight;
			num = tk2dCamera.GetSizeAtDistance(num2) * num3;
		}
		else if (Camera.main)
		{
			if (Camera.main.isOrthoGraphic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float num4 = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, num4);
			}
			num *= this._fontInst.invOrthoSize;
		}
		this.scale = new Vector3(Mathf.Sign(this.scale.x) * num, Mathf.Sign(this.scale.y) * num, Mathf.Sign(this.scale.z) * num);
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x00076DD8 File Offset: 0x00074FD8
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return !(this.data.font != null) || !(this.data.font.spriteCollection != null) || this.data.font.spriteCollection == spriteCollection;
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0000E2B1 File Offset: 0x0000C4B1
	private void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this._fontInst.materialInst)
		{
			base.renderer.material = this._fontInst.materialInst;
		}
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0000E2E9 File Offset: 0x0000C4E9
	public void ForceBuild()
	{
		if (this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
			this.UpdateMaterial();
		}
		this.Init(true);
	}

	// Token: 0x04001245 RID: 4677
	private tk2dFontData _fontInst;

	// Token: 0x04001246 RID: 4678
	private string _formattedText = string.Empty;

	// Token: 0x04001247 RID: 4679
	[SerializeField]
	private tk2dFontData _font;

	// Token: 0x04001248 RID: 4680
	[SerializeField]
	private string _text = string.Empty;

	// Token: 0x04001249 RID: 4681
	[SerializeField]
	private Color _color = Color.white;

	// Token: 0x0400124A RID: 4682
	[SerializeField]
	private Color _color2 = Color.white;

	// Token: 0x0400124B RID: 4683
	[SerializeField]
	private bool _useGradient;

	// Token: 0x0400124C RID: 4684
	[SerializeField]
	private int _textureGradient;

	// Token: 0x0400124D RID: 4685
	[SerializeField]
	private TextAnchor _anchor = TextAnchor.LowerLeft;

	// Token: 0x0400124E RID: 4686
	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x0400124F RID: 4687
	[SerializeField]
	private bool _kerning;

	// Token: 0x04001250 RID: 4688
	[SerializeField]
	private int _maxChars = 16;

	// Token: 0x04001251 RID: 4689
	[SerializeField]
	private bool _inlineStyling;

	// Token: 0x04001252 RID: 4690
	[SerializeField]
	private bool _formatting;

	// Token: 0x04001253 RID: 4691
	[SerializeField]
	private int _wordWrapWidth;

	// Token: 0x04001254 RID: 4692
	[SerializeField]
	private float spacing;

	// Token: 0x04001255 RID: 4693
	[SerializeField]
	private float lineSpacing;

	// Token: 0x04001256 RID: 4694
	[SerializeField]
	private tk2dTextMeshData data = new tk2dTextMeshData();

	// Token: 0x04001257 RID: 4695
	private Vector3[] vertices;

	// Token: 0x04001258 RID: 4696
	private Vector2[] uvs;

	// Token: 0x04001259 RID: 4697
	private Vector2[] uv2;

	// Token: 0x0400125A RID: 4698
	private Color32[] colors;

	// Token: 0x0400125B RID: 4699
	private Color32[] untintedColors;

	// Token: 0x0400125C RID: 4700
	private tk2dTextMesh.UpdateFlags updateFlags = tk2dTextMesh.UpdateFlags.UpdateBuffers;

	// Token: 0x0400125D RID: 4701
	private Mesh mesh;

	// Token: 0x0400125E RID: 4702
	private MeshFilter meshFilter;

	// Token: 0x02000242 RID: 578
	[Flags]
	private enum UpdateFlags
	{
		// Token: 0x04001260 RID: 4704
		UpdateNone = 0,
		// Token: 0x04001261 RID: 4705
		UpdateText = 1,
		// Token: 0x04001262 RID: 4706
		UpdateColors = 2,
		// Token: 0x04001263 RID: 4707
		UpdateBuffers = 4
	}
}
