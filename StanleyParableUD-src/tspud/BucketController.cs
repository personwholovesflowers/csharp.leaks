using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000099 RID: 153
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class BucketController : MonoBehaviour
{
	// Token: 0x060003B1 RID: 945 RVA: 0x00018070 File Offset: 0x00016270
	private void Awake()
	{
		BucketController.HASBUCKET = this.BucketConfigurable.GetBooleanValue();
		this.bucketAnimator = base.GetComponent<Animator>();
		this.bucketAudioSource = base.GetComponent<AudioSource>();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x000180AC File Offset: 0x000162AC
	private void Start()
	{
		SimpleEvent simpleEvent = this.dropImmediateEvent;
		simpleEvent.OnCall = (Action)Delegate.Combine(simpleEvent.OnCall, new Action(this.DropBucketImmediate));
		MotionController.OnReducedMotionValueChanged += this.ResetAnimationSpeedImmediate;
		this.OnSceneReady();
		base.InvokeRepeating("UpdatePortals", 0f, 0.5f);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001810C File Offset: 0x0001630C
	private void OnDestroy()
	{
		SimpleEvent simpleEvent = this.dropImmediateEvent;
		simpleEvent.OnCall = (Action)Delegate.Remove(simpleEvent.OnCall, new Action(this.DropBucketImmediate));
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00018146 File Offset: 0x00016346
	private void OnSceneReady()
	{
		this.portalProbeCopies.Clear();
		this.allPortals.Clear();
		this.allPortals.AddRange(Object.FindObjectsOfType<EasyPortal>());
		this.cachedPlayerTransform = StanleyController.Instance.transform;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001817E File Offset: 0x0001637E
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.OnSceneReady();
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00018188 File Offset: 0x00016388
	private EasyPortal FindClosestPortal(Transform reference)
	{
		if (reference == null)
		{
			return null;
		}
		float num = float.PositiveInfinity;
		EasyPortal easyPortal = null;
		foreach (EasyPortal easyPortal2 in this.allPortals)
		{
			if (!(easyPortal2 == null) && !easyPortal2.Disabled && easyPortal2.isActiveAndEnabled)
			{
				float num2 = Vector3.Distance(reference.position, easyPortal2.transform.position);
				if (num2 < num)
				{
					num = num2;
					easyPortal = easyPortal2;
				}
			}
		}
		return easyPortal;
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00018224 File Offset: 0x00016424
	private ReflectionProbe GetReflectionProbeCopy(EasyPortal easyPortal)
	{
		if (!this.portalProbeCopies.ContainsKey(easyPortal))
		{
			if (easyPortal.RoomReflectionProbe == null)
			{
				Debug.LogWarning("Easyportal " + easyPortal + " has no reflection probe set", easyPortal);
				return null;
			}
			this.portalProbeCopies[easyPortal] = Object.Instantiate<GameObject>(easyPortal.RoomReflectionProbe.gameObject).GetComponent<ReflectionProbe>();
			if (Mathf.Abs(Mathf.Abs(Quaternion.Angle(easyPortal.transform.rotation, easyPortal.linkedPortal.transform.rotation)) - 90f) < 0.1f)
			{
				Vector3 size = easyPortal.RoomReflectionProbe.size;
				this.portalProbeCopies[easyPortal].size = new Vector3(size.z, size.y, size.x);
			}
			this.portalProbeCopies[easyPortal].bakedTexture = easyPortal.RoomReflectionProbe.bakedTexture;
		}
		return this.portalProbeCopies[easyPortal];
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001831C File Offset: 0x0001651C
	internal void PlayAdditiveAnimation(string v)
	{
		this.bucketAnimator.GetNextAnimatorClipInfo(this.bucketAnimator.GetLayerIndex("Additive"));
		if (BucketController.HASBUCKET)
		{
			this.bucketAnimator.SetTrigger("Additive/" + v);
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00018358 File Offset: 0x00016558
	private void UpdatePortals()
	{
		this.closestPortalMain = this.FindClosestPortal(this.cachedPlayerTransform);
		if (this.closestPortalMain == null)
		{
			return;
		}
		this.closestPortalPair = this.closestPortalMain.linkedPortal;
		this.closestPortalMainRPCopy = this.GetReflectionProbeCopy(this.closestPortalMain);
		this.closestPortalPairRPCopy = this.GetReflectionProbeCopy(this.closestPortalPair);
		foreach (EasyPortal easyPortal in this.portalProbeCopies.Keys)
		{
			bool flag = false;
			if ((easyPortal == this.closestPortalMain || easyPortal == this.closestPortalPair) && !this.closestPortalMain.DisableRefProbeCopyOnThisPortalsRefProbe)
			{
				flag = true;
			}
			this.portalProbeCopies[easyPortal].gameObject.SetActive(flag);
		}
		if (this.closestPortalMainRPCopy == null || this.closestPortalPairRPCopy == null)
		{
			Debug.LogError("Could not get reflection probe copies");
			return;
		}
		this.distanceToPortal = Vector3.Distance(this.closestPortalMain.transform.position, this.bucketRenderer.transform.position);
		this.buketAtOffsetPosition = this.distanceToPortal < this.turnOnDistance;
		this.closestPortalPair.MoveToLinkedPosition(this.closestPortalMainRPCopy.transform, this.closestPortalMain.RoomReflectionProbe.transform);
		this.closestPortalMain.MoveToLinkedPosition(this.closestPortalPairRPCopy.transform, this.closestPortalPair.RoomReflectionProbe.transform);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000184F8 File Offset: 0x000166F8
	private void OnBucketPickup(bool suppressAudio)
	{
		this.OnBucketPickup(suppressAudio, false);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00018502 File Offset: 0x00016702
	private void OnBucketPickup(bool suppressAudio, bool instantBucket)
	{
		if (instantBucket)
		{
			this.bucketAnimator.SetTrigger("InstaBucket");
		}
		this.bucketAnimator.SetBool("HasBucket", true);
		if (!suppressAudio)
		{
			this.bucketAudioSource.Play();
		}
		BucketController.HASBUCKET = true;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001853C File Offset: 0x0001673C
	private void OnBucketRemoval(bool instantBucket)
	{
		if (instantBucket)
		{
			this.bucketAnimator.SetTrigger("InstaBucket");
		}
		this.bucketAnimator.SetBool("HasBucket", false);
		this.bucketAudioSource.Stop();
		BucketController.HASBUCKET = false;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00018573 File Offset: 0x00016773
	public void DropBucketImmediate()
	{
		this.SetBucket(false, true, true, true);
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001857F File Offset: 0x0001677F
	public void SetBucket(bool status)
	{
		this.SetBucket(status, false, false, false);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001858B File Offset: 0x0001678B
	public void SetBucketWithConfigurable(bool status)
	{
		this.SetBucket(status, true, false, false);
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00018597 File Offset: 0x00016797
	public void SetBucket(bool status, bool setConfigurable, bool suppressAudio, bool instantBucket)
	{
		if (status)
		{
			this.OnBucketPickup(suppressAudio, instantBucket);
		}
		else
		{
			this.OnBucketRemoval(instantBucket);
		}
		if (setConfigurable)
		{
			this.BucketConfigurable.SetValue(status);
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060003C1 RID: 961 RVA: 0x000185BE File Offset: 0x000167BE
	private float ReduceMotionMultiplier
	{
		get
		{
			return (float)(MotionController.Instance.ReducedMotionMode ? 0 : 1);
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x000185D1 File Offset: 0x000167D1
	public void SetWalkingSpeed(float speed)
	{
		this.bucketAnimator.SetFloat("WalkingSpeed", speed, this.bucketMovementDamp, Time.deltaTime);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x000185EF File Offset: 0x000167EF
	public void SetAnimationSpeed(float speed)
	{
		this.lastSetSpeed = speed;
		this.bucketAnimator.SetFloat("AnimationSpeed", speed * this.ReduceMotionMultiplier, this.bucketMovementDamp, Time.deltaTime);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001861B File Offset: 0x0001681B
	public void ResetAnimationSpeedImmediate()
	{
		this.SetAnimationSpeedImmediate(this.lastSetSpeed);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00018629 File Offset: 0x00016829
	public void SetAnimationSpeedImmediate(float speed)
	{
		this.lastSetSpeed = speed;
		this.bucketAnimator.SetFloat("AnimationSpeed", speed * this.ReduceMotionMultiplier);
	}

	// Token: 0x040003A3 RID: 931
	public static bool HASBUCKET;

	// Token: 0x040003A4 RID: 932
	[SerializeField]
	private Animator bucketAnimator;

	// Token: 0x040003A5 RID: 933
	[SerializeField]
	private Transform bucketModel;

	// Token: 0x040003A6 RID: 934
	[SerializeField]
	private AudioSource bucketAudioSource;

	// Token: 0x040003A7 RID: 935
	[SerializeField]
	private float bucketMovementDamp = 0.8f;

	// Token: 0x040003A8 RID: 936
	[SerializeField]
	private BooleanConfigurable BucketConfigurable;

	// Token: 0x040003A9 RID: 937
	[SerializeField]
	private SimpleEvent dropImmediateEvent;

	// Token: 0x040003AA RID: 938
	[SerializeField]
	private MeshRenderer bucketRenderer;

	// Token: 0x040003AB RID: 939
	[SerializeField]
	private Transform bucketOffset;

	// Token: 0x040003AC RID: 940
	[SerializeField]
	private BooleanConfigurable defaultConfigurationConfigurable;

	// Token: 0x040003AD RID: 941
	private bool disableReflectionProbeLogic;

	// Token: 0x040003AE RID: 942
	private List<EasyPortal> allPortals = new List<EasyPortal>();

	// Token: 0x040003AF RID: 943
	private Dictionary<EasyPortal, ReflectionProbe> portalProbeCopies = new Dictionary<EasyPortal, ReflectionProbe>();

	// Token: 0x040003B0 RID: 944
	public Vector3 reflectionProbeOffset = Vector3.down * 2000f;

	// Token: 0x040003B1 RID: 945
	public bool buketAtOffsetPosition = true;

	// Token: 0x040003B2 RID: 946
	public float distanceToPortal;

	// Token: 0x040003B3 RID: 947
	public float turnOnDistance = 0.5f;

	// Token: 0x040003B4 RID: 948
	[Header("Debug")]
	public EasyPortal closestPortalMain;

	// Token: 0x040003B5 RID: 949
	public EasyPortal closestPortalPair;

	// Token: 0x040003B6 RID: 950
	public ReflectionProbe closestPortalMainRPCopy;

	// Token: 0x040003B7 RID: 951
	public ReflectionProbe closestPortalPairRPCopy;

	// Token: 0x040003B8 RID: 952
	private Transform cachedPlayerTransform;

	// Token: 0x040003B9 RID: 953
	private bool BucketBeginSet;

	// Token: 0x040003BA RID: 954
	private float lastSetSpeed = 1f;
}
