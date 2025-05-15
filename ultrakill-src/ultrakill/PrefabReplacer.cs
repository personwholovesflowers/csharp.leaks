using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000046 RID: 70
[DefaultExecutionOrder(-20)]
public class PrefabReplacer : MonoBehaviour
{
	// Token: 0x06000146 RID: 326 RVA: 0x00006C4D File Offset: 0x00004E4D
	private void Awake()
	{
		PrefabReplacer.Instance = this;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00006C55 File Offset: 0x00004E55
	public GameObject LoadPrefab(string address)
	{
		return AssetHelper.LoadPrefab(address);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00006C60 File Offset: 0x00004E60
	private void PerformSwap(PlaceholderPrefab placeholder)
	{
		if (PrefabReplacer.ForceDisable)
		{
			return;
		}
		if (!placeholder.gameObject.activeSelf)
		{
			return;
		}
		Debug.Log("Swapping " + placeholder.name);
		Transform transform = placeholder.transform;
		if (transform == null)
		{
			return;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(placeholder.uniqueId);
		asyncOperationHandle.WaitForCompletion();
		GameObject gameObject = Object.Instantiate<GameObject>(asyncOperationHandle.Result, transform.position, transform.rotation, transform.parent);
		Debug.Log("Swapped " + placeholder.name + " with " + gameObject.name);
		IPlaceholdableComponent[] array = placeholder.GetComponentsInChildren<IPlaceholdableComponent>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].WillReplace(placeholder.gameObject, gameObject, true);
		}
		array = gameObject.GetComponentsInChildren<IPlaceholdableComponent>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].WillReplace(placeholder.gameObject, gameObject, false);
		}
		Object.Destroy(placeholder.gameObject);
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00006D5B File Offset: 0x00004F5B
	public void ReplacePrefab(PlaceholderPrefab placeholder)
	{
		this.PerformSwap(placeholder);
	}

	// Token: 0x0400013D RID: 317
	public static bool ForceDisable;

	// Token: 0x0400013E RID: 318
	public static PrefabReplacer Instance;
}
