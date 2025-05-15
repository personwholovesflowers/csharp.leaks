using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020002D6 RID: 726
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x170001A2 RID: 418
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x170001A3 RID: 419
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00065759 File Offset: 0x00063959
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0006574B File Offset: 0x0006394B
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00065766 File Offset: 0x00063966
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode jsonnode = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return jsonnode;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00065794 File Offset: 0x00063994
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x000657A4 File Offset: 0x000639A4
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x000657B4 File Offset: 0x000639B4
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode jsonnode in this.m_List)
			{
				yield return jsonnode;
			}
			List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000657C4 File Offset: 0x000639C4
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00065848 File Offset: 0x00063A48
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000658EC File Offset: 0x00063AEC
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x04000F03 RID: 3843
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
