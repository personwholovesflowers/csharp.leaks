using System;
using Nest.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Nest.Addons
{
	// Token: 0x0200024F RID: 591
	public class ScenePostbox : MonoBehaviour
	{
		// Token: 0x06000DFF RID: 3583 RVA: 0x0003EAC4 File Offset: 0x0003CCC4
		public void RecieveEvent(string message)
		{
			UnityEvent unityEvent;
			if (this.Events.TryGetValue(message, out unityEvent))
			{
				unityEvent.Invoke();
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0003EAE7 File Offset: 0x0003CCE7
		private void OnEnable()
		{
			Singleton<SceneController>.Instance.RegisterPostbox(this);
		}

		// Token: 0x04000C81 RID: 3201
		public ScenePostbox.EventDictionary Events;

		// Token: 0x02000448 RID: 1096
		[Serializable]
		public class EventDictionary : SerializableDictionaryBase<string, UnityEvent>
		{
		}
	}
}
