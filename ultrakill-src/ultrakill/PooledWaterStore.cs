using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000351 RID: 849
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PooledWaterStore : MonoSingleton<PooledWaterStore>
{
	// Token: 0x060013A9 RID: 5033 RVA: 0x0009D1FB File Offset: 0x0009B3FB
	private void Start()
	{
		this.thisTrans = base.transform;
		this.InitPools();
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x0009D210 File Offset: 0x0009B410
	private void InitPools()
	{
		DefaultReferenceManager instance = MonoSingleton<DefaultReferenceManager>.Instance;
		this.continuousSplash = instance.continuousSplash;
		this.bigSplash = instance.splash;
		this.smallSplash = instance.smallSplash;
		this.bubblePrefab = instance.bubbles;
		this.wetParticle = instance.wetParticle;
		base.StartCoroutine(this.InitPool(Water.WaterGOType.small));
		base.StartCoroutine(this.InitPool(Water.WaterGOType.big));
		base.StartCoroutine(this.InitPool(Water.WaterGOType.continuous));
		base.StartCoroutine(this.InitPool(Water.WaterGOType.bubble));
		base.StartCoroutine(this.InitPool(Water.WaterGOType.wetparticle));
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x0009D2A8 File Offset: 0x0009B4A8
	private GameObject GetPrefabByWaterType(Water.WaterGOType waterType)
	{
		switch (waterType)
		{
		case Water.WaterGOType.small:
			return this.smallSplash;
		case Water.WaterGOType.big:
			return this.bigSplash;
		case Water.WaterGOType.continuous:
			return this.continuousSplash;
		case Water.WaterGOType.bubble:
			return this.bubblePrefab;
		case Water.WaterGOType.wetparticle:
			return this.wetParticle;
		default:
			return null;
		}
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x0009D2F7 File Offset: 0x0009B4F7
	private IEnumerator InitPool(Water.WaterGOType type)
	{
		Queue<GameObject> queue = new Queue<GameObject>();
		this.waterGOQueues.Add(type, queue);
		GameObject prefabByWaterType = this.GetPrefabByWaterType(type);
		prefabByWaterType.SetActive(false);
		AsyncInstantiateOperation<GameObject> asyncOp = Object.InstantiateAsync<GameObject>(prefabByWaterType, 50, this.thisTrans);
		while (!asyncOp.isDone)
		{
			yield return null;
		}
		GameObject[] result = asyncOp.Result;
		for (int i = 0; i < 50; i++)
		{
			GameObject gameObject = result[i];
			gameObject.SetActive(false);
			queue.Enqueue(gameObject);
		}
		yield break;
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x0009D310 File Offset: 0x0009B510
	public GameObject GetFromQueue(Water.WaterGOType type)
	{
		GameObject gameObject = null;
		Queue<GameObject> queue = this.waterGOQueues[type];
		while (gameObject == null && queue.Count > 0)
		{
			gameObject = queue.Dequeue();
		}
		if (gameObject == null)
		{
			gameObject = Object.Instantiate<GameObject>(this.GetPrefabByWaterType(type), this.thisTrans);
		}
		if (gameObject == null)
		{
			return null;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x0009D378 File Offset: 0x0009B578
	public void ReturnToQueue(GameObject go, Water.WaterGOType type)
	{
		if (type == Water.WaterGOType.none)
		{
			Object.Destroy(go);
			return;
		}
		this.waterGOQueues[type].Enqueue(go);
		if (type == Water.WaterGOType.bubble || type == Water.WaterGOType.wetparticle)
		{
			go.transform.SetParent(this.thisTrans);
		}
		go.transform.localScale = Vector3.one;
		go.SetActive(false);
	}

	// Token: 0x04001AF7 RID: 6903
	private GameObject smallSplash;

	// Token: 0x04001AF8 RID: 6904
	private GameObject bigSplash;

	// Token: 0x04001AF9 RID: 6905
	private GameObject continuousSplash;

	// Token: 0x04001AFA RID: 6906
	private GameObject bubblePrefab;

	// Token: 0x04001AFB RID: 6907
	private GameObject wetParticle;

	// Token: 0x04001AFC RID: 6908
	private Dictionary<Water.WaterGOType, Queue<GameObject>> waterGOQueues = new Dictionary<Water.WaterGOType, Queue<GameObject>>();

	// Token: 0x04001AFD RID: 6909
	private Transform thisTrans;
}
