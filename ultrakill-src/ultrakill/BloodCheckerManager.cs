using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000502 RID: 1282
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class BloodCheckerManager : MonoSingleton<BloodCheckerManager>
{
	// Token: 0x06001D44 RID: 7492 RVA: 0x000F51CC File Offset: 0x000F33CC
	public void HigherAccuracy(bool useHigherAccuracy)
	{
		this.higherAccuracy = useHigherAccuracy;
		foreach (List<BloodAbsorber> list in this.rooms.Values)
		{
			foreach (BloodAbsorber bloodAbsorber in list)
			{
				bloodAbsorber.ToggleHigherAccuracy(this.higherAccuracy);
			}
		}
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x000F5264 File Offset: 0x000F3464
	private void Start()
	{
		foreach (GameObject gameObject in this.trackedRooms)
		{
			BloodAbsorber[] componentsInChildren = gameObject.GetComponentsInChildren<BloodAbsorber>();
			this.roomGore.Add(gameObject, new HashSet<GoreSplatter>());
			this.roomGibs.Add(gameObject, new HashSet<EnemyIdentifierIdentifier>());
			BloodAbsorber[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].owningRoom = gameObject;
			}
			this.rooms.Add(gameObject, componentsInChildren.ToList<BloodAbsorber>());
		}
		Transform parent = this.painterGUITemplate.transform.parent;
		this.painterGUITemplate.SetActive(false);
		foreach (KeyValuePair<GameObject, List<BloodAbsorber>> keyValuePair in this.rooms)
		{
			int num = this.trackedRooms.IndexOf(keyValuePair.Key);
			Cubemap cubemap = this.cleanedMaps[num];
			foreach (BloodAbsorber bloodAbsorber in keyValuePair.Value)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.painterGUITemplate, parent);
				gameObject2.transform.GetChild(0).gameObject.SetActive(false);
				gameObject2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = bloodAbsorber.painterName;
				gameObject2.name = bloodAbsorber.painterName;
				this.toDoEntries.Add(bloodAbsorber, gameObject2);
				bloodAbsorber.cleanedMap = cubemap;
			}
		}
		this.pondToDoEntry = Object.Instantiate<GameObject>(this.painterGUITemplate, parent);
		this.pondToDoEntry.transform.GetChild(0).gameObject.SetActive(false);
		this.pondToDoEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Pond";
		this.pondToDoEntry.name = "Pond";
		this.washingCanvas.enabled = false;
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000F54A0 File Offset: 0x000F36A0
	private void RemoveNullLitters()
	{
		this.litterCheckIndex = (this.litterCheckIndex + 1) % this.rooms.Count;
		GameObject gameObject = this.trackedRooms[this.litterCheckIndex];
		this.roomGore[gameObject].RemoveWhere((GoreSplatter x) => x == null || !x.gameObject.activeInHierarchy);
		this.roomGibs[gameObject].RemoveWhere((EnemyIdentifierIdentifier x) => x == null || !x.gameObject.activeInHierarchy || x.transform.lossyScale == Vector3.zero);
		this.pondLitter.RemoveWhere((GameObject x) => x == null || !x.activeInHierarchy || x.transform.lossyScale == Vector3.zero);
		base.Invoke("RemoveNullLitters", 0.033f);
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000F5576 File Offset: 0x000F3776
	private void CheckLevelStates()
	{
		this.CheckLitterCounts();
		this.CheckLevelCompletion();
		base.Invoke("CheckLevelStates", 1f);
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x000F5594 File Offset: 0x000F3794
	private void CheckLitterCounts()
	{
		int[] array = new int[5];
		this.roomLitterCounts.CopyTo(array, 0);
		foreach (KeyValuePair<GameObject, HashSet<GoreSplatter>> keyValuePair in this.roomGore)
		{
			int num = this.trackedRooms.IndexOf(keyValuePair.Key);
			this.roomLitterCounts[num] = keyValuePair.Value.Count;
		}
		foreach (KeyValuePair<GameObject, HashSet<EnemyIdentifierIdentifier>> keyValuePair2 in this.roomGibs)
		{
			int num2 = this.trackedRooms.IndexOf(keyValuePair2.Key);
			this.roomLitterCounts[num2] += keyValuePair2.Value.Count;
		}
		int num3 = this.trackedRooms.IndexOf(this.pond.owningRoom);
		this.roomLitterCounts[num3] += this.pondLitter.Count;
		this.totalLitterCount = 0;
		for (int i = 0; i < this.roomLitterCounts.Length; i++)
		{
			this.roomLitterCounts[i] = Math.Max(0, this.roomLitterCounts[i] - this.roomLitterForgiveness[i]);
			if (this.roomLitterCounts[i] <= 0)
			{
				int num4 = array[i];
				GameObject gameObject = this.trackedRooms[i];
				foreach (GoreSplatter goreSplatter in this.roomGore[gameObject])
				{
					goreSplatter.Repool();
				}
				using (HashSet<EnemyIdentifierIdentifier>.Enumerator enumerator4 = this.roomGibs[gameObject].GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Collider collider;
						if (enumerator4.Current.TryGetComponent<Collider>(out collider))
						{
							GibDestroyer.LimbBegone(collider);
						}
					}
				}
				if (gameObject == this.pond.owningRoom)
				{
					foreach (GameObject gameObject2 in this.pondLitter)
					{
						GoreSplatter goreSplatter2;
						Collider collider2;
						if (gameObject2.TryGetComponent<GoreSplatter>(out goreSplatter2))
						{
							goreSplatter2.Repool();
						}
						else if (gameObject2.TryGetComponent<Collider>(out collider2))
						{
							GibDestroyer.LimbBegone(collider2);
						}
					}
				}
			}
			this.totalLitterCount += this.roomLitterCounts[i];
		}
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000F5850 File Offset: 0x000F3A50
	public void StartCheckingBlood()
	{
		if (!this.startedWashing)
		{
			this.startedWashing = true;
			foreach (List<BloodAbsorber> list in this.rooms.Values)
			{
				foreach (BloodAbsorber bloodAbsorber in list)
				{
					bloodAbsorber.StartCheckingFill();
				}
			}
			base.Invoke("RemoveNullLitters", 0.033f);
			base.Invoke("CheckLevelStates", 0.5f);
		}
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000F590C File Offset: 0x000F3B0C
	private void ToggleHigherAccuracy(bool isTrue)
	{
		foreach (List<BloodAbsorber> list in this.rooms.Values)
		{
			foreach (BloodAbsorber bloodAbsorber in list)
			{
				bloodAbsorber.ToggleHigherAccuracy(isTrue);
			}
		}
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000F5998 File Offset: 0x000F3B98
	private void Update()
	{
		if (!this.startedWashing)
		{
			return;
		}
		this.washingCanvas.enabled = false;
		Transform transform = MonoSingleton<CameraController>.Instance.transform;
		bool flag = false;
		RaycastHit raycastHit;
		if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 50f, LayerMaskDefaults.Get(LMD.Environment) | 16, QueryTriggerInteraction.Collide))
		{
			Pond pond;
			if (raycastHit.transform.gameObject.layer != 4)
			{
				BloodAbsorber bloodGroup;
				raycastHit.transform.TryGetComponent<BloodAbsorber>(out bloodGroup);
				BloodAbsorberChild bloodAbsorberChild;
				if (bloodGroup == null && raycastHit.transform.TryGetComponent<BloodAbsorberChild>(out bloodAbsorberChild))
				{
					bloodGroup = bloodAbsorberChild.bloodGroup;
				}
				if (bloodGroup != null)
				{
					this.UpdateDisplay(bloodGroup);
					flag = true;
				}
			}
			else if (raycastHit.transform.TryGetComponent<Pond>(out pond))
			{
				flag = true;
				this.UpdateDisplay(null);
			}
		}
		if (!flag && this.playerInPond)
		{
			this.UpdateDisplay(null);
			return;
		}
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x000F5A74 File Offset: 0x000F3C74
	private void CheckLevelCompletion()
	{
		bool flag = this.totalLitterCount <= 0;
		foreach (GameObject gameObject in this.trackedRooms)
		{
			flag &= this.IsRoomCompleted(gameObject);
		}
		if (flag)
		{
			this.finalDoorOpener.SetActive(true);
		}
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000F5AE8 File Offset: 0x000F3CE8
	private bool IsRoomCompleted(GameObject roomToCheck)
	{
		bool flag = true;
		foreach (BloodAbsorber bloodAbsorber in this.rooms[roomToCheck])
		{
			this.toDoEntries[bloodAbsorber].transform.GetChild(0).gameObject.SetActive(bloodAbsorber.isCompleted);
			flag &= bloodAbsorber.isCompleted;
		}
		if (roomToCheck == this.pond.owningRoom)
		{
			bool flag2 = this.pond.bloodFillAmount <= 0.001f;
			this.pondToDoEntry.transform.GetChild(0).gameObject.SetActive(flag2);
			flag = flag && flag2;
		}
		int num = this.trackedRooms.IndexOf(roomToCheck);
		flag &= this.roomLitterCounts[num] == 0;
		this.completedRoomStates[num].SetActive(flag);
		this.roomCompletions[num] = flag;
		return flag;
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000F5BF0 File Offset: 0x000F3DF0
	public void StoreBlood()
	{
		foreach (List<BloodAbsorber> list in this.rooms.Values)
		{
			foreach (BloodAbsorber bloodAbsorber in list)
			{
				bloodAbsorber.StoreBloodCopy();
			}
		}
		this.pond.StoreBlood();
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000F5C84 File Offset: 0x000F3E84
	public void RestoreBlood()
	{
		foreach (List<BloodAbsorber> list in this.rooms.Values)
		{
			foreach (BloodAbsorber bloodAbsorber in list)
			{
				bloodAbsorber.RestoreBloodCopy();
			}
		}
		this.pond.RestoreBlood();
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x000F5D18 File Offset: 0x000F3F18
	public void UpdateDisplay(BloodAbsorber bA)
	{
		this.washingCanvas.enabled = !HideUI.Active;
		GameObject gameObject = null;
		if (bA != null)
		{
			if (bA.painterName != this.activePainterName)
			{
				this.activePainter.text = bA.painterName;
			}
			gameObject = bA.owningRoom;
		}
		else
		{
			gameObject = this.pond.owningRoom;
			this.activePainter.text = "Pond";
		}
		if (gameObject == null)
		{
			Debug.LogError("No room found on UpdateDisplay");
		}
		this.roomName.SetText(gameObject.name, true);
		this.pondToDoEntry.SetActive(false);
		foreach (GameObject gameObject2 in this.toDoEntries.Values)
		{
			gameObject2.SetActive(false);
		}
		int num = this.trackedRooms.IndexOf(gameObject);
		if (this.roomCompletions[num])
		{
			this.litterCount.transform.parent.gameObject.SetActive(false);
			this.cleanText.gameObject.SetActive(true);
			this.toDoText.gameObject.SetActive(false);
			return;
		}
		this.litterCount.transform.parent.gameObject.SetActive(true);
		this.toDoText.gameObject.SetActive(true);
		int num2 = this.roomLitterCounts[num];
		this.litterCount.text = num2.ToString();
		foreach (BloodAbsorber bloodAbsorber in this.rooms[gameObject])
		{
			this.toDoEntries[bloodAbsorber].SetActive(true);
		}
		if (gameObject == this.pond.owningRoom)
		{
			this.pondToDoEntry.SetActive(true);
		}
		if (bA == null)
		{
			if (this.pond.bloodFillAmount <= 0.001f)
			{
				this.activePercentSlider.value = 100f;
				return;
			}
			this.activePercentSlider.value = (1f - this.pond.bloodFillAmount) * 100f;
			return;
		}
		else
		{
			if (bA.isCompleted)
			{
				this.activePercentSlider.value = 100f;
				return;
			}
			float num3 = (1f - bA.fillAmount) * 100f;
			this.activePercentSlider.value = num3;
			return;
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x000F5FA0 File Offset: 0x000F41A0
	public void AddPondGore(GoreSplatter litter)
	{
		this.pondLitter.Add(litter.gameObject);
		foreach (HashSet<GoreSplatter> hashSet in this.roomGore.Values)
		{
			hashSet.Remove(litter);
		}
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x000F600C File Offset: 0x000F420C
	public void AddPondGib(EnemyIdentifierIdentifier litter)
	{
		this.pondLitter.Add(litter.gameObject);
		foreach (HashSet<EnemyIdentifierIdentifier> hashSet in this.roomGibs.Values)
		{
			hashSet.Remove(litter);
		}
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x000F6078 File Offset: 0x000F4278
	public void AddGoreToRoom(BloodAbsorber absorber, GoreSplatter litter)
	{
		this.pondLitter.Remove(litter.gameObject);
		foreach (HashSet<GoreSplatter> hashSet in this.roomGore.Values)
		{
			hashSet.Remove(litter);
		}
		this.roomGore[absorber.owningRoom].Add(litter);
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x000F60FC File Offset: 0x000F42FC
	public void AddGibToRoom(BloodAbsorber absorber, EnemyIdentifierIdentifier litter)
	{
		this.pondLitter.Remove(litter.gameObject);
		foreach (HashSet<EnemyIdentifierIdentifier> hashSet in this.roomGibs.Values)
		{
			hashSet.Remove(litter);
		}
		this.roomGibs[absorber.owningRoom].Add(litter);
	}

	// Token: 0x0400297B RID: 10619
	public Canvas washingCanvas;

	// Token: 0x0400297C RID: 10620
	public GameObject painterGUITemplate;

	// Token: 0x0400297D RID: 10621
	public TextMeshProUGUI roomName;

	// Token: 0x0400297E RID: 10622
	public TextMeshProUGUI activePainter;

	// Token: 0x0400297F RID: 10623
	public TextMeshProUGUI toDoText;

	// Token: 0x04002980 RID: 10624
	public TextMeshProUGUI cleanText;

	// Token: 0x04002981 RID: 10625
	public TextMeshProUGUI litterCount;

	// Token: 0x04002982 RID: 10626
	public Slider activePercentSlider;

	// Token: 0x04002983 RID: 10627
	private string activePainterName;

	// Token: 0x04002984 RID: 10628
	public GameObject finalDoorOpener;

	// Token: 0x04002985 RID: 10629
	public List<GameObject> trackedRooms = new List<GameObject>();

	// Token: 0x04002986 RID: 10630
	public int[] roomLitterForgiveness = new int[5];

	// Token: 0x04002987 RID: 10631
	private Dictionary<GameObject, List<BloodAbsorber>> rooms = new Dictionary<GameObject, List<BloodAbsorber>>();

	// Token: 0x04002988 RID: 10632
	private Dictionary<GameObject, HashSet<GoreSplatter>> roomGore = new Dictionary<GameObject, HashSet<GoreSplatter>>();

	// Token: 0x04002989 RID: 10633
	private Dictionary<GameObject, HashSet<EnemyIdentifierIdentifier>> roomGibs = new Dictionary<GameObject, HashSet<EnemyIdentifierIdentifier>>();

	// Token: 0x0400298A RID: 10634
	public Dictionary<BloodAbsorber, GameObject> toDoEntries = new Dictionary<BloodAbsorber, GameObject>();

	// Token: 0x0400298B RID: 10635
	public Cubemap[] cleanedMaps = new Cubemap[5];

	// Token: 0x0400298C RID: 10636
	private GameObject pondToDoEntry;

	// Token: 0x0400298D RID: 10637
	public Pond pond;

	// Token: 0x0400298E RID: 10638
	public HashSet<GameObject> pondLitter = new HashSet<GameObject>();

	// Token: 0x0400298F RID: 10639
	public bool startedWashing;

	// Token: 0x04002990 RID: 10640
	private int litterCheckIndex;

	// Token: 0x04002991 RID: 10641
	public int[] roomLitterCounts = new int[5];

	// Token: 0x04002992 RID: 10642
	public bool[] roomCompletions = new bool[5];

	// Token: 0x04002993 RID: 10643
	private int totalLitterCount = 999;

	// Token: 0x04002994 RID: 10644
	public List<GameObject> completedRoomStates = new List<GameObject>();

	// Token: 0x04002995 RID: 10645
	[HideInInspector]
	public bool playerInPond;

	// Token: 0x04002996 RID: 10646
	public bool higherAccuracy;
}
