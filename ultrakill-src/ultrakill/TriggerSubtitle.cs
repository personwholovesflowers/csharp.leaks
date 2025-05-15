using System;
using UnityEngine;

// Token: 0x02000467 RID: 1127
public class TriggerSubtitle : MonoBehaviour
{
	// Token: 0x060019C2 RID: 6594 RVA: 0x000D360C File Offset: 0x000D180C
	private void Awake()
	{
		Collider component = base.GetComponent<Collider>();
		if (component && component.isTrigger)
		{
			this.col = component;
		}
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000D3637 File Offset: 0x000D1837
	private void OnEnable()
	{
		if (!this.col && this.activateOnEnableIfNoTrigger)
		{
			this.PushCaption();
		}
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000D3654 File Offset: 0x000D1854
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		this.PushCaption();
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000D366A File Offset: 0x000D186A
	public void PushCaption()
	{
		this.PushCaptionOverride(this.caption);
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000D3678 File Offset: 0x000D1878
	public void PushCaptionOverride(string caption)
	{
		if (!MonoSingleton<SubtitleController>.Instance)
		{
			return;
		}
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle(caption, null, this.ignorePlayerPreferences);
	}

	// Token: 0x04002411 RID: 9233
	[SerializeField]
	[TextArea]
	private string caption;

	// Token: 0x04002412 RID: 9234
	[SerializeField]
	private bool ignorePlayerPreferences;

	// Token: 0x04002413 RID: 9235
	[SerializeField]
	private bool activateOnEnableIfNoTrigger = true;

	// Token: 0x04002414 RID: 9236
	private Collider col;
}
