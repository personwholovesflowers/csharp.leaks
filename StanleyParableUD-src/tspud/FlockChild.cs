using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class FlockChild : MonoBehaviour
{
	// Token: 0x0600008E RID: 142 RVA: 0x00005CE0 File Offset: 0x00003EE0
	public void Start()
	{
		this.FindRequiredComponents();
		this.Wander(0f);
		this.SetRandomScale();
		this._thisT.position = this.findWaypoint();
		this.RandomizeStartAnimationFrame();
		this.InitAvoidanceValues();
		this._speed = this._spawner._minSpeed;
		this._spawner._activeChildren += 1f;
		this._instantiated = true;
		if (this._spawner._updateDivisor > 1)
		{
			int num = this._spawner._updateDivisor - 1;
			FlockChild._updateNextSeed++;
			this._updateSeed = FlockChild._updateNextSeed;
			FlockChild._updateNextSeed %= num;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00005D8F File Offset: 0x00003F8F
	public void Update()
	{
		if (this._spawner._updateDivisor <= 1 || this._spawner._updateCounter == this._updateSeed)
		{
			this.SoarTimeLimit();
			this.CheckForDistanceToWaypoint();
			this.RotationBasedOnWaypointOrAvoidance();
			this.LimitRotationOfModel();
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00005DCA File Offset: 0x00003FCA
	public void OnDisable()
	{
		base.CancelInvoke();
		this._spawner._activeChildren -= 1f;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00005DEC File Offset: 0x00003FEC
	public void OnEnable()
	{
		if (this._instantiated)
		{
			this._spawner._activeChildren += 1f;
			if (this._landing)
			{
				this._model.GetComponent<Animation>().Play(this._spawner._idleAnimation);
				return;
			}
			this._model.GetComponent<Animation>().Play(this._spawner._flapAnimation);
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00005E5C File Offset: 0x0000405C
	public void FindRequiredComponents()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._model == null)
		{
			this._model = this._thisT.Find("Model").gameObject;
		}
		if (this._modelT == null)
		{
			this._modelT = this._model.transform;
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00005ECC File Offset: 0x000040CC
	public void RandomizeStartAnimationFrame()
	{
		foreach (object obj in this._model.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			animationState.time = Random.value * animationState.length;
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00005F38 File Offset: 0x00004138
	public void InitAvoidanceValues()
	{
		this._avoidValue = Random.Range(0.3f, 0.1f);
		if (this._spawner._birdAvoidDistanceMax != this._spawner._birdAvoidDistanceMin)
		{
			this._avoidDistance = Random.Range(this._spawner._birdAvoidDistanceMax, this._spawner._birdAvoidDistanceMin);
			return;
		}
		this._avoidDistance = this._spawner._birdAvoidDistanceMin;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00005FA8 File Offset: 0x000041A8
	public void SetRandomScale()
	{
		float num = Random.Range(this._spawner._minScale, this._spawner._maxScale);
		this._thisT.localScale = new Vector3(num, num, num);
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00005FE4 File Offset: 0x000041E4
	public void SoarTimeLimit()
	{
		if (this._soar && this._spawner._soarMaxTime > 0f)
		{
			if (this._soarTimer > this._spawner._soarMaxTime)
			{
				this.Flap();
				this._soarTimer = 0f;
				return;
			}
			this._soarTimer += this._spawner._newDelta;
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00006048 File Offset: 0x00004248
	public void CheckForDistanceToWaypoint()
	{
		if (!this._landing && (this._thisT.position - this._wayPoint).magnitude < this._spawner._waypointDistance + this._stuckCounter)
		{
			this.Wander(0f);
			this._stuckCounter = 0f;
			return;
		}
		if (!this._landing)
		{
			this._stuckCounter += this._spawner._newDelta;
			return;
		}
		this._stuckCounter = 0f;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x000060D4 File Offset: 0x000042D4
	public void RotationBasedOnWaypointOrAvoidance()
	{
		Vector3 vector = this._wayPoint - this._thisT.position;
		if (this._targetSpeed > -1f && vector != Vector3.zero)
		{
			Quaternion quaternion = Quaternion.LookRotation(vector);
			this._thisT.rotation = Quaternion.Slerp(this._thisT.rotation, quaternion, this._spawner._newDelta * this._damping);
		}
		if (this._spawner._childTriggerPos && (this._thisT.position - this._spawner._posBuffer).magnitude < 1f)
		{
			this._spawner.SetFlockRandomPosition();
		}
		this._speed = Mathf.Lerp(this._speed, this._targetSpeed, this._spawner._newDelta * 2.5f);
		if (this._move)
		{
			this._thisT.position += this._thisT.forward * this._speed * this._spawner._newDelta;
			if (this._avoid && this._spawner._birdAvoid)
			{
				this.Avoidance();
			}
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00006214 File Offset: 0x00004414
	public bool Avoidance()
	{
		RaycastHit raycastHit = default(RaycastHit);
		Vector3 forward = this._modelT.forward;
		bool flag = false;
		Quaternion quaternion = Quaternion.identity;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		vector2 = this._thisT.position;
		quaternion = this._thisT.rotation;
		vector = this._thisT.rotation.eulerAngles;
		if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * this._avoidValue, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			vector.y -= (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			quaternion.eulerAngles = vector;
			this._thisT.rotation = quaternion;
			flag = true;
		}
		else if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * -this._avoidValue, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			vector.y += (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			quaternion.eulerAngles = vector;
			this._thisT.rotation = quaternion;
			flag = true;
		}
		if (this._spawner._birdAvoidDown && !this._landing && Physics.Raycast(this._thisT.position, -Vector3.up, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			vector.x -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			quaternion.eulerAngles = vector;
			this._thisT.rotation = quaternion;
			vector2.y += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = vector2;
			flag = true;
		}
		else if (this._spawner._birdAvoidUp && !this._landing && Physics.Raycast(this._thisT.position, Vector3.up, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			vector.x += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			quaternion.eulerAngles = vector;
			this._thisT.rotation = quaternion;
			vector2.y -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = vector2;
			flag = true;
		}
		return flag;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0000651C File Offset: 0x0000471C
	public void LimitRotationOfModel()
	{
		Quaternion quaternion = Quaternion.identity;
		Vector3 vector = Vector3.zero;
		quaternion = this._modelT.localRotation;
		vector = quaternion.eulerAngles;
		if ((((this._soar && this._spawner._flatSoar) || (this._spawner._flatFly && !this._soar)) && this._wayPoint.y > this._thisT.position.y) || this._landing)
		{
			vector.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, -this._thisT.localEulerAngles.x, this._spawner._newDelta * 1.75f);
			quaternion.eulerAngles = vector;
			this._modelT.localRotation = quaternion;
			return;
		}
		vector.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, 0f, this._spawner._newDelta * 1.75f);
		quaternion.eulerAngles = vector;
		this._modelT.localRotation = quaternion;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00006634 File Offset: 0x00004834
	public void Wander(float delay)
	{
		if (!this._landing)
		{
			this._damping = Random.Range(this._spawner._minDamping, this._spawner._maxDamping);
			this._targetSpeed = Random.Range(this._spawner._minSpeed, this._spawner._maxSpeed);
			base.Invoke("SetRandomMode", delay);
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00006698 File Offset: 0x00004898
	public void SetRandomMode()
	{
		base.CancelInvoke("SetRandomMode");
		if (!this._dived && Random.value < this._spawner._soarFrequency)
		{
			this.Soar();
			return;
		}
		if (!this._dived && Random.value < this._spawner._diveFrequency)
		{
			this.Dive();
			return;
		}
		this.Flap();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000066F8 File Offset: 0x000048F8
	public void Flap()
	{
		if (this._move)
		{
			if (this._model != null)
			{
				this._model.GetComponent<Animation>().CrossFade(this._spawner._flapAnimation, 0.5f);
			}
			this._soar = false;
			this.animationSpeed();
			this._wayPoint = this.findWaypoint();
			this._dived = false;
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000675C File Offset: 0x0000495C
	public Vector3 findWaypoint()
	{
		Vector3 zero = Vector3.zero;
		zero.x = Random.Range(-this._spawner._spawnSphere, this._spawner._spawnSphere) + this._spawner._posBuffer.x;
		zero.z = Random.Range(-this._spawner._spawnSphereDepth, this._spawner._spawnSphereDepth) + this._spawner._posBuffer.z;
		zero.y = Random.Range(-this._spawner._spawnSphereHeight, this._spawner._spawnSphereHeight) + this._spawner._posBuffer.y;
		return zero;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000680C File Offset: 0x00004A0C
	public void Soar()
	{
		if (this._move)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
			this._wayPoint = this.findWaypoint();
			this._soar = true;
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000684C File Offset: 0x00004A4C
	public void Dive()
	{
		if (this._spawner._soarAnimation != null)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
		}
		else
		{
			foreach (object obj in this._model.GetComponent<Animation>())
			{
				AnimationState animationState = (AnimationState)obj;
				if (this._thisT.position.y < this._wayPoint.y + 25f)
				{
					animationState.speed = 0.1f;
				}
			}
		}
		this._wayPoint = this.findWaypoint();
		this._wayPoint.y = this._wayPoint.y - this._spawner._diveValue;
		this._dived = true;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00006930 File Offset: 0x00004B30
	public void animationSpeed()
	{
		foreach (object obj in this._model.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			if (!this._dived && !this._landing)
			{
				animationState.speed = Random.Range(this._spawner._minAnimationSpeed, this._spawner._maxAnimationSpeed);
			}
			else
			{
				animationState.speed = this._spawner._maxAnimationSpeed;
			}
		}
	}

	// Token: 0x040000C1 RID: 193
	[HideInInspector]
	public FlockController _spawner;

	// Token: 0x040000C2 RID: 194
	[HideInInspector]
	public Vector3 _wayPoint;

	// Token: 0x040000C3 RID: 195
	public float _speed;

	// Token: 0x040000C4 RID: 196
	[HideInInspector]
	public bool _dived = true;

	// Token: 0x040000C5 RID: 197
	[HideInInspector]
	public float _stuckCounter;

	// Token: 0x040000C6 RID: 198
	[HideInInspector]
	public float _damping;

	// Token: 0x040000C7 RID: 199
	[HideInInspector]
	public bool _soar = true;

	// Token: 0x040000C8 RID: 200
	[HideInInspector]
	public bool _landing;

	// Token: 0x040000C9 RID: 201
	[HideInInspector]
	public float _targetSpeed;

	// Token: 0x040000CA RID: 202
	[HideInInspector]
	public bool _move = true;

	// Token: 0x040000CB RID: 203
	public GameObject _model;

	// Token: 0x040000CC RID: 204
	public Transform _modelT;

	// Token: 0x040000CD RID: 205
	[HideInInspector]
	public float _avoidValue;

	// Token: 0x040000CE RID: 206
	[HideInInspector]
	public float _avoidDistance;

	// Token: 0x040000CF RID: 207
	private float _soarTimer;

	// Token: 0x040000D0 RID: 208
	private bool _instantiated;

	// Token: 0x040000D1 RID: 209
	private static int _updateNextSeed;

	// Token: 0x040000D2 RID: 210
	private int _updateSeed = -1;

	// Token: 0x040000D3 RID: 211
	[HideInInspector]
	public bool _avoid = true;

	// Token: 0x040000D4 RID: 212
	public Transform _thisT;

	// Token: 0x040000D5 RID: 213
	public Vector3 _landingPosOffset;
}
