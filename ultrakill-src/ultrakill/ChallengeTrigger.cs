using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class ChallengeTrigger : MonoBehaviour
{
	// Token: 0x06000F02 RID: 3842 RVA: 0x0006F950 File Offset: 0x0006DB50
	private void Start()
	{
		if (this.type == ChallengeType.Fail)
		{
			MonoSingleton<ChallengeDoneByDefault>.Instance.Prepare();
		}
		this.colliderless = base.GetComponent<Collider>() == null && base.GetComponent<Rigidbody>() == null;
		if (this.colliderless && (this.evenIfPlayerDead || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.Entered();
		}
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x0006F9B5 File Offset: 0x0006DBB5
	private void OnEnable()
	{
		if (this.colliderless && (this.evenIfPlayerDead || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.Entered();
		}
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x0006F9DC File Offset: 0x0006DBDC
	private void OnDisable()
	{
		if (this.colliderless && this.disableOnExit && base.gameObject.scene.isLoaded)
		{
			this.Exited();
		}
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0006FA14 File Offset: 0x0006DC14
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && (!this.checkForNoEnemies || !DisableEnemySpawns.DisableArenaTriggers) && (this.evenIfPlayerDead || !MonoSingleton<NewMovement>.Instance.dead))
		{
			this.Entered();
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0006FA51 File Offset: 0x0006DC51
	private void OnTriggerExit(Collider other)
	{
		if (this.disableOnExit && other.gameObject.CompareTag("Player") && (!this.checkForNoEnemies || !DisableEnemySpawns.DisableArenaTriggers))
		{
			this.Exited();
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0006FA82 File Offset: 0x0006DC82
	public void Entered()
	{
		if (this.type == ChallengeType.Fail)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeFailed = true;
			MonoSingleton<ChallengeManager>.Instance.challengeDone = false;
			return;
		}
		if (this.type == ChallengeType.Succeed)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeFailed = false;
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
		}
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0006FAC2 File Offset: 0x0006DCC2
	public void Exited()
	{
		if (this.type == ChallengeType.Fail)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeFailed = false;
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
			return;
		}
		if (this.type == ChallengeType.Succeed)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeFailed = true;
			MonoSingleton<ChallengeManager>.Instance.challengeDone = false;
		}
	}

	// Token: 0x0400142E RID: 5166
	public ChallengeType type;

	// Token: 0x0400142F RID: 5167
	public bool checkForNoEnemies;

	// Token: 0x04001430 RID: 5168
	public bool evenIfPlayerDead;

	// Token: 0x04001431 RID: 5169
	private bool colliderless;

	// Token: 0x04001432 RID: 5170
	public bool disableOnExit;
}
