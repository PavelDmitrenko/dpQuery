using System;
using System.Globalization;

namespace dpQuery
{
	public static class SafeConvert
	{

		#region ToBool
		public static bool ToBool(object s)
		{
			switch (s)
			{
				case DBNull _:
				{
					return false;
				}

				case bool val:
				{
					return val;
				}
				case int val:
				{
					return val == 1;
				}
				case short val:
				{
					return val == 1;
				}
				case byte val:
				{
					return val == 1;
				}
				case long val:
				{
					return val == 1;
				}
				case string val:
				{
					if (int.TryParse(val, out int num))
						return num == 1;

					return val.Equals("TRUE", StringComparison.CurrentCultureIgnoreCase);
				}
			}

			throw new Exception("Unknown Bool type");
		}
		#endregion

		#region BoolToInt
		public static int BoolToInt(bool pVal)
		{
			return pVal ? 1 : 0;
		}
		#endregion

		#region ToInt
		public static int ToInt(object val)
		{
			switch (val)
			{
				case int i:
					return i;

				case long _:
				case byte _:
				case short _:
				case decimal _:
				case double _:
					return Convert.ToInt32(val);

				case string s:
					int.TryParse(s, out var res);
					return res;

				case bool res1:
					return res1 ? 1 : 0;

				case DBNull _:
					return 0;

				case null:
					return 0;
			}

			throw new Exception("Unknow INT");
		}
		#endregion

		#region ToDecimal
		public static decimal ToDecimal(object val)
		{
			switch (val)
			{
				case decimal _:
					return (decimal) val;

				case double _:
				case long _:
					return Convert.ToDecimal(val);

				case DBNull _:
					return 0;

				case string _:
					char separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

					if (decimal.TryParse(((string) val).Replace('.', separator).Replace(',', separator), out var outVal))
						return outVal;
					break;

				case int _:
					return Convert.ToDecimal(val);
			}

			throw new Exception("Unknown DECIMAL");
		}
		#endregion

		#region ToGuid
		public static Guid ToGuid(object s)
		{
			switch (s)
			{
				case Guid _:
					return (Guid) s;

				case string _:
					return !Guid.TryParse((string) s, out var val) ? default(Guid) : val;
			}

			throw new Exception("Неизвестный тип Guid");
		}
		#endregion

		#region ToString
		public static string ToString(object val)
		{
			switch (val)
			{
				case string s:
					return s;

				case DBNull _:
					return string.Empty;

				case null:
					return "";
			}

			return val.ToString();
		}
		#endregion

		#region ToNullable
		public static T? ToNullable<T>(object val) where T : struct

		{
			if (val is DBNull)
				return null;

			return val as T?;
		}
		#endregion

	}
}