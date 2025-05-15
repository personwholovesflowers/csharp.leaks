using System;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class SwitchMovableTree : MonoBehaviour
{
	// Token: 0x06000A15 RID: 2581 RVA: 0x0002FA15 File Offset: 0x0002DC15
	private void Start()
	{
		this.MoveToPosition(CullForSwitchController.IsSwitchEnvironment);
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0002FA24 File Offset: 0x0002DC24
	[ContextMenu("Move All To Switch Position")]
	private void MoveAllToSwitchPosition()
	{
		SwitchMovableTree[] array = Object.FindObjectsOfType<SwitchMovableTree>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MoveToPosition(true);
		}
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0002FA50 File Offset: 0x0002DC50
	[ContextMenu("Move All To Normal Position")]
	private void MoveAllToNormalPosition()
	{
		SwitchMovableTree[] array = Object.FindObjectsOfType<SwitchMovableTree>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MoveToPosition(false);
		}
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0002FA7A File Offset: 0x0002DC7A
	private void MoveToPosition(bool toSwitchPosition)
	{
		if (toSwitchPosition)
		{
			this.objectToMove.transform.position = this.switchPosition.position;
			return;
		}
		this.objectToMove.transform.localPosition = Vector3.zero;
	}

	// Token: 0x04000A0B RID: 2571
	public Transform switchPosition;

	// Token: 0x04000A0C RID: 2572
	public GameObject objectToMove;
}
