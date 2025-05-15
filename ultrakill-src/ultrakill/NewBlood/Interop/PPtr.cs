using System;
using UnityEngine;

namespace NewBlood.Interop
{
	// Token: 0x020005F5 RID: 1525
	public struct PPtr<T> : IEquatable<PPtr<T>>, IComparable<PPtr<T>>, IComparable where T : Object
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x0010B264 File Offset: 0x00109464
		public readonly bool IsValid
		{
			get
			{
				return Resources.InstanceIDIsValid(this.m_InstanceID);
			}
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0010B271 File Offset: 0x00109471
		public PPtr(T o)
		{
			this.m_InstanceID = ((o == null) ? 0 : o.GetInstanceID());
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0010B295 File Offset: 0x00109495
		public PPtr(int instanceID)
		{
			this.m_InstanceID = instanceID;
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0010B295 File Offset: 0x00109495
		public void SetInstanceID(int instanceID)
		{
			this.m_InstanceID = instanceID;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0010B29E File Offset: 0x0010949E
		public readonly int GetInstanceID()
		{
			return this.m_InstanceID;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0010B2A6 File Offset: 0x001094A6
		public readonly T ForceLoadPtr()
		{
			return (T)((object)Resources.InstanceIDToObject(this.m_InstanceID));
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0010B2B8 File Offset: 0x001094B8
		public override readonly bool Equals(object obj)
		{
			if (obj is PPtr<T>)
			{
				PPtr<T> pptr = (PPtr<T>)obj;
				return this.Equals(pptr);
			}
			return false;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0010B2DD File Offset: 0x001094DD
		public readonly bool Equals(PPtr<T> other)
		{
			return this.m_InstanceID == other.m_InstanceID;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0010B2F0 File Offset: 0x001094F0
		public readonly int CompareTo(object obj)
		{
			if (obj is PPtr<T>)
			{
				PPtr<T> pptr = (PPtr<T>)obj;
				return this.CompareTo(pptr);
			}
			return -1;
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x0010B317 File Offset: 0x00109517
		public readonly int CompareTo(PPtr<T> other)
		{
			return this.m_InstanceID.CompareTo(other.m_InstanceID);
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0010B29E File Offset: 0x0010949E
		public override readonly int GetHashCode()
		{
			return this.m_InstanceID;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0010B32A File Offset: 0x0010952A
		public override readonly string ToString()
		{
			return PPtr<T>.s_TypeString;
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0010B331 File Offset: 0x00109531
		public static implicit operator T(PPtr<T> p)
		{
			return p.ForceLoadPtr();
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0010B2DD File Offset: 0x001094DD
		public static bool operator ==(PPtr<T> p1, PPtr<T> p2)
		{
			return p1.m_InstanceID == p2.m_InstanceID;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0010B33A File Offset: 0x0010953A
		public static bool operator !=(PPtr<T> p1, PPtr<T> p2)
		{
			return p1.m_InstanceID != p2.m_InstanceID;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0010B34D File Offset: 0x0010954D
		public static bool operator <(PPtr<T> p1, PPtr<T> p2)
		{
			return p1.m_InstanceID < p2.m_InstanceID;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0010B35D File Offset: 0x0010955D
		public static bool operator >(PPtr<T> p1, PPtr<T> p2)
		{
			return p1.m_InstanceID > p2.m_InstanceID;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0010B36D File Offset: 0x0010956D
		public static bool operator <=(PPtr<T> p1, PPtr<T> p2)
		{
			return p1.m_InstanceID <= p2.m_InstanceID;
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0010B380 File Offset: 0x00109580
		public static bool operator >=(PPtr<T> p1, PPtr<T> p2)
		{
			return p1.m_InstanceID >= p2.m_InstanceID;
		}

		// Token: 0x04002DCB RID: 11723
		private int m_InstanceID;

		// Token: 0x04002DCC RID: 11724
		private static readonly string s_TypeString = "PPtr<" + typeof(T).Name + ">";
	}
}
