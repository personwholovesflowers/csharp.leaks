using System;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class SequelTitleMessageBoxTextReplacer : MonoBehaviour, IMessageBoxKeyReplacer
{
	// Token: 0x06000905 RID: 2309 RVA: 0x0002AF66 File Offset: 0x00029166
	public string DoReplaceStep(string originalText)
	{
		return SequelTools.DoStandardSequelReplacementStep(originalText, this.sequelCountConfigurable, this.prefixIndexConfigurable, this.postfixIndexConfigurable);
	}

	// Token: 0x040008E3 RID: 2275
	public IntConfigurable sequelCountConfigurable;

	// Token: 0x040008E4 RID: 2276
	public IntConfigurable prefixIndexConfigurable;

	// Token: 0x040008E5 RID: 2277
	public IntConfigurable postfixIndexConfigurable;
}
