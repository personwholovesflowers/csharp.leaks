using System;

// Token: 0x02000022 RID: 34
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class AdditionalMapDetails : MonoSingleton<AdditionalMapDetails>
{
	// Token: 0x040000A5 RID: 165
	public bool hasAuthorLinks;

	// Token: 0x040000A6 RID: 166
	public AuthorLink[] authorLinks;
}
