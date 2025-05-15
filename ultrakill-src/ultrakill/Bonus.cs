using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class Bonus : MonoBehaviour
{
	// Token: 0x060002A7 RID: 679 RVA: 0x0000F528 File Offset: 0x0000D728
	private void Start()
	{
		this.cRotation = new Vector3((float)Random.Range(-5, 5), (float)Random.Range(-5, 5), (float)Random.Range(-5, 5));
		if (this.secretNumber >= 0 && !this.ghostVersion && MonoSingleton<StatsManager>.Instance.secretObjects[this.secretNumber] != base.gameObject)
		{
			MonoSingleton<StatsManager>.Instance.secretObjects[this.secretNumber] = base.gameObject;
		}
		if (this.beenFound || (this.secretNumber >= 0 && MonoSingleton<StatsManager>.Instance.newSecrets.Contains(this.secretNumber)))
		{
			Debug.Log(string.Concat(new string[]
			{
				"Name: ",
				base.gameObject.name,
				". Been Found: ",
				this.beenFound.ToString(),
				". Secret Number: ",
				this.secretNumber.ToString()
			}));
			this.BeenFound();
		}
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000F628 File Offset: 0x0000D828
	public void UpdateStatsManagerReference()
	{
		if (this.secretNumber >= 0 && !this.ghost && MonoSingleton<StatsManager>.Instance.secretObjects[this.secretNumber] != base.gameObject)
		{
			MonoSingleton<StatsManager>.Instance.secretObjects[this.secretNumber] = base.gameObject;
		}
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000F67B File Offset: 0x0000D87B
	private void Update()
	{
		base.transform.Rotate(this.cRotation * Time.deltaTime * 5f);
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !this.activated)
		{
			if (!this.ghost)
			{
				this.activated = true;
				MonoSingleton<TimeController>.Instance.ParryFlash();
				StyleHUD instance = MonoSingleton<StyleHUD>.Instance;
				StatsManager instance2 = MonoSingleton<StatsManager>.Instance;
				Object.Instantiate<GameObject>(this.breakEffect, base.transform.position, Quaternion.identity);
				instance.AddPoints(0, "ultrakill.secret", null, null, -1, "", "");
				instance2.secrets++;
				instance2.SecretFound(this.secretNumber);
				Object.Destroy(base.gameObject);
			}
			else
			{
				if (this.tutorial)
				{
					MonoSingleton<TimeController>.Instance.ParryFlash();
				}
				Object.Instantiate<GameObject>(this.breakEffect, base.transform.position, Quaternion.identity);
				Object.Destroy(base.gameObject);
			}
			if (this.superCharge)
			{
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
				{
					MonoSingleton<NewMovement>.Instance.SuperCharge();
				}
				else
				{
					MonoSingleton<PlatformerMovement>.Instance.AddExtraHit(2);
				}
				if (!MonoSingleton<PrefsManager>.Instance.GetBool("hideSuperChargePopup", false) && MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0) > 0)
				{
					MonoSingleton<PrefsManager>.Instance.SetBool("hideSuperChargePopup", true);
					MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=red>RED SOUL ORBS</color> give <color=green>200 HEALTH</color>. \nOverheal cannot be regained with blood.", "", "", 1, false, false, true);
					return;
				}
			}
			else if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
			{
				MonoSingleton<PlatformerMovement>.Instance.AddExtraHit(1);
			}
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000F820 File Offset: 0x0000DA20
	public void BeenFound()
	{
		if (this.ghostVersion == null)
		{
			Debug.Log("No ghost version for " + base.gameObject.name);
			this.ghostVersion = PrefabReplacer.Instance.LoadPrefab("Bonus Ghost");
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.ghostVersion, base.transform.position, base.transform.rotation);
		if (base.transform.parent)
		{
			gameObject.transform.SetParent(base.transform.parent, true);
		}
		DualWieldPickup dualWieldPickup;
		DualWieldPickup dualWieldPickup2;
		if (base.TryGetComponent<DualWieldPickup>(out dualWieldPickup) && gameObject.TryGetComponent<DualWieldPickup>(out dualWieldPickup2))
		{
			dualWieldPickup2.juiceAmount = dualWieldPickup.juiceAmount;
		}
		base.gameObject.SetActive(false);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000329 RID: 809
	private Vector3 cRotation;

	// Token: 0x0400032A RID: 810
	public GameObject breakEffect;

	// Token: 0x0400032B RID: 811
	private bool activated;

	// Token: 0x0400032C RID: 812
	public bool ghost;

	// Token: 0x0400032D RID: 813
	public bool tutorial;

	// Token: 0x0400032E RID: 814
	public bool superCharge;

	// Token: 0x0400032F RID: 815
	public bool dontReplaceWithGhost;

	// Token: 0x04000330 RID: 816
	[HideInInspector]
	public bool beenFound;

	// Token: 0x04000331 RID: 817
	public int secretNumber = -1;

	// Token: 0x04000332 RID: 818
	public GameObject ghostVersion;
}
