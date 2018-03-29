using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dpQuery
{
	public partial class Qs
	{
		public object ExecuteScalar(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null,
			   CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteScalar] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				conn.Open();

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					return dbCommand.ExecuteScalar();
				}
			}
		}

		public async Task<object> ExecuteScalarAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null,
			CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteScalarAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				await conn.OpenAsync().ConfigureAwait(false);

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					return await dbCommand.ExecuteScalarAsync().ConfigureAwait(false);
				}
			}
		}

		public object ExecuteScalarInTransaction(string sql,
																SqlTransaction transaction,
																Action<SqlParameterCollection> addAdditionalParametersAction = null,
																CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteScalarInTransaction] {sql}");
			}

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				return dbCommand.ExecuteScalar();
			}

		}

		public async Task<object> ExecuteScalarInTransactionAsync(string sql,
			SqlTransaction transaction,
			Action<SqlParameterCollection> addAdditionalParametersAction = null,
			CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteScalarInTransaction] {sql}");
			}

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				return await dbCommand.ExecuteScalarAsync();
			}

		}
	}
}
