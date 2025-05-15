using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020000E9 RID: 233
public class InGameEvent : MonoBehaviour
{
	// Token: 0x060005A6 RID: 1446 RVA: 0x0001F995 File Offset: 0x0001DB95
	private void Start()
	{
		GameMaster.OnPrepareLoadingLevel += this.ForceUpdateNextFrame;
		this.ForceUpdateNextFrame();
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0001F9AE File Offset: 0x0001DBAE
	private void ForceUpdateNextFrame()
	{
		this.forceUpdate = true;
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x000168FE File Offset: 0x00014AFE
	private bool IsInGame
	{
		get
		{
			return !GameMaster.PAUSEMENUACTIVE && !GameMaster.ONMAINMENUORSETTINGS && !Singleton<GameMaster>.Instance.IsLoading && !Singleton<GameMaster>.Instance.FullScreenMoviePlaying && !StanleyController.Instance.motionFrozen;
		}
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0001F9B8 File Offset: 0x0001DBB8
	private void Update()
	{
		bool isInGame = this.IsInGame;
		if (!isInGame && (this.wasInGame || this.forceUpdate))
		{
			this.onMenuToggle.Invoke(false);
			this.onGameSuspend.Invoke();
		}
		if (isInGame && (!this.wasInGame || this.forceUpdate))
		{
			this.onMenuToggle.Invoke(true);
			this.onGameResume.Invoke();
		}
		if (this.forceUpdate)
		{
			this.forceUpdate = false;
		}
		this.wasInGame = isInGame;
	}

	// Token: 0x040005E5 RID: 1509
	private bool forceUpdate;

	// Token: 0x040005E6 RID: 1510
	private bool wasInGame;

	// Token: 0x040005E7 RID: 1511
	[SerializeField]
	private BooleanValueChangedEvent onMenuToggle;

	// Token: 0x040005E8 RID: 1512
	[SerializeField]
	[FormerlySerializedAs("onMenuEnter")]
	private UnityEvent onGameSuspend;

	// Token: 0x040005E9 RID: 1513
	[SerializeField]
	[FormerlySerializedAs("onMenuExit")]
	private UnityEvent onGameResume;
}
