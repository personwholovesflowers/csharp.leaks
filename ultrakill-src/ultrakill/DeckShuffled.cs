using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000177 RID: 375
internal class DeckShuffled<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x06000738 RID: 1848 RVA: 0x0002F04D File Offset: 0x0002D24D
	public DeckShuffled(IEnumerable<T> target)
	{
		this.current = DeckShuffled<T>.Randomize(target).ToList<T>();
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0002F068 File Offset: 0x0002D268
	public void Reshuffle()
	{
		if (this.current.Count <= 1)
		{
			return;
		}
		IEnumerable<T> enumerable = this.current.Take(Mathf.FloorToInt((float)(this.current.Count / 2)));
		IEnumerable<T> enumerable2 = this.current.Skip(Mathf.FloorToInt((float)(this.current.Count / 2)));
		this.current = DeckShuffled<T>.Randomize(enumerable).Concat(DeckShuffled<T>.Randomize(enumerable2)).ToList<T>();
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0002F0DE File Offset: 0x0002D2DE
	private static IEnumerable<T> Randomize(IEnumerable<T> source)
	{
		T[] arr = source.ToArray<T>();
		int num;
		for (int i = arr.Length - 1; i > 0; i = num - 1)
		{
			int swapIndex = Random.Range(0, i + 1);
			yield return arr[swapIndex];
			arr[swapIndex] = arr[i];
			num = i;
		}
		yield return arr[0];
		yield break;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002F0EE File Offset: 0x0002D2EE
	public IEnumerator<T> GetEnumerator()
	{
		return this.current.GetEnumerator();
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x0002F0EE File Offset: 0x0002D2EE
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.current.GetEnumerator();
	}

	// Token: 0x04000946 RID: 2374
	private List<T> current;
}
