using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class FakeMassActivator : MonoBehaviour
{
	// Token: 0x06000917 RID: 2327 RVA: 0x0003C424 File Offset: 0x0003A624
	private void OnEnable()
	{
		StatueIntroChecker instance = MonoSingleton<StatueIntroChecker>.Instance;
		base.transform.parent.GetComponentInChildren<MassAnimationReceiver>().GetComponent<Animator>().speed = 1f;
		if (instance != null && instance.beenSeen)
		{
			base.transform.parent.GetComponentInChildren<MassAnimationReceiver>().GetComponent<Animator>().Play("Intro", 0, 0.715f);
		}
		if (instance != null)
		{
			instance.beenSeen = true;
		}
		base.enabled = false;
	}
}
