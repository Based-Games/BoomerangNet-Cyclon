using System;

// Token: 0x02000102 RID: 258
public class CardManager
{
	// Token: 0x060008EB RID: 2283 RVA: 0x000096CF File Offset: 0x000078CF
	public static void Init()
	{
		Logger.Log("CardManager", "Master reader init", new object[0]);
		CardManagerR1.Init();
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000096EB File Offset: 0x000078EB
	public static bool read(byte sector, byte block, byte[] pBuffer)
	{
		return CardManagerR1.read(sector, block, pBuffer);
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x000096F5 File Offset: 0x000078F5
	public static bool loadBase()
	{
		return CardManagerR1.loadBase();
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x000096FC File Offset: 0x000078FC
	public static bool isInsertCard()
	{
		return CardManagerR1.isInsertCard();
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00009703 File Offset: 0x00007903
	public static bool isDefaultPassword()
	{
		return CardManagerR1.isDefaultPassword();
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0000970A File Offset: 0x0000790A
	public static bool verifySecureKey(byte sector, byte[] pBuffer)
	{
		return CardManagerR1.verifySecureKey(sector, pBuffer);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00009713 File Offset: 0x00007913
	public static bool eject()
	{
		return CardManagerR1.eject();
	}
}
