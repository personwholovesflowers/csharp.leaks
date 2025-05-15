using System;
using UnityEngine;

namespace DebugOverlays
{
	// Token: 0x02000602 RID: 1538
	public class EnemyIdentifierDebugOverlay : MonoBehaviour
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x0010BF6E File Offset: 0x0010A16E
		public void ConsumeData(EnemyType enemyType, EnemyClass enemyClass, bool dead, bool ignorePlayer, bool attackEnemies, EnemyTarget target)
		{
			this.enemyType = enemyType;
			this.enemyClass = enemyClass;
			this.dead = dead;
			this.ignorePlayer = ignorePlayer;
			this.attackEnemies = attackEnemies;
			this.target = target;
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x0010BFA0 File Offset: 0x0010A1A0
		private void OnGUI()
		{
			Rect? onScreenRect = OnGUIHelper.GetOnScreenRect(base.transform.position, 300f, 100f);
			if (onScreenRect == null)
			{
				return;
			}
			Rect value = onScreenRect.Value;
			GUI.Label(value, string.Format("{0} ({1})", this.enemyType, this.enemyClass));
			if (this.dead)
			{
				GUI.color = Color.red;
				value.y += 20f;
				GUI.Label(value, "Dead!");
				return;
			}
			GUI.color = Color.white;
			value.y += 20f;
			GUI.Label(value, string.Format("Ignore player: {0}", this.ignorePlayer));
			value.y += 20f;
			GUI.Label(value, string.Format("Attack enemies: {0}", this.attackEnemies));
			value.y += 20f;
			if (this.target == null)
			{
				GUI.color = Color.red;
				GUI.Label(value, "Target: Null");
			}
			else if (this.target.isPlayer)
			{
				GUI.Label(value, "Target: (Player)");
			}
			else if (this.target.targetTransform != null)
			{
				GUI.Label(value, "Target: (" + this.target.targetTransform.name + ")");
				Vector3 position = this.target.position;
				Vector3 vector = MonoSingleton<CameraController>.Instance.cam.WorldToScreenPoint(position);
				if (position.z > 0f)
				{
					Rect rect = new Rect(vector.x - 5f, (float)Screen.height - vector.y - 5f, 10f, 10f);
					GUI.color = Color.yellow;
					GUI.Box(rect, "");
					GUI.color = Color.white;
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x04002DFF RID: 11775
		private EnemyType enemyType;

		// Token: 0x04002E00 RID: 11776
		private EnemyClass enemyClass;

		// Token: 0x04002E01 RID: 11777
		private bool dead;

		// Token: 0x04002E02 RID: 11778
		private bool ignorePlayer;

		// Token: 0x04002E03 RID: 11779
		private bool attackEnemies;

		// Token: 0x04002E04 RID: 11780
		private EnemyTarget target;
	}
}
