using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class GameData
{
	// Token: 0x170001BF RID: 447
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x00009B1F File Offset: 0x00007D1F
	// (set) Token: 0x0600095C RID: 2396 RVA: 0x00009B26 File Offset: 0x00007D26
	public static int Stage { get; set; }

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x0600095D RID: 2397 RVA: 0x00009B2E File Offset: 0x00007D2E
	public static bool IsHausMixMaxStage
	{
		get
		{
			return GameData.Stage == 3;
		}
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x000453FC File Offset: 0x000435FC
	public static float GetLogTime(float fValue)
	{
		if (0f > fValue && -0.4f < fValue)
		{
			fValue *= 1f + fValue;
		}
		float num = Mathf.Abs(fValue);
		float num2 = 10f - num * 10f;
		if (1f > num2)
		{
			num2 = 1f;
		}
		if (10f <= num2)
		{
			num2 = 10f;
		}
		float num3 = 1f - Mathf.Log10(num2);
		if (0f > fValue)
		{
			num3 *= -1f;
		}
		return num3;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00009B38 File Offset: 0x00007D38
	public static string GetAbPath()
	{
		return ("file://" + Path.GetFullPath("../Pack/")).Replace("\\", "/");
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00009B5D File Offset: 0x00007D5D
	public static string GetSongPath()
	{
		return ("file://" + Path.GetFullPath("../Pack") + "/SongAb/").Replace("\\", "/");
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00009B87 File Offset: 0x00007D87
	public static string GetTestSongPath()
	{
		return (Path.GetFullPath("../Pack") + "/SongAb/").Replace("\\", "/");
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00045474 File Offset: 0x00043674
	public static EMBLEM GetEmblem(GRADE eGrade)
	{
		EMBLEM emblem = EMBLEM.NONE;
		switch (eGrade)
		{
		case GRADE.B:
			emblem = EMBLEM.CPOOER;
			break;
		case GRADE.A:
		case GRADE.A_P:
		case GRADE.A_PP:
			emblem = EMBLEM.SILVER;
			break;
		case GRADE.S:
		case GRADE.S_P:
		case GRADE.S_PP:
			emblem = EMBLEM.GOLD;
			break;
		}
		return emblem;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x000454B4 File Offset: 0x000436B4
	public static GRADE GetGrade(float fRate)
	{
		if (GameData.RANK_VALUE[9] <= fRate)
		{
			return GRADE.S_PP;
		}
		if (GameData.RANK_VALUE[8] <= fRate)
		{
			return GRADE.S_P;
		}
		if (GameData.RANK_VALUE[7] <= fRate)
		{
			return GRADE.S;
		}
		if (GameData.RANK_VALUE[6] <= fRate)
		{
			return GRADE.A_PP;
		}
		if (GameData.RANK_VALUE[5] <= fRate)
		{
			return GRADE.A_P;
		}
		if (GameData.RANK_VALUE[4] <= fRate)
		{
			return GRADE.A;
		}
		if (GameData.RANK_VALUE[3] <= fRate)
		{
			return GRADE.B;
		}
		if (GameData.RANK_VALUE[2] <= fRate)
		{
			return GRADE.C;
		}
		if (GameData.RANK_VALUE[1] <= fRate)
		{
			return GRADE.D;
		}
		return GRADE.F;
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00045530 File Offset: 0x00043730
	public static bool OnExtremeAngle(float fAngle)
	{
		float num = GameData.START_EXTREME_DEGREE - GameData.END_EXTREME_DEGREE;
		if (0f > num)
		{
			num += 360f;
		}
		float num2 = GameData.START_EXTREME_DEGREE - fAngle;
		if (0f > num2)
		{
			num2 += 360f;
		}
		return num >= num2;
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00045578 File Offset: 0x00043778
	public static int GetEnumIndex<T>(string enumValueName)
	{
		int num = -1;
		for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
		{
			if (Enum.GetName(typeof(T), Enum.GetValues(typeof(T)).GetValue(i)) == enumValueName)
			{
				num = i;
				break;
			}
		}
		return num;
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x000455D8 File Offset: 0x000437D8
	public static bool isContainHangul(string s)
	{
		if (s == null)
		{
			return false;
		}
		if (s.Length <= 0)
		{
			return false;
		}
		char[] array = s.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (char.GetUnicodeCategory(array[i]) == UnicodeCategory.OtherLetter)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00009BAC File Offset: 0x00007DAC
	public static T ParseEnum<T>(string value)
	{
		return (T)((object)Enum.Parse(typeof(T), value, true));
	}

	// Token: 0x04000781 RID: 1921
	public const string FIRMWARE = "TSR220A";

	// Token: 0x04000782 RID: 1922
	public const string S_COPYRIGHT = "CopyRight";

	// Token: 0x04000783 RID: 1923
	public const string S_WARNING = "warning";

	// Token: 0x04000784 RID: 1924
	public const string S_CI = "Ci";

	// Token: 0x04000785 RID: 1925
	public const string S_MANAGEMENT_MAIN = "MainManagement";

	// Token: 0x04000786 RID: 1926
	public const string S_TITLE = "Title";

	// Token: 0x04000787 RID: 1927
	public const string S_CARDLOGIN = "CardLogin";

	// Token: 0x04000788 RID: 1928
	public const string S_MODESELECT = "ModeSelect";

	// Token: 0x04000789 RID: 1929
	public const string S_ALLSONGMODESELECT = "HausMixAllSongTest";

	// Token: 0x0400078A RID: 1930
	public const string S_GAME = "game";

	// Token: 0x0400078B RID: 1931
	public const string S_THANKS = "ThanksForPlaying";

	// Token: 0x0400078C RID: 1932
	public const string S_RANKING = "Ranking";

	// Token: 0x0400078D RID: 1933
	public const string S_TUTORIAL = "Tutorial";

	// Token: 0x0400078E RID: 1934
	public const string S_PLAYTUTORIAL = "PlayTutorial";

	// Token: 0x0400078F RID: 1935
	public const string S_HAUSMIX_SONGSELECT = "HausMix";

	// Token: 0x04000790 RID: 1936
	public const string S_HAUSMIX_RESULT = "HausMixResult";

	// Token: 0x04000791 RID: 1937
	public const string S_HAUSMIX_TOTALRESULT = "HausMixAllClearResult";

	// Token: 0x04000792 RID: 1938
	public const string S_RAVEUP_ALBUMSELECT = "RaveUp";

	// Token: 0x04000793 RID: 1939
	public const string S_RAVEUP_TOTALRESULT = "RaveUpResult";

	// Token: 0x04000794 RID: 1940
	public const string S_RAVEUP_FAILEDRESULT = "ClubFailedResult";

	// Token: 0x04000795 RID: 1941
	public const string S_CLUBMISSION_LOBBY = "ClubTour";

	// Token: 0x04000796 RID: 1942
	public const string S_CLUBMISSION_TOTALRESULT = "ClubTourResult";

	// Token: 0x04000797 RID: 1943
	public const int MAX_HAUSMIX_STAGE = 3;

	// Token: 0x04000798 RID: 1944
	public const int MAX_RAVEUP_STAGE = 4;

	// Token: 0x04000799 RID: 1945
	public const int MAX_MISSIONSTAGE = 3;

	// Token: 0x0400079A RID: 1946
	public const int MAX_MISSIONCOUNT = 3;

	// Token: 0x0400079B RID: 1947
	public const float NOTECOLOR = 0.5f;

	// Token: 0x0400079C RID: 1948
	public const float TWEEN_TIME = 3000f;

	// Token: 0x0400079D RID: 1949
	public const int LOCALIZE = 0;

	// Token: 0x0400079E RID: 1950
	public const float FADEIN_TIME = 5f;

	// Token: 0x0400079F RID: 1951
	public const int UILAYER = 5;

	// Token: 0x040007A0 RID: 1952
	public const int EVENTMAXDATA = 8;

	// Token: 0x040007A1 RID: 1953
	public const int TIME_HAUSMIX = 70;

	// Token: 0x040007A2 RID: 1954
	public const int TIME_RAVEUP = 80;

	// Token: 0x040007A3 RID: 1955
	public const int TIME_CLUBTOUR = 60;

	// Token: 0x040007A4 RID: 1956
	public const int TIME_MODESELECT = 15;

	// Token: 0x040007A5 RID: 1957
	public const int TIME_CARD = 30;

	// Token: 0x040007A6 RID: 1958
	public const float MAXFEVER = 100f;

	// Token: 0x040007A7 RID: 1959
	public const float BASE_NOTEBACK = 80f;

	// Token: 0x040007A8 RID: 1960
	public const float BASE_NOTEFRONT = 0f;

	// Token: 0x040007A9 RID: 1961
	public const string INPUTNAME = "input";

	// Token: 0x040007AA RID: 1962
	public const int BGM_TRACK = 31;

	// Token: 0x040007AB RID: 1963
	public const int TUTORIAL_TRACK = 18;

	// Token: 0x040007AC RID: 1964
	public const int MAX_DIFFICULT = 12;

	// Token: 0x040007AD RID: 1965
	public const int Line = 12;

	// Token: 0x040007AE RID: 1966
	public const float ROT_BEATVALUE = 1440f;

	// Token: 0x040007AF RID: 1967
	public const int MAX_LONGCHECK = 10;

	// Token: 0x040007B0 RID: 1968
	public const int VIEWMISSION = 2;

	// Token: 0x040007B1 RID: 1969
	public const int MAX_LOONGCOOLBOMBCOUNT = 16;

	// Token: 0x040007B2 RID: 1970
	public const int KEYLATENCY_MS = 5;

	// Token: 0x040007B3 RID: 1971
	public const float SPEED_CHANGEMOVE = 45f;

	// Token: 0x040007B4 RID: 1972
	public const float TEMP_SAMELINE_ALPHA = 0.85f;

	// Token: 0x040007B5 RID: 1973
	public const float SAMELINE_DISTANCE = 0.4f;

	// Token: 0x040007B6 RID: 1974
	public const int ROUNDCOUNT = 80;

	// Token: 0x040007B7 RID: 1975
	public const float NOTE_HEIGHT = -4.8f;

	// Token: 0x040007B8 RID: 1976
	public const int MAXHAUS_SELECTRANK = 100;

	// Token: 0x040007B9 RID: 1977
	public const float KEYSOUND_HIGH = 0.7f;

	// Token: 0x040007BA RID: 1978
	public const float KEYSOUND_MID = 0.55f;

	// Token: 0x040007BB RID: 1979
	public const float KEYSOUND_LOW = 0.4f;

	// Token: 0x040007BC RID: 1980
	public static VERSION E_VERSION = VERSION.EN;

	// Token: 0x040007BD RID: 1981
	public static PLATFORM E_PLATFORM = PLATFORM.ANDROID;

	// Token: 0x040007BE RID: 1982
	public static string MACHINE_ID = "EC16605C01003217";

	// Token: 0x040007BF RID: 1983
	public static CHECKSYSTEM CheckSystem = CHECKSYSTEM.NONE;

	// Token: 0x040007C0 RID: 1984
	public static PLAYTYPE E_PLAYTYPE = PLAYTYPE.NORMAL;

	// Token: 0x040007C1 RID: 1985
	public static bool INCOMETEST = false;

	// Token: 0x040007C2 RID: 1986
	public static string S_CURSCENE = "CopyRight";

	// Token: 0x040007C3 RID: 1987
	public static bool FLIP = false;

	// Token: 0x040007C4 RID: 1988
	public static bool m_bOnPressBonus = false;

	// Token: 0x040007C5 RID: 1989
	public static bool m_bOnPressCoin = false;

	// Token: 0x040007C6 RID: 1990
	public static bool m_bOnPressManagement = false;

	// Token: 0x040007C7 RID: 1991
	public static EXTREME_STATE EXTREMEITEM = EXTREME_STATE.NONE;

	// Token: 0x040007C8 RID: 1992
	public static int INGAMGE_SPEED = 1;

	// Token: 0x040007C9 RID: 1993
	public static EFFECTOR_SPEED SPEEDEFFECTOR = EFFECTOR_SPEED.X_1;

	// Token: 0x040007CA RID: 1994
	public static EFFECTOR_FADER FADEEFFCTOR = EFFECTOR_FADER.NONE;

	// Token: 0x040007CB RID: 1995
	public static EFFECTOR_RAND RANDEFFECTOR = EFFECTOR_RAND.NONE;

	// Token: 0x040007CC RID: 1996
	public static PLAYFNORMALITEM PLAYITEM = PLAYFNORMALITEM.NONE;

	// Token: 0x040007CD RID: 1997
	public static PLAYFREFILLITEM REFILLITEM = PLAYFREFILLITEM.NONE;

	// Token: 0x040007CE RID: 1998
	public static PLAYFSHIELDITEM SHIELDITEM = PLAYFSHIELDITEM.NONE;

	// Token: 0x040007CF RID: 1999
	public static float[] ARR_TIME = new float[] { 2f, 1.9f, 1.8f, 1.7f, 1.6f, 1.5f, 1.4f, 1.3f, 1.2f, 1.1f };

	// Token: 0x040007D0 RID: 2000
	public static float[] FEVER_CHANGE_TIME = new float[] { 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };

	// Token: 0x040007D1 RID: 2001
	public static float MAXCOMBO = 25f;

	// Token: 0x040007D2 RID: 2002
	public static float[] FEVER_TIME = new float[] { 10f, 9f, 8f, 7f, 6f, 5f };

	// Token: 0x040007D3 RID: 2003
	public static float[] RANK_VALUE = new float[10];

	// Token: 0x040007D4 RID: 2004
	public static float[] EXTREME_TIME = new float[] { 9f, 7f };

	// Token: 0x040007D5 RID: 2005
	public static float[] EXTREME_BONUSRATE = new float[] { 2f, 3f };

	// Token: 0x040007D6 RID: 2006
	public static float MAXENERGY = 100f;

	// Token: 0x040007D7 RID: 2007
	public static float BreakeMinus = 10f;

	// Token: 0x040007D8 RID: 2008
	public static float PoorMinus = 1f;

	// Token: 0x040007D9 RID: 2009
	public static float PerfectAdd = 0.5f;

	// Token: 0x040007DA RID: 2010
	public static float Perfect10LvAdd = 0.1f;

	// Token: 0x040007DB RID: 2011
	public static float Perfect5LvAdd = 0.1f;

	// Token: 0x040007DC RID: 2012
	public static float PerfectS1S2Add = 0.1f;

	// Token: 0x040007DD RID: 2013
	public static float[] FeverAddGage = new float[] { -10f, 0f, 1f, 2f, 3f };

	// Token: 0x040007DE RID: 2014
	public static float[] CHAOS_UP_SPEED = new float[] { 2f, 1.75f, 1.5f, 1.3f, 1.1f, 0.95f, 0.75f, 0.65f };

	// Token: 0x040007DF RID: 2015
	public static float[] CHAOS_DN_SPEED = new float[] { 0.95f, 1.1f, 1.3f, 1.5f, 1.75f, 2f, 2.8f, 3.3f };

	// Token: 0x040007E0 RID: 2016
	public static float[] CHAOS_X_RANDOM = new float[12];

	// Token: 0x040007E1 RID: 2017
	public static float[] CHAOS_X = new float[12];

	// Token: 0x040007E2 RID: 2018
	public static float CHAOS_W_MIN = 2f;

	// Token: 0x040007E3 RID: 2019
	public static float CHAOS_W_MAX = 0.65f;

	// Token: 0x040007E4 RID: 2020
	public static float CHAOS_W_TIME = 2f;

	// Token: 0x040007E5 RID: 2021
	public static int LONGNOTE_TIMECOUNT = 80;

	// Token: 0x040007E6 RID: 2022
	public static int AUTOLONGNOTE = 240;

	// Token: 0x040007E7 RID: 2023
	public static int AUTOMOVENOTE = 240;

	// Token: 0x040007E8 RID: 2024
	public static int MINAUTOLONGNOTE = 40;

	// Token: 0x040007E9 RID: 2025
	public static float DIRMOVE = 0.5f;

	// Token: 0x040007EA RID: 2026
	public static int[] InJudgment_Per = new int[] { 95, 80, 30, 1, 0 };

	// Token: 0x040007EB RID: 2027
	public static int[] OutJudgment_Per = new int[] { 95, 80, 30, 1, 0 };

	// Token: 0x040007EC RID: 2028
	public static int InJudgment_Time = 160;

	// Token: 0x040007ED RID: 2029
	public static int OutJudgment_Time = 160;

	// Token: 0x040007EE RID: 2030
	public static int[] InJudgment_View = new int[]
	{
		100, 96, 91, 81, 71, 61, 51, 41, 31, 21,
		11, 2, 1, 0
	};

	// Token: 0x040007EF RID: 2031
	public static int[] OutJudgment_View = new int[]
	{
		100, 96, 91, 81, 71, 61, 51, 41, 31, 21,
		11, 2, 1, 0
	};

	// Token: 0x040007F0 RID: 2032
	public static int[] Judgment_Score = new int[] { 0, 1000, 2000, 3000, 5000 };

	// Token: 0x040007F1 RID: 2033
	public static float JogJudgmentDistance = 100f;

	// Token: 0x040007F2 RID: 2034
	public static float[] MODE_EZ_DEFICULT = new float[] { 0.6f, 0.4f, 0.2f };

	// Token: 0x040007F3 RID: 2035
	public static float[] MODE_NM_DEFICULT = new float[] { 0.7f, 0.5f, 0.3f };

	// Token: 0x040007F4 RID: 2036
	public static float[] MODE_HD_DEFICULT = new float[] { 0.8f, 0.6f, 0.4f };

	// Token: 0x040007F5 RID: 2037
	public static ArrayList ArrExtremeLvToGage = new ArrayList();

	// Token: 0x040007F6 RID: 2038
	public static Dictionary<PTLEVEL, int> PTTYPEBONUS = new Dictionary<PTLEVEL, int>();

	// Token: 0x040007F7 RID: 2039
	public static float FADEOUT_START = 0.6f;

	// Token: 0x040007F8 RID: 2040
	public static float FADEOUT_END = 0.2f;

	// Token: 0x040007F9 RID: 2041
	public static float FADEIN_START = 0.4f;

	// Token: 0x040007FA RID: 2042
	public static float FADEIN_END = 0f;

	// Token: 0x040007FB RID: 2043
	public static float BLINK_VALUE = 0.25f;

	// Token: 0x040007FC RID: 2044
	public static int CONVERT_VALUE = 8;

	// Token: 0x040007FD RID: 2045
	public static bool ITEM_FEVER = false;

	// Token: 0x040007FE RID: 2046
	public static bool ITEM_EXTREME = false;

	// Token: 0x040007FF RID: 2047
	public static bool ON_FEVER = false;

	// Token: 0x04000800 RID: 2048
	public static bool INGAME_AUTO = false;

	// Token: 0x04000801 RID: 2049
	public static bool NEVERDIE = false;

	// Token: 0x04000802 RID: 2050
	public static int BGM_MOVETIME = 0;

	// Token: 0x04000803 RID: 2051
	public static float[] GRADE_RATE = new float[] { 120f, 110f, 100f, 90f, 80f };

	// Token: 0x04000804 RID: 2052
	public static Vector3 ViewJudgmentScale = new Vector3(1.5f, 1.5f, 10f);

	// Token: 0x04000805 RID: 2053
	public static float START_EXTREME_DEGREE = 0f;

	// Token: 0x04000806 RID: 2054
	public static float END_EXTREME_DEGREE = 360f * GameData.EXTREME_TEMPFILL;

	// Token: 0x04000807 RID: 2055
	public static float TEST_PLAYTIME = 3f;

	// Token: 0x04000808 RID: 2056
	public static float EXTREME_TEMPFILL = 0.9f;

	// Token: 0x04000809 RID: 2057
	public static Vector3[] MAXGUIDE = new Vector3[]
	{
		new Vector3(0f, 0f, -150f),
		new Vector3(0f, 0f, -120f),
		new Vector3(0f, 0f, -90f),
		new Vector3(0f, 0f, -60f),
		new Vector3(0f, 0f, -30f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 30f),
		new Vector3(0f, 0f, 60f),
		new Vector3(0f, 0f, 90f),
		new Vector3(0f, 0f, 120f),
		new Vector3(0f, 0f, 150f),
		new Vector3(0f, 0f, 180f)
	};

	// Token: 0x0400080A RID: 2058
	public static Vector3[] BASEGUIDE = new Vector3[]
	{
		new Vector3(0f, 0f, -150f),
		new Vector3(0f, 0f, -120f),
		new Vector3(0f, 0f, -90f),
		new Vector3(0f, 0f, -60f),
		new Vector3(0f, 0f, -30f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 30f),
		new Vector3(0f, 0f, 60f),
		new Vector3(0f, 0f, 90f),
		new Vector3(0f, 0f, 120f),
		new Vector3(0f, 0f, 150f),
		new Vector3(0f, 0f, 180f)
	};

	// Token: 0x0400080B RID: 2059
	public static int MAXCOMBO_SCORE = 1000;

	// Token: 0x0400080C RID: 2060
	public static int MAXFEVER_SCORE = 1000;

	// Token: 0x0400080D RID: 2061
	public static Dictionary<PTLEVEL, string> PTTYPE_SPRITENAME = new Dictionary<PTLEVEL, string>
	{
		{
			PTLEVEL.EZ,
			"Dif_Ez"
		},
		{
			PTLEVEL.NM,
			"Dif_Nm"
		},
		{
			PTLEVEL.HD,
			"Dif_Hd"
		},
		{
			PTLEVEL.PR,
			"Dif_Pr"
		},
		{
			PTLEVEL.MX,
			"Dif_Mx"
		},
		{
			PTLEVEL.S1,
			"Dif_Hid1"
		},
		{
			PTLEVEL.S2,
			"Dif_hid2"
		}
	};

	// Token: 0x0400080E RID: 2062
	public static int BASEGET_BEATPOINT = 30;

	// Token: 0x0400080F RID: 2063
	public static int ALLCLEAR_ADDBEATPOINT = 2;

	// Token: 0x04000810 RID: 2064
	public static int MISSIONCLEAR_BEATPOINT = 30;

	// Token: 0x04000811 RID: 2065
	public static int BASEGET_EXP = 300;

	// Token: 0x04000812 RID: 2066
	public static int ALLCLEAR_ADDEXP = 30;

	// Token: 0x04000813 RID: 2067
	public static int MISSIONCLEAR_EXP = 200;

	// Token: 0x04000814 RID: 2068
	public static Dictionary<int, int> DIF_BONUSEXP = new Dictionary<int, int>();

	// Token: 0x04000815 RID: 2069
	public static Dictionary<int, int> DIF_BONUSBEATPOINT = new Dictionary<int, int>();

	// Token: 0x04000816 RID: 2070
	public static Dictionary<PTLEVEL, int> PTTYPE_BONUS = new Dictionary<PTLEVEL, int>();

	// Token: 0x04000817 RID: 2071
	public static Dictionary<GRADE, float> GRADE_BONUSRATE = new Dictionary<GRADE, float>();

	// Token: 0x04000818 RID: 2072
	public static Dictionary<DISCSET_DIFFICULT, float> DISCSET_BONUSRATE = new Dictionary<DISCSET_DIFFICULT, float>();

	// Token: 0x04000819 RID: 2073
	public static Dictionary<MISSIONTYPE, string> MISSION_DATATYPE = new Dictionary<MISSIONTYPE, string>
	{
		{
			MISSIONTYPE.Clear,
			"All Stage Clear"
		},
		{
			MISSIONTYPE.Score,
			"Total Score"
		},
		{
			MISSIONTYPE.AllCombo,
			"All Combo Play"
		},
		{
			MISSIONTYPE.MaxCombo,
			"Max Combo Count"
		},
		{
			MISSIONTYPE.PerfectPlay,
			"Perfect Play"
		},
		{
			MISSIONTYPE.Perfect,
			"Perfect Count"
		},
		{
			MISSIONTYPE.Great,
			"Great Count"
		},
		{
			MISSIONTYPE.Good,
			"Good Count"
		},
		{
			MISSIONTYPE.Poor,
			"Poor Count"
		},
		{
			MISSIONTYPE.Break,
			"Break Count"
		},
		{
			MISSIONTYPE.Accuracy,
			"Accuracy Rate"
		},
		{
			MISSIONTYPE.Rank_SPP,
			"Class S++"
		},
		{
			MISSIONTYPE.Rank_SP,
			"Class S+"
		},
		{
			MISSIONTYPE.Rank_S,
			"Class S"
		},
		{
			MISSIONTYPE.Rank_APP,
			"Class A++"
		},
		{
			MISSIONTYPE.Rank_AP,
			"Class A+"
		},
		{
			MISSIONTYPE.Rank_A,
			"Class A"
		},
		{
			MISSIONTYPE.Rank_B,
			"Class B"
		},
		{
			MISSIONTYPE.Rank_C,
			"Class C"
		},
		{
			MISSIONTYPE.Rank_D,
			"Class D"
		},
		{
			MISSIONTYPE.Rank_F,
			"Class F"
		},
		{
			MISSIONTYPE.ExtremeBonus,
			"Extreme Bonus Score"
		},
		{
			MISSIONTYPE.ExtremeCount,
			"Extreme Launch Count"
		},
		{
			MISSIONTYPE.ExtremeMultiple_X2,
			"Extreme Count"
		},
		{
			MISSIONTYPE.ExtremeMultiple_X3,
			"Super Extreme Launch Count"
		},
		{
			MISSIONTYPE.FeverBonus,
			"Fever Bonus Score"
		},
		{
			MISSIONTYPE.FeverCount,
			"Fever Launch Count"
		},
		{
			MISSIONTYPE.FeverMultiple_X2,
			"X2 Fever Launch Count"
		},
		{
			MISSIONTYPE.FeverMultiple_X3,
			"X3 Fever Launch Count"
		},
		{
			MISSIONTYPE.FeverMultiple_X4,
			"X4 Fever Launch Count"
		},
		{
			MISSIONTYPE.FeverMultiple_X5,
			"X5 Fever Launch Count"
		},
		{
			MISSIONTYPE.ClearMember,
			"Clear Member"
		}
	};

	// Token: 0x0400081B RID: 2075
	public static bool AUTO_PLAY = false;

	// Token: 0x0400081C RID: 2076
	public static string XYCLON_VERSION = "1.602";

	// Token: 0x0400081D RID: 2077
	public static string LANGUAGE = "EN";
}
