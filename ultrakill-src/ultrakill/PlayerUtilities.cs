using System;
using Logic;
using UnityEngine;

// Token: 0x0200034F RID: 847
public class PlayerUtilities : MonoSingleton<PlayerUtilities>
{
	// Token: 0x0600137D RID: 4989 RVA: 0x0009C8A4 File Offset: 0x0009AAA4
	public void Update()
	{
		if (!this.enableOutput)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.distanceTraveledMapVar))
		{
			if (this.lastRecordedPosition == null)
			{
				this.lastRecordedPosition = new Vector3?(MonoSingleton<NewMovement>.Instance.transform.position);
			}
			this.distanceTraveled += Vector3.Distance(this.lastRecordedPosition.Value, MonoSingleton<NewMovement>.Instance.transform.position);
			this.lastRecordedPosition = new Vector3?(MonoSingleton<NewMovement>.Instance.transform.position);
			MonoSingleton<MapVarManager>.Instance.SetFloat(this.distanceTraveledMapVar, this.distanceTraveled, false);
		}
		if (!string.IsNullOrEmpty(this.currentHealthVar))
		{
			MonoSingleton<MapVarManager>.Instance.SetInt(this.currentHealthVar, MonoSingleton<NewMovement>.Instance.hp, false);
		}
		if (!string.IsNullOrEmpty(this.currentHardDamageVar))
		{
			MonoSingleton<MapVarManager>.Instance.SetFloat(this.currentHardDamageVar, MonoSingleton<NewMovement>.Instance.antiHp, false);
		}
		if (!string.IsNullOrEmpty(this.currentStyleScoreVar))
		{
			MonoSingleton<MapVarManager>.Instance.SetInt(this.currentStyleScoreVar, MonoSingleton<StatsManager>.Instance.stylePoints, false);
		}
		if (!string.IsNullOrEmpty(this.currentTimeVar))
		{
			MonoSingleton<MapVarManager>.Instance.SetFloat(this.currentTimeVar, MonoSingleton<StatsManager>.Instance.seconds, false);
		}
		if (!string.IsNullOrEmpty(this.currentKillCountVar))
		{
			MonoSingleton<MapVarManager>.Instance.SetInt(this.currentKillCountVar, MonoSingleton<StatsManager>.Instance.kills, false);
		}
		if (!string.IsNullOrEmpty(this.currentRankVar))
		{
			MonoSingleton<MapVarManager>.Instance.SetInt(this.currentRankVar, MonoSingleton<StatsManager>.Instance.rankScore, false);
		}
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x0009CA3E File Offset: 0x0009AC3E
	public void DisablePlayer()
	{
		MonoSingleton<NewMovement>.Instance.gameObject.SetActive(false);
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x0009CA50 File Offset: 0x0009AC50
	public void EnablePlayer()
	{
		MonoSingleton<NewMovement>.Instance.gameObject.SetActive(true);
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x0009CA62 File Offset: 0x0009AC62
	public void FreezePlayer()
	{
		MonoSingleton<NewMovement>.Instance.enabled = false;
		MonoSingleton<NewMovement>.Instance.rb.isKinematic = true;
		MonoSingleton<CameraController>.Instance.activated = false;
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0009CA8A File Offset: 0x0009AC8A
	public void UnfreezePlayer()
	{
		MonoSingleton<NewMovement>.Instance.enabled = true;
		MonoSingleton<NewMovement>.Instance.rb.isKinematic = false;
		MonoSingleton<CameraController>.Instance.activated = true;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0009CAB4 File Offset: 0x0009ACB4
	public void FadeOutFallingWhoosh()
	{
		if (PlayerUtilities.detachedWhoosh)
		{
			Object.Destroy(PlayerUtilities.detachedWhoosh.gameObject);
		}
		PlayerUtilities.detachedWhoosh = MonoSingleton<NewMovement>.Instance.DuplicateDetachWhoosh();
		FadeOut fadeOut = PlayerUtilities.detachedWhoosh.gameObject.AddComponent<FadeOut>();
		fadeOut.auds = new AudioSource[] { PlayerUtilities.detachedWhoosh };
		fadeOut.speed = 0.1f;
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0009CB1C File Offset: 0x0009AD1C
	public void FadeOutFallingWhooshCustomSpeed(float speed)
	{
		if (PlayerUtilities.detachedWhoosh)
		{
			Object.Destroy(PlayerUtilities.detachedWhoosh.gameObject);
		}
		PlayerUtilities.detachedWhoosh = MonoSingleton<NewMovement>.Instance.DuplicateDetachWhoosh();
		FadeOut fadeOut = PlayerUtilities.detachedWhoosh.gameObject.AddComponent<FadeOut>();
		fadeOut.auds = new AudioSource[] { PlayerUtilities.detachedWhoosh };
		fadeOut.speed = speed;
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x0009CB7E File Offset: 0x0009AD7E
	public void RestoreFallingWhoosh()
	{
		if (!PlayerUtilities.detachedWhoosh)
		{
			return;
		}
		AudioSource audioSource = MonoSingleton<NewMovement>.Instance.RestoreWhoosh();
		audioSource.time = PlayerUtilities.detachedWhoosh.time;
		audioSource.Play();
		Object.Destroy(PlayerUtilities.detachedWhoosh.gameObject);
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x0009CBBB File Offset: 0x0009ADBB
	public void YesWeapon()
	{
		MonoSingleton<GunControl>.Instance.YesWeapon();
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x0009CBC7 File Offset: 0x0009ADC7
	public void NoWeapon()
	{
		MonoSingleton<GunControl>.Instance.NoWeapon();
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x0009CBD3 File Offset: 0x0009ADD3
	public void YesFist()
	{
		MonoSingleton<FistControl>.Instance.YesFist();
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x0009CBDF File Offset: 0x0009ADDF
	public void NoFist()
	{
		MonoSingleton<FistControl>.Instance.NoFist();
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x0009CBEB File Offset: 0x0009ADEB
	public void HealPlayer(int health)
	{
		MonoSingleton<NewMovement>.Instance.GetHealth(health, false, false, true);
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x0009CBFB File Offset: 0x0009ADFB
	public void HealPlayerSilent(int health)
	{
		MonoSingleton<NewMovement>.Instance.GetHealth(health, true, false, true);
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x0009CC0B File Offset: 0x0009AE0B
	public void EmptyStamina()
	{
		MonoSingleton<NewMovement>.Instance.EmptyStamina();
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x0009CC17 File Offset: 0x0009AE17
	public void FullStamina()
	{
		MonoSingleton<NewMovement>.Instance.FullStamina();
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x0009CC23 File Offset: 0x0009AE23
	public void ResetHardDamage()
	{
		MonoSingleton<NewMovement>.Instance.ResetHardDamage();
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x0009CC2F File Offset: 0x0009AE2F
	public void MaxCharges()
	{
		MonoSingleton<WeaponCharges>.Instance.MaxCharges();
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x0009CC3C File Offset: 0x0009AE3C
	public void DestroyHeldObject()
	{
		if (!MonoSingleton<FistControl>.Instance || !MonoSingleton<FistControl>.Instance.heldObject)
		{
			return;
		}
		Object.Destroy(MonoSingleton<FistControl>.Instance.heldObject.gameObject);
		MonoSingleton<FistControl>.Instance.currentPunch.ResetHeldState();
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x0009CC8C File Offset: 0x0009AE8C
	public void PlaceHeldObject(ItemPlaceZone target)
	{
		if (!MonoSingleton<FistControl>.Instance || !MonoSingleton<FistControl>.Instance.currentPunch)
		{
			return;
		}
		if (!target)
		{
			return;
		}
		MonoSingleton<FistControl>.Instance.currentPunch.PlaceHeldObject(new ItemPlaceZone[] { target }, target.transform);
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x0009CCDF File Offset: 0x0009AEDF
	public void ForceHoldObject(ItemIdentifier pickup)
	{
		this.DestroyHeldObject();
		if (!MonoSingleton<FistControl>.Instance || !MonoSingleton<FistControl>.Instance.currentPunch)
		{
			return;
		}
		MonoSingleton<FistControl>.Instance.currentPunch.ForceHold(pickup);
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x0009CD15 File Offset: 0x0009AF15
	public void ParryFlash()
	{
		MonoSingleton<TimeController>.Instance.ParryFlash();
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x0009CD21 File Offset: 0x0009AF21
	public void QuitMap()
	{
		MonoSingleton<OptionsManager>.Instance.QuitMission();
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x0009CD2D File Offset: 0x0009AF2D
	public void FinishMap()
	{
		SceneHelper.SpawnFinalPitAndFinish();
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x0009CD34 File Offset: 0x0009AF34
	public void SetGravity(float gravity)
	{
		Physics.gravity = new Vector3(0f, gravity, 0f);
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x0009CD4B File Offset: 0x0009AF4B
	public void SetGravity(Vector3 gravity)
	{
		Physics.gravity = gravity;
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x0009CD53 File Offset: 0x0009AF53
	public void SetPlayerHealth(int health)
	{
		MonoSingleton<NewMovement>.Instance.hp = health;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0009CD60 File Offset: 0x0009AF60
	public void SetPlayerHardDamage(float damage)
	{
		MonoSingleton<NewMovement>.Instance.antiHp = damage;
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x0009CD6D File Offset: 0x0009AF6D
	public void SetPlayerStamina(float boostCharge)
	{
		MonoSingleton<NewMovement>.Instance.boostCharge = boostCharge;
	}

	// Token: 0x04001AD7 RID: 6871
	public bool enableOutput;

	// Token: 0x04001AD8 RID: 6872
	public string distanceTraveledMapVar;

	// Token: 0x04001AD9 RID: 6873
	public string currentHealthVar;

	// Token: 0x04001ADA RID: 6874
	public string currentHardDamageVar;

	// Token: 0x04001ADB RID: 6875
	public string currentStyleScoreVar;

	// Token: 0x04001ADC RID: 6876
	public string currentTimeVar;

	// Token: 0x04001ADD RID: 6877
	public string currentKillCountVar;

	// Token: 0x04001ADE RID: 6878
	public string currentRankVar;

	// Token: 0x04001ADF RID: 6879
	private static AudioSource detachedWhoosh;

	// Token: 0x04001AE0 RID: 6880
	private float distanceTraveled;

	// Token: 0x04001AE1 RID: 6881
	private Vector3? lastRecordedPosition;
}
