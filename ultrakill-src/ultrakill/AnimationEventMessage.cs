using System;
using UnityEngine;

// Token: 0x020001A5 RID: 421
[DisallowMultipleComponent]
public sealed class AnimationEventMessage : MessageDispatcher<global::AnimationEvent>.Callback<AnimationEventMessageEvent>
{
	// Token: 0x0600087A RID: 2170 RVA: 0x0003A54E File Offset: 0x0003874E
	private void OnAnimationEvent(global::AnimationEvent evt)
	{
		this.Handler.Invoke(evt);
	}
}
