using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020002BA RID: 698
	public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
	{
		// Token: 0x06001226 RID: 4646 RVA: 0x000624A2 File Offset: 0x000606A2
		static LocalizeTarget_UnityUI_Text()
		{
			LocalizeTarget_UnityUI_Text.AutoRegister();
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x000624A9 File Offset: 0x000606A9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>
			{
				Name = "Text",
				Priority = 100
			});
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0001C409 File Offset: 0x0001A609
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000624C8 File Offset: 0x000606C8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00062520 File Offset: 0x00060720
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && secondaryTranslatedObj != this.mTarget.font)
			{
				this.mTarget.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAnchor textAnchor;
				TextAnchor textAnchor2;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out textAnchor, out textAnchor2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAnchor2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAnchor))
				{
					this.mAlignment_LTR = textAnchor;
					this.mAlignment_RTL = textAnchor2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.text = mainTranslation;
				this.mTarget.SetVerticesDirty();
			}
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0006264C File Offset: 0x0006084C
		private void InitAlignment(bool isRTL, TextAnchor alignment, out TextAnchor alignLTR, out TextAnchor alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignLTR = TextAnchor.UpperRight;
					return;
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.UpperRight:
					alignLTR = TextAnchor.UpperLeft;
					return;
				case TextAnchor.MiddleLeft:
					alignLTR = TextAnchor.MiddleRight;
					return;
				case TextAnchor.MiddleRight:
					alignLTR = TextAnchor.MiddleLeft;
					return;
				case TextAnchor.LowerLeft:
					alignLTR = TextAnchor.LowerRight;
					return;
				case TextAnchor.LowerRight:
					alignLTR = TextAnchor.LowerLeft;
					return;
				default:
					return;
				}
			}
			else
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignRTL = TextAnchor.UpperRight;
					return;
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.UpperRight:
					alignRTL = TextAnchor.UpperLeft;
					return;
				case TextAnchor.MiddleLeft:
					alignRTL = TextAnchor.MiddleRight;
					return;
				case TextAnchor.MiddleRight:
					alignRTL = TextAnchor.MiddleLeft;
					return;
				case TextAnchor.LowerLeft:
					alignRTL = TextAnchor.LowerRight;
					break;
				case TextAnchor.LowerRight:
					alignRTL = TextAnchor.LowerLeft;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x04000E73 RID: 3699
		private TextAnchor mAlignment_RTL = TextAnchor.UpperRight;

		// Token: 0x04000E74 RID: 3700
		private TextAnchor mAlignment_LTR;

		// Token: 0x04000E75 RID: 3701
		private bool mAlignmentWasRTL;

		// Token: 0x04000E76 RID: 3702
		private bool mInitializeAlignment = true;
	}
}
