using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// Token: 0x0200011D RID: 285
public class Door : MonoBehaviour
{
	// Token: 0x06000538 RID: 1336 RVA: 0x000229F8 File Offset: 0x00020BF8
	private void Awake()
	{
		this.block = new MaterialPropertyBlock();
		this.lightsMeshRenderers = base.GetComponentsInChildren<MeshRenderer>();
		this.nmo = base.GetComponent<NavMeshObstacle>();
		this.occpor = base.GetComponent<OcclusionPortal>();
		if (!this.occpor)
		{
			this.occpor = base.GetComponentInChildren<OcclusionPortal>();
		}
		if (this.requests == 1)
		{
			this.requests = 0;
			this.startOpen = true;
		}
		if (this.nmo != null && (this.startOpen || this.open))
		{
			this.nmo.enabled = false;
		}
		if (this.doorType == DoorType.Normal)
		{
			this.aud = base.GetComponent<AudioSource>();
			if (this.useRigidbody)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			if (!this.gotPos)
			{
				this.GetPos();
			}
			else if (this.openPosRelative == Vector3.zero)
			{
				this.openPosRelative = base.transform.localPosition + this.openPos;
			}
		}
		else if (this.doorType == DoorType.BigDoorController)
		{
			this.bdoors = base.GetComponentsInChildren<BigDoor>(true);
			if (this.startOpen && !this.open)
			{
				BigDoor[] array = this.bdoors;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].open = true;
				}
			}
		}
		else if (this.doorType == DoorType.SubDoorController)
		{
			this.subdoors = base.GetComponentsInChildren<SubDoor>(true);
			if (this.startOpen)
			{
				foreach (SubDoor subDoor in this.subdoors)
				{
					subDoor.SetValues();
					if (subDoor.transform.localPosition == subDoor.origPos)
					{
						subDoor.transform.localPosition = subDoor.origPos + subDoor.openPos;
						subDoor.Open();
					}
				}
			}
		}
		if (this.noPass != null)
		{
			this.aud2 = base.transform.GetChild(0).GetComponent<AudioSource>();
			if (!this.aud2)
			{
				this.aud2 = base.GetComponentInChildren<AudioSource>();
			}
		}
		if (this.openLight != null && !this.startOpen)
		{
			this.openLight.enabled = false;
		}
		DoorLock[] componentsInChildren = base.GetComponentsInChildren<DoorLock>();
		if (componentsInChildren.Length != 0)
		{
			foreach (DoorLock doorLock in componentsInChildren)
			{
				if (doorLock.gameObject == base.gameObject)
				{
					this.thisLock = doorLock;
				}
				else
				{
					this.locks.Add(doorLock);
					doorLock.parentDoor = this;
				}
			}
		}
		if (this.doorType == DoorType.BigDoorController || this.doorType == DoorType.SubDoorController)
		{
			this.docons = base.GetComponentsInChildren<DoorController>();
		}
		else if (base.transform.parent != null)
		{
			this.docons = base.transform.parent.GetComponentsInChildren<DoorController>();
		}
		if (this.docons == null)
		{
			this.docons = new DoorController[0];
		}
		if (((componentsInChildren.Length != 0 && this.thisLock == null) || componentsInChildren.Length > 1) && this.docons.Length != 0)
		{
			for (int j = 0; j < this.docons.Length; j++)
			{
				if (this.docons[j].gameObject.activeInHierarchy)
				{
					this.origDoconStates.Add(true);
				}
				else
				{
					this.origDoconStates.Add(false);
				}
				this.docons[j].gameObject.SetActive(false);
			}
		}
		else if ((this.docons == null || this.docons.Length == 0) && !LayerMaskDefaults.IsMatchingLayer(base.gameObject.layer, LMD.Environment))
		{
			this.doconlessClosingCol = base.GetComponent<Collider>();
			if (this.doconlessClosingCol && !this.doconlessClosingCol.isTrigger)
			{
				this.doconless = true;
				if (!this.startOpen && !this.open && (this.doorType != DoorType.BigDoorController || this.bdoors.Length != 0) && (this.doorType != DoorType.SubDoorController || this.subdoors.Length != 0))
				{
					this.doconlessClosingCol.enabled = true;
				}
				else
				{
					this.doconlessClosingCol.enabled = false;
				}
			}
		}
		foreach (MeshRenderer meshRenderer in this.lightsMeshRenderers)
		{
			if (meshRenderer && meshRenderer.sharedMaterial && meshRenderer.sharedMaterial.IsKeywordEnabled("EMISSIVE"))
			{
				meshRenderer.GetPropertyBlock(this.block);
				this.defaultLightsColor = meshRenderer.sharedMaterial.GetColor(UKShaderProperties.EmissiveColor);
				if (this.noPass != null && this.noPass.activeInHierarchy)
				{
					this.block.SetColor(UKShaderProperties.EmissiveColor, this.turnEmissiveOffWhenLocked ? new Color(0f, 0f, 0f, 0f) : Color.red);
				}
				else if (this.locked)
				{
					this.locked = false;
					this.Lock();
				}
				meshRenderer.SetPropertyBlock(this.block);
			}
		}
		this.currentLightsColor = this.defaultLightsColor;
		this.gotValues = true;
		if (this.startOpen || this.open)
		{
			this.open = true;
			if (this.occpor)
			{
				this.occpor.open = true;
				return;
			}
		}
		else if (this.occpor)
		{
			this.occpor.open = false;
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00022F50 File Offset: 0x00021150
	private void GetPos()
	{
		this.gotPos = true;
		this.closedPos = base.transform.localPosition;
		this.openPosRelative = base.transform.localPosition + this.openPos;
		if (this.startOpen && this.doorType == DoorType.Normal)
		{
			base.transform.localPosition = this.openPosRelative;
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00022FB4 File Offset: 0x000211B4
	public void AltarControlled()
	{
		this.doconlessClosingCol = base.GetComponent<Collider>();
		foreach (DoorController doorController in this.docons)
		{
			if (!doorController.dontDeactivateOnAltarControl)
			{
				doorController.gameObject.SetActive(false);
			}
		}
		if (this.doconlessClosingCol && !this.doconlessClosingCol.isTrigger)
		{
			this.doconless = true;
			if (!this.startOpen)
			{
				this.doconlessClosingCol.enabled = true;
			}
		}
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00023030 File Offset: 0x00021230
	private void Update()
	{
		if (this.doorType == DoorType.Normal && !this.inPos)
		{
			if (!this.useRigidbody || !this.rb)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.targetPos, Time.deltaTime * (this.easeIn ? Mathf.Min(this.speed, Vector3.Distance(base.transform.localPosition, this.targetPos) / Mathf.Min(Vector3.Distance(this.closedPos, this.openPos), 100f) * 5f * this.speed + this.speed / 10f) : this.speed));
			}
			if (this.screenShake)
			{
				if (this.cc == null)
				{
					this.cc = MonoSingleton<CameraController>.Instance;
				}
				this.cc.CameraShake(0.05f);
			}
			if (Vector3.Distance(base.transform.localPosition, this.targetPos) < Mathf.Min(0.1f, Vector3.Distance(this.closedPos, this.openPos) / 100f))
			{
				base.transform.localPosition = this.targetPos;
				this.inPos = true;
				if (base.transform.localPosition == this.openPosRelative)
				{
					UnityEvent unityEvent = this.onFullyOpened;
					if (unityEvent != null)
					{
						unityEvent.Invoke();
					}
				}
				else
				{
					UnityEvent unityEvent2 = this.onFullyClosed;
					if (unityEvent2 != null)
					{
						unityEvent2.Invoke();
					}
					if (this.openLight)
					{
						this.openLight.enabled = false;
					}
				}
				if (this.closeSound != null && this.aud)
				{
					this.aud.clip = this.closeSound;
					this.aud.loop = false;
					this.aud.Play();
				}
				if (this.nmo != null)
				{
					if (base.transform.localPosition == this.closedPos)
					{
						this.nmo.enabled = true;
					}
					else
					{
						this.nmo.enabled = false;
					}
				}
				if (this.occpor && base.transform.localPosition == this.closedPos)
				{
					this.occpor.open = false;
				}
			}
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00023288 File Offset: 0x00021488
	private void FixedUpdate()
	{
		if (this.doorType == DoorType.Normal && !this.inPos && this.useRigidbody && this.rb)
		{
			Vector3 vector = (base.transform.parent ? base.transform.parent.TransformPoint(this.targetPos) : this.targetPos);
			this.rb.MovePosition(Vector3.MoveTowards(this.rb.position, vector, Time.fixedDeltaTime * this.speed));
		}
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00023313 File Offset: 0x00021513
	public void SimpleOpenOverride()
	{
		this.Open(false, true);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00023320 File Offset: 0x00021520
	public void Open(bool enemy = false, bool skull = false)
	{
		if (!this.gotPos)
		{
			this.GetPos();
		}
		if (!skull || this.docons.Length == 0)
		{
			this.requests++;
		}
		else if (skull && this.docons.Length != 0)
		{
			bool flag = false;
			DoorController[] array = this.docons;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.activeInHierarchy)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.requests++;
			}
		}
		if (this.activatedRooms.Length != 0)
		{
			foreach (GameObject gameObject in this.activatedRooms)
			{
				if (gameObject)
				{
					gameObject.SetActive(true);
				}
			}
		}
		if ((!this.open || skull) && (base.transform.localPosition != this.openPosRelative || this.doorType != DoorType.Normal))
		{
			if (!this.gotValues)
			{
				this.startOpen = true;
				return;
			}
			this.open = true;
			if (this.occpor)
			{
				this.occpor.open = true;
			}
			if (!enemy && !this.dontCloseOtherDoorsWhenOpening && this.docons.Length != 0)
			{
				this.allDoors = Object.FindObjectsOfType<Door>();
				foreach (Door door in this.allDoors)
				{
					if (door != null && door != this && door.open && !door.startOpen && !door.dontCloseWhenAnotherDoorOpens)
					{
						DoorController doorController = null;
						if (door.doorType != DoorType.Normal)
						{
							doorController = door.GetComponentInChildren<DoorController>();
						}
						else if (door.transform.parent != null)
						{
							doorController = door.transform.parent.GetComponentInChildren<DoorController>();
						}
						if (doorController != null && doorController.type == 0 && !doorController.enemyIn)
						{
							door.Close(false);
						}
					}
				}
			}
			if (this.doorType == DoorType.Normal)
			{
				if (this.aud == null)
				{
					this.aud = base.GetComponent<AudioSource>();
				}
				if (this.aud)
				{
					this.aud.clip = this.openSound;
					if (this.closeSound != null)
					{
						this.aud.loop = true;
					}
					this.aud.Play();
				}
				this.targetPos = this.openPosRelative;
				this.inPos = false;
			}
			else
			{
				if (this.doorType == DoorType.BigDoorController)
				{
					foreach (BigDoor bigDoor in this.bdoors)
					{
						if (this.reverseDirection)
						{
							bigDoor.reverseDirection = true;
						}
						else
						{
							bigDoor.reverseDirection = false;
						}
						bigDoor.Open();
					}
				}
				else if (this.doorType == DoorType.SubDoorController)
				{
					foreach (SubDoor subDoor in this.subdoors)
					{
						if (!subDoor.dr)
						{
							subDoor.dr = this;
						}
						subDoor.Open();
					}
				}
				if (this.nmo)
				{
					this.nmo.enabled = false;
				}
			}
			if (this.openLight != null)
			{
				this.openLight.enabled = true;
			}
			if (this.thisLock != null)
			{
				this.thisLock.Open();
			}
			if (this.doconless && this.doconlessClosingCol)
			{
				this.doconlessClosingCol.enabled = false;
			}
		}
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00023688 File Offset: 0x00021888
	public void Optimize()
	{
		if (this.deactivatedRooms.Length != 0)
		{
			GameObject[] array = this.deactivatedRooms;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x000236BC File Offset: 0x000218BC
	public void Close(bool force = false)
	{
		if (!this.gotPos)
		{
			this.GetPos();
		}
		if (this.requests > 1 && !force)
		{
			this.requests--;
			return;
		}
		if (base.transform.localPosition != this.closedPos || this.doorType != DoorType.Normal)
		{
			this.open = false;
			if (this.requests > 0 && !force)
			{
				this.requests--;
			}
			else if (force)
			{
				this.requests = 0;
			}
			if (this.startOpen)
			{
				this.startOpen = false;
			}
			if (this.doorType == DoorType.Normal)
			{
				if (this.aud == null)
				{
					this.aud = base.GetComponent<AudioSource>();
				}
				if (this.aud != null)
				{
					this.aud.clip = this.openSound;
					if (this.closeSound != null)
					{
						this.aud.loop = true;
					}
					this.aud.Play();
				}
				if (this.doorType == DoorType.Normal)
				{
					this.targetPos = this.closedPos;
				}
				this.inPos = false;
			}
			else if (this.doorType == DoorType.BigDoorController && this.bdoors != null)
			{
				foreach (BigDoor bigDoor in this.bdoors)
				{
					bigDoor.Close();
					if (this.openLight != null)
					{
						bigDoor.openLight = this.openLight;
					}
				}
				if (this.nmo)
				{
					this.nmo.enabled = true;
				}
			}
			else if (this.doorType == DoorType.SubDoorController && this.subdoors != null)
			{
				foreach (SubDoor subDoor in this.subdoors)
				{
					if (!subDoor.dr)
					{
						subDoor.dr = this;
					}
					subDoor.Close();
				}
				if (this.nmo)
				{
					this.nmo.enabled = true;
				}
			}
			if (this.thisLock != null)
			{
				this.thisLock.Close();
			}
			if (this.doconless && this.doconlessClosingCol)
			{
				this.doconlessClosingCol.enabled = true;
			}
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x000238D8 File Offset: 0x00021AD8
	public void Lock()
	{
		if (!this.locked)
		{
			this.locked = true;
			if (this.noPass)
			{
				this.noPass.SetActive(true);
			}
			if (this.doorType == DoorType.Normal)
			{
				if (base.transform.localPosition != this.closedPos)
				{
					this.Close(true);
				}
			}
			else if (this.doorType == DoorType.BigDoorController)
			{
				BigDoor[] array = this.bdoors;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].open)
					{
						this.Close(true);
						break;
					}
				}
			}
			else if (this.doorType == DoorType.SubDoorController)
			{
				SubDoor[] array2 = this.subdoors;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].isOpen)
					{
						this.Close(true);
						break;
					}
				}
			}
			if (this.aud2)
			{
				this.aud2.pitch = 0.2f;
				this.aud2.Play();
			}
			this.ChangeColor(this.turnEmissiveOffWhenLocked ? new Color(0f, 0f, 0f, 0f) : Color.red);
		}
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x000239F4 File Offset: 0x00021BF4
	public void Unlock()
	{
		if (this.locked && this.aud2)
		{
			this.aud2.pitch = 0.5f;
			this.aud2.Play();
		}
		this.locked = false;
		if (this.noPass)
		{
			this.noPass.SetActive(false);
		}
		this.ChangeColor(this.defaultLightsColor);
		if (this.openOnUnlock && !this.open)
		{
			this.Open(false, false);
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00023A78 File Offset: 0x00021C78
	public void ChangeColor(Color targetColor)
	{
		this.currentLightsColor = targetColor;
		if (this.lightsMeshRenderers != null && this.lightsMeshRenderers.Length != 0)
		{
			foreach (MeshRenderer meshRenderer in this.lightsMeshRenderers)
			{
				if (meshRenderer && meshRenderer.sharedMaterial.HasProperty(UKShaderProperties.EmissiveColor))
				{
					meshRenderer.GetPropertyBlock(this.block);
					this.block.SetColor(UKShaderProperties.EmissiveColor, targetColor);
					meshRenderer.SetPropertyBlock(this.block);
				}
			}
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00023AFC File Offset: 0x00021CFC
	public void LockOpen()
	{
		this.openLocks++;
		if (this.openLocks == this.locks.Count)
		{
			if (this.docons.Length != 0)
			{
				for (int i = 0; i < this.docons.Length; i++)
				{
					if (this.origDoconStates[i])
					{
						this.docons[i].gameObject.SetActive(true);
					}
				}
			}
			this.Open(false, true);
		}
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00023B6F File Offset: 0x00021D6F
	public void LockClose()
	{
		this.openLocks--;
		if (this.openLocks == this.locks.Count - 1)
		{
			this.Close(true);
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x00023B9C File Offset: 0x00021D9C
	public void BigDoorClosed()
	{
		UnityEvent unityEvent = this.onFullyClosed;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.occpor)
		{
			this.occpor.open = false;
		}
		if (this.openLight)
		{
			this.openLight.enabled = false;
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00023BEC File Offset: 0x00021DEC
	public void ForceStartOpen(bool force = true)
	{
		this.startOpen = force;
	}

	// Token: 0x04000729 RID: 1833
	public DoorType doorType;

	// Token: 0x0400072A RID: 1834
	private BigDoor[] bdoors;

	// Token: 0x0400072B RID: 1835
	private SubDoor[] subdoors;

	// Token: 0x0400072C RID: 1836
	public bool useRigidbody;

	// Token: 0x0400072D RID: 1837
	private Rigidbody rb;

	// Token: 0x0400072E RID: 1838
	public bool open;

	// Token: 0x0400072F RID: 1839
	public bool gotValues;

	// Token: 0x04000730 RID: 1840
	public bool gotPos;

	// Token: 0x04000731 RID: 1841
	public Vector3 closedPos;

	// Token: 0x04000732 RID: 1842
	public Vector3 openPos;

	// Token: 0x04000733 RID: 1843
	[HideInInspector]
	public Vector3 openPosRelative;

	// Token: 0x04000734 RID: 1844
	public bool startOpen;

	// Token: 0x04000735 RID: 1845
	[HideInInspector]
	public Vector3 targetPos;

	// Token: 0x04000736 RID: 1846
	public float speed;

	// Token: 0x04000737 RID: 1847
	public bool easeIn;

	// Token: 0x04000738 RID: 1848
	[HideInInspector]
	public bool inPos = true;

	// Token: 0x04000739 RID: 1849
	public bool reverseDirection;

	// Token: 0x0400073A RID: 1850
	public int requests;

	// Token: 0x0400073B RID: 1851
	private AudioSource aud;

	// Token: 0x0400073C RID: 1852
	public AudioClip openSound;

	// Token: 0x0400073D RID: 1853
	public AudioClip closeSound;

	// Token: 0x0400073E RID: 1854
	private AudioSource aud2;

	// Token: 0x0400073F RID: 1855
	private MaterialPropertyBlock block;

	// Token: 0x04000740 RID: 1856
	public bool locked;

	// Token: 0x04000741 RID: 1857
	public GameObject noPass;

	// Token: 0x04000742 RID: 1858
	private NavMeshObstacle nmo;

	// Token: 0x04000743 RID: 1859
	public GameObject[] activatedRooms;

	// Token: 0x04000744 RID: 1860
	public GameObject[] deactivatedRooms;

	// Token: 0x04000745 RID: 1861
	public Light openLight;

	// Token: 0x04000746 RID: 1862
	public UnityEvent onFullyOpened;

	// Token: 0x04000747 RID: 1863
	public UnityEvent onFullyClosed;

	// Token: 0x04000748 RID: 1864
	private Door[] allDoors;

	// Token: 0x04000749 RID: 1865
	public bool screenShake;

	// Token: 0x0400074A RID: 1866
	public bool dontCloseWhenAnotherDoorOpens;

	// Token: 0x0400074B RID: 1867
	public bool dontCloseOtherDoorsWhenOpening;

	// Token: 0x0400074C RID: 1868
	private CameraController cc;

	// Token: 0x0400074D RID: 1869
	private DoorLock thisLock;

	// Token: 0x0400074E RID: 1870
	private List<DoorLock> locks = new List<DoorLock>();

	// Token: 0x0400074F RID: 1871
	[HideInInspector]
	public int openLocks;

	// Token: 0x04000750 RID: 1872
	[HideInInspector]
	public DoorController[] docons;

	// Token: 0x04000751 RID: 1873
	[HideInInspector]
	public List<bool> origDoconStates = new List<bool>();

	// Token: 0x04000752 RID: 1874
	private bool doconless;

	// Token: 0x04000753 RID: 1875
	private Collider doconlessClosingCol;

	// Token: 0x04000754 RID: 1876
	private MeshRenderer[] lightsMeshRenderers;

	// Token: 0x04000755 RID: 1877
	public Color defaultLightsColor;

	// Token: 0x04000756 RID: 1878
	public Color currentLightsColor;

	// Token: 0x04000757 RID: 1879
	public bool turnEmissiveOffWhenLocked;

	// Token: 0x04000758 RID: 1880
	private OcclusionPortal occpor;

	// Token: 0x04000759 RID: 1881
	public bool ignoreHookCheck;

	// Token: 0x0400075A RID: 1882
	public bool openOnUnlock;
}
