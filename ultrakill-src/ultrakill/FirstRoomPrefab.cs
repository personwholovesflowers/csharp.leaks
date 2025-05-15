using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class FirstRoomPrefab : MonoBehaviour, IPlaceholdableComponent
{
	// Token: 0x06000118 RID: 280 RVA: 0x00006502 File Offset: 0x00004702
	public void WillReplace(GameObject oldObject, GameObject newObject, bool isSelfBeingReplaced)
	{
		if (!isSelfBeingReplaced)
		{
			return;
		}
		newObject.GetComponent<FirstRoomPrefab>().SwapData(this);
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00006514 File Offset: 0x00004714
	private void SwapData(FirstRoomPrefab source)
	{
		List<GameObject> list = new List<GameObject>();
		list.AddRange(this.mainDoor.activatedRooms);
		list.AddRange(source.activateOnFirstRoomDoorOpen);
		this.mainDoor.activatedRooms = list.ToArray();
		MonoSingleton<OnLevelStart>.Instance.levelNameOnStart = source.levelNameOnOpen;
	}

	// Token: 0x040000DA RID: 218
	[HideInInspector]
	public GameObject[] activateOnFirstRoomDoorOpen;

	// Token: 0x040000DB RID: 219
	[HideInInspector]
	public bool levelNameOnOpen = true;

	// Token: 0x040000DC RID: 220
	public Door mainDoor;

	// Token: 0x040000DD RID: 221
	public FinalDoor finalDoor;
}
