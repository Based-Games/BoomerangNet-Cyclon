using System;

// Token: 0x0200001D RID: 29
[Serializable]
public class InvStat
{
	// Token: 0x060000E2 RID: 226 RVA: 0x00003E1F File Offset: 0x0000201F
	public static string GetName(InvStat.Identifier i)
	{
		return i.ToString();
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000156A8 File Offset: 0x000138A8
	public static string GetDescription(InvStat.Identifier i)
	{
		switch (i)
		{
		case InvStat.Identifier.Strength:
			return "Strength increases melee damage";
		case InvStat.Identifier.Constitution:
			return "Constitution increases health";
		case InvStat.Identifier.Agility:
			return "Agility increases armor";
		case InvStat.Identifier.Intelligence:
			return "Intelligence increases mana";
		case InvStat.Identifier.Damage:
			return "Damage adds to the amount of damage done in combat";
		case InvStat.Identifier.Crit:
			return "Crit increases the chance of landing a critical strike";
		case InvStat.Identifier.Armor:
			return "Armor protects from damage";
		case InvStat.Identifier.Health:
			return "Health prolongs life";
		case InvStat.Identifier.Mana:
			return "Mana increases the number of spells that can be cast";
		default:
			return null;
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00015720 File Offset: 0x00013920
	public static int CompareArmor(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Armor)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Damage)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000157F4 File Offset: 0x000139F4
	public static int CompareWeapon(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Damage)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Armor)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x040000B7 RID: 183
	public InvStat.Identifier id;

	// Token: 0x040000B8 RID: 184
	public InvStat.Modifier modifier;

	// Token: 0x040000B9 RID: 185
	public int amount;

	// Token: 0x0200001E RID: 30
	public enum Identifier
	{
		// Token: 0x040000BB RID: 187
		Strength,
		// Token: 0x040000BC RID: 188
		Constitution,
		// Token: 0x040000BD RID: 189
		Agility,
		// Token: 0x040000BE RID: 190
		Intelligence,
		// Token: 0x040000BF RID: 191
		Damage,
		// Token: 0x040000C0 RID: 192
		Crit,
		// Token: 0x040000C1 RID: 193
		Armor,
		// Token: 0x040000C2 RID: 194
		Health,
		// Token: 0x040000C3 RID: 195
		Mana,
		// Token: 0x040000C4 RID: 196
		Other
	}

	// Token: 0x0200001F RID: 31
	public enum Modifier
	{
		// Token: 0x040000C6 RID: 198
		Added,
		// Token: 0x040000C7 RID: 199
		Percent
	}
}
