using System;

namespace I2.Loc
{
	// Token: 0x020002C7 RID: 711
	public class RTLFixer
	{
		// Token: 0x06001265 RID: 4709 RVA: 0x00063381 File Offset: 0x00061581
		public static string Fix(string str)
		{
			return RTLFixer.Fix(str, false, true);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0006338C File Offset: 0x0006158C
		public static string Fix(string str, bool rtl)
		{
			if (rtl)
			{
				return RTLFixer.Fix(str);
			}
			string[] array = str.Split(new char[] { ' ' });
			string text = "";
			string text2 = "";
			foreach (string text3 in array)
			{
				if (char.IsLower(text3.ToLower()[text3.Length / 2]))
				{
					text = text + RTLFixer.Fix(text2) + text3 + " ";
					text2 = "";
				}
				else
				{
					text2 = text2 + text3 + " ";
				}
			}
			if (text2 != "")
			{
				text += RTLFixer.Fix(text2);
			}
			return text;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00063438 File Offset: 0x00061638
		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			string text = HindiFixer.Fix(str);
			if (text != str)
			{
				return text;
			}
			RTLFixerTool.showTashkeel = showTashkeel;
			RTLFixerTool.useHinduNumbers = useHinduNumbers;
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", Environment.NewLine);
			}
			if (!str.Contains(Environment.NewLine))
			{
				return RTLFixerTool.FixLine(str);
			}
			string[] array = new string[] { Environment.NewLine };
			string[] array2 = str.Split(array, StringSplitOptions.None);
			if (array2.Length == 0)
			{
				return RTLFixerTool.FixLine(str);
			}
			if (array2.Length == 1)
			{
				return RTLFixerTool.FixLine(str);
			}
			string text2 = RTLFixerTool.FixLine(array2[0]);
			int i = 1;
			if (array2.Length > 1)
			{
				while (i < array2.Length)
				{
					text2 = text2 + Environment.NewLine + RTLFixerTool.FixLine(array2[i]);
					i++;
				}
			}
			return text2;
		}
	}
}
