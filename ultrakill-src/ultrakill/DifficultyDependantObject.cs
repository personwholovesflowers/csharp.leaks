using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200010E RID: 270
public class DifficultyDependantObject : MonoBehaviour
{
	// Token: 0x06000511 RID: 1297 RVA: 0x000220C8 File Offset: 0x000202C8
	private void Awake()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		bool flag = false;
		switch (@int)
		{
		case 0:
			if (this.veryEasy)
			{
				flag = true;
			}
			break;
		case 1:
			if (this.easy)
			{
				flag = true;
			}
			break;
		case 2:
			if (this.normal)
			{
				flag = true;
			}
			break;
		case 3:
			if (this.hard)
			{
				flag = true;
			}
			break;
		case 4:
			if (this.veryHard)
			{
				flag = true;
			}
			break;
		case 5:
			if (this.UKMD)
			{
				flag = true;
			}
			break;
		}
		if (!flag)
		{
			UnityEvent unityEvent = this.onWrongDifficulty;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			if (this.autoDeactivate)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		UnityEvent unityEvent2 = this.onRightDifficulty;
		if (unityEvent2 == null)
		{
			return;
		}
		unityEvent2.Invoke();
	}

	// Token: 0x040006F7 RID: 1783
	public bool autoDeactivate = true;

	// Token: 0x040006F8 RID: 1784
	[Header("Active in difficulties:")]
	public bool veryEasy = true;

	// Token: 0x040006F9 RID: 1785
	public bool easy = true;

	// Token: 0x040006FA RID: 1786
	public bool normal = true;

	// Token: 0x040006FB RID: 1787
	public bool hard = true;

	// Token: 0x040006FC RID: 1788
	public bool veryHard = true;

	// Token: 0x040006FD RID: 1789
	public bool UKMD = true;

	// Token: 0x040006FE RID: 1790
	[Header("Optional events: ")]
	public UnityEvent onRightDifficulty;

	// Token: 0x040006FF RID: 1791
	public UnityEvent onWrongDifficulty;
}
