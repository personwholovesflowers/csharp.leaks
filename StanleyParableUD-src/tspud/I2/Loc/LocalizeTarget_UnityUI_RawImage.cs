using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020002B9 RID: 697
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		// Token: 0x0600121C RID: 4636 RVA: 0x000623FA File Offset: 0x000605FA
		static LocalizeTarget_UnityUI_RawImage()
		{
			LocalizeTarget_UnityUI_RawImage.AutoRegister();
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00062401 File Offset: 0x00060601
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>
			{
				Name = "RawImage",
				Priority = 100
			});
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00062420 File Offset: 0x00060620
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00062423 File Offset: 0x00060623
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			secondaryTerm = null;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00062458 File Offset: 0x00060658
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Texture texture = this.mTarget.texture;
			if (texture == null || texture.name != mainTranslation)
			{
				this.mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
			}
		}
	}
}
