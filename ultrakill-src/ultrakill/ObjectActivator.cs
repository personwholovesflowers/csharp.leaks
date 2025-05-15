using System;
using System.Linq;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class ObjectActivator : MonoBehaviour
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06001250 RID: 4688 RVA: 0x000934A2 File Offset: 0x000916A2
	private bool canUseEvents
	{
		get
		{
			return !DisableEnemySpawns.DisableArenaTriggers || !this.dontUseEventsIfEnemiesDisabled;
		}
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x000934B8 File Offset: 0x000916B8
	private void Start()
	{
		if (!this.dontActivateOnEnable && base.GetComponent<Collider>() == null && base.GetComponent<Rigidbody>() == null)
		{
			this.nonCollider = true;
			if ((!this.obac || this.obac.readyToActivate) && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead) && (!this.oneTime || (!this.activating && !this.activated)))
			{
				base.Invoke("Activate", this.delay);
			}
		}
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x00093548 File Offset: 0x00091748
	private void Update()
	{
		if ((this.nonCollider || this.playerIn > 0) && !this.activating && !this.activated && this.obac && this.obac.readyToActivate && !this.onlyCheckObacOnce && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.activating = true;
			base.Invoke("Activate", this.delay);
		}
		if (this.disableIfObacOff && this.activated && this.obac && !this.obac.readyToActivate)
		{
			this.Deactivate();
		}
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x000935F4 File Offset: 0x000917F4
	private void OnTriggerEnter(Collider other)
	{
		if (this.ignoreColliders != null && this.ignoreColliders.Contains(other))
		{
			return;
		}
		if ((this.forEnemies && other.gameObject.CompareTag("Enemy")) || (!this.forEnemies && other.gameObject.CompareTag("Player")))
		{
			this.playerIn++;
		}
		if (((!this.forEnemies && (!this.oneTime || (!this.activating && !this.activated)) && other.gameObject.CompareTag("Player")) || (this.forEnemies && !this.activating && !this.activated && other.gameObject.CompareTag("Enemy"))) && this.playerIn == 1 && (!this.obac || this.obac.readyToActivate) && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead))
		{
			if (this.oneTime)
			{
				this.activating = true;
			}
			base.Invoke("Activate", this.delay);
		}
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x00093710 File Offset: 0x00091910
	private void OnTriggerExit(Collider other)
	{
		if (this.ignoreColliders != null && this.ignoreColliders.Contains(other))
		{
			return;
		}
		if ((this.forEnemies && other.gameObject.CompareTag("Enemy")) || (!this.forEnemies && other.gameObject.CompareTag("Player")))
		{
			this.playerIn--;
		}
		if (this.disableOnExit && ((!this.forEnemies && (this.activating || this.activated) && other.gameObject.CompareTag("Player") && this.playerIn == 0) || (this.forEnemies && (this.activating || this.activated) && other.gameObject.CompareTag("Enemy"))) && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.Deactivate();
		}
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0000A726 File Offset: 0x00008926
	public void ActivateDelayed(float delay)
	{
		base.Invoke("Activate", delay);
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x000937F4 File Offset: 0x000919F4
	public void Activate()
	{
		this.Activate(false);
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00093800 File Offset: 0x00091A00
	public void Activate(bool ignoreDisabled = false)
	{
		if ((base.gameObject.activeSelf || ignoreDisabled) && (!this.activated || !this.oneTime) && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead) && (!this.notIfEnemiesDisabled || !DisableEnemySpawns.DisableArenaTriggers) && (!this.obac || this.obac.readyToActivate))
		{
			this.activating = false;
			this.activated = true;
			if (this.canUseEvents)
			{
				this.events.Invoke("");
			}
		}
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x0009388F File Offset: 0x00091A8F
	public void Deactivate()
	{
		if (!this.oneTime)
		{
			this.activated = false;
			this.activating = false;
		}
		if (this.canUseEvents)
		{
			this.events.Revert();
		}
		base.CancelInvoke("Activate");
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x000938C5 File Offset: 0x00091AC5
	public void Switch()
	{
		if (this.activated)
		{
			this.Deactivate();
			return;
		}
		this.Activate();
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x000938DC File Offset: 0x00091ADC
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		this.activating = false;
		this.playerIn = 0;
		base.CancelInvoke("Activate");
		if ((!this.activated || !this.oneTime) && this.activateOnDisable && (!this.notIfEnemiesDisabled || !DisableEnemySpawns.DisableArenaTriggers) && (!this.obac || this.obac.readyToActivate) && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.Activate(true);
			return;
		}
		if (this.activated && this.nonCollider && this.disableOnExit && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.Deactivate();
		}
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x000939A8 File Offset: 0x00091BA8
	private void OnEnable()
	{
		if ((!this.activated || this.reactivateOnEnable) && this.nonCollider && (!this.obac || this.obac.readyToActivate) && (!this.onlyIfPlayerIsAlive || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.activating = true;
			base.Invoke("Activate", this.delay);
		}
	}

	// Token: 0x04001947 RID: 6471
	public bool oneTime;

	// Token: 0x04001948 RID: 6472
	public bool disableOnExit;

	// Token: 0x04001949 RID: 6473
	public bool dontActivateOnEnable;

	// Token: 0x0400194A RID: 6474
	public bool reactivateOnEnable;

	// Token: 0x0400194B RID: 6475
	public bool activateOnDisable;

	// Token: 0x0400194C RID: 6476
	public bool forEnemies;

	// Token: 0x0400194D RID: 6477
	public bool notIfEnemiesDisabled;

	// Token: 0x0400194E RID: 6478
	public bool onlyIfPlayerIsAlive;

	// Token: 0x0400194F RID: 6479
	public bool dontUseEventsIfEnemiesDisabled;

	// Token: 0x04001950 RID: 6480
	[HideInInspector]
	public bool activated;

	// Token: 0x04001951 RID: 6481
	[HideInInspector]
	public bool activating;

	// Token: 0x04001952 RID: 6482
	public float delay;

	// Token: 0x04001953 RID: 6483
	private bool nonCollider;

	// Token: 0x04001954 RID: 6484
	private int playerIn;

	// Token: 0x04001955 RID: 6485
	[Space(20f)]
	public Collider[] ignoreColliders;

	// Token: 0x04001956 RID: 6486
	[Space(20f)]
	public ObjectActivationCheck obac;

	// Token: 0x04001957 RID: 6487
	public bool onlyCheckObacOnce;

	// Token: 0x04001958 RID: 6488
	public bool disableIfObacOff;

	// Token: 0x04001959 RID: 6489
	[Space(10f)]
	public UltrakillEvent events;
}
