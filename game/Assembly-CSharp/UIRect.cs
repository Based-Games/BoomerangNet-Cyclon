using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
public abstract class UIRect : MonoBehaviour
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060003D3 RID: 979 RVA: 0x000061BB File Offset: 0x000043BB
	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060003D4 RID: 980 RVA: 0x000061E0 File Offset: 0x000043E0
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060003D5 RID: 981 RVA: 0x00006205 File Offset: 0x00004405
	public Camera anchorCamera
	{
		get
		{
			if (!this.mAnchorsCached)
			{
				this.ResetAnchors();
			}
			return this.mMyCam;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000621E File Offset: 0x0000441E
	public UIRect parent
	{
		get
		{
			if (!this.mParentFound)
			{
				this.mParentFound = true;
				this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
			}
			return this.mParent;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x00023A38 File Offset: 0x00021C38
	public UIRoot root
	{
		get
		{
			if (this.parent != null)
			{
				return this.mParent.root;
			}
			if (!this.mRootSet)
			{
				this.mRootSet = true;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
			}
			return this.mRoot;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060003D8 RID: 984 RVA: 0x00023A8C File Offset: 0x00021C8C
	public bool isAnchored
	{
		get
		{
			return this.leftAnchor.target || this.rightAnchor.target || this.topAnchor.target || this.bottomAnchor.target;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060003D9 RID: 985
	// (set) Token: 0x060003DA RID: 986
	public abstract float alpha { get; set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060003DB RID: 987
	public abstract float finalAlpha { get; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060003DC RID: 988
	public abstract Vector3[] localCorners { get; }

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060003DD RID: 989
	public abstract Vector3[] worldCorners { get; }

	// Token: 0x060003DE RID: 990 RVA: 0x00023AEC File Offset: 0x00021CEC
	public void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		if (includeChildren)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				this.mChildren.buffer[i].Invalidate(true);
			}
		}
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00023B38 File Offset: 0x00021D38
	public virtual Vector3[] GetSides(Transform relativeTo)
	{
		if (this.anchorCamera != null)
		{
			return this.anchorCamera.GetSides(relativeTo);
		}
		Vector3 position = this.cachedTransform.position;
		for (int i = 0; i < 4; i++)
		{
			UIRect.mSides[i] = position;
		}
		if (relativeTo != null)
		{
			for (int j = 0; j < 4; j++)
			{
				UIRect.mSides[j] = relativeTo.InverseTransformPoint(UIRect.mSides[j]);
			}
		}
		return UIRect.mSides;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00023BDC File Offset: 0x00021DDC
	protected Vector3 GetLocalPos(UIRect.AnchorPoint ac, Transform trans)
	{
		if (this.anchorCamera == null || ac.targetCam == null)
		{
			return this.cachedTransform.localPosition;
		}
		Vector3 vector = this.mMyCam.ViewportToWorldPoint(ac.targetCam.WorldToViewportPoint(ac.target.position));
		if (trans != null)
		{
			vector = trans.InverseTransformPoint(vector);
		}
		vector.x = Mathf.Floor(vector.x + 0.5f);
		vector.y = Mathf.Floor(vector.y + 0.5f);
		return vector;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0000624E File Offset: 0x0000444E
	protected virtual void OnEnable()
	{
		this.mChanged = true;
		this.mRootSet = false;
		this.mParentFound = false;
		if (this.parent != null)
		{
			this.mParent.mChildren.Add(this);
		}
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00006287 File Offset: 0x00004487
	protected virtual void OnDisable()
	{
		if (this.mParent)
		{
			this.mParent.mChildren.Remove(this);
		}
		this.mParent = null;
		this.mRoot = null;
		this.mRootSet = false;
		this.mParentFound = false;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x000062C7 File Offset: 0x000044C7
	protected void Start()
	{
		this.OnStart();
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00023C80 File Offset: 0x00021E80
	public void Update()
	{
		if (!this.mAnchorsCached)
		{
			this.ResetAnchors();
		}
		int frameCount = Time.frameCount;
		if (this.mUpdateFrame != frameCount)
		{
			this.mUpdateFrame = frameCount;
			bool flag = false;
			if (this.leftAnchor.target)
			{
				flag = true;
				if (this.leftAnchor.rect != null && this.leftAnchor.rect.mUpdateFrame != frameCount)
				{
					this.leftAnchor.rect.Update();
				}
			}
			if (this.bottomAnchor.target)
			{
				flag = true;
				if (this.bottomAnchor.rect != null && this.bottomAnchor.rect.mUpdateFrame != frameCount)
				{
					this.bottomAnchor.rect.Update();
				}
			}
			if (this.rightAnchor.target)
			{
				flag = true;
				if (this.rightAnchor.rect != null && this.rightAnchor.rect.mUpdateFrame != frameCount)
				{
					this.rightAnchor.rect.Update();
				}
			}
			if (this.topAnchor.target)
			{
				flag = true;
				if (this.topAnchor.rect != null && this.topAnchor.rect.mUpdateFrame != frameCount)
				{
					this.topAnchor.rect.Update();
				}
			}
			if (flag)
			{
				this.OnAnchor();
			}
			this.OnUpdate();
		}
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x000062CF File Offset: 0x000044CF
	public void UpdateAnchors()
	{
		if (this.isAnchored)
		{
			this.OnAnchor();
		}
	}

	// Token: 0x060003E6 RID: 998
	protected abstract void OnAnchor();

	// Token: 0x060003E7 RID: 999 RVA: 0x00023E18 File Offset: 0x00022018
	public void ResetAnchors()
	{
		this.mAnchorsCached = true;
		this.leftAnchor.rect = ((!this.leftAnchor.target) ? null : this.leftAnchor.target.GetComponent<UIRect>());
		this.bottomAnchor.rect = ((!this.bottomAnchor.target) ? null : this.bottomAnchor.target.GetComponent<UIRect>());
		this.rightAnchor.rect = ((!this.rightAnchor.target) ? null : this.rightAnchor.target.GetComponent<UIRect>());
		this.topAnchor.rect = ((!this.topAnchor.target) ? null : this.topAnchor.target.GetComponent<UIRect>());
		this.mMyCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
		this.FindCameraFor(this.leftAnchor);
		this.FindCameraFor(this.bottomAnchor);
		this.FindCameraFor(this.rightAnchor);
		this.FindCameraFor(this.topAnchor);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00023F4C File Offset: 0x0002214C
	private void FindCameraFor(UIRect.AnchorPoint ap)
	{
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
		}
		else
		{
			ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
			if (ap.targetCam == null)
			{
				ap.target = null;
				return;
			}
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x00023FBC File Offset: 0x000221BC
	public virtual void ParentHasChanged()
	{
		UIRect uirect = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
		if (this.mParent != uirect)
		{
			if (this.mParent)
			{
				this.mParent.mChildren.Remove(this);
			}
			this.mParent = uirect;
			if (this.mParent)
			{
				this.mParent.mChildren.Add(this);
			}
			this.mRootSet = false;
		}
	}

	// Token: 0x060003EA RID: 1002
	protected abstract void OnStart();

	// Token: 0x060003EB RID: 1003 RVA: 0x00003648 File Offset: 0x00001848
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x04000304 RID: 772
	public UIRect.AnchorPoint leftAnchor = new UIRect.AnchorPoint();

	// Token: 0x04000305 RID: 773
	public UIRect.AnchorPoint rightAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04000306 RID: 774
	public UIRect.AnchorPoint bottomAnchor = new UIRect.AnchorPoint();

	// Token: 0x04000307 RID: 775
	public UIRect.AnchorPoint topAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04000308 RID: 776
	protected GameObject mGo;

	// Token: 0x04000309 RID: 777
	protected Transform mTrans;

	// Token: 0x0400030A RID: 778
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();

	// Token: 0x0400030B RID: 779
	protected bool mChanged = true;

	// Token: 0x0400030C RID: 780
	protected float mFinalAlpha;

	// Token: 0x0400030D RID: 781
	private UIRoot mRoot;

	// Token: 0x0400030E RID: 782
	private UIRect mParent;

	// Token: 0x0400030F RID: 783
	private Camera mMyCam;

	// Token: 0x04000310 RID: 784
	private int mUpdateFrame = -1;

	// Token: 0x04000311 RID: 785
	private bool mAnchorsCached;

	// Token: 0x04000312 RID: 786
	private bool mParentFound;

	// Token: 0x04000313 RID: 787
	private bool mRootSet;

	// Token: 0x04000314 RID: 788
	private static Vector3[] mSides = new Vector3[4];

	// Token: 0x02000086 RID: 134
	[Serializable]
	public class AnchorPoint
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x00003B98 File Offset: 0x00001D98
		public AnchorPoint()
		{
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000062E2 File Offset: 0x000044E2
		public AnchorPoint(float relative)
		{
			this.relative = relative;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000062F1 File Offset: 0x000044F1
		public void Set(float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000630C File Offset: 0x0000450C
		public void SetToNearest(float abs0, float abs1, float abs2)
		{
			this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0002403C File Offset: 0x0002223C
		public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float num = Mathf.Abs(abs0);
			float num2 = Mathf.Abs(abs1);
			float num3 = Mathf.Abs(abs2);
			if (num < num2 && num < num3)
			{
				this.Set(rel0, abs0);
			}
			else if (num2 < num && num2 < num3)
			{
				this.Set(rel1, abs1);
			}
			else
			{
				this.Set(rel2, abs2);
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000240A4 File Offset: 0x000222A4
		public void SetHorizontal(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 vector = this.target.position;
				if (parent != null)
				{
					vector = parent.InverseTransformPoint(vector);
				}
				this.absolute = Mathf.FloorToInt(localPos - vector.x + 0.5f);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00024148 File Offset: 0x00022348
		public void SetVertical(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 vector = this.target.position;
				if (parent != null)
				{
					vector = parent.InverseTransformPoint(vector);
				}
				this.absolute = Mathf.FloorToInt(localPos - vector.y + 0.5f);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000241EC File Offset: 0x000223EC
		public Vector3[] GetSides(Transform relativeTo)
		{
			if (this.target != null)
			{
				if (this.rect != null)
				{
					return this.rect.GetSides(relativeTo);
				}
				if (this.target.camera != null)
				{
					return this.target.camera.GetSides(relativeTo);
				}
			}
			return null;
		}

		// Token: 0x04000315 RID: 789
		public Transform target;

		// Token: 0x04000316 RID: 790
		public float relative;

		// Token: 0x04000317 RID: 791
		public int absolute;

		// Token: 0x04000318 RID: 792
		[NonSerialized]
		public UIRect rect;

		// Token: 0x04000319 RID: 793
		[NonSerialized]
		public Camera targetCam;
	}
}
