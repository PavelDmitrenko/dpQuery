using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DpQuery
{
	public class Qs
	{

		private readonly ConnectionStringSettings connectionStringSettings;

		public Qs()
		{
	
		}

		public Qs(ConnectionStringSettings connectionStringSettings)
		{
			this.connectionStringSettings = connectionStringSettings;
		}

		#region FillDataSetAsync
		public async Task<DataSet> FillDataSetAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
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

		#region ExecuteNonQueryInTransaction
		public async Task ExecuteNonQueryInTransactionAsync(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction, CommandType commandType = CommandType.StoredProcedure)
		{
			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				await dbCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}
		#endregion

		#region BeginTransaction
		public async Task BeginTransaction(Action<SqlTransaction> execute)
		{
			using (SqlConnection connection = new SqlConnection(_GetConnectionString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						execute.Invoke(transaction);
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}

				}

			} // using SQLiteConnection
		}
		#endregion

		#region FillDataTableAsync
		public DataTable FillDataTableInTransaction(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
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

		public async Task<DataTable> FillDataTableAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
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
		public DataTable FillDataTableInTransaction(Func<string> sqlGet, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
		{
			using (SqlCommand dbCommand = new SqlCommand(sqlGet(), transaction.Connection, transaction))
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
			if (connectionStringSettings != null)
				return connectionStringSettings.ConnectionString;

			return ConfigurationManager.ConnectionStrings["Default"]?.ConnectionString;
			
		}
		#endregion

		#region ExecuteScalar
		public async Task<object> ExecuteScalarAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null,
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

		public object ExecuteScalarInTransaction(string sql, 
																SqlTransaction transaction, 
																Action<SqlParameterCollection> addAdditionalParametersAction = null,
																CommandType commandType = CommandType.StoredProcedure)
		{
			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);

				return dbCommand.ExecuteScalar();
			}
		
		}
		#endregion

		#region ExecuteNonQueryAsync
		public async Task ExecuteNonQueryAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
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

		#region ExecuteReaderAsync
		public async Task ExecuteReaderAsync(string sql, Action<SqlParameterCollection> addAdditionalParametersAction, Action<SqlDataReader> ReaderAction, CommandType commandType = CommandType.StoredProcedure)
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

}
