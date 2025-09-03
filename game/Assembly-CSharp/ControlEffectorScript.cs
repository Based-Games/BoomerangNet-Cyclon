using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class ControlEffectorScript : MonoBehaviour
{
	// Token: 0x06000886 RID: 2182 RVA: 0x000415AC File Offset: 0x0003F7AC
	private void Start()
	{
		this.m_sSlotSpeed = base.transform.FindChild("Slot1").GetComponent<UISprite>();
		this.m_sSlotFade = base.transform.FindChild("Slot2").GetComponent<UISprite>();
		this.m_sSlotRandom = base.transform.FindChild("Slot3").GetComponent<UISprite>();
		this.m_sSlotShield = base.transform.FindChild("Slot4").GetComponent<UISprite>();
		this.m_sSlotRefill = base.transform.FindChild("Slot5").GetComponent<UISprite>();
		this.m_sSlotSpeed.spriteName = "0_off";
		this.m_sSlotFade.spriteName = "0_off";
		this.m_sSlotRandom.spriteName = "0_off";
		this.m_sSlotShield.spriteName = "0_off";
		this.m_sSlotRefill.spriteName = "0_off";
		this.m_sSlotSpeed.MakePixelPerfect();
		this.m_sSlotFade.MakePixelPerfect();
		this.m_sSlotRandom.MakePixelPerfect();
		this.m_sSlotShield.MakePixelPerfect();
		this.m_sSlotRefill.MakePixelPerfect();
		this.SetEffect();
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00009377 File Offset: 0x00007577
	private void SetSpeed()
	{
		this.m_sSlotSpeed.spriteName = GameData.SPEEDEFFECTOR.ToString();
		this.m_sSlotSpeed.MakePixelPerfect();
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0000939E File Offset: 0x0000759E
	private void SetRand()
	{
		this.m_sSlotRandom.spriteName = GameData.RANDEFFECTOR.ToString();
		this.m_sSlotRandom.MakePixelPerfect();
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x000093C5 File Offset: 0x000075C5
	private void SetFade()
	{
		this.m_sSlotFade.spriteName = GameData.FADEEFFCTOR.ToString();
		this.m_sSlotFade.MakePixelPerfect();
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x000093EC File Offset: 0x000075EC
	public void SetRefill()
	{
		this.m_sSlotRefill.spriteName = GameData.REFILLITEM.ToString();
		this.m_sSlotRefill.MakePixelPerfect();
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00009413 File Offset: 0x00007613
	public void SetShield()
	{
		this.m_sSlotShield.spriteName = GameData.SHIELDITEM.ToString();
		this.m_sSlotShield.MakePixelPerfect();
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0000943A File Offset: 0x0000763A
	public void SetEffect()
	{
		this.SetSpeed();
		this.SetRand();
		this.SetFade();
		this.SetShield();
		this.SetRefill();
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000416D0 File Offset: 0x0003F8D0
	public void StartRefillAnimation()
	{
		this.m_sSlotRefill.color = Color.white;
		TweenColor component = this.m_sSlotRefill.GetComponent<TweenColor>();
		component.enabled = true;
		component.Play();
		base.Invoke("FinishRefilldAnimation", 2.4f);
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00041718 File Offset: 0x0003F918
	public void FinishRefilldAnimation()
	{
		TweenColor component = this.m_sSlotRefill.GetComponent<TweenColor>();
		component.enabled = false;
		this.m_sSlotRefill.color = Color.white;
		this.SetRefill();
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00041750 File Offset: 0x0003F950
	public void StartShieldAnimation()
	{
		this.m_sSlotShield.color = Color.white;
		TweenColor component = this.m_sSlotShield.GetComponent<TweenColor>();
		component.enabled = true;
		component.Play();
		base.Invoke("FinishShieldAnimation", 2.4f);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00041798 File Offset: 0x0003F998
	public void FinishShieldAnimation()
	{
		TweenColor component = this.m_sSlotShield.GetComponent<TweenColor>();
		component.enabled = false;
		this.m_sSlotShield.color = Color.white;
		this.SetShield();
	}

	// Token: 0x040006EF RID: 1775
	private const string OFF_NAME = "0_off";

	// Token: 0x040006F0 RID: 1776
	private const int MAX_ANIMATION = 4;

	// Token: 0x040006F1 RID: 1777
	private UISprite m_sSlotSpeed;

	// Token: 0x040006F2 RID: 1778
	private UISprite m_sSlotFade;

	// Token: 0x040006F3 RID: 1779
	private UISprite m_sSlotRandom;

	// Token: 0x040006F4 RID: 1780
	private UISprite m_sSlotShield;

	// Token: 0x040006F5 RID: 1781
	private UISprite m_sSlotRefill;
}
