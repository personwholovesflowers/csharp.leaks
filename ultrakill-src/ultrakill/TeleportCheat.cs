using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200046E RID: 1134
public class TeleportCheat : MonoBehaviour
{
	// Token: 0x06001A1A RID: 6682 RVA: 0x000D77E1 File Offset: 0x000D59E1
	private void Start()
	{
		this.GenerateList();
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000D77EC File Offset: 0x000D59EC
	private void GenerateList()
	{
		List<TeleportCheat.TeleportTarget> list = new List<TeleportCheat.TeleportTarget>();
		FirstRoomPrefab firstRoom = Object.FindObjectOfType<FirstRoomPrefab>();
		if (firstRoom)
		{
			GameObject gameObject = new GameObject("First Room Teleport Target");
			gameObject.transform.position = firstRoom.transform.position + new Vector3(0f, -9.75f, -1f);
			list.Add(new TeleportCheat.TeleportTarget
			{
				overrideName = "First Room",
				target = gameObject.transform,
				firstRoom = firstRoom
			});
		}
		CheckPoint[] array = Object.FindObjectsOfType<CheckPoint>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].unteleportable)
			{
				list.Add(new TeleportCheat.TeleportTarget
				{
					target = array[i].transform,
					checkpoint = array[i]
				});
			}
		}
		using (List<TeleportCheat.TeleportTarget>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TeleportCheat.TeleportTarget point = enumerator.Current;
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.buttonTemplate, this.buttonTemplate.transform.parent);
				gameObject2.GetComponentInChildren<TMP_Text>().text = ((!string.IsNullOrEmpty(point.overrideName)) ? point.overrideName : (point.checkpoint ? (point.checkpoint.toActivate ? this.ImproveCheckpointName(point.checkpoint.toActivate.name) : "<color=red>Missing toActivate</color>") : point.target.name));
				gameObject2.GetComponentInChildren<TMP_Text>().color = (point.checkpoint ? this.checkpointColor : this.roomColor);
				gameObject2.GetComponentInChildren<Button>().onClick.AddListener(delegate
				{
					this.Teleport(point.target);
					if (point.checkpoint)
					{
						point.checkpoint.toActivate.SetActive(true);
						if (point.checkpoint.doorsToUnlock.Length != 0)
						{
							foreach (Door door in point.checkpoint.doorsToUnlock)
							{
								if (door.locked)
								{
									door.Unlock();
								}
								if (door.startOpen)
								{
									door.Open(false, false);
								}
							}
						}
						UnityEvent onRestart = point.checkpoint.onRestart;
						if (onRestart != null)
						{
							onRestart.Invoke();
						}
					}
					if (firstRoom)
					{
						foreach (GameObject gameObject3 in firstRoom.mainDoor.activatedRooms)
						{
							if (gameObject3 != null)
							{
								gameObject3.SetActive(true);
							}
						}
					}
				});
				gameObject2.SetActive(true);
			}
		}
		this.buttonTemplate.SetActive(false);
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000D7A40 File Offset: 0x000D5C40
	private void Update()
	{
		if (MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame)
		{
			base.gameObject.SetActive(false);
			MonoSingleton<OptionsManager>.Instance.UnFreeze();
		}
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x000D7A70 File Offset: 0x000D5C70
	private string ImproveCheckpointName(string original)
	{
		string text = original;
		if (original.Contains("- "))
		{
			string[] array = original.Split('-', StringSplitOptions.None);
			text = array[array.Length - 1];
		}
		if (this.checkpointNames.Contains(text))
		{
			for (int i = 2; i <= 99; i++)
			{
				if (!this.checkpointNames.Contains(text + string.Format(" ({0})", i)))
				{
					text += string.Format(" ({0})", i);
					break;
				}
			}
		}
		this.checkpointNames.Add(text);
		return text;
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x000D7B00 File Offset: 0x000D5D00
	private void Teleport(Transform target)
	{
		MonoSingleton<NewMovement>.Instance.transform.position = target.position + Vector3.up * 1.25f;
		float num = target.rotation.eulerAngles.y + 0.01f;
		if (MonoSingleton<NewMovement>.Instance.transform.parent && MonoSingleton<NewMovement>.Instance.transform.parent.gameObject.CompareTag("Moving"))
		{
			num -= MonoSingleton<NewMovement>.Instance.transform.parent.rotation.eulerAngles.y;
		}
		MonoSingleton<CameraController>.Instance.ResetCamera(num, 0f);
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
		{
			MonoSingleton<PlatformerMovement>.Instance.transform.position = target.position;
			MonoSingleton<PlatformerMovement>.Instance.rb.velocity = Vector3.zero;
			MonoSingleton<PlatformerMovement>.Instance.playerModel.rotation = target.rotation;
			MonoSingleton<PlatformerMovement>.Instance.SnapCamera();
		}
		base.gameObject.SetActive(false);
		MonoSingleton<OptionsManager>.Instance.UnFreeze();
		MonoSingleton<PlayerTracker>.Instance.LevelStart();
	}

	// Token: 0x04002494 RID: 9364
	[SerializeField]
	private GameObject buttonTemplate;

	// Token: 0x04002495 RID: 9365
	[SerializeField]
	private Color checkpointColor;

	// Token: 0x04002496 RID: 9366
	[SerializeField]
	private Color roomColor;

	// Token: 0x04002497 RID: 9367
	private List<string> checkpointNames = new List<string>();

	// Token: 0x0200046F RID: 1135
	private class TeleportTarget
	{
		// Token: 0x04002498 RID: 9368
		public string overrideName;

		// Token: 0x04002499 RID: 9369
		public CheckPoint checkpoint;

		// Token: 0x0400249A RID: 9370
		public FirstRoomPrefab firstRoom;

		// Token: 0x0400249B RID: 9371
		public Transform target;
	}
}
