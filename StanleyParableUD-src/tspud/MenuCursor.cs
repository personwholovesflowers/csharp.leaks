using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class MenuCursor : MonoBehaviour
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060006AB RID: 1707 RVA: 0x00023BF6 File Offset: 0x00021DF6
	// (set) Token: 0x060006AC RID: 1708 RVA: 0x00023BFE File Offset: 0x00021DFE
	public string hoveringOver { get; private set; }

	// Token: 0x060006AD RID: 1709 RVA: 0x00023C07 File Offset: 0x00021E07
	private void Awake()
	{
		this.RT = base.GetComponent<RectTransform>();
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00023C18 File Offset: 0x00021E18
	private void Update()
	{
		this.MovementController();
		this.MovementMouse();
		Ray ray = new Ray(this.camera.transform.position, base.transform.position - this.camera.transform.position);
		RaycastHit[] array = Physics.RaycastAll(ray.origin, ray.direction, 1000f);
		this.hoveringOver = "";
		for (int i = 0; i < array.Length; i++)
		{
			MenuButton component = array[i].collider.GetComponent<MenuButton>();
			if (component != null)
			{
				if (Singleton<GameMaster>.Instance.stanleyActions.UseAction.WasPressed)
				{
					component.OnHover();
					component.OnClick(array[i].point);
				}
				else if (Singleton<GameMaster>.Instance.stanleyActions.UseAction.IsPressed)
				{
					component.OnHover();
					component.OnHold(array[i].point);
				}
				else
				{
					component.OnHover();
				}
				this.hoveringOver = component.name;
			}
		}
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00023D30 File Offset: 0x00021F30
	private void MovementController()
	{
		Vector3 vector = Vector3.zero;
		vector += Vector3.right * Singleton<GameMaster>.Instance.stanleyActions.View.X;
		vector += Vector3.up * Singleton<GameMaster>.Instance.stanleyActions.View.Y;
		if (vector != Vector3.zero)
		{
			this.cursorSpeed += this.cursorAcceleration * Time.deltaTime;
			this.cursorSpeed = Mathf.Clamp(this.cursorSpeed, 0f, this.cursorMaxSpeed);
			vector = vector.normalized * Mathf.Pow(vector.magnitude, this.cursorRampPower);
			vector *= this.cursorSpeed;
			vector = this.RT.anchoredPosition3D + vector;
			vector.x = Mathf.Clamp(vector.x, 3f, 2048f);
			vector.y = Mathf.Clamp(vector.y, -1024f, -1f);
			this.RT.anchoredPosition = vector;
			return;
		}
		this.cursorSpeed = 0f;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00023E64 File Offset: 0x00022064
	private void MovementMouse()
	{
		Vector3 vector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * this.mouseSensitivity;
		vector = this.RT.anchoredPosition3D + vector;
		vector.x = Mathf.Clamp(vector.x, 3f, 2048f);
		vector.y = Mathf.Clamp(vector.y, -1024f, -1f);
		this.RT.anchoredPosition = vector;
	}

	// Token: 0x040006FF RID: 1791
	private RectTransform RT;

	// Token: 0x04000700 RID: 1792
	public GameObject camera;

	// Token: 0x04000701 RID: 1793
	[Space(10f)]
	public float cursorMaxSpeed = 15f;

	// Token: 0x04000702 RID: 1794
	public float cursorAcceleration = 1f;

	// Token: 0x04000703 RID: 1795
	public float cursorRampPower = 2f;

	// Token: 0x04000704 RID: 1796
	[Space(5f)]
	public float mouseSensitivity = 20f;

	// Token: 0x04000705 RID: 1797
	private float cursorSpeed;

	// Token: 0x04000707 RID: 1799
	private bool controller;

	// Token: 0x04000708 RID: 1800
	private bool mouse = true;
}
