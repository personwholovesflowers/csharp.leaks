using System;
using UnityEngine;

// Token: 0x02000495 RID: 1173
public class UnlockMouse : MonoBehaviour
{
	// Token: 0x06001AE4 RID: 6884 RVA: 0x000DD10B File Offset: 0x000DB30B
	private void OnEnable()
	{
		if (!this.unlockOnEnable)
		{
			return;
		}
		this.Unlock();
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x000DD11C File Offset: 0x000DB31C
	public void Unlock()
	{
		GameStateManager.Instance.RegisterState(new GameState("unlock-mouse-component", base.gameObject)
		{
			cursorLock = LockMode.Unlock
		});
		this.wasEnabled = true;
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x000DD148 File Offset: 0x000DB348
	private void OnDisable()
	{
		if (this.lockOnDisable && this.wasEnabled && base.gameObject.scene.isLoaded)
		{
			this.Lock();
		}
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x000DD180 File Offset: 0x000DB380
	public void Lock()
	{
		if (this.wasEnabled)
		{
			this.wasEnabled = false;
			GameStateManager.Instance.PopState("unlock-mouse-component");
		}
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000DD1A0 File Offset: 0x000DB3A0
	private void OnApplicationQuit()
	{
		this.lockOnDisable = false;
	}

	// Token: 0x040025DB RID: 9691
	public bool unlockOnEnable = true;

	// Token: 0x040025DC RID: 9692
	public bool lockOnDisable;

	// Token: 0x040025DD RID: 9693
	private bool wasEnabled;
}
