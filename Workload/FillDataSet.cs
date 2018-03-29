using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DpQuery
{
	public partial class Qs
	{
		public async Task<DataSet> FillDataSetAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataSetAsync] {sql}");
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
						var result = new DataSet();
						da.Fill(result);
						return result;
					}
				}
			}
		}
		public DataSet FillDataSet(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{

			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataSet] {sql}");
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
						var result = new DataSet();
						da.Fill(result);
						return result;
					}
				}
			}
		}
	}
}
