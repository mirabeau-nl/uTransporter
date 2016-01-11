using System;
using System.Configuration;
using System.Data.SqlClient;

using Microsoft.SqlServer.Management.Common;

using Mirabeau.uTransporter.Services;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Services
{
    public class SqlObjectManagerTests
    {
        private SqlObjectManager sqlObjectManager;

        [SetUp]
        public void SetUp()
        {
            sqlObjectManager = new SqlObjectManager();
        }

        [Test]
        public void BuildConnectionString_ShouldBuildAValidConnectionString_ReturnConnectionString()
        {
            // Arrange
            ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings();
            connectionStringSettings.ConnectionString = "server=Foo;database=Bar;user id=Foo_Bar;password=Baz";

            // Act
            var actual = sqlObjectManager.BuildConnectionString(connectionStringSettings);
            var expected = new SqlConnectionStringBuilder();
            expected.ConnectionString = "server=Foo;database=Bar;user id=Foo_Bar;password=Baz";

            // Assert
            Assert.That(actual.ConnectionString, Is.EqualTo(expected.ConnectionString));
        }
    }
}
