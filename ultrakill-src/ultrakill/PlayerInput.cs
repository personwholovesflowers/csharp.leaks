using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

// Token: 0x0200026E RID: 622
public class PlayerInput
{
	// Token: 0x06000DB1 RID: 3505 RVA: 0x00067196 File Offset: 0x00065396
	public PlayerInput()
	{
		this.Actions = new InputActions();
		this.RebuildActions();
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x000671BC File Offset: 0x000653BC
	public void ValidateBindings(InputControlScheme scheme)
	{
		this.conflicts.Clear();
		IEnumerable<InputAction> enumerable = from action in new InputActionMap[]
			{
				this.Actions.Movement,
				this.Actions.Weapon,
				this.Actions.Fist,
				this.Actions.HUD
			}.SelectMany((InputActionMap map) => map.actions)
			where action.name != "Look" && action.name != "WheelLook"
			select action;
		this.Actions.RemoveAllBindingOverrides();
		foreach (IGrouping<InputControl, InputBinding> grouping in from binding in enumerable.SelectMany((InputAction action) => action.bindings)
			where binding.groups != null
			where binding.groups.Contains(scheme.bindingGroup)
			where !binding.isComposite
			group binding by InputSystem.FindControl(binding.path))
		{
			if (grouping.Key != null)
			{
				InputBinding[] array = grouping.ToArray<InputBinding>();
				if (array.Length > 1)
				{
					this.conflicts.Add(grouping.Key, array);
					foreach (InputBinding inputBinding in array)
					{
						InputAction inputAction = this.Actions.FindAction(inputBinding.action, false);
						inputBinding.overridePath = "";
						inputAction.ApplyBindingOverride(inputBinding);
					}
				}
			}
		}
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x000673C4 File Offset: 0x000655C4
	private void RebuildActions()
	{
		this.Move = new InputActionState(this.Actions.Movement.Move);
		this.Look = new InputActionState(this.Actions.Movement.Look);
		this.WheelLook = new InputActionState(this.Actions.Weapon.WheelLook);
		this.Punch = new InputActionState(this.Actions.Fist.Punch);
		this.Hook = new InputActionState(this.Actions.Fist.Hook);
		this.Fire1 = new InputActionState(this.Actions.Weapon.PrimaryFire);
		this.Fire2 = new InputActionState(this.Actions.Weapon.SecondaryFire);
		this.Jump = new InputActionState(this.Actions.Movement.Jump);
		this.Slide = new InputActionState(this.Actions.Movement.Slide);
		this.Dodge = new InputActionState(this.Actions.Movement.Dodge);
		this.ChangeFist = new InputActionState(this.Actions.Fist.ChangeFist);
		this.NextVariation = new InputActionState(this.Actions.Weapon.NextVariation);
		this.PreviousVariation = new InputActionState(this.Actions.Weapon.PreviousVariation);
		this.NextWeapon = new InputActionState(this.Actions.Weapon.NextWeapon);
		this.PrevWeapon = new InputActionState(this.Actions.Weapon.PreviousWeapon);
		this.LastWeapon = new InputActionState(this.Actions.Weapon.LastUsedWeapon);
		this.SelectVariant1 = new InputActionState(this.Actions.Weapon.VariationSlot1);
		this.SelectVariant2 = new InputActionState(this.Actions.Weapon.VariationSlot2);
		this.SelectVariant3 = new InputActionState(this.Actions.Weapon.VariationSlot3);
		this.Pause = new InputActionState(this.Actions.UI.Pause);
		this.Stats = new InputActionState(this.Actions.HUD.Stats);
		this.Slot1 = new InputActionState(this.Actions.Weapon.Revolver);
		this.Slot2 = new InputActionState(this.Actions.Weapon.Shotgun);
		this.Slot3 = new InputActionState(this.Actions.Weapon.Nailgun);
		this.Slot4 = new InputActionState(this.Actions.Weapon.Railcannon);
		this.Slot5 = new InputActionState(this.Actions.Weapon.RocketLauncher);
		this.Slot6 = new InputActionState(this.Actions.Weapon.SpawnerArm);
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x000676FC File Offset: 0x000658FC
	public InputBinding[] GetConflicts(InputBinding binding)
	{
		InputControl inputControl = InputSystem.FindControl(binding.path);
		if (inputControl == null)
		{
			return new InputBinding[0];
		}
		InputBinding[] array;
		if (this.conflicts.TryGetValue(inputControl, out array))
		{
			return array;
		}
		return new InputBinding[0];
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x00067738 File Offset: 0x00065938
	public void Enable()
	{
		this.Actions.Enable();
		this.ValidateBindings(this.Actions.KeyboardMouseScheme);
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x00067756 File Offset: 0x00065956
	public void Disable()
	{
		this.Actions.Disable();
	}

	// Token: 0x04001223 RID: 4643
	public InputActions Actions;

	// Token: 0x04001224 RID: 4644
	public InputActionState Move;

	// Token: 0x04001225 RID: 4645
	public InputActionState Look;

	// Token: 0x04001226 RID: 4646
	public InputActionState WheelLook;

	// Token: 0x04001227 RID: 4647
	public InputActionState Punch;

	// Token: 0x04001228 RID: 4648
	public InputActionState Hook;

	// Token: 0x04001229 RID: 4649
	public InputActionState Fire1;

	// Token: 0x0400122A RID: 4650
	public InputActionState Fire2;

	// Token: 0x0400122B RID: 4651
	public InputActionState Jump;

	// Token: 0x0400122C RID: 4652
	public InputActionState Slide;

	// Token: 0x0400122D RID: 4653
	public InputActionState Dodge;

	// Token: 0x0400122E RID: 4654
	public InputActionState ChangeFist;

	// Token: 0x0400122F RID: 4655
	public InputActionState NextVariation;

	// Token: 0x04001230 RID: 4656
	public InputActionState PreviousVariation;

	// Token: 0x04001231 RID: 4657
	public InputActionState NextWeapon;

	// Token: 0x04001232 RID: 4658
	public InputActionState PrevWeapon;

	// Token: 0x04001233 RID: 4659
	public InputActionState LastWeapon;

	// Token: 0x04001234 RID: 4660
	public InputActionState SelectVariant1;

	// Token: 0x04001235 RID: 4661
	public InputActionState SelectVariant2;

	// Token: 0x04001236 RID: 4662
	public InputActionState SelectVariant3;

	// Token: 0x04001237 RID: 4663
	public InputActionState Pause;

	// Token: 0x04001238 RID: 4664
	public InputActionState Stats;

	// Token: 0x04001239 RID: 4665
	public InputActionState Slot1;

	// Token: 0x0400123A RID: 4666
	public InputActionState Slot2;

	// Token: 0x0400123B RID: 4667
	public InputActionState Slot3;

	// Token: 0x0400123C RID: 4668
	public InputActionState Slot4;

	// Token: 0x0400123D RID: 4669
	public InputActionState Slot5;

	// Token: 0x0400123E RID: 4670
	public InputActionState Slot6;

	// Token: 0x0400123F RID: 4671
	private Dictionary<InputControl, InputBinding[]> conflicts = new Dictionary<InputControl, InputBinding[]>();
}
