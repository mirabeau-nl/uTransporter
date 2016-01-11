using System.Configuration;
using System.Data.SqlClient;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Services;
using Mirabeau.uTransporter.UnitOfWork;

using Umbraco.Core.Persistence.UnitOfWork;

namespace Mirabeau.uTransporter.Providers
{
    /// <summary>
    /// Custom database provider that accepts connection strings
    /// </summary>
    public class DatabaseUnitOfWorkProvider : IDatabaseUnitOfWorkProvider
    {
        private readonly string _connectionStringName;

        private readonly string _databasePostFix;
        private readonly ISqlObjectManager _sqlObjectManager;
        private IDatabaseUnitOfWork _unitOfWorkProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUnitOfWorkProvider"/> class.
        /// </summary>
        /// <param name="connectionString">Connection String for the target database</param>
        public DatabaseUnitOfWorkProvider(string connectionStringName, ISqlObjectManager sqlObjectManager, string databasePostFix = null)
        {
            _connectionStringName = connectionStringName;
            _sqlObjectManager = sqlObjectManager;
            _databasePostFix = databasePostFix;
        }

        /// <summary>
        /// Get the database Unit Of Work
        /// </summary>
        /// <returns>Database Unit Of Work</returns>
        public IDatabaseUnitOfWork GetUnitOfWork()
        {
            ConnectionStringSettings connectionStringSettings = _sqlObjectManager.GetConnectionStringSettings(_connectionStringName);
            SqlConnectionStringBuilder sqlConnection = _sqlObjectManager.BuildConnectionString(connectionStringSettings);

            if (!string.IsNullOrEmpty(_databasePostFix))
            {
                sqlConnection.InitialCatalog = string.Format("{0}_{1}", sqlConnection.InitialCatalog, _databasePostFix);
            }

            return _unitOfWorkProvider ?? (_unitOfWorkProvider = new DatabaseUnitOfWork(sqlConnection.ToString()));
        }
    }
}
