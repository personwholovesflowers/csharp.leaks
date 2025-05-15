using System;
using System.IO;

namespace SimpleJSON
{
	// Token: 0x02000234 RID: 564
	public class JSONData : JSONNode
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0003BC8B File Offset: 0x00039E8B
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0003BC93 File Offset: 0x00039E93
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0003BC9C File Offset: 0x00039E9C
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0003BCAB File Offset: 0x00039EAB
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0003BCBA File Offset: 0x00039EBA
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0003BCC9 File Offset: 0x00039EC9
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0003BCD8 File Offset: 0x00039ED8
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0003BCE7 File Offset: 0x00039EE7
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0003BCE7 File Offset: 0x00039EE7
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0003BD04 File Offset: 0x00039F04
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData("");
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x04000BFF RID: 3071
		private string m_Data;
	}
}
