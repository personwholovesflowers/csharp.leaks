using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000457 RID: 1111
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class StyleHUD : MonoSingleton<StyleHUD>
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06001963 RID: 6499 RVA: 0x000D0FDE File Offset: 0x000CF1DE
	public StyleRank currentRank
	{
		get
		{
			return this.ranks[this.rankIndex];
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06001964 RID: 6500 RVA: 0x000D0FF1 File Offset: 0x000CF1F1
	// (set) Token: 0x06001965 RID: 6501 RVA: 0x000D0FF9 File Offset: 0x000CF1F9
	public int rankIndex
	{
		get
		{
			return this._rankIndex;
		}
		private set
		{
			this._rankIndex = Mathf.Clamp(value, 0, this.ranks.Count - 1);
			this.rankImage.sprite = this.currentRank.sprite;
		}
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000D102C File Offset: 0x000CF22C
	public string GetLocalizedName(string id)
	{
		string text;
		if (!this.idNameDict.TryGetValue(id, out text))
		{
			return id;
		}
		return text;
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06001967 RID: 6503 RVA: 0x000D104C File Offset: 0x000CF24C
	private bool freshnessEnabled
	{
		get
		{
			return MonoSingleton<AssistController>.Instance == null || !MonoSingleton<AssistController>.Instance.majorEnabled || !MonoSingleton<AssistController>.Instance.disableWeaponFreshness;
		}
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000D1078 File Offset: 0x000CF278
	private void Start()
	{
		this.styleHud = base.transform.GetChild(0).gameObject;
		this.styleSlider = base.GetComponentInChildren<Slider>();
		this.styleInfo = base.GetComponentInChildren<TMP_Text>();
		this.freshnessStateDict = this.freshnessStateData.ToDictionary((StyleFreshnessData x) => x.state, (StyleFreshnessData x) => x);
		this.sman = MonoSingleton<StatsManager>.Instance;
		this.gc = MonoSingleton<GunControl>.Instance;
		this.weaponFreshness.Clear();
		foreach (GameObject gameObject in this.gc.allWeapons)
		{
			if (gameObject != null)
			{
				this.weaponFreshness.Add(gameObject, 10f);
			}
		}
		foreach (StyleFreshnessData styleFreshnessData in this.freshnessStateData)
		{
			styleFreshnessData.slider.minValue = styleFreshnessData.min;
			styleFreshnessData.slider.maxValue = styleFreshnessData.max;
		}
		this.ComboOver();
		this.defaultPos = this.rankImage.transform.localPosition;
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x000D1208 File Offset: 0x000CF408
	protected override void Awake()
	{
		base.Awake();
		this.defaultPos = this.rankImage.transform.localPosition;
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000D1226 File Offset: 0x000CF426
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.updateItemsRoutine != null)
		{
			base.StopCoroutine(this.updateItemsRoutine);
		}
		this.updateItemsRoutine = base.StartCoroutine(this.UpdateItems());
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x000D1254 File Offset: 0x000CF454
	private void OnDisable()
	{
		if (this.updateItemsRoutine != null)
		{
			base.StopCoroutine(this.updateItemsRoutine);
		}
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x000D126A File Offset: 0x000CF46A
	private void Update()
	{
		this.UpdateMeter();
		this.UpdateFreshness();
		this.UpdateHUD();
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000D127E File Offset: 0x000CF47E
	private IEnumerator UpdateItems()
	{
		for (;;)
		{
			if (this.hudItemsQueue.Count > 0)
			{
				string text = this.hudItemsQueue.Dequeue();
				this.styleInfo.text = text + "\n" + this.styleInfo.text;
				AudioSource audioSource = this.aud;
				if (audioSource != null)
				{
					audioSource.Play();
				}
				base.Invoke("RemoveText", 3f);
				yield return this.styleWait;
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x000D1290 File Offset: 0x000CF490
	private void UpdateMeter()
	{
		if (this.currentMeter > 0f && !this.comboActive)
		{
			this.ComboStart();
		}
		if (this.currentMeter < 0f)
		{
			this.DescendRank();
		}
		else
		{
			this.currentMeter -= Time.deltaTime * (this.currentRank.drainSpeed * 15f);
		}
		bool flag = this.comboActive || this.forceMeterOn;
		if (this.styleHud.activeSelf != flag)
		{
			this.styleHud.SetActive(flag);
		}
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000D1320 File Offset: 0x000CF520
	private void UpdateFreshness()
	{
		if (!this.comboActive)
		{
			return;
		}
		if (!this.freshnessEnabled)
		{
			return;
		}
		if (!this.gc.activated)
		{
			return;
		}
		foreach (GameObject gameObject in this.gc.allWeapons)
		{
			if (gameObject == this.gc.currentWeapon)
			{
				this.AddFreshness(gameObject, -this.freshnessDecayPerSec * Time.deltaTime);
				ValueTuple<float, float> valueTuple;
				if (this.slotFreshnessLock.TryGetValue(this.gc.currentSlotIndex, out valueTuple))
				{
					this.weaponFreshness[gameObject] = Mathf.Clamp(this.weaponFreshness[gameObject], valueTuple.Item1, valueTuple.Item2);
				}
			}
			else
			{
				this.AddFreshness(gameObject, this.freshnessRegenPerSec * Time.deltaTime);
			}
		}
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x000D1418 File Offset: 0x000CF618
	private void UpdateHUD()
	{
		this.styleSlider.value = this.currentMeter / (float)this.currentRank.maxMeter;
		if (this.freshnessEnabled)
		{
			if (!this.freshnessSliderText.gameObject.activeSelf)
			{
				this.freshnessSliderText.gameObject.SetActive(true);
			}
			if (!this.gc.currentWeapon)
			{
				goto IL_01A7;
			}
			float num;
			bool flag = this.weaponFreshness.TryGetValue(this.gc.currentWeapon, out num);
			if (!flag)
			{
				Debug.LogWarning("Current weapon not in StyleHUD weaponFreshness dict!!!");
			}
			float num2 = 30f * Time.deltaTime;
			using (Dictionary<StyleFreshnessState, StyleFreshnessData>.Enumerator enumerator = this.freshnessStateDict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<StyleFreshnessState, StyleFreshnessData> keyValuePair = enumerator.Current;
					Slider slider = keyValuePair.Value.slider;
					if (this.gc.activated)
					{
						if (slider != null && this.gc != null && this.gc.allWeapons.Count > 0 && this.gc.currentWeapon != null && flag)
						{
							this.freshnessSliderValue = Mathf.Lerp(this.freshnessSliderValue, num, num2);
						}
						slider.value = this.freshnessSliderValue;
					}
				}
				goto IL_01A7;
			}
		}
		if (this.freshnessSliderText.gameObject.activeSelf)
		{
			this.freshnessSliderText.gameObject.SetActive(false);
			foreach (StyleFreshnessData styleFreshnessData in this.freshnessStateData)
			{
				styleFreshnessData.slider.gameObject.SetActive(false);
			}
		}
		IL_01A7:
		if (this.styleNameTime > 0f)
		{
			this.styleNameTime = Mathf.MoveTowards(this.styleNameTime, 0f, Time.deltaTime * 2f);
		}
		else
		{
			this.styleNameTime = 0.1f;
		}
		if (this.rankShaking > 0f)
		{
			this.rankImage.transform.localPosition = new Vector3(this.defaultPos.x + this.rankShaking * (float)Random.Range(-3, 3), this.defaultPos.y + this.rankShaking * (float)Random.Range(-3, 3), this.defaultPos.z);
			this.rankShaking -= Time.deltaTime * 5f;
		}
		else
		{
			this.rankImage.transform.localPosition = this.defaultPos;
		}
		if (this.rankScale > 0f)
		{
			this.rankImage.transform.localScale = new Vector3(1f, 1f, 1f) + Vector3.one * this.rankScale;
			this.rankScale -= Time.deltaTime;
			return;
		}
		this.rankImage.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000D1738 File Offset: 0x000CF938
	public void RegisterStyleItem(string id, string name)
	{
		this.idNameDict.Add(id, name);
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000D1747 File Offset: 0x000CF947
	public void ComboStart()
	{
		base.CancelInvoke("ResetFreshness");
		this.currentMeter = Mathf.Max(this.currentMeter, (float)(this.currentRank.maxMeter / 4));
		this.comboActive = true;
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x000D177A File Offset: 0x000CF97A
	public void ComboOver()
	{
		this.currentMeter = 0f;
		this.rankIndex = 0;
		base.Invoke("ResetFreshness", 10f);
		this.comboActive = false;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000D17A8 File Offset: 0x000CF9A8
	private void AscendRank()
	{
		while (this.currentMeter >= (float)this.currentRank.maxMeter)
		{
			this.currentMeter -= (float)this.currentRank.maxMeter;
			int rankIndex = this.rankIndex;
			this.rankIndex = rankIndex + 1;
			if (this.rankIndex + 1 == this.ranks.Count - 1)
			{
				break;
			}
		}
		this.currentMeter = Mathf.Max(this.currentMeter, (float)(this.currentRank.maxMeter / 4));
		this.maxReachedRank = Mathf.Max(this.maxReachedRank, this.rankIndex);
		DiscordController.UpdateRank(this.rankIndex);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000D184C File Offset: 0x000CFA4C
	private void UpdateFreshnessSlider()
	{
		StyleFreshnessState freshnessState = this.GetFreshnessState(this.gc.currentWeapon);
		this.freshnessSliderText.text = this.freshnessStateDict[freshnessState].text;
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000D1888 File Offset: 0x000CFA88
	public void ResetFreshness()
	{
		this.gc = this.gc ?? MonoSingleton<GunControl>.Instance;
		foreach (GameObject gameObject in this.gc.allWeapons)
		{
			this.weaponFreshness[gameObject] = 10f;
		}
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000D1900 File Offset: 0x000CFB00
	public void SnapFreshnessSlider()
	{
		if (this.gc == null || this.gc.currentWeapon == null)
		{
			return;
		}
		float num;
		if (this.weaponFreshness.TryGetValue(this.gc.currentWeapon, out num))
		{
			this.freshnessSliderValue = num;
		}
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x000D1950 File Offset: 0x000CFB50
	public StyleFreshnessState GetFreshnessState(GameObject sourceWeapon)
	{
		StyleFreshnessState styleFreshnessState = StyleFreshnessState.Dull;
		float num;
		if (!this.weaponFreshness.TryGetValue(sourceWeapon, out num))
		{
			Debug.LogWarning("Current weapon not in StyleHUD weaponFreshness dict!!!");
			return StyleFreshnessState.Fresh;
		}
		foreach (KeyValuePair<StyleFreshnessState, StyleFreshnessData> keyValuePair in this.freshnessStateDict)
		{
			if (num >= keyValuePair.Value.min)
			{
				styleFreshnessState = keyValuePair.Key;
			}
		}
		return styleFreshnessState;
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x000D19D4 File Offset: 0x000CFBD4
	public void LockFreshness(int slot, float? min = null, float? max = null)
	{
		ValueTuple<float, float> valueTuple;
		if (this.slotFreshnessLock.TryGetValue(slot, out valueTuple))
		{
			this.slotFreshnessLock[slot] = new ValueTuple<float, float>(min ?? valueTuple.Item1, max ?? valueTuple.Item2);
			return;
		}
		this.slotFreshnessLock.Add(slot, new ValueTuple<float, float>(min.GetValueOrDefault(), max ?? 10f));
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000D1A68 File Offset: 0x000CFC68
	public void LockFreshness(int slot, StyleFreshnessState? minState = null, StyleFreshnessState? maxState = null)
	{
		StyleFreshnessData styleFreshnessData = ((maxState != null) ? this.freshnessStateDict[maxState.Value] : null);
		StyleFreshnessData styleFreshnessData2 = ((minState != null) ? this.freshnessStateDict[minState.Value] : null);
		float num = 0f;
		float num2 = 10f;
		if (styleFreshnessData2 != null)
		{
			num = styleFreshnessData2.justAboveMin;
		}
		if (styleFreshnessData != null)
		{
			num2 = styleFreshnessData.max - 0.01f;
		}
		this.LockFreshness(slot, new float?(num), new float?(num2));
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000D1AEC File Offset: 0x000CFCEC
	public void UnlockFreshness(int slot)
	{
		this.slotFreshnessLock.Remove(slot);
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000D1AFC File Offset: 0x000CFCFC
	private void ClampFreshness(GameObject sourceWeapon, float amt)
	{
		float num = 10f;
		this.UpdateMinFreshnessCache(this.gc.allWeapons.Count);
		float num2 = this.minFreshnessCache;
		ValueTuple<float, float> valueTuple;
		if (sourceWeapon == this.gc.currentWeapon && this.slotFreshnessLock.TryGetValue(this.gc.currentSlotIndex, out valueTuple))
		{
			num2 = Mathf.Max(num2, valueTuple.Item1);
			num = valueTuple.Item2;
		}
		this.weaponFreshness[sourceWeapon] = Mathf.Clamp(amt, num2, num);
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000D1B84 File Offset: 0x000CFD84
	public void UpdateMinFreshnessCache(int count)
	{
		if (this.weaponCountCache != count)
		{
			this.weaponCountCache = count;
			if (SummonSandboxArm.armSlot.Count > 0)
			{
				count--;
			}
			if (count <= 1)
			{
				this.minFreshnessCache = this.freshnessStateDict[StyleFreshnessState.Fresh].max;
				return;
			}
			if (count == 2)
			{
				this.minFreshnessCache = this.freshnessStateDict[StyleFreshnessState.Used].justAboveMin;
				return;
			}
			if (count == 3 || count == 4)
			{
				this.minFreshnessCache = this.freshnessStateDict[StyleFreshnessState.Stale].justAboveMin;
				return;
			}
			this.minFreshnessCache = this.freshnessStateDict[StyleFreshnessState.Dull].justAboveMin;
		}
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000D1C25 File Offset: 0x000CFE25
	public float GetFreshness(GameObject sourceWeapon)
	{
		return this.weaponFreshness[sourceWeapon];
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x000D1C33 File Offset: 0x000CFE33
	public void SetFreshness(GameObject sourceWeapon, float amt)
	{
		this.ClampFreshness(sourceWeapon, amt);
		GunControl gunControl = this.gc;
		if (sourceWeapon == ((gunControl != null) ? gunControl.currentWeapon : null))
		{
			this.UpdateFreshnessSlider();
		}
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x000D1C60 File Offset: 0x000CFE60
	public void AddFreshness(GameObject sourceWeapon, float amt)
	{
		float num = amt;
		int dualWieldCount = this.gc.dualWieldCount;
		if (this.dualWieldScale && dualWieldCount > 0)
		{
			num /= (float)(dualWieldCount + 1);
		}
		this.SetFreshness(sourceWeapon, this.GetFreshness(sourceWeapon) + num);
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x000D1CA0 File Offset: 0x000CFEA0
	public void DecayFreshness(GameObject sourceWeapon, string pointID, bool boss)
	{
		float num;
		if (!this.weaponFreshness.TryGetValue(sourceWeapon, out num))
		{
			Debug.LogWarning(string.Format("Weapon {0} not in StyleHUD weaponFreshness dict", sourceWeapon));
			return;
		}
		float num2 = this.freshnessDecayPerMove;
		float num3 = (float)this.gc.dualWieldCount;
		if (this.dualWieldScale && num3 > 0f)
		{
			num2 /= num3 + 1f;
		}
		float num4;
		if (this.freshnessDecayMultiplierDict.TryGetValue(pointID, out num4))
		{
			num2 *= num4;
		}
		if (boss)
		{
			num2 *= this.bossFreshnessDecayMultiplier;
		}
		this.SetFreshness(sourceWeapon, num - num2);
		int num5 = this.gc.slotDict[sourceWeapon];
		foreach (GameObject gameObject in this.gc.allWeapons)
		{
			if (!(gameObject == sourceWeapon) && this.gc.slotDict[gameObject] != num5)
			{
				float num6 = this.freshnessRegenPerMove;
				if (num4 > 0f)
				{
					num6 *= num4;
				}
				this.AddFreshness(gameObject, num6);
			}
		}
		GunControl gunControl = this.gc;
		if (sourceWeapon == ((gunControl != null) ? gunControl.currentWeapon : null))
		{
			this.UpdateFreshnessSlider();
		}
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x000D1DE0 File Offset: 0x000CFFE0
	public void DescendRank()
	{
		if (!this.comboActive)
		{
			return;
		}
		if (this.rankIndex > 0)
		{
			this.currentMeter = (float)this.currentRank.maxMeter;
			int rankIndex = this.rankIndex;
			this.rankIndex = rankIndex - 1;
			this.rankImage.sprite = this.ranks[this.rankIndex].sprite;
			this.currentMeter = (float)(this.currentRank.maxMeter - this.currentRank.maxMeter / 4);
		}
		else if (this.rankIndex == 0)
		{
			this.ComboOver();
		}
		DiscordController.UpdateRank(this.rankIndex);
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x000D1E80 File Offset: 0x000D0080
	public void AddPoints(int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
	{
		GameObject gameObject = ((pointID == "ultrakill.arsenal") ? this.gc.currentWeapon : sourceWeapon);
		if (eid && eid.puppet)
		{
			return;
		}
		bool flag = false;
		if (eid)
		{
			flag = eid.isBoss;
		}
		if (points > 0)
		{
			float num = (float)points;
			if (this.freshnessEnabled && gameObject != null)
			{
				StyleFreshnessState freshnessState = this.GetFreshnessState(gameObject);
				num *= this.freshnessStateDict[freshnessState].scoreMultiplier;
				this.DecayFreshness(gameObject, pointID, flag);
			}
			if (flag)
			{
				num *= this.bossStyleGainMultiplier;
			}
			this.sman.stylePoints += Mathf.RoundToInt(num);
			this.currentMeter += num;
			this.rankScale = 0.2f;
		}
		string localizedName = this.GetLocalizedName(pointID);
		if (localizedName != "")
		{
			if (count >= 0)
			{
				this.hudItemsQueue.Enqueue(string.Concat(new object[] { "+ ", prefix, localizedName, postfix, " x", count }));
			}
			else
			{
				this.hudItemsQueue.Enqueue("+ " + prefix + localizedName + postfix);
			}
		}
		if (this.currentMeter >= (float)this.currentRank.maxMeter && this.rankIndex < 7)
		{
			this.AscendRank();
			return;
		}
		if (this.currentMeter > (float)this.currentRank.maxMeter)
		{
			this.currentMeter = (float)this.currentRank.maxMeter;
		}
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000D2009 File Offset: 0x000D0209
	public void RemovePoints(int points)
	{
		this.rankShaking = 5f;
		this.currentMeter -= (float)points;
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x000D2025 File Offset: 0x000D0225
	public void ResetFreshness(GameObject weapon)
	{
		if (this.weaponFreshness.ContainsKey(weapon))
		{
			this.weaponFreshness[weapon] = 10f;
		}
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x000D2048 File Offset: 0x000D0248
	public void ResetAllFreshness()
	{
		foreach (GameObject gameObject in this.gc.allWeapons)
		{
			this.ResetFreshness(gameObject);
		}
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x000D20A0 File Offset: 0x000D02A0
	private void RemoveText()
	{
		this.styleInfo.text = this.styleInfo.text.Substring(0, this.styleInfo.text.LastIndexOf("+"));
	}

	// Token: 0x040023A0 RID: 9120
	public Image rankImage;

	// Token: 0x040023A1 RID: 9121
	public List<StyleRank> ranks;

	// Token: 0x040023A2 RID: 9122
	public bool showStyleMeter;

	// Token: 0x040023A3 RID: 9123
	public bool forceMeterOn;

	// Token: 0x040023A4 RID: 9124
	private int _rankIndex;

	// Token: 0x040023A5 RID: 9125
	public int maxReachedRank;

	// Token: 0x040023A6 RID: 9126
	private Queue<string> hudItemsQueue = new Queue<string>();

	// Token: 0x040023A7 RID: 9127
	private float currentMeter;

	// Token: 0x040023A8 RID: 9128
	private GameObject styleHud;

	// Token: 0x040023A9 RID: 9129
	private Slider styleSlider;

	// Token: 0x040023AA RID: 9130
	private TMP_Text styleInfo;

	// Token: 0x040023AB RID: 9131
	private float rankShaking;

	// Token: 0x040023AC RID: 9132
	private Vector3 defaultPos;

	// Token: 0x040023AD RID: 9133
	private float rankScale;

	// Token: 0x040023AE RID: 9134
	private bool comboActive;

	// Token: 0x040023AF RID: 9135
	private StatsManager sman;

	// Token: 0x040023B0 RID: 9136
	private GunControl gc;

	// Token: 0x040023B1 RID: 9137
	private float styleNameTime = 0.1f;

	// Token: 0x040023B2 RID: 9138
	private AudioSource aud;

	// Token: 0x040023B3 RID: 9139
	[Header("Multipliers")]
	public float bossStyleGainMultiplier = 1.5f;

	// Token: 0x040023B4 RID: 9140
	public float bossFreshnessDecayMultiplier = 1.5f;

	// Token: 0x040023B5 RID: 9141
	[Header("Freshness")]
	public bool dualWieldScale;

	// Token: 0x040023B6 RID: 9142
	public float freshnessDecayPerMove = 0.5f;

	// Token: 0x040023B7 RID: 9143
	public float freshnessDecayPerSec = 0.25f;

	// Token: 0x040023B8 RID: 9144
	[Space]
	public float freshnessRegenPerMove = 1f;

	// Token: 0x040023B9 RID: 9145
	public float freshnessRegenPerSec = 0.5f;

	// Token: 0x040023BA RID: 9146
	[Space]
	public List<StyleFreshnessData> freshnessStateData = new List<StyleFreshnessData>();

	// Token: 0x040023BB RID: 9147
	private Dictionary<StyleFreshnessState, StyleFreshnessData> freshnessStateDict;

	// Token: 0x040023BC RID: 9148
	public TMP_Text freshnessSliderText;

	// Token: 0x040023BD RID: 9149
	private float freshnessSliderValue;

	// Token: 0x040023BE RID: 9150
	private Dictionary<GameObject, float> weaponFreshness = new Dictionary<GameObject, float>();

	// Token: 0x040023BF RID: 9151
	private float minFreshnessCache;

	// Token: 0x040023C0 RID: 9152
	private int weaponCountCache;

	// Token: 0x040023C1 RID: 9153
	private Dictionary<int, ValueTuple<float, float>> slotFreshnessLock = new Dictionary<int, ValueTuple<float, float>>();

	// Token: 0x040023C2 RID: 9154
	public Dictionary<string, float> freshnessDecayMultiplierDict = new Dictionary<string, float>
	{
		{ "ultrakill.shotgunhit", 0.15f },
		{ "ultrakill.nailhit", 0.1f },
		{ "ultrakill.explosionhit", 0.75f },
		{ "ultrakill.exploded", 1.25f },
		{ "ultrakill.kill", 0.3f },
		{ "ultrakill.firehit", 0f },
		{ "ultrakill.quickdraw", 0f },
		{ "ultrakill.projectileboost", 0f },
		{ "ultrakill.doublekill", 0.1f },
		{ "ultrakill.triplekill", 0.1f },
		{ "ultrakill.multikill", 0.1f },
		{ "ultrakill.arsenal", 0f },
		{ "ultrakill.drillhit", 0.025f },
		{ "ultrakill.drillpunch", 1f },
		{ "ultrakill.drillpunchkill", 1f },
		{ "ultrakill.hammerhit", 3f },
		{ "ultrakill.hammerhitheavy", 6f },
		{ "ultrakill.hammerhitred", 8f },
		{ "ultrakill.hammerhityellow", 5f },
		{ "ultrakill.hammerhitgreen", 3f }
	};

	// Token: 0x040023C3 RID: 9155
	private Dictionary<string, string> idNameDict = new Dictionary<string, string>
	{
		{ "ultrakill.kill", "KILL" },
		{ "ultrakill.doublekill", "<color=orange>DOUBLE KILL</color>" },
		{ "ultrakill.triplekill", "<color=orange>TRIPLE KILL</color>" },
		{ "ultrakill.bigkill", "BIG KILL" },
		{ "ultrakill.bigfistkill", "BIG FISTKILL" },
		{ "ultrakill.headshot", "HEADSHOT" },
		{ "ultrakill.bigheadshot", "BIG HEADSHOT" },
		{ "ultrakill.headshotcombo", "<color=#00ffffff>HEADSHOT COMBO</color>" },
		{ "ultrakill.criticalpunch", "CRITICAL PUNCH" },
		{ "ultrakill.ricoshot", "<color=#00ffffff>RICOSHOT</color>" },
		{ "ultrakill.limbhit", "LIMB HIT" },
		{ "ultrakill.secret", "<color=#00ffffff>SECRET</color>" },
		{ "ultrakill.cannonballed", "CANNONBALLED" },
		{ "ultrakill.cannonballedfrombounce", "<color=green>DUNKED</color>" },
		{ "ultrakill.cannonboost", "<color=green>CANNONBOOST</color>" },
		{ "ultrakill.insurrknockdown", "<color=green>TIME OUT</color>" },
		{ "ultrakill.quickdraw", "<color=#00ffffff>QUICKDRAW</color>" },
		{ "ultrakill.interruption", "<color=green>INTERRUPTION</color>" },
		{ "ultrakill.fistfullofdollar", "<color=#00ffffff>FISTFUL OF DOLLAR</color>" },
		{ "ultrakill.homerun", "HOMERUN" },
		{ "ultrakill.arsenal", "<color=#00ffffff>ARSENAL</color>" },
		{ "ultrakill.catapulted", "<color=#00ffffff>CATAPULTED</color>" },
		{ "ultrakill.splattered", "SPLATTERED" },
		{ "ultrakill.enraged", "<color=red>ENRAGED</color>" },
		{ "ultrakill.instakill", "<color=green>INSTAKILL</color>" },
		{ "ultrakill.fireworks", "<color=#00ffffff>FIREWORKS</color>" },
		{ "ultrakill.fireworksweak", "<color=#00ffffff>JUGGLE</color>" },
		{ "ultrakill.airslam", "<color=#00ffffff>AIR SLAM</color>" },
		{ "ultrakill.airshot", "<color=#00ffffff>AIRSHOT</color>" },
		{ "ultrakill.downtosize", "<color=#00ffffff>DOWN TO SIZE</color>" },
		{ "ultrakill.projectileboost", "<color=green>PROJECTILE BOOST</color>" },
		{ "ultrakill.parry", "<color=green>PARRY</color>" },
		{ "ultrakill.chargeback", "CHARGEBACK" },
		{ "ultrakill.disrespect", "DISRESPECT" },
		{ "ultrakill.groundslam", "GROUND SLAM" },
		{ "ultrakill.overkill", "OVERKILL" },
		{ "ultrakill.friendlyfire", "FRIENDLY FIRE" },
		{ "ultrakill.exploded", "EXPLODED" },
		{ "ultrakill.fried", "FRIED" },
		{ "ultrakill.finishedoff", "<color=#00ffffff>FINISHED OFF</color>" },
		{ "ultrakill.halfoff", "<color=#00ffffff>HALF OFF</color>" },
		{ "ultrakill.mauriced", "MAURICED" },
		{ "ultrakill.bipolar", "BIPOLAR" },
		{ "ultrakill.attripator", "<color=#00ffffff>ATTRAPTOR</color>" },
		{ "ultrakill.nailbombed", "NAILBOMBED" },
		{ "ultrakill.nailbombedalive", "<color=grey>NAILBOMBED</color>" },
		{ "ultrakill.multikill", "<color=orange>MULTIKILL</color>" },
		{ "ultrakill.shotgunhit", "" },
		{ "ultrakill.nailhit", "" },
		{ "ultrakill.explosionhit", "" },
		{ "ultrakill.firehit", "" },
		{ "ultrakill.zapperhit", "" },
		{ "ultrakill.compressed", "COMPRESSED" },
		{ "ultrakill.strike", "<color=#00ffffff>STRIKE!</color>" },
		{ "ultrakill.rocketreturn", "<color=#00ffffff>ROCKET RETURN</color>" },
		{ "ultrakill.roundtrip", "<color=green>ROUND TRIP</color>" },
		{ "ultrakill.serve", "<color=#00ffffff>SERVED</color>" },
		{ "ultrakill.landyours", "<color=green>LANDYOURS</color>" },
		{ "ultrakill.iconoclasm", "ICONOCLASM" },
		{ "ultrakill.drillhit", "" },
		{ "ultrakill.drillpunch", "<color=green>CORKSCREW BLOW</color>" },
		{ "ultrakill.drillpunchkill", "<color=green>GIGA DRILL BREAK</color>" },
		{ "ultrakill.hammerhit", "" },
		{ "ultrakill.hammerhitheavy", "BLASTING AWAY" },
		{ "ultrakill.hammerhitred", "FULL IMPACT" },
		{ "ultrakill.hammerhityellow", "HEAVY HITTER" },
		{ "ultrakill.hammerhitgreen", "BLUNT FORCE" },
		{ "ultrakill.lightningbolt", "<color=green>RIDE THE LIGHTNING</color>" }
	};

	// Token: 0x040023C4 RID: 9156
	private Coroutine updateItemsRoutine;

	// Token: 0x040023C5 RID: 9157
	private WaitForSeconds styleWait = new WaitForSeconds(0.05f);
}
