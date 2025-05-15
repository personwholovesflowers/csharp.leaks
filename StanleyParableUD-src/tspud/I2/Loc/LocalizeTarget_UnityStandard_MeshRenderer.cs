using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B3 RID: 691
	public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
	{
		// Token: 0x060011E6 RID: 4582 RVA: 0x00061E04 File Offset: 0x00060004
		static LocalizeTarget_UnityStandard_MeshRenderer()
		{
			LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00061E0B File Offset: 0x0006000B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>
			{
				Name = "MeshRenderer",
				Priority = 800
			});
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00061E2D File Offset: 0x0006002D
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Mesh;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00061E30 File Offset: 0x00060030
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Material;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0001C409 File Offset: 0x0001A609
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00061E34 File Offset: 0x00060034
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			if (this.mTarget == null)
			{
				string text;
				secondaryTerm = (text = null);
				primaryTerm = text;
			}
			else
			{
				MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
				if (component == null || component.sharedMesh == null)
				{
					primaryTerm = null;
				}
				else
				{
					primaryTerm = component.sharedMesh.name;
				}
			}
			if (this.mTarget == null || this.mTarget.sharedMaterial == null)
			{
				secondaryTerm = null;
				return;
			}
			secondaryTerm = this.mTarget.sharedMaterial.name;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00061ECC File Offset: 0x000600CC
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.sharedMaterial != secondaryTranslatedObj)
			{
				this.mTarget.material = secondaryTranslatedObj;
			}
			Mesh mesh = cmp.FindTranslatedObject<Mesh>(mainTranslation);
			MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
			if (mesh != null && component.sharedMesh != mesh)
			{
				component.mesh = mesh;
			}
		}
	}
}
