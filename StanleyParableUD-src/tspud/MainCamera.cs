using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200004B RID: 75
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
	// Token: 0x060001BB RID: 443 RVA: 0x0000C944 File Offset: 0x0000AB44
	private void Awake()
	{
		if (this.raycastTarget != null)
		{
			MainCamera.RaycastTarget = this.raycastTarget;
		}
		this.UpdatePortals();
		GameMaster.OnPause += this.OnPause;
		GameMaster.OnResume += this.OnResume;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000C9A3 File Offset: 0x0000ABA3
	private void Start()
	{
		MainCamera.Camera = base.GetComponent<Camera>();
		this.blur.enabled = false;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000C9BC File Offset: 0x0000ABBC
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.OnResume();
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
	private void OnPause()
	{
		this.HandleBlur();
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000C9CC File Offset: 0x0000ABCC
	private void OnResume()
	{
		this.blur.enabled = false;
		MainCamera.BlurValue = 0f;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
	private void OnDestroy()
	{
		GameMaster.OnPause -= this.OnPause;
		GameMaster.OnResume -= this.OnResume;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000CA1C File Offset: 0x0000AC1C
	private void HandleBlur()
	{
		if (this.TSPVersion.GetIntValue() == 1)
		{
			this.blur.material.SetColor("_Tint", Color.red);
		}
		else
		{
			this.blur.material.SetColor("_Tint", Color.white);
		}
		this.blur.BlurAmount = 0f;
		this.blur.enabled = true;
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000CA8C File Offset: 0x0000AC8C
	private void Update()
	{
		if (this.blur.enabled)
		{
			float num = Mathf.Lerp(this.blur.BlurAmount, 1.5f, Time.unscaledDeltaTime * 2f);
			this.blur.BlurAmount = num;
			MainCamera.BlurValue = num;
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000CADC File Offset: 0x0000ACDC
	public void UpdatePortals()
	{
		MainCamera.UseVicinityRenderingOnly = this.DefaultConfigurationConfigurable.GetBooleanValue();
		MainCamera.Portals = Object.FindObjectsOfType<EasyPortal>();
		this.portalGameObjects = new GameObject[MainCamera.Portals.Length];
		for (int i = 0; i < MainCamera.Portals.Length; i++)
		{
			this.portalGameObjects[i] = MainCamera.Portals[i].gameObject;
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000CB3B File Offset: 0x0000AD3B
	private void SortArray()
	{
		Array.Sort<EasyPortal>(MainCamera.Portals);
		this.portals = MainCamera.Portals;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000CB54 File Offset: 0x0000AD54
	private void OnPreCull()
	{
		for (int i = 0; i < MainCamera.Portals.Length; i++)
		{
			if (!MainCamera.Portals[i].disabled)
			{
				MainCamera.Portals[i].Render();
			}
		}
		for (int j = 0; j < MainCamera.Portals.Length; j++)
		{
			if (!MainCamera.Portals[j].disabled)
			{
				MainCamera.Portals[j].PostPortalRender();
			}
		}
	}

	// Token: 0x04000205 RID: 517
	public static Camera Camera;

	// Token: 0x04000206 RID: 518
	public static Transform RaycastTarget;

	// Token: 0x04000207 RID: 519
	public static float BlurValue;

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private MobileBlur blur;

	// Token: 0x04000209 RID: 521
	public static bool UseVicinityRenderingOnly;

	// Token: 0x0400020A RID: 522
	public static EasyPortal[] Portals;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	private BooleanConfigurable DefaultConfigurationConfigurable;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	private IntConfigurable TSPVersion;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	private Transform raycastTarget;

	// Token: 0x0400020E RID: 526
	[SerializeField]
	private GameObject[] portalGameObjects;

	// Token: 0x0400020F RID: 527
	[SerializeField]
	private EasyPortal[] portals;
}
