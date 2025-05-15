using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class Crossfade : MonoBehaviour
{
	// Token: 0x060004BB RID: 1211 RVA: 0x000202E8 File Offset: 0x0001E4E8
	private void Awake()
	{
		if (!this.multipleTargets)
		{
			if (this.from)
			{
				this.froms = new AudioSource[1];
				this.froms[0] = this.from;
			}
			else
			{
				this.froms = new AudioSource[0];
			}
			if (this.to)
			{
				this.tos = new AudioSource[1];
				this.tos[0] = this.to;
			}
			else
			{
				this.tos = new AudioSource[0];
			}
		}
		if (this.fromMaxVolumes == null || this.fromMaxVolumes.Length == 0)
		{
			this.fromMaxVolumes = new float[this.froms.Length];
		}
		if (this.toOriginalVolumes == null || this.toOriginalVolumes.Length == 0)
		{
			this.toOriginalVolumes = new float[this.tos.Length];
		}
		if (this.toMaxVolumes == null || this.toMaxVolumes.Length == 0)
		{
			this.toMaxVolumes = new float[this.tos.Length];
		}
		if (this.toMinVolumes == null || this.toMinVolumes.Length == 0)
		{
			this.toMinVolumes = new float[this.tos.Length];
		}
		if (this.tos.Length != 0)
		{
			for (int i = 0; i < this.tos.Length; i++)
			{
				this.toOriginalVolumes[i] = this.tos[i].volume;
			}
		}
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00020428 File Offset: 0x0001E628
	private void Start()
	{
		if (!this.dontActivateOnStart && !this.inProgress)
		{
			this.StartFade();
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00020428 File Offset: 0x0001E628
	private void OnEnable()
	{
		if (!this.dontActivateOnStart && !this.inProgress)
		{
			this.StartFade();
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00020440 File Offset: 0x0001E640
	private void Update()
	{
		if (!this.inProgress)
		{
			return;
		}
		this.fadeAmount = Mathf.MoveTowards(this.fadeAmount, 1f, (this.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / this.time);
		if (this.froms.Length != 0)
		{
			for (int i = 0; i < this.froms.Length; i++)
			{
				if (!(this.froms[i] == null))
				{
					this.froms[i].volume = Mathf.Lerp(this.fromMaxVolumes[i], 0f, this.fadeAmount);
				}
			}
		}
		if (this.tos.Length != 0)
		{
			for (int j = 0; j < this.tos.Length; j++)
			{
				if (!(this.tos[j] == null))
				{
					this.tos[j].volume = Mathf.Lerp(this.toMinVolumes[j], this.toMaxVolumes[j], this.fadeAmount);
				}
			}
		}
		if (this.fadeAmount == 1f)
		{
			this.StopFade();
		}
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00020540 File Offset: 0x0001E740
	public void StartFade()
	{
		if (!this.activated)
		{
			this.activated = true;
		}
		else if (this.oneTime)
		{
			return;
		}
		if (this.froms.Length != 0)
		{
			for (int i = 0; i < this.froms.Length; i++)
			{
				if (!(this.froms[i] == null))
				{
					if (MonoSingleton<CrossfadeTracker>.Instance.actives.Count > 0)
					{
						for (int j = MonoSingleton<CrossfadeTracker>.Instance.actives.Count - 1; j >= 0; j--)
						{
							if (!(MonoSingleton<CrossfadeTracker>.Instance.actives[j] == null))
							{
								if (MonoSingleton<CrossfadeTracker>.Instance.actives[j].froms != null && MonoSingleton<CrossfadeTracker>.Instance.actives[j].froms.Length != 0)
								{
									for (int k = MonoSingleton<CrossfadeTracker>.Instance.actives[j].froms.Length - 1; k >= 0; k--)
									{
										if (!(MonoSingleton<CrossfadeTracker>.Instance.actives[j].froms[k] == null) && MonoSingleton<CrossfadeTracker>.Instance.actives[j].froms[k] == this.froms[i])
										{
											MonoSingleton<CrossfadeTracker>.Instance.actives[j].StopFade();
										}
									}
								}
								if (MonoSingleton<CrossfadeTracker>.Instance.actives[j].tos != null && MonoSingleton<CrossfadeTracker>.Instance.actives[j].tos.Length != 0)
								{
									for (int l = MonoSingleton<CrossfadeTracker>.Instance.actives[j].tos.Length - 1; l >= 0; l--)
									{
										if (!(MonoSingleton<CrossfadeTracker>.Instance.actives[j].tos[l] == null) && MonoSingleton<CrossfadeTracker>.Instance.actives[j].tos[l] == this.froms[i])
										{
											MonoSingleton<CrossfadeTracker>.Instance.actives[j].StopFade();
										}
									}
								}
							}
						}
					}
					if (this.fromMaxVolumes != null && this.fromMaxVolumes.Length != 0)
					{
						this.fromMaxVolumes[i] = this.froms[i].volume;
					}
				}
			}
		}
		if (this.tos.Length != 0)
		{
			for (int m = 0; m < this.tos.Length; m++)
			{
				if (!(this.tos[m] == null))
				{
					if (MonoSingleton<CrossfadeTracker>.Instance.actives.Count > 0)
					{
						bool flag = false;
						for (int n = MonoSingleton<CrossfadeTracker>.Instance.actives.Count - 1; n >= 0; n--)
						{
							if (!(MonoSingleton<CrossfadeTracker>.Instance.actives[n] == null))
							{
								if (MonoSingleton<CrossfadeTracker>.Instance.actives[n].froms != null && MonoSingleton<CrossfadeTracker>.Instance.actives[n].froms.Length != 0)
								{
									for (int num = MonoSingleton<CrossfadeTracker>.Instance.actives[n].froms.Length - 1; num >= 0; num--)
									{
										if (!(MonoSingleton<CrossfadeTracker>.Instance.actives[n].froms[num] == null) && MonoSingleton<CrossfadeTracker>.Instance.actives[n].froms[num] == this.tos[m])
										{
											flag = true;
										}
									}
								}
								if (MonoSingleton<CrossfadeTracker>.Instance.actives[n].tos != null && MonoSingleton<CrossfadeTracker>.Instance.actives[n].tos.Length != 0)
								{
									for (int num2 = MonoSingleton<CrossfadeTracker>.Instance.actives[n].tos.Length - 1; num2 >= 0; num2--)
									{
										if (!(MonoSingleton<CrossfadeTracker>.Instance.actives[n].tos[num2] == null) && MonoSingleton<CrossfadeTracker>.Instance.actives[n].tos[num2] == this.tos[m])
										{
											flag = true;
										}
									}
								}
								if (flag)
								{
									MonoSingleton<CrossfadeTracker>.Instance.actives[n].StopFade();
									this.toMinVolumes[m] = this.tos[m].volume;
								}
							}
						}
						if (!flag && this.firstTime)
						{
							this.tos[m].volume = 0f;
						}
					}
					else if (this.firstTime)
					{
						this.tos[m].volume = 0f;
					}
					if (this.toMinVolumes != null && this.toMinVolumes.Length != 0)
					{
						this.toMinVolumes[m] = this.tos[m].volume;
					}
					if (this.toMaxVolumes != null && this.toMaxVolumes.Length != 0)
					{
						this.toMaxVolumes[m] = this.toOriginalVolumes[m];
					}
					if (!this.tos[m].isPlaying)
					{
						this.tos[m].Play();
					}
					if (this.match && this.froms.Length != 0)
					{
						this.tos[m].time = this.froms[0].time % this.tos[m].clip.length;
					}
				}
			}
		}
		MonoSingleton<CrossfadeTracker>.Instance.actives.Add(this);
		this.fadeAmount = 0f;
		this.inProgress = true;
		this.firstTime = false;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00020AA1 File Offset: 0x0001ECA1
	public void StopFade()
	{
		if (this.inProgress)
		{
			this.inProgress = false;
			if (MonoSingleton<CrossfadeTracker>.Instance.actives.Contains(this))
			{
				MonoSingleton<CrossfadeTracker>.Instance.actives.Remove(this);
			}
		}
	}

	// Token: 0x04000665 RID: 1637
	public bool multipleTargets;

	// Token: 0x04000666 RID: 1638
	public AudioSource from;

	// Token: 0x04000667 RID: 1639
	public AudioSource to;

	// Token: 0x04000668 RID: 1640
	public AudioSource[] froms;

	// Token: 0x04000669 RID: 1641
	public AudioSource[] tos;

	// Token: 0x0400066A RID: 1642
	[HideInInspector]
	public float[] fromMaxVolumes;

	// Token: 0x0400066B RID: 1643
	[HideInInspector]
	public float[] toOriginalVolumes;

	// Token: 0x0400066C RID: 1644
	[HideInInspector]
	public float[] toMaxVolumes;

	// Token: 0x0400066D RID: 1645
	[HideInInspector]
	public float[] toMinVolumes;

	// Token: 0x0400066E RID: 1646
	[HideInInspector]
	public bool inProgress;

	// Token: 0x0400066F RID: 1647
	public float time;

	// Token: 0x04000670 RID: 1648
	public bool unscaledTime;

	// Token: 0x04000671 RID: 1649
	private float fadeAmount;

	// Token: 0x04000672 RID: 1650
	public bool match;

	// Token: 0x04000673 RID: 1651
	public bool dontActivateOnStart;

	// Token: 0x04000674 RID: 1652
	public bool oneTime;

	// Token: 0x04000675 RID: 1653
	private bool activated;

	// Token: 0x04000676 RID: 1654
	private bool firstTime = true;
}
