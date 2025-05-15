using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020002B8 RID: 696
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		// Token: 0x06001212 RID: 4626 RVA: 0x000622E5 File Offset: 0x000604E5
		static LocalizeTarget_UnityUI_Image()
		{
			LocalizeTarget_UnityUI_Image.AutoRegister();
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x000622EC File Offset: 0x000604EC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>
			{
				Name = "Image",
				Priority = 100
			});
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0006230B File Offset: 0x0006050B
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			if (!(this.mTarget.sprite == null))
			{
				return eTermType.Sprite;
			}
			return eTermType.Texture;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00062324 File Offset: 0x00060524
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			if (this.mTarget.sprite != null && this.mTarget.sprite.name != primaryTerm)
			{
				primaryTerm = primaryTerm + "." + this.mTarget.sprite.name;
			}
			secondaryTerm = null;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x000623B0 File Offset: 0x000605B0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Sprite sprite = this.mTarget.sprite;
			if (sprite == null || sprite.name != mainTranslation)
			{
				this.mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
			}
		}
	}
}
