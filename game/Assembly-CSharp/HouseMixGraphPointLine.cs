using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class HouseMixGraphPointLine : MonoBehaviour
{
	// Token: 0x06000D25 RID: 3365 RVA: 0x0000BC7B File Offset: 0x00009E7B
	public void StartLineUpdate()
	{
		this.isStart = true;
		this.oldDist = 0.0;
		this.Line.width = 2;
		this.Frame = 0f;
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x0005B86C File Offset: 0x00059A6C
	private void Update()
	{
		if (!this.isStart)
		{
			return;
		}
		this.Frame += Time.deltaTime;
		if (this.Frame >= 1f)
		{
			this.isStart = false;
			return;
		}
		Vector3 vector = Vector3.zero;
		Vector3 localPosition = this.PointFrom.localPosition;
		Vector3 localPosition2 = this.PointTo.localPosition;
		if (this.PointFrom.localPosition.x - this.PointTo.localPosition.x > 0f)
		{
			vector = localPosition - localPosition2;
		}
		else if (this.PointFrom.localPosition.x - this.PointTo.localPosition.x < 0f)
		{
			vector = localPosition2 - localPosition;
		}
		double num = Math.Sqrt((double)(vector.x * vector.x + vector.y * vector.y));
		this.Line.width = (int)num;
		double num2 = Math.Atan2((double)vector.y, (double)vector.x) * 57.295780181884766;
		if (num2 != 0.0)
		{
			this.Line.transform.rotation = Quaternion.Euler(this.Line.transform.eulerAngles.x, this.Line.transform.eulerAngles.y, (float)num2);
		}
	}

	// Token: 0x04000D1E RID: 3358
	public UISprite Line;

	// Token: 0x04000D1F RID: 3359
	public Transform PointFrom;

	// Token: 0x04000D20 RID: 3360
	public Transform PointTo;

	// Token: 0x04000D21 RID: 3361
	[HideInInspector]
	public bool isStart;

	// Token: 0x04000D22 RID: 3362
	private double oldDist;

	// Token: 0x04000D23 RID: 3363
	private float Frame;
}
