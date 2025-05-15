using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000144 RID: 324
public abstract class DirectoryTreeBrowser<T> : MonoBehaviour
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000650 RID: 1616
	protected abstract int maxPageLength { get; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000651 RID: 1617
	protected abstract IDirectoryTree<T> baseDirectory { get; }

	// Token: 0x06000652 RID: 1618 RVA: 0x0002BE78 File Offset: 0x0002A078
	public static FakeDirectoryTree<T> Folder(string name, List<T> files = null, List<IDirectoryTree<T>> children = null, IDirectoryTree<T> parent = null)
	{
		FakeDirectoryTree<T> fakeDirectoryTree = new FakeDirectoryTree<T>(name, files, children, null);
		if (children != null)
		{
			foreach (IDirectoryTree<T> directoryTree in children)
			{
				directoryTree.parent = fakeDirectoryTree;
			}
		}
		return fakeDirectoryTree;
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0002BED4 File Offset: 0x0002A0D4
	private void Awake()
	{
		this.currentDirectory = this.baseDirectory;
		this.Rebuild(true);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0002BEE9 File Offset: 0x0002A0E9
	public int PageOf(int index)
	{
		return Mathf.CeilToInt((float)(index / this.maxPageLength));
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0002BEF9 File Offset: 0x0002A0F9
	public void SetPage(int target)
	{
		this.currentPage = Mathf.Clamp(target, 0, this.maxPages - 1);
		this.Rebuild(false);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0002BF17 File Offset: 0x0002A117
	public void NextPage()
	{
		this.SetPage(this.currentPage + 1);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0002BF27 File Offset: 0x0002A127
	public void PreviousPage()
	{
		this.SetPage(this.currentPage - 1);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002BF37 File Offset: 0x0002A137
	public void StepUp()
	{
		this.currentDirectory = this.currentDirectory.parent ?? this.currentDirectory;
		this.Rebuild(true);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0002BF5B File Offset: 0x0002A15B
	public void StepDown(IDirectoryTree<T> dir)
	{
		this.currentDirectory = dir;
		this.Rebuild(true);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0002BF6B File Offset: 0x0002A16B
	public void GoToBase()
	{
		if (this.currentDirectory == this.baseDirectory)
		{
			return;
		}
		this.currentDirectory = this.baseDirectory;
		this.Rebuild(true);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0002BF90 File Offset: 0x0002A190
	public virtual void Rebuild(bool setToPageZero = true)
	{
		if (setToPageZero)
		{
			this.currentPage = 0;
		}
		this.currentDirectory.Refresh();
		int num = this.maxPageLength;
		if (this.backButton)
		{
			bool flag = this.currentDirectory.parent != null;
			this.backButton.SetActive(flag);
			if (flag && this.backButton.transform.IsChildOf(this.itemParent))
			{
				num--;
			}
		}
		foreach (Action action in this.cleanupActions)
		{
			if (action != null)
			{
				action();
			}
		}
		this.cleanupActions.Clear();
		List<IDirectoryTree<T>> list = this.currentDirectory.children.Skip(this.currentPage * num).Take(num).ToList<IDirectoryTree<T>>();
		int num2 = 0;
		foreach (IDirectoryTree<T> directoryTree in list)
		{
			Action action2 = this.BuildDirectory(directoryTree, num2++);
			if (action2 != null)
			{
				this.cleanupActions.Add(action2);
			}
		}
		List<T> list2 = this.currentDirectory.files.Skip(this.currentPage * num - this.currentDirectory.children.Count<IDirectoryTree<T>>()).Take(num - list.Count).ToList<T>();
		num2 = 0;
		foreach (T t in list2)
		{
			Action action3 = this.BuildLeaf(t, num2++);
			if (action3 != null)
			{
				this.cleanupActions.Add(action3);
			}
		}
		int num3 = this.currentDirectory.children.Count<IDirectoryTree<T>>() + this.currentDirectory.files.Count<T>();
		if (this.plusButton)
		{
			num3++;
			this.plusButton.transform.SetAsLastSibling();
			if (list.Count + list2.Count < this.maxPageLength)
			{
				this.plusButton.SetActive(true);
			}
			else
			{
				this.plusButton.SetActive(false);
			}
		}
		this.maxPages = Mathf.CeilToInt((float)num3 / (float)num);
		this.pageText.text = string.Format("{0}/{1}", this.currentPage + 1, this.maxPages);
	}

	// Token: 0x0600065C RID: 1628
	protected abstract Action BuildLeaf(T item, int indexInPage);

	// Token: 0x0600065D RID: 1629 RVA: 0x0002C220 File Offset: 0x0002A420
	protected virtual Action BuildDirectory(IDirectoryTree<T> folder, int indexInPage)
	{
		GameObject btn = Object.Instantiate<GameObject>(this.folderButtonTemplate, this.itemParent, false);
		btn.GetComponent<Button>().onClick.RemoveAllListeners();
		btn.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.StepDown(folder);
		});
		btn.GetComponentInChildren<TMP_Text>().text = folder.name;
		btn.SetActive(true);
		return delegate
		{
			Object.Destroy(btn);
		};
	}

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	protected GameObject itemButtonTemplate;

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	protected GameObject folderButtonTemplate;

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	protected Transform itemParent;

	// Token: 0x0400088B RID: 2187
	[SerializeField]
	protected GameObject backButton;

	// Token: 0x0400088C RID: 2188
	[SerializeField]
	protected GameObject plusButton;

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private TMP_Text pageText;

	// Token: 0x0400088E RID: 2190
	private List<Action> cleanupActions = new List<Action>();

	// Token: 0x0400088F RID: 2191
	protected IDirectoryTree<T> currentDirectory;

	// Token: 0x04000890 RID: 2192
	protected int maxPages;

	// Token: 0x04000891 RID: 2193
	protected int currentPage;
}
