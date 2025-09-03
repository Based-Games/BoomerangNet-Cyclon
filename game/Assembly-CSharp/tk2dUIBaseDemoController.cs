using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public class tk2dUIBaseDemoController : MonoBehaviour
{
	// Token: 0x06001493 RID: 5267 RVA: 0x0008ADDC File Offset: 0x00088FDC
	protected void RegisterWindow(Transform t)
	{
		this.RemoveUnity3HackFromWindow(t);
		this.ShowWindow(t);
		tk2dUIBaseDemoController.InitTransform initTransform = new tk2dUIBaseDemoController.InitTransform();
		initTransform.pos = t.position;
		initTransform.scale = t.localScale;
		initTransform.angle = t.eulerAngles.z;
		this.registeredWindows.Add(t, initTransform);
		this.HideWindow(t);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0008AE40 File Offset: 0x00089040
	protected void AnimateShowWindow(Transform t)
	{
		if (!this.registeredWindows.ContainsKey(t))
		{
			this.RegisterWindow(t);
		}
		tk2dUIBaseDemoController.InitTransform initTransform = this.registeredWindows[t];
		this.ShowWindow(t);
		t.localPosition = new Vector3(-5f, 0f, 0f);
		t.localScale = Vector3.zero;
		t.localEulerAngles = new Vector3(0f, 0f, 10f);
		base.StartCoroutine(this.coTweenTransformTo(t, 0.3f, initTransform.pos, initTransform.scale, initTransform.angle));
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x00011CC0 File Offset: 0x0000FEC0
	protected void AnimateHideWindow(Transform t)
	{
		if (!this.registeredWindows.ContainsKey(t))
		{
			this.RegisterWindow(t);
		}
		base.StartCoroutine(this.coAnimateHideWindow(t));
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x0008AEE0 File Offset: 0x000890E0
	private IEnumerator coAnimateHideWindow(Transform t)
	{
		yield return base.StartCoroutine(this.coTweenTransformTo(t, 0.3f, new Vector3(5f, 0f, 0f), Vector3.zero, -10f));
		this.HideWindow(t);
		yield break;
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0008AF0C File Offset: 0x0008910C
	protected IEnumerator coResizeLayout(tk2dUILayout layout, Vector3 min, Vector3 max, float time)
	{
		Vector3 minFrom = layout.GetMinBounds();
		Vector3 maxFrom = layout.GetMaxBounds();
		for (float t = 0f; t < time; t += tk2dUITime.deltaTime)
		{
			float nt = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / time));
			Vector3 currMin = Vector3.Lerp(minFrom, min, nt);
			Vector3 currMax = Vector3.Lerp(maxFrom, max, nt);
			layout.SetBounds(currMin, currMax);
			yield return 0;
		}
		layout.SetBounds(min, max);
		yield break;
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0008AF5C File Offset: 0x0008915C
	protected IEnumerator coTweenAngle(Transform t, float xAngle, float time)
	{
		float xStart = t.localEulerAngles.x;
		if (xStart > 0f)
		{
			xStart -= 360f;
		}
		for (float ut = 0f; ut < time; ut += Time.deltaTime)
		{
			float nt = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(ut / time));
			float a = Mathf.Lerp(xStart, xAngle, nt);
			t.localEulerAngles = new Vector3(a, 0f, 0f);
			yield return 0;
		}
		t.localEulerAngles = new Vector3(xAngle, 0f, 0f);
		yield break;
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0008AF9C File Offset: 0x0008919C
	protected IEnumerator coMove(Transform t, Vector3 targetPosition, float time)
	{
		Vector3 startPosition = t.position;
		for (float ut = 0f; ut < time; ut += Time.deltaTime)
		{
			float nt = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(ut / time));
			t.position = Vector3.Lerp(startPosition, targetPosition, nt);
			yield return 0;
		}
		t.position = targetPosition;
		yield break;
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x0008AFDC File Offset: 0x000891DC
	protected IEnumerator coShake(Transform t, Vector3 translateConstraint, Vector3 rotationConstraint, float time)
	{
		Vector3 pos = t.position;
		Quaternion rot = t.rotation;
		for (float ut = 0f; ut < time; ut += Time.deltaTime)
		{
			float nt = Mathf.Clamp01(ut / time);
			float strength = 1f - nt;
			t.position = pos + Vector3.Scale(UnityEngine.Random.onUnitSphere, translateConstraint).normalized * strength * 0.01f;
			t.rotation = rot;
			t.Rotate(Vector3.Scale(UnityEngine.Random.onUnitSphere, rotationConstraint), 2f * strength);
			yield return 0;
		}
		t.position = pos;
		t.rotation = rot;
		yield break;
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0008B02C File Offset: 0x0008922C
	protected IEnumerator coTweenTransformTo(Transform transform, float time, Vector3 toPos, Vector3 toScale, float toRotation)
	{
		Vector3 fromPos = transform.localPosition;
		Vector3 fromScale = transform.localScale;
		Vector3 euler = transform.localEulerAngles;
		float fromRotation = euler.z;
		for (float t = 0f; t < time; t += tk2dUITime.deltaTime)
		{
			float nt = Mathf.Clamp01(t / time);
			nt = Mathf.Sin(nt * 3.1415927f * 0.5f);
			transform.localPosition = Vector3.Lerp(fromPos, toPos, nt);
			transform.localScale = Vector3.Lerp(fromScale, toScale, nt);
			euler.z = Mathf.Lerp(fromRotation, toRotation, nt);
			transform.localEulerAngles = euler;
			yield return 0;
		}
		euler.z = toRotation;
		transform.localPosition = toPos;
		transform.localScale = toScale;
		transform.localEulerAngles = euler;
		yield break;
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x00011CE8 File Offset: 0x0000FEE8
	protected void DoSetActive(Transform t, bool state)
	{
		t.gameObject.SetActive(state);
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x00011CF6 File Offset: 0x0000FEF6
	protected void ShowWindow(Transform t)
	{
		t.gameObject.SetActive(true);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x00011D04 File Offset: 0x0000FF04
	protected void HideWindow(Transform t)
	{
		t.gameObject.SetActive(false);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x0008B08C File Offset: 0x0008928C
	protected void RemoveUnity3HackFromWindow(Transform t)
	{
		Vector3 position = t.position;
		position.y %= 1f;
		position.x %= 2f;
		t.position = position;
	}

	// Token: 0x040015E5 RID: 5605
	private Dictionary<Transform, tk2dUIBaseDemoController.InitTransform> registeredWindows = new Dictionary<Transform, tk2dUIBaseDemoController.InitTransform>();

	// Token: 0x020002BA RID: 698
	private class InitTransform
	{
		// Token: 0x040015E6 RID: 5606
		public Vector3 pos;

		// Token: 0x040015E7 RID: 5607
		public Vector3 scale;

		// Token: 0x040015E8 RID: 5608
		public float angle;
	}
}
