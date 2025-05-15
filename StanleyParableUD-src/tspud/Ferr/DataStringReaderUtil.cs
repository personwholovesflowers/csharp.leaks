using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002E4 RID: 740
	public class DataStringReaderUtil
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x00066B35 File Offset: 0x00064D35
		public int NameCount
		{
			get
			{
				return this._names.Length;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x00066B3F File Offset: 0x00064D3F
		public bool HasNext
		{
			get
			{
				return this._curr < this._words.Length;
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00066B54 File Offset: 0x00064D54
		public DataStringReaderUtil(string aData, DataStringType aType)
		{
			List<string> list = DataStringUtil.SplitSmart(aData, ',');
			if (list == null)
			{
				throw new ArgumentException("Poorly formed data string! Ensure sure quotes and brackets all match!");
			}
			this._type = aType;
			this._words = (string.IsNullOrEmpty(aData) ? new string[0] : list.ToArray());
			if (this._type == DataStringType.Named)
			{
				this._names = new string[this._words.Length];
				for (int i = 0; i < this._words.Length; i++)
				{
					int num = this._words[i].IndexOf(':');
					string text = this._words[i].Substring(0, num);
					string text2 = this._words[i].Substring(num + 1);
					this._words[i] = text2;
					this._names[i] = text;
				}
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00066C15 File Offset: 0x00064E15
		public string GetName(int aIndex)
		{
			return this._names[aIndex];
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00066C1F File Offset: 0x00064E1F
		public int Int()
		{
			return int.Parse(this.Read());
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00066C2C File Offset: 0x00064E2C
		public int Int(string aName)
		{
			return int.Parse(this.Read(aName));
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00066C3A File Offset: 0x00064E3A
		public long Long()
		{
			return long.Parse(this.Read());
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00066C47 File Offset: 0x00064E47
		public long Long(string aName)
		{
			return long.Parse(this.Read(aName));
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00066C55 File Offset: 0x00064E55
		public bool Bool()
		{
			return bool.Parse(this.Read());
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00066C62 File Offset: 0x00064E62
		public bool Bool(string aName)
		{
			return bool.Parse(this.Read(aName));
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00066C70 File Offset: 0x00064E70
		public float Float()
		{
			return float.Parse(this.Read());
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00066C7D File Offset: 0x00064E7D
		public float Float(string aName)
		{
			return float.Parse(this.Read(aName));
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00066C8B File Offset: 0x00064E8B
		public string String()
		{
			return this.Read();
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00066C93 File Offset: 0x00064E93
		public string String(string aName)
		{
			return this.Read(aName);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00066C9C File Offset: 0x00064E9C
		public object Data()
		{
			return this.CreateObject(this.Read());
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00066CAA File Offset: 0x00064EAA
		public object Data(string aName)
		{
			return this.CreateObject(this.Read(aName));
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00066CBC File Offset: 0x00064EBC
		public void Data(ref IToFromDataString aBaseObject)
		{
			string text = this.Read();
			string text2 = text.Substring(text.IndexOf('=') + 1);
			aBaseObject.FromDataString(text2);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00066CE8 File Offset: 0x00064EE8
		public void Data(string aName, ref IToFromDataString aBaseObject)
		{
			string text = this.Read(aName);
			string text2 = text.Substring(text.IndexOf('=') + 1);
			aBaseObject.FromDataString(text2);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00066D14 File Offset: 0x00064F14
		private string Read(string aName)
		{
			if (this._type == DataStringType.Ordered)
			{
				throw new Exception("Can't do a named read from an ordered list!");
			}
			int num = Array.IndexOf<string>(this._names, aName);
			if (num == -1)
			{
				throw new Exception("Can't find data from given name: " + aName);
			}
			return this._words[num];
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00066D60 File Offset: 0x00064F60
		private string Read()
		{
			if (this._type == DataStringType.Named)
			{
				throw new Exception("Can't do an ordered read from a named list!");
			}
			if (this._curr >= this._words.Length)
			{
				throw new Exception("Reading past the end of an ordered data string!");
			}
			string text = this._words[this._curr];
			this._curr++;
			return text;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00066DB8 File Offset: 0x00064FB8
		private object CreateObject(string aDataString)
		{
			if (string.IsNullOrEmpty(aDataString) || aDataString == "null")
			{
				return null;
			}
			int num = aDataString.IndexOf('=');
			string text = aDataString.Substring(0, num);
			string text2 = aDataString.Substring(num + 1);
			Type type = Type.GetType(text);
			object obj = null;
			if (typeof(IToFromDataString).IsAssignableFrom(type))
			{
				if (typeof(ScriptableObject).IsAssignableFrom(type))
				{
					obj = ScriptableObject.CreateInstance(type);
				}
				else
				{
					obj = Activator.CreateInstance(type);
				}
				((IToFromDataString)obj).FromDataString(text2);
			}
			return obj;
		}

		// Token: 0x04000F17 RID: 3863
		private DataStringType _type;

		// Token: 0x04000F18 RID: 3864
		private string[] _words;

		// Token: 0x04000F19 RID: 3865
		private string[] _names;

		// Token: 0x04000F1A RID: 3866
		private int _curr;
	}
}
