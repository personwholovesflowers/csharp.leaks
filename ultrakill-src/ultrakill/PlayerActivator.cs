using System;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class PlayerActivator : MonoBehaviour
{
	// Token: 0x06001331 RID: 4913 RVA: 0x0009AD03 File Offset: 0x00098F03
	private void OnTriggerEnter(Collider other)
	{
		if (this.activated)
		{
			return;
		}
		if (!other.gameObject.CompareTag("Player"))
		{
			return;
		}
		this.Activate();
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0009AD28 File Offset: 0x00098F28
	public void Activate()
	{
		if (this.activated)
		{
			return;
		}
		this.nm = MonoSingleton<NewMovement>.Instance;
		this.gc = MonoSingleton<GunControl>.Instance;
		GameStateManager.Instance.PopState("pit-falling");
		if (!this.nm.activated)
		{
			this.nm.activated = true;
			this.nm.cc.activated = true;
			this.nm.cc.CameraShake(1f);
			AudioSource component = base.GetComponent<AudioSource>();
			if (component)
			{
				component.Play();
			}
		}
		this.activated = true;
		if (!this.onlyActivatePlayer)
		{
			this.gc.YesWeapon();
			this.ActivateObjects();
		}
		if (this.startTimer)
		{
			MonoSingleton<StatsManager>.Instance.StartTimer();
		}
		MonoSingleton<FistControl>.Instance.YesFist();
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x0009ADF3 File Offset: 0x00098FF3
	private void ActivateObjects()
	{
		MonoSingleton<PlayerActivatorRelay>.Instance.Activate();
		PlayerActivator.lastActivatedPosition = MonoSingleton<NewMovement>.Instance.transform.position;
	}

	// Token: 0x04001A87 RID: 6791
	private NewMovement nm;

	// Token: 0x04001A88 RID: 6792
	private bool activated;

	// Token: 0x04001A89 RID: 6793
	[SerializeField]
	private bool startTimer;

	// Token: 0x04001A8A RID: 6794
	[SerializeField]
	private bool onlyActivatePlayer;

	// Token: 0x04001A8B RID: 6795
	private GunControl gc;

	// Token: 0x04001A8C RID: 6796
	public static Vector3 lastActivatedPosition;
}
