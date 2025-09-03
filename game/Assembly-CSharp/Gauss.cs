using System;

// Token: 0x0200021C RID: 540
public class Gauss
{
	// Token: 0x06000F86 RID: 3974 RVA: 0x0000D54D File Offset: 0x0000B74D
	public Gauss(Random rnd, double m, double s)
	{
		this.rnd = rnd;
		this.m = m;
		this.s = s;
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x0000D56A File Offset: 0x0000B76A
	public double Next()
	{
		return this.s * this.gauss_N() + this.m;
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x00070720 File Offset: 0x0006E920
	public double gauss_N()
	{
		double num;
		double num3;
		do
		{
			num = (double)this.rnd.Next(-500000, 500001) / 500000.0;
			double num2 = (double)this.rnd.Next(-500000, 500001) / 500000.0;
			num3 = num * num + num2 * num2;
		}
		while (num3 >= 1.0 || num3 == 0.0);
		num3 = Math.Sqrt(-2.0 * Math.Log(num3) / num3);
		return num * num3;
	}

	// Token: 0x04001157 RID: 4439
	private Random rnd;

	// Token: 0x04001158 RID: 4440
	private double m;

	// Token: 0x04001159 RID: 4441
	private double s;
}
