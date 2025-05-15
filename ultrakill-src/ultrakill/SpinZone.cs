using System;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class SpinZone : MonoBehaviour
{
	// Token: 0x06001849 RID: 6217 RVA: 0x000C644D File Offset: 0x000C464D
	private void OnDisable()
	{
		this.interactedWithItem = false;
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000C6456 File Offset: 0x000C4656
	private void OnEnable()
	{
		if (MonoSingleton<FistControl>.Instance.heldObject)
		{
			MonoSingleton<FistControl>.Instance.currentPunch.ForceThrow();
		}
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000C6478 File Offset: 0x000C4678
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10 || other.gameObject.layer == 11)
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && !enemyIdentifierIdentifier.eid.dead)
			{
				this.SpinEnemy(enemyIdentifierIdentifier.eid);
				return;
			}
		}
		else
		{
			EnemyIdentifier enemyIdentifier;
			if ((other.gameObject.layer == 12 || other.gameObject.CompareTag("Armor")) && other.TryGetComponent<EnemyIdentifier>(out enemyIdentifier) && !enemyIdentifier.dead)
			{
				this.SpinEnemy(enemyIdentifier);
				return;
			}
			Breakable breakable;
			if (other.TryGetComponent<Breakable>(out breakable) && !breakable.precisionOnly && !breakable.specialCaseOnly)
			{
				breakable.Break();
				return;
			}
			Glass glass;
			if (other.TryGetComponent<Glass>(out glass))
			{
				glass.Shatter();
				return;
			}
			if (other.gameObject.layer == 22 && !this.interactedWithItem)
			{
				ItemIdentifier component = other.GetComponent<ItemIdentifier>();
				ItemPlaceZone[] components = other.GetComponents<ItemPlaceZone>();
				if (MonoSingleton<FistControl>.Instance.heldObject && components != null && components.Length != 0)
				{
					this.interactedWithItem = true;
					MonoSingleton<FistControl>.Instance.heldObject.SendMessage("PutDown", SendMessageOptions.DontRequireReceiver);
					MonoSingleton<FistControl>.Instance.currentPunch.PlaceHeldObject(components, other.transform);
					return;
				}
				if (!MonoSingleton<FistControl>.Instance.heldObject && component != null)
				{
					this.interactedWithItem = true;
					component.SendMessage("PickUp", SendMessageOptions.DontRequireReceiver);
					MonoSingleton<FistControl>.Instance.currentPunch.ForceHold(component);
					if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
					{
						MonoSingleton<PlatformerMovement>.Instance.CheckItem();
						return;
					}
				}
				else if (MonoSingleton<FistControl>.Instance.heldObject)
				{
					MonoSingleton<FistControl>.Instance.heldObject.SendMessage("HitWith", other.gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000C664C File Offset: 0x000C484C
	private void SpinEnemy(EnemyIdentifier eid)
	{
		if (eid.blessed)
		{
			return;
		}
		if (this.dontSpinEnemies)
		{
			eid.Explode(false);
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.position = eid.transform.position;
		gameObject.transform.SetParent(GoreZone.ResolveGoreZone(eid.transform).gibZone);
		gameObject.gameObject.layer = 1;
		GameObject gameObject2 = Object.Instantiate<GameObject>(eid.gameObject, eid.transform.position, eid.transform.rotation);
		gameObject2.transform.localScale = eid.transform.lossyScale;
		gameObject2.transform.SetParent(gameObject.transform, true);
		eid.gameObject.SetActive(false);
		eid.hitter = "spin";
		eid.InstaKill();
		Object.Destroy(eid.gameObject);
		SandboxUtils.StripForPreview(gameObject2.transform, null, true);
		Object.Instantiate<GameObject>(this.spinSound, base.transform.position + (gameObject2.transform.position - base.transform.position).normalized, Quaternion.identity, gameObject2.transform);
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.velocity = (new Vector3(gameObject.transform.position.x, base.transform.position.y, gameObject.transform.position.z) - base.transform.position).normalized * 250f;
		rigidbody.useGravity = false;
		Collider collider;
		if (gameObject2.TryGetComponent<Collider>(out collider))
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(collider.bounds.size.x, collider.bounds.size.y, collider.bounds.size.z);
			boxCollider.isTrigger = true;
			gameObject.AddComponent<SpinZone>().dontSpinEnemies = true;
		}
		gameObject.AddComponent<RemoveOnTime>().time = 3f;
	}

	// Token: 0x04002217 RID: 8727
	public GameObject spinSound;

	// Token: 0x04002218 RID: 8728
	public bool dontSpinEnemies;

	// Token: 0x04002219 RID: 8729
	private bool interactedWithItem;
}
