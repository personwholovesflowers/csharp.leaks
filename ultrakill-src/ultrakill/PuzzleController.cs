using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036A RID: 874
public class PuzzleController : MonoBehaviour
{
	// Token: 0x06001457 RID: 5207 RVA: 0x000A4DE8 File Offset: 0x000A2FE8
	private void Start()
	{
		this.panels = base.GetComponentsInChildren<PuzzlePanel>();
		this.img = base.GetComponent<Image>();
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x000A4E02 File Offset: 0x000A3002
	private void OnDisable()
	{
		if (!this.solved)
		{
			this.ResetPuzzle();
		}
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x000A4E14 File Offset: 0x000A3014
	private void Update()
	{
		if (!this.backOnBlack)
		{
			this.img.color = Color.Lerp(this.img.color, Color.white, Time.deltaTime);
			if (this.img.color == Color.white)
			{
				this.backOnBlack = true;
			}
		}
		if (this.checkForHold > 0f)
		{
			this.checkForHold = Mathf.MoveTowards(this.checkForHold, 0f, Time.deltaTime);
		}
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x000A4E94 File Offset: 0x000A3094
	public void Clicked(PuzzlePanel other)
	{
		if (other.tileType == TileType.Start)
		{
			if (!this.puzzleInProgress && !this.activatedPanels.Contains(other))
			{
				if (this.solved)
				{
					this.solved = false;
					this.backOnBlack = false;
					this.ResetPuzzle();
				}
				this.checkForHold = 0.3f;
				Object.Instantiate<GameObject>(this.puzzleClick, other.transform.position, Quaternion.identity);
				this.starts++;
				this.puzzleInProgress = true;
				this.currentColor = other.tileColor;
				other.Activate(this.currentColor);
				this.activatedPanels.Add(other);
				this.activatedColors.Add(this.currentColor);
				return;
			}
			this.ResetPuzzle();
			return;
		}
		else
		{
			if (other.tileType != TileType.End || !this.puzzleInProgress || !(this.activatedPanels[this.activatedPanels.Count - 1] == other))
			{
				if (this.puzzleInProgress)
				{
					this.ResetPuzzle();
					Object.Instantiate<GameObject>(this.puzzleClick, other.transform.position, Quaternion.identity).GetComponent<AudioSource>().pitch -= 0.5f;
				}
				return;
			}
			if (this.currentColor == other.tileColor || other.tileColor == TileColor.None)
			{
				this.CheckSolution();
				return;
			}
			this.Failure();
			return;
		}
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x000A4FF4 File Offset: 0x000A31F4
	public void Unclicked()
	{
		if (this.puzzleInProgress && this.checkForHold == 0f)
		{
			this.Clicked(this.activatedPanels[this.activatedPanels.Count - 1]);
			if (this.punch == null)
			{
				this.punch = MonoSingleton<FistControl>.Instance.currentPunch;
			}
			this.punch.anim.SetTrigger("ShopTap");
		}
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x000A5068 File Offset: 0x000A3268
	public void Hovered(PuzzlePanel other)
	{
		if (this.puzzleInProgress)
		{
			if (!this.activatedPanels.Contains(other))
			{
				if (Vector3.Distance(other.transform.localPosition, this.activatedPanels[this.activatedPanels.Count - 1].transform.localPosition) < (float)(other.pl.length - 3))
				{
					other.pl.DrawLine(other.transform.localPosition, this.activatedPanels[this.activatedPanels.Count - 1].transform.localPosition, this.currentColor);
					other.Activate(this.currentColor);
					this.activatedPanels.Add(other);
					this.activatedColors.Add(this.currentColor);
					return;
				}
			}
			else if (this.activatedPanels.IndexOf(other) == this.activatedPanels.Count - 2)
			{
				this.activatedPanels[this.activatedPanels.Count - 1].DeActivate();
				this.activatedPanels[this.activatedPanels.Count - 1].pl.Hide();
				this.activatedPanels.Remove(this.activatedPanels[this.activatedPanels.Count - 1]);
				this.activatedColors.Remove(this.activatedColors[this.activatedColors.Count - 1]);
			}
		}
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x000A51E4 File Offset: 0x000A33E4
	public void Success()
	{
		this.img.color = Color.green;
		this.puzzleInProgress = false;
		this.solved = true;
		this.backOnBlack = true;
		if (this.toActivate.Length != 0)
		{
			base.Invoke("ActivateNow", 0.5f);
		}
		Object.Instantiate<GameObject>(this.puzzleCorrect, base.transform.position, Quaternion.identity);
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x000A524B File Offset: 0x000A344B
	public void Failure()
	{
		this.img.color = Color.red;
		this.backOnBlack = false;
		this.ResetPuzzle();
		Object.Instantiate<GameObject>(this.puzzleWrong, base.transform.position, Quaternion.identity);
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x000A5288 File Offset: 0x000A3488
	public void ResetPuzzle()
	{
		this.starts = 0;
		this.ends = 0;
		this.puzzleInProgress = false;
		this.activatedPanels.Clear();
		this.activatedColors.Clear();
		foreach (PuzzlePanel puzzlePanel in this.panels)
		{
			puzzlePanel.DeActivate();
			puzzlePanel.pl.Hide();
		}
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000A52E8 File Offset: 0x000A34E8
	private void CheckSolution()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		this.ends = 0;
		for (int i = 0; i < this.panels.Length; i++)
		{
			if (this.panels[i].tileType == TileType.End)
			{
				if (!this.activatedPanels.Contains(this.panels[i]))
				{
					flag2 = true;
				}
				else
				{
					this.ends++;
					if (this.panels[i].tileColor != this.activatedColors[this.activatedPanels.IndexOf(this.panels[i])] && this.panels[i].tileColor != TileColor.None)
					{
						flag = true;
					}
				}
			}
			else if (this.panels[i].tileType == TileType.Fill)
			{
				if (!this.activatedPanels.Contains(this.panels[i]) && this.panels[i].tileColor == this.currentColor)
				{
					flag = true;
				}
				else if (!this.activatedPanels.Contains(this.panels[i]) && this.panels[i].tileColor == TileColor.None)
				{
					flag3 = true;
				}
				else if (this.activatedPanels.Contains(this.panels[i]) && this.panels[i].tileColor != this.activatedColors[this.activatedPanels.IndexOf(this.panels[i])] && this.panels[i].tileColor != TileColor.None)
				{
					flag = true;
				}
			}
			else if (this.panels[i].tileType == TileType.Pit && this.puzzleInProgress && this.activatedPanels.Contains(this.panels[i]) && (this.panels[i].tileColor == this.activatedColors[this.activatedPanels.IndexOf(this.panels[i])] || this.panels[i].tileColor == TileColor.None))
			{
				flag = true;
			}
			if (flag)
			{
				break;
			}
		}
		if (this.starts != this.ends)
		{
			this.Failure();
			return;
		}
		if (!flag && !flag3 && !flag2)
		{
			this.Success();
			return;
		}
		if (flag || (flag3 && !flag2))
		{
			this.Failure();
			return;
		}
		if (flag2)
		{
			this.WhiteFlash();
		}
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x000A5511 File Offset: 0x000A3711
	private void WhiteFlash()
	{
		this.puzzleInProgress = false;
		this.backOnBlack = false;
		this.img.color = Color.white;
		Object.Instantiate<GameObject>(this.puzzleClick, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x000A5550 File Offset: 0x000A3750
	private void ActivateNow()
	{
		GameObject[] array = this.toActivate;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
	}

	// Token: 0x04001BE9 RID: 7145
	private bool backOnBlack = true;

	// Token: 0x04001BEA RID: 7146
	private Image img;

	// Token: 0x04001BEB RID: 7147
	private PuzzlePanel[] panels;

	// Token: 0x04001BEC RID: 7148
	public List<PuzzlePanel> activatedPanels = new List<PuzzlePanel>();

	// Token: 0x04001BED RID: 7149
	public List<TileColor> activatedColors = new List<TileColor>();

	// Token: 0x04001BEE RID: 7150
	public bool puzzleInProgress;

	// Token: 0x04001BEF RID: 7151
	public bool solved;

	// Token: 0x04001BF0 RID: 7152
	public GameObject[] toActivate;

	// Token: 0x04001BF1 RID: 7153
	private TileColor currentColor;

	// Token: 0x04001BF2 RID: 7154
	private int starts;

	// Token: 0x04001BF3 RID: 7155
	private int ends;

	// Token: 0x04001BF4 RID: 7156
	public GameObject puzzleCorrect;

	// Token: 0x04001BF5 RID: 7157
	public GameObject puzzleWrong;

	// Token: 0x04001BF6 RID: 7158
	public GameObject puzzleClick;

	// Token: 0x04001BF7 RID: 7159
	private float checkForHold;

	// Token: 0x04001BF8 RID: 7160
	private Punch punch;
}
