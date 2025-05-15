using System;
using UnityEngine;

// Token: 0x02000234 RID: 564
public class GoToTarget : MonoBehaviour
{
	// Token: 0x06000C07 RID: 3079 RVA: 0x000543E5 File Offset: 0x000525E5
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x000543F4 File Offset: 0x000525F4
	private void FixedUpdate()
	{
		if (!this.stopped)
		{
			if (this.easeIn && this.currentSpeed != this.speed)
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.speed, Time.fixedDeltaTime * (Mathf.Abs(this.currentSpeed + 1f) * this.easeInSpeed));
			}
			else
			{
				this.currentSpeed = this.speed;
			}
			if (Vector3.Distance(base.transform.position, this.target.position) < this.rb.velocity.magnitude * Time.fixedDeltaTime)
			{
				this.Activate();
			}
			else
			{
				this.rb.velocity = (this.target.position - base.transform.position).normalized * this.currentSpeed;
			}
			if (Vector3.Distance(base.transform.position, this.target.position) < 0.5f)
			{
				this.Activate();
			}
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00054504 File Offset: 0x00052704
	private void Activate()
	{
		if (this.activated)
		{
			return;
		}
		this.activated = true;
		if (this.events != null)
		{
			this.events.Invoke("");
		}
		ToDo toDo = this.onTargetReach;
		if (toDo == ToDo.Disable)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (toDo != ToDo.Destroy)
		{
			this.stopped = true;
			this.rb.velocity = Vector3.zero;
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000FC2 RID: 4034
	public ToDo onTargetReach;

	// Token: 0x04000FC3 RID: 4035
	public float speed;

	// Token: 0x04000FC4 RID: 4036
	public bool easeIn;

	// Token: 0x04000FC5 RID: 4037
	public float easeInSpeed;

	// Token: 0x04000FC6 RID: 4038
	private float currentSpeed;

	// Token: 0x04000FC7 RID: 4039
	public Transform target;

	// Token: 0x04000FC8 RID: 4040
	private Rigidbody rb;

	// Token: 0x04000FC9 RID: 4041
	private bool stopped;

	// Token: 0x04000FCA RID: 4042
	public UltrakillEvent events;

	// Token: 0x04000FCB RID: 4043
	[HideInInspector]
	public bool activated;
}
