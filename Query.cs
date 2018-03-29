using System.Configuration;

namespace dpQuery
{
	public partial class Qs
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
	
		#region _GetConnectionString
		private string _GetConnectionString()
		{
			if (connectionStringSettings != null)
				return connectionStringSettings.ConnectionString;

			return ConfigurationManager.ConnectionStrings["Default"]?.ConnectionString;
			
		}
		#endregion

	}

}
