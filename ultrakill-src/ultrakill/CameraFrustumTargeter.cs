using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x0200009F RID: 159
[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CameraFrustumTargeter : MonoSingleton<CameraFrustumTargeter>
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000312 RID: 786 RVA: 0x000128BD File Offset: 0x00010ABD
	// (set) Token: 0x06000313 RID: 787 RVA: 0x000128C5 File Offset: 0x00010AC5
	public Collider CurrentTarget { get; private set; }

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000314 RID: 788 RVA: 0x000128CE File Offset: 0x00010ACE
	// (set) Token: 0x06000315 RID: 789 RVA: 0x000128D6 File Offset: 0x00010AD6
	public bool IsAutoAimed { get; private set; }

	// Token: 0x06000316 RID: 790 RVA: 0x000128E0 File Offset: 0x00010AE0
	protected override void Awake()
	{
		base.Awake();
		this.frustum = new Plane[6];
		this.corners = new Vector3[4];
		this.targets = new Collider[256];
		this.camera = base.GetComponent<Camera>();
		this.occluders = new RaycastHit[16];
		this.occlusionMask = LayerMaskDefaults.Get(LMD.Environment);
		this.occlusionMask |= 1;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00012958 File Offset: 0x00010B58
	private void Start()
	{
		CameraFrustumTargeter.isEnabled = MonoSingleton<PrefsManager>.Instance.GetBool("autoAim", false);
		this.maxHorAim = MonoSingleton<PrefsManager>.Instance.GetFloat("autoAimAmount", 0f);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00012989 File Offset: 0x00010B89
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000319 RID: 793 RVA: 0x000129B1 File Offset: 0x00010BB1
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600031A RID: 794 RVA: 0x000129D4 File Offset: 0x00010BD4
	private void OnPrefChanged(string key, object value)
	{
		if (!(key == "autoAim"))
		{
			if (!(key == "autoAimAmount"))
			{
				return;
			}
			if (value is float)
			{
				float num = (float)value;
				this.maxHorAim = num;
			}
		}
		else if (value is bool)
		{
			bool flag = (bool)value;
			CameraFrustumTargeter.isEnabled = flag;
			return;
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00012A2A File Offset: 0x00010C2A
	private bool RaycastFromViewportCenter(out RaycastHit hit)
	{
		return Physics.Raycast(this.camera.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, this.maximumRange, this.mask.value);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00012A64 File Offset: 0x00010C64
	private void CalculateFrustumInformation()
	{
		this.camera.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), this.maximumRange, Camera.MonoOrStereoscopicEye.Mono, this.corners);
		this.bounds = GeometryUtility.CalculateBounds(this.corners, this.camera.transform.localToWorldMatrix);
		this.bounds.size = new Vector3(this.bounds.size.x, this.bounds.size.y, this.maximumRange);
		this.bounds.center = base.transform.position;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00012B10 File Offset: 0x00010D10
	private void Update()
	{
		if (!CameraFrustumTargeter.isEnabled || this.maxHorAim == 0f)
		{
			this.CurrentTarget = null;
			this.IsAutoAimed = false;
			return;
		}
		RaycastHit raycastHit;
		if (this.RaycastFromViewportCenter(out raycastHit) && !Physics.Raycast(this.camera.ViewportPointToRay(new Vector2(0.5f, 0.5f)), raycastHit.distance, this.occlusionMask))
		{
			Collider collider = raycastHit.collider;
			if (!collider.isTrigger || collider.gameObject.layer == 22)
			{
				this.CurrentTarget = collider;
				this.IsAutoAimed = false;
				return;
			}
		}
		this.CalculateFrustumInformation();
		int num = Physics.OverlapBoxNonAlloc(this.bounds.center, this.bounds.extents, this.targets, base.transform.rotation, this.mask.value);
		float num2 = float.PositiveInfinity;
		Collider collider2 = null;
		for (int i = 0; i < num; i++)
		{
			Vector3 position = base.transform.position;
			Vector3 vector = this.targets[i].bounds.center - position;
			HookPoint hookPoint;
			Coin coin;
			Grenade grenade;
			if ((this.targets[i].gameObject.layer == 22 || !this.targets[i].isTrigger) && (this.targets[i].gameObject.layer != 22 || (this.targets[i].TryGetComponent<HookPoint>(out hookPoint) && hookPoint.active)) && (this.targets[i].gameObject.layer != 10 || this.targets[i].TryGetComponent<Coin>(out coin)) && (this.targets[i].gameObject.layer != 14 || this.targets[i].TryGetComponent<Grenade>(out grenade)))
			{
				int num3 = Physics.RaycastNonAlloc(position, vector, this.occluders, vector.magnitude, this.occlusionMask.value, QueryTriggerInteraction.Ignore);
				for (int j = 0; j < num3; j++)
				{
					if (!(this.occluders[j].collider == null))
					{
						goto IL_02EB;
					}
				}
				Vector3 vector2 = this.camera.WorldToViewportPoint(this.targets[i].bounds.center);
				float num4 = Vector3.Distance(vector2, new Vector2(0.5f, 0.5f));
				if (vector2.x <= 0.5f + this.maxHorAim / 2f && vector2.x >= 0.5f - this.maxHorAim / 2f && vector2.y <= 0.5f + this.maxHorAim / 2f && vector2.y >= 0.5f - this.maxHorAim / 2f && vector2.z >= 0f && num4 < num2)
				{
					num2 = num4;
					collider2 = this.targets[i];
				}
			}
			IL_02EB:;
		}
		this.CurrentTarget = collider2;
		this.IsAutoAimed = true;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00012E24 File Offset: 0x00011024
	private void LateUpdate()
	{
		if (this.CurrentTarget == null || !this.IsAutoAimed)
		{
			this.crosshair.anchoredPosition = Vector2.zero;
			return;
		}
		Vector2 vector = this.camera.WorldToViewportPoint(this.CurrentTarget.bounds.center) - new Vector2(0.5f, 0.5f);
		Vector2 referenceResolution = this.crosshair.GetParentCanvas().GetComponent<CanvasScaler>().referenceResolution;
		this.crosshair.anchoredPosition = Vector2.Scale(vector, referenceResolution);
	}

	// Token: 0x040003CF RID: 975
	private const int MaxPotentialTargets = 256;

	// Token: 0x040003D2 RID: 978
	public static bool isEnabled;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	private RectTransform crosshair;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	private LayerMask mask;

	// Token: 0x040003D5 RID: 981
	private LayerMask occlusionMask;

	// Token: 0x040003D6 RID: 982
	[SerializeField]
	private float maximumRange = 1000f;

	// Token: 0x040003D7 RID: 983
	public float maxHorAim = 1f;

	// Token: 0x040003D8 RID: 984
	private RaycastHit[] occluders;

	// Token: 0x040003D9 RID: 985
	private Plane[] frustum;

	// Token: 0x040003DA RID: 986
	private Vector3[] corners;

	// Token: 0x040003DB RID: 987
	private Bounds bounds;

	// Token: 0x040003DC RID: 988
	private Collider[] targets;

	// Token: 0x040003DD RID: 989
	private Camera camera;
}
