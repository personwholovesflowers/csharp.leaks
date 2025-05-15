using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B7 RID: 695
	public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
	{
		// Token: 0x06001208 RID: 4616 RVA: 0x0006214E File Offset: 0x0006034E
		static LocalizeTarget_UnityStandard_TextMesh()
		{
			LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00062155 File Offset: 0x00060355
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>
			{
				Name = "TextMesh",
				Priority = 100
			});
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0001C409 File Offset: 0x0001A609
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00062174 File Offset: 0x00060374
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((string.IsNullOrEmpty(Secondary) && this.mTarget.font != null) ? this.mTarget.font.name : null);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x000621D0 File Offset: 0x000603D0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.font != secondaryTranslatedObj)
			{
				this.mTarget.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignment_LTR = (this.mAlignment_RTL = this.mTarget.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == TextAlignment.Right)
				{
					this.mAlignment_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == TextAlignment.Left)
				{
					this.mAlignment_RTL = TextAlignment.Right;
				}
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && this.mTarget.alignment != TextAlignment.Center)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.font.RequestCharactersInTexture(mainTranslation);
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x04000E6F RID: 3695
		private TextAlignment mAlignment_RTL = TextAlignment.Right;

		// Token: 0x04000E70 RID: 3696
		private TextAlignment mAlignment_LTR;

		// Token: 0x04000E71 RID: 3697
		private bool mAlignmentWasRTL;

		// Token: 0x04000E72 RID: 3698
		private bool mInitializeAlignment = true;
	}
}
