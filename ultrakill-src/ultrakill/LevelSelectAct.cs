using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class LevelSelectAct : MonoBehaviour
{
	// Token: 0x06000F44 RID: 3908 RVA: 0x00070577 File Offset: 0x0006E777
	private void Awake()
	{
		this.childLayers = base.GetComponentsInChildren<LayerSelect>(true);
		this.inputSource = MonoSingleton<InputManager>.Instance.InputSource;
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00070598 File Offset: 0x0006E798
	private void Update()
	{
		if (!GameStateManager.ShowLeaderboards)
		{
			return;
		}
		if (!this.inputSource.NextWeapon.WasPerformedThisFrame && !this.inputSource.LastWeapon.WasPerformedThisFrame && !this.inputSource.PreviousVariation.WasPerformedThisFrame)
		{
			return;
		}
		string text = "";
		if (this.inputSource.NextWeapon.WasPerformedThisFrame)
		{
			text = this.inputSource.NextWeapon.LastUsedBinding;
		}
		else if (this.inputSource.LastWeapon.WasPerformedThisFrame)
		{
			text = this.inputSource.LastWeapon.LastUsedBinding;
		}
		else if (this.inputSource.PreviousVariation.WasPerformedThisFrame)
		{
			text = this.inputSource.PreviousVariation.LastUsedBinding;
		}
		if (text == null || string.IsNullOrEmpty(text) || text.Contains("/dpad/"))
		{
			return;
		}
		this.ChangeLeaderboardType(!this.currentLeaderboardMode);
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00070680 File Offset: 0x0006E880
	public void ChangeLeaderboardType(bool pRank)
	{
		this.currentLeaderboardMode = pRank;
		base.StopAllCoroutines();
		foreach (LayerSelect layerSelect in this.childLayers)
		{
			base.StartCoroutine(this.SwitchLeaderboardsSequence(layerSelect, pRank));
		}
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000706C2 File Offset: 0x0006E8C2
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000706CA File Offset: 0x0006E8CA
	private IEnumerator SwitchLeaderboardsSequence(LayerSelect layer, bool pRank)
	{
		LevelSelectLeaderboard[] array = layer.childLeaderboards;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SwitchLeaderboardType(pRank);
			yield return new WaitForSeconds(0.1f);
		}
		array = null;
		yield break;
	}

	// Token: 0x0400146E RID: 5230
	private LayerSelect[] childLayers;

	// Token: 0x0400146F RID: 5231
	private PlayerInput inputSource;

	// Token: 0x04001470 RID: 5232
	private bool currentLeaderboardMode;
}
