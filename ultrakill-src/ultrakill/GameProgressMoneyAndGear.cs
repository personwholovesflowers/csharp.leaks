using System;

// Token: 0x02000217 RID: 535
[Serializable]
public class GameProgressMoneyAndGear
{
	// Token: 0x06000B42 RID: 2882 RVA: 0x00050993 File Offset: 0x0004EB93
	public GameProgressMoneyAndGear()
	{
		this.secretMissions = new int[10];
		this.newEnemiesFound = new int[Enum.GetValues(typeof(EnemyType)).Length];
	}

	// Token: 0x04000EE4 RID: 3812
	public int money;

	// Token: 0x04000EE5 RID: 3813
	public bool introSeen;

	// Token: 0x04000EE6 RID: 3814
	public bool tutorialBeat;

	// Token: 0x04000EE7 RID: 3815
	public bool clashModeUnlocked;

	// Token: 0x04000EE8 RID: 3816
	public bool ghostDroneModeUnlocked;

	// Token: 0x04000EE9 RID: 3817
	public int rev0;

	// Token: 0x04000EEA RID: 3818
	public int rev1;

	// Token: 0x04000EEB RID: 3819
	public int rev2;

	// Token: 0x04000EEC RID: 3820
	public int rev3;

	// Token: 0x04000EED RID: 3821
	public int revalt;

	// Token: 0x04000EEE RID: 3822
	public int sho0;

	// Token: 0x04000EEF RID: 3823
	public int sho1;

	// Token: 0x04000EF0 RID: 3824
	public int sho2;

	// Token: 0x04000EF1 RID: 3825
	public int sho3;

	// Token: 0x04000EF2 RID: 3826
	public int shoalt;

	// Token: 0x04000EF3 RID: 3827
	public int nai0;

	// Token: 0x04000EF4 RID: 3828
	public int nai1;

	// Token: 0x04000EF5 RID: 3829
	public int nai2;

	// Token: 0x04000EF6 RID: 3830
	public int nai3;

	// Token: 0x04000EF7 RID: 3831
	public int naialt;

	// Token: 0x04000EF8 RID: 3832
	public int rai0;

	// Token: 0x04000EF9 RID: 3833
	public int rai1;

	// Token: 0x04000EFA RID: 3834
	public int rai2;

	// Token: 0x04000EFB RID: 3835
	public int rai3;

	// Token: 0x04000EFC RID: 3836
	public int rock0;

	// Token: 0x04000EFD RID: 3837
	public int rock1;

	// Token: 0x04000EFE RID: 3838
	public int rock2;

	// Token: 0x04000EFF RID: 3839
	public int rock3;

	// Token: 0x04000F00 RID: 3840
	public int beam0;

	// Token: 0x04000F01 RID: 3841
	public int beam1;

	// Token: 0x04000F02 RID: 3842
	public int beam2;

	// Token: 0x04000F03 RID: 3843
	public int beam3;

	// Token: 0x04000F04 RID: 3844
	public int arm1;

	// Token: 0x04000F05 RID: 3845
	public int arm2;

	// Token: 0x04000F06 RID: 3846
	public int arm3;

	// Token: 0x04000F07 RID: 3847
	public int[] secretMissions;

	// Token: 0x04000F08 RID: 3848
	public bool[] limboSwitches;

	// Token: 0x04000F09 RID: 3849
	public bool[] shotgunSwitches;

	// Token: 0x04000F0A RID: 3850
	public int[] newEnemiesFound;

	// Token: 0x04000F0B RID: 3851
	public bool[] unlockablesFound;

	// Token: 0x04000F0C RID: 3852
	public bool revCustomizationUnlocked;

	// Token: 0x04000F0D RID: 3853
	public bool shoCustomizationUnlocked;

	// Token: 0x04000F0E RID: 3854
	public bool naiCustomizationUnlocked;

	// Token: 0x04000F0F RID: 3855
	public bool raiCustomizationUnlocked;

	// Token: 0x04000F10 RID: 3856
	public bool rockCustomizationUnlocked;
}
