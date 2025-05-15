using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001A5 RID: 421
public class StanleyInputEvent : MonoBehaviour
{
	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002F1D8 File Offset: 0x0002D3D8
	private StanleyActions actions
	{
		get
		{
			return Singleton<GameMaster>.Instance.stanleyActions;
		}
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0002F1E4 File Offset: 0x0002D3E4
	public void SetListening(bool value)
	{
		this.listening = value;
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0002F1F0 File Offset: 0x0002D3F0
	private void Update()
	{
		if (GameMaster.PAUSEMENUACTIVE || !this.listening)
		{
			return;
		}
		switch (this.inputEvent)
		{
		case StanleyInputEvent.InputEvent.ExtraEvent1:
			if (this.actions.ExtraAction(0, false).WasPressed)
			{
				Debug.Log("");
			}
			if (this.actions.ExtraAction(0, false).WasPressed)
			{
				this.OnInput.Invoke();
				return;
			}
			break;
		case StanleyInputEvent.InputEvent.ExtraEvent2:
			if (this.actions.ExtraAction(1, false).WasPressed)
			{
				this.OnInput.Invoke();
				return;
			}
			break;
		case StanleyInputEvent.InputEvent.ExtraEvent3:
			if (this.actions.ExtraAction(2, false).WasPressed)
			{
				this.OnInput.Invoke();
				return;
			}
			break;
		case StanleyInputEvent.InputEvent.ExtraEvent4:
			if (this.actions.ExtraAction(3, false).WasPressed)
			{
				this.OnInput.Invoke();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x040009DF RID: 2527
	[SerializeField]
	private bool listening = true;

	// Token: 0x040009E0 RID: 2528
	[SerializeField]
	private StanleyInputEvent.InputEvent inputEvent;

	// Token: 0x040009E1 RID: 2529
	[SerializeField]
	private UnityEvent OnInput;

	// Token: 0x020003F9 RID: 1017
	public enum InputEvent
	{
		// Token: 0x040014BA RID: 5306
		ExtraEvent1,
		// Token: 0x040014BB RID: 5307
		ExtraEvent2,
		// Token: 0x040014BC RID: 5308
		ExtraEvent3,
		// Token: 0x040014BD RID: 5309
		ExtraEvent4
	}
}
