using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class GoreZone : MonoBehaviour
{
	// Token: 0x06000BF0 RID: 3056 RVA: 0x00053D7C File Offset: 0x00051F7C
	public static GoreZone ResolveGoreZone(Transform transform)
	{
		if (!transform.parent)
		{
			if (GoreZone._globalRootAutomaticGz)
			{
				transform.SetParent(GoreZone._globalRootAutomaticGz.transform);
				return GoreZone._globalRootAutomaticGz;
			}
			GoreZone goreZone = new GameObject("Automated Gore Zone").AddComponent<GoreZone>();
			transform.SetParent(goreZone.transform);
			GoreZone._globalRootAutomaticGz = goreZone;
			return goreZone;
		}
		else
		{
			GoreZone componentInParent = transform.GetComponentInParent<GoreZone>();
			if (componentInParent)
			{
				return componentInParent;
			}
			GoreZone componentInChildren = transform.parent.GetComponentInChildren<GoreZone>();
			if (componentInChildren)
			{
				transform.SetParent(componentInChildren.transform);
				return componentInChildren;
			}
			GoreZone goreZone2 = new GameObject("Automated Gore Zone").AddComponent<GoreZone>();
			Transform transform2 = goreZone2.transform;
			transform2.SetParent(transform.parent);
			transform.SetParent(transform2);
			return goreZone2;
		}
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00053E38 File Offset: 0x00052038
	private void Awake()
	{
		if (this.goreZone == null)
		{
			GameObject gameObject = new GameObject("Gore Zone");
			this.goreZone = gameObject.transform;
			this.goreZone.SetParent(base.transform, true);
		}
		if (this.gibZone == null)
		{
			GameObject gameObject2 = new GameObject("Gib Zone");
			this.gibZone = gameObject2.transform;
			this.gibZone.SetParent(base.transform, true);
		}
		this.stains = base.gameObject.GetOrAddComponent<BloodstainParent>();
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00053EC4 File Offset: 0x000520C4
	private void Start()
	{
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		this.maxGore = MonoSingleton<OptionsManager>.Instance.maxGore;
		this.endlessMode = MonoSingleton<EndlessGrid>.Instance != null;
		if (this.endlessMode)
		{
			this.maxGibs = Mathf.RoundToInt(this.maxGore / 40f);
		}
		else
		{
			this.maxGibs = Mathf.RoundToInt(this.maxGore / 20f);
		}
		this.SlowUpdate();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x00053F5B File Offset: 0x0005215B
	private void OnDestroy()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x00053F7D File Offset: 0x0005217D
	private void OnPrefChanged(string key, object value)
	{
		if (key == "maxGore")
		{
			this.UpdateMaxGore((float)value);
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00053F98 File Offset: 0x00052198
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 1f);
		if (this.bsm.forceGibs)
		{
			this.maxGore = 3000f;
			this.maxGibs = 150;
		}
		for (int i = Mathf.FloorToInt((float)this.goreZone.childCount - this.maxGore) - 1; i >= 0; i--)
		{
			Transform child = this.goreZone.GetChild(i);
			GameObject gameObject = child.gameObject;
			Bloodsplatter bloodsplatter;
			if (gameObject.activeSelf && !child.TryGetComponent<Bloodsplatter>(out bloodsplatter) && gameObject.layer != 1)
			{
				Object.Destroy(child.gameObject);
			}
		}
		for (int j = Mathf.FloorToInt((float)(this.gibZone.childCount - this.maxGibs)) - 1; j >= 0; j--)
		{
			GameObject gameObject2 = this.gibZone.GetChild(j).gameObject;
			if (!gameObject2.GetComponentInChildren<Bloodsplatter>())
			{
				Object.Destroy(gameObject2);
			}
		}
		for (int k = Mathf.FloorToInt((float)this.outsideGore.Count - this.maxGore / 5f) - 1; k >= 0; k--)
		{
			GameObject gameObject3 = this.outsideGore[k];
			if (gameObject3 != null)
			{
				if (!gameObject3.GetComponentInChildren<Bloodsplatter>())
				{
					Object.Destroy(gameObject3);
					this.outsideGore.RemoveAt(k);
				}
			}
			else
			{
				this.outsideGore.RemoveAt(k);
			}
		}
		if (this.toDestroy.Count > 0)
		{
			base.StartCoroutine(this.DestroyNextFrame());
		}
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0005411D File Offset: 0x0005231D
	private IEnumerator DestroyNextFrame()
	{
		yield return null;
		for (int i = this.toDestroy.Count - 1; i >= 0; i--)
		{
			Object.Destroy(this.toDestroy[i]);
		}
		this.toDestroy.Clear();
		yield return null;
		yield break;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0005412C File Offset: 0x0005232C
	public void SetGoreZone(GameObject gib)
	{
		gib.transform.SetParent(this.gibZone, true);
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00054140 File Offset: 0x00052340
	private void Update()
	{
		if (this.goreRenderDistance != 0f)
		{
			this.CheckRenderDistance();
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00054158 File Offset: 0x00052358
	private void CheckRenderDistance()
	{
		if (Vector3.Distance(MonoSingleton<CameraController>.Instance.transform.position, base.transform.position) > this.goreRenderDistance)
		{
			if (!this.goreUnrendered)
			{
				this.goreUnrendered = true;
				this.goreZone.gameObject.SetActive(false);
				this.gibZone.gameObject.SetActive(false);
				return;
			}
		}
		else if (this.goreUnrendered)
		{
			this.goreUnrendered = false;
			this.goreZone.gameObject.SetActive(true);
			this.gibZone.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x000541EF File Offset: 0x000523EF
	public void Combine()
	{
		StaticBatchingUtility.Combine(this.goreZone.gameObject);
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x00054201 File Offset: 0x00052401
	public void AddDeath()
	{
		this.checkpoint.restartKills++;
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00054216 File Offset: 0x00052416
	public void AddKillHitterTarget(int id)
	{
		if (this.checkpoint && !this.checkpoint.succesfulHitters.Contains(id))
		{
			this.checkpoint.succesfulHitters.Add(id);
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0005424C File Offset: 0x0005244C
	public void ResetGibs()
	{
		for (int i = this.gibZone.childCount - 1; i > 0; i--)
		{
			Transform child = this.gibZone.GetChild(i);
			Bloodsplatter bloodsplatter;
			GoreSplatter goreSplatter;
			if (child.TryGetComponent<Bloodsplatter>(out bloodsplatter))
			{
				bloodsplatter.Repool();
			}
			else if (child.TryGetComponent<GoreSplatter>(out goreSplatter))
			{
				goreSplatter.Repool();
			}
			else
			{
				Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x000542AD File Offset: 0x000524AD
	public void ResetBlood()
	{
		this.stains.ClearChildren();
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x000542BA File Offset: 0x000524BA
	public void UpdateMaxGore(float amount)
	{
		this.maxGore = amount;
		if (this.endlessMode)
		{
			this.maxGibs = Mathf.RoundToInt(this.maxGore / 40f);
			return;
		}
		this.maxGibs = Mathf.RoundToInt(this.maxGore / 20f);
	}

	// Token: 0x04000FAB RID: 4011
	[HideInInspector]
	public bool isNewest = true;

	// Token: 0x04000FAC RID: 4012
	[Header("Optional")]
	public Transform goreZone;

	// Token: 0x04000FAD RID: 4013
	public Transform gibZone;

	// Token: 0x04000FAE RID: 4014
	[HideInInspector]
	public CheckPoint checkpoint;

	// Token: 0x04000FAF RID: 4015
	[HideInInspector]
	public float maxGore;

	// Token: 0x04000FB0 RID: 4016
	[HideInInspector]
	public List<GameObject> outsideGore = new List<GameObject>();

	// Token: 0x04000FB1 RID: 4017
	private bool endlessMode;

	// Token: 0x04000FB2 RID: 4018
	private int maxGibs;

	// Token: 0x04000FB3 RID: 4019
	public float goreRenderDistance;

	// Token: 0x04000FB4 RID: 4020
	private bool goreUnrendered;

	// Token: 0x04000FB5 RID: 4021
	public List<GameObject> toDestroy = new List<GameObject>();

	// Token: 0x04000FB6 RID: 4022
	public Queue<Bloodsplatter> splatterQueue = new Queue<Bloodsplatter>();

	// Token: 0x04000FB7 RID: 4023
	public Queue<GameObject> stainQueue = new Queue<GameObject>();

	// Token: 0x04000FB8 RID: 4024
	private static GoreZone _globalRootAutomaticGz;

	// Token: 0x04000FB9 RID: 4025
	private BloodsplatterManager bsm;

	// Token: 0x04000FBA RID: 4026
	public BloodstainParent stains;
}
