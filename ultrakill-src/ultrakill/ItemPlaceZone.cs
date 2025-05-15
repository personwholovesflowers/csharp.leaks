using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class ItemPlaceZone : MonoBehaviour
{
	// Token: 0x06000E64 RID: 3684 RVA: 0x0006B0EC File Offset: 0x000692EC
	private void Start()
	{
		this.col = base.GetComponent<Collider>();
		this.ColorDoors(this.doors);
		this.ColorDoors(this.reverseDoors);
		this.CheckItem(true);
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0006B119 File Offset: 0x00069319
	private void Awake()
	{
		this.GetDoorBounds(this.doors, this.doorsBounds);
		this.GetDoorBounds(this.reverseDoors, this.reverseDoorsBounds);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0006B140 File Offset: 0x00069340
	private void GetDoorBounds(Door[] doors, List<Bounds> boundies)
	{
		if (doors.Length != 0)
		{
			for (int i = 0; i < doors.Length; i++)
			{
				if (!doors[i].ignoreHookCheck)
				{
					List<Collider> list = new List<Collider>();
					if (doors[i].doorType == DoorType.Normal)
					{
						foreach (Collider collider in doors[i].GetComponentsInChildren<Collider>())
						{
							list.Add(collider);
						}
					}
					else if (doors[i].doorType == DoorType.BigDoorController)
					{
						BigDoor[] componentsInChildren = doors[i].GetComponentsInChildren<BigDoor>();
						for (int j = 0; j < componentsInChildren.Length; j++)
						{
							foreach (Collider collider2 in componentsInChildren[j].GetComponentsInChildren<Collider>())
							{
								list.Add(collider2);
							}
						}
					}
					else if (doors[i].doorType == DoorType.SubDoorController)
					{
						SubDoor[] componentsInChildren2 = doors[i].GetComponentsInChildren<SubDoor>();
						for (int j = 0; j < componentsInChildren2.Length; j++)
						{
							foreach (Collider collider3 in componentsInChildren2[j].GetComponentsInChildren<Collider>())
							{
								list.Add(collider3);
							}
						}
					}
					if (list.Count > 0)
					{
						Bounds bounds = list[0].bounds;
						if (list.Count > 1)
						{
							for (int l = 1; l < list.Count; l++)
							{
								bounds.Encapsulate(list[l].bounds);
							}
						}
						boundies.Add(bounds);
					}
					else
					{
						boundies.Add(new Bounds(Vector3.zero, Vector3.zero));
					}
				}
			}
		}
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x0006B2B4 File Offset: 0x000694B4
	public bool CheckDoorBounds(Vector3 origin, Vector3 end, bool reverseBounds)
	{
		bool flag = true;
		foreach (Bounds bounds in (reverseBounds ? this.reverseDoorsBounds : this.doorsBounds))
		{
			float num;
			if (bounds.IntersectRay(new Ray(origin, end - origin), out num) && num < Vector3.Distance(origin, end) + 1f)
			{
				Object.Instantiate<GameObject>(this.boundsError, bounds.center, Quaternion.identity).transform.localScale = bounds.size * 1.1f;
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x0006B36C File Offset: 0x0006956C
	private void ColorDoors(Door[] drs)
	{
		foreach (Door door in drs)
		{
			switch (this.acceptedItemType)
			{
			case ItemType.SkullBlue:
				door.defaultLightsColor = new Color(0f, 0.75f, 1f);
				break;
			case ItemType.SkullRed:
				door.defaultLightsColor = new Color(1f, 0.2f, 0.2f);
				break;
			case ItemType.SkullGreen:
				door.defaultLightsColor = new Color(0.25f, 1f, 0.25f);
				break;
			}
			if (!door.noPass || !door.noPass.activeSelf)
			{
				door.ChangeColor(door.defaultLightsColor);
			}
			door.AltarControlled();
		}
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0006B430 File Offset: 0x00069630
	public void CheckItem(bool prelim = false)
	{
		ItemIdentifier componentInChildren = base.GetComponentInChildren<ItemIdentifier>();
		if (componentInChildren != null)
		{
			if (componentInChildren.itemType == this.acceptedItemType)
			{
				if (!prelim && !this.hideEffects && !this.acceptedItemPlaced)
				{
					if (this.elementChangeEffect)
					{
						this.elementChangeEffect.Play();
					}
					if (this.soundOnActivated)
					{
						Object.Instantiate<AudioSource>(this.soundOnActivated, base.transform.position, Quaternion.identity);
					}
				}
				this.acceptedItemPlaced = true;
				componentInChildren.ipz = this;
				if (!this.hideEffects)
				{
					componentInChildren.SendMessage("OnCorrectUse", SendMessageOptions.DontRequireReceiver);
				}
				GameObject[] array = this.activateOnSuccess;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(true);
				}
				array = this.deactivateOnSuccess;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(false);
				}
				Door[] array2 = this.doors;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].Open(false, true);
				}
				array2 = this.reverseDoors;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].Close(false);
				}
				if (!this.hideEffects)
				{
					foreach (InstantiateObject instantiateObject in this.altarElements)
					{
						instantiateObject.gameObject.SetActive(true);
						if (!prelim && instantiateObject.gameObject.activeInHierarchy)
						{
							instantiateObject.Instantiate();
						}
					}
				}
				if (!prelim)
				{
					ArenaStatus[] array4 = this.arenaStatuses;
					for (int i = 0; i < array4.Length; i++)
					{
						array4[i].currentStatus++;
					}
					array4 = this.reverseArenaStatuses;
					for (int i = 0; i < array4.Length; i++)
					{
						array4[i].currentStatus--;
					}
				}
			}
			else
			{
				GameObject[] array = this.activateOnFailure;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(true);
				}
			}
			if (this.col)
			{
				this.col.enabled = false;
				return;
			}
		}
		else
		{
			if (this.col)
			{
				this.col.enabled = true;
			}
			if (prelim || this.acceptedItemPlaced)
			{
				GameObject[] array = this.activateOnSuccess;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(false);
				}
				array = this.activateOnFailure;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(false);
				}
				array = this.deactivateOnSuccess;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(true);
				}
				foreach (Door door in this.doors)
				{
					if (door.doorType != DoorType.Normal || door.transform.localPosition != door.closedPos)
					{
						door.Close(false);
					}
				}
				foreach (Door door2 in this.reverseDoors)
				{
					if (door2.doorType != DoorType.Normal || door2.transform.localPosition != door2.closedPos + door2.openPos)
					{
						door2.Open(false, true);
					}
				}
				if (!prelim)
				{
					this.acceptedItemPlaced = false;
					if (!this.hideEffects)
					{
						if (this.elementChangeEffect)
						{
							this.elementChangeEffect.Play();
						}
						if (this.soundOnDeactivated)
						{
							Object.Instantiate<AudioSource>(this.soundOnDeactivated, base.transform.position, Quaternion.identity);
						}
					}
					ArenaStatus[] array4 = this.arenaStatuses;
					for (int i = 0; i < array4.Length; i++)
					{
						array4[i].currentStatus--;
					}
					array4 = this.reverseArenaStatuses;
					for (int i = 0; i < array4.Length; i++)
					{
						array4[i].currentStatus++;
					}
				}
				InstantiateObject[] array3 = this.altarElements;
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i].gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x04001315 RID: 4885
	private bool acceptedItemPlaced;

	// Token: 0x04001316 RID: 4886
	public ItemType acceptedItemType;

	// Token: 0x04001317 RID: 4887
	public GameObject[] activateOnSuccess;

	// Token: 0x04001318 RID: 4888
	public GameObject[] deactivateOnSuccess;

	// Token: 0x04001319 RID: 4889
	public GameObject[] activateOnFailure;

	// Token: 0x0400131A RID: 4890
	public Door[] doors;

	// Token: 0x0400131B RID: 4891
	public Door[] reverseDoors;

	// Token: 0x0400131C RID: 4892
	public ArenaStatus[] arenaStatuses;

	// Token: 0x0400131D RID: 4893
	public ArenaStatus[] reverseArenaStatuses;

	// Token: 0x0400131E RID: 4894
	private Collider col;

	// Token: 0x0400131F RID: 4895
	private List<Bounds> doorsBounds = new List<Bounds>();

	// Token: 0x04001320 RID: 4896
	private List<Bounds> reverseDoorsBounds = new List<Bounds>();

	// Token: 0x04001321 RID: 4897
	public GameObject boundsError;

	// Token: 0x04001322 RID: 4898
	public bool hideEffects;

	// Token: 0x04001323 RID: 4899
	public InstantiateObject[] altarElements;

	// Token: 0x04001324 RID: 4900
	public ParticleSystem elementChangeEffect;

	// Token: 0x04001325 RID: 4901
	public AudioSource soundOnActivated;

	// Token: 0x04001326 RID: 4902
	public AudioSource soundOnDeactivated;
}
