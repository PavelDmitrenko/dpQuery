using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DpQuery
{
	public class Qs
	{

		private readonly ConnectionStringSettings connectionStringSettings;
		public bool DebugMode;

		public Qs()
		{
		}

		public Qs(ConnectionStringSettings connectionStringSettings) : this()
		{
			this.connectionStringSettings = connectionStringSettings;
		}

		#region FillDataSet
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
		#endregion

		#region ExecuteNonQueryInTransaction
		public async Task ExecuteNonQueryInTransactionAsync(string sql, SqlTransaction transaction, Action<SqlParameterCollection> addAdditionalParametersAction, CommandType commandType = CommandType.StoredProcedure)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[ExecuteNonQueryInTransactionAsync] {sql}");
			}

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
			{
				Debug.WriteLine($"[ExecuteNonQueryInTransaction] {sql}");
			}

			using (SqlCommand dbCommand = new SqlCommand(sql, transaction.Connection, transaction))
			{
				dbCommand.CommandType = commandType;
				addAdditionalParametersAction?.Invoke(dbCommand.Parameters);
				dbCommand.ExecuteNonQuery();
			}
		}
		#endregion

		#region BeginTransaction
		public async Task BeginTransactionAsync(Action<SqlTransaction> execute)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[BeginTransactionAsync]");
			}

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

		public void BeginTransaction(Action<SqlTransaction> execute)
		{
			if (DebugMode)
			{
				Debug.WriteLine($"[FillDataTableInTransaction]");
			}

			using (SqlConnection connection = new SqlConnection(_GetConnectionString()))
			{
				connection.Open();

				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						execute.Invoke(transaction);
						transaction.Commit();
					}
					catch(Exception ex)
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
		#endregion

		#region ExecuteNonQuery
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

		public  void ExecuteNonQuery(string sql, Action<SqlParameterCollection> addAdditionalParametersAction = null, CommandType commandType = CommandType.StoredProcedure)
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
		#endregion

		#region ExecuteReaderAsync
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
		#endregion

	}

}
