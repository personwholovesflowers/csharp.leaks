using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B6 RID: 694
	public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
	{
		// Token: 0x060011FE RID: 4606 RVA: 0x000620AA File Offset: 0x000602AA
		static LocalizeTarget_UnityStandard_SpriteRenderer()
		{
			LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000620B1 File Offset: 0x000602B1
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>
			{
				Name = "SpriteRenderer",
				Priority = 100
			});
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0005FC04 File Offset: 0x0005DE04
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000620D0 File Offset: 0x000602D0
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((this.mTarget.sprite != null) ? this.mTarget.sprite.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00062104 File Offset: 0x00060304
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
