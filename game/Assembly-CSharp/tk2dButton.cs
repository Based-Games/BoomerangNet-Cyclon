using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000243 RID: 579
[AddComponentMenu("2D Toolkit/Deprecated/GUI/tk2dButton")]
public class tk2dButton : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06001098 RID: 4248 RVA: 0x0000E324 File Offset: 0x0000C524
	// (remove) Token: 0x06001099 RID: 4249 RVA: 0x0000E33D File Offset: 0x0000C53D
	public event tk2dButton.ButtonHandlerDelegate ButtonPressedEvent;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600109A RID: 4250 RVA: 0x0000E356 File Offset: 0x0000C556
	// (remove) Token: 0x0600109B RID: 4251 RVA: 0x0000E36F File Offset: 0x0000C56F
	public event tk2dButton.ButtonHandlerDelegate ButtonAutoFireEvent;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600109C RID: 4252 RVA: 0x0000E388 File Offset: 0x0000C588
	// (remove) Token: 0x0600109D RID: 4253 RVA: 0x0000E3A1 File Offset: 0x0000C5A1
	public event tk2dButton.ButtonHandlerDelegate ButtonDownEvent;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600109E RID: 4254 RVA: 0x0000E3BA File Offset: 0x0000C5BA
	// (remove) Token: 0x0600109F RID: 4255 RVA: 0x0000E3D3 File Offset: 0x0000C5D3
	public event tk2dButton.ButtonHandlerDelegate ButtonUpEvent;

	// Token: 0x060010A0 RID: 4256 RVA: 0x0000E3EC File Offset: 0x0000C5EC
	private void OnEnable()
	{
		this.buttonDown = false;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x00076EA8 File Offset: 0x000750A8
	private void Start()
	{
		if (this.viewCamera == null)
		{
			Transform transform = base.transform;
			while (transform && transform.camera == null)
			{
				transform = transform.parent;
			}
			if (transform && transform.camera != null)
			{
				this.viewCamera = transform.camera;
			}
			if (this.viewCamera == null && tk2dCamera.Instance)
			{
				this.viewCamera = tk2dCamera.Instance.camera;
			}
			if (this.viewCamera == null)
			{
				this.viewCamera = Camera.main;
			}
		}
		this.sprite = base.GetComponent<tk2dBaseSprite>();
		if (this.sprite)
		{
			this.UpdateSpriteIds();
		}
		if (base.collider == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			Vector3 size = boxCollider.size;
			size.z = 0.2f;
			boxCollider.size = size;
		}
		if ((this.buttonDownSound != null || this.buttonPressedSound != null || this.buttonUpSound != null) && base.audio == null)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x0007701C File Offset: 0x0007521C
	public void UpdateSpriteIds()
	{
		this.buttonDownSpriteId = ((this.buttonDownSprite.Length <= 0) ? (-1) : this.sprite.GetSpriteIdByName(this.buttonDownSprite));
		this.buttonUpSpriteId = ((this.buttonUpSprite.Length <= 0) ? (-1) : this.sprite.GetSpriteIdByName(this.buttonUpSprite));
		this.buttonPressedSpriteId = ((this.buttonPressedSprite.Length <= 0) ? (-1) : this.sprite.GetSpriteIdByName(this.buttonPressedSprite));
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0000E3F5 File Offset: 0x0000C5F5
	private void PlaySound(AudioClip source)
	{
		if (base.audio && source)
		{
			base.audio.PlayOneShot(source);
		}
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x000770B4 File Offset: 0x000752B4
	private IEnumerator coScale(Vector3 defaultScale, float startScale, float endScale)
	{
		float t0 = Time.realtimeSinceStartup;
		Vector3 scale = defaultScale;
		for (float s = 0f; s < this.scaleTime; s = Time.realtimeSinceStartup - t0)
		{
			float t = Mathf.Clamp01(s / this.scaleTime);
			float scl = Mathf.Lerp(startScale, endScale, t);
			scale = defaultScale * scl;
			base.transform.localScale = scale;
			yield return 0;
		}
		base.transform.localScale = defaultScale * endScale;
		yield break;
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x000770FC File Offset: 0x000752FC
	private IEnumerator LocalWaitForSeconds(float seconds)
	{
		float t0 = Time.realtimeSinceStartup;
		for (float s = 0f; s < seconds; s = Time.realtimeSinceStartup - t0)
		{
			yield return 0;
		}
		yield break;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x00077120 File Offset: 0x00075320
	private IEnumerator coHandleButtonPress(int fingerId)
	{
		this.buttonDown = true;
		bool buttonPressed = true;
		Vector3 defaultScale = base.transform.localScale;
		if (this.targetScale != 1f)
		{
			yield return base.StartCoroutine(this.coScale(defaultScale, 1f, this.targetScale));
		}
		this.PlaySound(this.buttonDownSound);
		if (this.buttonDownSpriteId != -1)
		{
			this.sprite.spriteId = this.buttonDownSpriteId;
		}
		if (this.ButtonDownEvent != null)
		{
			this.ButtonDownEvent(this);
		}
		for (;;)
		{
			Vector3 cursorPosition = Vector3.zero;
			bool cursorActive = true;
			if (fingerId != -1)
			{
				bool found = false;
				for (int i = 0; i < Input.touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					if (touch.fingerId == fingerId)
					{
						if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
						{
							break;
						}
						cursorPosition = touch.position;
						found = true;
					}
				}
				if (!found)
				{
					cursorActive = false;
				}
			}
			else
			{
				if (!Input.GetMouseButton(0))
				{
					cursorActive = false;
				}
				cursorPosition = Input.mousePosition;
			}
			if (!cursorActive)
			{
				break;
			}
			Ray ray = this.viewCamera.ScreenPointToRay(cursorPosition);
			RaycastHit hitInfo;
			bool colliderHit = base.collider.Raycast(ray, out hitInfo, float.PositiveInfinity);
			if (buttonPressed && !colliderHit)
			{
				if (this.targetScale != 1f)
				{
					yield return base.StartCoroutine(this.coScale(defaultScale, this.targetScale, 1f));
				}
				this.PlaySound(this.buttonUpSound);
				if (this.buttonUpSpriteId != -1)
				{
					this.sprite.spriteId = this.buttonUpSpriteId;
				}
				if (this.ButtonUpEvent != null)
				{
					this.ButtonUpEvent(this);
				}
				buttonPressed = false;
			}
			else if (!buttonPressed && colliderHit)
			{
				if (this.targetScale != 1f)
				{
					yield return base.StartCoroutine(this.coScale(defaultScale, 1f, this.targetScale));
				}
				this.PlaySound(this.buttonDownSound);
				if (this.buttonDownSpriteId != -1)
				{
					this.sprite.spriteId = this.buttonDownSpriteId;
				}
				if (this.ButtonDownEvent != null)
				{
					this.ButtonDownEvent(this);
				}
				buttonPressed = true;
			}
			if (buttonPressed && this.ButtonAutoFireEvent != null)
			{
				this.ButtonAutoFireEvent(this);
			}
			yield return 0;
		}
		if (buttonPressed)
		{
			if (this.targetScale != 1f)
			{
				yield return base.StartCoroutine(this.coScale(defaultScale, this.targetScale, 1f));
			}
			this.PlaySound(this.buttonPressedSound);
			if (this.buttonPressedSpriteId != -1)
			{
				this.sprite.spriteId = this.buttonPressedSpriteId;
			}
			if (this.targetObject)
			{
				this.targetObject.SendMessage(this.messageName);
			}
			if (this.ButtonUpEvent != null)
			{
				this.ButtonUpEvent(this);
			}
			if (this.ButtonPressedEvent != null)
			{
				this.ButtonPressedEvent(this);
			}
			if (base.gameObject.activeInHierarchy)
			{
				yield return base.StartCoroutine(this.LocalWaitForSeconds(this.pressedWaitTime));
			}
			if (this.buttonUpSpriteId != -1)
			{
				this.sprite.spriteId = this.buttonUpSpriteId;
			}
		}
		this.buttonDown = false;
		yield break;
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0007714C File Offset: 0x0007534C
	private void Update()
	{
		if (this.buttonDown)
		{
			return;
		}
		bool flag = false;
		if (Input.multiTouchEnabled)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					Ray ray = this.viewCamera.ScreenPointToRay(touch.position);
					RaycastHit raycastHit;
					if (base.collider.Raycast(ray, out raycastHit, 100000000f) && !Physics.Raycast(ray, raycastHit.distance - 0.01f))
					{
						base.StartCoroutine(this.coHandleButtonPress(touch.fingerId));
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag && Input.GetMouseButtonDown(0))
		{
			Ray ray2 = this.viewCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit2;
			if (base.collider.Raycast(ray2, out raycastHit2, 100000000f) && !Physics.Raycast(ray2, raycastHit2.distance - 0.01f))
			{
				base.StartCoroutine(this.coHandleButtonPress(-1));
			}
		}
	}

	// Token: 0x04001264 RID: 4708
	public Camera viewCamera;

	// Token: 0x04001265 RID: 4709
	public string buttonDownSprite = "button_down";

	// Token: 0x04001266 RID: 4710
	public string buttonUpSprite = "button_up";

	// Token: 0x04001267 RID: 4711
	public string buttonPressedSprite = "button_up";

	// Token: 0x04001268 RID: 4712
	private int buttonDownSpriteId = -1;

	// Token: 0x04001269 RID: 4713
	private int buttonUpSpriteId = -1;

	// Token: 0x0400126A RID: 4714
	private int buttonPressedSpriteId = -1;

	// Token: 0x0400126B RID: 4715
	public AudioClip buttonDownSound;

	// Token: 0x0400126C RID: 4716
	public AudioClip buttonUpSound;

	// Token: 0x0400126D RID: 4717
	public AudioClip buttonPressedSound;

	// Token: 0x0400126E RID: 4718
	public GameObject targetObject;

	// Token: 0x0400126F RID: 4719
	public string messageName = string.Empty;

	// Token: 0x04001270 RID: 4720
	private tk2dBaseSprite sprite;

	// Token: 0x04001271 RID: 4721
	private bool buttonDown;

	// Token: 0x04001272 RID: 4722
	public float targetScale = 1.1f;

	// Token: 0x04001273 RID: 4723
	public float scaleTime = 0.05f;

	// Token: 0x04001274 RID: 4724
	public float pressedWaitTime = 0.3f;

	// Token: 0x02000244 RID: 580
	// (Invoke) Token: 0x060010A9 RID: 4265
	public delegate void ButtonHandlerDelegate(tk2dButton source);
}
