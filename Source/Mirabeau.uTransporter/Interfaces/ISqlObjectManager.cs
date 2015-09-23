using System.Configuration;
using System.Data.SqlClient;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface ISqlObjectManager
    {
        ConnectionStringSettings GetConnectionStringSettings(string key);

        SqlConnectionStringBuilder BuildConnectionString(ConnectionStringSettings connectionStringSettings);

        SqlConnectionStringBuilder BuildConnectionString(string connectionStringName);

        ConnectionStringSettings BuildConnectionStringSettings(string connectionString);

        void CreateDatabase(SqlConnectionStringBuilder sqlConnectionStringBuilder, string copyDatabasePostFix);

        ServerConnection CreateNewConnection(string dbName, string dbUsername, string dbPassword);

        void DeleteDatabase(SqlConnectionStringBuilder _sqlConnectionStringBuilder);

        void CloseAllDatbaseConnections(Server server, string databaseName);

        void RefreshDatabase(Database database);
    }
}