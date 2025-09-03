using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[AddComponentMenu("NGUI/Examples/Slider Colors")]
[RequireComponent(typeof(UISlider))]
public class UISliderColors : MonoBehaviour
{
	// Token: 0x060000FD RID: 253 RVA: 0x00003FEA File Offset: 0x000021EA
	private void Start()
	{
		this.mSlider = base.GetComponent<UISlider>();
		this.Update();
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00015D50 File Offset: 0x00013F50
	private void Update()
	{
		if (this.sprite == null || this.colors.Length == 0)
		{
			return;
		}
		float num = this.mSlider.value;
		num *= (float)(this.colors.Length - 1);
		int num2 = Mathf.FloorToInt(num);
		Color color = this.colors[0];
		if (num2 >= 0)
		{
			if (num2 + 1 < this.colors.Length)
			{
				float num3 = num - (float)num2;
				color = Color.Lerp(this.colors[num2], this.colors[num2 + 1], num3);
			}
			else if (num2 < this.colors.Length)
			{
				color = this.colors[num2];
			}
			else
			{
				color = this.colors[this.colors.Length - 1];
			}
		}
		color.a = this.sprite.color.a;
		this.sprite.color = color;
	}

	// Token: 0x040000E4 RID: 228
	public UISprite sprite;

	// Token: 0x040000E5 RID: 229
	public Color[] colors = new Color[]
	{
		Color.red,
		Color.yellow,
		Color.green
	};

	// Token: 0x040000E6 RID: 230
	private UISlider mSlider;
}
