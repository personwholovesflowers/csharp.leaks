using System;
using System.Collections.Generic;
using Logic;
using Sandbox.Arm;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

// Token: 0x020000BF RID: 191
public class CheckPoint : MonoBehaviour
{
	// Token: 0x060003BC RID: 956 RVA: 0x000171C0 File Offset: 0x000153C0
	private void Start()
	{
		foreach (GameObject gameObject in this.rooms)
		{
			this.defaultRooms.Add(gameObject);
		}
		for (int j = 0; j < this.defaultRooms.Count; j++)
		{
			GoreZone goreZone;
			if (!this.defaultRooms[j].TryGetComponent<GoreZone>(out goreZone))
			{
				goreZone = this.defaultRooms[j].AddComponent<GoreZone>();
			}
			goreZone.checkpoint = this;
			this.newRooms.Add(Object.Instantiate<GameObject>(this.defaultRooms[j], this.defaultRooms[j].transform.position, this.defaultRooms[j].transform.rotation, this.defaultRooms[j].transform.parent));
			this.defaultRooms[j].gameObject.SetActive(false);
			this.newRooms[j].gameObject.SetActive(true);
			Bonus[] componentsInChildren = this.newRooms[j].GetComponentsInChildren<Bonus>(true);
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				Bonus[] array2 = componentsInChildren;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].UpdateStatsManagerReference();
				}
			}
			this.defaultRooms[j].transform.position = new Vector3(this.defaultRooms[j].transform.position.x + 10000f, this.defaultRooms[j].transform.position.y, this.defaultRooms[j].transform.position.z);
		}
		this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		this.sm = MonoSingleton<StatsManager>.Instance;
		if (this.shud == null)
		{
			this.shud = MonoSingleton<StyleHUD>.Instance;
		}
		if (this.inheritAllRooms)
		{
			this.roomsToInherit.Clear();
			this.inheritNames.Clear();
			this.inheritParents.Clear();
			GoreZone[] array3 = Object.FindObjectsOfType<GoreZone>(true);
			for (int k = 0; k < array3.Length; k++)
			{
				if (array3[k].isNewest)
				{
					this.roomsToInherit.Add(array3[k].gameObject);
				}
			}
		}
		for (int l = 0; l < this.roomsToInherit.Count; l++)
		{
			this.inheritNames.Add(this.roomsToInherit[l].name);
			this.inheritParents.Add(this.roomsToInherit[l].transform.parent);
		}
		if (this.unlockAllDoors)
		{
			Door[] array4 = Object.FindObjectsOfType<Door>(true);
			if (this.doorsToIgnore == null || this.doorsToIgnore.Length == 0)
			{
				this.doorsToUnlock = new Door[array4.Length];
				for (int m = 0; m < array4.Length; m++)
				{
					this.doorsToUnlock[m] = array4[m];
				}
			}
			else
			{
				List<Door> list = new List<Door>();
				bool flag = false;
				for (int n = 0; n < array4.Length; n++)
				{
					for (int num = 0; num < this.doorsToIgnore.Length; num++)
					{
						if (array4[n] == this.doorsToIgnore[num])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(array4[n]);
					}
					else
					{
						flag = false;
					}
				}
				this.doorsToUnlock = new Door[list.Count];
				for (int num2 = 0; num2 < list.Count; num2++)
				{
					this.doorsToUnlock[num2] = list[num2];
				}
			}
		}
		MonoSingleton<CheckPointsController>.Instance.AddCheckpoint(this);
		if (this.startOff)
		{
			this.activated = true;
			GameObject gameObject2 = this.graphic;
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
			ModifyMaterial modifyMaterial;
			if (base.TryGetComponent<ModifyMaterial>(out modifyMaterial))
			{
				modifyMaterial.ChangeEmissionIntensity(0f);
			}
		}
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001759F File Offset: 0x0001579F
	private void OnDisable()
	{
		this.inDuringResetSafety = false;
	}

	// Token: 0x060003BE RID: 958 RVA: 0x000175A8 File Offset: 0x000157A8
	private bool ShouldBeOff()
	{
		return this.forceOff || (this.disableDuringCombat && MonoSingleton<MusicManager>.Instance.requestedThemes > 0f);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x000175D0 File Offset: 0x000157D0
	private void Update()
	{
		if (this.resetSafetyTimer > 0f)
		{
			this.resetSafetyTimer = Mathf.MoveTowards(this.resetSafetyTimer, 0f, Time.deltaTime);
			if (this.resetSafetyTimer == 0f && this.inDuringResetSafety && !this.activated)
			{
				this.ActivateCheckPoint();
			}
		}
		if (this.activated && this.resetOnDistance && Vector3.Distance(MonoSingleton<PlayerTracker>.Instance.GetPlayer().position, base.transform.position) > this.autoResetDistance)
		{
			this.ReactivateCheckpoint();
		}
		if (!this.activated && this.graphic)
		{
			if ((this.ShouldBeOff() || this.invisible) && this.graphic.activeSelf)
			{
				this.graphic.SetActive(false);
				return;
			}
			if (!this.ShouldBeOff() && !this.invisible && !this.graphic.activeSelf)
			{
				this.ReactivationEffect();
			}
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x000176C8 File Offset: 0x000158C8
	private void OnTriggerEnter(Collider other)
	{
		if (!this.activated && !this.ShouldBeOff() && ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject)))
		{
			if (this.resetSafetyTimer > 0.25f)
			{
				this.inDuringResetSafety = true;
				return;
			}
			this.ActivateCheckPoint();
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00017748 File Offset: 0x00015948
	private void OnTriggerExit(Collider other)
	{
		if (!this.inDuringResetSafety)
		{
			return;
		}
		if ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject))
		{
			this.inDuringResetSafety = false;
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x000177AC File Offset: 0x000159AC
	public void ActivateCheckPoint()
	{
		this.sm = MonoSingleton<StatsManager>.Instance;
		this.inDuringResetSafety = false;
		if (this.sm.currentCheckPoint && this.sm.currentCheckPoint != this)
		{
			MonoSingleton<NewMovement>.Instance.sameCheckpointRestarts = 0;
			if (this.sm.currentCheckPoint.resetOnGetOtherCheckpoint)
			{
				this.sm.currentCheckPoint.ReactivateCheckpoint();
			}
		}
		this.sm.currentCheckPoint = this;
		this.activated = true;
		if (!this.invisible && this.activateEffect.RuntimeKeyIsValid())
		{
			Object.Instantiate<GameObject>(this.activateEffect.ToAsset(), MonoSingleton<PlayerTracker>.Instance.GetPlayer().position, Quaternion.identity);
		}
		if (this.graphic)
		{
			this.graphic.SetActive(false);
		}
		if (MonoSingleton<PlatformerMovement>.Instance)
		{
			MonoSingleton<CrateCounter>.Instance.SaveStuff();
		}
		if (MonoSingleton<MapVarManager>.Instance)
		{
			MonoSingleton<MapVarManager>.Instance.StashStore();
		}
		this.stylePoints = this.sm.stylePoints;
		this.restartKills = 0;
		if (MonoSingleton<ChallengeManager>.Instance)
		{
			this.challengeAlreadyFailed = MonoSingleton<ChallengeManager>.Instance.challengeFailed;
		}
		if (MonoSingleton<ChallengeManager>.Instance)
		{
			this.challengeAlreadyDone = MonoSingleton<ChallengeManager>.Instance.challengeDone;
		}
		if (!this.firstTime)
		{
			this.defaultRooms.Clear();
			this.newRooms.Clear();
			if (this.rooms.Length != 0)
			{
				foreach (GameObject gameObject in this.rooms)
				{
					this.roomsToInherit.Add(gameObject);
					this.inheritNames.Add(gameObject.name);
					this.inheritParents.Add(gameObject.transform.parent);
				}
				this.rooms = new GameObject[0];
			}
		}
		if (this.shud == null)
		{
			this.shud = MonoSingleton<StyleHUD>.Instance;
		}
		if (this.roomsToInherit.Count != 0)
		{
			for (int j = 0; j < this.roomsToInherit.Count; j++)
			{
				string text = this.inheritNames[j];
				text = text.Replace("(Clone)", "");
				GameObject gameObject2 = null;
				for (int k = this.inheritParents[j].childCount - 1; k >= 0; k--)
				{
					GameObject gameObject3 = this.inheritParents[j].GetChild(k).gameObject;
					if (gameObject3.name.Replace("(Clone)", "") == text)
					{
						if (gameObject2 == null)
						{
							gameObject2 = gameObject3;
						}
						else
						{
							Object.Destroy(gameObject3);
						}
					}
				}
				this.InheritRoom(gameObject2);
			}
		}
		MonoSingleton<BloodsplatterManager>.Instance.SaveBloodstains();
		this.firstTime = false;
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00017A74 File Offset: 0x00015C74
	public void OnRespawn()
	{
		MonoSingleton<StainVoxelManager>.Instance.RemoveAllStains();
		MonoSingleton<BloodsplatterManager>.Instance.LoadBloodstains();
		if (this.player == null)
		{
			this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (this.player.GetComponentInParent<GoreZone>() != null)
		{
			this.player.transform.parent = null;
		}
		this.player.transform.position = Vector3.one * -1000f;
		if (MonoSingleton<PlatformerMovement>.Instance)
		{
			if (MonoSingleton<PlatformerMovement>.Instance.GetComponentInParent<GoreZone>() != null)
			{
				MonoSingleton<PlatformerMovement>.Instance.transform.parent = null;
			}
			MonoSingleton<PlatformerMovement>.Instance.transform.position = Vector3.one * -1000f;
		}
		if (MonoSingleton<MapVarManager>.Instance)
		{
			MonoSingleton<MapVarManager>.Instance.RestoreStashedStore();
		}
		this.i = 0;
		if (SandboxArm.debugZone && !MapInfoBase.Instance.sandboxTools)
		{
			Object.Destroy(SandboxArm.debugZone.gameObject);
		}
		if (!this.activated)
		{
			this.activated = true;
			if (this.graphic != null)
			{
				this.graphic.SetActive(false);
			}
		}
		this.sm.kills -= this.restartKills;
		this.restartKills = 0;
		this.sm.stylePoints = this.stylePoints;
		if (MonoSingleton<ChallengeManager>.Instance)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = this.challengeAlreadyDone && !MonoSingleton<ChallengeManager>.Instance.challengeFailedPermanently;
			MonoSingleton<ChallengeManager>.Instance.challengeFailed = this.challengeAlreadyFailed || MonoSingleton<ChallengeManager>.Instance.challengeFailedPermanently;
		}
		if (this.succesfulHitters.Count > 0)
		{
			KillHitterCache instance = MonoSingleton<KillHitterCache>.Instance;
			if (instance && !instance.ignoreRestarts)
			{
				foreach (int num in this.succesfulHitters)
				{
					instance.RemoveId(num);
				}
			}
		}
		if (this.shud == null)
		{
			this.shud = MonoSingleton<StyleHUD>.Instance;
		}
		this.shud.ComboOver();
		this.shud.ResetAllFreshness();
		MonoSingleton<FistControl>.Instance.fistCooldown = 0f;
		if (this.doorsToUnlock.Length != 0)
		{
			foreach (Door door in this.doorsToUnlock)
			{
				if (!(door == null))
				{
					if (door.locked)
					{
						door.Unlock();
					}
					if (door.startOpen && !door.open)
					{
						door.Open(false, false);
					}
				}
			}
		}
		DestroyOnCheckpointRestart[] array2 = Object.FindObjectsOfType<DestroyOnCheckpointRestart>();
		if (array2 != null && array2.Length != 0)
		{
			foreach (DestroyOnCheckpointRestart destroyOnCheckpointRestart in array2)
			{
				if (destroyOnCheckpointRestart.gameObject.activeInHierarchy && !destroyOnCheckpointRestart.dontDestroy)
				{
					Object.Destroy(destroyOnCheckpointRestart.gameObject);
				}
			}
		}
		Harpoon[] array4 = Object.FindObjectsOfType<Harpoon>();
		if (array4 != null && array4.Length != 0)
		{
			foreach (Harpoon harpoon in array4)
			{
				if (harpoon.gameObject.activeInHierarchy)
				{
					TimeBomb componentInChildren = harpoon.GetComponentInChildren<TimeBomb>();
					if (componentInChildren)
					{
						componentInChildren.dontExplode = true;
					}
					Object.Destroy(harpoon.gameObject);
				}
			}
		}
		DoorController[] array6 = Object.FindObjectsOfType<DoorController>();
		if (array6 != null && array6.Length != 0)
		{
			DoorController[] array7 = array6;
			for (int i = 0; i < array7.Length; i++)
			{
				array7[i].ForcePlayerOut();
			}
		}
		HookPoint[] array8 = Object.FindObjectsOfType<HookPoint>();
		if (array8 != null && array8.Length != 0)
		{
			foreach (HookPoint hookPoint in array8)
			{
				if (hookPoint.timer > 0f)
				{
					hookPoint.TimerStop();
				}
			}
		}
		if (MonoSingleton<CoinList>.Instance && MonoSingleton<CoinList>.Instance.revolverCoinsList.Count > 0)
		{
			for (int j = MonoSingleton<CoinList>.Instance.revolverCoinsList.Count - 1; j >= 0; j--)
			{
				if (!MonoSingleton<CoinList>.Instance.revolverCoinsList[j].dontDestroyOnPlayerRespawn)
				{
					Object.Destroy(MonoSingleton<CoinList>.Instance.revolverCoinsList[j].gameObject);
					MonoSingleton<CoinList>.Instance.revolverCoinsList.RemoveAt(j);
				}
			}
		}
		if (this.newRooms.Count > 0)
		{
			this.ResetRoom();
		}
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00017F04 File Offset: 0x00016104
	public void ResetRoom()
	{
		Vector3 position = this.newRooms[this.i].transform.position;
		this.newRooms[this.i].SetActive(false);
		Object.Destroy(this.newRooms[this.i]);
		this.newRooms[this.i] = Object.Instantiate<GameObject>(this.defaultRooms[this.i], position, this.defaultRooms[this.i].transform.rotation, this.defaultRooms[this.i].transform.parent);
		this.newRooms[this.i].SetActive(true);
		Bonus[] componentsInChildren = this.newRooms[this.i].GetComponentsInChildren<Bonus>(true);
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			Bonus[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateStatsManagerReference();
			}
		}
		if (this.i + 1 < this.defaultRooms.Count)
		{
			this.i++;
			this.ResetRoom();
			return;
		}
		if (this.toActivate)
		{
			this.toActivate.SetActive(true);
		}
		UnityEvent unityEvent = this.onRestart;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (!this.activated)
		{
			this.activated = true;
			if (this.graphic)
			{
				this.graphic.SetActive(false);
			}
		}
		this.player.transform.position = base.transform.position + Vector3.up * 1.25f;
		this.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (this.nm == null)
		{
			this.nm = MonoSingleton<NewMovement>.Instance;
		}
		float num = base.transform.rotation.eulerAngles.y + 0.01f + this.additionalSpawnRotation;
		if (this.player && this.player.transform.parent && this.player.transform.parent.gameObject.CompareTag("Moving"))
		{
			num -= this.player.transform.parent.rotation.eulerAngles.y;
		}
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
		{
			this.nm.cc.ResetCamera(num, 0f);
		}
		else
		{
			MonoSingleton<PlatformerMovement>.Instance.ResetCamera(num, 0f);
		}
		MonoSingleton<CameraController>.Instance.activated = true;
		if (!this.nm.enabled)
		{
			this.nm.enabled = true;
		}
		this.nm.Respawn();
		this.nm.GetHealth(0, true, false, true);
		this.nm.cc.StopShake();
		this.nm.ActivatePlayer();
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
		{
			MonoSingleton<PlatformerMovement>.Instance.transform.position = base.transform.position;
			MonoSingleton<PlatformerMovement>.Instance.rb.velocity = Vector3.zero;
			MonoSingleton<PlatformerMovement>.Instance.playerModel.rotation = base.transform.rotation;
			if (this.additionalSpawnRotation != 0f)
			{
				MonoSingleton<PlatformerMovement>.Instance.playerModel.Rotate(Vector3.up, this.additionalSpawnRotation);
			}
			MonoSingleton<PlatformerMovement>.Instance.gameObject.SetActive(true);
			MonoSingleton<PlatformerMovement>.Instance.SnapCamera();
			MonoSingleton<PlatformerMovement>.Instance.Respawn();
			MonoSingleton<CrateCounter>.Instance.ResetUnsavedStuff();
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x000182B0 File Offset: 0x000164B0
	public void UpdateRooms()
	{
		Vector3 position = this.newRooms[this.i].transform.position;
		Object.Destroy(this.newRooms[this.i]);
		this.newRooms[this.i] = Object.Instantiate<GameObject>(this.defaultRooms[this.i], position, this.defaultRooms[this.i].transform.rotation, this.defaultRooms[this.i].transform.parent);
		this.newRooms[this.i].SetActive(true);
		Bonus[] componentsInChildren = this.newRooms[this.i].GetComponentsInChildren<Bonus>(true);
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			Bonus[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateStatsManagerReference();
			}
		}
		this.newRooms[this.i].GetComponent<GoreZone>().isNewest = true;
		if (this.i + 1 < this.defaultRooms.Count)
		{
			this.i++;
			this.UpdateRooms();
			return;
		}
		this.i = 0;
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x000183E8 File Offset: 0x000165E8
	public void InheritRoom(GameObject targetRoom)
	{
		new List<GameObject>();
		new List<GameObject>();
		this.defaultRooms.Add(targetRoom);
		int num = this.defaultRooms.IndexOf(targetRoom);
		GoreZone component = this.defaultRooms[num].GetComponent<GoreZone>();
		component.checkpoint = this;
		component.isNewest = false;
		RemoveOnTime[] componentsInChildren = this.defaultRooms[num].GetComponentsInChildren<RemoveOnTime>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.SetActive(false);
		}
		this.newRooms.Add(Object.Instantiate<GameObject>(this.defaultRooms[num], this.defaultRooms[num].transform.position, this.defaultRooms[num].transform.rotation, this.defaultRooms[num].transform.parent));
		Flammable[] componentsInChildren2 = this.defaultRooms[num].GetComponentsInChildren<Flammable>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].CancelInvoke("Pulse");
		}
		this.defaultRooms[num].gameObject.SetActive(false);
		this.newRooms[num].gameObject.SetActive(true);
		Bonus[] componentsInChildren3 = this.newRooms[num].GetComponentsInChildren<Bonus>(true);
		if (componentsInChildren3 != null && componentsInChildren3.Length != 0)
		{
			Bonus[] array = componentsInChildren3;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateStatsManagerReference();
			}
		}
		this.newRooms[num].GetComponent<GoreZone>().isNewest = true;
		this.defaultRooms[num].transform.position = new Vector3(this.defaultRooms[num].transform.position.x + 10000f, this.defaultRooms[num].transform.position.y, this.defaultRooms[num].transform.position.z);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x000185E0 File Offset: 0x000167E0
	public void ReactivateCheckpoint()
	{
		this.activated = false;
		this.firstTime = false;
		this.inDuringResetSafety = false;
		this.ReactivationEffect();
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00018600 File Offset: 0x00016800
	public void ReactivationEffect()
	{
		if (!this.activated && this.graphic && !this.ShouldBeOff())
		{
			this.graphic.SetActive(true);
			ScaleTransform scaleTransform;
			if (this.graphic.TryGetComponent<ScaleTransform>(out scaleTransform))
			{
				this.graphic.transform.localScale = new Vector3(this.graphic.transform.localScale.x, 0f, this.graphic.transform.localScale.z);
			}
			AudioSource audioSource;
			if (this.graphic.TryGetComponent<AudioSource>(out audioSource))
			{
				audioSource.Play();
			}
			this.resetSafetyTimer = 0.5f;
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x000186AF File Offset: 0x000168AF
	public void ApplyCurrentStyleAndKills()
	{
		this.ApplyCurrentKills();
		this.ApplyCurrentStyle();
	}

	// Token: 0x060003CA RID: 970 RVA: 0x000186BD File Offset: 0x000168BD
	public void ApplyCurrentKills()
	{
		this.restartKills = 0;
	}

	// Token: 0x060003CB RID: 971 RVA: 0x000186C6 File Offset: 0x000168C6
	public void ApplyCurrentStyle()
	{
		this.stylePoints = this.sm.stylePoints;
	}

	// Token: 0x060003CC RID: 972 RVA: 0x000186D9 File Offset: 0x000168D9
	public void AddCustomKill()
	{
		MonoSingleton<StatsManager>.Instance.kills++;
		this.restartKills++;
	}

	// Token: 0x060003CD RID: 973 RVA: 0x000186FB File Offset: 0x000168FB
	public void ChangeSpawnRotation(float degrees)
	{
		this.additionalSpawnRotation = degrees;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00018704 File Offset: 0x00016904
	public void SetInvisibility(bool state)
	{
		this.invisible = state;
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001870D File Offset: 0x0001690D
	public void SetForceOff(bool state)
	{
		this.forceOff = state;
	}

	// Token: 0x04000491 RID: 1169
	[HideInInspector]
	public StatsManager sm;

	// Token: 0x04000492 RID: 1170
	[HideInInspector]
	public bool activated;

	// Token: 0x04000493 RID: 1171
	private bool firstTime = true;

	// Token: 0x04000494 RID: 1172
	public GameObject graphic;

	// Token: 0x04000495 RID: 1173
	public AssetReference activateEffect;

	// Token: 0x04000496 RID: 1174
	[Required]
	public GameObject toActivate;

	// Token: 0x04000497 RID: 1175
	[Header("Targets")]
	public bool inheritAllRooms;

	// Token: 0x04000498 RID: 1176
	public bool unlockAllDoors;

	// Token: 0x04000499 RID: 1177
	public GameObject[] rooms;

	// Token: 0x0400049A RID: 1178
	public List<GameObject> roomsToInherit = new List<GameObject>();

	// Token: 0x0400049B RID: 1179
	private List<string> inheritNames = new List<string>();

	// Token: 0x0400049C RID: 1180
	private List<Transform> inheritParents = new List<Transform>();

	// Token: 0x0400049D RID: 1181
	[HideInInspector]
	public List<GameObject> defaultRooms = new List<GameObject>();

	// Token: 0x0400049E RID: 1182
	[HideInInspector]
	public List<GameObject> newRooms = new List<GameObject>();

	// Token: 0x0400049F RID: 1183
	public Door[] doorsToUnlock;

	// Token: 0x040004A0 RID: 1184
	public Door[] doorsToIgnore;

	// Token: 0x040004A1 RID: 1185
	private int i;

	// Token: 0x040004A2 RID: 1186
	private GameObject player;

	// Token: 0x040004A3 RID: 1187
	private NewMovement nm;

	// Token: 0x040004A4 RID: 1188
	private float tempRot;

	// Token: 0x040004A5 RID: 1189
	[HideInInspector]
	public int restartKills;

	// Token: 0x040004A6 RID: 1190
	[HideInInspector]
	public int stylePoints;

	// Token: 0x040004A7 RID: 1191
	[HideInInspector]
	public bool challengeAlreadyFailed;

	// Token: 0x040004A8 RID: 1192
	[HideInInspector]
	public bool challengeAlreadyDone;

	// Token: 0x040004A9 RID: 1193
	private StyleHUD shud;

	// Token: 0x040004AA RID: 1194
	[Header("Automatic Resets")]
	public bool resetOnGetOtherCheckpoint;

	// Token: 0x040004AB RID: 1195
	public bool resetOnDistance;

	// Token: 0x040004AC RID: 1196
	public float autoResetDistance = 15f;

	// Token: 0x040004AD RID: 1197
	private float resetSafetyTimer;

	// Token: 0x040004AE RID: 1198
	private bool inDuringResetSafety;

	// Token: 0x040004AF RID: 1199
	[Space]
	public bool startOff;

	// Token: 0x040004B0 RID: 1200
	public bool forceOff;

	// Token: 0x040004B1 RID: 1201
	public bool disableDuringCombat;

	// Token: 0x040004B2 RID: 1202
	public bool unteleportable;

	// Token: 0x040004B3 RID: 1203
	public bool invisible;

	// Token: 0x040004B4 RID: 1204
	[HideInInspector]
	public List<int> succesfulHitters = new List<int>();

	// Token: 0x040004B5 RID: 1205
	[Space]
	public UnityEvent onRestart;

	// Token: 0x040004B6 RID: 1206
	[HideInInspector]
	public float additionalSpawnRotation;
}
