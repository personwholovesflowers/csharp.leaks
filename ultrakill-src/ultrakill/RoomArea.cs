using System;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class RoomArea : MonoBehaviour
{
	// Token: 0x0600150C RID: 5388 RVA: 0x000AC854 File Offset: 0x000AAA54
	private void Awake()
	{
		this.rm = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x000AC86B File Offset: 0x000AAA6B
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.ChangeLevel();
		}
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x000AC885 File Offset: 0x000AAA85
	private void ChangeLevel()
	{
		if (this.roomType == State.Hallway)
		{
			this.rm.SwitchRooms("Hallway");
			return;
		}
		if (this.roomType == State.Room)
		{
			this.rm.SwitchRooms("Room");
		}
	}

	// Token: 0x04001D48 RID: 7496
	public State roomType;

	// Token: 0x04001D49 RID: 7497
	private RoomManager rm;

	// Token: 0x04001D4A RID: 7498
	private string roomName;
}
