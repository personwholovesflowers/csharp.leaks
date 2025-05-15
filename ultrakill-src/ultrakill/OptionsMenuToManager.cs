using System;
using System.Collections.Generic;
using SettingsMenu.Components;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000326 RID: 806
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class OptionsMenuToManager : MonoSingleton<OptionsMenuToManager>
{
	// Token: 0x0600129D RID: 4765 RVA: 0x00094D91 File Offset: 0x00092F91
	private void Start()
	{
		this.SetPauseMenu();
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x00094D99 File Offset: 0x00092F99
	protected override void OnEnable()
	{
		base.OnEnable();
		this.SetPauseMenu();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x00094DC7 File Offset: 0x00092FC7
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x00094DE9 File Offset: 0x00092FE9
	private void OnPrefChanged(string key, object value)
	{
		if (this.optionsMenu == null)
		{
			return;
		}
		this.optionsMenu.OnPrefChanged(key, value);
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x00094E08 File Offset: 0x00093008
	private void SetPauseMenu()
	{
		this.opm = MonoSingleton<OptionsManager>.Instance;
		if (this.opm.pauseMenu)
		{
			if (this.opm.pauseMenu == this.pauseMenu)
			{
				return;
			}
			this.opm.pauseMenu.SetActive(false);
			this.opm.optionsMenu.gameObject.SetActive(false);
		}
		this.opm.pauseMenu = this.pauseMenu;
		this.opm.optionsMenu = this.optionsMenu;
		this.optionsMenu.Initialize();
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x00094E9F File Offset: 0x0009309F
	public void EnableGamepadLookAndMove()
	{
		this.EnableGamepadLook();
		this.EnableGamepadMove();
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x00094EAD File Offset: 0x000930AD
	public void DisableGamepadLookAndMove()
	{
		this.DisableGamepadLook();
		this.DisableGamepadMove();
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x00094EBB File Offset: 0x000930BB
	public void EnableGamepadMove()
	{
		if (MonoSingleton<NewMovement>.Instance.gamepadFreezeCount > 0)
		{
			MonoSingleton<NewMovement>.Instance.gamepadFreezeCount--;
		}
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x00094EDC File Offset: 0x000930DC
	public void EnableGamepadLook()
	{
		if (MonoSingleton<CameraController>.Instance.gamepadFreezeCount > 0)
		{
			MonoSingleton<CameraController>.Instance.gamepadFreezeCount--;
		}
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x00094EFD File Offset: 0x000930FD
	public void DisableGamepadMove()
	{
		MonoSingleton<NewMovement>.Instance.gamepadFreezeCount++;
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x00094F11 File Offset: 0x00093111
	public void DisableGamepadLook()
	{
		MonoSingleton<CameraController>.Instance.gamepadFreezeCount++;
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x00094F25 File Offset: 0x00093125
	public void SetSelected(Selectable selectable)
	{
		SettingsMenu.SetSelected(selectable);
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x00094F2D File Offset: 0x0009312D
	public void Pause()
	{
		this.opm.Pause();
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x00094F3A File Offset: 0x0009313A
	public void UnPause()
	{
		this.opm.UnPause();
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x00094F47 File Offset: 0x00093147
	public void RestartCheckpoint()
	{
		this.opm.RestartCheckpoint();
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x00094F54 File Offset: 0x00093154
	public void RestartMission()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("pauseMenuConfirmationDialogs", 0);
		string currentScene = SceneHelper.CurrentScene;
		if (@int == 0 || (@int == 1 && currentScene == "Endless"))
		{
			this.resetDialog.ShowDialog();
			return;
		}
		this.RestartMissionNoConfirm();
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x00094FA4 File Offset: 0x000931A4
	public void RestartMissionNoConfirm()
	{
		this.opm.RestartMission();
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x00094FB1 File Offset: 0x000931B1
	public void OpenOptions()
	{
		this.opm.OpenOptions();
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x00094FBE File Offset: 0x000931BE
	public void CloseOptions()
	{
		this.opm.CloseOptions();
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x00094FCC File Offset: 0x000931CC
	public void QuitMission()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("pauseMenuConfirmationDialogs", 0);
		string currentScene = SceneHelper.CurrentScene;
		if (@int == 0 || (@int == 1 && currentScene == "Endless"))
		{
			this.quitDialog.ShowDialog();
			return;
		}
		this.QuitMissionNoConfirm();
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x0009501C File Offset: 0x0009321C
	public void QuitMissionNoConfirm()
	{
		this.opm.QuitMission();
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x00095029 File Offset: 0x00093229
	public void QuitGame()
	{
		this.opm.QuitGame();
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x00095036 File Offset: 0x00093236
	public void CheckIfTutorialBeaten()
	{
		if (!GameProgressSaver.GetTutorial())
		{
			SceneHelper.LoadScene("Tutorial", false);
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0009504A File Offset: 0x0009324A
	public void ChangeLevel(string levelname)
	{
		this.opm.ChangeLevel(levelname);
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x00095058 File Offset: 0x00093258
	public void MasterVolume(float stuff)
	{
		MonoSingleton<PrefsManager>.Instance.SetFloat("allVolume", stuff / 100f);
		AudioListener.volume = stuff / 100f;
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0009507C File Offset: 0x0009327C
	public void SFXVolume(float stuff)
	{
		MonoSingleton<PrefsManager>.Instance.SetFloat("sfxVolume", stuff / 100f);
		if (MonoSingleton<AudioMixerController>.Instance)
		{
			MonoSingleton<AudioMixerController>.Instance.SetSFXVolume(stuff / 100f);
		}
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x000950B4 File Offset: 0x000932B4
	public void MusicVolume(float stuff)
	{
		MonoSingleton<PrefsManager>.Instance.SetFloat("musicVolume", stuff / 100f);
		if (MonoSingleton<AudioMixerController>.Instance)
		{
			MonoSingleton<AudioMixerController>.Instance.optionsMusicVolume = stuff / 100f;
			MonoSingleton<AudioMixerController>.Instance.SetMusicVolume(stuff / 100f);
		}
	}

	// Token: 0x04001993 RID: 6547
	public GameObject pauseMenu;

	// Token: 0x04001994 RID: 6548
	public SettingsMenu optionsMenu;

	// Token: 0x04001995 RID: 6549
	private OptionsManager opm;

	// Token: 0x04001996 RID: 6550
	private Camera mainCam;

	// Token: 0x04001997 RID: 6551
	private List<string> options;

	// Token: 0x04001998 RID: 6552
	[Space]
	public BasicConfirmationDialog quitDialog;

	// Token: 0x04001999 RID: 6553
	public BasicConfirmationDialog resetDialog;
}
