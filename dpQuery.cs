using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class DpQuery
{

	private string connectionString;

	#region FillDataSet
	public async Task<DataSet> FillDataSet(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
	{

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
	#endregion

	#region FillDataTable
	public async Task<DataTable> FillDataTable(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
	{
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

	public async Task<DataTable> FillDataTable(Func<string> sqlGet, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
	{
		using (SqlConnection conn = new SqlConnection(_GetConnectionString()))
		{
			await conn.OpenAsync().ConfigureAwait(false);

			using (SqlCommand dbCommand = new SqlCommand(sqlGet(), conn))
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
	#endregion

	#region _GetConnectionString
	private string _GetConnectionString()
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			connectionString = ConfigurationManager.ConnectionStrings["Default"]?.ConnectionString;
		}
		return connectionString;
	}
	#endregion

	#region ExecuteScalar
	public async Task<object> ExecuteScalar(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null,
		CommandType commandType = CommandType.StoredProcedure)
	{
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
	#endregion

	#region ExecuteNonQuery
	public async Task ExecuteNonQuery(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
	{
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
	#endregion

	#region ExecuteReader
	public async Task ExecuteReader(string sql, Action<SqlParameterCollection> addAdditionalParametersAction, Action<SqlDataReader> ReaderAction, CommandType commandType = CommandType.StoredProcedure)
	{
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
	#endregion


}