using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000004 RID: 4
public class InputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
	public InputActionAsset asset { get; }

	// Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
	public InputActions()
	{
		this.asset = InputActionAsset.FromJson("{\n    \"name\": \"InputActions\",\n    \"maps\": [\n        {\n            \"name\": \"UI\",\n            \"id\": \"272f6d14-89ba-496f-b7ff-215263d3219f\",\n            \"actions\": [\n                {\n                    \"name\": \"Navigate\",\n                    \"type\": \"Value\",\n                    \"id\": \"c95b2375-e6d9-4b88-9c4c-c5e76515df4b\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Submit\",\n                    \"type\": \"Button\",\n                    \"id\": \"7607c7b6-cd76-4816-beef-bd0341cfe950\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Cancel\",\n                    \"type\": \"Button\",\n                    \"id\": \"15cef263-9014-4fd5-94d9-4e4a6234a6ef\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Point\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"32b35790-4ed0-4e9a-aa41-69ac6d629449\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Click\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"3c7022bf-7922-4f7c-a998-c437916075ad\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"ScrollWheel\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"0489e84a-4833-4c40-bfae-cea84b696689\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"MiddleClick\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"dad70c86-b58c-4b17-88ad-f5e53adf419e\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"RightClick\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"44b200b1-1557-4083-816c-b22cbdf77ddf\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrackedDevicePosition\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"24908448-c609-4bc3-a128-ea258674378a\",\n                    \"expectedControlType\": \"Vector3\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrackedDeviceOrientation\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"9caa3d8a-6b2f-4e8e-8bad-6ede561bd9be\",\n                    \"expectedControlType\": \"Quaternion\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ScrollSublist\",\n                    \"type\": \"Value\",\n                    \"id\": \"6a48eebe-4a36-47fa-a511-0489aa7c315f\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Pause\",\n                    \"type\": \"Button\",\n                    \"id\": \"97668417-6564-4b1c-8acf-ec55ca459e96\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"809f371f-c5e2-4e7a-83a1-d867598f40dd\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf\",\n                    \"path\": \"<Gamepad>/leftStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"2db08d65-c5fb-421b-983f-c71163608d67\",\n                    \"path\": \"<Gamepad>/leftStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"8ba04515-75aa-45de-966d-393d9bbd1c14\",\n                    \"path\": \"<Gamepad>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"fcd248ae-a788-4676-a12e-f4d81205600b\",\n                    \"path\": \"<Gamepad>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"fb8277d4-c5cd-4663-9dc7-ee3f0b506d90\",\n                    \"path\": \"<Gamepad>/dpad\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Joystick\",\n                    \"id\": \"e25d9774-381c-4a61-b47c-7b6b299ad9f9\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"ff527021-f211-4c02-933e-5976594c46ed\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"eb480147-c587-4a33-85ed-eb0ab9942c43\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"85d264ad-e0a0-4565-b7ff-1a37edde51ac\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"cea9b045-a000-445b-95b8-0c171af70a3b\",\n                    \"path\": \"<Keyboard>/leftArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"4cda81dc-9edd-4e03-9d7c-a71a14345d0b\",\n                    \"path\": \"<Keyboard>/rightArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"9e92bb26-7e3b-4ec4-b06b-3c8f8e498ddc\",\n                    \"path\": \"*/{Submit}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Submit\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"82627dcc-3b13-4ba9-841d-e4b746d6553e\",\n                    \"path\": \"*/{Cancel}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Cancel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c52c8e0b-8179-41d3-b8a1-d149033bbe86\",\n                    \"path\": \"<Mouse>/position\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Point\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"e1394cbc-336e-44ce-9ea8-6007ed6193f7\",\n                    \"path\": \"<Pen>/position\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Point\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"4faf7dc9-b979-4210-aa8c-e808e1ef89f5\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard & Mouse\",\n                    \"action\": \"Click\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8d66d5ba-88d7-48e6-b1cd-198bbfef7ace\",\n                    \"path\": \"<Pen>/tip\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard & Mouse\",\n                    \"action\": \"Click\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"38c99815-14ea-4617-8627-164d27641299\",\n                    \"path\": \"<Mouse>/scroll\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard & Mouse\",\n                    \"action\": \"ScrollWheel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"24066f69-da47-44f3-a07e-0015fb02eb2e\",\n                    \"path\": \"<Mouse>/middleButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard & Mouse\",\n                    \"action\": \"MiddleClick\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"4c191405-5738-4d4b-a523-c6a301dbf754\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard & Mouse\",\n                    \"action\": \"RightClick\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"9463292f-a2ff-4649-a9c3-067667e79776\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ScrollSublist\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"9bc3b935-5dc2-4404-8371-7b2485dff1ce\",\n                    \"path\": \"<Gamepad>/rightStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"ScrollSublist\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"7ade12cb-a9f7-4a5a-96ae-e9fe630b2134\",\n                    \"path\": \"<Gamepad>/rightStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"ScrollSublist\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"5f79f2a3-f8fc-432c-ae63-af30cb39d55d\",\n                    \"path\": \"<Gamepad>/rightStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"ScrollSublist\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"2868eca4-a73a-4197-9c69-ddeb36bcf151\",\n                    \"path\": \"<Gamepad>/rightStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"ScrollSublist\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"5b1930a7-f80a-47a3-a5fc-ae79cf605e47\",\n                    \"path\": \"<Keyboard>/escape\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Pause\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"4fa5dbc3-d196-44a5-b7b0-0ea98081c9de\",\n                    \"path\": \"<Gamepad>/start\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Pause\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Movement\",\n            \"id\": \"e96bd924-debe-467e-b08f-8b58a3e62a8e\",\n            \"actions\": [\n                {\n                    \"name\": \"Move\",\n                    \"type\": \"Value\",\n                    \"id\": \"cb0ce271-47aa-4c76-82e1-9c39bb2a7eb3\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Look\",\n                    \"type\": \"Value\",\n                    \"id\": \"7ef2043f-2b68-4e31-9373-8e06a7366297\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Dodge\",\n                    \"type\": \"Button\",\n                    \"id\": \"33b91605-d5d0-4013-9789-7592610c7cf8\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Slide\",\n                    \"type\": \"Button\",\n                    \"id\": \"624c1b28-2b1e-4f89-bbb5-17cc64cba594\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Jump\",\n                    \"type\": \"Button\",\n                    \"id\": \"0fb09bdc-b16f-45ea-b0e1-9ae06cd92ce9\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"WASD\",\n                    \"id\": \"a431c87e-8bbb-44ed-9798-da71bb2c7d86\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"8d3ef497-0b3d-4b31-86a5-5663b2ba2ffa\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"4ef08ab9-0134-405a-b2ba-eac89353df45\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard & Mouse\",\n            [...string is too long...]");
		this.m_UI = this.asset.FindActionMap("UI", true);
		this.m_UI_Navigate = this.m_UI.FindAction("Navigate", true);
		this.m_UI_Submit = this.m_UI.FindAction("Submit", true);
		this.m_UI_Cancel = this.m_UI.FindAction("Cancel", true);
		this.m_UI_Point = this.m_UI.FindAction("Point", true);
		this.m_UI_Click = this.m_UI.FindAction("Click", true);
		this.m_UI_ScrollWheel = this.m_UI.FindAction("ScrollWheel", true);
		this.m_UI_MiddleClick = this.m_UI.FindAction("MiddleClick", true);
		this.m_UI_RightClick = this.m_UI.FindAction("RightClick", true);
		this.m_UI_TrackedDevicePosition = this.m_UI.FindAction("TrackedDevicePosition", true);
		this.m_UI_TrackedDeviceOrientation = this.m_UI.FindAction("TrackedDeviceOrientation", true);
		this.m_UI_ScrollSublist = this.m_UI.FindAction("ScrollSublist", true);
		this.m_UI_Pause = this.m_UI.FindAction("Pause", true);
		this.m_Movement = this.asset.FindActionMap("Movement", true);
		this.m_Movement_Move = this.m_Movement.FindAction("Move", true);
		this.m_Movement_Look = this.m_Movement.FindAction("Look", true);
		this.m_Movement_Dodge = this.m_Movement.FindAction("Dodge", true);
		this.m_Movement_Slide = this.m_Movement.FindAction("Slide", true);
		this.m_Movement_Jump = this.m_Movement.FindAction("Jump", true);
		this.m_Fist = this.asset.FindActionMap("Fist", true);
		this.m_Fist_Punch = this.m_Fist.FindAction("Punch", true);
		this.m_Fist_ChangeFist = this.m_Fist.FindAction("Change Fist", true);
		this.m_Fist_PunchFeedbacker = this.m_Fist.FindAction("Punch (Feedbacker)", true);
		this.m_Fist_PunchKnuckleblaster = this.m_Fist.FindAction("Punch (Knuckleblaster)", true);
		this.m_Fist_Hook = this.m_Fist.FindAction("Hook", true);
		this.m_Weapon = this.asset.FindActionMap("Weapon", true);
		this.m_Weapon_PrimaryFire = this.m_Weapon.FindAction("Primary Fire", true);
		this.m_Weapon_SecondaryFire = this.m_Weapon.FindAction("Secondary Fire", true);
		this.m_Weapon_NextVariation = this.m_Weapon.FindAction("Next Variation", true);
		this.m_Weapon_PreviousVariation = this.m_Weapon.FindAction("Previous Variation", true);
		this.m_Weapon_Revolver = this.m_Weapon.FindAction("Revolver", true);
		this.m_Weapon_Shotgun = this.m_Weapon.FindAction("Shotgun", true);
		this.m_Weapon_Nailgun = this.m_Weapon.FindAction("Nailgun", true);
		this.m_Weapon_Railcannon = this.m_Weapon.FindAction("Railcannon", true);
		this.m_Weapon_RocketLauncher = this.m_Weapon.FindAction("Rocket Launcher", true);
		this.m_Weapon_SpawnerArm = this.m_Weapon.FindAction("Spawner Arm", true);
		this.m_Weapon_NextWeapon = this.m_Weapon.FindAction("Next Weapon", true);
		this.m_Weapon_PreviousWeapon = this.m_Weapon.FindAction("Previous Weapon", true);
		this.m_Weapon_LastUsedWeapon = this.m_Weapon.FindAction("Last Used Weapon", true);
		this.m_Weapon_WheelLook = this.m_Weapon.FindAction("WheelLook", true);
		this.m_Weapon_VariationSlot1 = this.m_Weapon.FindAction("Variation Slot 1", true);
		this.m_Weapon_VariationSlot2 = this.m_Weapon.FindAction("Variation Slot 2", true);
		this.m_Weapon_VariationSlot3 = this.m_Weapon.FindAction("Variation Slot 3", true);
		this.m_HUD = this.asset.FindActionMap("HUD", true);
		this.m_HUD_Stats = this.m_HUD.FindAction("Stats", true);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000024D3 File Offset: 0x000006D3
	public void Dispose()
	{
		Object.Destroy(this.asset);
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000006 RID: 6 RVA: 0x000024E0 File Offset: 0x000006E0
	// (set) Token: 0x06000007 RID: 7 RVA: 0x000024ED File Offset: 0x000006ED
	public InputBinding? bindingMask
	{
		get
		{
			return this.asset.bindingMask;
		}
		set
		{
			this.asset.bindingMask = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000024FB File Offset: 0x000006FB
	// (set) Token: 0x06000009 RID: 9 RVA: 0x00002508 File Offset: 0x00000708
	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return this.asset.devices;
		}
		set
		{
			this.asset.devices = value;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600000A RID: 10 RVA: 0x00002516 File Offset: 0x00000716
	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			return this.asset.controlSchemes;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002523 File Offset: 0x00000723
	public bool Contains(InputAction action)
	{
		return this.asset.Contains(action);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002531 File Offset: 0x00000731
	public IEnumerator<InputAction> GetEnumerator()
	{
		return this.asset.GetEnumerator();
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000253E File Offset: 0x0000073E
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002546 File Offset: 0x00000746
	public void Enable()
	{
		this.asset.Enable();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002553 File Offset: 0x00000753
	public void Disable()
	{
		this.asset.Disable();
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000010 RID: 16 RVA: 0x00002560 File Offset: 0x00000760
	public IEnumerable<InputBinding> bindings
	{
		get
		{
			return this.asset.bindings;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000256D File Offset: 0x0000076D
	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return this.asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000257C File Offset: 0x0000077C
	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return this.asset.FindBinding(bindingMask, out action);
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000013 RID: 19 RVA: 0x0000258B File Offset: 0x0000078B
	public InputActions.UIActions UI
	{
		get
		{
			return new InputActions.UIActions(this);
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002593 File Offset: 0x00000793
	public InputActions.MovementActions Movement
	{
		get
		{
			return new InputActions.MovementActions(this);
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000015 RID: 21 RVA: 0x0000259B File Offset: 0x0000079B
	public InputActions.FistActions Fist
	{
		get
		{
			return new InputActions.FistActions(this);
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000016 RID: 22 RVA: 0x000025A3 File Offset: 0x000007A3
	public InputActions.WeaponActions Weapon
	{
		get
		{
			return new InputActions.WeaponActions(this);
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000017 RID: 23 RVA: 0x000025AB File Offset: 0x000007AB
	public InputActions.HUDActions HUD
	{
		get
		{
			return new InputActions.HUDActions(this);
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000018 RID: 24 RVA: 0x000025B4 File Offset: 0x000007B4
	public InputControlScheme KeyboardMouseScheme
	{
		get
		{
			if (this.m_KeyboardMouseSchemeIndex == -1)
			{
				this.m_KeyboardMouseSchemeIndex = this.asset.FindControlSchemeIndex("Keyboard & Mouse");
			}
			return this.asset.controlSchemes[this.m_KeyboardMouseSchemeIndex];
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000019 RID: 25 RVA: 0x000025FC File Offset: 0x000007FC
	public InputControlScheme GamepadScheme
	{
		get
		{
			if (this.m_GamepadSchemeIndex == -1)
			{
				this.m_GamepadSchemeIndex = this.asset.FindControlSchemeIndex("Gamepad");
			}
			return this.asset.controlSchemes[this.m_GamepadSchemeIndex];
		}
	}

	// Token: 0x04000002 RID: 2
	private readonly InputActionMap m_UI;

	// Token: 0x04000003 RID: 3
	private List<InputActions.IUIActions> m_UIActionsCallbackInterfaces = new List<InputActions.IUIActions>();

	// Token: 0x04000004 RID: 4
	private readonly InputAction m_UI_Navigate;

	// Token: 0x04000005 RID: 5
	private readonly InputAction m_UI_Submit;

	// Token: 0x04000006 RID: 6
	private readonly InputAction m_UI_Cancel;

	// Token: 0x04000007 RID: 7
	private readonly InputAction m_UI_Point;

	// Token: 0x04000008 RID: 8
	private readonly InputAction m_UI_Click;

	// Token: 0x04000009 RID: 9
	private readonly InputAction m_UI_ScrollWheel;

	// Token: 0x0400000A RID: 10
	private readonly InputAction m_UI_MiddleClick;

	// Token: 0x0400000B RID: 11
	private readonly InputAction m_UI_RightClick;

	// Token: 0x0400000C RID: 12
	private readonly InputAction m_UI_TrackedDevicePosition;

	// Token: 0x0400000D RID: 13
	private readonly InputAction m_UI_TrackedDeviceOrientation;

	// Token: 0x0400000E RID: 14
	private readonly InputAction m_UI_ScrollSublist;

	// Token: 0x0400000F RID: 15
	private readonly InputAction m_UI_Pause;

	// Token: 0x04000010 RID: 16
	private readonly InputActionMap m_Movement;

	// Token: 0x04000011 RID: 17
	private List<InputActions.IMovementActions> m_MovementActionsCallbackInterfaces = new List<InputActions.IMovementActions>();

	// Token: 0x04000012 RID: 18
	private readonly InputAction m_Movement_Move;

	// Token: 0x04000013 RID: 19
	private readonly InputAction m_Movement_Look;

	// Token: 0x04000014 RID: 20
	private readonly InputAction m_Movement_Dodge;

	// Token: 0x04000015 RID: 21
	private readonly InputAction m_Movement_Slide;

	// Token: 0x04000016 RID: 22
	private readonly InputAction m_Movement_Jump;

	// Token: 0x04000017 RID: 23
	private readonly InputActionMap m_Fist;

	// Token: 0x04000018 RID: 24
	private List<InputActions.IFistActions> m_FistActionsCallbackInterfaces = new List<InputActions.IFistActions>();

	// Token: 0x04000019 RID: 25
	private readonly InputAction m_Fist_Punch;

	// Token: 0x0400001A RID: 26
	private readonly InputAction m_Fist_ChangeFist;

	// Token: 0x0400001B RID: 27
	private readonly InputAction m_Fist_PunchFeedbacker;

	// Token: 0x0400001C RID: 28
	private readonly InputAction m_Fist_PunchKnuckleblaster;

	// Token: 0x0400001D RID: 29
	private readonly InputAction m_Fist_Hook;

	// Token: 0x0400001E RID: 30
	private readonly InputActionMap m_Weapon;

	// Token: 0x0400001F RID: 31
	private List<InputActions.IWeaponActions> m_WeaponActionsCallbackInterfaces = new List<InputActions.IWeaponActions>();

	// Token: 0x04000020 RID: 32
	private readonly InputAction m_Weapon_PrimaryFire;

	// Token: 0x04000021 RID: 33
	private readonly InputAction m_Weapon_SecondaryFire;

	// Token: 0x04000022 RID: 34
	private readonly InputAction m_Weapon_NextVariation;

	// Token: 0x04000023 RID: 35
	private readonly InputAction m_Weapon_PreviousVariation;

	// Token: 0x04000024 RID: 36
	private readonly InputAction m_Weapon_Revolver;

	// Token: 0x04000025 RID: 37
	private readonly InputAction m_Weapon_Shotgun;

	// Token: 0x04000026 RID: 38
	private readonly InputAction m_Weapon_Nailgun;

	// Token: 0x04000027 RID: 39
	private readonly InputAction m_Weapon_Railcannon;

	// Token: 0x04000028 RID: 40
	private readonly InputAction m_Weapon_RocketLauncher;

	// Token: 0x04000029 RID: 41
	private readonly InputAction m_Weapon_SpawnerArm;

	// Token: 0x0400002A RID: 42
	private readonly InputAction m_Weapon_NextWeapon;

	// Token: 0x0400002B RID: 43
	private readonly InputAction m_Weapon_PreviousWeapon;

	// Token: 0x0400002C RID: 44
	private readonly InputAction m_Weapon_LastUsedWeapon;

	// Token: 0x0400002D RID: 45
	private readonly InputAction m_Weapon_WheelLook;

	// Token: 0x0400002E RID: 46
	private readonly InputAction m_Weapon_VariationSlot1;

	// Token: 0x0400002F RID: 47
	private readonly InputAction m_Weapon_VariationSlot2;

	// Token: 0x04000030 RID: 48
	private readonly InputAction m_Weapon_VariationSlot3;

	// Token: 0x04000031 RID: 49
	private readonly InputActionMap m_HUD;

	// Token: 0x04000032 RID: 50
	private List<InputActions.IHUDActions> m_HUDActionsCallbackInterfaces = new List<InputActions.IHUDActions>();

	// Token: 0x04000033 RID: 51
	private readonly InputAction m_HUD_Stats;

	// Token: 0x04000034 RID: 52
	private int m_KeyboardMouseSchemeIndex = -1;

	// Token: 0x04000035 RID: 53
	private int m_GamepadSchemeIndex = -1;

	// Token: 0x02000005 RID: 5
	public struct UIActions
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002641 File Offset: 0x00000841
		public UIActions(InputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000264A File Offset: 0x0000084A
		public InputAction Navigate
		{
			get
			{
				return this.m_Wrapper.m_UI_Navigate;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002657 File Offset: 0x00000857
		public InputAction Submit
		{
			get
			{
				return this.m_Wrapper.m_UI_Submit;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002664 File Offset: 0x00000864
		public InputAction Cancel
		{
			get
			{
				return this.m_Wrapper.m_UI_Cancel;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002671 File Offset: 0x00000871
		public InputAction Point
		{
			get
			{
				return this.m_Wrapper.m_UI_Point;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000267E File Offset: 0x0000087E
		public InputAction Click
		{
			get
			{
				return this.m_Wrapper.m_UI_Click;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000268B File Offset: 0x0000088B
		public InputAction ScrollWheel
		{
			get
			{
				return this.m_Wrapper.m_UI_ScrollWheel;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002698 File Offset: 0x00000898
		public InputAction MiddleClick
		{
			get
			{
				return this.m_Wrapper.m_UI_MiddleClick;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000026A5 File Offset: 0x000008A5
		public InputAction RightClick
		{
			get
			{
				return this.m_Wrapper.m_UI_RightClick;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000026B2 File Offset: 0x000008B2
		public InputAction TrackedDevicePosition
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDevicePosition;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000026BF File Offset: 0x000008BF
		public InputAction TrackedDeviceOrientation
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDeviceOrientation;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000026CC File Offset: 0x000008CC
		public InputAction ScrollSublist
		{
			get
			{
				return this.m_Wrapper.m_UI_ScrollSublist;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026D9 File Offset: 0x000008D9
		public InputAction Pause
		{
			get
			{
				return this.m_Wrapper.m_UI_Pause;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026E6 File Offset: 0x000008E6
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_UI;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000026F3 File Offset: 0x000008F3
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002700 File Offset: 0x00000900
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600002A RID: 42 RVA: 0x0000270D File Offset: 0x0000090D
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000271A File Offset: 0x0000091A
		public static implicit operator InputActionMap(InputActions.UIActions set)
		{
			return set.Get();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002724 File Offset: 0x00000924
		public void AddCallbacks(InputActions.IUIActions instance)
		{
			if (instance == null || this.m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
			this.Navigate.started += instance.OnNavigate;
			this.Navigate.performed += instance.OnNavigate;
			this.Navigate.canceled += instance.OnNavigate;
			this.Submit.started += instance.OnSubmit;
			this.Submit.performed += instance.OnSubmit;
			this.Submit.canceled += instance.OnSubmit;
			this.Cancel.started += instance.OnCancel;
			this.Cancel.performed += instance.OnCancel;
			this.Cancel.canceled += instance.OnCancel;
			this.Point.started += instance.OnPoint;
			this.Point.performed += instance.OnPoint;
			this.Point.canceled += instance.OnPoint;
			this.Click.started += instance.OnClick;
			this.Click.performed += instance.OnClick;
			this.Click.canceled += instance.OnClick;
			this.ScrollWheel.started += instance.OnScrollWheel;
			this.ScrollWheel.performed += instance.OnScrollWheel;
			this.ScrollWheel.canceled += instance.OnScrollWheel;
			this.MiddleClick.started += instance.OnMiddleClick;
			this.MiddleClick.performed += instance.OnMiddleClick;
			this.MiddleClick.canceled += instance.OnMiddleClick;
			this.RightClick.started += instance.OnRightClick;
			this.RightClick.performed += instance.OnRightClick;
			this.RightClick.canceled += instance.OnRightClick;
			this.TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
			this.TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
			this.ScrollSublist.started += instance.OnScrollSublist;
			this.ScrollSublist.performed += instance.OnScrollSublist;
			this.ScrollSublist.canceled += instance.OnScrollSublist;
			this.Pause.started += instance.OnPause;
			this.Pause.performed += instance.OnPause;
			this.Pause.canceled += instance.OnPause;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002ABC File Offset: 0x00000CBC
		private void UnregisterCallbacks(InputActions.IUIActions instance)
		{
			this.Navigate.started -= instance.OnNavigate;
			this.Navigate.performed -= instance.OnNavigate;
			this.Navigate.canceled -= instance.OnNavigate;
			this.Submit.started -= instance.OnSubmit;
			this.Submit.performed -= instance.OnSubmit;
			this.Submit.canceled -= instance.OnSubmit;
			this.Cancel.started -= instance.OnCancel;
			this.Cancel.performed -= instance.OnCancel;
			this.Cancel.canceled -= instance.OnCancel;
			this.Point.started -= instance.OnPoint;
			this.Point.performed -= instance.OnPoint;
			this.Point.canceled -= instance.OnPoint;
			this.Click.started -= instance.OnClick;
			this.Click.performed -= instance.OnClick;
			this.Click.canceled -= instance.OnClick;
			this.ScrollWheel.started -= instance.OnScrollWheel;
			this.ScrollWheel.performed -= instance.OnScrollWheel;
			this.ScrollWheel.canceled -= instance.OnScrollWheel;
			this.MiddleClick.started -= instance.OnMiddleClick;
			this.MiddleClick.performed -= instance.OnMiddleClick;
			this.MiddleClick.canceled -= instance.OnMiddleClick;
			this.RightClick.started -= instance.OnRightClick;
			this.RightClick.performed -= instance.OnRightClick;
			this.RightClick.canceled -= instance.OnRightClick;
			this.TrackedDevicePosition.started -= instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.performed -= instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.canceled -= instance.OnTrackedDevicePosition;
			this.TrackedDeviceOrientation.started -= instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.performed -= instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.canceled -= instance.OnTrackedDeviceOrientation;
			this.ScrollSublist.started -= instance.OnScrollSublist;
			this.ScrollSublist.performed -= instance.OnScrollSublist;
			this.ScrollSublist.canceled -= instance.OnScrollSublist;
			this.Pause.started -= instance.OnPause;
			this.Pause.performed -= instance.OnPause;
			this.Pause.canceled -= instance.OnPause;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002E29 File Offset: 0x00001029
		public void RemoveCallbacks(InputActions.IUIActions instance)
		{
			if (this.m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002E48 File Offset: 0x00001048
		public void SetCallbacks(InputActions.IUIActions instance)
		{
			foreach (InputActions.IUIActions iuiactions in this.m_Wrapper.m_UIActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(iuiactions);
			}
			this.m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x04000036 RID: 54
		private InputActions m_Wrapper;
	}

	// Token: 0x02000006 RID: 6
	public struct MovementActions
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002EB8 File Offset: 0x000010B8
		public MovementActions(InputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002EC1 File Offset: 0x000010C1
		public InputAction Move
		{
			get
			{
				return this.m_Wrapper.m_Movement_Move;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002ECE File Offset: 0x000010CE
		public InputAction Look
		{
			get
			{
				return this.m_Wrapper.m_Movement_Look;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002EDB File Offset: 0x000010DB
		public InputAction Dodge
		{
			get
			{
				return this.m_Wrapper.m_Movement_Dodge;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002EE8 File Offset: 0x000010E8
		public InputAction Slide
		{
			get
			{
				return this.m_Wrapper.m_Movement_Slide;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002EF5 File Offset: 0x000010F5
		public InputAction Jump
		{
			get
			{
				return this.m_Wrapper.m_Movement_Jump;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F02 File Offset: 0x00001102
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Movement;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002F0F File Offset: 0x0000110F
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F1C File Offset: 0x0000111C
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002F29 File Offset: 0x00001129
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002F36 File Offset: 0x00001136
		public static implicit operator InputActionMap(InputActions.MovementActions set)
		{
			return set.Get();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002F40 File Offset: 0x00001140
		public void AddCallbacks(InputActions.IMovementActions instance)
		{
			if (instance == null || this.m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
			this.Move.started += instance.OnMove;
			this.Move.performed += instance.OnMove;
			this.Move.canceled += instance.OnMove;
			this.Look.started += instance.OnLook;
			this.Look.performed += instance.OnLook;
			this.Look.canceled += instance.OnLook;
			this.Dodge.started += instance.OnDodge;
			this.Dodge.performed += instance.OnDodge;
			this.Dodge.canceled += instance.OnDodge;
			this.Slide.started += instance.OnSlide;
			this.Slide.performed += instance.OnSlide;
			this.Slide.canceled += instance.OnSlide;
			this.Jump.started += instance.OnJump;
			this.Jump.performed += instance.OnJump;
			this.Jump.canceled += instance.OnJump;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000030E0 File Offset: 0x000012E0
		private void UnregisterCallbacks(InputActions.IMovementActions instance)
		{
			this.Move.started -= instance.OnMove;
			this.Move.performed -= instance.OnMove;
			this.Move.canceled -= instance.OnMove;
			this.Look.started -= instance.OnLook;
			this.Look.performed -= instance.OnLook;
			this.Look.canceled -= instance.OnLook;
			this.Dodge.started -= instance.OnDodge;
			this.Dodge.performed -= instance.OnDodge;
			this.Dodge.canceled -= instance.OnDodge;
			this.Slide.started -= instance.OnSlide;
			this.Slide.performed -= instance.OnSlide;
			this.Slide.canceled -= instance.OnSlide;
			this.Jump.started -= instance.OnJump;
			this.Jump.performed -= instance.OnJump;
			this.Jump.canceled -= instance.OnJump;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003255 File Offset: 0x00001455
		public void RemoveCallbacks(InputActions.IMovementActions instance)
		{
			if (this.m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003274 File Offset: 0x00001474
		public void SetCallbacks(InputActions.IMovementActions instance)
		{
			foreach (InputActions.IMovementActions movementActions in this.m_Wrapper.m_MovementActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(movementActions);
			}
			this.m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x04000037 RID: 55
		private InputActions m_Wrapper;
	}

	// Token: 0x02000007 RID: 7
	public struct FistActions
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000032E4 File Offset: 0x000014E4
		public FistActions(InputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000032ED File Offset: 0x000014ED
		public InputAction Punch
		{
			get
			{
				return this.m_Wrapper.m_Fist_Punch;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000032FA File Offset: 0x000014FA
		public InputAction ChangeFist
		{
			get
			{
				return this.m_Wrapper.m_Fist_ChangeFist;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003307 File Offset: 0x00001507
		public InputAction PunchFeedbacker
		{
			get
			{
				return this.m_Wrapper.m_Fist_PunchFeedbacker;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003314 File Offset: 0x00001514
		public InputAction PunchKnuckleblaster
		{
			get
			{
				return this.m_Wrapper.m_Fist_PunchKnuckleblaster;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003321 File Offset: 0x00001521
		public InputAction Hook
		{
			get
			{
				return this.m_Wrapper.m_Fist_Hook;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000332E File Offset: 0x0000152E
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Fist;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000333B File Offset: 0x0000153B
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003348 File Offset: 0x00001548
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003355 File Offset: 0x00001555
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003362 File Offset: 0x00001562
		public static implicit operator InputActionMap(InputActions.FistActions set)
		{
			return set.Get();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000336C File Offset: 0x0000156C
		public void AddCallbacks(InputActions.IFistActions instance)
		{
			if (instance == null || this.m_Wrapper.m_FistActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_FistActionsCallbackInterfaces.Add(instance);
			this.Punch.started += instance.OnPunch;
			this.Punch.performed += instance.OnPunch;
			this.Punch.canceled += instance.OnPunch;
			this.ChangeFist.started += instance.OnChangeFist;
			this.ChangeFist.performed += instance.OnChangeFist;
			this.ChangeFist.canceled += instance.OnChangeFist;
			this.PunchFeedbacker.started += instance.OnPunchFeedbacker;
			this.PunchFeedbacker.performed += instance.OnPunchFeedbacker;
			this.PunchFeedbacker.canceled += instance.OnPunchFeedbacker;
			this.PunchKnuckleblaster.started += instance.OnPunchKnuckleblaster;
			this.PunchKnuckleblaster.performed += instance.OnPunchKnuckleblaster;
			this.PunchKnuckleblaster.canceled += instance.OnPunchKnuckleblaster;
			this.Hook.started += instance.OnHook;
			this.Hook.performed += instance.OnHook;
			this.Hook.canceled += instance.OnHook;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000350C File Offset: 0x0000170C
		private void UnregisterCallbacks(InputActions.IFistActions instance)
		{
			this.Punch.started -= instance.OnPunch;
			this.Punch.performed -= instance.OnPunch;
			this.Punch.canceled -= instance.OnPunch;
			this.ChangeFist.started -= instance.OnChangeFist;
			this.ChangeFist.performed -= instance.OnChangeFist;
			this.ChangeFist.canceled -= instance.OnChangeFist;
			this.PunchFeedbacker.started -= instance.OnPunchFeedbacker;
			this.PunchFeedbacker.performed -= instance.OnPunchFeedbacker;
			this.PunchFeedbacker.canceled -= instance.OnPunchFeedbacker;
			this.PunchKnuckleblaster.started -= instance.OnPunchKnuckleblaster;
			this.PunchKnuckleblaster.performed -= instance.OnPunchKnuckleblaster;
			this.PunchKnuckleblaster.canceled -= instance.OnPunchKnuckleblaster;
			this.Hook.started -= instance.OnHook;
			this.Hook.performed -= instance.OnHook;
			this.Hook.canceled -= instance.OnHook;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003681 File Offset: 0x00001881
		public void RemoveCallbacks(InputActions.IFistActions instance)
		{
			if (this.m_Wrapper.m_FistActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000036A0 File Offset: 0x000018A0
		public void SetCallbacks(InputActions.IFistActions instance)
		{
			foreach (InputActions.IFistActions fistActions in this.m_Wrapper.m_FistActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(fistActions);
			}
			this.m_Wrapper.m_FistActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x04000038 RID: 56
		private InputActions m_Wrapper;
	}

	// Token: 0x02000008 RID: 8
	public struct WeaponActions
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00003710 File Offset: 0x00001910
		public WeaponActions(InputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003719 File Offset: 0x00001919
		public InputAction PrimaryFire
		{
			get
			{
				return this.m_Wrapper.m_Weapon_PrimaryFire;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003726 File Offset: 0x00001926
		public InputAction SecondaryFire
		{
			get
			{
				return this.m_Wrapper.m_Weapon_SecondaryFire;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003733 File Offset: 0x00001933
		public InputAction NextVariation
		{
			get
			{
				return this.m_Wrapper.m_Weapon_NextVariation;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003740 File Offset: 0x00001940
		public InputAction PreviousVariation
		{
			get
			{
				return this.m_Wrapper.m_Weapon_PreviousVariation;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000374D File Offset: 0x0000194D
		public InputAction Revolver
		{
			get
			{
				return this.m_Wrapper.m_Weapon_Revolver;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000375A File Offset: 0x0000195A
		public InputAction Shotgun
		{
			get
			{
				return this.m_Wrapper.m_Weapon_Shotgun;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003767 File Offset: 0x00001967
		public InputAction Nailgun
		{
			get
			{
				return this.m_Wrapper.m_Weapon_Nailgun;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003774 File Offset: 0x00001974
		public InputAction Railcannon
		{
			get
			{
				return this.m_Wrapper.m_Weapon_Railcannon;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003781 File Offset: 0x00001981
		public InputAction RocketLauncher
		{
			get
			{
				return this.m_Wrapper.m_Weapon_RocketLauncher;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000058 RID: 88 RVA: 0x0000378E File Offset: 0x0000198E
		public InputAction SpawnerArm
		{
			get
			{
				return this.m_Wrapper.m_Weapon_SpawnerArm;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000379B File Offset: 0x0000199B
		public InputAction NextWeapon
		{
			get
			{
				return this.m_Wrapper.m_Weapon_NextWeapon;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000037A8 File Offset: 0x000019A8
		public InputAction PreviousWeapon
		{
			get
			{
				return this.m_Wrapper.m_Weapon_PreviousWeapon;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000037B5 File Offset: 0x000019B5
		public InputAction LastUsedWeapon
		{
			get
			{
				return this.m_Wrapper.m_Weapon_LastUsedWeapon;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000037C2 File Offset: 0x000019C2
		public InputAction WheelLook
		{
			get
			{
				return this.m_Wrapper.m_Weapon_WheelLook;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000037CF File Offset: 0x000019CF
		public InputAction VariationSlot1
		{
			get
			{
				return this.m_Wrapper.m_Weapon_VariationSlot1;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000037DC File Offset: 0x000019DC
		public InputAction VariationSlot2
		{
			get
			{
				return this.m_Wrapper.m_Weapon_VariationSlot2;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000037E9 File Offset: 0x000019E9
		public InputAction VariationSlot3
		{
			get
			{
				return this.m_Wrapper.m_Weapon_VariationSlot3;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000037F6 File Offset: 0x000019F6
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Weapon;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003803 File Offset: 0x00001A03
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003810 File Offset: 0x00001A10
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000381D File Offset: 0x00001A1D
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000382A File Offset: 0x00001A2A
		public static implicit operator InputActionMap(InputActions.WeaponActions set)
		{
			return set.Get();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003834 File Offset: 0x00001A34
		public void AddCallbacks(InputActions.IWeaponActions instance)
		{
			if (instance == null || this.m_Wrapper.m_WeaponActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_WeaponActionsCallbackInterfaces.Add(instance);
			this.PrimaryFire.started += instance.OnPrimaryFire;
			this.PrimaryFire.performed += instance.OnPrimaryFire;
			this.PrimaryFire.canceled += instance.OnPrimaryFire;
			this.SecondaryFire.started += instance.OnSecondaryFire;
			this.SecondaryFire.performed += instance.OnSecondaryFire;
			this.SecondaryFire.canceled += instance.OnSecondaryFire;
			this.NextVariation.started += instance.OnNextVariation;
			this.NextVariation.performed += instance.OnNextVariation;
			this.NextVariation.canceled += instance.OnNextVariation;
			this.PreviousVariation.started += instance.OnPreviousVariation;
			this.PreviousVariation.performed += instance.OnPreviousVariation;
			this.PreviousVariation.canceled += instance.OnPreviousVariation;
			this.Revolver.started += instance.OnRevolver;
			this.Revolver.performed += instance.OnRevolver;
			this.Revolver.canceled += instance.OnRevolver;
			this.Shotgun.started += instance.OnShotgun;
			this.Shotgun.performed += instance.OnShotgun;
			this.Shotgun.canceled += instance.OnShotgun;
			this.Nailgun.started += instance.OnNailgun;
			this.Nailgun.performed += instance.OnNailgun;
			this.Nailgun.canceled += instance.OnNailgun;
			this.Railcannon.started += instance.OnRailcannon;
			this.Railcannon.performed += instance.OnRailcannon;
			this.Railcannon.canceled += instance.OnRailcannon;
			this.RocketLauncher.started += instance.OnRocketLauncher;
			this.RocketLauncher.performed += instance.OnRocketLauncher;
			this.RocketLauncher.canceled += instance.OnRocketLauncher;
			this.SpawnerArm.started += instance.OnSpawnerArm;
			this.SpawnerArm.performed += instance.OnSpawnerArm;
			this.SpawnerArm.canceled += instance.OnSpawnerArm;
			this.NextWeapon.started += instance.OnNextWeapon;
			this.NextWeapon.performed += instance.OnNextWeapon;
			this.NextWeapon.canceled += instance.OnNextWeapon;
			this.PreviousWeapon.started += instance.OnPreviousWeapon;
			this.PreviousWeapon.performed += instance.OnPreviousWeapon;
			this.PreviousWeapon.canceled += instance.OnPreviousWeapon;
			this.LastUsedWeapon.started += instance.OnLastUsedWeapon;
			this.LastUsedWeapon.performed += instance.OnLastUsedWeapon;
			this.LastUsedWeapon.canceled += instance.OnLastUsedWeapon;
			this.WheelLook.started += instance.OnWheelLook;
			this.WheelLook.performed += instance.OnWheelLook;
			this.WheelLook.canceled += instance.OnWheelLook;
			this.VariationSlot1.started += instance.OnVariationSlot1;
			this.VariationSlot1.performed += instance.OnVariationSlot1;
			this.VariationSlot1.canceled += instance.OnVariationSlot1;
			this.VariationSlot2.started += instance.OnVariationSlot2;
			this.VariationSlot2.performed += instance.OnVariationSlot2;
			this.VariationSlot2.canceled += instance.OnVariationSlot2;
			this.VariationSlot3.started += instance.OnVariationSlot3;
			this.VariationSlot3.performed += instance.OnVariationSlot3;
			this.VariationSlot3.canceled += instance.OnVariationSlot3;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003D34 File Offset: 0x00001F34
		private void UnregisterCallbacks(InputActions.IWeaponActions instance)
		{
			this.PrimaryFire.started -= instance.OnPrimaryFire;
			this.PrimaryFire.performed -= instance.OnPrimaryFire;
			this.PrimaryFire.canceled -= instance.OnPrimaryFire;
			this.SecondaryFire.started -= instance.OnSecondaryFire;
			this.SecondaryFire.performed -= instance.OnSecondaryFire;
			this.SecondaryFire.canceled -= instance.OnSecondaryFire;
			this.NextVariation.started -= instance.OnNextVariation;
			this.NextVariation.performed -= instance.OnNextVariation;
			this.NextVariation.canceled -= instance.OnNextVariation;
			this.PreviousVariation.started -= instance.OnPreviousVariation;
			this.PreviousVariation.performed -= instance.OnPreviousVariation;
			this.PreviousVariation.canceled -= instance.OnPreviousVariation;
			this.Revolver.started -= instance.OnRevolver;
			this.Revolver.performed -= instance.OnRevolver;
			this.Revolver.canceled -= instance.OnRevolver;
			this.Shotgun.started -= instance.OnShotgun;
			this.Shotgun.performed -= instance.OnShotgun;
			this.Shotgun.canceled -= instance.OnShotgun;
			this.Nailgun.started -= instance.OnNailgun;
			this.Nailgun.performed -= instance.OnNailgun;
			this.Nailgun.canceled -= instance.OnNailgun;
			this.Railcannon.started -= instance.OnRailcannon;
			this.Railcannon.performed -= instance.OnRailcannon;
			this.Railcannon.canceled -= instance.OnRailcannon;
			this.RocketLauncher.started -= instance.OnRocketLauncher;
			this.RocketLauncher.performed -= instance.OnRocketLauncher;
			this.RocketLauncher.canceled -= instance.OnRocketLauncher;
			this.SpawnerArm.started -= instance.OnSpawnerArm;
			this.SpawnerArm.performed -= instance.OnSpawnerArm;
			this.SpawnerArm.canceled -= instance.OnSpawnerArm;
			this.NextWeapon.started -= instance.OnNextWeapon;
			this.NextWeapon.performed -= instance.OnNextWeapon;
			this.NextWeapon.canceled -= instance.OnNextWeapon;
			this.PreviousWeapon.started -= instance.OnPreviousWeapon;
			this.PreviousWeapon.performed -= instance.OnPreviousWeapon;
			this.PreviousWeapon.canceled -= instance.OnPreviousWeapon;
			this.LastUsedWeapon.started -= instance.OnLastUsedWeapon;
			this.LastUsedWeapon.performed -= instance.OnLastUsedWeapon;
			this.LastUsedWeapon.canceled -= instance.OnLastUsedWeapon;
			this.WheelLook.started -= instance.OnWheelLook;
			this.WheelLook.performed -= instance.OnWheelLook;
			this.WheelLook.canceled -= instance.OnWheelLook;
			this.VariationSlot1.started -= instance.OnVariationSlot1;
			this.VariationSlot1.performed -= instance.OnVariationSlot1;
			this.VariationSlot1.canceled -= instance.OnVariationSlot1;
			this.VariationSlot2.started -= instance.OnVariationSlot2;
			this.VariationSlot2.performed -= instance.OnVariationSlot2;
			this.VariationSlot2.canceled -= instance.OnVariationSlot2;
			this.VariationSlot3.started -= instance.OnVariationSlot3;
			this.VariationSlot3.performed -= instance.OnVariationSlot3;
			this.VariationSlot3.canceled -= instance.OnVariationSlot3;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004209 File Offset: 0x00002409
		public void RemoveCallbacks(InputActions.IWeaponActions instance)
		{
			if (this.m_Wrapper.m_WeaponActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004228 File Offset: 0x00002428
		public void SetCallbacks(InputActions.IWeaponActions instance)
		{
			foreach (InputActions.IWeaponActions weaponActions in this.m_Wrapper.m_WeaponActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(weaponActions);
			}
			this.m_Wrapper.m_WeaponActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x04000039 RID: 57
		private InputActions m_Wrapper;
	}

	// Token: 0x02000009 RID: 9
	public struct HUDActions
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00004298 File Offset: 0x00002498
		public HUDActions(InputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000042A1 File Offset: 0x000024A1
		public InputAction Stats
		{
			get
			{
				return this.m_Wrapper.m_HUD_Stats;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000042AE File Offset: 0x000024AE
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_HUD;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000042BB File Offset: 0x000024BB
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000042C8 File Offset: 0x000024C8
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000042D5 File Offset: 0x000024D5
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000042E2 File Offset: 0x000024E2
		public static implicit operator InputActionMap(InputActions.HUDActions set)
		{
			return set.Get();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000042EC File Offset: 0x000024EC
		public void AddCallbacks(InputActions.IHUDActions instance)
		{
			if (instance == null || this.m_Wrapper.m_HUDActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_HUDActionsCallbackInterfaces.Add(instance);
			this.Stats.started += instance.OnStats;
			this.Stats.performed += instance.OnStats;
			this.Stats.canceled += instance.OnStats;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000436C File Offset: 0x0000256C
		private void UnregisterCallbacks(InputActions.IHUDActions instance)
		{
			this.Stats.started -= instance.OnStats;
			this.Stats.performed -= instance.OnStats;
			this.Stats.canceled -= instance.OnStats;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000043C1 File Offset: 0x000025C1
		public void RemoveCallbacks(InputActions.IHUDActions instance)
		{
			if (this.m_Wrapper.m_HUDActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000043E0 File Offset: 0x000025E0
		public void SetCallbacks(InputActions.IHUDActions instance)
		{
			foreach (InputActions.IHUDActions ihudactions in this.m_Wrapper.m_HUDActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(ihudactions);
			}
			this.m_Wrapper.m_HUDActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x0400003A RID: 58
		private InputActions m_Wrapper;
	}

	// Token: 0x0200000A RID: 10
	public interface IUIActions
	{
		// Token: 0x06000074 RID: 116
		void OnNavigate(InputAction.CallbackContext context);

		// Token: 0x06000075 RID: 117
		void OnSubmit(InputAction.CallbackContext context);

		// Token: 0x06000076 RID: 118
		void OnCancel(InputAction.CallbackContext context);

		// Token: 0x06000077 RID: 119
		void OnPoint(InputAction.CallbackContext context);

		// Token: 0x06000078 RID: 120
		void OnClick(InputAction.CallbackContext context);

		// Token: 0x06000079 RID: 121
		void OnScrollWheel(InputAction.CallbackContext context);

		// Token: 0x0600007A RID: 122
		void OnMiddleClick(InputAction.CallbackContext context);

		// Token: 0x0600007B RID: 123
		void OnRightClick(InputAction.CallbackContext context);

		// Token: 0x0600007C RID: 124
		void OnTrackedDevicePosition(InputAction.CallbackContext context);

		// Token: 0x0600007D RID: 125
		void OnTrackedDeviceOrientation(InputAction.CallbackContext context);

		// Token: 0x0600007E RID: 126
		void OnScrollSublist(InputAction.CallbackContext context);

		// Token: 0x0600007F RID: 127
		void OnPause(InputAction.CallbackContext context);
	}

	// Token: 0x0200000B RID: 11
	public interface IMovementActions
	{
		// Token: 0x06000080 RID: 128
		void OnMove(InputAction.CallbackContext context);

		// Token: 0x06000081 RID: 129
		void OnLook(InputAction.CallbackContext context);

		// Token: 0x06000082 RID: 130
		void OnDodge(InputAction.CallbackContext context);

		// Token: 0x06000083 RID: 131
		void OnSlide(InputAction.CallbackContext context);

		// Token: 0x06000084 RID: 132
		void OnJump(InputAction.CallbackContext context);
	}

	// Token: 0x0200000C RID: 12
	public interface IFistActions
	{
		// Token: 0x06000085 RID: 133
		void OnPunch(InputAction.CallbackContext context);

		// Token: 0x06000086 RID: 134
		void OnChangeFist(InputAction.CallbackContext context);

		// Token: 0x06000087 RID: 135
		void OnPunchFeedbacker(InputAction.CallbackContext context);

		// Token: 0x06000088 RID: 136
		void OnPunchKnuckleblaster(InputAction.CallbackContext context);

		// Token: 0x06000089 RID: 137
		void OnHook(InputAction.CallbackContext context);
	}

	// Token: 0x0200000D RID: 13
	public interface IWeaponActions
	{
		// Token: 0x0600008A RID: 138
		void OnPrimaryFire(InputAction.CallbackContext context);

		// Token: 0x0600008B RID: 139
		void OnSecondaryFire(InputAction.CallbackContext context);

		// Token: 0x0600008C RID: 140
		void OnNextVariation(InputAction.CallbackContext context);

		// Token: 0x0600008D RID: 141
		void OnPreviousVariation(InputAction.CallbackContext context);

		// Token: 0x0600008E RID: 142
		void OnRevolver(InputAction.CallbackContext context);

		// Token: 0x0600008F RID: 143
		void OnShotgun(InputAction.CallbackContext context);

		// Token: 0x06000090 RID: 144
		void OnNailgun(InputAction.CallbackContext context);

		// Token: 0x06000091 RID: 145
		void OnRailcannon(InputAction.CallbackContext context);

		// Token: 0x06000092 RID: 146
		void OnRocketLauncher(InputAction.CallbackContext context);

		// Token: 0x06000093 RID: 147
		void OnSpawnerArm(InputAction.CallbackContext context);

		// Token: 0x06000094 RID: 148
		void OnNextWeapon(InputAction.CallbackContext context);

		// Token: 0x06000095 RID: 149
		void OnPreviousWeapon(InputAction.CallbackContext context);

		// Token: 0x06000096 RID: 150
		void OnLastUsedWeapon(InputAction.CallbackContext context);

		// Token: 0x06000097 RID: 151
		void OnWheelLook(InputAction.CallbackContext context);

		// Token: 0x06000098 RID: 152
		void OnVariationSlot1(InputAction.CallbackContext context);

		// Token: 0x06000099 RID: 153
		void OnVariationSlot2(InputAction.CallbackContext context);

		// Token: 0x0600009A RID: 154
		void OnVariationSlot3(InputAction.CallbackContext context);
	}

	// Token: 0x0200000E RID: 14
	public interface IHUDActions
	{
		// Token: 0x0600009B RID: 155
		void OnStats(InputAction.CallbackContext context);
	}
}
