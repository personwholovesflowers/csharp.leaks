using System;
using System.Linq;
using UnityEngine;

namespace GameConsole
{
	// Token: 0x020005A0 RID: 1440
	public class ClipboardStuff : MonoBehaviour
	{
		// Token: 0x06002049 RID: 8265 RVA: 0x00104E06 File Offset: 0x00103006
		public void TogglePopup()
		{
			base.gameObject.SetActive(!base.gameObject.activeSelf);
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00104E21 File Offset: 0x00103021
		public void CopyToClipboard()
		{
			GUIUtility.systemCopyBuffer = string.Join("\n", MonoSingleton<Console>.Instance.logs.Select((ConsoleLog c) => string.Format("[{0:HH:mm:ss.f}] [{1}] {2}\n{3}", new object[]
			{
				c.log.Timestamp,
				c.log.Level,
				c.log.Message,
				c.log.StackTrace
			})));
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00104E60 File Offset: 0x00103060
		public void OpenLogFile()
		{
			string text = Application.persistentDataPath + "/Player.log";
			Application.OpenURL("file://" + text);
		}
	}
}
