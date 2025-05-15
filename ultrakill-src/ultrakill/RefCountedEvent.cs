using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000385 RID: 901
public sealed class RefCountedEvent : MonoBehaviour
{
	// Token: 0x060014C2 RID: 5314 RVA: 0x000A78A0 File Offset: 0x000A5AA0
	public void AddRef()
	{
		if (this.m_RefCount == 0)
		{
			UnityEvent activate = this.m_Activate;
			if (activate != null)
			{
				activate.Invoke();
			}
		}
		this.m_RefCount++;
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x000A78C9 File Offset: 0x000A5AC9
	public void Release()
	{
		if (this.m_RefCount == 0)
		{
			throw new InvalidOperationException();
		}
		if (this.m_RefCount == 1)
		{
			UnityEvent deactivate = this.m_Deactivate;
			if (deactivate != null)
			{
				deactivate.Invoke();
			}
		}
		this.m_RefCount--;
	}

	// Token: 0x04001C8F RID: 7311
	private int m_RefCount;

	// Token: 0x04001C90 RID: 7312
	[SerializeField]
	private UnityEvent m_Activate;

	// Token: 0x04001C91 RID: 7313
	[SerializeField]
	private UnityEvent m_Deactivate;
}
