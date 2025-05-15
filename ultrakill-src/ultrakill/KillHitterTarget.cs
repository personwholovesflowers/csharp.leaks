using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class KillHitterTarget : MonoBehaviour
{
	// Token: 0x06000F17 RID: 3863 RVA: 0x0006FCE6 File Offset: 0x0006DEE6
	private void Start()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x0006FCF4 File Offset: 0x0006DEF4
	private void Update()
	{
		if (!this.done && this.eid.dead)
		{
			this.done = true;
			Debug.Log(this.eid.hitter);
			string[] array = this.acceptedHitters;
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i];
				if (this.eid.hitter == text)
				{
					if (this.khc == null)
					{
						this.khc = MonoSingleton<KillHitterCache>.Instance;
					}
					this.khc.OneDone(this.id);
					GoreZone componentInParent = base.GetComponentInParent<GoreZone>();
					if (componentInParent)
					{
						componentInParent.AddKillHitterTarget(this.id);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			Object.Destroy(this);
		}
	}

	// Token: 0x04001441 RID: 5185
	public string[] acceptedHitters;

	// Token: 0x04001442 RID: 5186
	private KillHitterCache khc;

	// Token: 0x04001443 RID: 5187
	public int id;

	// Token: 0x04001444 RID: 5188
	private EnemyIdentifier eid;

	// Token: 0x04001445 RID: 5189
	private bool done;
}
