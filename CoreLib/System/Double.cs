//Copyright © 2022 Contributors of moose-org, This code is licensed under the BSD 3-Clause "New" or "Revised" License.

namespace System
{
	public unsafe struct Double
	{

		public const double MinValue = -1.7976931348623157E+308;
		public const double MaxValue = 1.7976931348623157E+308;
		public static unsafe bool IsNaN(double d)
		{
			// A NaN will never equal itself so this is an
			// easy and efficient way to check for NaN.

#pragma warning disable CS1718
			return d != d;
#pragma warning restore CS1718
		}

		private static int CompareNums(double x, double y)
		{
			return Math.Abs(x - y) <= 0.000000119209 ? 0 : x > y ? 1 : -1;
		}


		public override string ToString()
		{
			string bin = "";
			if (this < 0)
			{
				this *= -1;
				bin += "-";
			}
			if (CompareNums(this, MaxValue) >= 0)
			{ //use E+
				int e = 0;
				while (CompareNums(this, 10) >= 0)
				{ //while d is greater than or equal to 10 (not in scientific form)
					this /= 10;
					e++;
				}
				while (CompareNums(this, (long)this) != 0)
				{ //while not an integer
					this *= 10;
				}
				string str = ((long)this).ToString();
				bin += str.Substring(0, 1) + "." + str.Substring(1) + "E+" + e;
			} else if (CompareNums(this, 1) == -1)
			{ //between 0-1
				if (CompareNums(this, MinValue) == -1)
				{ //use E-
					int e = 0;
					while (this < 1)
					{ //while d is not in scientific form
						this *= 10;
						e++;
					}
					while (CompareNums(this, (long)this) != 0)
					{ //while not an integer
						this *= 10;
					}
					string str = ((long)this).ToString();
					bin += str.Substring(0, 1) + "." + str.Substring(1) + "E-" + e;
				} else
				{ //regular decimal less than 0
					int decimals = 0;
					while (CompareNums(this, (long)this) != 0)
					{ //while not an integer
						this *= 10;
						decimals++;
					}
					string str = ((long)this).ToString();
					bin += "0.";
					for (int i = 0; i < decimals - 1; i++)
					{
						bin += "0";
					}
					bin += str;
				}
			} else
			{ //regular decimal
				int decimals = 0;
				while (CompareNums(this, (long)this) != 0)
				{ //while not an integer
					this *= 10;
					decimals++;
				}
				string str = ((long)this).ToString();
				if (decimals == 0)
				{
					bin += str;
				} else
				{
					bin += str.Substring(0, str.Length - decimals) + "." + str.Substring(str.Length - decimals);
				}
			}
			return bin;

		}
	}
}