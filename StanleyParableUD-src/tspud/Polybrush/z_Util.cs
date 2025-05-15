using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x0200022E RID: 558
	public static class z_Util
	{
		// Token: 0x06000CAB RID: 3243 RVA: 0x0003A548 File Offset: 0x00038748
		public static T[] Fill<T>(T value, int count)
		{
			T[] array = new T[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = value;
			}
			return array;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0003A574 File Offset: 0x00038774
		public static T[] Fill<T>(Func<int, T> constructor, int count)
		{
			T[] array = new T[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = constructor(i);
			}
			return array;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0003A5A4 File Offset: 0x000387A4
		public static T[] Duplicate<T>(T[] array)
		{
			if (array == null)
			{
				return null;
			}
			T[] array2 = new T[array.Length];
			Array.Copy(array, 0, array2, 0, array.Length);
			return array2;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0003A5CC File Offset: 0x000387CC
		public static Dictionary<K, V> InitDictionary<K, V>(Func<int, K> keyFunc, Func<int, V> valueFunc, int count)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>(count);
			for (int i = 0; i < count; i++)
			{
				dictionary.Add(keyFunc(i), valueFunc(i));
			}
			return dictionary;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0003A604 File Offset: 0x00038804
		public static string ToString<T>(this IEnumerable<T> enumerable, string delim)
		{
			if (enumerable == null)
			{
				return "";
			}
			return string.Join(delim ?? "", enumerable.Select(delegate(T x)
			{
				if (x == null)
				{
					return "";
				}
				return x.ToString();
			}).ToArray<string>());
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0003A654 File Offset: 0x00038854
		public static string ToString<K, V>(this Dictionary<K, V> dictionary, string delim)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> keyValuePair in dictionary)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				K key = keyValuePair.Key;
				string text = key.ToString();
				string text2 = ": ";
				V value = keyValuePair.Value;
				stringBuilder2.AppendLine(text + text2 + value.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003A6E4 File Offset: 0x000388E4
		public static void Resize<T>(ref T[] array, int newSize)
		{
			T[] array2 = new T[newSize];
			Array.Copy(array, array2, Math.Min(array.Length, newSize));
			array = array2;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0003A710 File Offset: 0x00038910
		public static Dictionary<K, T> SetValuesAsKey<T, K>(this Dictionary<T, IEnumerable<K>> dic)
		{
			Dictionary<K, T> dictionary = new Dictionary<K, T>();
			foreach (KeyValuePair<T, IEnumerable<K>> keyValuePair in dic)
			{
				foreach (K k in keyValuePair.Value)
				{
					dictionary.Add(k, keyValuePair.Key);
				}
			}
			return dictionary;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0003A7A4 File Offset: 0x000389A4
		public static Dictionary<T, int> GetCommonLookup<T>(this List<List<T>> lists)
		{
			Dictionary<T, int> dictionary = new Dictionary<T, int>();
			int num = 0;
			foreach (List<T> list in lists)
			{
				foreach (T t in list)
				{
					dictionary.Add(t, num);
				}
				num++;
			}
			return dictionary;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003A838 File Offset: 0x00038A38
		public static Color32 Lerp(Color32 lhs, Color32 rhs, z_ColorMask mask, float alpha)
		{
			return new Color32(mask.r ? ((byte)((float)lhs.r * (1f - alpha) + (float)rhs.r * alpha)) : lhs.r, mask.g ? ((byte)((float)lhs.g * (1f - alpha) + (float)rhs.g * alpha)) : lhs.g, mask.b ? ((byte)((float)lhs.b * (1f - alpha) + (float)rhs.b * alpha)) : lhs.b, mask.a ? ((byte)((float)lhs.a * (1f - alpha) + (float)rhs.a * alpha)) : lhs.a);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0003A8F4 File Offset: 0x00038AF4
		public static Color32 Lerp(Color32 lhs, Color32 rhs, float alpha)
		{
			return new Color32((byte)((float)lhs.r * (1f - alpha) + (float)rhs.r * alpha), (byte)((float)lhs.g * (1f - alpha) + (float)rhs.g * alpha), (byte)((float)lhs.b * (1f - alpha) + (float)rhs.b * alpha), (byte)((float)lhs.a * (1f - alpha) + (float)rhs.a * alpha));
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0003A970 File Offset: 0x00038B70
		public static AnimationCurve ClampAnimationKeys(AnimationCurve curve, float firstKeyTime, float firstKeyValue, float secondKeyTime, float secondKeyValue)
		{
			Keyframe[] keys = curve.keys;
			int num = curve.length - 1;
			keys[0].time = firstKeyTime;
			keys[0].value = firstKeyValue;
			keys[num].time = secondKeyTime;
			keys[num].value = secondKeyValue;
			curve.keys = keys;
			return new AnimationCurve(keys);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0003A9D0 File Offset: 0x00038BD0
		public static Enum Next(this Enum value)
		{
			int num = Enum.GetNames(value.GetType()).Length;
			return (Enum)Enum.ToObject(value.GetType(), (Convert.ToInt32(value) + 1) % num);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0003AA05 File Offset: 0x00038C05
		public static bool IsValid<T>(this T target) where T : z_IValid
		{
			return target != null && target.IsValid;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0003AA20 File Offset: 0x00038C20
		internal static string IncrementPrefix(string prefix, string name)
		{
			Match match = new Regex("^(" + prefix + "[0-9]*_)").Match(name);
			string text2;
			if (match.Success)
			{
				string text = match.Value.Replace(prefix, "").Replace("_", "");
				int num = 0;
				if (int.TryParse(text, out num))
				{
					text2 = name.Replace(match.Value, prefix + (num + 1) + "_");
				}
				else
				{
					text2 = prefix + "0_" + name;
				}
			}
			else
			{
				text2 = prefix + "0_" + name;
			}
			return text2;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0003AAC0 File Offset: 0x00038CC0
		public static Mesh GetMesh(this GameObject gameObject)
		{
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				return component.sharedMesh;
			}
			SkinnedMeshRenderer component2 = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null && component2.sharedMesh != null)
			{
				return component2.sharedMesh;
			}
			return null;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003AB18 File Offset: 0x00038D18
		public static List<Material> GetMaterials(this GameObject gameObject)
		{
			List<Material> list = new List<Material>();
			foreach (Renderer renderer in gameObject.GetComponents<Renderer>())
			{
				list.AddRange(renderer.sharedMaterials);
			}
			return list;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003AB54 File Offset: 0x00038D54
		public static Dictionary<T, List<K>> ToDictionary<T, K>(this IEnumerable<IGrouping<T, K>> groups)
		{
			Dictionary<T, List<K>> dictionary = new Dictionary<T, List<K>>();
			foreach (IGrouping<T, K> grouping in groups)
			{
				dictionary.Add(grouping.Key, grouping.ToList<K>());
			}
			return dictionary;
		}
	}
}
