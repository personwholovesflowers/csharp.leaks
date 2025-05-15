using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002AF RID: 687
	public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
	{
		// Token: 0x060011C5 RID: 4549 RVA: 0x00061A07 File Offset: 0x0005FC07
		static LocalizeTarget_TextMeshPro_UGUI()
		{
			LocalizeTarget_TextMeshPro_UGUI.AutoRegister();
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00061A0E File Offset: 0x0005FC0E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI>
			{
				Name = "TextMeshPro UGUI",
				Priority = 100
			});
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000168F3 File Offset: 0x00014AF3
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.TextMeshPFont;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00061A30 File Offset: 0x0005FC30
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00061A88 File Offset: 0x0005FC88
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TMP_FontAsset tmp_FontAsset = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (tmp_FontAsset != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
			}
			else
			{
				Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget.fontMaterial != secondaryTranslatedObj)
				{
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget.font.name, StringComparison.Ordinal))
					{
						tmp_FontAsset = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, secondaryTranslation.EndsWith(secondaryTranslatedObj.name, StringComparison.Ordinal) ? secondaryTranslation : secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
						{
							LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
						}
					}
					LocalizeTarget_TextMeshPro_Label.SetMaterial(this.mTarget, secondaryTranslatedObj);
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAlignmentOptions textAlignmentOptions;
				TextAlignmentOptions textAlignmentOptions2;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out textAlignmentOptions, out textAlignmentOptions2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAlignmentOptions2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAlignmentOptions))
				{
					this.mAlignment_LTR = textAlignmentOptions;
					this.mAlignment_RTL = textAlignmentOptions2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (mainTranslation != null && cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
					this.mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
					if (LocalizationManager.IsRight2Left)
					{
						mainTranslation = I2Utils.ReverseText(mainTranslation);
					}
				}
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x04000E6B RID: 3691
		public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		// Token: 0x04000E6C RID: 3692
		public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		// Token: 0x04000E6D RID: 3693
		public bool mAlignmentWasRTL;

		// Token: 0x04000E6E RID: 3694
		public bool mInitializeAlignment = true;
	}
}
