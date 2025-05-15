using System;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class FinalPit : MonoBehaviour
{
	// Token: 0x06000971 RID: 2417 RVA: 0x00040B38 File Offset: 0x0003ED38
	private void Start()
	{
		this.sm = MonoSingleton<StatsManager>.Instance;
		this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		this.targetRotation = Quaternion.Euler(base.transform.rotation.eulerAngles + Vector3.up * 0.01f);
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00040B94 File Offset: 0x0003ED94
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == this.player && MonoSingleton<NewMovement>.Instance && MonoSingleton<NewMovement>.Instance.hp > 0)
		{
			if (this.musicFadeOut)
			{
				MonoSingleton<MusicManager>.Instance.off = true;
			}
			MonoSingleton<FogFadeController>.Instance.FadeOut(false, 10f);
			GameStateManager.Instance.RegisterState(new GameState("pit-falling", base.gameObject)
			{
				cursorLock = LockMode.Lock,
				cameraInputLock = LockMode.Lock
			});
			this.nmov = MonoSingleton<NewMovement>.Instance;
			this.nmov.gameObject.layer = 15;
			this.rb = this.nmov.rb;
			this.nmov.activated = false;
			this.nmov.cc.enabled = false;
			this.nmov.levelOver = true;
			this.sm.HideShit();
			this.sm.StopTimer();
			if (this.nmov.sliding)
			{
				this.nmov.StopSlide();
			}
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
			if (MonoSingleton<PowerUpMeter>.Instance)
			{
				MonoSingleton<PowerUpMeter>.Instance.juice = 0f;
			}
			CrateCounter instance = MonoSingleton<CrateCounter>.Instance;
			if (instance != null)
			{
				instance.SaveStuff();
			}
			CrateCounter instance2 = MonoSingleton<CrateCounter>.Instance;
			if (instance2 != null)
			{
				instance2.CoinsToPoints();
			}
			OutOfBounds[] array = Object.FindObjectsOfType<OutOfBounds>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(false);
			}
			DeathZone[] array2 = Object.FindObjectsOfType<DeathZone>();
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.SetActive(false);
			}
			base.Invoke("SendInfo", 5f);
			return;
		}
		if (other.gameObject.CompareTag("Player") && MonoSingleton<PlatformerMovement>.Instance && !MonoSingleton<PlatformerMovement>.Instance.dead)
		{
			MonoSingleton<PlayerTracker>.Instance.ChangeToFPS();
		}
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00040D94 File Offset: 0x0003EF94
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject == this.player && MonoSingleton<NewMovement>.Instance && MonoSingleton<NewMovement>.Instance.hp > 0)
		{
			if (this.nmov == null)
			{
				this.nmov = other.gameObject.GetComponent<NewMovement>();
				this.rb = this.nmov.rb;
			}
			if (other.transform.position.x != base.transform.position.x || other.transform.position.z != base.transform.position.z)
			{
				Vector3 vector = new Vector3(base.transform.position.x, other.transform.position.y, base.transform.position.z);
				float num = Vector3.Distance(other.transform.position, vector);
				other.transform.position = Vector3.MoveTowards(other.transform.position, vector, 1f + num * Time.deltaTime);
				this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
			}
			if (!this.rotationReady)
			{
				this.nmov.cc.transform.rotation = Quaternion.RotateTowards(this.nmov.cc.transform.rotation, this.targetRotation, Time.fixedDeltaTime * 10f * (Quaternion.Angle(this.nmov.cc.transform.rotation, this.targetRotation) + 1f));
				if (Quaternion.Angle(this.nmov.cc.transform.rotation, this.targetRotation) < 0.01f)
				{
					this.nmov.cc.transform.rotation = this.targetRotation;
					this.rotationReady = true;
				}
			}
			if (this.rotationReady && !this.infoSent)
			{
				this.SendInfo();
			}
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00040FBC File Offset: 0x0003F1BC
	private void SendInfo()
	{
		base.CancelInvoke();
		if (!this.infoSent)
		{
			this.infoSent = true;
			if (!this.rankless)
			{
				FinalRank fr = this.sm.fr;
				if (!this.sm.infoSent)
				{
					this.levelNumber = MonoSingleton<StatsManager>.Instance.levelNumber;
					if (SceneHelper.IsPlayingCustom)
					{
						GameProgressSaver.SaveProgress(SceneHelper.CurrentLevelNumber);
					}
					else if (this.levelNumber >= 420)
					{
						GameProgressSaver.SaveProgress(0);
					}
					else if (this.levelNumber >= 100)
					{
						GameProgressSaver.SetEncoreProgress(this.levelNumber - 99);
					}
					else
					{
						GameProgressSaver.SaveProgress(this.levelNumber + 1);
					}
					fr.targetLevelName = this.targetLevelName;
				}
				if (this.secondPit)
				{
					fr.finalPitPos = base.transform.position;
					fr.reachedSecondPit = true;
				}
				if (!this.sm.infoSent)
				{
					this.sm.SendInfo();
					return;
				}
			}
			else if (this.secondPit)
			{
				GameProgressSaver.SetTutorial(true);
				FinalRank fr2 = MonoSingleton<StatsManager>.Instance.fr;
				fr2.gameObject.SetActive(true);
				fr2.finalPitPos = base.transform.position;
				fr2.RanklessNextLevel(this.targetLevelName);
			}
		}
	}

	// Token: 0x04000C1E RID: 3102
	private NewMovement nmov;

	// Token: 0x04000C1F RID: 3103
	private StatsManager sm;

	// Token: 0x04000C20 RID: 3104
	private Rigidbody rb;

	// Token: 0x04000C21 RID: 3105
	private bool rotationReady;

	// Token: 0x04000C22 RID: 3106
	private GameObject player;

	// Token: 0x04000C23 RID: 3107
	private bool infoSent;

	// Token: 0x04000C24 RID: 3108
	public bool rankless;

	// Token: 0x04000C25 RID: 3109
	public bool secondPit;

	// Token: 0x04000C26 RID: 3110
	public string targetLevelName;

	// Token: 0x04000C27 RID: 3111
	private int levelNumber;

	// Token: 0x04000C28 RID: 3112
	public bool musicFadeOut;

	// Token: 0x04000C29 RID: 3113
	private Quaternion targetRotation;
}
