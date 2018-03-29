using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dpQuery
{
	public partial class Qs
	{
		public void ExecuteReader(string sql, Action<SqlParameterCollection> addAdditionalParametersAction, Action<SqlDataReader> ReaderAction, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteReaderAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				conn.Open();

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					SqlDataReader Reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
					ReaderAction?.Invoke(Reader);
					Reader.Close();

				}
			}
		}

		public async Task ExecuteReaderAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction, Action<SqlDataReader> ReaderAction, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteReaderAsync] {sql}");
			}

			using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
			{
				await conn.OpenAsync().ConfigureAwait(false);

				using (SqlCommand dbCommand = new SqlCommand(sql, conn))
				{
					dbCommand.CommandType = commandType;
					addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

					SqlDataReader Reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
					ReaderAction?.Invoke(Reader);
					Reader.Close();

				}
			}
		}
	}
}
