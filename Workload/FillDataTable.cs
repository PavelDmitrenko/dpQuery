using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dpQuery
{
	public partial class Qs
	{
		public DataTable FillDataTableInTransaction(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableInTransaction] {sql}");
			}

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
				{
					var result = new DataTable();
					da.Fill(result);

					return result;
				}
			}
		}

		public async Task<DataTable> FillDataTableInTransactionAsync(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
				Debug.WriteLine($"[FillDataTableInTransactionAsync] {sql}");

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
				{
					var result = new DataTable();
					da.Fill(result); // Async method is missing
					return result;
				}
			}
		}
		public DataTable FillDataTable(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				conn.Open();

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
					{
						var result = new DataTable();
						da.Fill(result);
						return result;
					}
				}
			}
		}

		public async Task<DataTable> FillDataTableAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				await conn.OpenAsync().ConfigureAwait(false);

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
					{
						var result = new DataTable();
						da.Fill(result);
						return result;
					}
				}
			}
		}

		public async Task<DataTable> FillDataTableInTransactionAsync(Func<string> sqlGet, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			string sql = sqlGet();

			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableInTransaction] {sql}");
			}

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
				{
					var result = new DataTable();
					await Task.Run(() => da.Fill(result)); // Async method is missing
					return result;
				}
			}
		}

		public DataTable FillDataTableInTransaction(Func<string> sqlGet, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			string sql = sqlGet();

			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableInTransaction] {sql}");
			}

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
				{
					var result = new DataTable();
					da.Fill(result);
					return result;
				}
			}
		}

		public async Task<DataTable> FillDataTableAsync(Func<string> sqlGet, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			string sql = sqlGet();

			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				await conn.OpenAsync().ConfigureAwait(false);

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					using (SqlDataAdapter da = new SqlDataAdapter(dbCommand))
					{
						var result = new DataTable();
						da.Fill(result);
						return result;
					}
				}

			}
		}

	}
}
