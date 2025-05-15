using System;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class FloatingPointErrorPreventer : MonoBehaviour
{
	// Token: 0x06000A4D RID: 2637 RVA: 0x00048A15 File Offset: 0x00046C15
	private void Start()
	{
		this.startPos = base.transform.position;
		this.SlowCheck();
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x00048A30 File Offset: 0x00046C30
	private void SlowCheck()
	{
		if (!this.deactivated)
		{
			base.Invoke("SlowCheck", this.checkFrequency);
		}
		if (base.gameObject.activeInHierarchy)
		{
			Vector3 position = base.transform.position;
			if (base.transform.position.y > 0f && base.transform.position.y < 10000f)
			{
				position.y = this.startPos.y;
			}
			if (Vector3.Distance(position, this.startPos) > 5000f)
			{
				this.deactivated = true;
				CharacterJoint[] componentsInChildren = base.GetComponentsInChildren<CharacterJoint>();
				Rigidbody[] componentsInChildren2 = base.GetComponentsInChildren<Rigidbody>();
				foreach (CharacterJoint characterJoint in componentsInChildren)
				{
					EnemyIdentifierIdentifier enemyIdentifierIdentifier;
					if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
					{
						enemyIdentifierIdentifier.SetupForHellBath();
					}
					Object.Destroy(characterJoint);
				}
				foreach (Rigidbody rigidbody in componentsInChildren2)
				{
					if (rigidbody != null)
					{
						rigidbody.useGravity = false;
						rigidbody.isKinematic = true;
					}
				}
				base.gameObject.SetActive(false);
				base.transform.position = new Vector3(-100f, -100f, -100f);
				base.transform.localScale = Vector3.zero;
				Object.Destroy(this);
				MonoBehaviour.print("called fperrorprevent");
			}
		}
	}

	// Token: 0x04000DA1 RID: 3489
	private bool deactivated;

	// Token: 0x04000DA2 RID: 3490
	private Vector3 startPos;

	// Token: 0x04000DA3 RID: 3491
	public float checkFrequency = 3f;
}
