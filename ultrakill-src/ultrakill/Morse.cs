using System;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class Morse : MonoBehaviour
{
	// Token: 0x06001188 RID: 4488 RVA: 0x000886CD File Offset: 0x000868CD
	private void Update()
	{
		if (this.timer > this.speed)
		{
			this.timer = 0f;
			this.Tick();
		}
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x000886F8 File Offset: 0x000868F8
	private void Tick()
	{
		if (this.current >= this.code.Length)
		{
			this.current = 0;
			return;
		}
		char c = this.code[this.current];
		if (c != ' ')
		{
			switch (c)
			{
			case '-':
			{
				UltrakillEvent ultrakillEvent = this.onDash;
				if (ultrakillEvent == null)
				{
					goto IL_008F;
				}
				ultrakillEvent.Invoke("");
				goto IL_008F;
			}
			case '.':
			{
				UltrakillEvent ultrakillEvent2 = this.onDot;
				if (ultrakillEvent2 == null)
				{
					goto IL_008F;
				}
				ultrakillEvent2.Invoke("");
				goto IL_008F;
			}
			case '/':
				break;
			default:
				goto IL_008F;
			}
		}
		UltrakillEvent ultrakillEvent3 = this.onSpace;
		if (ultrakillEvent3 != null)
		{
			ultrakillEvent3.Invoke("");
		}
		IL_008F:
		this.current++;
	}

	// Token: 0x040017CB RID: 6091
	[SerializeField]
	private string code;

	// Token: 0x040017CC RID: 6092
	[SerializeField]
	private float speed;

	// Token: 0x040017CD RID: 6093
	[HideInInspector]
	public int current;

	// Token: 0x040017CE RID: 6094
	[SerializeField]
	private UltrakillEvent onDot;

	// Token: 0x040017CF RID: 6095
	[SerializeField]
	private UltrakillEvent onDash;

	// Token: 0x040017D0 RID: 6096
	[SerializeField]
	private UltrakillEvent onSpace;

	// Token: 0x040017D1 RID: 6097
	private TimeSince timer;
}
