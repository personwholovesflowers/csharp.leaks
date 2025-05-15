using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SimpleJSON
{
	// Token: 0x02000231 RID: 561
	public class JSONNode
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000110 RID: 272
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

		// Token: 0x17000111 RID: 273
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0003ACE7 File Offset: 0x00038EE7
		// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x00005444 File Offset: 0x00003644
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

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0001A562 File Offset: 0x00018762
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0003ACEE File Offset: 0x00038EEE
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00008FD9 File Offset: 0x000071D9
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00008FD9 File Offset: 0x000071D9
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0003ACFC File Offset: 0x00038EFC
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0003ACFF File Offset: 0x00038EFF
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003AD08 File Offset: 0x00038F08
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

		// Token: 0x06000CCF RID: 3279 RVA: 0x0003AD18 File Offset: 0x00038F18
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0003AD18 File Offset: 0x00038F18
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0003AD20 File Offset: 0x00038F20
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0003AD41 File Offset: 0x00038F41
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0003AD50 File Offset: 0x00038F50
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x0003AD79 File Offset: 0x00038F79
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0003AD88 File Offset: 0x00038F88
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x0003ADB9 File Offset: 0x00038FB9
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0003ADC8 File Offset: 0x00038FC8
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x0003ADF6 File Offset: 0x00038FF6
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

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0003AE0D File Offset: 0x0003900D
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0003AE15 File Offset: 0x00039015
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0003AE1D File Offset: 0x0003901D
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0003AE25 File Offset: 0x00039025
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003AE38 File Offset: 0x00039038
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0003AE4B File Offset: 0x0003904B
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0003AE57 File Offset: 0x00039057
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0003AE5D File Offset: 0x0003905D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0003AE68 File Offset: 0x00039068
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

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0003AF38 File Offset: 0x00039138
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

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0003B390 File Offset: 0x00039590
		public void SaveToStream(Stream aData)
		{
			BinaryWriter binaryWriter = new BinaryWriter(aData);
			this.Serialize(binaryWriter);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0003B3AB File Offset: 0x000395AB
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0003B3AB File Offset: 0x000395AB
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0003B3AB File Offset: 0x000395AB
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0003B3B8 File Offset: 0x000395B8
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0003B408 File Offset: 0x00039608
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

		// Token: 0x06000CEA RID: 3306 RVA: 0x0003B454 File Offset: 0x00039654
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

		// Token: 0x06000CEB RID: 3307 RVA: 0x0003B3AB File Offset: 0x000395AB
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0003B3AB File Offset: 0x000395AB
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0003B3AB File Offset: 0x000395AB
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0003B548 File Offset: 0x00039748
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jsonnode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jsonnode = JSONNode.Deserialize(binaryReader);
			}
			return jsonnode;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0003B580 File Offset: 0x00039780
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode jsonnode;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				jsonnode = JSONNode.LoadFromStream(fileStream);
			}
			return jsonnode;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003B5B8 File Offset: 0x000397B8
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
