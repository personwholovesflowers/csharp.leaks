using System;
using I2.Loc;

// Token: 0x02000182 RID: 386
public static class SequelTools
{
	// Token: 0x06000907 RID: 2311 RVA: 0x0002AF80 File Offset: 0x00029180
	public static string PrefixTerm(int i)
	{
		return string.Format("{0}_{1:00}", SequelTools.PrefixTermBase, i);
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0002AF97 File Offset: 0x00029197
	public static string PostfixTerm(int i)
	{
		return string.Format("{0}_{1:00}", SequelTools.PostfixTermBase, i);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0002AFAE File Offset: 0x000291AE
	public static string PrefixLocalizedText(int i)
	{
		return LocalizationManager.GetTranslation(SequelTools.PrefixTerm(i), true, 0, true, false, null, null);
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0002AFC1 File Offset: 0x000291C1
	public static string PostfixLocalizedText(int i)
	{
		return LocalizationManager.GetTranslation(SequelTools.PostfixTerm(i), true, 0, true, false, null, null);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0002AFD4 File Offset: 0x000291D4
	public static string DoStandardSequelReplacementStep(string originalText, IntConfigurable sequelCountConfigurable, IntConfigurable prefixIndexConfigurable, IntConfigurable postfixIndexConfigurable)
	{
		int intValue = prefixIndexConfigurable.GetIntValue();
		string text = "";
		if (intValue != -1)
		{
			text = SequelTools.PrefixLocalizedText(intValue);
		}
		int intValue2 = postfixIndexConfigurable.GetIntValue();
		string text2 = "";
		if (intValue2 != -1)
		{
			text2 = SequelTools.PostfixLocalizedText(intValue2);
		}
		return originalText.Replace("\\n", "\n").Replace("%!N!%", sequelCountConfigurable.GetIntValue().ToString()).Replace("%!P!%", text)
			.Replace("%!S!%", text2);
	}

	// Token: 0x040008E6 RID: 2278
	public static string PrefixTermBase = "General_Title_Gag_prefix";

	// Token: 0x040008E7 RID: 2279
	public static string PostfixTermBase = "General_Title_Gag_postfix";
}
