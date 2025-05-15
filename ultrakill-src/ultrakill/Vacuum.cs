using System;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public class Vacuum : MonoBehaviour
{
	// Token: 0x06001DC3 RID: 7619 RVA: 0x000F8054 File Offset: 0x000F6254
	private void Update()
	{
		if (MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			this.UpdateInput();
		}
		if (!this._isSucking && !this._isBlowing)
		{
			return;
		}
		if (!this._suckSound.isPlaying || this._suckSound.time > this._suckSound.clip.length - 0.1f)
		{
			if (this._stuckObject.rigidbody != null)
			{
				this._suckSound.clip = this.suckStuckLoopSound;
			}
			else if (this._isBlowing)
			{
				this._suckSound.clip = this.blowLoopSound;
			}
			else
			{
				this._suckSound.clip = this.suckLoopSound;
			}
			this._suckSound.loop = true;
			this._suckSound.Play();
		}
		float num = (float)((double)Time.time % 6.283185);
		this._suckSound.pitch = 1.1f + Mathf.Sin(num) * 0.025f;
		this.SuckObjects();
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000F815E File Offset: 0x000F635E
	private void FixedUpdate()
	{
		if (!this._isSucking)
		{
			return;
		}
		this.UpdateStuckObject();
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000F8170 File Offset: 0x000F6370
	private void UpdateInput()
	{
		if (!this._isBlowing && MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed)
		{
			this.StartBlowing();
		}
		else if (this._isBlowing && !MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed)
		{
			this.StopBlowing();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && !this._isBlowing && !this._isSucking)
		{
			this.StartVacuuming();
			return;
		}
		if (this._isSucking && !MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
		{
			this.StopVacuuming();
		}
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x000F8218 File Offset: 0x000F6418
	private void UpdateStuckObject()
	{
		if (this._stuckObject.rigidbody == null)
		{
			return;
		}
		Vector3 vector = this._suckPoint.position - this._stuckObject.rigidbody.worldCenterOfMass;
		this._stuckObject.rigidbody.velocity = vector / Time.fixedDeltaTime;
		float num = Camera.main.transform.eulerAngles.y - this._lastCameraRotation.y;
		num *= 0.017453292f;
		this._stuckObject.rigidbody.angularVelocity = new Vector3(0f, num / Time.fixedDeltaTime, 0f);
		this._lastCameraRotation = Camera.main.transform.eulerAngles;
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000F82DC File Offset: 0x000F64DC
	private void UpdateColliders()
	{
		int i = this._colliders.Offset;
		int num = i + this._colliders.Count;
		while (i < num)
		{
			Collider collider = this._colliders.Array[i];
			Cork cork;
			if (!(collider == null) && !(collider.attachedRigidbody == null) && collider.attachedRigidbody.TryGetComponent<Cork>(out cork))
			{
				cork.insideSuckZone = false;
			}
			i++;
		}
		Vector3 vector = this._suckBox.transform.TransformPoint(this._suckBox.center);
		Vector3 vector2 = Vector3.Scale(this._suckBox.size, this._suckBox.transform.lossyScale) * 0.5f;
		this._colliders = new ArraySegment<Collider>(this._colliders.Array, 0, Physics.OverlapBoxNonAlloc(vector, vector2, this._colliders.Array, this._suckBox.transform.rotation));
		int j = this._colliders.Offset;
		int num2 = j + this._colliders.Count;
		while (j < num2)
		{
			Collider collider2 = this._colliders.Array[j];
			Cork cork2;
			if (!(collider2 == null) && !(collider2.attachedRigidbody == null) && collider2.attachedRigidbody.TryGetComponent<Cork>(out cork2))
			{
				cork2.insideSuckZone = true;
			}
			j++;
		}
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000F843C File Offset: 0x000F663C
	private void SuckObjects()
	{
		if ((!this._isSucking && !this._isBlowing) || this._stuckObject.rigidbody != null)
		{
			return;
		}
		this.UpdateColliders();
		int i = this._colliders.Offset;
		int num = i + this._colliders.Count;
		while (i < num)
		{
			Collider collider = this._colliders.Array[i];
			NewMovement newMovement;
			if (!(collider == null) && !(collider.attachedRigidbody == null) && !collider.attachedRigidbody.TryGetComponent<NewMovement>(out newMovement))
			{
				Rigidbody attachedRigidbody = collider.attachedRigidbody;
				Cork cork;
				if (this._isSucking && attachedRigidbody.TryGetComponent<Cork>(out cork))
				{
					cork.StartWiggle();
				}
				TerribleTasteBook terribleTasteBook;
				if (this._isSucking && collider.TryGetComponent<TerribleTasteBook>(out terribleTasteBook))
				{
					terribleTasteBook.ActivateBookShelf();
				}
				GhostDrone ghostDrone = null;
				if (this._isSucking && attachedRigidbody.TryGetComponent<GhostDrone>(out ghostDrone))
				{
					Vector3 vector = this._suckPoint.position - attachedRigidbody.position;
					Vector3 vector2 = vector.normalized * Mathf.Max(1f / vector.sqrMagnitude, 2f);
					ghostDrone.vacuumVelocity = vector2;
				}
				if (this._isSucking)
				{
					attachedRigidbody.velocity = Vector3.Normalize(this._suckPoint.position - attachedRigidbody.worldCenterOfMass) * this._suckStrength;
				}
				else
				{
					attachedRigidbody.velocity = MonoSingleton<CameraController>.Instance.transform.forward.normalized * this._suckStrength * 2f;
				}
				if (!this._isBlowing && Vector3.Distance(attachedRigidbody.worldCenterOfMass, this._suckPoint.position) < this._stuckDistance)
				{
					GoreSplatter goreSplatter;
					if (!attachedRigidbody.TryGetComponent<GoreSplatter>(out goreSplatter))
					{
						if (!ghostDrone)
						{
							this.SetStuckObject(attachedRigidbody);
							return;
						}
						ghostDrone.KillGhost();
					}
					else
					{
						if (!this.musicStarted)
						{
							if (this.music)
							{
								this.music.SetActive(true);
							}
							this.musicStarted = true;
						}
						this._consumeSound.pitch = Random.Range(0.9f, 1.1f);
						this._consumeSound.PlayOneShot(this._consumeSound.clip);
						goreSplatter.Repool();
					}
				}
			}
			i++;
		}
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000F8688 File Offset: 0x000F6888
	private void StopCorkPull()
	{
		int i = this._colliders.Offset;
		int num = i + this._colliders.Count;
		while (i < num)
		{
			Collider collider = this._colliders.Array[i];
			Cork cork;
			if (!(collider == null) && !(collider.attachedRigidbody == null) && collider.attachedRigidbody.TryGetComponent<Cork>(out cork))
			{
				cork.StopWiggle();
			}
			i++;
		}
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000F86F4 File Offset: 0x000F68F4
	private void StartVacuuming()
	{
		if (this._isBlowing)
		{
			this.StopBlowing();
		}
		this._suckSystem.Play();
		this._suckSound.clip = this.suckStartSound;
		this._suckSound.loop = false;
		this._suckSound.Play();
		this._isSucking = true;
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000F874C File Offset: 0x000F694C
	private void StopVacuuming()
	{
		this._suckSystem.Stop();
		this._suckSound.clip = this.suckStopSound;
		this._suckSound.loop = false;
		this._suckSound.Play();
		this._isSucking = false;
		this.StopCorkPull();
		this.SetStuckObject(null);
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000F87A0 File Offset: 0x000F69A0
	private void StartBlowing()
	{
		if (this._isSucking)
		{
			this.StopVacuuming();
		}
		this._blowSystem.Play();
		this._suckSound.clip = this.blowStartSound;
		this._suckSound.loop = false;
		this._suckSound.Play();
		this._isBlowing = true;
		this.StopCorkPull();
		this.SetStuckObject(null);
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000F8802 File Offset: 0x000F6A02
	private void StopBlowing()
	{
		this._blowSystem.Stop();
		this._suckSound.clip = this.blowStopSound;
		this._suckSound.loop = false;
		this._suckSound.Play();
		this._isBlowing = false;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000F8840 File Offset: 0x000F6A40
	private void SetStuckObject(Rigidbody rigidbody)
	{
		if (this._stuckObject.rigidbody != null)
		{
			this._stuckObject.UndoPropertyModifications();
			this._stuckObject.rigidbody.velocity = MonoSingleton<NewMovement>.Instance.rb.velocity;
			this._stuckObject.rigidbody.angularVelocity = Vector3.zero;
			GoreSplatter goreSplatter;
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (this._stuckObject.rigidbody.TryGetComponent<GoreSplatter>(out goreSplatter))
			{
				goreSplatter.bloodAbsorberCount--;
			}
			else if (this._stuckObject.rigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
			{
				enemyIdentifierIdentifier.bloodAbsorberCount--;
			}
		}
		if (rigidbody == null)
		{
			this._stuckObject = default(Vacuum.StuckObject);
			return;
		}
		GoreSplatter goreSplatter2;
		EnemyIdentifierIdentifier enemyIdentifierIdentifier2;
		if (rigidbody.TryGetComponent<GoreSplatter>(out goreSplatter2))
		{
			goreSplatter2.bloodAbsorberCount++;
		}
		else if (rigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier2))
		{
			enemyIdentifierIdentifier2.bloodAbsorberCount++;
		}
		if (!this.musicStarted)
		{
			if (this.music)
			{
				this.music.SetActive(true);
			}
			this.musicStarted = true;
		}
		this._suckSystem.Stop();
		this._suckSound.loop = false;
		this._suckSound.clip = this.suckStuckSound;
		this._suckSound.Play();
		this._stuckObject = new Vacuum.StuckObject(rigidbody);
		this.StopCorkPull();
	}

	// Token: 0x04002A25 RID: 10789
	[SerializeField]
	private float _suckStrength = 10f;

	// Token: 0x04002A26 RID: 10790
	[SerializeField]
	private float _stuckDistance = 0.5f;

	// Token: 0x04002A27 RID: 10791
	[SerializeField]
	private Transform _suckPoint;

	// Token: 0x04002A28 RID: 10792
	[SerializeField]
	private BoxCollider _suckBox;

	// Token: 0x04002A29 RID: 10793
	[SerializeField]
	private AudioSource _consumeSound;

	// Token: 0x04002A2A RID: 10794
	[SerializeField]
	private AudioSource _suckSound;

	// Token: 0x04002A2B RID: 10795
	[SerializeField]
	private ParticleSystem _suckSystem;

	// Token: 0x04002A2C RID: 10796
	[SerializeField]
	private ParticleSystem _blowSystem;

	// Token: 0x04002A2D RID: 10797
	private ArraySegment<Collider> _colliders = new ArraySegment<Collider>(new Collider[256], 0, 0);

	// Token: 0x04002A2E RID: 10798
	private bool _isSucking;

	// Token: 0x04002A2F RID: 10799
	private bool _isBlowing;

	// Token: 0x04002A30 RID: 10800
	private Vacuum.StuckObject _stuckObject;

	// Token: 0x04002A31 RID: 10801
	private Vector3 _lastCameraRotation;

	// Token: 0x04002A32 RID: 10802
	private bool musicStarted;

	// Token: 0x04002A33 RID: 10803
	[SerializeField]
	private GameObject music;

	// Token: 0x04002A34 RID: 10804
	[Header("Sound Effects")]
	[SerializeField]
	private AudioClip suckStartSound;

	// Token: 0x04002A35 RID: 10805
	[SerializeField]
	private AudioClip suckLoopSound;

	// Token: 0x04002A36 RID: 10806
	[SerializeField]
	private AudioClip suckStopSound;

	// Token: 0x04002A37 RID: 10807
	[SerializeField]
	private AudioClip suckStuckSound;

	// Token: 0x04002A38 RID: 10808
	[SerializeField]
	private AudioClip suckStuckLoopSound;

	// Token: 0x04002A39 RID: 10809
	[SerializeField]
	private AudioClip blowStartSound;

	// Token: 0x04002A3A RID: 10810
	[SerializeField]
	private AudioClip blowLoopSound;

	// Token: 0x04002A3B RID: 10811
	[SerializeField]
	private AudioClip blowStopSound;

	// Token: 0x0200051A RID: 1306
	private readonly struct StuckObject
	{
		// Token: 0x06001DD0 RID: 7632 RVA: 0x000F89D2 File Offset: 0x000F6BD2
		public void UndoPropertyModifications()
		{
			if (this.rigidbody == null)
			{
				return;
			}
			this.rigidbody.maxAngularVelocity = this._maxAngularVelocity;
			this.rigidbody.interpolation = this._interpolation;
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000F8A05 File Offset: 0x000F6C05
		public StuckObject(Rigidbody instance)
		{
			this.rigidbody = instance;
			this._maxAngularVelocity = instance.maxAngularVelocity;
			this._interpolation = instance.interpolation;
			instance.maxAngularVelocity = float.PositiveInfinity;
			instance.interpolation = RigidbodyInterpolation.Interpolate;
		}

		// Token: 0x04002A3C RID: 10812
		public readonly Rigidbody rigidbody;

		// Token: 0x04002A3D RID: 10813
		private readonly float _maxAngularVelocity;

		// Token: 0x04002A3E RID: 10814
		private readonly RigidbodyInterpolation _interpolation;
	}
}
