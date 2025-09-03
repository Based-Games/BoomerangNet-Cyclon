using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class ControlCoolBombScript : MonoBehaviour
{
	// Token: 0x06000876 RID: 2166 RVA: 0x00040F7C File Offset: 0x0003F17C
	private void Awake()
	{
		for (int i = 0; i < 12; i++)
		{
			this.AllTrackNormal[i] = base.transform.FindChild("CoolBomb" + i.ToString()).GetComponent<tk2dSpriteAnimator>();
			this.AllTrackNormal[i].Stop();
			this.AllTrackNormal[i].gameObject.SetActive(false);
			this.AllTrackExtreme[i] = base.transform.FindChild("ExtremeCoolBomb" + i.ToString()).GetComponent<tk2dSpriteAnimator>();
			this.AllTrackExtreme[i].Stop();
			this.AllTrackExtreme[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00009359 File Offset: 0x00007559
	public void SetGameManager(GameManagerScript sManager)
	{
		this.m_sGameManager = sManager;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00009362 File Offset: 0x00007562
	private void AnimationComplete(tk2dSpriteAnimator tSame, tk2dSpriteAnimationClip tClip)
	{
		tSame.gameObject.SetActive(false);
		tSame.AnimationCompleted = null;
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00041030 File Offset: 0x0003F230
	public void EffectNormalCoolBomb(ScoreEventBase pEvt, bool bOnExtreme, Vector3 vPos, Vector3 vRot)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[pEvt.Track];
		tk2dSpriteAnimator.gameObject.SetActive(true);
		string text = "SinglePerfect";
		if (pEvt.JudgmentEnd == JUDGMENT_TYPE.PERFECT)
		{
			text = "SinglePerfect";
		}
		else if (pEvt.JudgmentEnd == JUDGMENT_TYPE.GREAT || pEvt.JudgmentEnd == JUDGMENT_TYPE.GOOD)
		{
			text = "SingleGG";
		}
		else if (pEvt.JudgmentEnd == JUDGMENT_TYPE.POOR)
		{
			text = "SinglePoor";
		}
		tk2dSpriteAnimator.transform.position = vPos;
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.PlayFrom(text, 0f);
		tk2dSpriteAnimator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationComplete);
		if (bOnExtreme)
		{
			tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[pEvt.Track];
			tk2dSpriteAnimator2.gameObject.SetActive(true);
			tk2dSpriteAnimator2.transform.position = vPos;
			tk2dSpriteAnimator2.Stop();
			tk2dSpriteAnimator2.PlayFrom("ExtremeSingle", 0f);
			tk2dSpriteAnimator2.transform.localEulerAngles = vRot;
			tk2dSpriteAnimator2.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationComplete);
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0004113C File Offset: 0x0003F33C
	public void EffectSwingCoolBomb(ScoreEventBase pEvt, Vector3 vPos, bool bOnExtreme, bool bUp)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[pEvt.Track];
		tk2dSpriteAnimator.gameObject.SetActive(true);
		tk2dSpriteAnimator.transform.position = vPos;
		Vector3 vector = GameData.MAXGUIDE[pEvt.Track];
		vector.z -= 90f;
		tk2dSpriteAnimator.GetComponent<tk2dSprite>().FlipX = false;
		if (bUp)
		{
			vector.z += 180f;
			tk2dSpriteAnimator.GetComponent<tk2dSprite>().FlipX = true;
		}
		string text = "Swing";
		tk2dSpriteAnimator.transform.localEulerAngles = vector;
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.PlayFrom(text, 0f);
		tk2dSpriteAnimator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationComplete);
		if (bOnExtreme)
		{
			tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[pEvt.Track];
			tk2dSpriteAnimator2.GetComponent<tk2dSprite>().FlipX = false;
			tk2dSpriteAnimator2.gameObject.SetActive(true);
			tk2dSpriteAnimator2.transform.position = vPos;
			vector = GameData.MAXGUIDE[pEvt.Track];
			tk2dSpriteAnimator2.transform.localEulerAngles = vector;
			tk2dSpriteAnimator2.Stop();
			tk2dSpriteAnimator2.PlayFrom("ExtremeSingle", 0f);
			tk2dSpriteAnimator2.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationComplete);
			if (bUp)
			{
				tk2dSpriteAnimator2.GetComponent<tk2dSprite>().FlipX = true;
			}
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00041298 File Offset: 0x0003F498
	public void EffectLongEndCoolBomb(int iTrack, bool bOnExtreme)
	{
		Vector3 effectPos = this.m_sGameManager.GetEffectPos(iTrack);
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		tk2dSpriteAnimator.gameObject.SetActive(true);
		effectPos.z = -0.1f;
		tk2dSpriteAnimator.transform.position = effectPos;
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.PlayFrom("LongEnd", 0f);
		tk2dSpriteAnimator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationComplete);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00041308 File Offset: 0x0003F508
	public void EffectLongLineCoolBomb(int iTrack, Vector3 vRot, bool bOnExtreme)
	{
		Vector3 effectPos = this.m_sGameManager.GetEffectPos(iTrack);
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		tk2dSpriteAnimator.gameObject.SetActive(true);
		tk2dSpriteAnimator.AnimationCompleted = null;
		effectPos.z = -0.1f;
		tk2dSpriteAnimator.transform.position = effectPos;
		tk2dSpriteAnimator.UpdateAnimation(5f);
		tk2dSpriteAnimator.Stop();
		string text = "LongLoop";
		tk2dSpriteAnimator.PlayFrom(text, 0f);
		if (bOnExtreme)
		{
			tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[iTrack];
			tk2dSpriteAnimator2.AnimationCompleted = null;
			tk2dSpriteAnimator2.gameObject.SetActive(true);
			tk2dSpriteAnimator2.transform.position = effectPos;
			tk2dSpriteAnimator2.Stop();
			tk2dSpriteAnimator2.transform.localEulerAngles = vRot;
			tk2dSpriteAnimator2.PlayFrom("ExtremeLong", 0f);
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x000413CC File Offset: 0x0003F5CC
	public void StopLongLineCoolBomb(int iTrack)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.gameObject.SetActive(false);
		tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[iTrack];
		tk2dSpriteAnimator2.Stop();
		tk2dSpriteAnimator2.gameObject.SetActive(false);
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00041410 File Offset: 0x0003F610
	public void MoveLongCoolBomb(int iTrack, Vector3 vPos, bool bOnExtreme, Vector3 vRot)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		vPos.z = -0.1f;
		tk2dSpriteAnimator.transform.position = vPos;
		tk2dSpriteAnimator.transform.localEulerAngles = vRot;
		tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[iTrack];
		tk2dSpriteAnimator2.transform.position = vPos;
		tk2dSpriteAnimator2.transform.localEulerAngles = vRot;
		if (!bOnExtreme)
		{
			tk2dSpriteAnimator2.gameObject.SetActive(false);
			tk2dSpriteAnimator2.Stop();
		}
		else
		{
			tk2dSpriteAnimator2.gameObject.SetActive(true);
			tk2dSpriteAnimator2.PlayFrom("ExtremeLong", 0f);
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x000414A8 File Offset: 0x0003F6A8
	public void EffectJogEndCoolBomb(int iTrack, Vector3 vPos, bool bOnExtreme)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		tk2dSpriteAnimator.gameObject.SetActive(true);
		vPos.z = -0.1f;
		tk2dSpriteAnimator.transform.position = vPos;
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.PlayFrom("JogEnd", 0f);
		tk2dSpriteAnimator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationComplete);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0004150C File Offset: 0x0003F70C
	public void EffectJogLineCoolBomb(int iTrack, Vector3 vRot, bool bOnExtreme)
	{
		Vector3 effectPos = this.m_sGameManager.GetEffectPos(iTrack);
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		tk2dSpriteAnimator.gameObject.SetActive(true);
		effectPos.z = -0.1f;
		tk2dSpriteAnimator.transform.position = effectPos;
		tk2dSpriteAnimator.AnimationCompleted = null;
		string text = "JogLoop";
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.PlayFrom(text, 0f);
		tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[iTrack];
		tk2dSpriteAnimator2.transform.position = effectPos;
		tk2dSpriteAnimator2.transform.localEulerAngles = vRot;
		if (bOnExtreme)
		{
			tk2dSpriteAnimator2.gameObject.SetActive(true);
			tk2dSpriteAnimator2.Play(text);
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00041410 File Offset: 0x0003F610
	public void MoveJogCoolBomb(int iTrack, Vector3 vPos, bool bOnExtreme, Vector3 vRot)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		vPos.z = -0.1f;
		tk2dSpriteAnimator.transform.position = vPos;
		tk2dSpriteAnimator.transform.localEulerAngles = vRot;
		tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[iTrack];
		tk2dSpriteAnimator2.transform.position = vPos;
		tk2dSpriteAnimator2.transform.localEulerAngles = vRot;
		if (!bOnExtreme)
		{
			tk2dSpriteAnimator2.gameObject.SetActive(false);
			tk2dSpriteAnimator2.Stop();
		}
		else
		{
			tk2dSpriteAnimator2.gameObject.SetActive(true);
			tk2dSpriteAnimator2.PlayFrom("ExtremeLong", 0f);
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x000413CC File Offset: 0x0003F5CC
	public void StopJogCoolBomb(int iTrack)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.AllTrackNormal[iTrack];
		tk2dSpriteAnimator.Stop();
		tk2dSpriteAnimator.gameObject.SetActive(false);
		tk2dSpriteAnimator tk2dSpriteAnimator2 = this.AllTrackExtreme[iTrack];
		tk2dSpriteAnimator2.Stop();
		tk2dSpriteAnimator2.gameObject.SetActive(false);
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x040006EB RID: 1771
	private const string END_ANI = "End";

	// Token: 0x040006EC RID: 1772
	private GameManagerScript m_sGameManager;

	// Token: 0x040006ED RID: 1773
	private tk2dSpriteAnimator[] AllTrackNormal = new tk2dSpriteAnimator[12];

	// Token: 0x040006EE RID: 1774
	private tk2dSpriteAnimator[] AllTrackExtreme = new tk2dSpriteAnimator[12];
}
