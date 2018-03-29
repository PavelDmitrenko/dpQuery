using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dpQuery
{
	public partial class Qs
	{
		public async Task BeginTransactionAsync(Func<SqlTransaction, Task> execute)
		{
			if (DebugMode)
				Debug.WriteLine($"[BeginTransactionAsync]");

			using (SqlConnection connection = new SqlConnection(_GetConnectionString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						await execute.Invoke(transaction);

						if (DebugMode)
							Debug.WriteLine($"[Transaction Commit]");

						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}

				}
			}
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
					catch (Exception ex)
					{
						transaction.Rollback();
						throw;
					}

				}

			}
		}
	}
}
