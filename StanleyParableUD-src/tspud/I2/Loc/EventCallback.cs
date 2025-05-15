using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000293 RID: 659
	[Serializable]
	public class EventCallback
	{
		// Token: 0x06001089 RID: 4233 RVA: 0x00056A7B File Offset: 0x00054C7B
		public void Execute(Object Sender = null)
		{
			if (this.HasCallback() && Application.isPlaying)
			{
				this.Target.gameObject.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00056AA4 File Offset: 0x00054CA4
		public bool HasCallback()
		{
			return this.Target != null && !string.IsNullOrEmpty(this.MethodName);
		}

		// Token: 0x04000DCF RID: 3535
		public MonoBehaviour Target;

		// Token: 0x04000DD0 RID: 3536
		public string MethodName = string.Empty;
	}
}
