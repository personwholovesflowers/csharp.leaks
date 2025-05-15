using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nest.Util
{
	// Token: 0x0200023C RID: 572
	[Serializable]
	public class ShuffleBag<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>
	{
		// Token: 0x06000D72 RID: 3442 RVA: 0x0003CC0C File Offset: 0x0003AE0C
		public T Next()
		{
			if (this.cursor >= 1)
			{
				int num = Mathf.FloorToInt(Random.value * (float)(this.cursor + 1));
				T t = this.data[num];
				this.data[num] = this.data[this.cursor];
				this.data[this.cursor] = t;
				this.cursor--;
				return t;
			}
			this.cursor = this.data.Count - 1;
			if (this.data.Count < 1)
			{
				return default(T);
			}
			return this.data[0];
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0003CCBC File Offset: 0x0003AEBC
		public int NextIndex()
		{
			if (this.cursor < 1)
			{
				this.cursor = this.data.Count - 1;
				return 0;
			}
			int num = Mathf.FloorToInt(Random.value * (float)(this.cursor + 1));
			T t = this.data[num];
			this.data[num] = this.data[this.cursor];
			this.data[this.cursor] = t;
			this.cursor--;
			return num;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0003CD48 File Offset: 0x0003AF48
		public ShuffleBag(T[] initalValues)
		{
			for (int i = 0; i < initalValues.Length; i++)
			{
				this.Add(initalValues[i]);
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0003CD81 File Offset: 0x0003AF81
		public ShuffleBag()
		{
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0003CD94 File Offset: 0x0003AF94
		public int IndexOf(T item)
		{
			return this.data.IndexOf(item);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0003CDA2 File Offset: 0x0003AFA2
		public void Insert(int index, T item)
		{
			this.cursor = this.data.Count;
			this.data.Insert(index, item);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0003CDC2 File Offset: 0x0003AFC2
		public void RemoveAt(int index)
		{
			this.cursor = this.data.Count - 2;
			this.data.RemoveAt(index);
		}

		// Token: 0x17000135 RID: 309
		public T this[int index]
		{
			get
			{
				return this.data[index];
			}
			set
			{
				this.data[index] = value;
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0003CE00 File Offset: 0x0003B000
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003CE12 File Offset: 0x0003B012
		public void Add(T item)
		{
			this.data.Add(item);
			this.cursor = this.data.Count - 1;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0003CE33 File Offset: 0x0003B033
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003CE40 File Offset: 0x0003B040
		public void Clear()
		{
			this.data.Clear();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0003CE4D File Offset: 0x0003B04D
		public bool Contains(T item)
		{
			return this.data.Contains(item);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003CE5C File Offset: 0x0003B05C
		public void CopyTo(T[] array, int arrayIndex)
		{
			foreach (T t in this.data)
			{
				array.SetValue(t, arrayIndex);
				arrayIndex++;
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003CEBC File Offset: 0x0003B0BC
		public bool Remove(T item)
		{
			this.cursor = this.data.Count - 2;
			return this.data.Remove(item);
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0001A562 File Offset: 0x00018762
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0003CE00 File Offset: 0x0003B000
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x04000C0C RID: 3084
		private List<T> data = new List<T>();

		// Token: 0x04000C0D RID: 3085
		private int cursor;

		// Token: 0x04000C0E RID: 3086
		private T last;
	}
}
