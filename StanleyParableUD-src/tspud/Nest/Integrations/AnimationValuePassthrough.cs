using System;
using UnityEngine;

namespace Nest.Integrations
{
	// Token: 0x0200023F RID: 575
	[AddComponentMenu("Cast/Integrations/Animation Value Passthrough")]
	public class AnimationValuePassthrough : BaseIntegration
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0003CFE3 File Offset: 0x0003B1E3
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0003CFDA File Offset: 0x0003B1DA
		public override float InputValue
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0003CFEC File Offset: 0x0003B1EC
		public void Start()
		{
			this._animator = base.GetComponent<Animator>();
			if (this.ParameterName == string.Empty)
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
			{
				if (animatorControllerParameter.name == this.ParameterName)
				{
					this._parameter = animatorControllerParameter.nameHash;
					this._parameterType = animatorControllerParameter.type;
					return;
				}
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003D064 File Offset: 0x0003B264
		public void Update()
		{
			if (this.ParameterName == string.Empty)
			{
				return;
			}
			AnimatorControllerParameterType parameterType = this._parameterType;
			switch (parameterType)
			{
			case AnimatorControllerParameterType.Float:
				this._animator.SetFloat(this._parameter, this.InputValue);
				return;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				this._animator.SetInteger(this._parameter, Mathf.FloorToInt(this.InputValue));
				return;
			case AnimatorControllerParameterType.Bool:
				this._animator.SetBool(this._parameter, this.InputValue > this.Threshold);
				return;
			default:
				if (parameterType == AnimatorControllerParameterType.Trigger)
				{
					if (this.InputValue > this.Threshold)
					{
						this._animator.SetTrigger(this._parameter);
						return;
					}
					return;
				}
				break;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x04000C16 RID: 3094
		private Animator _animator;

		// Token: 0x04000C17 RID: 3095
		[SerializeField]
		private float _value;

		// Token: 0x04000C18 RID: 3096
		private int _parameter = -1;

		// Token: 0x04000C19 RID: 3097
		private AnimatorControllerParameterType _parameterType;

		// Token: 0x04000C1A RID: 3098
		public string ParameterName;

		// Token: 0x04000C1B RID: 3099
		[Tooltip("Used For Bool Parameters")]
		public float Threshold;
	}
}
