using System;
using System.Linq;
using UnityEngine;

namespace Nest.Components
{
	// Token: 0x0200024B RID: 587
	[AddComponentMenu("Nest/Components/Trigger Volume")]
	[RequireComponent(typeof(Collider))]
	public class TriggerVolume : NestInput
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0003E564 File Offset: 0x0003C764
		public bool IsEnterAndExit
		{
			get
			{
				return this.TriggerEvent == (TriggerVolume.TriggerType.Enter | TriggerVolume.TriggerType.Exit);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0003E56F File Offset: 0x0003C76F
		public bool IsEnterAndStay
		{
			get
			{
				return this.TriggerEvent == (TriggerVolume.TriggerType.Enter | TriggerVolume.TriggerType.Stay);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0003E57A File Offset: 0x0003C77A
		public bool IsExitAndStay
		{
			get
			{
				return this.TriggerEvent == (TriggerVolume.TriggerType.Stay | TriggerVolume.TriggerType.Exit);
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0003E585 File Offset: 0x0003C785
		public void OnTriggerEnter(Collider other)
		{
			if (!this._tagValues.Contains(other.tag))
			{
				return;
			}
			if (this.IsEnterAndExit || this.IsEnterAndStay)
			{
				this.SetValue(true);
			}
			this.Invoke(TriggerVolume.TriggerType.Enter);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0003E5B9 File Offset: 0x0003C7B9
		public void OnTriggerExit(Collider other)
		{
			if (!this._tagValues.Contains(other.tag))
			{
				return;
			}
			if (this.IsEnterAndExit)
			{
				this.SetValue(false);
			}
			this.Invoke(TriggerVolume.TriggerType.Exit);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0003E5E5 File Offset: 0x0003C7E5
		public void Invoke(TriggerVolume.TriggerType type)
		{
			if ((this.TriggerEvent & type) == (TriggerVolume.TriggerType)0)
			{
				return;
			}
			base.Invoke();
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003E5F8 File Offset: 0x0003C7F8
		public void SetValue(bool value)
		{
			base.SetBool(value);
		}

		// Token: 0x04000C73 RID: 3187
		public TriggerVolume.TriggerType TriggerEvent = TriggerVolume.TriggerType.Enter;

		// Token: 0x04000C74 RID: 3188
		[SerializeField]
		public int TagMask = -1;

		// Token: 0x04000C75 RID: 3189
		[SerializeField]
		private string[] _tagValues;

		// Token: 0x02000442 RID: 1090
		[Flags]
		public enum TriggerType
		{
			// Token: 0x040015E0 RID: 5600
			Enter = 1,
			// Token: 0x040015E1 RID: 5601
			Stay = 2,
			// Token: 0x040015E2 RID: 5602
			Exit = 4
		}
	}
}
