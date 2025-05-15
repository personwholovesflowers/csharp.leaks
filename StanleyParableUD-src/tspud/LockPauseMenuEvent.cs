using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class LockPauseMenuEvent : MonoBehaviour
{
	// Token: 0x06000628 RID: 1576 RVA: 0x00021FC1 File Offset: 0x000201C1
	public void LockPauseMenu()
	{
		Singleton<GameMaster>.Instance.pauseMenuBlocked = true;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00021FCE File Offset: 0x000201CE
	public void UnlockPauseMenu()
	{
		Singleton<GameMaster>.Instance.pauseMenuBlocked = false;
	}
}
