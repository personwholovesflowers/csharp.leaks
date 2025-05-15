using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B0 RID: 688
	public class LocalizeTarget_UnityStandard_AudioSource : LocalizeTarget<AudioSource>
	{
		// Token: 0x060011CF RID: 4559 RVA: 0x00061C63 File Offset: 0x0005FE63
		static LocalizeTarget_UnityStandard_AudioSource()
		{
			LocalizeTarget_UnityStandard_AudioSource.AutoRegister();
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00061C6A File Offset: 0x0005FE6A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource>
			{
				Name = "AudioSource",
				Priority = 100
			});
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00061C89 File Offset: 0x0005FE89
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.AudioClip;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0001A562 File Offset: 0x00018762
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0001A562 File Offset: 0x00018762
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00061C8C File Offset: 0x0005FE8C
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.clip ? this.mTarget.clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00061CC0 File Offset: 0x0005FEC0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			bool flag = (this.mTarget.isPlaying || this.mTarget.loop) && Application.isPlaying;
			Object clip = this.mTarget.clip;
			AudioClip audioClip = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
			if (clip != audioClip)
			{
				this.mTarget.clip = audioClip;
			}
			if (flag && this.mTarget.clip)
			{
				this.mTarget.Play();
			}
		}
	}
}
