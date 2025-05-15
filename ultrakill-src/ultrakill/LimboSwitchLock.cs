using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002D6 RID: 726
public class LimboSwitchLock : MonoBehaviour
{
	// Token: 0x06000FC1 RID: 4033 RVA: 0x0007578C File Offset: 0x0007398C
	private void Start()
	{
		this.block = new MaterialPropertyBlock();
		this.lockIntensities = new float[this.locks.Length + ((this.primeBossLocks != null && this.primeBossLocks.Length != 0) ? 1 : 0)];
		this.lockStates = new bool[this.locks.Length + ((this.primeBossLocks != null && this.primeBossLocks.Length != 0) ? 1 : 0)];
		this.CheckSaves();
		this.CheckLocks();
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x00075804 File Offset: 0x00073A04
	public void CheckSaves()
	{
		if (this.type != SwitchLockType.None)
		{
			if (this.type == SwitchLockType.Limbo || this.type == SwitchLockType.Shotgun)
			{
				for (int i = 0; i < this.locks.Length; i++)
				{
					if ((this.type == SwitchLockType.Limbo && GameProgressSaver.GetLimboSwitch(i + this.minimumOrderNumber)) || (this.type == SwitchLockType.Shotgun && GameProgressSaver.GetShotgunSwitch(i + this.minimumOrderNumber)))
					{
						this.lockIntensities[i] = 2f;
						this.lockStates[i] = true;
						this.locks[i].GetPropertyBlock(this.block);
						this.block.SetFloat(UKShaderProperties.EmissiveIntensity, 2f);
						this.locks[i].SetPropertyBlock(this.block);
						this.openedLocks++;
					}
				}
				return;
			}
			if (this.type == SwitchLockType.PRank)
			{
				int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
				for (int j = 0; j < this.locks.Length; j++)
				{
					RankData rank = GameProgressSaver.GetRank(j + this.minimumOrderNumber + 1, true);
					if (rank != null && rank.ranks[@int] == 12 && (rank.majorAssists == null || !rank.majorAssists[@int]))
					{
						this.lockIntensities[j] = 2f;
						this.lockStates[j] = true;
						this.locks[j].GetPropertyBlock(this.block);
						this.block.SetFloat(UKShaderProperties.EmissiveIntensity, 2f);
						this.locks[j].SetPropertyBlock(this.block);
						this.openedLocks++;
					}
				}
				if (this.primeBossLocks.Length != 0)
				{
					RankData rank2 = GameProgressSaver.GetRank(this.primeBossLockNumber + 665, true);
					if (rank2 != null && rank2.ranks[@int] >= 0 && (rank2.majorAssists == null || !rank2.majorAssists[@int]))
					{
						this.lockIntensities[this.locks.Length] = 2f;
						this.lockStates[this.locks.Length] = true;
						foreach (MeshRenderer meshRenderer in this.primeBossLocks)
						{
							meshRenderer.GetPropertyBlock(this.block);
							this.block.SetFloat(UKShaderProperties.EmissiveIntensity, 2f);
							meshRenderer.SetPropertyBlock(this.block);
						}
						this.openedLocks++;
					}
				}
			}
		}
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x00075A6C File Offset: 0x00073C6C
	private void Update()
	{
		for (int i = 0; i < this.locks.Length; i++)
		{
			if (this.lockStates[i] && this.lockIntensities[i] != 2f)
			{
				this.lockIntensities[i] = Mathf.MoveTowards(this.lockIntensities[i], 2f, Time.deltaTime);
				this.locks[i].GetPropertyBlock(this.block);
				this.block.SetFloat(UKShaderProperties.EmissiveIntensity, this.lockIntensities[i]);
				this.locks[i].SetPropertyBlock(this.block);
				if (this.lockIntensities[i] == 2f)
				{
					this.openedLocks++;
					this.CheckLocks();
				}
			}
		}
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x00075B31 File Offset: 0x00073D31
	private void CheckLocks()
	{
		if (this.openedLocks == this.locks.Length + ((this.primeBossLocks != null && this.primeBossLocks.Length != 0) ? 1 : 0))
		{
			UnityEvent unityEvent = this.onAllLocksOpen;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x00075B6C File Offset: 0x00073D6C
	public void OpenLock(int num)
	{
		this.lockStates[num - 1] = true;
		AudioSource audioSource;
		if (this.locks[num - 1] != null && this.locks[num - 1].TryGetComponent<AudioSource>(out audioSource))
		{
			audioSource.Play();
		}
	}

	// Token: 0x04001552 RID: 5458
	public SwitchLockType type;

	// Token: 0x04001553 RID: 5459
	public MeshRenderer[] locks;

	// Token: 0x04001554 RID: 5460
	public MeshRenderer[] primeBossLocks;

	// Token: 0x04001555 RID: 5461
	private MaterialPropertyBlock block;

	// Token: 0x04001556 RID: 5462
	private float[] lockIntensities;

	// Token: 0x04001557 RID: 5463
	private bool[] lockStates;

	// Token: 0x04001558 RID: 5464
	private int openedLocks;

	// Token: 0x04001559 RID: 5465
	public UnityEvent onAllLocksOpen;

	// Token: 0x0400155A RID: 5466
	public int minimumOrderNumber;

	// Token: 0x0400155B RID: 5467
	public int primeBossLockNumber = 1;
}
