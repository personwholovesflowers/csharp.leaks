using System;
using System.Collections;
using SoftMasking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010A RID: 266
public class MainMenu : MonoBehaviour
{
	// Token: 0x06000661 RID: 1633 RVA: 0x00022DD9 File Offset: 0x00020FD9
	private void Awake()
	{
		this.rootCanvas = base.GetComponent<Canvas>();
		this.cachedCaster = base.GetComponent<GraphicRaycaster>();
		this.menuAudio.SetActive(false);
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00022E00 File Offset: 0x00021000
	private void Start()
	{
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.Instance_OnInputDeviceTypeChanged;
		this.scrollRects = base.GetComponentsInChildren<ScrollRect>(true);
		this.softMasks = base.GetComponentsInChildren<SoftMask>(true);
		this.textMeshPros = base.GetComponentsInChildren<TextMeshProUGUI>(true);
		this.uiCameras = base.GetComponentsInChildren<Camera>(true);
		this.canvases = base.GetComponentsInChildren<Canvas>(true);
		this.audioSources = base.GetComponentsInChildren<AudioSource>(true);
		if (!GameMaster.ONMAINMENUORSETTINGS)
		{
			this.SetRoot(false);
		}
		Canvas component = base.GetComponent<Canvas>();
		component.worldCamera.gameObject.SetActive(false);
		component.renderMode = RenderMode.ScreenSpaceOverlay;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00022E9C File Offset: 0x0002109C
	private void Instance_OnInputDeviceTypeChanged(GameMaster.InputDevice deviceType)
	{
		if (this.InAMenu)
		{
			if (deviceType == GameMaster.InputDevice.KeyboardAndMouse)
			{
				GameMaster.CursorVisible = Singleton<GameMaster>.Instance.MouseMoved;
				return;
			}
			GameMaster.CursorVisible = false;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x00022EBF File Offset: 0x000210BF
	// (set) Token: 0x06000665 RID: 1637 RVA: 0x00022EC7 File Offset: 0x000210C7
	public bool InAMenu { get; private set; }

	// Token: 0x06000666 RID: 1638 RVA: 0x00022ED0 File Offset: 0x000210D0
	private void Update()
	{
		if (this.InAMenu && Singleton<GameMaster>.Instance.MouseMoved && !GameMaster.CursorVisible)
		{
			GameMaster.CursorVisible = true;
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00022EF3 File Offset: 0x000210F3
	private void EnterMenu()
	{
		this.InAMenu = true;
		this.HideConsoleOnlyOptions();
		this.SetRoot(true);
		Cursor.lockState = CursorLockMode.None;
		GameMaster.CursorVisible = false;
		this.menuAudio.SetActive(true);
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00022F21 File Offset: 0x00021121
	public void ExitMenu()
	{
		this.SetRoot(false);
		this.menuAudio.SetActive(false);
		this.InAMenu = false;
		Cursor.lockState = CursorLockMode.Locked;
		GameMaster.CursorVisible = false;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00022F49 File Offset: 0x00021149
	private void SetRoot(bool status)
	{
		this.cachedCaster.enabled = status;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00022F57 File Offset: 0x00021157
	private IEnumerator DelayRootStatus(bool status)
	{
		yield return new WaitForSeconds(0.1f);
		this.root.gameObject.SetActive(status);
		yield break;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00022F70 File Offset: 0x00021170
	public void CallPauseMenu()
	{
		this.EnterMenu();
		for (int i = 0; i < this.pauseScreenEvents.Length; i++)
		{
			this.pauseScreenEvents[i].Invoke();
		}
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00022FA3 File Offset: 0x000211A3
	public void CallMainMenu()
	{
		this.EnterMenu();
		base.StartCoroutine(this.WaitOneFrameAndInvokeMainMenuEvents());
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00022FB8 File Offset: 0x000211B8
	private IEnumerator WaitOneFrameAndInvokeMainMenuEvents()
	{
		yield return null;
		yield return null;
		yield return null;
		for (int i = 0; i < this.mainMenuEvents.Length; i++)
		{
			this.mainMenuEvents[i].Invoke();
		}
		yield break;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00022FC7 File Offset: 0x000211C7
	public void BeginTheGame()
	{
		this.ExitMenu();
		Singleton<GameMaster>.Instance.TSP_Reload();
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00022FD9 File Offset: 0x000211D9
	public void BeginTheEpilogue()
	{
		this.ExitMenu();
		Singleton<GameMaster>.Instance.TSP_Reload(GameMaster.TSP_Reload_Behaviour.Epilogue);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00022FEC File Offset: 0x000211EC
	public void ReturnToMainMenu()
	{
		Singleton<GameMaster>.Instance.TSP_MainMenu();
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00022FF8 File Offset: 0x000211F8
	public void ResumeGame()
	{
		Singleton<GameMaster>.Instance.ClosePauseMenu(true);
		this.ExitMenu();
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0002300B File Offset: 0x0002120B
	public void QuitGame()
	{
		Application.Quit();
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00023014 File Offset: 0x00021214
	public void HidePcOnlyOptions()
	{
		for (int i = 0; i < this.PCOnlyElements.Length; i++)
		{
			this.PCOnlyElements[i].SetActive(false);
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00023044 File Offset: 0x00021244
	public void HideConsoleOnlyOptions()
	{
		for (int i = 0; i < this.ConsoleOnlyElements.Length; i++)
		{
			this.ConsoleOnlyElements[i].SetActive(false);
		}
	}

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private ConfigurableEvent[] mainMenuEvents;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	private ConfigurableEvent[] pauseScreenEvents;

	// Token: 0x040006C7 RID: 1735
	[SerializeField]
	private GameObject menuAudio;

	// Token: 0x040006C8 RID: 1736
	[Space]
	[SerializeField]
	private GameObject[] PCOnlyElements;

	// Token: 0x040006C9 RID: 1737
	[SerializeField]
	private GameObject[] ConsoleOnlyElements;

	// Token: 0x040006CA RID: 1738
	[SerializeField]
	private ScrollRect[] scrollRects;

	// Token: 0x040006CB RID: 1739
	[SerializeField]
	private SoftMask[] softMasks;

	// Token: 0x040006CC RID: 1740
	[SerializeField]
	private TextMeshProUGUI[] textMeshPros;

	// Token: 0x040006CD RID: 1741
	[SerializeField]
	private Camera[] uiCameras;

	// Token: 0x040006CE RID: 1742
	[SerializeField]
	private Canvas[] canvases;

	// Token: 0x040006CF RID: 1743
	[SerializeField]
	private AudioSource[] audioSources;

	// Token: 0x040006D0 RID: 1744
	private Canvas rootCanvas;

	// Token: 0x040006D1 RID: 1745
	private GraphicRaycaster cachedCaster;

	// Token: 0x040006D2 RID: 1746
	[SerializeField]
	private RectTransform root;
}
