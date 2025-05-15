using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200039B RID: 923
[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class RumbleManager : MonoSingleton<RumbleManager>
{
	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06001532 RID: 5426 RVA: 0x000AD1E7 File Offset: 0x000AB3E7
	// (set) Token: 0x06001533 RID: 5427 RVA: 0x000AD1EF File Offset: 0x000AB3EF
	public float currentIntensity { get; private set; }

	// Token: 0x06001534 RID: 5428 RVA: 0x000AD1F8 File Offset: 0x000AB3F8
	public PendingVibration SetVibration(RumbleKey key)
	{
		PendingVibration pendingVibration;
		if (this.pendingVibrations.TryGetValue(key, out pendingVibration))
		{
			pendingVibration.timeSinceStart = 0f;
			if (pendingVibration.isTracking)
			{
				pendingVibration.isTracking = false;
			}
			return pendingVibration;
		}
		PendingVibration pendingVibration2 = new PendingVibration
		{
			key = key,
			timeSinceStart = 0f,
			isTracking = false
		};
		this.pendingVibrations.Add(key, pendingVibration2);
		return pendingVibration2;
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x000AD268 File Offset: 0x000AB468
	public PendingVibration SetVibrationTracked(RumbleKey key, GameObject tracked)
	{
		PendingVibration pendingVibration;
		if (this.pendingVibrations.TryGetValue(key, out pendingVibration))
		{
			pendingVibration.timeSinceStart = 0f;
			pendingVibration.isTracking = true;
			pendingVibration.trackedObject = tracked;
			return pendingVibration;
		}
		PendingVibration pendingVibration2 = new PendingVibration
		{
			key = key,
			timeSinceStart = 0f,
			isTracking = true,
			trackedObject = tracked
		};
		this.pendingVibrations.Add(key, pendingVibration2);
		return pendingVibration2;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x000AD2DE File Offset: 0x000AB4DE
	public void StopVibration(RumbleKey key)
	{
		if (this.pendingVibrations.ContainsKey(key))
		{
			this.pendingVibrations.Remove(key);
		}
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000AD2FB File Offset: 0x000AB4FB
	public void StopAllVibrations()
	{
		this.pendingVibrations.Clear();
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000AD308 File Offset: 0x000AB508
	private void Update()
	{
		this.discardedKeys.Clear();
		foreach (KeyValuePair<RumbleKey, PendingVibration> keyValuePair in this.pendingVibrations)
		{
			if (keyValuePair.Value.isTracking && (keyValuePair.Value.trackedObject == null || !keyValuePair.Value.trackedObject.activeInHierarchy))
			{
				this.discardedKeys.Add(keyValuePair.Key);
			}
			else if (keyValuePair.Value.IsFinished)
			{
				this.discardedKeys.Add(keyValuePair.Key);
			}
		}
		foreach (RumbleKey rumbleKey in this.discardedKeys)
		{
			this.pendingVibrations.Remove(rumbleKey);
		}
		float num = 0f;
		foreach (KeyValuePair<RumbleKey, PendingVibration> keyValuePair2 in this.pendingVibrations)
		{
			if (keyValuePair2.Value.Intensity > num)
			{
				num = keyValuePair2.Value.Intensity;
			}
		}
		num *= MonoSingleton<PrefsManager>.Instance.GetFloat("totalRumbleIntensity", 0f);
		if (MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused)
		{
			num = 0f;
		}
		this.currentIntensity = num;
		if (Gamepad.current != null)
		{
			Gamepad.current.SetMotorSpeeds(num, num);
		}
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x000AD4C0 File Offset: 0x000AB6C0
	private void OnDisable()
	{
		if (Gamepad.current != null)
		{
			Gamepad.current.SetMotorSpeeds(0f, 0f);
		}
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x000AD4E0 File Offset: 0x000AB6E0
	public float ResolveDuration(RumbleKey key)
	{
		string text = key.name + ".duration";
		if (MonoSingleton<PrefsManager>.Instance.HasKey(text))
		{
			return MonoSingleton<PrefsManager>.Instance.GetFloat(text, 0f);
		}
		return this.ResolveDefaultDuration(key);
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000AD524 File Offset: 0x000AB724
	public float ResolveDefaultDuration(RumbleKey key)
	{
		float num;
		if (RumbleManager.rumbleDurations.TryGetValue(key, out num))
		{
			return num;
		}
		Debug.LogError("No duration found for key: " + ((key != null) ? key.ToString() : null));
		return 0.5f;
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x000AD564 File Offset: 0x000AB764
	public float ResolveIntensity(RumbleKey key)
	{
		if (MonoSingleton<PrefsManager>.Instance.HasKey(key.name + ".intensity"))
		{
			return MonoSingleton<PrefsManager>.Instance.GetFloat(key.name + ".intensity", 0f);
		}
		return this.ResolveDefaultIntensity(key);
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x000AD5B4 File Offset: 0x000AB7B4
	public float ResolveDefaultIntensity(RumbleKey key)
	{
		float num;
		if (RumbleManager.rumbleIntensities.TryGetValue(key, out num))
		{
			return num;
		}
		Debug.LogError("No intensity found for key: " + ((key != null) ? key.ToString() : null));
		return 0.5f;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x000AD5F3 File Offset: 0x000AB7F3
	public static string ResolveFullName(RumbleKey key)
	{
		if (RumbleManager.fullNames.ContainsKey(key))
		{
			return RumbleManager.fullNames[key];
		}
		return key.ToString();
	}

	// Token: 0x04001D75 RID: 7541
	public readonly Dictionary<RumbleKey, PendingVibration> pendingVibrations = new Dictionary<RumbleKey, PendingVibration>();

	// Token: 0x04001D77 RID: 7543
	private List<RumbleKey> discardedKeys = new List<RumbleKey>();

	// Token: 0x04001D78 RID: 7544
	private static readonly Dictionary<RumbleKey, float> rumbleDurations = new Dictionary<RumbleKey, float>
	{
		{
			RumbleProperties.Slide,
			float.PositiveInfinity
		},
		{
			RumbleProperties.WhiplashThrow,
			float.PositiveInfinity
		},
		{
			RumbleProperties.WhiplashPull,
			float.PositiveInfinity
		},
		{
			RumbleProperties.RailcannonIdle,
			float.PositiveInfinity
		},
		{
			RumbleProperties.ShotgunCharge,
			float.PositiveInfinity
		},
		{
			RumbleProperties.NailgunFire,
			float.PositiveInfinity
		},
		{
			RumbleProperties.RevolverCharge,
			float.PositiveInfinity
		},
		{
			RumbleProperties.FallImpact,
			0.5f
		},
		{
			RumbleProperties.FallImpactHeavy,
			0.5f
		},
		{
			RumbleProperties.Jump,
			0.2f
		},
		{
			RumbleProperties.Dash,
			0.2f
		},
		{
			RumbleProperties.Punch,
			0.2f
		},
		{
			RumbleProperties.Sawblade,
			0.2f
		},
		{
			RumbleProperties.GunFire,
			0.4f
		},
		{
			RumbleProperties.SuperSaw,
			0.7f
		},
		{
			RumbleProperties.GunFireStrong,
			0.7f
		},
		{
			RumbleProperties.GunFireProjectiles,
			0.8f
		},
		{
			RumbleProperties.ParryFlash,
			0.1f
		},
		{
			RumbleProperties.CoinToss,
			0.1f
		},
		{
			RumbleProperties.Magnet,
			0.1f
		},
		{
			RumbleProperties.WeaponWheelTick,
			0.025f
		}
	};

	// Token: 0x04001D79 RID: 7545
	public static readonly Dictionary<RumbleKey, float> rumbleIntensities = new Dictionary<RumbleKey, float>
	{
		{
			RumbleProperties.Slide,
			0.1f
		},
		{
			RumbleProperties.Dash,
			0.2f
		},
		{
			RumbleProperties.FallImpact,
			0.2f
		},
		{
			RumbleProperties.Jump,
			0.1f
		},
		{
			RumbleProperties.FallImpactHeavy,
			0.5f
		},
		{
			RumbleProperties.WhiplashThrow,
			0.2f
		},
		{
			RumbleProperties.WhiplashPull,
			0.35f
		},
		{
			RumbleProperties.GunFire,
			0.8f
		},
		{
			RumbleProperties.GunFireStrong,
			1f
		},
		{
			RumbleProperties.GunFireProjectiles,
			0.7f
		},
		{
			RumbleProperties.RailcannonIdle,
			0.2f
		},
		{
			RumbleProperties.NailgunFire,
			0.2f
		},
		{
			RumbleProperties.SuperSaw,
			0.7f
		},
		{
			RumbleProperties.ShotgunCharge,
			0.7f
		},
		{
			RumbleProperties.Sawblade,
			0.5f
		},
		{
			RumbleProperties.RevolverCharge,
			0.5f
		},
		{
			RumbleProperties.Punch,
			0.2f
		},
		{
			RumbleProperties.ParryFlash,
			0.2f
		},
		{
			RumbleProperties.Magnet,
			0.2f
		},
		{
			RumbleProperties.CoinToss,
			0.1f
		},
		{
			RumbleProperties.WeaponWheelTick,
			0.05f
		}
	};

	// Token: 0x04001D7A RID: 7546
	public static readonly Dictionary<RumbleKey, string> fullNames = new Dictionary<RumbleKey, string>
	{
		{
			RumbleProperties.Slide,
			"Sliding"
		},
		{
			RumbleProperties.Dash,
			"Dashing"
		},
		{
			RumbleProperties.FallImpact,
			"Fall Impact"
		},
		{
			RumbleProperties.Jump,
			"Jumping"
		},
		{
			RumbleProperties.FallImpactHeavy,
			"Heavy Fall Impact"
		},
		{
			RumbleProperties.WhiplashThrow,
			"Whiplash Throw"
		},
		{
			RumbleProperties.WhiplashPull,
			"Whiplash Pull"
		},
		{
			RumbleProperties.GunFire,
			"Gun Fire"
		},
		{
			RumbleProperties.GunFireStrong,
			"Stronger Gun Fire"
		},
		{
			RumbleProperties.GunFireProjectiles,
			"Gun Fire (projectiles)"
		},
		{
			RumbleProperties.RailcannonIdle,
			"Railcannon Idle"
		},
		{
			RumbleProperties.NailgunFire,
			"Nailgun Fire"
		},
		{
			RumbleProperties.Sawblade,
			"Sawblade"
		},
		{
			RumbleProperties.SuperSaw,
			"Super Saw"
		},
		{
			RumbleProperties.Magnet,
			"Magnet"
		},
		{
			RumbleProperties.ShotgunCharge,
			"Shotgun Charge"
		},
		{
			RumbleProperties.RevolverCharge,
			"Revolver Charge"
		},
		{
			RumbleProperties.ParryFlash,
			"Parry Flash"
		},
		{
			RumbleProperties.CoinToss,
			"Coin Toss"
		},
		{
			RumbleProperties.Punch,
			"Punching"
		},
		{
			RumbleProperties.WeaponWheelTick,
			"Weapon Wheel Tick"
		}
	};
}
