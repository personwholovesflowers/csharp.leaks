using System;
using Nest.Components;
using UnityEngine;

namespace Nest.Addons
{
	// Token: 0x0200024D RID: 589
	public class ButtonInterpreter : MonoBehaviour
	{
		// Token: 0x06000DEA RID: 3562 RVA: 0x0003E618 File Offset: 0x0003C818
		public void Awake()
		{
			if ((this._camera = base.GetComponent<Camera>()) == null)
			{
				this._camera = base.GetComponentInChildren<Camera>();
			}
			this._results = new RaycastHit[3];
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0003E654 File Offset: 0x0003C854
		public void Update()
		{
			this._canInteract = this.FindInteractable();
			if (this._currentInteractableEvent == null)
			{
				return;
			}
			switch (this._eventState)
			{
			case ButtonInterpreter.InteractionState.Dormant:
				if (Input.GetKey(this.InteractKey))
				{
					this.InvokeButton(true);
					this._eventState = ButtonInterpreter.InteractionState.Active;
					return;
				}
				break;
			case ButtonInterpreter.InteractionState.Active:
				if (!Input.GetKey(this.InteractKey) || !this._canInteract)
				{
					this.InvokeButton(false);
					this._eventState = ButtonInterpreter.InteractionState.Used;
					return;
				}
				break;
			case ButtonInterpreter.InteractionState.Used:
				this._eventState = ButtonInterpreter.InteractionState.Dormant;
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0003E6E5 File Offset: 0x0003C8E5
		private void InvokeButton(bool state)
		{
			if (this._currentInteractableEvent == null)
			{
				return;
			}
			this._currentInteractableEvent.SetBool(state);
			this._currentInteractableEvent.Invoke();
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0003E710 File Offset: 0x0003C910
		protected virtual bool FindInteractable()
		{
			int num;
			if ((num = Physics.RaycastNonAlloc(this._camera.transform.position, this._camera.transform.forward, this._results, this.MaxInteractDistance, LayerMask.GetMask(new string[] { "Interact" }))) <= 0)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (!(this._results[i].transform == null))
				{
					NestInput component = this._results[i].transform.GetComponent<NestInput>();
					if (this._currentInteractableEvent != component && this._eventState == ButtonInterpreter.InteractionState.Active)
					{
						this._eventState = ButtonInterpreter.InteractionState.Used;
						this.InvokeButton(false);
					}
					else if (this._currentInteractableEvent == component)
					{
						return true;
					}
					this._currentInteractableEvent = component;
				}
			}
			return this._currentInteractableEvent != null;
		}

		// Token: 0x04000C76 RID: 3190
		[Header("Sets the Max interaction Distance")]
		public float MaxInteractDistance = 25f;

		// Token: 0x04000C77 RID: 3191
		public KeyCode InteractKey;

		// Token: 0x04000C78 RID: 3192
		private Camera _camera;

		// Token: 0x04000C79 RID: 3193
		private ButtonInterpreter.InteractionState _eventState;

		// Token: 0x04000C7A RID: 3194
		private bool _canInteract;

		// Token: 0x04000C7B RID: 3195
		private NestInput _currentInteractableEvent;

		// Token: 0x04000C7C RID: 3196
		private RaycastHit[] _results;

		// Token: 0x02000443 RID: 1091
		public enum InteractionState
		{
			// Token: 0x040015E4 RID: 5604
			Dormant,
			// Token: 0x040015E5 RID: 5605
			Active,
			// Token: 0x040015E6 RID: 5606
			Used
		}
	}
}
