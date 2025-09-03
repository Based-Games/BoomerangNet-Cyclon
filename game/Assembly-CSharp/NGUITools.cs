using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x02000076 RID: 118
public static class NGUITools
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000316 RID: 790 RVA: 0x00005A26 File Offset: 0x00003C26
	// (set) Token: 0x06000317 RID: 791 RVA: 0x00005A51 File Offset: 0x00003C51
	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000318 RID: 792 RVA: 0x00005A75 File Offset: 0x00003C75
	public static bool fileAccess
	{
		get
		{
			return Application.platform != RuntimePlatform.WindowsWebPlayer && Application.platform != RuntimePlatform.OSXWebPlayer;
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00005A90 File Offset: 0x00003C90
	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00005AA2 File Offset: 0x00003CA2
	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000210EC File Offset: 0x0001F2EC
	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				NGUITools.mListener = UnityEngine.Object.FindObjectOfType(typeof(AudioListener)) as AudioListener;
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera;
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.enabled && NGUITools.GetActive(NGUITools.mListener.gameObject))
			{
				AudioSource audioSource = NGUITools.mListener.audio;
				if (audioSource == null)
				{
					audioSource = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
				}
				audioSource.pitch = pitch;
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00021210 File Offset: 0x0001F410
	public static WWW OpenURL(string url)
	{
		WWW www = null;
		try
		{
			www = new WWW(url);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
		}
		return www;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00021250 File Offset: 0x0001F450
	public static WWW OpenURL(string url, WWWForm form)
	{
		if (form == null)
		{
			return NGUITools.OpenURL(url);
		}
		WWW www = null;
		try
		{
			www = new WWW(url, form);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError((ex == null) ? "<null>" : ex.Message);
		}
		return www;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00005AB0 File Offset: 0x00003CB0
	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000212AC File Offset: 0x0001F4AC
	public static string GetHierarchy(GameObject obj)
	{
		string text = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = obj.name + "\\" + text;
		}
		return text;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00005AC4 File Offset: 0x00003CC4
	public static T[] FindActive<T>() where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00021300 File Offset: 0x0001F500
	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			Camera cachedCamera = UICamera.list.buffer[i].cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return cachedCamera;
			}
		}
		Camera[] array = NGUITools.FindActive<Camera>();
		int j = 0;
		int num2 = array.Length;
		while (j < num2)
		{
			Camera camera = array[j];
			if ((camera.cullingMask & num) != 0)
			{
				return camera;
			}
			j++;
		}
		return null;
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00005ADA File Offset: 0x00003CDA
	public static BoxCollider AddWidgetCollider(GameObject go)
	{
		return NGUITools.AddWidgetCollider(go, false);
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00021398 File Offset: 0x0001F598
	public static BoxCollider AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider == null)
			{
				if (component != null)
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(component);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(component);
					}
				}
				boxCollider = go.AddComponent<BoxCollider>();
				boxCollider.isTrigger = true;
				UIWidget component2 = go.GetComponent<UIWidget>();
				if (component2 != null)
				{
					component2.autoResizeBoxCollider = true;
				}
			}
			NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
			return boxCollider;
		}
		return null;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00005AE3 File Offset: 0x00003CE3
	public static void UpdateWidgetCollider(GameObject go)
	{
		NGUITools.UpdateWidgetCollider(go, false);
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00005AEC File Offset: 0x00003CEC
	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			NGUITools.UpdateWidgetCollider(go.GetComponent<BoxCollider>(), considerInactive);
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00005B06 File Offset: 0x00003D06
	public static void UpdateWidgetCollider(BoxCollider bc)
	{
		NGUITools.UpdateWidgetCollider(bc, false);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00021424 File Offset: 0x0001F624
	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				if (!component.isVisible)
				{
					return;
				}
				Vector4 drawingDimensions = component.drawingDimensions;
				box.center = new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f);
				box.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				box.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
			}
		}
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00021518 File Offset: 0x0001F718
	public static string GetTypeName<T>()
	{
		string text = typeof(T).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0002156C File Offset: 0x0001F76C
	public static string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00003648 File Offset: 0x00001848
	public static void RegisterUndo(UnityEngine.Object obj, string name)
	{
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00005B0F File Offset: 0x00003D0F
	public static GameObject AddChild(GameObject parent)
	{
		return NGUITools.AddChild(parent, true);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x000215D0 File Offset: 0x0001F7D0
	public static GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00021630 File Offset: 0x0001F830
	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000216A4 File Offset: 0x0001F8A4
	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = int.MaxValue;
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			if (componentsInChildren[i].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
			i++;
		}
		return num;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00021718 File Offset: 0x0001F918
	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			num = Mathf.Max(num, componentsInChildren[i].depth);
			i++;
		}
		return num + 1;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00021758 File Offset: 0x0001F958
	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				UIWidget uiwidget = componentsInChildren[i];
				if (!(uiwidget.cachedGameObject != go) || !(uiwidget.collider != null))
				{
					num = Mathf.Max(num, uiwidget.depth);
				}
				i++;
			}
			return num + 1;
		}
		return NGUITools.CalculateNextDepth(go);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000217D0 File Offset: 0x0001F9D0
	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (!(go != null))
		{
			return 0;
		}
		UIPanel component = go.GetComponent<UIPanel>();
		if (component != null)
		{
			foreach (UIPanel uipanel in go.GetComponentsInChildren<UIPanel>(true))
			{
				uipanel.depth += adjustment;
			}
			return 1;
		}
		UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(true);
		int j = 0;
		int num = componentsInChildren2.Length;
		while (j < num)
		{
			UIWidget uiwidget = componentsInChildren2[j];
			uiwidget.depth += adjustment;
			j++;
		}
		return 2;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0002186C File Offset: 0x0001FA6C
	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	// Token: 0x06000333 RID: 819 RVA: 0x000218A4 File Offset: 0x0001FAA4
	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00005B18 File Offset: 0x00003D18
	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	// Token: 0x06000335 RID: 821 RVA: 0x000218DC File Offset: 0x0001FADC
	public static void NormalizeWidgetDepths()
	{
		UIWidget[] array = NGUITools.FindActive<UIWidget>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIWidget>(array, new Comparison<UIWidget>(UIWidget.CompareFunc));
			int num2 = 0;
			int num3 = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIWidget uiwidget = array[i];
				if (uiwidget.depth == num3)
				{
					uiwidget.depth = num2;
				}
				else
				{
					num3 = uiwidget.depth;
					num2 = (uiwidget.depth = num2 + 1);
				}
			}
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00021964 File Offset: 0x0001FB64
	public static void NormalizePanelDepths()
	{
		UIPanel[] array = NGUITools.FindActive<UIPanel>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIPanel>(array, new Comparison<UIPanel>(UIPanel.CompareFunc));
			int num2 = 0;
			int num3 = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIPanel uipanel = array[i];
				if (uipanel.depth == num3)
				{
					uipanel.depth = num2;
				}
				else
				{
					num3 = uipanel.depth;
					num2 = (uipanel.depth = num2 + 1);
				}
			}
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00005B24 File Offset: 0x00003D24
	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00005B2E File Offset: 0x00003D2E
	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x000219EC File Offset: 0x0001FBEC
	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uiroot = ((!(trans != null)) ? null : NGUITools.FindInParents<UIRoot>(trans.gameObject));
		if (uiroot == null && UIRoot.list.Count > 0)
		{
			uiroot = UIRoot.list[0];
		}
		if (uiroot == null)
		{
			GameObject gameObject = NGUITools.AddChild(null, false);
			uiroot = gameObject.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject.layer = layer;
			if (advanced3D)
			{
				gameObject.name = "UI Root (3D)";
				uiroot.scalingStyle = UIRoot.Scaling.FixedSize;
			}
			else
			{
				gameObject.name = "UI Root";
				uiroot.scalingStyle = UIRoot.Scaling.PixelPerfect;
			}
		}
		UIPanel uipanel = uiroot.GetComponentInChildren<UIPanel>();
		if (uipanel == null)
		{
			Camera[] array = NGUITools.FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uiroot.gameObject.layer;
			foreach (Camera camera in array)
			{
				if (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.depth);
				camera.cullingMask &= ~num2;
			}
			Camera camera2 = NGUITools.AddChild<Camera>(uiroot.gameObject, false);
			camera2.gameObject.AddComponent<UICamera>();
			camera2.clearFlags = ((!flag) ? CameraClearFlags.Color : CameraClearFlags.Depth);
			camera2.backgroundColor = Color.grey;
			camera2.cullingMask = num2;
			camera2.depth = num + 1f;
			if (advanced3D)
			{
				camera2.nearClipPlane = 0.1f;
				camera2.farClipPlane = 4f;
				camera2.transform.localPosition = new Vector3(0f, 0f, -700f);
			}
			else
			{
				camera2.orthographic = true;
				camera2.orthographicSize = 1f;
				camera2.nearClipPlane = -10f;
				camera2.farClipPlane = 10f;
			}
			AudioListener[] array2 = NGUITools.FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.gameObject.AddComponent<AudioListener>();
			}
			uipanel = uiroot.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			trans.parent = uipanel.transform;
			trans.localScale = Vector3.one;
			trans.localPosition = Vector3.zero;
			NGUITools.SetChildLayer(uipanel.cachedTransform, uipanel.cachedGameObject.layer);
		}
		return uipanel;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00021CA0 File Offset: 0x0001FEA0
	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			NGUITools.SetChildLayer(child, layer);
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00021CE0 File Offset: 0x0001FEE0
	public static T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00021D08 File Offset: 0x0001FF08
	public static T AddChild<T>(GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent, undo);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00021D30 File Offset: 0x0001FF30
	public static T AddWidget<T>(GameObject go) where T : UIWidget
	{
		int num = NGUITools.CalculateNextDepth(go);
		T t = NGUITools.AddChild<T>(go);
		t.width = 100;
		t.height = 100;
		t.depth = num;
		t.gameObject.layer = go.layer;
		return t;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00021D90 File Offset: 0x0001FF90
	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UISpriteData uispriteData = ((!(atlas != null)) ? null : atlas.GetSprite(spriteName));
		UISprite uisprite = NGUITools.AddWidget<UISprite>(go);
		uisprite.type = ((uispriteData != null && uispriteData.hasBorder) ? UISprite.Type.Sliced : UISprite.Type.Simple);
		uisprite.atlas = atlas;
		uisprite.spriteName = spriteName;
		return uisprite;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00021DEC File Offset: 0x0001FFEC
	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.transform;
		for (;;)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.gameObject;
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00021E2C File Offset: 0x0002002C
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		object obj = go.GetComponent<T>();
		if (obj == null)
		{
			Transform transform = go.transform.parent;
			while (transform != null && obj == null)
			{
				obj = transform.gameObject.GetComponent<T>();
				transform = transform.parent;
			}
		}
		return (T)((object)obj);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00021EA0 File Offset: 0x000200A0
	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return (T)((object)null);
		}
		object obj = trans.GetComponent<T>();
		if (obj == null)
		{
			Transform transform = trans.transform.parent;
			while (transform != null && obj == null)
			{
				obj = transform.gameObject.GetComponent<T>();
				transform = transform.parent;
			}
		}
		return (T)((object)obj);
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00021F14 File Offset: 0x00020114
	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				if (obj is GameObject)
				{
					GameObject gameObject = obj as GameObject;
					gameObject.transform.parent = null;
				}
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00005B38 File Offset: 0x00003D38
	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00021F68 File Offset: 0x00020168
	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00021FAC File Offset: 0x000201AC
	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00021FF0 File Offset: 0x000201F0
	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00022040 File Offset: 0x00020240
	private static void Activate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, true);
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				return;
			}
			i++;
		}
		int j = 0;
		int childCount2 = t.childCount;
		while (j < childCount2)
		{
			Transform child2 = t.GetChild(j);
			NGUITools.Activate(child2);
			j++;
		}
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00005B61 File Offset: 0x00003D61
	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, false);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00005B6F File Offset: 0x00003D6F
	public static void SetActive(GameObject go, bool state)
	{
		if (go)
		{
			if (state)
			{
				NGUITools.Activate(go.transform);
				NGUITools.CallCreatePanel(go.transform);
			}
			else
			{
				NGUITools.Deactivate(go.transform);
			}
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x000220B8 File Offset: 0x000202B8
	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.CallCreatePanel(t.GetChild(i));
			i++;
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00022104 File Offset: 0x00020304
	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.transform;
		if (state)
		{
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				Transform child = transform.GetChild(i);
				NGUITools.Activate(child);
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount2 = transform.childCount;
			while (j < childCount2)
			{
				Transform child2 = transform.GetChild(j);
				NGUITools.Deactivate(child2);
				j++;
			}
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00005BA8 File Offset: 0x00003DA8
	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00005BA8 File Offset: 0x00003DA8
	public static bool GetActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00005BCF File Offset: 0x00003DCF
	public static bool GetActive(GameObject go)
	{
		return go && go.activeInHierarchy;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00005BE5 File Offset: 0x00003DE5
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0002217C File Offset: 0x0002037C
	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			NGUITools.SetLayer(child.gameObject, layer);
			i++;
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00005BEE File Offset: 0x00003DEE
	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x000221C4 File Offset: 0x000203C4
	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.MakePixelPerfect(t.GetChild(i));
			i++;
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00022254 File Offset: 0x00020454
	public static bool Save(string fileName, byte[] bytes)
	{
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string text = Application.persistentDataPath + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(text);
		}
		catch (Exception ex)
		{
			NGUIDebug.Log(new object[] { ex.Message });
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x000222EC File Offset: 0x000204EC
	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string text = Application.persistentDataPath + "/" + fileName;
		if (File.Exists(text))
		{
			return File.ReadAllBytes(text);
		}
		return null;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0002232C File Offset: 0x0002052C
	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0002238C File Offset: 0x0002058C
	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			componentsInChildren[i].ParentHasChanged();
			i++;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000357 RID: 855 RVA: 0x000223C0 File Offset: 0x000205C0
	// (set) Token: 0x06000358 RID: 856 RVA: 0x000223E8 File Offset: 0x000205E8
	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.text;
		}
		set
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(value);
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00005C2A File Offset: 0x00003E2A
	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor(c);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00005C32 File Offset: 0x00003E32
	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor(text, offset);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00005C3B File Offset: 0x00003E3B
	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00022414 File Offset: 0x00020614
	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00005C43 File Offset: 0x00003E43
	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00005C62 File Offset: 0x00003E62
	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00005C6C File Offset: 0x00003E6C
	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00022444 File Offset: 0x00020644
	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
		NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
		NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
		NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00005C8B File Offset: 0x00003E8B
	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00005CAA File Offset: 0x00003EAA
	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00005CB4 File Offset: 0x00003EB4
	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00022534 File Offset: 0x00020734
	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
		NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
		NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
		NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	// Token: 0x040002C4 RID: 708
	private static AudioListener mListener;

	// Token: 0x040002C5 RID: 709
	private static bool mLoaded = false;

	// Token: 0x040002C6 RID: 710
	private static float mGlobalVolume = 1f;

	// Token: 0x040002C7 RID: 711
	private static Vector3[] mSides = new Vector3[4];
}
