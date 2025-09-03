using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000072 RID: 114
public static class NGUIMath
{
	// Token: 0x060002DC RID: 732 RVA: 0x0000582B File Offset: 0x00003A2B
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float Lerp(float from, float to, float factor)
	{
		return from * (1f - factor) + to * factor;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000583A File Offset: 0x00003A3A
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static int ClampIndex(int val, int max)
	{
		return (val >= 0) ? ((val >= max) ? (max - 1) : val) : 0;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00005859 File Offset: 0x00003A59
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int RepeatIndex(int val, int max)
	{
		if (max < 1)
		{
			return 0;
		}
		while (val < 0)
		{
			val += max;
		}
		while (val >= max)
		{
			val -= max;
		}
		return val;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00005887 File Offset: 0x00003A87
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float WrapAngle(float angle)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x000058BC File Offset: 0x00003ABC
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float Wrap01(float val)
	{
		return val - (float)Mathf.FloorToInt(val);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0001EAF0 File Offset: 0x0001CCF0
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static int HexToDecimal(char ch)
	{
		switch (ch)
		{
		case '0':
			return 0;
		case '1':
			return 1;
		case '2':
			return 2;
		case '3':
			return 3;
		case '4':
			return 4;
		case '5':
			return 5;
		case '6':
			return 6;
		case '7':
			return 7;
		case '8':
			return 8;
		case '9':
			return 9;
		default:
			switch (ch)
			{
			case 'a':
				break;
			case 'b':
				return 11;
			case 'c':
				return 12;
			case 'd':
				return 13;
			case 'e':
				return 14;
			case 'f':
				return 15;
			default:
				return 15;
			}
			break;
		case 'A':
			break;
		case 'B':
			return 11;
		case 'C':
			return 12;
		case 'D':
			return 13;
		case 'E':
			return 14;
		case 'F':
			return 15;
		}
		return 10;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000058C7 File Offset: 0x00003AC7
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static char DecimalToHexChar(int num)
	{
		if (num > 15)
		{
			return 'F';
		}
		if (num < 10)
		{
			return (char)(48 + num);
		}
		return (char)(65 + num - 10);
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000058EA File Offset: 0x00003AEA
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static string DecimalToHex(int num)
	{
		num &= 16777215;
		return num.ToString("X6");
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0001EBB4 File Offset: 0x0001CDB4
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int ColorToInt(Color c)
	{
		int num = 0;
		num |= Mathf.RoundToInt(c.r * 255f) << 24;
		num |= Mathf.RoundToInt(c.g * 255f) << 16;
		num |= Mathf.RoundToInt(c.b * 255f) << 8;
		return num | Mathf.RoundToInt(c.a * 255f);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0001EC20 File Offset: 0x0001CE20
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color IntToColor(int val)
	{
		float num = 0.003921569f;
		Color black = Color.black;
		black.r = num * (float)((val >> 24) & 255);
		black.g = num * (float)((val >> 16) & 255);
		black.b = num * (float)((val >> 8) & 255);
		black.a = num * (float)(val & 255);
		return black;
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0001EC88 File Offset: 0x0001CE88
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string IntToBinary(int val, int bits)
	{
		string text = string.Empty;
		int i = bits;
		while (i > 0)
		{
			if (i == 8 || i == 16 || i == 24)
			{
				text += " ";
			}
			text += (((val & (1 << --i)) == 0) ? '0' : '1');
		}
		return text;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00005901 File Offset: 0x00003B01
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color HexToColor(uint val)
	{
		return NGUIMath.IntToColor((int)val);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0001ECF8 File Offset: 0x0001CEF8
	public static Rect ConvertToTexCoords(Rect rect, int width, int height)
	{
		Rect rect2 = rect;
		if ((float)width != 0f && (float)height != 0f)
		{
			rect2.xMin = rect.xMin / (float)width;
			rect2.xMax = rect.xMax / (float)width;
			rect2.yMin = 1f - rect.yMax / (float)height;
			rect2.yMax = 1f - rect.yMin / (float)height;
		}
		return rect2;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0001ED70 File Offset: 0x0001CF70
	public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
	{
		Rect rect2 = rect;
		if (round)
		{
			rect2.xMin = (float)Mathf.RoundToInt(rect.xMin * (float)width);
			rect2.xMax = (float)Mathf.RoundToInt(rect.xMax * (float)width);
			rect2.yMin = (float)Mathf.RoundToInt((1f - rect.yMax) * (float)height);
			rect2.yMax = (float)Mathf.RoundToInt((1f - rect.yMin) * (float)height);
		}
		else
		{
			rect2.xMin = rect.xMin * (float)width;
			rect2.xMax = rect.xMax * (float)width;
			rect2.yMin = (1f - rect.yMax) * (float)height;
			rect2.yMax = (1f - rect.yMin) * (float)height;
		}
		return rect2;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0001EE44 File Offset: 0x0001D044
	public static Rect MakePixelPerfect(Rect rect)
	{
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return rect;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
	public static Rect MakePixelPerfect(Rect rect, int width, int height)
	{
		rect = NGUIMath.ConvertToPixels(rect, width, height, true);
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return NGUIMath.ConvertToTexCoords(rect, width, height);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0001EF14 File Offset: 0x0001D114
	public static Vector2 ConstrainRect(Vector2 minRect, Vector2 maxRect, Vector2 minArea, Vector2 maxArea)
	{
		Vector2 zero = Vector2.zero;
		float num = maxRect.x - minRect.x;
		float num2 = maxRect.y - minRect.y;
		float num3 = maxArea.x - minArea.x;
		float num4 = maxArea.y - minArea.y;
		if (num > num3)
		{
			float num5 = num - num3;
			minArea.x -= num5;
			maxArea.x += num5;
		}
		if (num2 > num4)
		{
			float num6 = num2 - num4;
			minArea.y -= num6;
			maxArea.y += num6;
		}
		if (minRect.x < minArea.x)
		{
			zero.x += minArea.x - minRect.x;
		}
		if (maxRect.x > maxArea.x)
		{
			zero.x -= maxRect.x - maxArea.x;
		}
		if (minRect.y < minArea.y)
		{
			zero.y += minArea.y - minRect.y;
		}
		if (maxRect.y > maxArea.y)
		{
			zero.y -= maxRect.y - maxArea.y;
		}
		return zero;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0001F084 File Offset: 0x0001D284
	public static Bounds CalculateAbsoluteWidgetBounds(Transform trans)
	{
		UIWidget[] componentsInChildren = trans.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return new Bounds(trans.position, Vector3.zero);
		}
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			UIWidget uiwidget = componentsInChildren[i];
			if (uiwidget.enabled)
			{
				Vector3[] worldCorners = uiwidget.worldCorners;
				for (int j = 0; j < 4; j++)
				{
					vector2 = Vector3.Max(worldCorners[j], vector2);
					vector = Vector3.Min(worldCorners[j], vector);
				}
			}
			i++;
		}
		Bounds bounds = new Bounds(vector, Vector3.zero);
		bounds.Encapsulate(vector2);
		return bounds;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00005909 File Offset: 0x00003B09
	public static Bounds CalculateRelativeWidgetBounds(Transform trans)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans, false);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00005913 File Offset: 0x00003B13
	public static Bounds CalculateRelativeWidgetBounds(Transform trans, bool considerInactive)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans, considerInactive);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000591D File Offset: 0x00003B1D
	public static Bounds CalculateRelativeWidgetBounds(Transform root, Transform child)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(root, child, false);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0001F16C File Offset: 0x0001D36C
	public static Bounds CalculateRelativeWidgetBounds(Transform root, Transform child, bool considerInactive)
	{
		UIWidget[] componentsInChildren = child.GetComponentsInChildren<UIWidget>(considerInactive);
		if (componentsInChildren.Length > 0)
		{
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			Matrix4x4 worldToLocalMatrix = root.worldToLocalMatrix;
			bool flag = false;
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIWidget uiwidget = componentsInChildren[i];
				if (considerInactive || uiwidget.enabled)
				{
					Vector3[] worldCorners = uiwidget.worldCorners;
					for (int j = 0; j < 4; j++)
					{
						Vector3 vector3 = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[j]);
						vector2 = Vector3.Max(vector3, vector2);
						vector = Vector3.Min(vector3, vector);
					}
					flag = true;
				}
				i++;
			}
			if (flag)
			{
				Bounds bounds = new Bounds(vector, Vector3.zero);
				bounds.Encapsulate(vector2);
				return bounds;
			}
		}
		return new Bounds(Vector3.zero, Vector3.zero);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0001F274 File Offset: 0x0001D474
	public static Vector3 SpringDampen(ref Vector3 velocity, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		float num = 1f - strength * 0.001f;
		int num2 = Mathf.RoundToInt(deltaTime * 1000f);
		float num3 = Mathf.Pow(num, (float)num2);
		Vector3 vector = velocity * ((num3 - 1f) / Mathf.Log(num));
		velocity *= num3;
		return vector * 0.06f;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
	public static Vector2 SpringDampen(ref Vector2 velocity, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		float num = 1f - strength * 0.001f;
		int num2 = Mathf.RoundToInt(deltaTime * 1000f);
		float num3 = Mathf.Pow(num, (float)num2);
		Vector2 vector = velocity * ((num3 - 1f) / Mathf.Log(num));
		velocity *= num3;
		return vector * 0.06f;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0001F36C File Offset: 0x0001D56C
	public static float SpringLerp(float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 = Mathf.Lerp(num2, 1f, deltaTime);
		}
		return num2;
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
	public static float SpringLerp(float from, float to, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		for (int i = 0; i < num; i++)
		{
			from = Mathf.Lerp(from, to, deltaTime);
		}
		return from;
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00005927 File Offset: 0x00003B27
	public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime)
	{
		return Vector2.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00005937 File Offset: 0x00003B37
	public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
	{
		return Vector3.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00005947 File Offset: 0x00003B47
	public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
	{
		return Quaternion.Slerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0001F41C File Offset: 0x0001D61C
	public static float RotateTowards(float from, float to, float maxAngle)
	{
		float num = NGUIMath.WrapAngle(to - from);
		if (Mathf.Abs(num) > maxAngle)
		{
			num = maxAngle * Mathf.Sign(num);
		}
		return from + num;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0001F44C File Offset: 0x0001D64C
	private static float DistancePointToLineSegment(Vector2 point, Vector2 a, Vector2 b)
	{
		float sqrMagnitude = (b - a).sqrMagnitude;
		if (sqrMagnitude == 0f)
		{
			return (point - a).magnitude;
		}
		float num = Vector2.Dot(point - a, b - a) / sqrMagnitude;
		if (num < 0f)
		{
			return (point - a).magnitude;
		}
		if (num > 1f)
		{
			return (point - b).magnitude;
		}
		Vector2 vector = a + num * (b - a);
		return (point - vector).magnitude;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0001F4F8 File Offset: 0x0001D6F8
	public static float DistanceToRectangle(Vector2[] screenPoints, Vector2 mousePos)
	{
		bool flag = false;
		int num = 4;
		for (int i = 0; i < 5; i++)
		{
			Vector3 vector = screenPoints[NGUIMath.RepeatIndex(i, 4)];
			Vector3 vector2 = screenPoints[NGUIMath.RepeatIndex(num, 4)];
			if (vector.y > mousePos.y != vector2.y > mousePos.y && mousePos.x < (vector2.x - vector.x) * (mousePos.y - vector.y) / (vector2.y - vector.y) + vector.x)
			{
				flag = !flag;
			}
			num = i;
		}
		if (!flag)
		{
			float num2 = -1f;
			for (int j = 0; j < 4; j++)
			{
				Vector3 vector3 = screenPoints[j];
				Vector3 vector4 = screenPoints[NGUIMath.RepeatIndex(j + 1, 4)];
				float num3 = NGUIMath.DistancePointToLineSegment(mousePos, vector3, vector4);
				if (num3 < num2 || num2 < 0f)
				{
					num2 = num3;
				}
			}
			return num2;
		}
		return 0f;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0001F648 File Offset: 0x0001D848
	public static float DistanceToRectangle(Vector3[] worldPoints, Vector2 mousePos, Camera cam)
	{
		Vector2[] array = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = cam.WorldToScreenPoint(worldPoints[i]);
		}
		return NGUIMath.DistanceToRectangle(array, mousePos);
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0001F698 File Offset: 0x0001D898
	public static Vector2 GetPivotOffset(UIWidget.Pivot pv)
	{
		Vector2 zero = Vector2.zero;
		if (pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Bottom)
		{
			zero.x = 0.5f;
		}
		else if (pv == UIWidget.Pivot.TopRight || pv == UIWidget.Pivot.Right || pv == UIWidget.Pivot.BottomRight)
		{
			zero.x = 1f;
		}
		else
		{
			zero.x = 0f;
		}
		if (pv == UIWidget.Pivot.Left || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Right)
		{
			zero.y = 0.5f;
		}
		else if (pv == UIWidget.Pivot.TopLeft || pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.TopRight)
		{
			zero.y = 1f;
		}
		else
		{
			zero.y = 0f;
		}
		return zero;
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0001F75C File Offset: 0x0001D95C
	public static void MoveWidget(UIWidget w, float x, float y)
	{
		int num = Mathf.FloorToInt(x + 0.5f);
		int num2 = Mathf.FloorToInt(y + 0.5f);
		Transform cachedTransform = w.cachedTransform;
		cachedTransform.localPosition += new Vector3((float)num, (float)num2);
		int num3 = 0;
		if (w.leftAnchor.target)
		{
			num3++;
			w.leftAnchor.absolute += num;
		}
		if (w.rightAnchor.target)
		{
			num3++;
			w.rightAnchor.absolute += num;
		}
		if (w.bottomAnchor.target)
		{
			num3++;
			w.bottomAnchor.absolute += num2;
		}
		if (w.topAnchor.target)
		{
			num3++;
			w.topAnchor.absolute += num2;
		}
		if (num3 != 0)
		{
			w.UpdateAnchors();
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001F864 File Offset: 0x0001DA64
	public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight)
	{
		if (pivot == UIWidget.Pivot.Center)
		{
			NGUIMath.MoveWidget(w, x, y);
			return;
		}
		Vector3 vector = new Vector3(x, y);
		vector = Quaternion.Inverse(w.cachedTransform.localRotation) * vector;
		switch (pivot)
		{
		case UIWidget.Pivot.TopLeft:
			NGUIMath.AdjustWidget(w, vector.x, 0f, 0f, vector.y, minWidth, minHeight);
			break;
		case UIWidget.Pivot.Top:
			NGUIMath.AdjustWidget(w, 0f, 0f, 0f, vector.y, minWidth, minHeight);
			break;
		case UIWidget.Pivot.TopRight:
			NGUIMath.AdjustWidget(w, 0f, 0f, vector.x, vector.y, minWidth, minHeight);
			break;
		case UIWidget.Pivot.Left:
			NGUIMath.AdjustWidget(w, vector.x, 0f, 0f, 0f, minWidth, minHeight);
			break;
		case UIWidget.Pivot.Right:
			NGUIMath.AdjustWidget(w, 0f, 0f, vector.x, 0f, minWidth, minHeight);
			break;
		case UIWidget.Pivot.BottomLeft:
			NGUIMath.AdjustWidget(w, vector.x, vector.y, 0f, 0f, minWidth, minHeight);
			break;
		case UIWidget.Pivot.Bottom:
			NGUIMath.AdjustWidget(w, 0f, vector.y, 0f, 0f, minWidth, minHeight);
			break;
		case UIWidget.Pivot.BottomRight:
			NGUIMath.AdjustWidget(w, 0f, vector.y, vector.x, 0f, minWidth, minHeight);
			break;
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0001FA04 File Offset: 0x0001DC04
	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight)
	{
		Vector2 pivotOffset = w.pivotOffset;
		Transform transform = w.cachedTransform;
		Quaternion localRotation = transform.localRotation;
		int num = Mathf.FloorToInt(left + 0.5f);
		int num2 = Mathf.FloorToInt(bottom + 0.5f);
		int num3 = Mathf.FloorToInt(right + 0.5f);
		int num4 = Mathf.FloorToInt(top + 0.5f);
		if (pivotOffset.x == 0.5f)
		{
			num = num >> 1 << 1;
			num3 = num3 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num2 = num2 >> 1 << 1;
			num4 = num4 >> 1 << 1;
		}
		Vector3 vector = localRotation * new Vector3((float)num, (float)num4);
		Vector3 vector2 = localRotation * new Vector3((float)num3, (float)num4);
		Vector3 vector3 = localRotation * new Vector3((float)num, (float)num2);
		Vector3 vector4 = localRotation * new Vector3((float)num3, (float)num2);
		Vector3 vector5 = localRotation * new Vector3((float)num, 0f);
		Vector3 vector6 = localRotation * new Vector3((float)num3, 0f);
		Vector3 vector7 = localRotation * new Vector3(0f, (float)num4);
		Vector3 vector8 = localRotation * new Vector3(0f, (float)num2);
		Vector3 zero = Vector3.zero;
		if (pivotOffset.x == 0f && pivotOffset.y == 1f)
		{
			zero.x = vector.x;
			zero.y = vector.y;
		}
		else if (pivotOffset.x == 1f && pivotOffset.y == 0f)
		{
			zero.x = vector4.x;
			zero.y = vector4.y;
		}
		else if (pivotOffset.x == 0f && pivotOffset.y == 0f)
		{
			zero.x = vector3.x;
			zero.y = vector3.y;
		}
		else if (pivotOffset.x == 1f && pivotOffset.y == 1f)
		{
			zero.x = vector2.x;
			zero.y = vector2.y;
		}
		else if (pivotOffset.x == 0f && pivotOffset.y == 0.5f)
		{
			zero.x = vector5.x + (vector7.x + vector8.x) * 0.5f;
			zero.y = vector5.y + (vector7.y + vector8.y) * 0.5f;
		}
		else if (pivotOffset.x == 1f && pivotOffset.y == 0.5f)
		{
			zero.x = vector6.x + (vector7.x + vector8.x) * 0.5f;
			zero.y = vector6.y + (vector7.y + vector8.y) * 0.5f;
		}
		else if (pivotOffset.x == 0.5f && pivotOffset.y == 1f)
		{
			zero.x = vector7.x + (vector5.x + vector6.x) * 0.5f;
			zero.y = vector7.y + (vector5.y + vector6.y) * 0.5f;
		}
		else if (pivotOffset.x == 0.5f && pivotOffset.y == 0f)
		{
			zero.x = vector8.x + (vector5.x + vector6.x) * 0.5f;
			zero.y = vector8.y + (vector5.y + vector6.y) * 0.5f;
		}
		else if (pivotOffset.x == 0.5f && pivotOffset.y == 0.5f)
		{
			zero.x = (vector5.x + vector6.x + vector7.x + vector8.x) * 0.5f;
			zero.y = (vector7.y + vector8.y + vector5.y + vector6.y) * 0.5f;
		}
		int num5 = Mathf.Max(minWidth, w.minWidth);
		int num6 = Mathf.Max(minHeight, w.minHeight);
		int num7 = w.width + num3 - num;
		int num8 = w.height + num4 - num2;
		Vector3 zero2 = Vector3.zero;
		if (num7 < num5)
		{
			if (num != 0)
			{
				zero2.x -= Mathf.Lerp((float)(num5 - num7), 0f, pivotOffset.x);
			}
			else
			{
				zero2.x += Mathf.Lerp(0f, (float)(num5 - num7), pivotOffset.x);
			}
			num7 = num5;
		}
		if (num8 < num6)
		{
			if (num2 != 0)
			{
				zero2.y -= Mathf.Lerp((float)(num6 - num8), 0f, pivotOffset.y);
			}
			else
			{
				zero2.y += Mathf.Lerp(0f, (float)(num6 - num8), pivotOffset.y);
			}
			num8 = num6;
		}
		if (num7 < minWidth)
		{
			num7 = minWidth;
		}
		if (num8 < minHeight)
		{
			num8 = minHeight;
		}
		if (pivotOffset.x == 0.5f)
		{
			num7 = num7 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num8 = num8 >> 1 << 1;
		}
		Vector3 vector9 = transform.localPosition + zero + localRotation * zero2;
		transform.localPosition = vector9;
		w.width = num7;
		w.height = num8;
		if (w.isAnchored)
		{
			transform = transform.parent;
			float num9 = vector9.x - pivotOffset.x * (float)num7;
			float num10 = vector9.y - pivotOffset.y * (float)num8;
			if (w.leftAnchor.target)
			{
				w.leftAnchor.SetHorizontal(transform, num9);
			}
			if (w.rightAnchor.target)
			{
				w.rightAnchor.SetHorizontal(transform, num9 + (float)num7);
			}
			if (w.bottomAnchor.target)
			{
				w.bottomAnchor.SetVertical(transform, num10);
			}
			if (w.topAnchor.target)
			{
				w.topAnchor.SetVertical(transform, num10 + (float)num8);
			}
		}
	}
}
