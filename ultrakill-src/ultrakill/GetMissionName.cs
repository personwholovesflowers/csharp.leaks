using System;

// Token: 0x02000227 RID: 551
public static class GetMissionName
{
	// Token: 0x06000BD5 RID: 3029 RVA: 0x00052E61 File Offset: 0x00051061
	public static string GetMission(int missionNum)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return MapInfoBase.Instance.levelName;
		}
		if (missionNum == 0)
		{
			return "MAIN MENU";
		}
		return GetMissionName.GetMissionNumberOnly(missionNum) + ": " + GetMissionName.GetMissionNameOnly(missionNum);
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00052E94 File Offset: 0x00051094
	public static string GetMissionNumberOnly(int missionNum)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return "";
		}
		switch (missionNum)
		{
		case 1:
			return "0-1";
		case 2:
			return "0-2";
		case 3:
			return "0-3";
		case 4:
			return "0-4";
		case 5:
			return "0-5";
		case 6:
			return "1-1";
		case 7:
			return "1-2";
		case 8:
			return "1-3";
		case 9:
			return "1-4";
		case 10:
			return "2-1";
		case 11:
			return "2-2";
		case 12:
			return "2-3";
		case 13:
			return "2-4";
		case 14:
			return "3-1";
		case 15:
			return "3-2";
		case 16:
			return "4-1";
		case 17:
			return "4-2";
		case 18:
			return "4-3";
		case 19:
			return "4-4";
		case 20:
			return "5-1";
		case 21:
			return "5-2";
		case 22:
			return "5-3";
		case 23:
			return "5-4";
		case 24:
			return "6-1";
		case 25:
			return "6-2";
		case 26:
			return "7-1";
		case 27:
			return "7-2";
		case 28:
			return "7-3";
		case 29:
			return "7-4";
		case 30:
			return "8-1";
		case 31:
			return "8-2";
		case 32:
			return "8-3";
		case 33:
			return "8-4";
		case 34:
			return "9-1";
		case 35:
			return "9-2";
		default:
			switch (missionNum)
			{
			case 100:
				return "0-E";
			case 101:
				return "1-E";
			case 102:
				return "2-E";
			case 103:
				return "3-E";
			case 104:
				return "4-E";
			case 105:
				return "5-E";
			case 106:
				return "6-E";
			case 107:
				return "7-E";
			case 108:
				return "8-E";
			case 109:
				return "9-E";
			default:
				switch (missionNum)
				{
				case 666:
					return "P-1";
				case 667:
					return "P-2";
				case 668:
					return "P-3";
				default:
					return "";
				}
				break;
			}
			break;
		}
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x000530B0 File Offset: 0x000512B0
	public static string GetMissionNameOnly(int missionNum)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return MapInfoBase.Instance.levelName;
		}
		switch (missionNum)
		{
		case 0:
			return "MAIN MENU";
		case 1:
			return "INTO THE FIRE";
		case 2:
			return "THE MEATGRINDER";
		case 3:
			return "DOUBLE DOWN";
		case 4:
			return "A ONE-MACHINE ARMY";
		case 5:
			return "CERBERUS";
		case 6:
			return "HEART OF THE SUNRISE";
		case 7:
			return "THE BURNING WORLD";
		case 8:
			return "HALLS OF SACRED REMAINS";
		case 9:
			return "CLAIR DE LUNE";
		case 10:
			return "BRIDGEBURNER";
		case 11:
			return "DEATH AT 20,000 VOLTS";
		case 12:
			return "SHEER HEART ATTACK";
		case 13:
			return "COURT OF THE CORPSE KING";
		case 14:
			return "BELLY OF THE BEAST";
		case 15:
			return "IN THE FLESH";
		case 16:
			return "SLAVES TO POWER";
		case 17:
			return "GOD DAMN THE SUN";
		case 18:
			return "A SHOT IN THE DARK";
		case 19:
			return "CLAIR DE SOLEIL";
		case 20:
			return "IN THE WAKE OF POSEIDON";
		case 21:
			return "WAVES OF THE STARLESS SEA";
		case 22:
			return "SHIP OF FOOLS";
		case 23:
			return "LEVIATHAN";
		case 24:
			return "CRY FOR THE WEEPER";
		case 25:
			return "AESTHETICS OF HATE";
		case 26:
			return "GARDEN OF FORKING PATHS";
		case 27:
			return "LIGHT UP THE NIGHT";
		case 28:
			return "NO SOUND, NO MEMORY";
		case 29:
			return "...LIKE ANTENNAS TO HEAVEN";
		case 30:
			return "???";
		case 31:
			return "???";
		case 32:
			return "???";
		case 33:
			return "???";
		case 34:
			return "???";
		case 35:
			return "???";
		default:
			switch (missionNum)
			{
			case 100:
				return "THIS HEAT, AN EVIL HEAT";
			case 101:
				return "...THEN FELL THE ASHES";
			case 102:
				return "???";
			case 103:
				return "???";
			case 104:
				return "???";
			case 105:
				return "???";
			case 106:
				return "???";
			case 107:
				return "???";
			case 108:
				return "???";
			case 109:
				return "???";
			default:
				switch (missionNum)
				{
				case 666:
					return "SOUL SURVIVOR";
				case 667:
					return "WAIT OF THE WORLD";
				case 668:
					return "???";
				default:
					return "MISSION NAME NOT FOUND";
				}
				break;
			}
			break;
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x000532DC File Offset: 0x000514DC
	public static string GetSceneName(int missionNum)
	{
		switch (missionNum)
		{
		case 1:
			return "Level 0-1";
		case 2:
			return "Level 0-2";
		case 3:
			return "Level 0-3";
		case 4:
			return "Level 0-4";
		case 5:
			return "Level 0-5";
		case 6:
			return "Level 1-1";
		case 7:
			return "Level 1-2";
		case 8:
			return "Level 1-3";
		case 9:
			return "Level 1-4";
		case 10:
			return "Level 2-1";
		case 11:
			return "Level 2-2";
		case 12:
			return "Level 2-3";
		case 13:
			return "Level 2-4";
		case 14:
			return "Level 3-1";
		case 15:
			return "Level 3-2";
		case 16:
			return "Level 4-1";
		case 17:
			return "Level 4-2";
		case 18:
			return "Level 4-3";
		case 19:
			return "Level 4-4";
		case 20:
			return "Level 5-1";
		case 21:
			return "Level 5-2";
		case 22:
			return "Level 5-3";
		case 23:
			return "Level 5-4";
		case 24:
			return "Level 6-1";
		case 25:
			return "Level 6-2";
		case 26:
			return "Level 7-1";
		case 27:
			return "Level 7-2";
		case 28:
			return "Level 7-3";
		case 29:
			return "Level 7-4";
		case 30:
			return "Level 8-1";
		case 31:
			return "Level 8-2";
		case 32:
			return "Level 8-3";
		case 33:
			return "Level 8-4";
		case 34:
			return "Level 9-1";
		case 35:
			return "Level 9-2";
		default:
			switch (missionNum)
			{
			case 100:
				return "Level 0-E";
			case 101:
				return "Level 1-E";
			case 102:
				return "Level 2-E";
			case 103:
				return "Level 3-E";
			case 104:
				return "Level 4-E";
			case 105:
				return "Level 5-E";
			case 106:
				return "Level 6-E";
			case 107:
				return "Level 7-E";
			case 108:
				return "Level 8-E";
			case 109:
				return "Level 9-E";
			default:
				switch (missionNum)
				{
				case 666:
					return "Level P-1";
				case 667:
					return "Level P-2";
				case 668:
					return "Level P-3";
				default:
					return "Main Menu";
				}
				break;
			}
			break;
		}
	}
}
