using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020002D5 RID: 725
	public class JSONNode
	{
		// Token: 0x06001292 RID: 4754 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000196 RID: 406
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000197 RID: 407
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0003ACE7 File Offset: 0x00038EE7
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x00005444 File Offset: 0x00003644
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0001A562 File Offset: 0x00018762
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00064E31 File Offset: 0x00063031
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00008FD9 File Offset: 0x000071D9
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00008FD9 File Offset: 0x000071D9
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0003ACFC File Offset: 0x00038EFC
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x00064E3F File Offset: 0x0006303F
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00064E48 File Offset: 0x00063048
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode jsonnode in this.Childs)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChilds)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0003AD18 File Offset: 0x00038F18
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0003AD18 File Offset: 0x00038F18
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x00064E58 File Offset: 0x00063058
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x00064E79 File Offset: 0x00063079
		public virtual int AsInt
		{
			get
			{
				int num = 0;
				if (int.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x00064E88 File Offset: 0x00063088
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x00064EB1 File Offset: 0x000630B1
		public virtual float AsFloat
		{
			get
			{
				float num = 0f;
				if (float.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x00064EC0 File Offset: 0x000630C0
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x00064EF1 File Offset: 0x000630F1
		public virtual double AsDouble
		{
			get
			{
				double num = 0.0;
				if (double.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x00064F00 File Offset: 0x00063100
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x00064F2E File Offset: 0x0006312E
		public virtual bool AsBool
		{
			get
			{
				bool flag = false;
				if (bool.TryParse(this.Value, out flag))
				{
					return flag;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x00064F45 File Offset: 0x00063145
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x00064F4D File Offset: 0x0006314D
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00064F55 File Offset: 0x00063155
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00064F5D File Offset: 0x0006315D
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00064F70 File Offset: 0x00063170
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00064F83 File Offset: 0x00063183
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0003AE57 File Offset: 0x00039057
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0003AE5D File Offset: 0x0003905D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00064F90 File Offset: 0x00063190
		internal static string Escape(string aText)
		{
			string text = "";
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\v':
					goto IL_00A3;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_00A3;
						}
						text += "\\\\";
					}
					else
					{
						text += "\\\"";
					}
					break;
				}
				IL_00B1:
				i++;
				continue;
				IL_00A3:
				text += c.ToString();
				goto IL_00B1;
			}
			return text;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00065060 File Offset: 0x00063260
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = "";
			string text2 = "";
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_0429;
						case '\v':
						case '\f':
							goto IL_0412;
						default:
							if (c != ' ')
							{
								goto IL_0412;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_0412;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
						else
						{
							if (text != "")
							{
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != "")
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = "";
							text = "";
						}
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									text += aJSON[i].ToString();
									goto IL_0429;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != "")
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = "";
								text = "";
								jsonnode = stack.Peek();
								goto IL_0429;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											text += "\b";
											goto IL_0429;
										}
										if (c2 == 'f')
										{
											text += "\f";
											goto IL_0429;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											text += "\n";
											goto IL_0429;
										}
										switch (c2)
										{
										case 'r':
											text += "\r";
											goto IL_0429;
										case 't':
											text += "\t";
											goto IL_0429;
										case 'u':
										{
											string text3 = aJSON.Substring(i + 1, 4);
											text += ((char)int.Parse(text3, NumberStyles.AllowHexSpecifier)).ToString();
											i += 4;
											goto IL_0429;
										}
										}
									}
									text += c2.ToString();
									goto IL_0429;
								}
								goto IL_0429;
							case ']':
								break;
							default:
								goto IL_0412;
							}
						}
						else
						{
							if (flag)
							{
								text += aJSON[i].ToString();
								goto IL_0429;
							}
							text2 = text;
							text = "";
							goto IL_0429;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_0412;
						}
					}
					else
					{
						if (flag)
						{
							text += aJSON[i].ToString();
							goto IL_0429;
						}
						stack.Push(new JSONClass());
						if (jsonnode != null)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, stack.Peek());
							}
						}
						text2 = "";
						text = "";
						jsonnode = stack.Peek();
						goto IL_0429;
					}
					if (flag)
					{
						text += aJSON[i].ToString();
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != "")
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(text);
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, text);
							}
						}
						text2 = "";
						text = "";
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_0429:
				i++;
				continue;
				IL_0412:
				text += aJSON[i].ToString();
				goto IL_0429;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x000654B8 File Offset: 0x000636B8
		public void SaveToStream(Stream aData)
		{
			BinaryWriter binaryWriter = new BinaryWriter(aData);
			this.Serialize(binaryWriter);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0003B3AB File Offset: 0x000395AB
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0003B3AB File Offset: 0x000395AB
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0003B3AB File Offset: 0x000395AB
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x000654D4 File Offset: 0x000636D4
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00065524 File Offset: 0x00063724
		public string SaveToBase64()
		{
			string text;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				text = Convert.ToBase64String(memoryStream.ToArray());
			}
			return text;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00065570 File Offset: 0x00063770
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string text = aReader.ReadString();
					JSONNode jsonnode = JSONNode.Deserialize(aReader);
					jsonclass.Add(text, jsonnode);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0003B3AB File Offset: 0x000395AB
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0003B3AB File Offset: 0x000395AB
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0003B3AB File Offset: 0x000395AB
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00065664 File Offset: 0x00063864
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jsonnode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jsonnode = JSONNode.Deserialize(binaryReader);
			}
			return jsonnode;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0006569C File Offset: 0x0006389C
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode jsonnode;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				jsonnode = JSONNode.LoadFromStream(fileStream);
			}
			return jsonnode;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x000656D4 File Offset: 0x000638D4
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
