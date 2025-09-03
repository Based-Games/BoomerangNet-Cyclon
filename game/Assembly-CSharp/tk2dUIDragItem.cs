using System;
using UnityEngine;

// Token: 0x0200029A RID: 666
[AddComponentMenu("2D Toolkit/UI/tk2dUIDragItem")]
public class tk2dUIDragItem : tk2dUIBaseItemControl
{
	// Token: 0x0600130E RID: 4878 RVA: 0x000103AF File Offset: 0x0000E5AF
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			this.uiItem.OnRelease += this.ButtonRelease;
		}
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x00084CDC File Offset: 0x00082EDC
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			this.uiItem.OnRelease -= this.ButtonRelease;
		}
		if (this.isBtnActive)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.UpdateBtnPosition;
			}
			this.isBtnActive = false;
		}
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x000103EF File Offset: 0x0000E5EF
	private void UpdateBtnPosition()
	{
		base.transform.position = this.CalculateNewPos();
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x00084D60 File Offset: 0x00082F60
	private Vector3 CalculateNewPos()
	{
		Vector2 position = this.uiItem.Touch.position;
		Camera uicameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		Vector3 vector = uicameraForControl.ScreenToWorldPoint(new Vector3(position.x, position.y, base.transform.position.z - uicameraForControl.transform.position.z));
		vector.z = base.transform.position.z;
		return vector + this.offset;
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00084E00 File Offset: 0x00083000
	public void ButtonDown()
	{
		if (!this.isBtnActive)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.UpdateBtnPosition;
		}
		this.isBtnActive = true;
		this.offset = Vector3.zero;
		Vector3 vector = this.CalculateNewPos();
		this.offset = base.transform.position - vector;
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x00010402 File Offset: 0x0000E602
	public void ButtonRelease()
	{
		if (this.isBtnActive)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.UpdateBtnPosition;
		}
		this.isBtnActive = false;
	}

	// Token: 0x040014DA RID: 5338
	public tk2dUIManager uiManager;

	// Token: 0x040014DB RID: 5339
	private Vector3 offset = Vector3.zero;

	// Token: 0x040014DC RID: 5340
	private bool isBtnActive;
}
