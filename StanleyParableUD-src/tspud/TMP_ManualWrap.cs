using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class TMP_ManualWrap : MonoBehaviour
{
	// Token: 0x06000A1A RID: 2586 RVA: 0x0002FAB0 File Offset: 0x0002DCB0
	private string GetWordWrappedText(TMP_TextInfo ti)
	{
		string text = ti.textComponent.text;
		TMP_ManualWrap.IntStringPair[] array;
		string text2 = this.ExtractTags(text, out array);
		int num = 0;
		foreach (TMP_LineInfo tmp_LineInfo in ti.lineInfo)
		{
			num += tmp_LineInfo.characterCount;
			text2 = text2.Insert(num, "\n");
			num++;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].i >= num)
				{
					TMP_ManualWrap.IntStringPair[] array2 = array;
					int num2 = j;
					array2[num2].i = array2[num2].i + 1;
				}
			}
			if (num >= text2.Length)
			{
				break;
			}
		}
		for (int k = array.Length - 1; k >= 0; k--)
		{
			text2 = text2.Insert(array[k].i, array[k].s);
		}
		return text2.Replace("\n\n", "\n");
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0002FB98 File Offset: 0x0002DD98
	private string ExtractTags(string raw, out TMP_ManualWrap.IntStringPair[] tagsArray)
	{
		List<TMP_ManualWrap.IntStringPair> list = new List<TMP_ManualWrap.IntStringPair>();
		for (int i = 0; i < 1000; i++)
		{
			int num = raw.IndexOf('<');
			int num2 = raw.IndexOf('>');
			if (num == -1 || num2 == -1 || num2 < num)
			{
				break;
			}
			int num3 = num2 - num;
			list.Add(new TMP_ManualWrap.IntStringPair
			{
				i = num,
				s = raw.Substring(num, num3 + 1)
			});
			raw = raw.Remove(num, num3 + 1);
		}
		tagsArray = list.ToArray();
		return raw;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0002FC1E File Offset: 0x0002DE1E
	private void Start()
	{
		this.tmp.OnPreRenderText += this.Tmp_OnPreRenderText;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0002FC37 File Offset: 0x0002DE37
	private void Tmp_OnPreRenderText(TMP_TextInfo textInfo)
	{
		if (this.inOnPreRender)
		{
			return;
		}
		this.inOnPreRender = true;
		this.tmp.text = this.GetWordWrappedText(textInfo);
		this.inOnPreRender = false;
	}

	// Token: 0x04000A0D RID: 2573
	public TMP_Text tmp;

	// Token: 0x04000A0E RID: 2574
	private bool inOnPreRender;

	// Token: 0x020003FC RID: 1020
	private struct IntStringPair
	{
		// Token: 0x040014CF RID: 5327
		public int i;

		// Token: 0x040014D0 RID: 5328
		public string s;
	}
}
