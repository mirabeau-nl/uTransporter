using Umbraco.Core.Persistence;

namespace Mirabeau.uTransporter.Persistence.UnitOfWork
{
    public class DatabaseUnitOfWork
    {
        public DatabaseUnitOfWork(string connectionString)
        {
            Database = new Database(connectionString, "System.Data.SqlClient");
        }

        public Database Database { get; set; }
    }
}
