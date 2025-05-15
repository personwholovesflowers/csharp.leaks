using System;

namespace Sandbox.Arm
{
	// Token: 0x0200056E RID: 1390
	public interface ISandboxArmMode
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06001F81 RID: 8065
		string Name { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001F82 RID: 8066
		bool CanOpenMenu { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001F83 RID: 8067
		bool Raycast { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06001F84 RID: 8068
		string Icon { get; }

		// Token: 0x06001F85 RID: 8069
		void OnEnable(SandboxArm hostArm);

		// Token: 0x06001F86 RID: 8070
		void OnDisable();

		// Token: 0x06001F87 RID: 8071
		void OnDestroy();

		// Token: 0x06001F88 RID: 8072
		void Update();

		// Token: 0x06001F89 RID: 8073
		void FixedUpdate();

		// Token: 0x06001F8A RID: 8074
		void OnPrimaryDown();

		// Token: 0x06001F8B RID: 8075
		void OnPrimaryUp();

		// Token: 0x06001F8C RID: 8076
		void OnSecondaryDown();

		// Token: 0x06001F8D RID: 8077
		void OnSecondaryUp();
	}
}
