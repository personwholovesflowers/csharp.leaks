using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B2 RID: 690
	public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
	{
		// Token: 0x060011DB RID: 4571 RVA: 0x00061D55 File Offset: 0x0005FF55
		static LocalizeTarget_UnityStandard_Child()
		{
			LocalizeTarget_UnityStandard_Child.AutoRegister();
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00061D5C File Offset: 0x0005FF5C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Child
			{
				Name = "Child",
				Priority = 200
			});
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00061D3D File Offset: 0x0005FF3D
		public override bool IsValid(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0002C806 File Offset: 0x0002AA06
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00061D7E File Offset: 0x0005FF7E
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00061D90 File Offset: 0x0005FF90
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			Transform transform = cmp.transform;
			string text = mainTranslation;
			int num = mainTranslation.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num >= 0)
			{
				text = text.Substring(num + 1);
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.gameObject.SetActive(child.name == text);
			}
		}
	}
}
