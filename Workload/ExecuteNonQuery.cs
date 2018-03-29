using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DpQuery
{
	public partial class Qs
	{
		public async Task ExecuteNonQueryAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{

			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteNonQueryAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				await conn.OpenAsync().ConfigureAwait(false);

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					await dbCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
				}
			}
		}

		public void ExecuteNonQuery(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				conn.Open();

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);
					dbCommand.ExecuteNonQuery();
				}
			}
		}

		public async Task ExecuteNonQueryInTransactionAsync(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
				Debug.WriteLine($"[ExecuteNonQueryInTransactionAsync] {sql}");

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				await dbCommand.ExecuteNonQueryAsync();
			}
		}

		public void ExecuteNonQueryInTransaction(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
				Debug.WriteLine($"[ExecuteNonQueryInTransaction] {sql}");

			SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction)
			{
				CommandType = commandType
			};

			addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

			dbCommand.ExecuteNonQuery();
		}
	}
}
