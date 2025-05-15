using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class AnimationSliderEvent : MonoBehaviour
{
	// Token: 0x06000361 RID: 865 RVA: 0x00016B08 File Offset: 0x00014D08
	private void Awake()
	{
		this._animation[this._animation.clip.name].speed = 0f;
		this._animation.Play(this._animation.clip.name);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00016B56 File Offset: 0x00014D56
	public void ValueChanged(float value)
	{
		this._animation[this._animation.clip.name].normalizedTime = value;
	}

	// Token: 0x04000359 RID: 857
	[SerializeField]
	private Animation _animation;
}
