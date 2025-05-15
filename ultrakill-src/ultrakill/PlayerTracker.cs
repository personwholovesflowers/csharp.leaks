using System;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class PlayerTracker : MonoSingleton<PlayerTracker>
{
	// Token: 0x0600136E RID: 4974 RVA: 0x0009BFB1 File Offset: 0x0009A1B1
	private void Start()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x0009BFC1 File Offset: 0x0009A1C1
	public Transform GetPlayer()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		return this.player;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0009BFD7 File Offset: 0x0009A1D7
	public Transform GetTarget()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		return this.target;
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0009BFED File Offset: 0x0009A1ED
	public Rigidbody GetRigidbody()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		return this.playerRb;
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x0009C004 File Offset: 0x0009A204
	public Vector3 PredictPlayerPosition(float time, bool aimAtHead = false, bool ignoreCollision = false)
	{
		Vector3 vector = this.GetPlayerVelocity(false) * time;
		RaycastHit raycastHit;
		if (!ignoreCollision && Physics.Raycast(this.playerRb.position, vector, out raycastHit, vector.magnitude, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			return raycastHit.point;
		}
		if (aimAtHead)
		{
			return this.target.position + vector;
		}
		return this.playerRb.position + vector;
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x0009C078 File Offset: 0x0009A278
	public Vector3 GetPlayerVelocity(bool trueVelocity = false)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		Vector3 vector = this.playerRb.velocity;
		if (!trueVelocity && MonoSingleton<NewMovement>.Instance.boost && !MonoSingleton<NewMovement>.Instance.sliding)
		{
			vector /= 3f;
		}
		if (MonoSingleton<NewMovement>.Instance.ridingRocket)
		{
			vector += MonoSingleton<NewMovement>.Instance.ridingRocket.rb.velocity;
		}
		if (MonoSingleton<PlayerMovementParenting>.Instance != null)
		{
			Vector3 vector2 = MonoSingleton<PlayerMovementParenting>.Instance.currentDelta;
			vector2 *= 60f;
			vector += vector2;
		}
		return vector;
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x0009C120 File Offset: 0x0009A320
	public bool GetOnGround()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		return (this.playerType == PlayerType.FPS && MonoSingleton<NewMovement>.Instance.gc.onGround) || (this.playerType == PlayerType.Platformer && MonoSingleton<PlatformerMovement>.Instance.groundCheck.onGround);
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x0009C170 File Offset: 0x0009A370
	public void ChangeToPlatformer()
	{
		this.ChangeToPlatformer(false);
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0009C17C File Offset: 0x0009A37C
	public void ChangeToPlatformer(bool ignorePreviousRotation = false)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		if (!this.pmov || !this.nmov || !this.currentPlatformerPlayerPrefab)
		{
			return;
		}
		if (this.cameraType == PlatformerCameraType.PlayerControlled)
		{
			this.pmov.freeCamera = true;
		}
		else
		{
			this.pmov.freeCamera = false;
		}
		if (!this.levelStarted)
		{
			this.startAsPlatformer = true;
			return;
		}
		if (this.playerType == PlayerType.Platformer)
		{
			return;
		}
		if (this.cameraType == PlatformerCameraType.PlayerControlled && !ignorePreviousRotation)
		{
			this.pmov.ResetCamera(MonoSingleton<CameraController>.Instance.rotationY, MonoSingleton<CameraController>.Instance.rotationX + 20f);
		}
		this.playerType = PlayerType.Platformer;
		this.ChangeTargetParent(this.player, this.pmov.transform, Vector3.up * 2.5f);
		this.ChangeTargetParent(this.target, this.pmov.transform, Vector3.up * 2.5f);
		this.nmov.gameObject.SetActive(false);
		this.currentPlatformerPlayerPrefab.transform.position = this.nmov.transform.position - Vector3.up * 1.5f;
		this.pmov.transform.position = this.currentPlatformerPlayerPrefab.transform.position;
		this.pmov.platformerCamera.transform.localPosition = Vector3.up * 2.5f;
		this.pmov.playerModel.transform.rotation = this.nmov.transform.rotation;
		this.currentPlatformerPlayerPrefab.SetActive(true);
		this.pmov.gameObject.SetActive(true);
		if (this.pmov.rb)
		{
			this.playerRb = this.pmov.rb;
		}
		else
		{
			this.playerRb = this.pmov.GetComponent<Rigidbody>();
		}
		this.pmov.CheckItem();
		this.playerRb.velocity = this.nmov.rb.velocity;
		PostProcessV2_Handler instance = MonoSingleton<PostProcessV2_Handler>.Instance;
		if (instance != null)
		{
			instance.ChangeCamera(true);
		}
		foreach (GameObject gameObject in this.platformerFailSafes)
		{
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x0009C3E0 File Offset: 0x0009A5E0
	public void ChangeToFPS()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		if (!this.pmov || !this.nmov || !this.currentPlatformerPlayerPrefab)
		{
			return;
		}
		if (!this.levelStarted)
		{
			this.startAsPlatformer = false;
			return;
		}
		if (this.playerType == PlayerType.FPS)
		{
			return;
		}
		this.playerType = PlayerType.FPS;
		this.nmov.transform.position = this.pmov.transform.position + Vector3.up * 1.5f;
		this.currentPlatformerPlayerPrefab.SetActive(false);
		this.playerRb = this.nmov.rb;
		this.nmov.gameObject.SetActive(true);
		this.ChangeTargetParent(this.player, this.nmov.transform, default(Vector3));
		this.ChangeTargetParent(this.target, this.cc.transform, default(Vector3));
		this.pmov.gameObject.SetActive(false);
		PostProcessV2_Handler instance = MonoSingleton<PostProcessV2_Handler>.Instance;
		if (instance != null)
		{
			instance.ChangeCamera(false);
		}
		if (this.pmov.freeCamera)
		{
			MonoSingleton<CameraController>.Instance.ResetCamera(this.pmov.rotationY, this.pmov.rotationX - 20f);
		}
		else
		{
			MonoSingleton<CameraController>.Instance.ResetCamera(this.pmov.playerModel.transform.rotation.eulerAngles.y, 0f);
		}
		if (this.pmov.rb)
		{
			this.nmov.rb.velocity = this.pmov.rb.velocity;
		}
		foreach (GameObject gameObject in this.platformerFailSafes)
		{
			if (gameObject)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x0009C5CC File Offset: 0x0009A7CC
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.cc = MonoSingleton<CameraController>.Instance;
		if (!this.nmov || !this.cc)
		{
			return;
		}
		Camera camera = null;
		if (this.cc && this.cc.cam)
		{
			camera = this.cc.cam;
		}
		else if (this.cc)
		{
			camera = this.cc.GetComponent<Camera>();
		}
		if (this.playerType == PlayerType.Platformer && !this.levelStarted)
		{
			this.startAsPlatformer = true;
			this.playerType = PlayerType.FPS;
		}
		GameObject gameObject = new GameObject();
		this.player = gameObject.transform;
		this.ChangeTargetParent(this.player, this.nmov.transform, default(Vector3));
		if (this.nmov.rb)
		{
			this.playerRb = this.nmov.rb;
		}
		else
		{
			this.playerRb = this.nmov.GetComponent<Rigidbody>();
		}
		GameObject gameObject2 = new GameObject();
		this.target = gameObject2.transform;
		this.ChangeTargetParent(this.target, this.cc.transform, default(Vector3));
		if (!this.pmov)
		{
			if (this.player == null || this.platformerPlayerPrefab == null)
			{
				return;
			}
			this.currentPlatformerPlayerPrefab = Object.Instantiate<GameObject>(this.platformerPlayerPrefab, this.player.position, Quaternion.identity);
			this.pmov = this.currentPlatformerPlayerPrefab.GetComponentInChildren<PlatformerMovement>(true);
			if (camera)
			{
				this.currentPlatformerPlayerPrefab.GetComponentInChildren<Camera>(true).clearFlags = camera.clearFlags;
			}
		}
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x0009C799 File Offset: 0x0009A999
	private void ChangeTargetParent(Transform toMove, Transform newParent, Vector3 offset = default(Vector3))
	{
		toMove.position = newParent.position + offset;
		toMove.SetParent(newParent);
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x0009C7B4 File Offset: 0x0009A9B4
	public void CheckPlayerType()
	{
		if (this.playerType == PlayerType.FPS && (!MonoSingleton<NewMovement>.Instance || !MonoSingleton<NewMovement>.Instance.gameObject.activeInHierarchy))
		{
			this.ChangeToFPS();
			return;
		}
		if (this.playerType == PlayerType.Platformer && (!this.currentPlatformerPlayerPrefab || !this.currentPlatformerPlayerPrefab.gameObject.activeInHierarchy))
		{
			this.ChangeToPlatformer();
		}
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x0009C81C File Offset: 0x0009AA1C
	public void LevelStart()
	{
		if (this.levelStarted)
		{
			return;
		}
		this.levelStarted = true;
		if (MonoSingleton<OnLevelStart>.Instance != null)
		{
			MonoSingleton<OnLevelStart>.Instance.StartLevel(true, true);
		}
		if (this.startAsPlatformer)
		{
			this.ChangeToPlatformer(this.pmov.freeCamera);
			return;
		}
		if (this.playerType == PlayerType.FPS)
		{
			foreach (GameObject gameObject in this.platformerFailSafes)
			{
				if (gameObject)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x04001AC9 RID: 6857
	public PlayerType playerType;

	// Token: 0x04001ACA RID: 6858
	private bool initialized;

	// Token: 0x04001ACB RID: 6859
	private NewMovement nmov;

	// Token: 0x04001ACC RID: 6860
	private CameraController cc;

	// Token: 0x04001ACD RID: 6861
	public GameObject platformerPlayerPrefab;

	// Token: 0x04001ACE RID: 6862
	[HideInInspector]
	public GameObject currentPlatformerPlayerPrefab;

	// Token: 0x04001ACF RID: 6863
	[HideInInspector]
	public PlatformerMovement pmov;

	// Token: 0x04001AD0 RID: 6864
	private Transform player;

	// Token: 0x04001AD1 RID: 6865
	private Transform target;

	// Token: 0x04001AD2 RID: 6866
	private Rigidbody playerRb;

	// Token: 0x04001AD3 RID: 6867
	[HideInInspector]
	public bool levelStarted;

	// Token: 0x04001AD4 RID: 6868
	private bool startAsPlatformer;

	// Token: 0x04001AD5 RID: 6869
	public PlatformerCameraType cameraType;

	// Token: 0x04001AD6 RID: 6870
	public GameObject[] platformerFailSafes;
}
