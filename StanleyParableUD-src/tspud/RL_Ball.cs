using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000175 RID: 373
[ExecuteInEditMode]
public class RL_Ball : MonoBehaviour
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00028E45 File Offset: 0x00027045
	public Rigidbody rb
	{
		get
		{
			if (!(this._rb != null))
			{
				return base.GetComponent<Rigidbody>();
			}
			return this._rb;
		}
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00028E64 File Offset: 0x00027064
	private void OnCollisionEnter(Collision collision)
	{
		Object componentInParent = collision.gameObject.GetComponentInParent<StanleyController>();
		RL_Ball componentInParent2 = collision.gameObject.GetComponentInParent<RL_Ball>();
		if (!(componentInParent != null))
		{
			if (componentInParent2 != null)
			{
				this.OnBallCollisionEnter(collision.GetContact(0).normal.normalized);
				return;
			}
			this.surfaceObjectsTouched.Add(collision.collider);
			if (this.rollingSurfaceColliderCount == 0)
			{
				this.OnSurfaceCollisionEnter(collision.GetContact(0).normal.normalized);
			}
		}
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00028EED File Offset: 0x000270ED
	private void OnCollisionExit(Collision collision)
	{
		this.surfaceObjectsTouched.Remove(collision.collider);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00028F04 File Offset: 0x00027104
	private void OnBallCollisionEnter(Vector3 surfaceNormal)
	{
		float num = Vector3.Dot(surfaceNormal, this.rb.velocity.normalized);
		float num2 = Mathf.InverseLerp(0f, this.ballCollisionSpeedForMaxVolume, this.rb.velocity.magnitude);
		this.PlaySoftBounce(num * num2);
		this.bounceCount++;
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00028F68 File Offset: 0x00027168
	private void OnSurfaceCollisionEnter(Vector3 surfaceNormal)
	{
		float num = Vector3.Dot(surfaceNormal, this.rb.velocity.normalized);
		float num2 = Mathf.InverseLerp(0f, this.surfaceCollisionSpeedForMaxVolume, this.rb.velocity.magnitude);
		if (this.bounceCount == 0)
		{
			this.PlayHardBounce(num * num2);
		}
		else
		{
			this.PlaySoftBounce(num * num2);
		}
		this.bounceCount++;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00028FDD File Offset: 0x000271DD
	private void PlayHardBounce(float volumeMultiplier)
	{
		this.bounceSound.Play();
		this.bounceSound.GetComponent<AudioSource>().volume *= volumeMultiplier;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00029002 File Offset: 0x00027202
	private void PlaySoftBounce(float volumeMultiplier)
	{
		this.bounceOnBallSound.Play();
		this.bounceOnBallSound.GetComponent<AudioSource>().volume *= volumeMultiplier;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00029027 File Offset: 0x00027227
	private void PlaySoftKick(float volumeMultiplier)
	{
		this.kickSound.Play();
		this.kickSound.GetComponent<AudioSource>().volume *= volumeMultiplier;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0002904C File Offset: 0x0002724C
	private void PlayHardKick(float volumeMultiplier)
	{
		this.kickPulseSound.Play();
		this.kickPulseSound.GetComponent<AudioSource>().volume *= volumeMultiplier;
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00029074 File Offset: 0x00027274
	private void UpdateRollingColliderCount()
	{
		int num = 3;
		if (this.surfaceCollidersCount.Count < num)
		{
			this.surfaceCollidersCount.AddLast(this.surfaceObjectsTouched.Count);
		}
		if (this.surfaceCollidersCount.Count >= num)
		{
			this.surfaceCollidersCount.RemoveFirst();
		}
		this.rollingSurfaceColliderCount = 0;
		foreach (int num2 in this.surfaceCollidersCount)
		{
			this.rollingSurfaceColliderCount = Mathf.Max(this.rollingSurfaceColliderCount, num2);
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0002911C File Offset: 0x0002731C
	private bool IsRolling
	{
		get
		{
			return this.rollingSurfaceColliderCount > 0;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00029127 File Offset: 0x00027327
	private float Radius
	{
		get
		{
			return base.GetComponentInChildren<SphereCollider>().radius * base.transform.lossyScale.x;
		}
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00029148 File Offset: 0x00027348
	private void Update()
	{
		if (Application.isPlaying)
		{
			this.meshRenderer.material.SetFloat(this.flashShaderKey, this.flashAmount);
		}
		else
		{
			this.meshRenderer.sharedMaterial.SetFloat(this.flashShaderKey, this.flashAmount);
		}
		if (Application.isPlaying)
		{
			if (base.transform.position.y < this.depthToDisableRenderer)
			{
				base.gameObject.SetActive(false);
			}
			this.UpdateRollingColliderCount();
			this.ballVelocity = this.rb.velocity.magnitude;
			bool flag = this.IsRolling;
			if (this.IsRolling)
			{
				this.footstepTypeUnderBall = this.FindFootstepFromMaterialUnderBall();
				if (this.footstepTypeUnderBall == StanleyData.FootstepSounds.Grass)
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.rollingSpeedNormalized = this.rollVolumeBySpeed.Evaluate(this.rb.velocity.magnitude);
				this.rollingLoop.volume = Mathf.MoveTowards(this.rollingLoop.volume, this.rollingSpeedNormalized, 4f);
			}
			else
			{
				this.footstepTypeUnderBall = StanleyData.FootstepSounds.None;
				this.rollingLoop.volume = 0f;
			}
			this.DEBUG_DIST = this.DistanceToStanleyCenter;
			bool flag2 = false;
			if (Singleton<GameMaster>.Instance.stanleyActions.UseAction.LastValue > 0f)
			{
				this.kickTime += Time.deltaTime;
			}
			else
			{
				this.kickTime = 0f;
			}
			if (Singleton<GameMaster>.Instance.stanleyActions.UseAction.Value == 0f && Singleton<GameMaster>.Instance.stanleyActions.UseAction.LastValue > 0f)
			{
				flag2 = true;
			}
			this.coolDownTimeRemaining -= Time.deltaTime;
			if (this.coolDownTimeRemaining <= 0f)
			{
				if (this.IsCollisionWithStanley(0.25f))
				{
					this.coolDownTimeRemaining = this.coolDownPeriod;
					this.bounceCount = 0;
					this.AddKickForceToBall(Mathf.Lerp(this.touchForce, this.touchForceAtFullSpeed, RocketLeaugeChartacterSpeedControl.Instance.NormalizedSpeed));
					this.PlaySoftKick(RocketLeaugeChartacterSpeedControl.Instance.NormalizedSpeed);
				}
				Vector3 vector;
				if (this.IsCollisionWithStanley(this.clickDistance) && flag2 && this.IsStanleyLookingAtMe(out vector))
				{
					this.coolDownTimeRemaining = this.coolDownPeriod;
					float num = Mathf.InverseLerp(0f, this.longClickTime, this.kickTime);
					float num2 = Mathf.Lerp(this.clickForce, this.longClickForce, num);
					this.bounceCount = 0;
					this.AddKickForceToBall(num2, vector, 1f);
					this.PlayHardKick(Mathf.Lerp(0.5f, 1f, num));
				}
			}
		}
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x000293E4 File Offset: 0x000275E4
	private bool IsStanleyLookingAtMe(out Vector3 lookDirection)
	{
		lookDirection = StanleyController.Instance.camTransform.forward;
		foreach (RaycastHit raycastHit in Physics.RaycastAll(StanleyController.Instance.camTransform.position, StanleyController.Instance.camTransform.forward, this.clickDistance * 2f))
		{
			RL_Ball componentInParent = raycastHit.collider.GetComponentInParent<RL_Ball>();
			if (componentInParent != null && componentInParent == this)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0002946E File Offset: 0x0002766E
	private void AddKickForceToBall(float force)
	{
		this.AddKickForceToBall(force, Vector3.up, 0f);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00029484 File Offset: 0x00027684
	private void AddKickForceToBall(float force, Vector3 lookDirection, float lookBlend)
	{
		Vector3 vector = Vector3.Slerp((base.transform.position - StanleyController.StanleyPosition).normalized, lookDirection, lookBlend);
		base.GetComponent<Rigidbody>().AddForce(vector * force);
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x000294C8 File Offset: 0x000276C8
	private float DistanceToStanleyCenter
	{
		get
		{
			return Vector3.Distance(StanleyController.StanleyPosition, base.transform.position);
		}
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000294E0 File Offset: 0x000276E0
	private bool IsCollisionWithStanley(float distanceToStanley)
	{
		float num = this.Radius + distanceToStanley;
		if (this.DistanceToStanleyCenter < num + StanleyController.Instance.GetComponent<CharacterController>().height)
		{
			Collider[] array = Physics.OverlapSphere(base.transform.position, num, this.stanleyMask.value);
			if (array.Length != 0)
			{
				if (Array.FindIndex<Collider>(array, (Collider c) => c.gameObject == StanleyController.Instance.gameObject) != -1)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0002955C File Offset: 0x0002775C
	private StanleyData.FootstepSounds FindFootstepFromMaterialUnderBall()
	{
		foreach (RaycastHit raycastHit in Physics.RaycastAll(new Ray(base.transform.position, Vector3.down), this.Radius + 0.25f))
		{
			if (!(raycastHit.collider.GetComponentInChildren<RL_Ball>() != null))
			{
				MeshCollider meshCollider = raycastHit.collider as MeshCollider;
				if (meshCollider != null && meshCollider.sharedMesh != null)
				{
					Mesh sharedMesh = meshCollider.sharedMesh;
					Renderer component = raycastHit.collider.GetComponent<Renderer>();
					int triangleIndex = raycastHit.triangleIndex;
					if (sharedMesh.triangles.Length >= triangleIndex * 3 + 3 && triangleIndex >= 0)
					{
						int num = sharedMesh.triangles[triangleIndex * 3];
						int num2 = sharedMesh.triangles[triangleIndex * 3 + 1];
						int num3 = sharedMesh.triangles[triangleIndex * 3 + 2];
						int num4 = -1;
						for (int j = 0; j < sharedMesh.subMeshCount; j++)
						{
							int[] triangles = sharedMesh.GetTriangles(j);
							for (int k = 0; k < triangles.Length; k += 3)
							{
								if (triangles[k] == num && triangles[k + 1] == num2 && triangles[k + 2] == num3)
								{
									num4 = j;
									break;
								}
							}
							if (num4 != -1)
							{
								break;
							}
						}
						if (num4 != -1)
						{
							if (component.materials[num4].HasProperty("_FootstepType"))
							{
								return (StanleyData.FootstepSounds)component.materials[num4].GetInt("_FootstepType");
							}
							return StanleyData.FootstepSounds.Missing;
						}
					}
				}
				else
				{
					MeshRenderer component2 = raycastHit.collider.GetComponent<MeshRenderer>();
					if (component2 != null && component2.sharedMaterials.Length != 0 && component2.sharedMaterial.HasProperty("_FootstepType"))
					{
						return (StanleyData.FootstepSounds)component2.sharedMaterial.GetFloat("_FootstepType");
					}
				}
			}
		}
		return StanleyData.FootstepSounds.None;
	}

	// Token: 0x04000877 RID: 2167
	[Header("Visuals")]
	[Range(0f, 1f)]
	public float flashAmount;

	// Token: 0x04000878 RID: 2168
	public MeshRenderer meshRenderer;

	// Token: 0x04000879 RID: 2169
	public string flashShaderKey;

	// Token: 0x0400087A RID: 2170
	[Header("Kick Mechanics")]
	public LayerMask stanleyMask;

	// Token: 0x0400087B RID: 2171
	public float coolDownPeriod = 1f;

	// Token: 0x0400087C RID: 2172
	private float coolDownTimeRemaining;

	// Token: 0x0400087D RID: 2173
	public float touchForce = 350f;

	// Token: 0x0400087E RID: 2174
	public float touchForceAtFullSpeed = 350f;

	// Token: 0x0400087F RID: 2175
	public float clickForce = 1000f;

	// Token: 0x04000880 RID: 2176
	public float longClickForce = 2000f;

	// Token: 0x04000881 RID: 2177
	public float longClickTime = 0.25f;

	// Token: 0x04000882 RID: 2178
	public float clickDistance = 4f;

	// Token: 0x04000883 RID: 2179
	[Header("Audio")]
	public PlaySoundFromAudioCollection kickSound;

	// Token: 0x04000884 RID: 2180
	public PlaySoundFromAudioCollection kickPulseSound;

	// Token: 0x04000885 RID: 2181
	public PlaySoundFromAudioCollection bounceSound;

	// Token: 0x04000886 RID: 2182
	public PlaySoundFromAudioCollection bounceOnBallSound;

	// Token: 0x04000887 RID: 2183
	public AudioSource rollingLoop;

	// Token: 0x04000888 RID: 2184
	public float ballCollisionSpeedForMaxVolume = 8f;

	// Token: 0x04000889 RID: 2185
	public float surfaceCollisionSpeedForMaxVolume = 8f;

	// Token: 0x0400088A RID: 2186
	public AnimationCurve rollVolumeBySpeed;

	// Token: 0x0400088B RID: 2187
	[Header("Other")]
	public float depthToDisableRenderer = -32f;

	// Token: 0x0400088C RID: 2188
	[Header("DEBUG")]
	public float kickTime;

	// Token: 0x0400088D RID: 2189
	public float DEBUG_DIST;

	// Token: 0x0400088E RID: 2190
	public List<Collider> surfaceObjectsTouched = new List<Collider>();

	// Token: 0x0400088F RID: 2191
	public LinkedList<int> surfaceCollidersCount = new LinkedList<int>();

	// Token: 0x04000890 RID: 2192
	public int rollingSurfaceColliderCount = -1;

	// Token: 0x04000891 RID: 2193
	public float rollingSpeedNormalized;

	// Token: 0x04000892 RID: 2194
	public int bounceCount;

	// Token: 0x04000893 RID: 2195
	public float ballVelocity;

	// Token: 0x04000894 RID: 2196
	public StanleyData.FootstepSounds footstepTypeUnderBall;

	// Token: 0x04000895 RID: 2197
	private Rigidbody _rb;
}
