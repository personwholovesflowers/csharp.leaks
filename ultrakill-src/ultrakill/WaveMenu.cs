using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class WaveMenu : MonoBehaviour
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000789 RID: 1929 RVA: 0x00032661 File Offset: 0x00030861
	// (set) Token: 0x0600078A RID: 1930 RVA: 0x00032669 File Offset: 0x00030869
	public int highestWave
	{
		get
		{
			return this._highestWave;
		}
		private set
		{
			this._highestWave = value;
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00032674 File Offset: 0x00030874
	private void Start()
	{
		int? highestWaveForDifficulty = WaveUtils.GetHighestWaveForDifficulty(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
		foreach (WaveSetter waveSetter in this.setters)
		{
			if (WaveUtils.IsWaveSelectable(waveSetter.wave, highestWaveForDifficulty.GetValueOrDefault()))
			{
				waveSetter.state = ButtonState.Unselected;
			}
			else if (waveSetter.wave == this.startWave)
			{
				waveSetter.state = ButtonState.Selected;
			}
			else
			{
				waveSetter.state = ButtonState.Locked;
			}
		}
		if (highestWaveForDifficulty == null)
		{
			this.customSetter.gameObject.SetActive(false);
			this.SetCurrentWave(0);
			return;
		}
		this.highestWave = highestWaveForDifficulty.Value;
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.startingWave", 0);
		this.startWave = (WaveUtils.IsWaveSelectable(@int, this.highestWave) ? @int : 0);
		if (this.highestWave >= 60)
		{
			this.customSetter.gameObject.SetActive(true);
			this.customSetter.wave = ((this.startWave >= 30) ? this.startWave : 30);
		}
		else
		{
			this.customSetter.gameObject.SetActive(false);
		}
		this.SetCurrentWave(this.startWave);
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x000327C4 File Offset: 0x000309C4
	private void GetHighestWave()
	{
		int? highestWaveForDifficulty = WaveUtils.GetHighestWaveForDifficulty(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
		if (highestWaveForDifficulty == null)
		{
			return;
		}
		this.highestWave = highestWaveForDifficulty.Value;
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.startingWave", 0);
		Debug.Log("Wanted wave: " + @int.ToString());
		this.startWave = (WaveUtils.IsWaveSelectable(@int, this.highestWave) ? @int : 0);
		MonoSingleton<EndlessGrid>.Instance.startWave = this.startWave;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00032850 File Offset: 0x00030A50
	public void SetCurrentWave(int wave)
	{
		if (wave * 2 > this.highestWave)
		{
			return;
		}
		this.startWave = wave;
		MonoSingleton<EndlessGrid>.Instance.startWave = this.startWave;
		MonoSingleton<PrefsManager>.Instance.SetInt("cyberGrind.startingWave", wave);
		foreach (WaveSetter waveSetter in this.setters)
		{
			if (waveSetter.state != ButtonState.Locked)
			{
				waveSetter.state = ((waveSetter.wave == wave) ? ButtonState.Selected : ButtonState.Unselected);
			}
		}
		this.customSetter.state = ((wave >= 30) ? WaveCustomSetter.ButtonState.Selected : WaveCustomSetter.ButtonState.Unselected);
	}

	// Token: 0x040009C6 RID: 2502
	public List<WaveSetter> setters = new List<WaveSetter>();

	// Token: 0x040009C7 RID: 2503
	public WaveCustomSetter customSetter;

	// Token: 0x040009C8 RID: 2504
	private int _highestWave = -1;

	// Token: 0x040009C9 RID: 2505
	private int startWave;
}
