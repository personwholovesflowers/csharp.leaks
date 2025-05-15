using System;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class EventOnSceneName : MonoBehaviour
{
	// Token: 0x06000878 RID: 2168 RVA: 0x0003A516 File Offset: 0x00038716
	private void OnEnable()
	{
		if (SceneHelper.CurrentScene == this.sceneName)
		{
			this.onSceneName.Invoke("");
			return;
		}
		if (this.emitRevertOnSceneMismatch)
		{
			this.onSceneName.Revert();
		}
	}

	// Token: 0x04000B43 RID: 2883
	public string sceneName;

	// Token: 0x04000B44 RID: 2884
	public bool emitRevertOnSceneMismatch;

	// Token: 0x04000B45 RID: 2885
	public UltrakillEvent onSceneName;
}
