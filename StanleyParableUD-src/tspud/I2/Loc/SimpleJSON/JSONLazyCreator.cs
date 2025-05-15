using System;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020002D9 RID: 729
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x00065F1B File Offset: 0x0006411B
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00065F31 File Offset: 0x00064131
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00065F47 File Offset: 0x00064147
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x170001AB RID: 427
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray { value });
			}
		}

		// Token: 0x170001AC RID: 428
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass { { aKey, value } });
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00065FC8 File Offset: 0x000641C8
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00065FEC File Offset: 0x000641EC
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass { { aKey, aItem } });
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0003BEEE File Offset: 0x0003A0EE
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0006600E File Offset: 0x0006420E
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0003BEEE File Offset: 0x0003A0EE
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0006601A File Offset: 0x0006421A
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0003ACE7 File Offset: 0x00038EE7
		public override string ToString()
		{
			return "";
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0003ACE7 File Offset: 0x00038EE7
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00066024 File Offset: 0x00064224
		// (set) Token: 0x060012FA RID: 4858 RVA: 0x00066040 File Offset: 0x00064240
		public override int AsInt
		{
			get
			{
				JSONData jsondata = new JSONData(0);
				this.Set(jsondata);
				return 0;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0006605C File Offset: 0x0006425C
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00066080 File Offset: 0x00064280
		public override float AsFloat
		{
			get
			{
				JSONData jsondata = new JSONData(0f);
				this.Set(jsondata);
				return 0f;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0006609C File Offset: 0x0006429C
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x000660C8 File Offset: 0x000642C8
		public override double AsDouble
		{
			get
			{
				JSONData jsondata = new JSONData(0.0);
				this.Set(jsondata);
				return 0.0;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x000660E4 File Offset: 0x000642E4
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x00066100 File Offset: 0x00064300
		public override bool AsBool
		{
			get
			{
				JSONData jsondata = new JSONData(false);
				this.Set(jsondata);
				return false;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0006611C File Offset: 0x0006431C
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x00066138 File Offset: 0x00064338
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x04000F06 RID: 3846
		private JSONNode m_Node;

		// Token: 0x04000F07 RID: 3847
		private string m_Key;
	}
}
