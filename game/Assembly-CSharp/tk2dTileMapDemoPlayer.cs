using System;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class tk2dTileMapDemoPlayer : MonoBehaviour
{
	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06001302 RID: 4866 RVA: 0x000102D0 File Offset: 0x0000E4D0
	private bool AllowAddForce
	{
		get
		{
			return this.forceWait < 0f;
		}
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x00084984 File Offset: 0x00082B84
	private void Awake()
	{
		this.sprite = base.GetComponent<tk2dSprite>();
		if (this.textMesh == null || this.textMesh.transform.parent != base.transform)
		{
			Debug.LogError("Text mesh must be assigned and parented to player.");
			base.enabled = false;
		}
		this.textMeshOffset = this.textMesh.transform.position - base.transform.position;
		this.textMesh.transform.parent = null;
		this.textMeshLabel.text = "instructions";
		this.textMeshLabel.Commit();
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
		{
			this.textMesh.text = "LEFT ARROW / RIGHT ARROW";
		}
		else
		{
			this.textMesh.text = "TAP LEFT / RIGHT SIDE OF SCREEN";
		}
		this.textMesh.Commit();
		Application.targetFrameRate = 60;
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x00084AB4 File Offset: 0x00082CB4
	private void Update()
	{
		this.forceWait -= Time.deltaTime;
		string text = ((!this.AllowAddForce) ? "player_disabled" : "player");
		if (this.sprite.CurrentSprite.name != text)
		{
			this.sprite.SetSprite(text);
		}
		if (this.AllowAddForce)
		{
			float num = 0f;
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				num = 1f;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				num = -1f;
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.touches[i].phase == TouchPhase.Began)
				{
					num = Mathf.Sign(Input.touches[i].position.x - (float)Screen.width * 0.5f);
					break;
				}
			}
			if (num != 0f)
			{
				if (!this.textInitialized)
				{
					this.textMeshLabel.text = "score";
					this.textMeshLabel.Commit();
					this.textMesh.text = "0";
					this.textMesh.Commit();
					this.textInitialized = true;
				}
				this.moveX = num;
			}
		}
		this.textMesh.transform.position = base.transform.position + this.textMeshOffset;
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x00084C34 File Offset: 0x00082E34
	private void FixedUpdate()
	{
		if (this.AllowAddForce && this.moveX != 0f)
		{
			this.forceWait = this.addForceLimit;
			base.rigidbody.AddForce(new Vector3(this.moveX * this.amount, this.amount, 0f) * Time.deltaTime, ForceMode.Impulse);
			base.rigidbody.AddTorque(new Vector3(0f, 0f, -this.moveX * this.torque) * Time.deltaTime, ForceMode.Impulse);
			this.moveX = 0f;
		}
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x000102DF File Offset: 0x0000E4DF
	private void OnTriggerEnter(Collider other)
	{
		UnityEngine.Object.Destroy(other.gameObject);
		this.score++;
		this.textMesh.text = this.score.ToString();
		this.textMesh.Commit();
	}

	// Token: 0x040014CE RID: 5326
	public tk2dTextMesh textMesh;

	// Token: 0x040014CF RID: 5327
	public tk2dTextMesh textMeshLabel;

	// Token: 0x040014D0 RID: 5328
	private Vector3 textMeshOffset;

	// Token: 0x040014D1 RID: 5329
	private bool textInitialized;

	// Token: 0x040014D2 RID: 5330
	public float addForceLimit = 1f;

	// Token: 0x040014D3 RID: 5331
	public float amount = 500f;

	// Token: 0x040014D4 RID: 5332
	public float torque = 50f;

	// Token: 0x040014D5 RID: 5333
	private tk2dSprite sprite;

	// Token: 0x040014D6 RID: 5334
	private int score;

	// Token: 0x040014D7 RID: 5335
	private float forceWait;

	// Token: 0x040014D8 RID: 5336
	private float moveX;
}
