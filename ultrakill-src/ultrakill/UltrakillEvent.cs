using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200048D RID: 1165
[Serializable]
public class UltrakillEvent
{
	// Token: 0x06001ACA RID: 6858 RVA: 0x000DC924 File Offset: 0x000DAB24
	public void Invoke(string name = "")
	{
		if (this.toDisActivateObjects != null)
		{
			foreach (GameObject gameObject in this.toDisActivateObjects)
			{
				if (gameObject)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (this.toActivateObjects != null)
		{
			foreach (GameObject gameObject2 in this.toActivateObjects)
			{
				if (gameObject2)
				{
					gameObject2.SetActive(true);
				}
			}
		}
		UnityEvent unityEvent = this.onActivate;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000DC9A4 File Offset: 0x000DABA4
	public void Revert()
	{
		if (this.toDisActivateObjects != null)
		{
			foreach (GameObject gameObject in this.toDisActivateObjects)
			{
				if (gameObject)
				{
					gameObject.SetActive(true);
				}
			}
		}
		if (this.toActivateObjects != null)
		{
			foreach (GameObject gameObject2 in this.toActivateObjects)
			{
				if (gameObject2)
				{
					gameObject2.SetActive(false);
				}
			}
		}
		UnityEvent unityEvent = this.onDisActivate;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040025A1 RID: 9633
	public GameObject[] toActivateObjects;

	// Token: 0x040025A2 RID: 9634
	public GameObject[] toDisActivateObjects;

	// Token: 0x040025A3 RID: 9635
	public UnityEvent onActivate;

	// Token: 0x040025A4 RID: 9636
	public UnityEvent onDisActivate;
}
