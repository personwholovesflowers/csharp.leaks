using System;
using TMPro;
using UnityEngine;

// Token: 0x02000456 RID: 1110
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class StyleCalculator : MonoSingleton<StyleCalculator>
{
	// Token: 0x0600195D RID: 6493 RVA: 0x000D0288 File Offset: 0x000CE488
	private void Start()
	{
		this.shud = MonoSingleton<StyleHUD>.Instance;
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.player = this.nmov.gameObject;
		this.airTimePos = this.airTimeText.transform.localPosition;
		this.sman = MonoSingleton<StatsManager>.Instance;
		this.gc = this.nmov.GetComponentInChildren<GunControl>();
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x000D02F0 File Offset: 0x000CE4F0
	private void Update()
	{
		if (!this.nmov.gc.onGround || this.nmov.sliding)
		{
			this.airTime = Mathf.MoveTowards(this.airTime, 3f, Time.deltaTime * 2f);
			if (!this.airTimeText.gameObject.activeSelf)
			{
				this.airTimeText.gameObject.SetActive(true);
			}
		}
		else if (!this.nmov.boost)
		{
			this.airTime = Mathf.MoveTowards(this.airTime, 1f, Time.deltaTime * 10f);
			this.airTimeText.transform.localPosition = this.airTimePos;
		}
		if (this.airTime >= 2f && this.airTime < 3f)
		{
			if (this.lastAirTime != this.airTime)
			{
				this.airTimeText.text = "<color=orange><size=60>x" + this.airTime.ToString("F2") + "</size></color>";
			}
			this.airTimeText.transform.localPosition = new Vector3(this.airTimePos.x + (float)Random.Range(-3, 3), this.airTimePos.y + (float)Random.Range(-3, 3), this.airTimePos.z);
		}
		else if (this.airTime == 3f)
		{
			if (this.lastAirTime != this.airTime)
			{
				this.airTimeText.text = this.maxAirTime;
			}
			this.airTimeText.transform.localPosition = new Vector3(this.airTimePos.x + (float)Random.Range(-6, 6), this.airTimePos.y + (float)Random.Range(-6, 6), this.airTimePos.z);
		}
		else if (this.airTime == 1f && this.airTimeText.gameObject.activeSelf)
		{
			this.airTimeText.gameObject.SetActive(false);
		}
		else
		{
			if (this.lastAirTime != this.airTime)
			{
				this.airTimeText.text = "x" + this.airTime.ToString("F2");
			}
			this.airTimeText.transform.localPosition = this.airTimePos;
		}
		if (this.multikillTimer > 0f)
		{
			this.multikillTimer -= Time.deltaTime * 10f;
		}
		else if (this.multikillCount != 0)
		{
			this.multikillTimer = 0f;
			this.multikillCount = 0;
		}
		this.lastAirTime = this.airTime;
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x000D0590 File Offset: 0x000CE790
	public void HitCalculator(string hitter, string enemyType, string hitLimb, bool dead, EnemyIdentifier eid = null, GameObject sourceWeapon = null)
	{
		if (eid != null && eid.blessed)
		{
			return;
		}
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
		{
			return;
		}
		if (hitter == "punch" || hitter == "heavypunch")
		{
			if (dead)
			{
				if (hitLimb == "head" || hitLimb == "limb")
				{
					this.AddPoints(60, "ultrakill.criticalpunch", eid, sourceWeapon);
				}
				else if (enemyType == "spider")
				{
					this.AddPoints(150, "ultrakill.bigfistkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(30, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else if (enemyType == "spider")
			{
				this.AddPoints(60, "ultrakill.disrespect", eid, sourceWeapon);
			}
			else
			{
				this.AddPoints(20, "", eid, sourceWeapon);
			}
		}
		else if (hitter == "ground slam")
		{
			if (dead)
			{
				this.AddPoints(60, "ultrakill.groundslam", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(20, "", eid, sourceWeapon);
			}
		}
		else if (hitter == "revolver")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (hitLimb == "head" && enemyType == "spider")
				{
					this.AddPoints(150, "ultrakill.bigheadshot", eid, sourceWeapon);
				}
				else if (hitLimb == "head")
				{
					this.AddPoints(80, "ultrakill.headshot", eid, sourceWeapon);
				}
				else if (hitLimb == "limb")
				{
					this.AddPoints(60, "ultrakill.limbhit", eid, sourceWeapon);
				}
				else if (enemyType == "spider")
				{
					this.AddPoints(100, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(30, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else if (hitLimb == "head")
			{
				this.AddPoints(25, "", eid, sourceWeapon);
			}
			else if (hitLimb == "limb")
			{
				this.AddPoints(15, "", eid, sourceWeapon);
			}
			else
			{
				this.AddPoints(10, "", eid, sourceWeapon);
			}
		}
		else if (hitter == "shotgun")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(100, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(45, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(4, "ultrakill.shotgunhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "hammer")
		{
			if (dead)
			{
				this.gc.AddKill();
			}
		}
		else if (hitter == "shotgunzone")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(125, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(100, "ultrakill.overkill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
		}
		else if (hitter == "nail" || hitter == "sawblade")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(100, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(45, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(2, "ultrakill.nailhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "railcannon")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(100, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(45, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(15, "", eid, sourceWeapon);
			}
		}
		else if (hitter == "zapper")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(175, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(80, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(60, "ultrakill.zapperhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "lightningbolt")
		{
			this.enemiesShot = true;
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(300, "ultrakill.lightningbolt", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(250, "ultrakill.lightningbolt", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(200, "ultrakill.lightningbolt", eid, sourceWeapon);
			}
		}
		else if (hitter == "projectile")
		{
			if (dead)
			{
				this.AddPoints(250, "ultrakill.friendlyfire", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(200, "ultrakill.friendlyfire", eid, sourceWeapon);
			}
		}
		else if (hitter == "ffexplosion")
		{
			if (dead)
			{
				this.AddPoints(250, "ultrakill.friendlyfire", eid, sourceWeapon);
				this.AddPoints(0, "ultrakill.exploded", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(200, "ultrakill.friendlyfire", eid, sourceWeapon);
			}
		}
		else if (hitter == "explosion")
		{
			if (dead)
			{
				this.AddPoints(45, "ultrakill.exploded", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(15, "ultrakill.explosionhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "fire")
		{
			if (dead)
			{
				this.AddPoints(20, "FRIED", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(2, "ultrakill.firehit", eid, sourceWeapon);
			}
		}
		else if (hitter == "harpoon")
		{
			if (dead)
			{
				if (enemyType == "spider")
				{
					this.AddPoints(100, "ultrakill.bigkill", eid, sourceWeapon);
				}
				else
				{
					this.AddPoints(45, "ultrakill.kill", eid, sourceWeapon);
				}
				this.gc.AddKill();
			}
		}
		else if (hitter == "chainsawprojectile")
		{
			if (dead)
			{
				this.AddPoints(80, "UNCHAINEDSAW", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(8, "ultrakill.nailhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "chainsaw")
		{
			if (dead)
			{
				this.AddPoints(60, "NO-NO", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(4, "ultrakill.nailhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "chainsawbounce")
		{
			if (dead)
			{
				this.AddPoints(100, "RE-NO-NO", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(6, "ultrakill.nailhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "chainsawzone")
		{
			if (dead)
			{
				this.AddPoints(60, "GROOVY", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(4, "ultrakill.drillhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "drill")
		{
			if (dead)
			{
				this.AddPoints(120, "SCREWED", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(4, "ultrakill.drillhit", eid, sourceWeapon);
			}
		}
		else if (hitter == "drillpunch")
		{
			if (dead)
			{
				this.AddPoints(120, "ultrakill.drillpunchkill", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(60, "ultrakill.drillpunch", eid, sourceWeapon);
			}
		}
		else if (hitter == "cannonball")
		{
			if (dead)
			{
				this.AddPoints(75, "ultrakill.cannonballed", eid, sourceWeapon);
				this.gc.AddKill();
			}
			else
			{
				this.AddPoints(25, "", eid, sourceWeapon);
			}
		}
		if (dead && !eid.puppet && hitter != "secret")
		{
			this.AddToMultiKill(sourceWeapon);
		}
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x000D0EC8 File Offset: 0x000CF0C8
	public void AddToMultiKill(GameObject sourceWeapon = null)
	{
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
		{
			return;
		}
		this.multikillCount++;
		this.multikillTimer = 5f;
		switch (this.multikillCount)
		{
		case 0:
		case 1:
			break;
		case 2:
			this.shud.AddPoints(25, "ultrakill.doublekill", sourceWeapon, null, -1, "", "");
			return;
		case 3:
			this.shud.AddPoints(50, "ultrakill.triplekill", sourceWeapon, null, -1, "", "");
			return;
		default:
		{
			StyleHUD styleHUD = this.shud;
			int num = 100;
			string text = "ultrakill.multikill";
			int num2 = this.multikillCount;
			styleHUD.AddPoints(num, text, sourceWeapon, null, num2, "", "");
			break;
		}
		}
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000D0F84 File Offset: 0x000CF184
	private void AddPoints(int points, string pointName, EnemyIdentifier eid, GameObject sourceWeapon = null)
	{
		int num = Mathf.RoundToInt((float)points * this.airTime - (float)points);
		this.shud.AddPoints(points + num, pointName, sourceWeapon, eid, -1, "", "");
	}

	// Token: 0x04002393 RID: 9107
	public StyleHUD shud;

	// Token: 0x04002394 RID: 9108
	private GameObject player;

	// Token: 0x04002395 RID: 9109
	private NewMovement nmov;

	// Token: 0x04002396 RID: 9110
	public TMP_Text airTimeText;

	// Token: 0x04002397 RID: 9111
	public float airTime = 1f;

	// Token: 0x04002398 RID: 9112
	private Vector3 airTimePos;

	// Token: 0x04002399 RID: 9113
	private StatsManager sman;

	// Token: 0x0400239A RID: 9114
	private GunControl gc;

	// Token: 0x0400239B RID: 9115
	public bool enemiesShot;

	// Token: 0x0400239C RID: 9116
	public float multikillTimer;

	// Token: 0x0400239D RID: 9117
	public int multikillCount;

	// Token: 0x0400239E RID: 9118
	private string maxAirTime = "<color=red><size=72>x3.00</size></color>";

	// Token: 0x0400239F RID: 9119
	private float lastAirTime;
}
