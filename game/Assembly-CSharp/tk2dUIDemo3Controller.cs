using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class tk2dUIDemo3Controller : tk2dUIBaseDemoController
{
	// Token: 0x060014D0 RID: 5328 RVA: 0x0008BB48 File Offset: 0x00089D48
	private IEnumerator Start()
	{
		this.overlayRestPosition = this.overlayInterface.position;
		this.HideOverlay();
		Vector3 instructionsRestPos = this.instructions.position;
		this.instructions.position = this.instructions.position + this.instructions.up * 10f;
		base.StartCoroutine(base.coMove(this.instructions, instructionsRestPos, 1f));
		yield return new WaitForSeconds(3f);
		base.StartCoroutine(base.coMove(this.instructions, instructionsRestPos - this.instructions.up * 10f, 1f));
		yield break;
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x0008BB64 File Offset: 0x00089D64
	public void ToggleCase(tk2dUIToggleButton button)
	{
		float num = (float)((!button.IsOn) ? 0 : (-66));
		base.StartCoroutine(base.coTweenAngle(button.transform, num, 0.5f));
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x0008BBA0 File Offset: 0x00089DA0
	private IEnumerator coRedButtonPressed()
	{
		base.StartCoroutine(base.coShake(this.perspectiveCamera, Vector3.one, Vector3.one, 1f));
		yield return new WaitForSeconds(0.3f);
		this.ShowOverlay();
		yield break;
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x0008BBBC File Offset: 0x00089DBC
	private void ShowOverlay()
	{
		this.overlayInterface.gameObject.SetActive(true);
		Vector3 vector = this.overlayRestPosition;
		vector.y = -2.5f;
		this.overlayInterface.position = vector;
		base.StartCoroutine(base.coMove(this.overlayInterface, this.overlayRestPosition, 0.15f));
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x0008BC18 File Offset: 0x00089E18
	private IEnumerator coHideOverlay()
	{
		Vector3 v = this.overlayRestPosition;
		v.y = -2.5f;
		yield return base.StartCoroutine(base.coMove(this.overlayInterface, v, 0.15f));
		this.HideOverlay();
		yield break;
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x00011DD6 File Offset: 0x0000FFD6
	private void HideOverlay()
	{
		this.overlayInterface.gameObject.SetActive(false);
	}

	// Token: 0x04001640 RID: 5696
	public Transform perspectiveCamera;

	// Token: 0x04001641 RID: 5697
	public Transform overlayInterface;

	// Token: 0x04001642 RID: 5698
	private Vector3 overlayRestPosition = Vector3.zero;

	// Token: 0x04001643 RID: 5699
	public Transform instructions;
}
