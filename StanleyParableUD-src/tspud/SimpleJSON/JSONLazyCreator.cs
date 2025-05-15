using System;

namespace SimpleJSON
{
	// Token: 0x02000235 RID: 565
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06000D19 RID: 3353 RVA: 0x0003BDFB File Offset: 0x00039FFB
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0003BE11 File Offset: 0x0003A011
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0003BE27 File Offset: 0x0003A027
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

		// Token: 0x17000125 RID: 293
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

		// Token: 0x17000126 RID: 294
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

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003BEA8 File Offset: 0x0003A0A8
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0003BECC File Offset: 0x0003A0CC
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass { { aKey, aItem } });
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0003BEEE File Offset: 0x0003A0EE
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0003BEF9 File Offset: 0x0003A0F9
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0003BEEE File Offset: 0x0003A0EE
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0003BF05 File Offset: 0x0003A105
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0003ACE7 File Offset: 0x00038EE7
		public override string ToString()
		{
			return "";
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0003ACE7 File Offset: 0x00038EE7
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x0003BF10 File Offset: 0x0003A110
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x0003BF2C File Offset: 0x0003A12C
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0003BF48 File Offset: 0x0003A148
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x0003BF6C File Offset: 0x0003A16C
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0003BF88 File Offset: 0x0003A188
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x0003BFB4 File Offset: 0x0003A1B4
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0003BFD0 File Offset: 0x0003A1D0
		// (set) Token: 0x06000D2F RID: 3375 RVA: 0x0003BFEC File Offset: 0x0003A1EC
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0003C008 File Offset: 0x0003A208
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0003C024 File Offset: 0x0003A224
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x04000C00 RID: 3072
		private JSONNode m_Node;

		// Token: 0x04000C01 RID: 3073
		private string m_Key;
	}
}
