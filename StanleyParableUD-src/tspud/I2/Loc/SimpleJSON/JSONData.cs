using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020002D8 RID: 728
	public class JSONData : JSONNode
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00065DAC File Offset: 0x00063FAC
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x00065DB4 File Offset: 0x00063FB4
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

		// Token: 0x060012E2 RID: 4834 RVA: 0x00065DBD File Offset: 0x00063FBD
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00065DCC File Offset: 0x00063FCC
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00065DDB File Offset: 0x00063FDB
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00065DEA File Offset: 0x00063FEA
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00065DF9 File Offset: 0x00063FF9
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00065E08 File Offset: 0x00064008
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00065E08 File Offset: 0x00064008
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00065E24 File Offset: 0x00064024
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

		// Token: 0x04000F05 RID: 3845
		private string m_Data;
	}
}
