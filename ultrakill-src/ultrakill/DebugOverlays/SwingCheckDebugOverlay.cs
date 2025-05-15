using System;
using UnityEngine;

namespace DebugOverlays
{
	// Token: 0x02000604 RID: 1540
	public class SwingCheckDebugOverlay : MonoBehaviour
	{
		// Token: 0x06002239 RID: 8761 RVA: 0x0010C204 File Offset: 0x0010A404
		public void ConsumeData(bool damaging, EnemyIdentifier eid)
		{
			this.damaging = damaging;
			this.eid = eid;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0010C214 File Offset: 0x0010A414
		private void OnGUI()
		{
			if (!this.damaging)
			{
				return;
			}
			Rect? onScreenRect = OnGUIHelper.GetOnScreenRect(base.transform.position, 300f, 100f);
			if (onScreenRect == null)
			{
				return;
			}
			Rect value = onScreenRect.Value;
			GUI.Label(value, "SWING!", new GUIStyle
			{
				fontSize = 20,
				fontStyle = FontStyle.Bold,
				normal = 
				{
					textColor = Color.red
				}
			});
			value.y += 20f;
			if (this.eid == null)
			{
				GUI.Label(value, "No EID", new GUIStyle
				{
					fontSize = 20,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = Color.magenta
					}
				});
				return;
			}
			if (this.eid.target == null)
			{
				GUI.Label(value, "No target", new GUIStyle
				{
					fontSize = 20,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = Color.yellow
					}
				});
				return;
			}
			if (this.eid.target.isPlayer)
			{
				GUI.Label(value, "Player target", new GUIStyle
				{
					fontSize = 20,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = Color.green
					}
				});
				return;
			}
			GUI.Label(value, this.eid.target.ToString(), new GUIStyle
			{
				fontSize = 20,
				fontStyle = FontStyle.Bold,
				normal = 
				{
					textColor = Color.blue
				}
			});
		}

		// Token: 0x04002E05 RID: 11781
		private bool damaging;

		// Token: 0x04002E06 RID: 11782
		private EnemyIdentifier eid;
	}
}
