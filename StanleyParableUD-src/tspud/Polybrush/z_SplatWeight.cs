using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x0200022B RID: 555
	public class z_SplatWeight
	{
		// Token: 0x1700010D RID: 269
		public Vector4 this[z_MeshChannel channel]
		{
			get
			{
				return this.GetVec4(this.map[channel]);
			}
			set
			{
				this.SetVec4(this.map[channel], value);
			}
		}

		// Token: 0x1700010E RID: 270
		public float this[z_AttributeLayout attribute]
		{
			get
			{
				return this.GetAttributeValue(attribute);
			}
			set
			{
				this.SetAttributeValue(attribute, value);
			}
		}

		// Token: 0x1700010F RID: 271
		public float this[int valueIndex]
		{
			get
			{
				return this.values[valueIndex];
			}
			set
			{
				this.values[valueIndex] = value;
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000380A8 File Offset: 0x000362A8
		public z_SplatWeight(Dictionary<z_MeshChannel, int> map)
		{
			this.map = new Dictionary<z_MeshChannel, int>();
			foreach (KeyValuePair<z_MeshChannel, int> keyValuePair in map)
			{
				this.map.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.values = new float[map.Count * 4];
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0003812C File Offset: 0x0003632C
		public z_SplatWeight(z_SplatWeight rhs)
		{
			this.map = new Dictionary<z_MeshChannel, int>();
			foreach (KeyValuePair<z_MeshChannel, int> keyValuePair in rhs.map)
			{
				this.map.Add(keyValuePair.Key, keyValuePair.Value);
			}
			int num = rhs.values.Length;
			this.values = new float[num];
			Array.Copy(rhs.values, this.values, num);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000381CC File Offset: 0x000363CC
		public static Dictionary<z_MeshChannel, int> GetChannelMap(z_AttributeLayout[] attributes)
		{
			int num = 0;
			Dictionary<z_MeshChannel, int> dictionary = new Dictionary<z_MeshChannel, int>();
			foreach (z_MeshChannel z_MeshChannel in attributes.Select((z_AttributeLayout x) => x.channel).Distinct<z_MeshChannel>())
			{
				dictionary.Add(z_MeshChannel, num++);
			}
			return dictionary;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0003824C File Offset: 0x0003644C
		public List<int> GetAffectedIndicesWithMask(z_AttributeLayout[] attributes, int mask)
		{
			List<int> list = new List<int>();
			foreach (z_AttributeLayout z_AttributeLayout in attributes)
			{
				if (z_AttributeLayout.mask == mask)
				{
					list.Add((int)(this.map[z_AttributeLayout.channel] * 4 + z_AttributeLayout.index));
				}
			}
			return list;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000382A0 File Offset: 0x000364A0
		public bool MatchesAttributes(z_AttributeLayout[] attributes)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				if (!this.map.ContainsKey(attributes[i].channel))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x000382D3 File Offset: 0x000364D3
		private Vector4 GetVec4(int index)
		{
			return new Vector4(this.values[index * 4], this.values[index * 4 + 1], this.values[index * 4 + 2], this.values[index * 4 + 3]);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00038308 File Offset: 0x00036508
		private void SetVec4(int index, Vector4 value)
		{
			this.values[index * 4] = value.x;
			this.values[index * 4 + 1] = value.y;
			this.values[index * 4 + 2] = value.z;
			this.values[index * 4 + 3] = value.w;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003835B File Offset: 0x0003655B
		public float GetAttributeValue(z_AttributeLayout attrib)
		{
			return this.values[(int)(this.map[attrib.channel] * 4 + attrib.index)];
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003837E File Offset: 0x0003657E
		public void SetAttributeValue(z_AttributeLayout attrib, float value)
		{
			this.values[(int)(this.map[attrib.channel] * 4 + attrib.index)] = value;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000383A4 File Offset: 0x000365A4
		public void CopyTo(z_SplatWeight other)
		{
			for (int i = 0; i < this.values.Length; i++)
			{
				other.values[i] = this.values[i];
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000383D4 File Offset: 0x000365D4
		public void Lerp(z_SplatWeight lhs, z_SplatWeight rhs, float alpha)
		{
			int num = this.values.Length;
			if (num == 4)
			{
				this.values[0] = Mathf.LerpUnclamped(lhs.values[0], rhs.values[0], alpha);
				this.values[1] = Mathf.LerpUnclamped(lhs.values[1], rhs.values[1], alpha);
				this.values[2] = Mathf.LerpUnclamped(lhs.values[2], rhs.values[2], alpha);
				this.values[3] = Mathf.LerpUnclamped(lhs.values[3], rhs.values[3], alpha);
				return;
			}
			if (num == 8)
			{
				this.values[0] = Mathf.LerpUnclamped(lhs.values[0], rhs.values[0], alpha);
				this.values[1] = Mathf.LerpUnclamped(lhs.values[1], rhs.values[1], alpha);
				this.values[2] = Mathf.LerpUnclamped(lhs.values[2], rhs.values[2], alpha);
				this.values[3] = Mathf.LerpUnclamped(lhs.values[3], rhs.values[3], alpha);
				this.values[4] = Mathf.LerpUnclamped(lhs.values[4], rhs.values[4], alpha);
				this.values[5] = Mathf.LerpUnclamped(lhs.values[5], rhs.values[5], alpha);
				this.values[6] = Mathf.LerpUnclamped(lhs.values[6], rhs.values[6], alpha);
				this.values[7] = Mathf.LerpUnclamped(lhs.values[7], rhs.values[7], alpha);
				return;
			}
			if (num == 16)
			{
				this.values[0] = Mathf.LerpUnclamped(lhs.values[0], rhs.values[0], alpha);
				this.values[1] = Mathf.LerpUnclamped(lhs.values[1], rhs.values[1], alpha);
				this.values[2] = Mathf.LerpUnclamped(lhs.values[2], rhs.values[2], alpha);
				this.values[3] = Mathf.LerpUnclamped(lhs.values[3], rhs.values[3], alpha);
				this.values[4] = Mathf.LerpUnclamped(lhs.values[4], rhs.values[4], alpha);
				this.values[5] = Mathf.LerpUnclamped(lhs.values[5], rhs.values[5], alpha);
				this.values[6] = Mathf.LerpUnclamped(lhs.values[6], rhs.values[6], alpha);
				this.values[7] = Mathf.LerpUnclamped(lhs.values[7], rhs.values[7], alpha);
				this.values[8] = Mathf.LerpUnclamped(lhs.values[8], rhs.values[8], alpha);
				this.values[9] = Mathf.LerpUnclamped(lhs.values[9], rhs.values[9], alpha);
				this.values[10] = Mathf.LerpUnclamped(lhs.values[10], rhs.values[10], alpha);
				this.values[11] = Mathf.LerpUnclamped(lhs.values[11], rhs.values[11], alpha);
				this.values[12] = Mathf.LerpUnclamped(lhs.values[12], rhs.values[12], alpha);
				this.values[13] = Mathf.LerpUnclamped(lhs.values[13], rhs.values[13], alpha);
				this.values[14] = Mathf.LerpUnclamped(lhs.values[14], rhs.values[14], alpha);
				this.values[15] = Mathf.LerpUnclamped(lhs.values[15], rhs.values[15], alpha);
				return;
			}
			for (int i = 0; i < lhs.values.Length; i++)
			{
				this.values[i] = Mathf.LerpUnclamped(lhs.values[i], rhs.values[i], alpha);
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00038790 File Offset: 0x00036990
		public void Lerp(z_SplatWeight lhs, z_SplatWeight rhs, float alpha, List<int> mask)
		{
			if (mask.Count == 4)
			{
				this.values[mask[0]] = Mathf.LerpUnclamped(lhs.values[mask[0]], rhs.values[mask[0]], alpha);
				this.values[mask[1]] = Mathf.LerpUnclamped(lhs.values[mask[1]], rhs.values[mask[1]], alpha);
				this.values[mask[2]] = Mathf.LerpUnclamped(lhs.values[mask[2]], rhs.values[mask[2]], alpha);
				this.values[mask[3]] = Mathf.LerpUnclamped(lhs.values[mask[3]], rhs.values[mask[3]], alpha);
				return;
			}
			if (mask.Count == 8)
			{
				this.values[mask[0]] = Mathf.LerpUnclamped(lhs.values[mask[0]], rhs.values[mask[0]], alpha);
				this.values[mask[1]] = Mathf.LerpUnclamped(lhs.values[mask[1]], rhs.values[mask[1]], alpha);
				this.values[mask[2]] = Mathf.LerpUnclamped(lhs.values[mask[2]], rhs.values[mask[2]], alpha);
				this.values[mask[3]] = Mathf.LerpUnclamped(lhs.values[mask[3]], rhs.values[mask[3]], alpha);
				this.values[mask[4]] = Mathf.LerpUnclamped(lhs.values[mask[4]], rhs.values[mask[4]], alpha);
				this.values[mask[5]] = Mathf.LerpUnclamped(lhs.values[mask[5]], rhs.values[mask[5]], alpha);
				this.values[mask[6]] = Mathf.LerpUnclamped(lhs.values[mask[6]], rhs.values[mask[6]], alpha);
				this.values[mask[7]] = Mathf.LerpUnclamped(lhs.values[mask[7]], rhs.values[mask[7]], alpha);
				return;
			}
			for (int i = 0; i < mask.Count; i++)
			{
				this.values[mask[i]] = Mathf.LerpUnclamped(lhs.values[mask[i]], rhs.values[mask[i]], alpha);
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00038A64 File Offset: 0x00036C64
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<z_MeshChannel, int> keyValuePair in this.map)
			{
				stringBuilder.Append(keyValuePair.Key.ToString());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(this.GetVec4(keyValuePair.Value).ToString("F2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000BEB RID: 3051
		private Dictionary<z_MeshChannel, int> map;

		// Token: 0x04000BEC RID: 3052
		private float[] values;
	}
}
