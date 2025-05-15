using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021E RID: 542
[DefaultExecutionOrder(-500)]
public class GameStateManager : MonoBehaviour
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000B99 RID: 2969 RVA: 0x00051FBC File Offset: 0x000501BC
	// (set) Token: 0x06000B9A RID: 2970 RVA: 0x00051FC3 File Offset: 0x000501C3
	public static GameStateManager Instance { get; private set; }

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00051FCB File Offset: 0x000501CB
	// (set) Token: 0x06000B9C RID: 2972 RVA: 0x00051FD3 File Offset: 0x000501D3
	public Vector3 defaultGravity { get; private set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00051FDC File Offset: 0x000501DC
	// (set) Token: 0x06000B9E RID: 2974 RVA: 0x00051FE4 File Offset: 0x000501E4
	public bool CameraLocked { get; private set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00051FED File Offset: 0x000501ED
	// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x00051FF5 File Offset: 0x000501F5
	public bool PlayerInputLocked { get; private set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00051FFE File Offset: 0x000501FE
	// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x00052006 File Offset: 0x00050206
	public bool CursorLocked { get; private set; }

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0005200F File Offset: 0x0005020F
	// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x00052017 File Offset: 0x00050217
	public float TimerModifier { get; private set; } = 1f;

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00052020 File Offset: 0x00050220
	public static bool CanSubmitScores
	{
		get
		{
			return !MonoSingleton<StatsManager>.Instance.majorUsed && !MonoSingleton<AssistController>.Instance.cheatsEnabled && !SceneHelper.IsPlayingCustom;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00052044 File Offset: 0x00050244
	public static bool ShowLeaderboards
	{
		get
		{
			return GameStateManager.CanSubmitScores;
		}
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0005204C File Offset: 0x0005024C
	private void Awake()
	{
		if (GameStateManager.Instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GameStateManager.Instance = this;
		base.transform.SetParent(null);
		Object.DontDestroyOnLoad(base.gameObject);
		this.defaultGravity = Physics.gravity;
		this.IntroCheck();
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x000520A0 File Offset: 0x000502A0
	private void IntroCheck()
	{
		if (this.introCheckComplete || GameProgressSaver.GetIntro())
		{
			this.introCheckComplete = true;
			AudioListener.volume = MonoSingleton<PrefsManager>.Instance.GetFloat("allVolume", 0f);
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x000520D1 File Offset: 0x000502D1
	public bool IsStateActive(string stateKey)
	{
		return this.activeStates.ContainsKey(stateKey);
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x000520E0 File Offset: 0x000502E0
	public void RegisterState(GameState newState)
	{
		if (this.activeStates.ContainsKey(newState.key))
		{
			Debug.LogWarning("State " + newState.key + " is already registered");
			return;
		}
		this.activeStates.Add(newState.key, newState);
		int num = this.stateOrder.Count;
		for (int i = this.stateOrder.Count - 1; i >= 0; i--)
		{
			string text = this.stateOrder[i];
			GameState gameState = this.activeStates[text];
			num = i;
			if (gameState.priority > newState.priority)
			{
				num++;
				break;
			}
		}
		this.stateOrder.Insert(num, newState.key);
		this.EvaluateState();
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00052198 File Offset: 0x00050398
	public void PopState(string stateKey)
	{
		if (!this.activeStates.ContainsKey(stateKey))
		{
			Debug.Log("Tried to pop state " + stateKey + ", but it was not registered");
			return;
		}
		this.activeStates.Remove(stateKey);
		this.stateOrder.Remove(stateKey);
		this.EvaluateState();
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x000521E9 File Offset: 0x000503E9
	public void SceneReset()
	{
		this.ResetGravity();
		this.IntroCheck();
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x000521F7 File Offset: 0x000503F7
	public void ResetGravity()
	{
		Physics.gravity = this.defaultGravity;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00052204 File Offset: 0x00050404
	private void EvaluateState()
	{
		float num = 1f;
		for (int i = this.stateOrder.Count - 1; i >= 0; i--)
		{
			string text = this.stateOrder[i];
			GameState gameState = this.activeStates[text];
			if (gameState.cursorLock != LockMode.None)
			{
				this.CursorLocked = gameState.cursorLock == LockMode.Lock;
			}
			if (gameState.playerInputLock != LockMode.None)
			{
				this.PlayerInputLocked = gameState.playerInputLock == LockMode.Lock;
			}
			if (gameState.cameraInputLock != LockMode.None)
			{
				this.CameraLocked = gameState.cameraInputLock == LockMode.Lock;
			}
			if (gameState.timerModifier != null)
			{
				num *= gameState.timerModifier.Value;
			}
		}
		Cursor.lockState = (this.CursorLocked ? CursorLockMode.Locked : CursorLockMode.None);
		Cursor.visible = !this.CursorLocked;
		this.TimerModifier = num;
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x000522D4 File Offset: 0x000504D4
	private void Update()
	{
		for (int i = this.stateOrder.Count - 1; i >= 0; i--)
		{
			string text = this.stateOrder[i];
			if (!this.activeStates[text].IsValid())
			{
				this.activeStates.Remove(text);
				this.stateOrder.Remove(text);
				this.EvaluateState();
				return;
			}
		}
	}

	// Token: 0x04000F35 RID: 3893
	public bool introCheckComplete;

	// Token: 0x04000F37 RID: 3895
	public CustomGameDetails currentCustomGame;

	// Token: 0x04000F38 RID: 3896
	private readonly Dictionary<string, GameState> activeStates = new Dictionary<string, GameState>();

	// Token: 0x04000F39 RID: 3897
	private readonly List<string> stateOrder = new List<string>();
}
