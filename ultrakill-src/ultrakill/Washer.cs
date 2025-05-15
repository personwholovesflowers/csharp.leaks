using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public class Washer : MonoBehaviour
{
	// Token: 0x06001DD2 RID: 7634 RVA: 0x000F8A38 File Offset: 0x000F6C38
	private void Start()
	{
		this.collisionEvents = new List<ParticleCollisionEvent>();
		this.defaultSprayPos = this.correctCameraView.transform.localPosition;
		this.defaultSprayRot = this.correctCameraView.transform.localRotation;
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x000F8A71 File Offset: 0x000F6C71
	private void OnEnable()
	{
		this.part = base.GetComponent<ParticleSystem>();
		this.part.Stop();
		this.aud = base.GetComponent<AudioSource>();
		this.aud.Stop();
		this.inputManager = MonoSingleton<InputManager>.Instance;
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x000F8AAC File Offset: 0x000F6CAC
	private void Update()
	{
		Transform transform = MonoSingleton<CameraController>.Instance.transform;
		RaycastHit raycastHit;
		if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 50f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			if (raycastHit.distance < 2.25f)
			{
				base.transform.position = transform.position;
				base.transform.rotation = transform.rotation;
				this.correctCameraView.canModifyTarget = false;
			}
			else
			{
				this.correctCameraView.canModifyTarget = true;
			}
		}
		if (MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			if (this.inputManager.InputSource.Fire1.IsPressed && !this.isSpraying)
			{
				this.StartWashing();
			}
			else if (!this.inputManager.InputSource.Fire1.IsPressed && this.isSpraying)
			{
				this.StopWashing();
			}
			if (this.inputManager.InputSource.Fire2.WasPerformedThisFrame)
			{
				this.SwitchNozzle();
			}
		}
		float num = (float)((double)Time.time % 6.283185);
		this.aud.pitch = ((this.nozzleMode == 2) ? 2.1f : 1.1f) + Mathf.Sin(num) * 0.025f;
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000F8BF4 File Offset: 0x000F6DF4
	private void SwitchNozzle()
	{
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.PlayOneShot(this.click);
		this.nozzleMode = (this.nozzleMode + 1) % 3;
		for (int i = 0; i < this.nozzles.Length; i++)
		{
			this.nozzles[i].SetActive(i == this.nozzleMode);
		}
		this.shapeModule = this.part.shape;
		this.mainModule = this.part.main;
		ParticleSystem.EmissionModule emission = this.part.emission;
		if (this.nozzleMode == 0)
		{
			this.mainModule.startLifetime = 0.5f;
			this.mainModule.startSpeed = 100f;
			emission.rateOverTime = 1000f;
			this.shapeModule.angle = 11f;
			this.shapeModule.rotation = new Vector3(0f, 0f, 0f);
			this.shapeModule.scale = new Vector3(0.1f, 1f, 1f);
		}
		if (this.nozzleMode == 1)
		{
			this.mainModule.startLifetime = 0.5f;
			this.mainModule.startSpeed = 100f;
			emission.rateOverTime = 1000f;
			this.shapeModule.angle = 11f;
			this.shapeModule.rotation = new Vector3(0f, 0f, 90f);
			this.shapeModule.scale = new Vector3(0.1f, 1f, 1f);
		}
		if (this.nozzleMode == 2)
		{
			this.mainModule.startLifetime = 1.2f;
			this.mainModule.startSpeed = 100f;
			emission.rateOverTime = 700f;
			this.shapeModule.angle = 0.75f;
			this.shapeModule.scale = Vector3.one;
		}
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x000F8E20 File Offset: 0x000F7020
	private void StartWashing()
	{
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.PlayOneShot(this.triggerOn);
		this.isSpraying = true;
		this.part.Play();
		this.aud.Play();
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000F8E78 File Offset: 0x000F7078
	private void StopWashing()
	{
		this.isSpraying = false;
		this.part.Stop();
		this.aud.Stop();
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.PlayOneShot(this.triggerOff);
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000F8ED0 File Offset: 0x000F70D0
	private void OnParticleCollision(GameObject other)
	{
		BloodAbsorber bloodAbsorber;
		BloodAbsorberChild bloodAbsorberChild;
		if (other.TryGetComponent<BloodAbsorber>(out bloodAbsorber))
		{
			if (!this.musicStarted)
			{
				if (this.music)
				{
					this.music.SetActive(true);
				}
				this.musicStarted = true;
			}
			Vector3 position = this.part.transform.position;
			this.part.GetCollisionEvents(other, this.collisionEvents);
			bloodAbsorber.ProcessWasherSpray(ref this.collisionEvents, position, null);
		}
		else if (other.TryGetComponent<BloodAbsorberChild>(out bloodAbsorberChild))
		{
			if (!this.musicStarted)
			{
				if (this.music)
				{
					this.music.SetActive(true);
				}
				this.musicStarted = true;
			}
			Vector3 position2 = this.part.transform.position;
			this.part.GetCollisionEvents(other, this.collisionEvents);
			bloodAbsorberChild.ProcessWasherSpray(ref this.collisionEvents, position2);
			SpinFromForce spinFromForce;
			if (other.TryGetComponent<SpinFromForce>(out spinFromForce))
			{
				spinFromForce.AddSpin(ref this.collisionEvents);
			}
		}
		GameObject gameObject = other.gameObject;
		EnemyIdentifier enemyIdentifier;
		if (gameObject.layer == 12 && gameObject.TryGetComponent<EnemyIdentifier>(out enemyIdentifier) && enemyIdentifier.enemyType == EnemyType.Streetcleaner && !enemyIdentifier.dead)
		{
			enemyIdentifier.InstaKill();
		}
	}

	// Token: 0x04002A3F RID: 10815
	private bool isSpraying;

	// Token: 0x04002A40 RID: 10816
	public ParticleSystem part;

	// Token: 0x04002A41 RID: 10817
	public List<ParticleCollisionEvent> collisionEvents;

	// Token: 0x04002A42 RID: 10818
	private InputManager inputManager;

	// Token: 0x04002A43 RID: 10819
	private AudioSource aud;

	// Token: 0x04002A44 RID: 10820
	[SerializeField]
	private AudioClip click;

	// Token: 0x04002A45 RID: 10821
	[SerializeField]
	private AudioClip triggerOn;

	// Token: 0x04002A46 RID: 10822
	[SerializeField]
	private AudioClip triggerOff;

	// Token: 0x04002A47 RID: 10823
	private ParticleSystem.ShapeModule shapeModule;

	// Token: 0x04002A48 RID: 10824
	private ParticleSystem.MainModule mainModule;

	// Token: 0x04002A49 RID: 10825
	[SerializeField]
	private GameObject[] nozzles;

	// Token: 0x04002A4A RID: 10826
	private bool musicStarted;

	// Token: 0x04002A4B RID: 10827
	[SerializeField]
	private GameObject music;

	// Token: 0x04002A4C RID: 10828
	private Vector3 defaultSprayPos;

	// Token: 0x04002A4D RID: 10829
	private Quaternion defaultSprayRot;

	// Token: 0x04002A4E RID: 10830
	private int nozzleMode;

	// Token: 0x04002A4F RID: 10831
	public CorrectCameraView correctCameraView;
}
