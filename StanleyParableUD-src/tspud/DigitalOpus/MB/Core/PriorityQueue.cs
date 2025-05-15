using System;
using System.Collections;
using System.Collections.Generic;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000280 RID: 640
	public class PriorityQueue<TPriority, TValue> : ICollection<KeyValuePair<TPriority, TValue>>, IEnumerable<KeyValuePair<TPriority, TValue>>, IEnumerable
	{
		// Token: 0x06000FC1 RID: 4033 RVA: 0x0005113A File Offset: 0x0004F33A
		public PriorityQueue()
			: this(Comparer<TPriority>.Default)
		{
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00051147 File Offset: 0x0004F347
		public PriorityQueue(int capacity)
			: this(capacity, Comparer<TPriority>.Default)
		{
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00051155 File Offset: 0x0004F355
		public PriorityQueue(int capacity, IComparer<TPriority> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(capacity);
			this._comparer = comparer;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00051179 File Offset: 0x0004F379
		public PriorityQueue(IComparer<TPriority> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>();
			this._comparer = comparer;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0005119C File Offset: 0x0004F39C
		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data)
			: this(data, Comparer<TPriority>.Default)
		{
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000511AC File Offset: 0x0004F3AC
		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data, IComparer<TPriority> comparer)
		{
			if (data == null || comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._comparer = comparer;
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(data);
			for (int i = this._baseHeap.Count / 2 - 1; i >= 0; i--)
			{
				this.HeapifyFromBeginningToEnd(i);
			}
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000511FF File Offset: 0x0004F3FF
		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2)
		{
			if (pq1 == null || pq2 == null)
			{
				throw new ArgumentNullException();
			}
			if (pq1._comparer != pq2._comparer)
			{
				throw new InvalidOperationException("Priority queues to be merged must have equal comparers");
			}
			return PriorityQueue<TPriority, TValue>.MergeQueues(pq1, pq2, pq1._comparer);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00051234 File Offset: 0x0004F434
		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2, IComparer<TPriority> comparer)
		{
			if (pq1 == null || pq2 == null || comparer == null)
			{
				throw new ArgumentNullException();
			}
			PriorityQueue<TPriority, TValue> priorityQueue = new PriorityQueue<TPriority, TValue>(pq1.Count + pq2.Count, pq1._comparer);
			priorityQueue._baseHeap.AddRange(pq1._baseHeap);
			priorityQueue._baseHeap.AddRange(pq2._baseHeap);
			for (int i = priorityQueue._baseHeap.Count / 2 - 1; i >= 0; i--)
			{
				priorityQueue.HeapifyFromBeginningToEnd(i);
			}
			return priorityQueue;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x000512AD File Offset: 0x0004F4AD
		public void Enqueue(TPriority priority, TValue value)
		{
			this.Insert(priority, value);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x000512B7 File Offset: 0x0004F4B7
		public KeyValuePair<TPriority, TValue> Dequeue()
		{
			if (!this.IsEmpty)
			{
				KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[0];
				this.DeleteRoot();
				return keyValuePair;
			}
			throw new InvalidOperationException("Priority queue is empty");
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x000512E0 File Offset: 0x0004F4E0
		public TValue DequeueValue()
		{
			return this.Dequeue().Value;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000512FB File Offset: 0x0004F4FB
		public KeyValuePair<TPriority, TValue> Peek()
		{
			if (!this.IsEmpty)
			{
				return this._baseHeap[0];
			}
			throw new InvalidOperationException("Priority queue is empty");
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0005131C File Offset: 0x0004F51C
		public TValue PeekValue()
		{
			return this.Peek().Value;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00051337 File Offset: 0x0004F537
		public bool IsEmpty
		{
			get
			{
				return this._baseHeap.Count == 0;
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00051348 File Offset: 0x0004F548
		private void ExchangeElements(int pos1, int pos2)
		{
			KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[pos1];
			this._baseHeap[pos1] = this._baseHeap[pos2];
			this._baseHeap[pos2] = keyValuePair;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00051388 File Offset: 0x0004F588
		private void Insert(TPriority priority, TValue value)
		{
			KeyValuePair<TPriority, TValue> keyValuePair = new KeyValuePair<TPriority, TValue>(priority, value);
			this._baseHeap.Add(keyValuePair);
			this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x000513C0 File Offset: 0x0004F5C0
		private int HeapifyFromEndToBeginning(int pos)
		{
			if (pos >= this._baseHeap.Count)
			{
				return -1;
			}
			while (pos > 0)
			{
				int num = (pos - 1) / 2;
				if (this._comparer.Compare(this._baseHeap[num].Key, this._baseHeap[pos].Key) <= 0)
				{
					break;
				}
				this.ExchangeElements(num, pos);
				pos = num;
			}
			return pos;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0005142C File Offset: 0x0004F62C
		private void DeleteRoot()
		{
			if (this._baseHeap.Count <= 1)
			{
				this._baseHeap.Clear();
				return;
			}
			this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
			this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
			this.HeapifyFromBeginningToEnd(0);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00051498 File Offset: 0x0004F698
		private void HeapifyFromBeginningToEnd(int pos)
		{
			if (pos >= this._baseHeap.Count)
			{
				return;
			}
			for (;;)
			{
				int num = pos;
				int num2 = 2 * pos + 1;
				int num3 = 2 * pos + 2;
				if (num2 < this._baseHeap.Count && this._comparer.Compare(this._baseHeap[num].Key, this._baseHeap[num2].Key) > 0)
				{
					num = num2;
				}
				if (num3 < this._baseHeap.Count && this._comparer.Compare(this._baseHeap[num].Key, this._baseHeap[num3].Key) > 0)
				{
					num = num3;
				}
				if (num == pos)
				{
					break;
				}
				this.ExchangeElements(num, pos);
				pos = num;
			}
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00051562 File Offset: 0x0004F762
		public void Add(KeyValuePair<TPriority, TValue> item)
		{
			this.Enqueue(item.Key, item.Value);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00051578 File Offset: 0x0004F778
		public void Clear()
		{
			this._baseHeap.Clear();
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00051585 File Offset: 0x0004F785
		public bool Contains(KeyValuePair<TPriority, TValue> item)
		{
			return this._baseHeap.Contains(item);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00051594 File Offset: 0x0004F794
		public bool TryFindValue(TPriority item, out TValue foundVersion)
		{
			for (int i = 0; i < this._baseHeap.Count; i++)
			{
				if (this._comparer.Compare(item, this._baseHeap[i].Key) == 0)
				{
					foundVersion = this._baseHeap[i].Value;
					return true;
				}
			}
			foundVersion = default(TValue);
			return false;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x000515FD File Offset: 0x0004F7FD
		public int Count
		{
			get
			{
				return this._baseHeap.Count;
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0005160A File Offset: 0x0004F80A
		public void CopyTo(KeyValuePair<TPriority, TValue>[] array, int arrayIndex)
		{
			this._baseHeap.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0001A562 File Offset: 0x00018762
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0005161C File Offset: 0x0004F81C
		public bool Remove(KeyValuePair<TPriority, TValue> item)
		{
			int num = this._baseHeap.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this._baseHeap[num] = this._baseHeap[this._baseHeap.Count - 1];
			this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
			if (this.HeapifyFromEndToBeginning(num) == num)
			{
				this.HeapifyFromBeginningToEnd(num);
			}
			return true;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0005168A File Offset: 0x0004F88A
		public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
		{
			return this._baseHeap.GetEnumerator();
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0005169C File Offset: 0x0004F89C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000DA8 RID: 3496
		public List<KeyValuePair<TPriority, TValue>> _baseHeap;

		// Token: 0x04000DA9 RID: 3497
		private IComparer<TPriority> _comparer;
	}
}
