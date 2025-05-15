using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B5 RID: 693
	public class LocalizeTarget_UnityStandard_Prefab : LocalizeTarget<GameObject>
	{
		// Token: 0x060011F2 RID: 4594 RVA: 0x00061F4E File Offset: 0x0006014E
		static LocalizeTarget_UnityStandard_Prefab()
		{
			LocalizeTarget_UnityStandard_Prefab.AutoRegister();
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00061F55 File Offset: 0x00060155
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Prefab
			{
				Name = "Prefab",
				Priority = 250
			});
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool IsValid(Localize cmp)
		{
			return true;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0002C806 File Offset: 0x0002AA06
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00061D7E File Offset: 0x0005FF7E
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00061F78 File Offset: 0x00060178
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			if (this.mTarget && this.mTarget.name == mainTranslation)
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
			Transform transform2 = this.InstantiateNewPrefab(cmp, mainTranslation);
			if (transform2 == null)
			{
				return;
			}
			transform2.name = text;
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform child = transform.GetChild(i);
				if (child != transform2)
				{
					Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00062024 File Offset: 0x00060224
		private Transform InstantiateNewPrefab(Localize cmp, string mainTranslation)
		{
			GameObject gameObject = cmp.FindTranslatedObject<GameObject>(mainTranslation);
			if (gameObject == null)
			{
				return null;
			}
			GameObject mTarget = this.mTarget;
			this.mTarget = Object.Instantiate<GameObject>(gameObject);
			if (this.mTarget == null)
			{
				return null;
			}
			Transform transform = cmp.transform;
			Transform transform2 = this.mTarget.transform;
			transform2.SetParent(transform);
			Transform transform3 = (mTarget ? mTarget.transform : transform);
			transform2.rotation = transform3.rotation;
			transform2.position = transform3.position;
			return transform2;
		}
	}
}
