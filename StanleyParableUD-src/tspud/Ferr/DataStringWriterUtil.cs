using System;
using System.Collections.Generic;
using System.Text;

namespace Ferr
{
	// Token: 0x020002E3 RID: 739
	public class DataStringWriterUtil
	{
		// Token: 0x06001337 RID: 4919 RVA: 0x00066827 File Offset: 0x00064A27
		public DataStringWriterUtil(DataStringType aType)
		{
			this._type = aType;
			this._builder = new StringBuilder();
			this._builder.Append('{');
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0006685A File Offset: 0x00064A5A
		public void Int(int aData)
		{
			this.Entry(aData.ToString());
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00066869 File Offset: 0x00064A69
		public void Int(string aName, int aData)
		{
			this.Entry(aName, aData.ToString());
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00066879 File Offset: 0x00064A79
		public void Long(long aData)
		{
			this.Entry(aData.ToString());
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00066888 File Offset: 0x00064A88
		public void Long(string aName, long aData)
		{
			this.Entry(aName, aData.ToString());
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00066898 File Offset: 0x00064A98
		public void Bool(bool aData)
		{
			this.Entry(aData.ToString());
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000668A7 File Offset: 0x00064AA7
		public void Bool(string aName, bool aData)
		{
			this.Entry(aName, aData.ToString());
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000668B7 File Offset: 0x00064AB7
		public void Float(float aData)
		{
			this.Entry(aData.ToString());
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x000668C6 File Offset: 0x00064AC6
		public void Float(string aName, float aData)
		{
			this.Entry(aName, aData.ToString());
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x000668D6 File Offset: 0x00064AD6
		public void Data(IToFromDataString aData)
		{
			if (aData == null)
			{
				this.Entry("null");
				return;
			}
			this.Entry(aData.GetType().Name + "=" + aData.ToDataString());
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00066908 File Offset: 0x00064B08
		public void Data(string aName, IToFromDataString aData)
		{
			if (aData == null)
			{
				this.Entry(aName, "null");
				return;
			}
			this.Entry(aName, aData.GetType().Name + "=" + aData.ToDataString());
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0006693C File Offset: 0x00064B3C
		public void String(string aData)
		{
			char quoteType = this.GetQuoteType(aData);
			if (quoteType != ' ')
			{
				this.Entry(quoteType.ToString() + aData + quoteType.ToString());
				return;
			}
			this.Entry(aData);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00066978 File Offset: 0x00064B78
		public void String(string aName, string aData)
		{
			char quoteType = this.GetQuoteType(aData);
			if (quoteType != ' ')
			{
				this.Entry(aName, quoteType.ToString() + aData + quoteType.ToString());
				return;
			}
			this.Entry(aName, aData);
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000669B8 File Offset: 0x00064BB8
		protected char GetQuoteType(string aData)
		{
			char c = ' ';
			if (!aData.StartsWith("{") && !aData.StartsWith("\"") && !aData.StartsWith("'"))
			{
				bool flag = aData.Contains("'");
				bool flag2 = aData.Contains("\"");
				if (flag && flag2)
				{
					throw new ArgumentException("String data contains -both- single and double quotes, what am I supposed to do with this?");
				}
				c = (flag ? '"' : '\'');
			}
			return c;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00066A20 File Offset: 0x00064C20
		protected void Entry(string aData)
		{
			if (this._type == DataStringType.Named)
			{
				throw new Exception("Need a name for a named list!");
			}
			if (this._builder.Length > 1)
			{
				this._builder.Append(',');
			}
			this._builder.Append(aData);
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00066A60 File Offset: 0x00064C60
		protected void Entry(string aName, string aData)
		{
			if (this._type == DataStringType.Ordered)
			{
				throw new Exception("Name doesn't apply for ordered lists!");
			}
			if (aName.Contains(":") || aName.Contains(","))
			{
				throw new Exception("Name includes a reserved character! (: or ,) - " + aName);
			}
			if (this._names.Contains(aName))
			{
				throw new Exception("Used the same name twice: " + aName);
			}
			this._names.Add(aName);
			if (this._builder.Length > 1)
			{
				this._builder.Append(',');
			}
			this._builder.Append(aName);
			this._builder.Append(":");
			this._builder.Append(aData);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00066B1E File Offset: 0x00064D1E
		public override string ToString()
		{
			return this._builder.ToString() + "}";
		}

		// Token: 0x04000F14 RID: 3860
		private DataStringType _type;

		// Token: 0x04000F15 RID: 3861
		private StringBuilder _builder;

		// Token: 0x04000F16 RID: 3862
		private HashSet<string> _names = new HashSet<string>();
	}
}
