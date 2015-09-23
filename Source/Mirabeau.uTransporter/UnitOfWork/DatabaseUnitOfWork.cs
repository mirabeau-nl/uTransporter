using System;
using System.Collections.Generic;

using Umbraco.Core.Models.EntityBase;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;

namespace Mirabeau.uTransporter.UnitOfWork
{
    /// <summary>
    /// Custom database unit of work that accepts connection strings (Based on PetaPoco UoW)
    /// </summary>
    public class DatabaseUnitOfWork : IDatabaseUnitOfWork
    {
        private readonly Queue<Operation> _operations = new Queue<Operation>();
        private Guid _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUnitOfWork"/> class.
        /// </summary>
        /// <param name="connectionString">Connection String for target database</param>
        public DatabaseUnitOfWork(string connectionString)
        {
            Database = new UmbracoDatabase(connectionString, "System.Data.SqlClient");
            _key = Guid.NewGuid();
        }

        /// <summary>
        /// Registers an <see cref="IEntity" /> instance to be added through this <see cref="UnitOfWork" />
        /// </summary>
        /// <param name="entity">The <see cref="IEntity" /></param>
        /// <param name="repository">The <see cref="IUnitOfWorkRepository" /> participating in the transaction</param>
        public void RegisterAdded(IEntity entity, IUnitOfWorkRepository repository)
        {
            _operations.Enqueue(new Operation
            {
                Entity = entity,
                Repository = repository,
                Type = TransactionType.Insert
            });
        }

        /// <summary>
        /// Registers an <see cref="IEntity" /> instance to be changed through this <see cref="UnitOfWork" />
        /// </summary>
        /// <param name="entity">The <see cref="IEntity" /></param>
        /// <param name="repository">The <see cref="IUnitOfWorkRepository" /> participating in the transaction</param>
        public void RegisterChanged(IEntity entity, IUnitOfWorkRepository repository)
        {
            _operations.Enqueue(
                new Operation
                {
                    Entity = entity,
                    Repository = repository,
                    Type = TransactionType.Update
                });
        }

        /// <summary>
        /// Registers an <see cref="IEntity" /> instance to be removed through this <see cref="UnitOfWork" />
        /// </summary>
        /// <param name="entity">The <see cref="IEntity" /></param>
        /// <param name="repository">The <see cref="IUnitOfWorkRepository" /> participating in the transaction</param>
        public void RegisterRemoved(IEntity entity, IUnitOfWorkRepository repository)
        {
            _operations.Enqueue(
                new Operation
                {
                    Entity = entity,
                    Repository = repository,
                    Type = TransactionType.Delete
                });
        }

        /// <summary>
        /// Commits all batched changes within the scope of a PetaPoco transaction <see cref="Transaction"/>
        /// </summary>
        /// <remarks>
        /// Unlike a typical unit of work, this UOW will let you commit more than once since a new transaction is creaed per
        /// Commit() call instead of having one Transaction per UOW. 
        /// </remarks>
        public void Commit()
        {
            Commit(null);
        }

        /// <summary>
        /// Commits all batched changes within the scope of a PetaPoco transaction <see cref="Transaction"/>
        /// </summary>
        /// <param name="transactionCompleting">
        /// Allows you to set a callback which is executed before the transaction is committed, allow you to add additional SQL
        /// operations to the overall commit process after the queue has been processed.
        /// </param>
        internal void Commit(Action<UmbracoDatabase> transactionCompleting)
        {
            using (var transaction = Database.GetTransaction())
            {
                while (_operations.Count > 0)
                {
                    var operation = _operations.Dequeue();
                    switch (operation.Type)
                    {
                        case TransactionType.Insert:
                            operation.Repository.PersistNewItem(operation.Entity);
                            break;
                        case TransactionType.Delete:
                            operation.Repository.PersistDeletedItem(operation.Entity);
                            break;
                        case TransactionType.Update:
                            operation.Repository.PersistUpdatedItem(operation.Entity);
                            break;
                    }
                }

                //Execute the callback if there is one
                if (transactionCompleting != null)
                {
                    transactionCompleting(Database);
                }

                transaction.Complete();
            }

            // Clear everything
            _operations.Clear();
            _key = Guid.NewGuid();
        }

        public object Key
        {
            get { return _key; }
        }

        public UmbracoDatabase Database { get; private set; }

        #region Operation

        /// <summary>
        /// Provides a snapshot of an entity and the repository reference it belongs to.
        /// </summary>
        private sealed class Operation
        {
            /// <summary>
            /// Gets or sets the entity.
            /// </summary>
            /// <value>The entity.</value>
            public IEntity Entity { get; set; }

            /// <summary>
            /// Gets or sets the repository.
            /// </summary>
            /// <value>The repository.</value>
            public IUnitOfWorkRepository Repository { get; set; }

            /// <summary>
            /// Gets or sets the type of operation.
            /// </summary>
            /// <value>The type of operation.</value>
            public TransactionType Type { get; set; }
        }

        internal enum TransactionType
        {
            Insert,
            Update,
            Delete
        }

        #endregion

        public void Dispose()
        {

        }
    }
}