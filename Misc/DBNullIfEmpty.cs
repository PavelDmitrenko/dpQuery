using System;

namespace dpQuery
{
	public static class DBNullIfEmpty
	{
		public static object Int(int? value)
		{
			if (!value.HasValue)
				return DBNull.Value;

			if (value.Value == 0)
				return DBNull.Value;

			return value.Value;
		}

		public static object String(string value)
		{
			if (string.IsNullOrEmpty(value))
				return DBNull.Value;

			return value;
		}

		public static object Date(DateTime? value)
		{
			if (!value.HasValue)
				return DBNull.Value;

			if (value.Value.Equals(DateTime.MaxValue) || value.Value.Equals(DateTime.MinValue))
				return DBNull.Value;

			if (value.Value.ToUniversalTime().Equals(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)))
				return DBNull.Value;

			return value.Value;
		}
	}
}
