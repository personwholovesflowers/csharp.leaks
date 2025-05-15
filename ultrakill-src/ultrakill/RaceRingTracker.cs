using System;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class RaceRingTracker : MonoBehaviour
{
	// Token: 0x0600146E RID: 5230 RVA: 0x000A58E8 File Offset: 0x000A3AE8
	private void Start()
	{
		this.hm = base.gameObject.AddComponent<HudMessage>();
		this.hm.timed = true;
		this.hm.message = "RACE START";
		if (this.infiniteRocketRide)
		{
			MonoSingleton<WeaponCharges>.Instance.infiniteRocketRide = true;
		}
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x000A5935 File Offset: 0x000A3B35
	private void Update()
	{
		if (!this.complete)
		{
			this.time += Time.deltaTime;
		}
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x000A5951 File Offset: 0x000A3B51
	public void AddScore()
	{
		this.currentRaceRings++;
		if (this.complete)
		{
			return;
		}
		if (this.currentRaceRings == this.raceRingAmount)
		{
			this.Victory();
		}
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x000A5980 File Offset: 0x000A3B80
	public void Victory()
	{
		this.complete = true;
		this.hm = base.gameObject.AddComponent<HudMessage>();
		this.hm.timed = true;
		this.hm.message = "TIME: <color=#00ff00ff>" + this.time.ToString("F3") + "</color>";
		if (this.infiniteRocketRide)
		{
			MonoSingleton<WeaponCharges>.Instance.infiniteRocketRide = false;
		}
		UltrakillEvent ultrakillEvent = this.onVictory;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x04001C13 RID: 7187
	public int raceRingAmount;

	// Token: 0x04001C14 RID: 7188
	private int currentRaceRings;

	// Token: 0x04001C15 RID: 7189
	private bool complete;

	// Token: 0x04001C16 RID: 7190
	private float time;

	// Token: 0x04001C17 RID: 7191
	private HudMessage hm;

	// Token: 0x04001C18 RID: 7192
	public UltrakillEvent onVictory;

	// Token: 0x04001C19 RID: 7193
	public bool infiniteRocketRide;
}
