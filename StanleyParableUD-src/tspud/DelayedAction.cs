using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000AD RID: 173
public class DelayedAction : MonoBehaviour
{
	// Token: 0x06000410 RID: 1040 RVA: 0x0001922F File Offset: 0x0001742F
	public void Invoke(float delay)
	{
		base.StartCoroutine(this.WaitSecondsAndDo(delay));
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001923F File Offset: 0x0001743F
	public void Invoke(int frames)
	{
		base.StartCoroutine(this.WaitFramesAndDo(frames));
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001924F File Offset: 0x0001744F
	public void CancelAnyDelayedActions()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00019257 File Offset: 0x00017457
	private IEnumerator WaitSecondsAndDo(float delay)
	{
		yield return new WaitForGameSeconds(delay);
		UnityEvent unityEvent = this.action;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		yield break;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001926D File Offset: 0x0001746D
	private IEnumerator WaitFramesAndDo(int frames)
	{
		int num;
		for (int i = 0; i < frames; i = num + 1)
		{
			if (Singleton<GameMaster>.Instance.GameDeltaTime != 0f)
			{
				yield return null;
			}
			num = i;
		}
		UnityEvent unityEvent = this.action;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		yield break;
	}

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	private UnityEvent action;
}
