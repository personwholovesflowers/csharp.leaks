using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012D RID: 301
[CreateAssetMenu(fileName = "TSP3BackgroundElementSet", menuName = "TSP3 Background ElementSet")]
public class TSP3BackgroundRandomSet : ScriptableObject
{
	// Token: 0x0600071A RID: 1818 RVA: 0x00025414 File Offset: 0x00023614
	public T RandomItem<T>(List<T> list, int seed = 14574572)
	{
		if (list == null || list.Count == 0)
		{
			return default(T);
		}
		Random.State state = Random.state;
		Random.InitState(seed);
		T t = list[Random.Range(0, list.Count)];
		Random.state = state;
		return t;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002545A File Offset: 0x0002365A
	public Texture2D GetLUTImage(int seed)
	{
		return this.RandomItem<Texture2D>(this.LUT, seed);
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00025469 File Offset: 0x00023669
	public Texture2D GetBackgroundImage(int seed)
	{
		return this.RandomItem<Texture2D>(this.backgroundImage, seed);
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00025478 File Offset: 0x00023678
	public IEnumerable<GameObject> GetAllToInstantiate(int seed)
	{
		yield return this.RandomItem<GameObject>(this.toInstantiate1, seed);
		yield return this.RandomItem<GameObject>(this.toInstantiate2, seed);
		yield return this.RandomItem<GameObject>(this.toInstantiate3, seed);
		yield break;
	}

	// Token: 0x04000755 RID: 1877
	[Header("Will pick **ONE** randomly from each list")]
	[SerializeField]
	private List<Texture2D> LUT;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	private List<Texture2D> backgroundImage;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	private List<GameObject> toInstantiate1;

	// Token: 0x04000758 RID: 1880
	[SerializeField]
	private List<GameObject> toInstantiate2;

	// Token: 0x04000759 RID: 1881
	[SerializeField]
	private List<GameObject> toInstantiate3;
}
