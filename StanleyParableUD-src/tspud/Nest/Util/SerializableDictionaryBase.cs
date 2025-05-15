using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nest.Util
{
	// Token: 0x0200023B RID: 571
	[Serializable]
	public class SerializableDictionaryBase<TKey, TValue> : DrawableDictionary, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, ISerializationCallbackReceiver
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x0003C8C9 File Offset: 0x0003AAC9
		public int Count
		{
			get
			{
				if (this._dict == null)
				{
					return 0;
				}
				return this._dict.Count;
			}
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003C8E0 File Offset: 0x0003AAE0
		public void Add(TKey key, TValue value)
		{
			if (this._dict == null)
			{
				this._dict = new Dictionary<TKey, TValue>();
			}
			this._dict.Add(key, value);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003C902 File Offset: 0x0003AB02
		public bool ContainsKey(TKey key)
		{
			return this._dict != null && this._dict.ContainsKey(key);
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x0003C91A File Offset: 0x0003AB1A
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>();
				}
				return this._dict.Keys;
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0003C93A File Offset: 0x0003AB3A
		public bool Remove(TKey key)
		{
			return this._dict != null && this._dict.Remove(key);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0003C952 File Offset: 0x0003AB52
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._dict == null)
			{
				value = default(TValue);
				return false;
			}
			return this._dict.TryGetValue(key, out value);
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x0003C972 File Offset: 0x0003AB72
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>();
				}
				return this._dict.Values;
			}
		}

		// Token: 0x17000133 RID: 307
		public TValue this[TKey key]
		{
			get
			{
				if (this._dict == null)
				{
					throw new KeyNotFoundException();
				}
				return this._dict[key];
			}
			set
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>();
				}
				this._dict[key] = value;
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0003C9D0 File Offset: 0x0003ABD0
		public void Clear()
		{
			if (this._dict != null)
			{
				this._dict.Clear();
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003C9E5 File Offset: 0x0003ABE5
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._dict == null)
			{
				this._dict = new Dictionary<TKey, TValue>();
			}
			((ICollection<KeyValuePair<TKey, TValue>>)this._dict).Add(item);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0003CA06 File Offset: 0x0003AC06
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return this._dict != null && ((ICollection<KeyValuePair<TKey, TValue>>)this._dict).Contains(item);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0003CA1E File Offset: 0x0003AC1E
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dict == null)
			{
				return;
			}
			((ICollection<KeyValuePair<TKey, TValue>>)this._dict).CopyTo(array, arrayIndex);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0003CA36 File Offset: 0x0003AC36
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return this._dict != null && ((ICollection<KeyValuePair<TKey, TValue>>)this._dict).Remove(item);
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0001A562 File Offset: 0x00018762
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0003CA50 File Offset: 0x0003AC50
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			if (this._dict == null)
			{
				return default(Dictionary<TKey, TValue>.Enumerator);
			}
			return this._dict.GetEnumerator();
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0003CA7A File Offset: 0x0003AC7A
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this._dict == null)
			{
				return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
			}
			return this._dict.GetEnumerator();
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003CA7A File Offset: 0x0003AC7A
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			if (this._dict == null)
			{
				return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
			}
			return this._dict.GetEnumerator();
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003CAA0 File Offset: 0x0003ACA0
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._keys != null && this._values != null)
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>(this._keys.Length);
				}
				else
				{
					this._dict.Clear();
				}
				for (int i = 0; i < this._keys.Length; i++)
				{
					if (i < this._values.Length)
					{
						this._dict[this._keys[i]] = this._values[i];
					}
					else
					{
						this._dict[this._keys[i]] = default(TValue);
					}
				}
			}
			this._keys = null;
			this._values = null;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0003CB5C File Offset: 0x0003AD5C
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._dict == null || this._dict.Count == 0)
			{
				this._keys = null;
				this._values = null;
				return;
			}
			int count = this._dict.Count;
			this._keys = new TKey[count];
			this._values = new TValue[count];
			int num = 0;
			Dictionary<TKey, TValue>.Enumerator enumerator = this._dict.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TKey[] keys = this._keys;
				int num2 = num;
				KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
				keys[num2] = keyValuePair.Key;
				TValue[] values = this._values;
				int num3 = num;
				keyValuePair = enumerator.Current;
				values[num3] = keyValuePair.Value;
				num++;
			}
		}

		// Token: 0x04000C09 RID: 3081
		[NonSerialized]
		private Dictionary<TKey, TValue> _dict;

		// Token: 0x04000C0A RID: 3082
		[SerializeField]
		private TKey[] _keys;

		// Token: 0x04000C0B RID: 3083
		[SerializeField]
		private TValue[] _values;
	}
}
