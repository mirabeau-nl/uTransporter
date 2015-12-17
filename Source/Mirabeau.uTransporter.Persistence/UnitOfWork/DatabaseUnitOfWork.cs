using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Mirabeau.uTransporter.Persistence.UnitOfWork
{
    public class DatabaseUnitOfWork : IDatabaseUnitOfWork
    {
        public Database CreateDatabaseUnitOfWork()
        {
            Database = ApplicationContext.Current.DatabaseContext.Database;

            return Database;
        }

        public Database Database { get; set; }
    }

    public interface IDatabaseUnitOfWork
    {
        Database CreateDatabaseUnitOfWork();
    }
}
