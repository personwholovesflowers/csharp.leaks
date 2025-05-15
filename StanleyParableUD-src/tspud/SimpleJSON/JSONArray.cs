using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SimpleJSON
{
	// Token: 0x02000232 RID: 562
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x1700011C RID: 284
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

		// Token: 0x1700011D RID: 285
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

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x0003B63D File Offset: 0x0003983D
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0003B62F File Offset: 0x0003982F
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0003B64A File Offset: 0x0003984A
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

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0003B678 File Offset: 0x00039878
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0003B688 File Offset: 0x00039888
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

		// Token: 0x06000CFB RID: 3323 RVA: 0x0003B698 File Offset: 0x00039898
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

		// Token: 0x06000CFC RID: 3324 RVA: 0x0003B6A8 File Offset: 0x000398A8
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

		// Token: 0x06000CFD RID: 3325 RVA: 0x0003B72C File Offset: 0x0003992C
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

		// Token: 0x06000CFE RID: 3326 RVA: 0x0003B7D0 File Offset: 0x000399D0
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x04000BFD RID: 3069
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
