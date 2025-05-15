using System;
using UnityEngine;

namespace Nest.Integrations
{
	// Token: 0x0200023E RID: 574
	[AddComponentMenu("Cast/Integrations/Animation Scrubbing")]
	public class AnimationScrubbing : BaseIntegration
	{
		// Token: 0x17000138 RID: 312
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0003CF6E File Offset: 0x0003B16E
		public override float InputValue
		{
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0003CF77 File Offset: 0x0003B177
		private void Start()
		{
			this._animator = base.GetComponent<Animator>();
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0003CF85 File Offset: 0x0003B185
		private void Update()
		{
			this.PlayAnimator();
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0003CF90 File Offset: 0x0003B190
		public void PlayAnimator()
		{
			if (Math.Abs(this._animator.speed) < 0.01f)
			{
				this._animator.Play(this._animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, this._value);
			}
		}

		// Token: 0x04000C14 RID: 3092
		private Animator _animator;

		// Token: 0x04000C15 RID: 3093
		private float _value;
	}
}
