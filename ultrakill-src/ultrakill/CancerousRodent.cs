using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class CancerousRodent : MonoBehaviour
{
	// Token: 0x06000322 RID: 802 RVA: 0x00012EEE File Offset: 0x000110EE
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00012EFC File Offset: 0x000110FC
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		base.GetComponent<Collider>();
		if (this.harmless)
		{
			this.mach = base.GetComponent<Machine>();
		}
		else
		{
			this.stat = base.GetComponent<Statue>();
		}
		this.eid = base.GetComponent<EnemyIdentifier>();
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00012F4C File Offset: 0x0001114C
	private void OnDisable()
	{
		if (!this.harmless && (!this.stat || this.stat.health <= 0f))
		{
			foreach (GameObject gameObject in this.activateOnDeath)
			{
				if (gameObject)
				{
					gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00012FA8 File Offset: 0x000111A8
	private void Update()
	{
		if (this.rb != null)
		{
			if (this.eid.target == null)
			{
				this.rb.velocity = Vector3.zero;
			}
			else if (this.eid.target != null)
			{
				base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
				this.rb.velocity = base.transform.forward * Time.deltaTime * 100f * this.eid.totalSpeedModifier;
			}
		}
		if (this.harmless)
		{
			if (this.mach.health <= 0f)
			{
				foreach (GameObject gameObject in this.activateOnDeath)
				{
					if (gameObject)
					{
						gameObject.SetActive(true);
					}
				}
				Object.Destroy(base.GetComponentInChildren<Light>().gameObject);
				Object.Destroy(this);
				return;
			}
		}
		else if (this.stat.health > 0f)
		{
			if (this.eid.target == null)
			{
				return;
			}
			if (this.coolDown != 0f)
			{
				this.coolDown = Mathf.MoveTowards(this.coolDown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
				return;
			}
			if (!Physics.Raycast(this.shootPoint.position, this.eid.target.position - this.shootPoint.position, Vector3.Distance(this.eid.target.position, this.shootPoint.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.coolDown = 3f;
				this.currentProjectiles = this.projectileAmount;
				this.FireBurst();
			}
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000131AC File Offset: 0x000113AC
	private void FireBurst()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.projectile, this.shootPoint.position, this.shootPoint.rotation);
		gameObject.GetComponent<Rigidbody>().AddForce(this.shootPoint.forward * 2f, ForceMode.VelocityChange);
		Projectile projectile;
		if (gameObject.TryGetComponent<Projectile>(out projectile))
		{
			projectile.target = this.eid.target;
			projectile.damage *= this.eid.totalDamageModifier;
		}
		this.currentProjectiles--;
		if (this.currentProjectiles > 0)
		{
			base.Invoke("FireBurst", 0.1f * this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x040003DF RID: 991
	private Rigidbody rb;

	// Token: 0x040003E0 RID: 992
	private Machine mach;

	// Token: 0x040003E1 RID: 993
	private Statue stat;

	// Token: 0x040003E2 RID: 994
	private EnemyIdentifier eid;

	// Token: 0x040003E3 RID: 995
	public bool harmless;

	// Token: 0x040003E4 RID: 996
	public GameObject[] activateOnDeath;

	// Token: 0x040003E5 RID: 997
	public Transform shootPoint;

	// Token: 0x040003E6 RID: 998
	public GameObject projectile;

	// Token: 0x040003E7 RID: 999
	private float coolDown = 2f;

	// Token: 0x040003E8 RID: 1000
	public int projectileAmount;

	// Token: 0x040003E9 RID: 1001
	private int currentProjectiles;
}
