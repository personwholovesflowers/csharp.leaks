using System;
using Logic;
using pcon.core;
using UnityEngine.Events;

namespace GameConsole.pcon
{
	// Token: 0x020005B9 RID: 1465
	[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
	public class MapVarRelay : MonoSingleton<MapVarRelay>
	{
		// Token: 0x060020D7 RID: 8407 RVA: 0x00107C54 File Offset: 0x00105E54
		private void Start()
		{
			MonoSingleton<MapVarManager>.Instance.RegisterGlobalWatcher(new UnityAction<string, object>(this.ReceiveChange));
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00107C6C File Offset: 0x00105E6C
		private void ReceiveChange(string name, object value)
		{
			this.UpdateMapVars(MonoSingleton<MapVarManager>.Instance.Store);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00107C7E File Offset: 0x00105E7E
		public void UpdateMapVars(VarStore store)
		{
			PCon.SendMessage(new MapVarsMessage(store));
		}
	}
}
