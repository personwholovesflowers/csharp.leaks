using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200021B RID: 539
public class AutoRegisterState : MonoBehaviour
{
	// Token: 0x06000B90 RID: 2960 RVA: 0x00051DC0 File Offset: 0x0004FFC0
	private void OnEnable()
	{
		List<GameObject> list = new List<GameObject>();
		if (this.trackSelf)
		{
			list.Add(base.gameObject);
		}
		if (this.additionalTrackedObjects != null)
		{
			list.AddRange(this.additionalTrackedObjects);
		}
		if (this.ownState == null)
		{
			if (list.Count == 0)
			{
				this.ownState = new GameState(this.stateKey);
			}
			else if (list.Count == 1)
			{
				this.ownState = new GameState(this.stateKey, list[0]);
			}
			else
			{
				this.ownState = new GameState(this.stateKey, list.ToArray());
			}
		}
		this.ownState.playerInputLock = this.playerInputLock;
		this.ownState.cameraInputLock = this.cameraInputLock;
		this.ownState.cursorLock = this.cursorLock;
		this.ownState.priority = this.priority;
		GameStateManager.Instance.RegisterState(this.ownState);
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00051EAC File Offset: 0x000500AC
	private void OnDestroy()
	{
		GameStateManager.Instance.PopState(this.stateKey);
	}

	// Token: 0x04000F1F RID: 3871
	public string stateKey;

	// Token: 0x04000F20 RID: 3872
	[Space]
	public bool trackSelf = true;

	// Token: 0x04000F21 RID: 3873
	[Tooltip("If any of the tracked objects remain active, the state will be considered valid")]
	public GameObject[] additionalTrackedObjects;

	// Token: 0x04000F22 RID: 3874
	[FormerlySerializedAs("playerInputBlocking")]
	[Space]
	public LockMode playerInputLock;

	// Token: 0x04000F23 RID: 3875
	[FormerlySerializedAs("cameraInputBlocking")]
	public LockMode cameraInputLock;

	// Token: 0x04000F24 RID: 3876
	public LockMode cursorLock;

	// Token: 0x04000F25 RID: 3877
	[Space]
	public int priority = 1;

	// Token: 0x04000F26 RID: 3878
	private GameState ownState;
}
