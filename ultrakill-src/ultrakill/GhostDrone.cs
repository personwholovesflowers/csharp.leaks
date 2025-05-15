using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

// Token: 0x02000508 RID: 1288
public class GhostDrone : MonoBehaviour
{
	// Token: 0x06001D6F RID: 7535 RVA: 0x000F6620 File Offset: 0x000F4820
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.avoidanceMask = LayerMaskDefaults.Get(LMD.Environment);
		this.aggroLight = base.GetComponentInChildren<Light>();
		this.aud = base.GetComponent<AudioSource>();
		this.startLightColor = this.aggroLight.color;
		this.startLightIntensity = this.aggroLight.intensity;
		this.pt = MonoSingleton<PlayerTracker>.Instance;
		this.animator = base.GetComponent<Animator>();
		this.originalPos = base.transform.position;
		this.crt = base.StartCoroutine(this.Fly());
		this.variableAttackSpeed = this.attackSpeed + Random.Range(-this.attackSpeed / 2f, this.attackSpeed / 2f);
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x000F66E4 File Offset: 0x000F48E4
	private void Update()
	{
		this.TryFindPlayer();
		if ((this.lastIdleSound > Random.Range(3f, 7f) && this.isAttacking) || this.lastIdleSound > Random.Range(8f, 12f))
		{
			if (!this.aud.isPlaying)
			{
				this.aud.clip = this.idleSounds[Random.Range(0, this.idleSounds.Length)];
				this.aud.pitch = Random.Range(0.9f, 1.1f);
				this.aud.Play();
			}
			this.lastIdleSound = 0f;
		}
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x000F679C File Offset: 0x000F499C
	private void LateUpdate()
	{
		this.isSucked = this.vacuumVelocity.magnitude > 1E-05f;
		if (this.isSucked)
		{
			this.animator.speed = 4f;
			this.isAttacking = false;
			if (!this.scaredAudioSource.isPlaying)
			{
				this.scaredAudioSource.pitch = Random.Range(0.8f, 1.2f);
				this.scaredAudioSource.Play();
			}
		}
		else
		{
			this.scaredAudioSource.Stop();
		}
		this.animator.SetBool(GhostDrone.IsScared, this.isSucked);
		if (this.wasSuckedLastFrame != this.isSucked)
		{
			if (this.isSucked)
			{
				base.StopCoroutine(this.crt);
				this.aggroLight.color = this.startLightColor;
				this.aggroLight.intensity = this.startLightIntensity;
			}
			else
			{
				this.crt = base.StartCoroutine(this.Fly());
			}
		}
		if (this.isSucked)
		{
			Vector3 normalized = (this.pt.GetTarget().position - base.transform.position).normalized;
			base.transform.rotation = Quaternion.LookRotation(normalized);
		}
		this.rb.position += this.vacuumVelocity * Time.deltaTime;
		this.vacuumVelocity = Vector3.zero;
		this.wasSuckedLastFrame = this.isSucked;
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000F690C File Offset: 0x000F4B0C
	private void TryFindPlayer()
	{
		if (!this.isAttacking && !this.isSucked)
		{
			Vector3 position = this.pt.GetTarget().position;
			Vector3 normalized = (position - base.transform.position).normalized;
			float num = Vector3.Angle(base.transform.forward, normalized);
			float num2 = Vector3.Distance(position, base.transform.position);
			if (this.alwaysAggro || (num2 <= this.detectionDistance && num < this.detectionAngle))
			{
				if (this.crt != null)
				{
					base.StopCoroutine(this.crt);
				}
				this.aud.clip = this.spottedSound;
				this.aud.pitch = Random.Range(0.9f, 1.1f);
				this.aud.Play();
				this.killZone.SetActive(true);
				base.transform.rotation = Quaternion.LookRotation(normalized);
				this.isAttacking = true;
				this.crt = base.StartCoroutine(this.Attack());
			}
		}
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x000F6A1C File Offset: 0x000F4C1C
	public void KillGhost()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.deathExplosion, base.transform.position, base.transform.rotation);
		gameObject.transform.localScale = Vector3.one * 0.1f;
		gameObject.GetComponent<AudioSource>().volume = 0.1f;
		gameObject.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.1f;
		this.ghostDeathSound.PlayClipAtPoint(this.audioGroup, base.transform.position, 128, 1f, 1f, Random.Range(0.8f, 1.1f), AudioRolloffMode.Linear, 1f, 100f);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x000F6ADF File Offset: 0x000F4CDF
	private IEnumerator Attack()
	{
		this.aggroLight.color = Color.red;
		this.aggroLight.intensity = this.startLightIntensity * 2f;
		this.animator.speed = 4f * (this.variableAttackSpeed / this.attackSpeed);
		while (this.alwaysAggro || Vector3.Distance(this.pt.GetTarget().position, base.transform.position) < 30f)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = this.pt.GetTarget().position;
			Quaternion rotation = base.transform.rotation;
			Quaternion quaternion = Quaternion.LookRotation((position2 - position).normalized);
			float num = this.variableAttackSpeed * Time.deltaTime;
			float num2 = Mathf.SmoothStep(0f, 1f, num * 4f);
			base.transform.rotation = Quaternion.Slerp(rotation, quaternion, num2);
			base.transform.position = Vector3.MoveTowards(base.transform.position, position2, num);
			yield return null;
		}
		this.aud.clip = this.lostSound;
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.Play();
		this.killZone.SetActive(false);
		this.isAttacking = false;
		base.StopCoroutine(this.crt);
		this.crt = base.StartCoroutine(this.Fly());
		yield break;
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x000F6AEE File Offset: 0x000F4CEE
	private IEnumerator Fly()
	{
		this.aggroLight.color = this.startLightColor;
		this.aggroLight.intensity = this.startLightIntensity;
		this.animator.speed = 0.5f;
		for (;;)
		{
			Vector3 startPos = base.transform.position;
			bool noTarget = true;
			Vector3 targetPos = Vector3.one;
			while (noTarget)
			{
				targetPos = this.RandomNavmeshLocation(60f);
				Vector3 normalized = (targetPos - startPos).normalized;
				if (!Physics.Raycast(startPos, normalized, (targetPos - startPos).magnitude, this.avoidanceMask))
				{
					noTarget = false;
				}
				yield return null;
			}
			if (targetPos != startPos)
			{
				targetPos.y += 5f;
				Quaternion startDir = base.transform.rotation;
				float num = Vector3.Distance(startPos, targetPos);
				Vector3 normalized2 = (targetPos - startPos).normalized;
				Quaternion lookDir = Quaternion.LookRotation(normalized2);
				float num2 = num / this.idleSpeed;
				float elapsed = 0f;
				this.animator.speed = 1f;
				while (Vector3.Distance(base.transform.position, targetPos) > 0.1f)
				{
					elapsed += Time.deltaTime;
					float num3 = Mathf.SmoothStep(0f, 1f, elapsed);
					base.transform.rotation = Quaternion.Slerp(startDir, lookDir, num3);
					float num4 = this.idleSpeed * Time.deltaTime;
					base.transform.position = Vector3.MoveTowards(base.transform.position, targetPos, num4);
					yield return null;
				}
				this.animator.speed = 0.5f;
				yield return new WaitForSeconds(1.5f);
				startDir = default(Quaternion);
				lookDir = default(Quaternion);
			}
			startPos = default(Vector3);
			targetPos = default(Vector3);
		}
		yield break;
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x000F6B00 File Offset: 0x000F4D00
	public Vector3 RandomNavmeshLocation(float radius)
	{
		Vector3 vector = Random.insideUnitSphere * radius + this.originalPos;
		Vector3 vector2 = Vector3.zero;
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(vector, out navMeshHit, radius, 1))
		{
			vector2 = navMeshHit.position;
		}
		return vector2;
	}

	// Token: 0x040029AF RID: 10671
	private LayerMask avoidanceMask;

	// Token: 0x040029B0 RID: 10672
	public float detectionAngle = 45f;

	// Token: 0x040029B1 RID: 10673
	public float detectionDistance = 12f;

	// Token: 0x040029B2 RID: 10674
	public float idleSpeed = 2f;

	// Token: 0x040029B3 RID: 10675
	public float attackSpeed = 6f;

	// Token: 0x040029B4 RID: 10676
	private float variableAttackSpeed;

	// Token: 0x040029B5 RID: 10677
	[HideInInspector]
	public Vector3 vacuumVelocity;

	// Token: 0x040029B6 RID: 10678
	private Vector3 originalPos = Vector3.zero;

	// Token: 0x040029B7 RID: 10679
	private Animator animator;

	// Token: 0x040029B8 RID: 10680
	private PlayerTracker pt;

	// Token: 0x040029B9 RID: 10681
	private Coroutine crt;

	// Token: 0x040029BA RID: 10682
	private bool isAttacking;

	// Token: 0x040029BB RID: 10683
	public bool alwaysAggro;

	// Token: 0x040029BC RID: 10684
	[SerializeField]
	private GameObject killZone;

	// Token: 0x040029BD RID: 10685
	private Light aggroLight;

	// Token: 0x040029BE RID: 10686
	private Color startLightColor;

	// Token: 0x040029BF RID: 10687
	private float startLightIntensity;

	// Token: 0x040029C0 RID: 10688
	private AudioSource aud;

	// Token: 0x040029C1 RID: 10689
	[SerializeField]
	private AudioClip spottedSound;

	// Token: 0x040029C2 RID: 10690
	[SerializeField]
	private AudioClip lostSound;

	// Token: 0x040029C3 RID: 10691
	[SerializeField]
	private AudioClip[] idleSounds;

	// Token: 0x040029C4 RID: 10692
	private TimeSince lastIdleSound;

	// Token: 0x040029C5 RID: 10693
	private Rigidbody rb;

	// Token: 0x040029C6 RID: 10694
	public AudioMixerGroup audioGroup;

	// Token: 0x040029C7 RID: 10695
	public GameObject deathExplosion;

	// Token: 0x040029C8 RID: 10696
	[SerializeField]
	private AudioClip ghostDeathSound;

	// Token: 0x040029C9 RID: 10697
	private bool isSucked;

	// Token: 0x040029CA RID: 10698
	private bool wasSuckedLastFrame;

	// Token: 0x040029CB RID: 10699
	private static readonly int IsScared = Animator.StringToHash("IsScared");

	// Token: 0x040029CC RID: 10700
	[SerializeField]
	private AudioSource scaredAudioSource;
}
