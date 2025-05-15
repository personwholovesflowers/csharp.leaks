using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class AssistController : MonoSingleton<AssistController>
{
	// Token: 0x060001F1 RID: 497 RVA: 0x0000A2CC File Offset: 0x000084CC
	private void Start()
	{
		this.majorEnabled = MonoSingleton<PrefsManager>.Instance.GetBool("majorAssist", false);
		if (this.majorEnabled)
		{
			this.MajorEnabled();
		}
		this.hidePopup = MonoSingleton<PrefsManager>.Instance.GetBool("hideMajorAssistPopup", false);
		this.gameSpeed = MonoSingleton<PrefsManager>.Instance.GetFloat("gameSpeed", 0f);
		this.damageTaken = MonoSingleton<PrefsManager>.Instance.GetFloat("damageTaken", 0f);
		this.infiniteStamina = MonoSingleton<PrefsManager>.Instance.GetBool("infiniteStamina", false);
		this.disableHardDamage = MonoSingleton<PrefsManager>.Instance.GetBool("disableHardDamage", false);
		this.disableWhiplashHardDamage = MonoSingleton<PrefsManager>.Instance.GetBool("disableWhiplashHardDamage", false);
		this.disableWeaponFreshness = MonoSingleton<PrefsManager>.Instance.GetBool("disableWeaponFreshness", false);
		this.difficultyOverride = MonoSingleton<PrefsManager>.Instance.GetInt("bossDifficultyOverride", 0) - 1;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000A3B7 File Offset: 0x000085B7
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000A3DF File Offset: 0x000085DF
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000A404 File Offset: 0x00008604
	private void OnPrefChanged(string key, object value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
		if (num <= 2707575058U)
		{
			if (num <= 380515917U)
			{
				if (num != 106933077U)
				{
					if (num != 380515917U)
					{
						return;
					}
					if (!(key == "disableWhiplashHardDamage"))
					{
						return;
					}
					if (value is bool)
					{
						bool flag = (bool)value;
						this.disableWhiplashHardDamage = flag;
						return;
					}
				}
				else
				{
					if (!(key == "bossDifficultyOverride"))
					{
						return;
					}
					if (value is int)
					{
						int num2 = (int)value;
						this.difficultyOverride = num2 - 1;
					}
				}
			}
			else if (num != 2471339101U)
			{
				if (num != 2707575058U)
				{
					return;
				}
				if (!(key == "gameSpeed"))
				{
					return;
				}
				if (value is float)
				{
					float num3 = (float)value;
					this.gameSpeed = num3;
					return;
				}
			}
			else
			{
				if (!(key == "hideMajorAssistPopup"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag2 = (bool)value;
					this.hidePopup = flag2;
					return;
				}
			}
		}
		else if (num <= 3393357534U)
		{
			if (num != 2954781219U)
			{
				if (num != 3393357534U)
				{
					return;
				}
				if (!(key == "infiniteStamina"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag3 = (bool)value;
					this.infiniteStamina = flag3;
					return;
				}
			}
			else
			{
				if (!(key == "disableHardDamage"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag4 = (bool)value;
					this.disableHardDamage = flag4;
					return;
				}
			}
		}
		else if (num != 3532082477U)
		{
			if (num != 3641621767U)
			{
				if (num != 4013504010U)
				{
					return;
				}
				if (!(key == "disableWeaponFreshness"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag5 = (bool)value;
					this.disableWeaponFreshness = flag5;
					return;
				}
			}
			else
			{
				if (!(key == "damageTaken"))
				{
					return;
				}
				if (value is float)
				{
					float num4 = (float)value;
					this.damageTaken = num4;
					return;
				}
			}
		}
		else
		{
			if (!(key == "majorAssist"))
			{
				return;
			}
			if (value is bool)
			{
				bool flag6 = (bool)value;
				this.majorEnabled = flag6;
				if (this.majorEnabled)
				{
					this.MajorEnabled();
					return;
				}
			}
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000A62F File Offset: 0x0000882F
	public void MajorEnabled()
	{
		this.majorEnabled = true;
		if (this.sman == null)
		{
			this.sman = MonoSingleton<StatsManager>.Instance;
		}
		this.sman.MajorUsed();
	}

	// Token: 0x0400020D RID: 525
	public bool majorEnabled;

	// Token: 0x0400020E RID: 526
	[HideInInspector]
	public bool cheatsEnabled;

	// Token: 0x0400020F RID: 527
	[HideInInspector]
	public bool hidePopup;

	// Token: 0x04000210 RID: 528
	[HideInInspector]
	public float gameSpeed;

	// Token: 0x04000211 RID: 529
	[HideInInspector]
	public float damageTaken;

	// Token: 0x04000212 RID: 530
	[HideInInspector]
	public bool infiniteStamina;

	// Token: 0x04000213 RID: 531
	[HideInInspector]
	public bool disableHardDamage;

	// Token: 0x04000214 RID: 532
	[HideInInspector]
	public bool disableWhiplashHardDamage;

	// Token: 0x04000215 RID: 533
	[HideInInspector]
	public bool disableWeaponFreshness;

	// Token: 0x04000216 RID: 534
	public int punchAssistFrames = 6;

	// Token: 0x04000217 RID: 535
	[HideInInspector]
	public int difficultyOverride = -1;

	// Token: 0x04000218 RID: 536
	private StatsManager sman;
}
