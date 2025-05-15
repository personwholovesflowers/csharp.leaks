using System;
using System.Collections;
using System.Collections.Generic;
using Nest.Components;
using Nest.Util;
using UnityEngine;

namespace Nest.Integrations
{
	// Token: 0x02000241 RID: 577
	public class Collection : BaseIntegration
	{
		// Token: 0x1700013B RID: 315
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x0003D13B File Offset: 0x0003B33B
		public override float InputValue
		{
			set
			{
				if (this.Type != Collection.CollectionType.None)
				{
					Debug.LogWarning("Called InputValue, but value will not be used!!");
				}
				if (this.SelectNewProperty(value))
				{
					this.InvokeSelected();
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003D15E File Offset: 0x0003B35E
		public Collection.Item Selected
		{
			get
			{
				if (this._selectedIndex < this.Items.Length && this._selectedIndex >= 0)
				{
					return this.Items[this._selectedIndex];
				}
				return null;
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0003D188 File Offset: 0x0003B388
		public void Start()
		{
			this.@switch = new Dictionary<Type, Action>
			{
				{
					typeof(AudioClip),
					new Action(this.InvokeAudio)
				},
				{
					typeof(NestInput),
					new Action(this.InvokeNest)
				}
			};
			if (this.Type == Collection.CollectionType.ShuffleBag)
			{
				this._shuffledItems = new ShuffleBag<Collection.Item>(this.Items);
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0003D1F2 File Offset: 0x0003B3F2
		public void Invoke()
		{
			this.SelectNewProperty((float)this._selectedIndex);
			this.InvokeSelected();
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0003D208 File Offset: 0x0003B408
		public void Invoke(int value)
		{
			this.SelectNewProperty((float)value);
			this.InvokeSelected();
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0003D21C File Offset: 0x0003B41C
		private bool SelectNewProperty(float value)
		{
			switch (this.Type)
			{
			case Collection.CollectionType.None:
			{
				int num = Mathf.FloorToInt(value);
				if (this._selectedIndex == num)
				{
					return false;
				}
				this._selectedIndex = num;
				return true;
			}
			case Collection.CollectionType.Sequence:
				this._selectedIndex = int.MinValue;
				return true;
			case Collection.CollectionType.ShuffleBag:
				this._selectedIndex = this._shuffledItems.NextIndex();
				return true;
			case Collection.CollectionType.DefinedRandom:
				this._selectedIndex = this.RandomWeighted(new int?(Mathf.FloorToInt(value * 100f)));
				return true;
			case Collection.CollectionType.UnityRandom:
				this._selectedIndex = this.RandomWeighted(null);
				return true;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0003D2C4 File Offset: 0x0003B4C4
		private void InvokeSelected()
		{
			if (-2147483648 == this._selectedIndex)
			{
				if (!this._sequenceRunning)
				{
					base.StartCoroutine(this.InvokeSequence());
				}
				return;
			}
			try
			{
				if (this.Selected != null && !(this.Selected.Object == null))
				{
					this.@switch[this.Selected.Object.GetType()]();
				}
			}
			catch (KeyNotFoundException ex)
			{
				Debug.LogError(string.Format("Collection doesn't support {0} at index {1}... {2}", this.Selected.Object.GetType(), this._selectedIndex, ex));
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0003D374 File Offset: 0x0003B574
		private void InvokeAudio()
		{
			if (this.Source == null && (this.Source = Camera.main.GetComponent<AudioSource>()) == null)
			{
				this.Source = Camera.main.gameObject.AddComponent<AudioSource>();
			}
			this.Source.clip = this.Selected.Object as AudioClip;
			this.Source.Play();
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0003D3E5 File Offset: 0x0003B5E5
		private void InvokeNest()
		{
			((NestInput)this.Selected.Object).Invoke();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0003D3FC File Offset: 0x0003B5FC
		private IEnumerator InvokeSequence()
		{
			if (this._sequenceRunning)
			{
				yield break;
			}
			this._sequenceRunning = true;
			int num;
			for (int i = 0; i < this.Items.Length; i = num + 1)
			{
				Collection.Item item = this.Items[i];
				this._selectedIndex = i;
				if (item.Object != null)
				{
					this.@switch[item.Object.GetType()]();
				}
				yield return new WaitForGameSeconds(item.Duration);
				num = i;
			}
			this._sequenceRunning = false;
			yield break;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003D40C File Offset: 0x0003B60C
		private int RandomWeighted(int? value)
		{
			bool flag = value != null;
			int num = 0;
			int num2 = value ?? Random.Range(0, 101);
			int i;
			for (i = 0; i < this.Items.Length; i++)
			{
				num += Mathf.FloorToInt(this.Items[i].Weight);
				if (num >= num2)
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x04000C1C RID: 3100
		public float RepeatInvokeTime;

		// Token: 0x04000C1D RID: 3101
		public Collection.CollectionType Type = Collection.CollectionType.DefinedRandom;

		// Token: 0x04000C1E RID: 3102
		public Collection.Item[] Items;

		// Token: 0x04000C1F RID: 3103
		public AudioSource Source;

		// Token: 0x04000C20 RID: 3104
		private Dictionary<Type, Action> @switch;

		// Token: 0x04000C21 RID: 3105
		[SerializeField]
		private int _selectedIndex;

		// Token: 0x04000C22 RID: 3106
		private bool _sequenceRunning;

		// Token: 0x04000C23 RID: 3107
		private ShuffleBag<Collection.Item> _shuffledItems;

		// Token: 0x04000C24 RID: 3108
		private const int _sequenceSelected = -2147483648;

		// Token: 0x02000432 RID: 1074
		public enum CollectionType
		{
			// Token: 0x040015A2 RID: 5538
			None,
			// Token: 0x040015A3 RID: 5539
			Sequence,
			// Token: 0x040015A4 RID: 5540
			ShuffleBag,
			// Token: 0x040015A5 RID: 5541
			DefinedRandom,
			// Token: 0x040015A6 RID: 5542
			UnityRandom
		}

		// Token: 0x02000433 RID: 1075
		[Serializable]
		public class Item
		{
			// Token: 0x040015A7 RID: 5543
			public float Weight = 50f;

			// Token: 0x040015A8 RID: 5544
			public float Duration = 1f;

			// Token: 0x040015A9 RID: 5545
			public Object Object;
		}
	}
}
