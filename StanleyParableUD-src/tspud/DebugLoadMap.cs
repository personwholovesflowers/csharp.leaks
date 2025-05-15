using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000AB RID: 171
public class DebugLoadMap : MonoBehaviour
{
	// Token: 0x0600040C RID: 1036 RVA: 0x0001919C File Offset: 0x0001739C
	public void TryLoadMap()
	{
		if (Singleton<GameMaster>.Instance.ChangeLevel(this.levelNameInput.text.Trim(new char[] { '\u200b' }), true))
		{
			this.levelNameInput.color = Color.black;
			this.OnChangeLevel.Invoke();
			Singleton<GameMaster>.Instance.ClosePauseMenu(false);
			return;
		}
		this.levelNameInput.color = Color.red;
	}

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private TextMeshProUGUI levelNameInput;

	// Token: 0x04000405 RID: 1029
	[SerializeField]
	private UnityEvent OnChangeLevel;
}
