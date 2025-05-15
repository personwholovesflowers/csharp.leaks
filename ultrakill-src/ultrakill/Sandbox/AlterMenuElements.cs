using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sandbox
{
	// Token: 0x02000555 RID: 1365
	public class AlterMenuElements : MonoBehaviour
	{
		// Token: 0x06001EC6 RID: 7878 RVA: 0x000FCEFC File Offset: 0x000FB0FC
		public void CreateTitle(string name)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.titleTemplate, this.container, false);
			gameObject.SetActive(true);
			gameObject.GetComponentInChildren<TMP_Text>().text = name;
			this.createdRows.Add(gameObject.GetInstanceID());
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000FCF40 File Offset: 0x000FB140
		public void CreateBoolRow(string name, bool initialState, Action<bool> callback, string tooltipMessage = null)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.boolRowTemplate, this.container, false);
			gameObject.SetActive(true);
			gameObject.GetComponentInChildren<TMP_Text>().text = name;
			Toggle componentInChildren = gameObject.GetComponentInChildren<Toggle>();
			componentInChildren.SetIsOnWithoutNotify(initialState);
			componentInChildren.interactable = callback != null;
			if (callback != null)
			{
				componentInChildren.onValueChanged.AddListener(delegate(bool state)
				{
					callback(state);
				});
			}
			if (tooltipMessage != null)
			{
				this.CreateTooltip(gameObject, tooltipMessage);
			}
			this.createdRows.Add(gameObject.GetInstanceID());
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000FCFDC File Offset: 0x000FB1DC
		public void CreateFloatRow(string name, float initialState, Action<float> callback, IConstraints constraints = null, string tooltipMessage = null)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.floatRowTemplate, this.container, false);
			gameObject.SetActive(true);
			gameObject.GetComponentInChildren<TMP_Text>().text = name;
			Slider componentInChildren = gameObject.GetComponentInChildren<Slider>();
			SliderConstraints sliderConstraints = constraints as SliderConstraints;
			if (sliderConstraints != null)
			{
				componentInChildren.minValue = sliderConstraints.min;
				componentInChildren.maxValue = sliderConstraints.max;
				componentInChildren.wholeNumbers = sliderConstraints.step % 1f == 0f;
				bool wholeNumbers = componentInChildren.wholeNumbers;
			}
			componentInChildren.SetValueWithoutNotify(initialState);
			componentInChildren.interactable = callback != null;
			if (callback != null)
			{
				componentInChildren.onValueChanged.AddListener(delegate(float value)
				{
					callback(value);
				});
			}
			if (tooltipMessage != null)
			{
				this.CreateTooltip(gameObject, tooltipMessage);
			}
			this.createdRows.Add(gameObject.GetInstanceID());
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000FD0BC File Offset: 0x000FB2BC
		public void CreateVector3Row(string name, Vector3 initialState, Action<Vector3> callback, string tooltipMessage = null)
		{
			AlterMenuVector3Field row = Object.Instantiate<AlterMenuVector3Field>(this.vector3RowTemplate, this.container, false);
			row.gameObject.SetActive(true);
			row.nameText.text = name;
			row.xField.SetTextWithoutNotify(initialState.x.ToString(CultureInfo.InvariantCulture));
			row.yField.SetTextWithoutNotify(initialState.y.ToString(CultureInfo.InvariantCulture));
			row.zField.SetTextWithoutNotify(initialState.z.ToString(CultureInfo.InvariantCulture));
			this.vector3ValueStore.Add(row.GetInstanceID(), initialState);
			row.xField.interactable = callback != null;
			row.yField.interactable = callback != null;
			row.zField.interactable = callback != null;
			if (callback != null)
			{
				row.xField.onValueChanged.AddListener(delegate(string value)
				{
					float num;
					if (!float.TryParse(value, out num))
					{
						return;
					}
					this.UpdateVector3Value(row.GetInstanceID(), num, AlterMenuElements.Axis.X);
					callback(this.vector3ValueStore[row.GetInstanceID()]);
				});
				row.yField.onValueChanged.AddListener(delegate(string value)
				{
					float num2;
					if (!float.TryParse(value, out num2))
					{
						return;
					}
					this.UpdateVector3Value(row.GetInstanceID(), num2, AlterMenuElements.Axis.Y);
					callback(this.vector3ValueStore[row.GetInstanceID()]);
				});
				row.zField.onValueChanged.AddListener(delegate(string value)
				{
					float num3;
					if (!float.TryParse(value, out num3))
					{
						return;
					}
					this.UpdateVector3Value(row.GetInstanceID(), num3, AlterMenuElements.Axis.Z);
					callback(this.vector3ValueStore[row.GetInstanceID()]);
				});
			}
			if (tooltipMessage != null)
			{
				this.CreateTooltip(row.gameObject, tooltipMessage);
			}
			this.createdRows.Add(row.gameObject.GetInstanceID());
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000FD27C File Offset: 0x000FB47C
		public void UpdateVector3Value(int id, float value, AlterMenuElements.Axis axis)
		{
			if (!this.vector3ValueStore.ContainsKey(id))
			{
				this.vector3ValueStore.Add(id, Vector3.zero);
			}
			Vector3 vector = this.vector3ValueStore[id];
			switch (axis)
			{
			case AlterMenuElements.Axis.X:
				vector.x = value;
				break;
			case AlterMenuElements.Axis.Y:
				vector.y = value;
				break;
			case AlterMenuElements.Axis.Z:
				vector.z = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("axis", axis, null);
			}
			this.vector3ValueStore[id] = vector;
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000FD308 File Offset: 0x000FB508
		public void CreateEnumRow(string name, int initialState, Action<int> callback, Type type, string tooltipMessage = null)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.dropdownRowTemplate, this.container, false);
			gameObject.SetActive(true);
			gameObject.GetComponentInChildren<TMP_Text>().text = name;
			Dropdown componentInChildren = gameObject.GetComponentInChildren<Dropdown>();
			string[] names = Enum.GetNames(type);
			componentInChildren.ClearOptions();
			componentInChildren.AddOptions(new List<string>(names));
			componentInChildren.SetValueWithoutNotify(initialState);
			componentInChildren.interactable = callback != null;
			if (callback != null)
			{
				componentInChildren.onValueChanged.AddListener(delegate(int value)
				{
					callback(value);
				});
			}
			if (tooltipMessage != null)
			{
				this.CreateTooltip(gameObject, tooltipMessage);
			}
			this.createdRows.Add(gameObject.GetInstanceID());
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000FD3BC File Offset: 0x000FB5BC
		public void Reset()
		{
			foreach (object obj in this.container)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf && !(transform.gameObject == this.titleTemplate) && !(transform.gameObject == this.boolRowTemplate) && !(transform.gameObject == this.floatRowTemplate) && this.createdRows.Contains(transform.gameObject.GetInstanceID()))
				{
					Object.Destroy(transform.gameObject);
				}
			}
			this.createdRows.Clear();
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000FD484 File Offset: 0x000FB684
		private void CreateTooltip(GameObject row, string message)
		{
			TooltipOnHover tooltipOnHover = row.AddComponent<TooltipOnHover>();
			tooltipOnHover.tooltipManager = this.tooltipManager;
			tooltipOnHover.text = message;
		}

		// Token: 0x04002B33 RID: 11059
		[SerializeField]
		private TooltipManager tooltipManager;

		// Token: 0x04002B34 RID: 11060
		[SerializeField]
		private Transform container;

		// Token: 0x04002B35 RID: 11061
		[Header("Templates")]
		[SerializeField]
		private GameObject titleTemplate;

		// Token: 0x04002B36 RID: 11062
		[SerializeField]
		private GameObject boolRowTemplate;

		// Token: 0x04002B37 RID: 11063
		[SerializeField]
		private GameObject floatRowTemplate;

		// Token: 0x04002B38 RID: 11064
		[SerializeField]
		private AlterMenuVector3Field vector3RowTemplate;

		// Token: 0x04002B39 RID: 11065
		[SerializeField]
		private GameObject dropdownRowTemplate;

		// Token: 0x04002B3A RID: 11066
		private readonly List<int> createdRows = new List<int>();

		// Token: 0x04002B3B RID: 11067
		private readonly Dictionary<int, Vector3> vector3ValueStore = new Dictionary<int, Vector3>();

		// Token: 0x02000556 RID: 1366
		public enum Axis
		{
			// Token: 0x04002B3D RID: 11069
			X,
			// Token: 0x04002B3E RID: 11070
			Y,
			// Token: 0x04002B3F RID: 11071
			Z
		}
	}
}
