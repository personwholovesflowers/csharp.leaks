using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Randomness
{
	// Token: 0x02000575 RID: 1397
	public abstract class RandomBase<T> : MonoBehaviour where T : RandomEntry, new()
	{
		// Token: 0x06001FC6 RID: 8134 RVA: 0x00101EE8 File Offset: 0x001000E8
		private void OnEnable()
		{
			if (this.randomizeOnEnable)
			{
				this.Randomize();
			}
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00101EF8 File Offset: 0x001000F8
		public virtual void Randomize()
		{
			this.RandomizeWithCount(this.toBeEnabledCount);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00101F08 File Offset: 0x00100108
		public virtual void RandomizeWithCount(int count)
		{
			List<T> list = new List<T>(this.entries);
			T localLastPicked = this.lastPicked;
			int num = 0;
			Func<T, bool> <>9__0;
			while (num < count && list.Count > 0)
			{
				List<T> list2;
				if (this.ensureNoRepeats && localLastPicked != null && list.Count > 1)
				{
					IEnumerable<T> enumerable = list;
					Func<T, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (T e) => e != localLastPicked);
					}
					list2 = enumerable.Where(func).ToList<T>();
					if (list2.Count == 0)
					{
						list2 = list;
					}
				}
				else
				{
					list2 = list;
				}
				T t = RandomBase<T>.WeightedPick(list2);
				if (t == null)
				{
					break;
				}
				this.PerformTheAction(t);
				localLastPicked = t;
				list.Remove(t);
				num++;
			}
			this.lastPicked = localLastPicked;
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x00101FE4 File Offset: 0x001001E4
		public static T WeightedPick(List<T> pool)
		{
			int num = 0;
			foreach (T t in pool)
			{
				num += t.weight;
			}
			if (num <= 0)
			{
				return default(T);
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			foreach (T t2 in pool)
			{
				num3 += t2.weight;
				if (num2 < num3)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x06001FCA RID: 8138
		public abstract void PerformTheAction(RandomEntry entry);

		// Token: 0x06001FCB RID: 8139 RVA: 0x001020B4 File Offset: 0x001002B4
		private void OnValidate()
		{
			if (this.firstDeserialization)
			{
				this.arrayLength = this.entries.Length;
				this.firstDeserialization = false;
				return;
			}
			if (this.entries.Length == this.arrayLength)
			{
				return;
			}
			if (this.entries.Length > this.arrayLength)
			{
				for (int i = this.arrayLength; i < this.entries.Length; i++)
				{
					this.entries[i] = new T();
				}
			}
			this.arrayLength = this.entries.Length;
		}

		// Token: 0x04002C03 RID: 11267
		public bool randomizeOnEnable = true;

		// Token: 0x04002C04 RID: 11268
		public bool ensureNoRepeats = true;

		// Token: 0x04002C05 RID: 11269
		public int toBeEnabledCount = 1;

		// Token: 0x04002C06 RID: 11270
		public T[] entries;

		// Token: 0x04002C07 RID: 11271
		private T lastPicked;

		// Token: 0x04002C08 RID: 11272
		private bool firstDeserialization = true;

		// Token: 0x04002C09 RID: 11273
		private int arrayLength;
	}
}
