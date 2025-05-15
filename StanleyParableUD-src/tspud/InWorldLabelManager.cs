using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EA RID: 234
public class InWorldLabelManager : MonoBehaviour
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001FA36 File Offset: 0x0001DC36
	// (set) Token: 0x060005AC RID: 1452 RVA: 0x0001FA3D File Offset: 0x0001DC3D
	public static InWorldLabelManager Instance { get; private set; }

	// Token: 0x060005AD RID: 1453 RVA: 0x0001FA45 File Offset: 0x0001DC45
	private void Awake()
	{
		InWorldLabelManager.Instance = this;
		this.rectTransform = this.scaler.GetComponent<RectTransform>();
		LocalizationManager.OnLocalizeEvent += this.LocalizationManager_OnLocalizeEvent;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0001FA6F File Offset: 0x0001DC6F
	private void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= this.LocalizationManager_OnLocalizeEvent;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0001FA84 File Offset: 0x0001DC84
	private void LocalizationManager_OnLocalizeEvent()
	{
		foreach (KeyValuePair<InWorldTextualObject, InWorldText> keyValuePair in this.inWorldObjects)
		{
			keyValuePair.Value.InitTextLabel(keyValuePair.Key);
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
	public void RegisterObject(InWorldTextualObject worldObj)
	{
		InWorldText component = Object.Instantiate<GameObject>(this.textPrefab.gameObject).GetComponent<InWorldText>();
		component.transform.parent = base.transform;
		component.transform.localScale = Vector3.one;
		component.transform.localPosition = new Vector2(-1000f, 1000f);
		this.inWorldObjects[worldObj] = component;
		component.InitTextLabel(worldObj);
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0001FB5C File Offset: 0x0001DD5C
	public void DeregisterObject(InWorldTextualObject obj)
	{
		if (!this.inWorldObjects.ContainsKey(obj))
		{
			Debug.LogError("Deregistering an in world object that has not been registered :(");
			return;
		}
		if (this.inWorldObjects[obj] != null && this.inWorldObjects[obj].gameObject != null)
		{
			Object.Destroy(this.inWorldObjects[obj].gameObject);
		}
		this.inWorldObjects.Remove(obj);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0001FBD4 File Offset: 0x0001DDD4
	private void Update()
	{
		foreach (KeyValuePair<InWorldTextualObject, InWorldText> keyValuePair in this.inWorldObjects)
		{
			InWorldTextualObject key = keyValuePair.Key;
			Vector2 vector = StanleyController.Instance.cam.WorldToViewportPoint(key.transform.position);
			Vector2 vector2 = (vector - Vector2.one / 2f) * this.rectTransform.sizeDelta;
			key.normalizedPosition_DEBUG = vector;
			bool flag = false;
			if (!this.normalizedIllegalZone.Contains(vector))
			{
				Vector3 vector3 = key.transform.position - StanleyController.Instance.camTransform.position;
				float magnitude = vector3.magnitude;
				key.currentRadius_DEBUG = magnitude;
				float num = Vector3.SignedAngle(vector3, StanleyController.Instance.transform.forward, Vector3.up);
				key.currentAngleFromCenter_DEBUG = num;
				flag = magnitude < key.activationRadius;
				if (Mathf.Abs(num) > 90f)
				{
					flag = false;
				}
				key.obstruction_DEBUG = null;
				RaycastHit raycastHit;
				if (flag && Physics.SphereCast(StanleyController.Instance.camTransform.position, this.sphereCastRadius, vector3, out raycastHit, magnitude - this.sphereCastRadius, this.raycastMask, QueryTriggerInteraction.Ignore))
				{
					flag = false;
					key.obstruction_DEBUG = raycastHit.collider;
				}
			}
			float num2 = (float)(flag ? 1 : 0);
			this.PlaceTextObject(keyValuePair.Key, vector2, num2);
		}
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0001FD7C File Offset: 0x0001DF7C
	private void PlaceTextObject(InWorldTextualObject worldObj, Vector2 screenPosition, float alpha)
	{
		InWorldText inWorldText = this.inWorldObjects[worldObj];
		inWorldText.transform.localPosition = screenPosition;
		inWorldText.UpdateTextLabel(worldObj, alpha, this.fadeTime);
	}

	// Token: 0x040005EB RID: 1515
	public InWorldLabelManager.InWorldLabelSizeProfile[] sizeProfiles;

	// Token: 0x040005EC RID: 1516
	public float fadeTime = 0.3f;

	// Token: 0x040005ED RID: 1517
	public InWorldText textPrefab;

	// Token: 0x040005EE RID: 1518
	public CanvasScaler scaler;

	// Token: 0x040005EF RID: 1519
	public LayerMask raycastMask;

	// Token: 0x040005F0 RID: 1520
	public float sphereCastRadius = 0.1f;

	// Token: 0x040005F1 RID: 1521
	public Rect normalizedIllegalZone = new Rect(0f, 0.8f, 1f, 0.2f);

	// Token: 0x040005F2 RID: 1522
	private Dictionary<InWorldTextualObject, InWorldText> inWorldObjects = new Dictionary<InWorldTextualObject, InWorldText>();

	// Token: 0x040005F3 RID: 1523
	private RectTransform rectTransform;

	// Token: 0x020003BF RID: 959
	[Serializable]
	public class InWorldLabelSizeProfile
	{
		// Token: 0x040013D9 RID: 5081
		public float fontSize = 38f;

		// Token: 0x040013DA RID: 5082
		public string i2LocalizationTerm = "None";
	}
}
