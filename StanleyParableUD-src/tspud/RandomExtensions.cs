using System;

// Token: 0x02000096 RID: 150
public static class RandomExtensions
{
	// Token: 0x060003A7 RID: 935 RVA: 0x00017EB4 File Offset: 0x000160B4
	public static void Shuffle<T>(this Random rng, T[] array)
	{
		int i = array.Length;
		while (i > 1)
		{
			int num = rng.Next(i--);
			T t = array[i];
			array[i] = array[num];
			array[num] = t;
		}
	}
}
