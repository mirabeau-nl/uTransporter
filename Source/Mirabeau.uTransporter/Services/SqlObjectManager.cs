using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Configuration;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

namespace Mirabeau.uTransporter.Services
{
    public class SqlObjectManager : ISqlObjectManager
    {
        private string _dbServer;

        private string _dbUsername;

        private string _dbPassword;

        private string _dbDatabase;

        /// <summary>
        /// Gets the connection string settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ConnectionStringSettings object</returns>
        public ConnectionStringSettings GetConnectionStringSettings(string key)
        {
            return WebConfigurationManager.ConnectionStrings[key];
        }

        /// <summary>
        /// Builds the connection string settings.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>ConnectionStringSetings object</returns>
        public ConnectionStringSettings BuildConnectionStringSettings(string connectionString)
        {
            return new ConnectionStringSettings("Bla", connectionString);
        }

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        /// <returns>SqlConnectionStringBuilder object</returns>
        public SqlConnectionStringBuilder BuildConnectionString(ConnectionStringSettings connectionStringSettings)
        {
            return new SqlConnectionStringBuilder(connectionStringSettings.ConnectionString);
        }

        /// <summary>
        /// Retreives a connectionstring and returns it in form of a SqlConnectionStringBuilder
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string</param>
        /// <returns>SqlConnectionStringBuilder object</returns>
        public SqlConnectionStringBuilder BuildConnectionString(string connectionStringName)
        {
            ConnectionStringSettings connectionStringSettings = GetConnectionStringSettings(connectionStringName);
            return new SqlConnectionStringBuilder(connectionStringSettings.ConnectionString);
        }

        /// <summary>
        /// Creates the new connection.
        /// </summary>
        /// <param name="dbServer">The database server.</param>
        /// <param name="dbUsername">The database username.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>ServerConnection object</returns>
        public ServerConnection CreateNewConnection(string dbServer, string dbUsername, string dbPassword)
        {
            return new ServerConnection(dbServer, dbUsername, dbPassword);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="_sqlConnectionStringBuilder">The _SQL connection string builder.</param>
        /// <param name="copyDatabasePostfix">The copy database postfix.</param>
        public void CreateDatabase(SqlConnectionStringBuilder _sqlConnectionStringBuilder, string copyDatabasePostfix)
        {
            /* SOURCE Database */
            _dbServer = _sqlConnectionStringBuilder.DataSource;
            _dbDatabase = _sqlConnectionStringBuilder.InitialCatalog;
            _dbUsername = _sqlConnectionStringBuilder.UserID;
            _dbPassword = _sqlConnectionStringBuilder.Password;

            /* DESTINATION Database */
            string destDatabase = _sqlConnectionStringBuilder.InitialCatalog + "_" + copyDatabasePostfix;

            // Create copy
            ServerConnection con = CreateNewConnection(_dbServer, _dbUsername, _dbPassword);
            Server sqlServer = new Server(con);
            Database existingDb = sqlServer.Databases[_dbDatabase];

            if (sqlServer.Databases[destDatabase] == null)
            {
                Database newDatabase = new Database(sqlServer, destDatabase);
                newDatabase.Create();

                Logger.WriteInfoLine<SqlObjectManager>("Created new dry-run database, with name {0}", newDatabase.Name);
            }

            this.TransferData(existingDb, destDatabase);
        }

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="_sqlConnectionStringBuilder">The _SQL connection string builder.</param>
        public void DeleteDatabase(SqlConnectionStringBuilder _sqlConnectionStringBuilder)
        {
            ServerConnection con = CreateNewConnection(
                _sqlConnectionStringBuilder.DataSource,
                _sqlConnectionStringBuilder.UserID,
                _sqlConnectionStringBuilder.Password);
            Server sqlServer = new Server(con);
            Database databaseToDrop = new Database(sqlServer, _sqlConnectionStringBuilder.InitialCatalog);

            // Refresh the database list in SQL server
            this.RefreshDatabase(databaseToDrop);

            // Close all active connections
            this.CloseAllDatbaseConnections(sqlServer, databaseToDrop.Name);
            databaseToDrop.Drop();
        }

        /// <summary>
        /// Refreshes the database.
        /// </summary>
        /// <param name="database">The database.</param>
        public void RefreshDatabase(Database database)
        {
            database.Refresh();
        }

        /// <summary>
        /// Closes all datbase connections.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <exception cref="System.NullReferenceException"></exception>
        public void CloseAllDatbaseConnections(Server server, string databaseName)
        {
            try
            {
                server.KillAllProcesses(databaseName);
            }
            catch (NullReferenceException ex)
            {
                Logger.WriteErrorLine<SqlObjectManager>("Can't close the connection on {0} with datbase {1}, exception {2}", server.Name, databaseName, ex);
                throw new NullReferenceException();
            }
        }

        private void TransferData(Database existingDb, string destDatabase)
        {
            Transfer transfer = new Transfer(existingDb);

            transfer.CopyAllUsers = false;
            transfer.CreateTargetDatabase = false;
            transfer.CopyAllObjects = false;
            transfer.CopyAllTables = true;

            transfer.Options.WithDependencies = true;
            transfer.Options.DriAllConstraints = false;
            transfer.Options.DriAllKeys = false;
            transfer.Options.DriForeignKeys = false;
            transfer.Options.DriPrimaryKey = false;

            transfer.Options.ContinueScriptingOnError = true;
            transfer.Options.DriAllConstraints = false;

            transfer.CopyData = true;
            transfer.DropDestinationObjectsFirst = true;
            transfer.CopySchema = true;
            transfer.CreateTargetDatabase = false;
            transfer.DestinationLoginSecure = false;
            transfer.DestinationServer = _dbServer;
            transfer.DestinationLogin = _dbUsername;
            transfer.DestinationPassword = _dbPassword;
            transfer.DestinationDatabase = destDatabase;
            transfer.TransferData();

            Logger.WriteInfoLine<SqlObjectManager>("All data transfered to the new database {0}", destDatabase);
        }
    }
}
