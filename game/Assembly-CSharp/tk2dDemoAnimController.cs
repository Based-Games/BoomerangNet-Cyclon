using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002CC RID: 716
[AddComponentMenu("2D Toolkit/Demo/tk2dDemoAnimController")]
public class tk2dDemoAnimController : MonoBehaviour
{
	// Token: 0x06001507 RID: 5383 RVA: 0x0008C80C File Offset: 0x0008AA0C
	private void Start()
	{
		this.animator = base.GetComponent<tk2dSpriteAnimator>();
		tk2dSpriteAnimator tk2dSpriteAnimator = this.animator;
		tk2dSpriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(tk2dSpriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventHandler));
		this.popupTextMesh.gameObject.SetActive(false);
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x0008C860 File Offset: 0x0008AA60
	private void AnimationEventHandler(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNum)
	{
		string text = string.Concat(new string[]
		{
			animator.name,
			"\n",
			clip.name,
			"\nINFO: ",
			clip.GetFrame(frameNum).eventInfo
		});
		base.StartCoroutine(this.PopupText(text));
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x0008C8B8 File Offset: 0x0008AAB8
	private IEnumerator PopupText(string text)
	{
		this.popupTextMesh.text = text;
		this.popupTextMesh.Commit();
		this.popupTextMesh.gameObject.SetActive(true);
		float fadeTime = 1f;
		Color c = this.popupTextMesh.color;
		Color c2 = this.popupTextMesh.color2;
		for (float f = 0f; f < fadeTime; f += Time.deltaTime)
		{
			float alpha = Mathf.Clamp01(2f * (1f - f / fadeTime));
			c.a = alpha;
			c2.a = alpha;
			this.popupTextMesh.color = c;
			this.popupTextMesh.color2 = c2;
			this.popupTextMesh.Commit();
			yield return 0;
		}
		this.popupTextMesh.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x0008C8E4 File Offset: 0x0008AAE4
	private void OnGUI()
	{
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Animation wrap modes", new GUILayoutOption[0]);
		if (GUILayout.Button("Loop", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_loop");
		}
		GUILayout.Label("  This animation will play indefinitely", new GUILayoutOption[0]);
		if (GUILayout.Button("LoopSection", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_loopsection");
		}
		GUILayout.Label("  This animation has been set up to loop from frame 3.\nIt will play 0 1 2 3 4 2 3 4 2 3 4 indefinitely", new GUILayoutOption[0]);
		if (GUILayout.Button("Once", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_once");
		}
		GUILayout.Label("  This animation will play once and stop at the last frame", new GUILayoutOption[0]);
		if (GUILayout.Button("Ping pong", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_pingpong");
		}
		GUILayout.Label("  This animation will play once forward, and then reverse, repeating indefinitely", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("Single", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_single1");
		}
		if (GUILayout.Button("Single", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_single2");
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("  Use this for non-animated states and placeholders.", new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label("Animation delegate example", new GUILayoutOption[0]);
		if (GUILayout.Button("Delegate", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_once");
			this.animator.AnimationCompleted = delegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
			{
				Debug.Log("Delegate");
				this.animator.Play("demo_pingpong");
				this.animator.AnimationCompleted = null;
			};
		}
		GUILayout.Label("Play demo_once, then immediately play demo_pingpong after that", new GUILayoutOption[0]);
		if (GUILayout.Button("Message", new GUILayoutOption[] { GUILayout.MaxWidth(100f) }))
		{
			this.animator.Play("demo_message");
		}
		GUILayout.Label("Plays demo_message once, will trigger an event when frame 3 is hit.\nLook at how this animation is set up.", new GUILayoutOption[0]);
		GUILayout.EndVertical();
	}

	// Token: 0x0400166F RID: 5743
	private tk2dSpriteAnimator animator;

	// Token: 0x04001670 RID: 5744
	public tk2dTextMesh popupTextMesh;
}
