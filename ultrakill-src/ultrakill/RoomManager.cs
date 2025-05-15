using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000394 RID: 916
public class RoomManager : MonoBehaviour
{
	// Token: 0x06001510 RID: 5392 RVA: 0x000AC8B9 File Offset: 0x000AAAB9
	private void Awake()
	{
		this.roomAmount = base.GetComponentInChildren<Text>();
		this.rsp = base.GetComponentInChildren<RandomSoundPlayer>();
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x000AC8D4 File Offset: 0x000AAAD4
	private void Update()
	{
		if (this.fadeToFin)
		{
			AudioSource component = GameObject.FindWithTag("EndingSong").GetComponent<AudioSource>();
			AudioSource component2 = GameObject.FindWithTag("EndingSongReverb").GetComponent<AudioSource>();
			Time.timeScale = 0.1f;
			Time.fixedDeltaTime = 0.002f;
			if (component.volume < 1f)
			{
				component.volume += 10f * Time.deltaTime;
			}
			component2.volume -= 10f * Time.deltaTime;
		}
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x000AC958 File Offset: 0x000AAB58
	public void SwitchRooms(string roomType)
	{
		if (roomType == "Hallway")
		{
			this.newRoomChance = Random.Range(0, this.newRoomMinChance);
			if (this.clearedRooms < this.rooms && this.totalLevels >= 4 && this.newRoomChance == 0)
			{
				int num = 0;
				while (num == 0)
				{
					this.nextRoom = Random.Range(1, this.rooms + 1);
					num++;
					foreach (int num2 in this.visitedRooms)
					{
						if (this.nextRoom == num2)
						{
							num = 0;
						}
					}
				}
			}
		}
		else if (roomType == "Room")
		{
			this.newRoomChance = Random.Range(0, this.newRoomMinChance);
			if (this.clearedHallways < this.rooms && this.totalLevels >= 4 && this.newRoomChance == 0)
			{
				int num3 = 0;
				while (num3 == 0)
				{
					this.nextRoom = Random.Range(this.rooms + 1, this.rooms * 2 + 1);
					num3++;
					foreach (int num4 in this.visitedRooms)
					{
						if (this.nextRoom == num4)
						{
							num3 = 0;
						}
					}
				}
			}
		}
		if (this.clearedRooms == this.rooms && this.clearedHallways == this.rooms && !this.allClear)
		{
			Application.LoadLevel(this.rooms * 2 + 1);
			this.allClear = true;
			this.rsp.playing = false;
			return;
		}
		if (this.allClear)
		{
			this.fadeToFin = true;
			base.Invoke("EndingStart", 0.1f);
			return;
		}
		if ((this.newRoomChance > 0 || this.totalLevels < 4 || this.clearedRooms == this.rooms) && roomType == "Hallway")
		{
			Application.LoadLevel(1);
			base.Invoke("RoomSwitched", 0.1f);
			if (this.newRoomChance > 0)
			{
				this.newRoomMinChance--;
				return;
			}
		}
		else if ((this.newRoomChance > 0 || this.totalLevels < 4 || this.clearedHallways == this.rooms) && roomType == "Room")
		{
			Application.LoadLevel(this.rooms + 1);
			base.Invoke("RoomSwitched", 0.1f);
			if (this.newRoomChance > 0)
			{
				this.newRoomMinChance--;
				return;
			}
		}
		else
		{
			if (roomType == "Hallway")
			{
				this.clearedRooms++;
			}
			else if (roomType == "Room")
			{
				this.clearedHallways++;
			}
			this.visitedRooms.Add(this.nextRoom);
			Application.LoadLevel(this.nextRoom);
			base.Invoke("RoomSwitched", 0.1f);
			this.newRoomMinChance = Random.Range(3, 6);
		}
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x000ACC6C File Offset: 0x000AAE6C
	private void RoomSwitched()
	{
		this.totalLevels++;
		if (this.totalLevels < 10)
		{
			this.roomAmount.text = "00" + this.totalLevels.ToString();
		}
		else if (this.totalLevels < 100)
		{
			this.roomAmount.text = "0" + this.totalLevels.ToString();
		}
		else if (this.totalLevels < 1000)
		{
			this.roomAmount.text = this.totalLevels.ToString() ?? "";
		}
		else
		{
			this.roomAmount.text = "???";
		}
		this.rsp.RollForPlay();
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x000ACD28 File Offset: 0x000AAF28
	private void EndingStart()
	{
		Application.LoadLevel(this.rooms * 2 + 2);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001D4B RID: 7499
	public List<int> visitedRooms = new List<int>();

	// Token: 0x04001D4C RID: 7500
	private int nextRoom;

	// Token: 0x04001D4D RID: 7501
	private int newRoomChance;

	// Token: 0x04001D4E RID: 7502
	private int newRoomMinChance = 4;

	// Token: 0x04001D4F RID: 7503
	public int totalLevels;

	// Token: 0x04001D50 RID: 7504
	public int rooms;

	// Token: 0x04001D51 RID: 7505
	public int clearedHallways;

	// Token: 0x04001D52 RID: 7506
	public int clearedRooms;

	// Token: 0x04001D53 RID: 7507
	public bool allClear;

	// Token: 0x04001D54 RID: 7508
	private Text roomAmount;

	// Token: 0x04001D55 RID: 7509
	private RandomSoundPlayer rsp;

	// Token: 0x04001D56 RID: 7510
	private bool fadeToFin;
}
