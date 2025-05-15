using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000049 RID: 73
public class EasyPortal : MonoBehaviour, IComparable<EasyPortal>
{
	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000182 RID: 386 RVA: 0x0000A58C File Offset: 0x0000878C
	public Transform Destination
	{
		get
		{
			return this.destination;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000183 RID: 387 RVA: 0x0000A594 File Offset: 0x00008794
	public bool Disabled
	{
		get
		{
			return this.disabled;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000184 RID: 388 RVA: 0x0000A59C File Offset: 0x0000879C
	public bool DisableRefProbeCopyOnThisPortalsRefProbe
	{
		get
		{
			return this.disableRefProbeCopyOnThisPortalsRefProbe;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000185 RID: 389 RVA: 0x0000A5A4 File Offset: 0x000087A4
	public ReflectionProbe RoomReflectionProbe
	{
		get
		{
			if (this.roomReflectionProbe == null && !this.hasAttemptedRoomReflectionProbeAutoFind)
			{
				this.FindClosestRefProbe();
				this.hasAttemptedRoomReflectionProbeAutoFind = true;
			}
			if (this.roomReflectionProbe != null)
			{
				string text = " for " + base.name;
				if (!this.roomReflectionProbe.name.EndsWith(text))
				{
					ReflectionProbe reflectionProbe = this.roomReflectionProbe;
					reflectionProbe.name += text;
				}
			}
			return this.roomReflectionProbe;
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000A624 File Offset: 0x00008824
	private void FindClosestRefProbe()
	{
		Transform transform = ((this.roomReflectionProbeLocation != null) ? this.roomReflectionProbeLocation : base.transform);
		List<ReflectionProbe> list = new List<ReflectionProbe>();
		TSPArea componentInParent = base.GetComponentInParent<TSPArea>();
		if (componentInParent != null)
		{
			list.AddRange(componentInParent.GetComponentsInChildren<ReflectionProbe>());
		}
		else
		{
			list.AddRange(Object.FindObjectsOfType<ReflectionProbe>());
		}
		float num = float.PositiveInfinity;
		ReflectionProbe reflectionProbe = null;
		foreach (ReflectionProbe reflectionProbe2 in list)
		{
			float num2 = Vector3.Distance(transform.position, reflectionProbe2.transform.position);
			if (num2 < num)
			{
				num = num2;
				reflectionProbe = reflectionProbe2;
			}
		}
		if (reflectionProbe != null)
		{
			this.roomReflectionProbe = reflectionProbe;
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A6FC File Offset: 0x000088FC
	public void MoveToLinkedPosition(Transform transformToMove, Transform targetAtLinkedPortal)
	{
		if (this.helperTransform == null)
		{
			return;
		}
		Transform transform = this.helperTransform;
		Transform transform2 = base.transform;
		Vector3 vector = transform.InverseTransformPoint(targetAtLinkedPortal.position);
		Vector3 vector2 = transform2.TransformPoint(vector);
		Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
		Matrix4x4 matrix4x = transform2.localToWorldMatrix * transform.worldToLocalMatrix * localToWorldMatrix;
		transformToMove.transform.position = vector2;
		transformToMove.transform.rotation = matrix4x.rotation;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000A778 File Offset: 0x00008978
	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(Vector3.zero, Vector3.forward * this.renderingDistance);
		Transform transform = ((this.destination != null) ? this.destination : ((this.linkedPortal != null) ? this.linkedPortal.transform : null));
		if (transform == null)
		{
			return;
		}
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(Vector3.zero + Vector3.up * 0.05f, Vector3.forward * this.portalCameraFarClip + Vector3.up * 0.05f);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000A850 File Offset: 0x00008A50
	private void Awake()
	{
		this.screenCollider = this.screen.GetComponent<BoxCollider>();
		this.screenCollider.enabled = true;
		this.screen.material.SetFloat("_StaticLerp", 1f);
		if (this.portalStyle == EasyPortal.PortalStyles.LinkedPortal)
		{
			this.destination = this.linkedPortal.transform;
		}
		this.screen.gameObject.name = base.name + "_Screen";
		this.portalScreen = this.screen.gameObject.AddComponent<PortalScreen>();
		PortalScreen portalScreen = this.portalScreen;
		portalScreen.OnVisible = (Action)Delegate.Combine(portalScreen.OnVisible, new Action(this.OnVisible));
		PortalScreen portalScreen2 = this.portalScreen;
		portalScreen2.OnInvisible = (Action)Delegate.Combine(portalScreen2.OnInvisible, new Action(this.OnInvisible));
		this.trackedTravellers = new List<PortalTraveller>();
		this.screen.material.SetInt("displayMask", 1);
		if (this.portalCam != null)
		{
			this.portalCam.eventMask = 0;
			this.portalCam.enabled = false;
		}
		if (this.extraCam != null)
		{
			this.extraCam.eventMask = 0;
			this.extraCam.enabled = false;
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000A9A0 File Offset: 0x00008BA0
	private void Start()
	{
		this.UpdateHelperTransform();
		if (MainCamera.Camera != null)
		{
			this.SetupCamera();
		}
		if (!this.useCustomScreenCollider)
		{
			this.UpdateCollider();
		}
		this.gotDisableProbe = this.disableRefProbeWhenActive != null;
		this.allTravelers = Object.FindObjectsOfType<PortalTraveller>();
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000A9F4 File Offset: 0x00008BF4
	private void UpdateCollider()
	{
		this.screenCollider.center = new Vector3(0f, 0.5f, 0f);
		this.screenCollider.size = new Vector3(1f, 1f, this.colliderLength);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000AA40 File Offset: 0x00008C40
	private void SetupCamera()
	{
		this.playerCam = MainCamera.Camera;
		this.raycastMask = this.playerCam.cullingMask & -4194305;
		this.raycastTarget = MainCamera.RaycastTarget;
		this.portalCam = base.GetComponentInChildren<Camera>();
		this.portalCam.enabled = false;
		if (MainCamera.UseVicinityRenderingOnly)
		{
			this.renderingStyle = EasyPortal.RenderingStyles.RenderInVicinity;
		}
		this.SetSecondPortalLayer(this.renderSecondInvisiblePortalLayer);
		this.portalCam.gameObject.AddComponent<EasyPortalPostEffect>().postprocessMaterial = new Material(Shader.Find("Hidden/ClearAlphaChannel"));
		this.FindRecursivePortals(ref this.recursiveVisiblePortals, true);
		this.UpdateStaticTexture(true, true);
		if (this.extraCam != null)
		{
			this.CreateExtraTexture();
		}
		this.setup = true;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000AB08 File Offset: 0x00008D08
	private void UpdateHelperTransform()
	{
		if (this.helperTransform == null)
		{
			this.helperTransform = new GameObject("HelperTransform").transform;
			this.helperTransform.name = base.gameObject.name + "_HelperTransform";
		}
		this.helperTransform.parent = this.destination.transform;
		this.helperTransform.localPosition = Vector3.zero;
		this.helperTransform.rotation = this.destination.rotation;
		this.helperTransform.localScale = Vector3.one;
		this.helperTransform.Rotate(0f, 180f, 0f);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000ABC0 File Offset: 0x00008DC0
	public void SetSecondPortalLayer(bool status)
	{
		if (this.useCustomLayerMaskRendering)
		{
			this.portalCam.cullingMask = this.customMask;
			return;
		}
		if (status)
		{
			this.portalCam.cullingMask = this.playerCam.cullingMask & -1048577;
			return;
		}
		this.portalCam.cullingMask = this.playerCam.cullingMask & -1048577;
		this.portalCam.cullingMask = this.portalCam.cullingMask & -2097153;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000AC44 File Offset: 0x00008E44
	public bool PlayerFarFromPortal()
	{
		return this.playerCam == null || Vector3.Distance(base.transform.position, this.playerCam.transform.position) > this.renderingDistance;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000AC80 File Offset: 0x00008E80
	private void Update()
	{
		if (!this.setup && MainCamera.Camera != null)
		{
			this.SetupCamera();
		}
		this.HandleTextureRelease();
		if (this.disabled)
		{
			return;
		}
		this.HandleTravellers();
		if (this.debugStatic)
		{
			foreach (Ray ray in this.regularRaysToDraw)
			{
				Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.yellow);
			}
			foreach (Ray ray2 in this.wrongRaysToDraw)
			{
				Debug.DrawRay(ray2.origin, ray2.direction * 1000f, Color.red);
			}
			foreach (Ray ray3 in this.hitRaysToDraw)
			{
				Debug.DrawRay(ray3.origin, ray3.direction * 1000f, Color.green);
			}
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000ADE4 File Offset: 0x00008FE4
	private void HandleTextureRelease()
	{
		if (this.markedForRelease)
		{
			bool flag = false;
			EasyPortal.PortalStyles portalStyles = this.portalStyle;
			if (portalStyles != EasyPortal.PortalStyles.LinkedPortal)
			{
				if (portalStyles == EasyPortal.PortalStyles.DestinationOnly)
				{
					flag = this.PlayerFarFromPortal();
				}
			}
			else
			{
				flag = this.PlayerFarFromPortal() && this.linkedPortal.PlayerFarFromPortal();
			}
			if (flag)
			{
				this.ReleaseTextures();
			}
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000AE34 File Offset: 0x00009034
	public void SetExtraCam(bool status)
	{
		this.extraCamEnabled = status;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000AE40 File Offset: 0x00009040
	private void LateUpdate()
	{
		if (this.gotDisableProbe)
		{
			this.disableRefProbeWhenActive.enabled = this.disabled;
		}
		if (this.disabled)
		{
			return;
		}
		this.visibleFromCam = CameraUtility.VisibleFromCamera(this.screen, this.playerCam);
		if (this.renderingStyle == EasyPortal.RenderingStyles.RenderInVicinity || this.renderingStyle == EasyPortal.RenderingStyles.AlwaysRender)
		{
			this.staticLerp = 0f;
			this.screen.material.SetFloat("_StaticLerp", this.staticLerp);
			return;
		}
		this.playerVeryCloseToPortal = Vector3.Distance(base.transform.position, this.raycastTarget.transform.position) <= this.notStaticDistance;
		this.portalSeesPlayer = this.RaycastPortalToPlayer(4f, 4f);
		if (GameMaster.PAUSEMENUACTIVE || this.playerVeryCloseToPortal || this.forcedRendering || this.portalSeesPlayer || this.justTeleported)
		{
			this.staticLerp = 0f;
			this.forcedRendering = false;
		}
		else
		{
			this.staticLerp = 1f;
		}
		this.screen.material.SetFloat("_StaticLerp", this.staticLerp);
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000AF68 File Offset: 0x00009168
	private void HandleTravellers()
	{
		for (int i = 0; i < this.allTravelers.Length; i++)
		{
			PortalTraveller portalTraveller = this.allTravelers[i];
			if (this.distanceToPortal <= this.renderingDistance && !this.trackedTravellers.Contains(portalTraveller))
			{
				if (this.PointInOABB(portalTraveller.transform.position, this.screenCollider))
				{
					portalTraveller.EnterPortalThreshold();
					this.OnTravellerEnterPortal(portalTraveller, false);
					this.trackedTravellers.Add(portalTraveller);
					this.trackedTravellers.RemoveAt(i);
				}
			}
			else if (!this.PointInOABB(portalTraveller.transform.position, this.screenCollider))
			{
				portalTraveller.ExitPortalThreshold();
				this.trackedTravellers.Remove(portalTraveller);
			}
		}
		for (int j = 0; j < this.trackedTravellers.Count; j++)
		{
			PortalTraveller portalTraveller2 = this.trackedTravellers[j];
			Transform transform = portalTraveller2.transform;
			Matrix4x4 matrix4x = this.helperTransform.localToWorldMatrix * base.transform.worldToLocalMatrix * transform.localToWorldMatrix;
			Vector3 vector = transform.position - base.transform.position;
			int num = Math.Sign(Vector3.Dot(vector, base.transform.forward));
			int num2 = Math.Sign(Vector3.Dot(portalTraveller2.previousOffsetFromPortal, base.transform.forward));
			if (num != num2)
			{
				Vector3 position = transform.position;
				Quaternion rotation = transform.rotation;
				portalTraveller2.Teleport(base.transform, this.destination, matrix4x.GetColumn(3), matrix4x.rotation);
				if (portalTraveller2.InstantiateClone)
				{
					portalTraveller2.graphicsClone.transform.SetPositionAndRotation(position, rotation);
				}
				if (this.linkedPortal != null)
				{
					this.linkedPortal.OnTravellerEnterPortal(portalTraveller2, false);
				}
				this.trackedTravellers.RemoveAt(j);
				if (this.printDebug)
				{
					Debug.Log(base.gameObject.name + "REMOVING TRAVELLER BECAUSE HE IS TELEPORTING!");
				}
				j--;
				this.justTeleported = true;
				if (this.linkedPortal != null)
				{
					this.linkedPortal.justTeleported = true;
				}
				this.screen.enabled = false;
				this.SetVisiblePortalsVisible();
			}
			else
			{
				if (portalTraveller2.InstantiateClone)
				{
					portalTraveller2.graphicsClone.transform.SetPositionAndRotation(matrix4x.GetColumn(3), matrix4x.rotation);
				}
				portalTraveller2.previousOffsetFromPortal = vector;
			}
		}
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000B1D8 File Offset: 0x000093D8
	private bool PointInOABB(Vector3 point, BoxCollider box)
	{
		point = box.transform.InverseTransformPoint(point);
		Vector3 center = box.center;
		Vector3 size = box.size;
		float num = size.x * 0.5f + point.x - center.x;
		if (num < 0f || num > size.x)
		{
			return false;
		}
		float num2 = size.y * 0.5f + point.y - center.y;
		if (num2 < 0f || num2 > size.y)
		{
			return false;
		}
		float num3 = size.z * 0.5f + point.z - center.z;
		return num3 >= 0f && num3 <= size.z;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000B290 File Offset: 0x00009490
	private bool RaycastPortalToPlayer(float xSamples = 4f, float ySamples = 4f)
	{
		Vector3 position = this.raycastTarget.transform.position;
		if (this.onlyCheckForwardDirection)
		{
			Vector3 vector = base.transform.position - position;
			if (Vector3.Dot(base.transform.forward, vector) < 0f)
			{
				return false;
			}
		}
		if (Vector3.Distance(position, base.transform.position) > this.renderingDistance)
		{
			return false;
		}
		MeshRenderer meshRenderer = this.screen;
		float x = this.screen.transform.lossyScale.x;
		float y = this.screen.transform.lossyScale.y;
		float z = this.screen.transform.lossyScale.z;
		Physics.queriesHitBackfaces = true;
		int num = 0;
		while ((float)num <= xSamples)
		{
			float num2 = (float)num / xSamples * x;
			int num3 = 0;
			while ((float)num3 <= ySamples)
			{
				float num4 = (float)num3 / ySamples * y;
				Vector3 vector2 = new Vector3(num2 - x / 2f, num4, z);
				vector2 = base.transform.TransformPoint(vector2);
				Ray ray = new Ray(vector2, position - vector2);
				RaycastHit raycastHit;
				if (Physics.Raycast(vector2, (position - vector2).normalized * this.renderingDistance, out raycastHit, this.renderingDistance, this.raycastMask, QueryTriggerInteraction.Ignore))
				{
					if (raycastHit.collider.tag.Equals("Player"))
					{
						Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(vector2, raycastHit.point), Color.green);
						Physics.queriesHitBackfaces = false;
						return true;
					}
					if (raycastHit.collider != null)
					{
						Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(vector2, raycastHit.point), Color.yellow);
					}
				}
				else
				{
					Debug.DrawRay(ray.origin, ray.direction * float.PositiveInfinity, Color.red);
				}
				num3++;
			}
			num++;
		}
		Physics.queriesHitBackfaces = false;
		return false;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000B4B0 File Offset: 0x000096B0
	private bool IsValidRenderingStyle()
	{
		switch (this.renderingStyle)
		{
		case EasyPortal.RenderingStyles.AlwaysRender:
			return true;
		case EasyPortal.RenderingStyles.RenderInVicinity:
			return this.distanceToPortal <= this.renderingDistance;
		case EasyPortal.RenderingStyles.RaycastBeforeRender:
			return this.portalSeesPlayer;
		default:
			return false;
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000B4F4 File Offset: 0x000096F4
	private bool DestinationSeesPortal(EasyPortal portalToCheck, float xSamples = 4f, float ySamples = 4f)
	{
		MeshRenderer meshRenderer = portalToCheck.screen;
		float x = meshRenderer.transform.lossyScale.x;
		float y = meshRenderer.transform.lossyScale.y;
		float z = meshRenderer.transform.lossyScale.z;
		Physics.queriesHitBackfaces = true;
		int num = 0;
		while ((float)num <= xSamples)
		{
			float num2 = (float)num / xSamples * x;
			int num3 = 0;
			while ((float)num3 <= ySamples)
			{
				float num4 = (float)num3 / ySamples * y;
				Vector3 vector = new Vector3(num2 - x / 2f, num4, z);
				vector = portalToCheck.transform.TransformPoint(vector);
				if (this.portalStyle == EasyPortal.PortalStyles.LinkedPortal)
				{
					Vector3 vector2 = this.linkedPortal.transform.position + Vector3.up - vector;
					Ray ray = new Ray(vector, vector2);
					RaycastHit raycastHit;
					if (Physics.Raycast(vector, vector2.normalized * portalToCheck.renderingDistance, out raycastHit, portalToCheck.renderingDistance, this.playerCam.cullingMask, QueryTriggerInteraction.Collide))
					{
						if (raycastHit.collider.tag.Equals("Portal"))
						{
							bool flag = false;
							if (raycastHit.collider.gameObject == this.linkedPortal.screen.gameObject)
							{
								flag = true;
							}
							else
							{
								this.wrongRaysToDraw.Add(ray);
							}
							Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(vector, raycastHit.point), flag ? Color.green : Color.magenta);
							Physics.queriesHitBackfaces = false;
							if (flag)
							{
								this.hitRaysToDraw.Add(ray);
								return true;
							}
						}
						else if (raycastHit.collider != null)
						{
							this.regularRaysToDraw.Add(ray);
							Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(vector, raycastHit.point), Color.yellow);
						}
					}
					else
					{
						Debug.DrawRay(ray.origin, ray.direction * float.PositiveInfinity, Color.red);
					}
				}
				num3++;
			}
			num++;
		}
		Physics.queriesHitBackfaces = false;
		return false;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000B720 File Offset: 0x00009920
	public void SetNewDestination(Transform newDestination)
	{
		EasyPortal easyPortal = newDestination.GetComponent<EasyPortal>();
		if (easyPortal == null)
		{
			PortalDoor component = newDestination.GetComponent<PortalDoor>();
			if (component != null)
			{
				easyPortal = component.EasyPortal;
			}
			if (easyPortal == null)
			{
				PortalDoor component2 = newDestination.GetComponent<PortalDoor>();
				if (component2 != null)
				{
					easyPortal = component2.EasyPortal;
				}
			}
		}
		if (easyPortal != null)
		{
			this.portalStyle = EasyPortal.PortalStyles.LinkedPortal;
			this.linkedPortal = easyPortal;
			this.destination = this.linkedPortal.transform;
		}
		else
		{
			this.portalStyle = EasyPortal.PortalStyles.DestinationOnly;
			this.destination = newDestination;
		}
		this.UpdateHelperTransform();
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00005444 File Offset: 0x00003644
	public void PrePortalRender()
	{
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000B7B4 File Offset: 0x000099B4
	public void SetStatus(bool status, bool release = false)
	{
		this.disabled = !status;
		this.screen.enabled = status;
		if (this.screenCollider != null)
		{
			this.screenCollider.enabled = status;
		}
		if (!status)
		{
			if (this.printDebug)
			{
				Debug.Log("CLEARING ALL TRAVELLERS!!");
			}
			if (!this.clearTravellersOnlyThroughColliders)
			{
				this.trackedTravellers.Clear();
			}
			if (release)
			{
				this.ReleaseTextures();
			}
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000B822 File Offset: 0x00009A22
	public void SetStatus(bool status)
	{
		this.SetStatus(status, true);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000B82C File Offset: 0x00009A2C
	private void OnVisible()
	{
		if (this.disabled)
		{
			return;
		}
		this.CreateStaticTexture();
		if (this.extraCam != null)
		{
			this.CreateExtraTexture();
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000B851 File Offset: 0x00009A51
	private void OnInvisible()
	{
		this.markedForRelease = true;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000B85C File Offset: 0x00009A5C
	private void OnDestroy()
	{
		this.ReleaseTextures();
		PortalScreen portalScreen = this.portalScreen;
		portalScreen.OnVisible = (Action)Delegate.Remove(portalScreen.OnVisible, new Action(this.OnVisible));
		PortalScreen portalScreen2 = this.portalScreen;
		portalScreen2.OnInvisible = (Action)Delegate.Remove(portalScreen2.OnInvisible, new Action(this.OnInvisible));
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000B8C0 File Offset: 0x00009AC0
	private void ReleaseTextures()
	{
		if (this.viewTexture != null)
		{
			this.viewTexture.Release();
		}
		this.viewTexture = null;
		if (this.staticViewTexture != null)
		{
			this.staticViewTexture.Release();
		}
		this.staticViewTexture = null;
		if (this.extraTexture != null)
		{
			this.extraTexture.Release();
		}
		this.extraTexture = null;
		this.markedForRelease = false;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000B934 File Offset: 0x00009B34
	public void Render()
	{
		if (this.disabled || !this.setup || this.portalCam == null)
		{
			return;
		}
		this.distanceToPortal = Vector3.Distance(this.playerCam.transform.position, base.transform.position);
		this.portalCam.farClipPlane = this.distanceToPortal + this.portalCameraFarClip;
		Debug.DrawLine(this.playerCam.transform.position, base.transform.position, Color.cyan);
		if (!this.visibleFromCam)
		{
			if (this.visible)
			{
				this.CreateStaticTexture();
				this.UpdateStaticTexture(true, false);
			}
			this.visible = false;
			return;
		}
		if (this.justTeleported || this.renderingStyle == EasyPortal.RenderingStyles.AlwaysRender)
		{
			this.screen.material.SetFloat("_StaticLerp", 0f);
			this.RenderRecursiveVisiblePortals();
			this.visible = true;
			this.justTeleported = false;
			this.screen.enabled = true;
		}
		else if (this.distanceToPortal <= this.notStaticDistance)
		{
			this.visible = true;
		}
		else if (this.distanceToPortal > this.notStaticDistance && this.distanceToPortal <= this.renderingDistance)
		{
			bool flag = this.IsValidRenderingStyle();
			if (!flag && this.visible)
			{
				this.CreateStaticTexture();
				this.UpdateStaticTexture(!this.visibleFromCam, false);
			}
			this.visible = flag;
		}
		else
		{
			if (this.visible)
			{
				this.CreateStaticTexture();
				this.UpdateStaticTexture(!this.visibleFromCam, false);
			}
			this.visible = false;
		}
		if (this.visible)
		{
			this.CreateViewTexture();
			this.portalCam.projectionMatrix = this.playerCam.projectionMatrix;
			Vector3 vector = base.transform.InverseTransformPoint(this.playerCam.transform.position);
			Vector3 vector2 = this.helperTransform.TransformPoint(vector);
			Matrix4x4 localToWorldMatrix = this.playerCam.transform.localToWorldMatrix;
			Matrix4x4 matrix4x = this.helperTransform.localToWorldMatrix * base.transform.worldToLocalMatrix * localToWorldMatrix;
			this.portalCam.transform.position = vector2;
			this.portalCam.transform.rotation = matrix4x.rotation;
			if (this.linkedPortal != null)
			{
				this.linkedPortal.screen.enabled = false;
			}
			this.HandleOcclusionCulling();
			this.SetNearClipPlane(null);
			this.RenderRecursiveVisiblePortals();
			this.portalCam.targetTexture = this.viewTexture;
			if (this.extraCamEnabled && this.extraCam != null)
			{
				this.extraCam.fieldOfView = this.playerCam.fieldOfView;
				this.extraCam.targetTexture = this.extraTexture;
				this.extraCam.Render();
				this.screen.material.SetTexture("_ExtraTex", this.extraTexture);
			}
			else if (this.extraCam != null)
			{
				this.extraCam.enabled = false;
			}
			this.portalCam.Render();
			this.screen.material.SetTexture("_MainTex", this.viewTexture);
			this.ClearRecursive();
			if (this.linkedPortal != null && !this.linkedPortal.disabled)
			{
				this.linkedPortal.screen.enabled = true;
			}
		}
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000BC88 File Offset: 0x00009E88
	public void ClearRecursive()
	{
		for (int i = 0; i < MainCamera.Portals.Length; i++)
		{
			EasyPortal easyPortal = MainCamera.Portals[i];
			easyPortal.screen.material.SetTexture("_MainTex", easyPortal.viewTexture);
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000BCCC File Offset: 0x00009ECC
	public void RenderRecursiveVisiblePortals()
	{
		for (int i = 0; i < this.recursiveVisiblePortals.Count; i++)
		{
			EasyPortal easyPortal = this.recursiveVisiblePortals[i];
			if (!easyPortal.gameObject.activeInHierarchy)
			{
				return;
			}
			if (CameraUtility.VisibleFromCamera(easyPortal.screen, this.portalCam))
			{
				easyPortal.ForceRenderingFromCamera(this.portalCam, 0);
			}
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000BD2C File Offset: 0x00009F2C
	public void SetVisiblePortalsVisible()
	{
		for (int i = 0; i < this.recursiveVisiblePortals.Count; i++)
		{
			if (CameraUtility.VisibleFromCamera(this.recursiveVisiblePortals[i].screen, this.playerCam))
			{
				this.recursiveVisiblePortals[i].justTeleported = true;
				Matrix4x4 matrix4x = this.playerCam.projectionMatrix * this.playerCam.worldToCameraMatrix;
				this.recursiveVisiblePortals[i].screen.material.SetMatrix("_WorldToCam", matrix4x);
			}
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000BDBC File Offset: 0x00009FBC
	private void FindRecursivePortals(ref List<EasyPortal> visiblePortalList, bool returnPortalsOnly = false)
	{
		for (int i = 0; i < MainCamera.Portals.Length; i++)
		{
			EasyPortal easyPortal = MainCamera.Portals[i];
			if (easyPortal != this && easyPortal != this.linkedPortal && returnPortalsOnly && this.DestinationSeesPortal(easyPortal, 4f, 4f) && !this.visiblePortals.Contains(easyPortal))
			{
				this.visiblePortals.Add(easyPortal);
			}
		}
		if (returnPortalsOnly)
		{
			for (int j = 0; j < this.visiblePortals.Count; j++)
			{
				EasyPortal easyPortal2 = this.visiblePortals[j];
				if (!visiblePortalList.Contains(easyPortal2))
				{
					visiblePortalList.Add(easyPortal2);
				}
			}
		}
		else
		{
			for (int k = 0; k < this.visiblePortals.Count; k++)
			{
				this.visiblePortals[k].linkedPortal.ForceRenderingFromCamera(this.portalCam, 0);
			}
		}
		this.visiblePortals.Clear();
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
	private void ForceRenderingFromCamera(Camera cam, int currentDepth = 0)
	{
		if (this.disabled)
		{
			return;
		}
		this.SetSecondPortalLayer(false);
		this.portalCam.projectionMatrix = cam.projectionMatrix;
		Vector3 vector = base.transform.InverseTransformPoint(cam.transform.position);
		Vector3 vector2 = this.helperTransform.TransformPoint(vector);
		Matrix4x4 localToWorldMatrix = cam.transform.localToWorldMatrix;
		Matrix4x4 matrix4x = this.helperTransform.localToWorldMatrix * base.transform.worldToLocalMatrix * localToWorldMatrix;
		this.portalCam.transform.position = vector2;
		this.portalCam.transform.rotation = matrix4x.rotation;
		this.CreateRecursiveTexture();
		this.portalCam.targetTexture = this.recursiveTexture;
		this.screen.material.SetTexture("_MainTex", this.recursiveTexture);
		this.screen.material.SetFloat("_StaticLerp", 0f);
		this.SetNearClipPlane(cam);
		this.HandleOcclusionCulling();
		if (this.linkedPortal != null)
		{
			this.linkedPortal.screen.enabled = false;
		}
		this.portalCam.Render();
		if (this.linkedPortal != null && !this.linkedPortal.disabled)
		{
			this.linkedPortal.screen.enabled = true;
		}
		this.forcedRendering = true;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000C008 File Offset: 0x0000A208
	private void UpdateStaticTexture(bool neutralPosition = false, bool fixedDistance = false)
	{
		if (this.debugStatic)
		{
			Debug.Log("Updating static texture. Neutral : " + neutralPosition.ToString());
		}
		Vector3 position = this.playerCam.transform.position;
		Quaternion rotation = this.playerCam.transform.rotation;
		if (neutralPosition)
		{
			float num = Mathf.Clamp(Vector3.Distance(this.playerCam.transform.position, base.transform.position), 12f, this.renderingDistance);
			if (fixedDistance)
			{
				num = 3.5f;
			}
			this.playerCam.transform.position = base.transform.TransformPoint(Vector3.forward * num + Vector3.up * 0.6f);
			this.playerCam.transform.rotation = Quaternion.LookRotation(base.transform.position + Vector3.up - this.playerCam.transform.position, Vector3.up);
		}
		this.portalCam.targetTexture = this.staticViewTexture;
		Vector3 vector = base.transform.InverseTransformPoint(this.playerCam.transform.position);
		Vector3 vector2 = this.helperTransform.TransformPoint(vector);
		Matrix4x4 localToWorldMatrix = this.playerCam.transform.localToWorldMatrix;
		Matrix4x4 matrix4x = this.helperTransform.localToWorldMatrix * base.transform.worldToLocalMatrix * localToWorldMatrix;
		this.portalCam.transform.position = vector2;
		this.portalCam.transform.rotation = matrix4x.rotation;
		this.HandleOcclusionCulling();
		this.SetNearClipPlane(null);
		if (this.linkedPortal != null)
		{
			this.linkedPortal.screen.enabled = false;
		}
		this.portalCam.Render();
		if (this.linkedPortal != null && !this.linkedPortal.disabled)
		{
			this.linkedPortal.screen.enabled = true;
		}
		this.camMatrix = this.playerCam.projectionMatrix * this.playerCam.worldToCameraMatrix;
		this.screen.material.SetMatrix("_WorldToCam", this.camMatrix);
		this.playerCam.transform.position = position;
		this.playerCam.transform.rotation = rotation;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000C270 File Offset: 0x0000A470
	private void HandleOcclusionCulling()
	{
		if (this.disableCameraOcclusionCulling)
		{
			this.portalCam.useOcclusionCulling = false;
			return;
		}
		if (Vector3.Distance(base.transform.position, this.playerCam.transform.position) <= this.occlusionCullingDisableDistance)
		{
			this.portalCam.useOcclusionCulling = false;
			return;
		}
		this.portalCam.useOcclusionCulling = true;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
	private void CreateStaticTexture()
	{
		if (this.staticViewTexture == null || this.staticViewTexture.width != Screen.width / 2 || this.staticViewTexture.height != Screen.height / 2)
		{
			if (this.staticViewTexture != null)
			{
				this.staticViewTexture.Release();
			}
			this.staticViewTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 32, RenderTextureFormat.Default);
			this.screen.material.SetTexture("_StaticTex", this.staticViewTexture);
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000C368 File Offset: 0x0000A568
	private void CreateViewTexture()
	{
		if (this.viewTexture == null || this.viewTexture.width != Screen.width || this.viewTexture.height != Screen.height)
		{
			if (this.viewTexture != null)
			{
				this.viewTexture.Release();
			}
			this.viewTexture = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.Default);
			this.viewTexture.antiAliasing = 2;
			this.portalCam.targetTexture = this.viewTexture;
			this.screen.material.SetTexture("_MainTex", this.viewTexture);
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000C410 File Offset: 0x0000A610
	private void CreateExtraTexture()
	{
		if (this.extraTexture == null || this.extraTexture.width != Screen.width || this.extraTexture.height != Screen.height)
		{
			if (this.extraTexture != null)
			{
				this.extraTexture.Release();
			}
			this.extraTexture = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.Default);
			this.extraCam.targetTexture = this.extraTexture;
			this.screen.material.SetTexture("_ExtraTex", this.extraTexture);
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000C4AC File Offset: 0x0000A6AC
	private void CreateRecursiveTexture()
	{
		if (this.recursiveTexture == null || this.recursiveTexture.width != Screen.width || this.recursiveTexture.height != Screen.height)
		{
			if (this.recursiveTexture != null)
			{
				this.recursiveTexture.Release();
			}
			this.recursiveTexture = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.Default);
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000C51C File Offset: 0x0000A71C
	public void PostPortalRender()
	{
		if (this.playerCam != null)
		{
			this.ProtectScreenFromClipping(this.playerCam.transform.position);
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000C544 File Offset: 0x0000A744
	private float ProtectScreenFromClipping(Vector3 viewPoint)
	{
		if (this.screen == null)
		{
			return 0f;
		}
		float num = this.playerCam.nearClipPlane * Mathf.Tan(this.playerCam.fieldOfView * 0.5f * 0.017453292f);
		float magnitude = new Vector3(num * this.playerCam.aspect, num, this.playerCam.nearClipPlane).magnitude;
		Transform transform = this.screen.transform;
		bool flag = Vector3.Dot(base.transform.forward, base.transform.position - viewPoint) > 0f;
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, magnitude);
		transform.localPosition = Vector3.forward * magnitude * (flag ? 0.5f : (-0.5f));
		return magnitude;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000C634 File Offset: 0x0000A834
	private void SetNearClipPlane(Camera customCam = null)
	{
		Transform transform = base.transform;
		int num = Math.Sign(Vector3.Dot(transform.forward, base.transform.position - this.playerCam.transform.position));
		Camera camera = ((customCam != null) ? customCam : this.playerCam);
		Vector3 vector = camera.worldToCameraMatrix.MultiplyPoint(transform.position);
		Vector3 vector2 = camera.worldToCameraMatrix.MultiplyVector(transform.forward) * (float)num;
		float num2 = -Vector3.Dot(vector, vector2) + this.nearClipOffset;
		if (this.useOblique && Mathf.Abs(num2) > this.nearClipLimit)
		{
			Vector4 vector3 = new Vector4(vector2.x, vector2.y, vector2.z, num2);
			this.portalCam.projectionMatrix = camera.CalculateObliqueMatrix(vector3);
			return;
		}
		this.portalCam.nearClipPlane = Mathf.Abs(num2);
		this.portalCam.projectionMatrix = this.playerCam.projectionMatrix;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0000C73C File Offset: 0x0000A93C
	private void OnTravellerEnterPortal(PortalTraveller traveller, bool triggerEnter = false)
	{
		if (!this.trackedTravellers.Contains(traveller))
		{
			traveller.EnterPortalThreshold();
			traveller.previousOffsetFromPortal = traveller.transform.position - base.transform.position;
			this.trackedTravellers.Add(traveller);
			if (this.printDebug)
			{
				if (triggerEnter)
				{
					Debug.Log(base.gameObject.name + "ADD TRAVELER THROUGH TRIGGER!!!");
					return;
				}
				Debug.Log(base.gameObject.name + "ADD TRAVELER THROUGH EXTERNALLY");
			}
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000C7CA File Offset: 0x0000A9CA
	private int SideOfPortal(Vector3 pos)
	{
		return Math.Sign(Vector3.Dot(pos - base.transform.position, base.transform.forward));
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000C7F2 File Offset: 0x0000A9F2
	private bool SameSideOfPortal(Vector3 posA, Vector3 posB)
	{
		return this.SideOfPortal(posA) == this.SideOfPortal(posB);
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000C804 File Offset: 0x0000AA04
	private Vector3 portalCamPos
	{
		get
		{
			return this.portalCam.transform.position;
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00005444 File Offset: 0x00003644
	private void OnValidate()
	{
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000C816 File Offset: 0x0000AA16
	public float GetDistanceToPlayer()
	{
		return Vector3.Distance(base.transform.position, this.playerCam.transform.position);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000C838 File Offset: 0x0000AA38
	public float CompareForwardToPlayerCam()
	{
		return Vector3.Dot(Vector3.ProjectOnPlane(this.playerCam.transform.forward, Vector3.up), Vector3.ProjectOnPlane(base.transform.forward, Vector3.up));
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000C86E File Offset: 0x0000AA6E
	int IComparable<EasyPortal>.CompareTo(EasyPortal other)
	{
		if (this.GetDistanceToPlayer() <= other.GetDistanceToPlayer())
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	private EasyPortal.PortalStyles portalStyle;

	// Token: 0x040001C9 RID: 457
	[Header("Main Settings")]
	public EasyPortal linkedPortal;

	// Token: 0x040001CA RID: 458
	[SerializeField]
	private Transform destination;

	// Token: 0x040001CB RID: 459
	public MeshRenderer screen;

	// Token: 0x040001CC RID: 460
	private PortalScreen portalScreen;

	// Token: 0x040001CD RID: 461
	public float colliderLength = 100f;

	// Token: 0x040001CE RID: 462
	[Header("Rendering Settings")]
	[SerializeField]
	private EasyPortal.RenderingStyles renderingStyle = EasyPortal.RenderingStyles.RaycastBeforeRender;

	// Token: 0x040001CF RID: 463
	public float renderingDistance = 12f;

	// Token: 0x040001D0 RID: 464
	public float portalCameraFarClip = 12f;

	// Token: 0x040001D1 RID: 465
	[Header("Advanced Rendering Settings")]
	[SerializeField]
	private float nearClipOffset = 0.01f;

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	private float nearClipLimit = 0.2f;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private bool onlyCheckForwardDirection;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private bool useOblique = true;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	private bool useCustomLayerMaskRendering;

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	private LayerMask customMask;

	// Token: 0x040001D7 RID: 471
	public float occlusionCullingDisableDistance = 5f;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	private bool renderSecondInvisiblePortalLayer;

	// Token: 0x040001D9 RID: 473
	[Header("Customization")]
	[SerializeField]
	private Camera extraCam;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	private bool extraCamEnabled;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private bool useCustomScreenCollider;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	private bool clearTravellersOnlyThroughColliders;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	private bool printDebug;

	// Token: 0x040001DE RID: 478
	[Header("Debug - Dont change these")]
	[SerializeField]
	private float distanceToPortal;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	public bool justTeleported;

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	private bool visible;

	// Token: 0x040001E1 RID: 481
	[SerializeField]
	public bool disabled;

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	private bool portalSeesPlayer;

	// Token: 0x040001E3 RID: 483
	[SerializeField]
	private List<EasyPortal> recursiveVisiblePortals;

	// Token: 0x040001E4 RID: 484
	[HideInInspector]
	[SerializeField]
	private bool setup;

	// Token: 0x040001E5 RID: 485
	[HideInInspector]
	public RenderTexture viewTexture;

	// Token: 0x040001E6 RID: 486
	[HideInInspector]
	public RenderTexture staticViewTexture;

	// Token: 0x040001E7 RID: 487
	[HideInInspector]
	public RenderTexture recursiveTexture;

	// Token: 0x040001E8 RID: 488
	[HideInInspector]
	public RenderTexture extraTexture;

	// Token: 0x040001E9 RID: 489
	[HideInInspector]
	public Camera portalCam;

	// Token: 0x040001EA RID: 490
	public Transform helperTransform;

	// Token: 0x040001EB RID: 491
	private Camera playerCam;

	// Token: 0x040001EC RID: 492
	private Transform raycastTarget;

	// Token: 0x040001ED RID: 493
	private float staticLerp;

	// Token: 0x040001EE RID: 494
	private Matrix4x4 camMatrix;

	// Token: 0x040001EF RID: 495
	[SerializeField]
	private List<PortalTraveller> trackedTravellers = new List<PortalTraveller>();

	// Token: 0x040001F0 RID: 496
	private List<EasyPortal> visiblePortals = new List<EasyPortal>();

	// Token: 0x040001F1 RID: 497
	private float notStaticDistance = 2f;

	// Token: 0x040001F2 RID: 498
	private bool playerVeryCloseToPortal;

	// Token: 0x040001F3 RID: 499
	private bool visibleFromCam;

	// Token: 0x040001F4 RID: 500
	private bool forcedRendering;

	// Token: 0x040001F5 RID: 501
	private BoxCollider screenCollider;

	// Token: 0x040001F6 RID: 502
	private List<Ray> regularRaysToDraw = new List<Ray>();

	// Token: 0x040001F7 RID: 503
	private List<Ray> wrongRaysToDraw = new List<Ray>();

	// Token: 0x040001F8 RID: 504
	private List<Ray> hitRaysToDraw = new List<Ray>();

	// Token: 0x040001F9 RID: 505
	[Header("Reflection Probe Stuff")]
	[InspectorButton("FindClosestRefProbe", "Find Closest Ref Probe")]
	[SerializeField]
	[FormerlySerializedAs("closestRefProbe")]
	private ReflectionProbe roomReflectionProbe;

	// Token: 0x040001FA RID: 506
	[SerializeField]
	private Transform roomReflectionProbeLocation;

	// Token: 0x040001FB RID: 507
	[SerializeField]
	private bool disableRefProbeCopyOnThisPortalsRefProbe;

	// Token: 0x040001FC RID: 508
	public bool debugStatic;

	// Token: 0x040001FD RID: 509
	private bool hasAttemptedRoomReflectionProbeAutoFind;

	// Token: 0x040001FE RID: 510
	private LayerMask raycastMask;

	// Token: 0x040001FF RID: 511
	private bool disableCameraOcclusionCulling = true;

	// Token: 0x04000200 RID: 512
	[Header("The Reflection Probe on the other side of the door that needs to be disabled if the door is being useda as a portal")]
	public ReflectionProbe disableRefProbeWhenActive;

	// Token: 0x04000201 RID: 513
	private bool gotDisableProbe;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	private PortalTraveller[] allTravelers;

	// Token: 0x04000203 RID: 515
	private bool markedForRelease;

	// Token: 0x02000363 RID: 867
	public enum RenderingFallbacks
	{
		// Token: 0x04001246 RID: 4678
		NoFallback,
		// Token: 0x04001247 RID: 4679
		LastRenderedFrame
	}

	// Token: 0x02000364 RID: 868
	public enum RenderingStyles
	{
		// Token: 0x04001249 RID: 4681
		AlwaysRender,
		// Token: 0x0400124A RID: 4682
		RenderInVicinity,
		// Token: 0x0400124B RID: 4683
		RaycastBeforeRender
	}

	// Token: 0x02000365 RID: 869
	public enum PortalStyles
	{
		// Token: 0x0400124D RID: 4685
		LinkedPortal,
		// Token: 0x0400124E RID: 4686
		DestinationOnly
	}
}
