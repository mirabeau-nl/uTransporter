using System;

using Mirabeau.uTransporter.Logging;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Logging
{
    [TestFixture]
    public class LogFileServiceTests
    {
        [Test]
        public void UmbracoSyncLogPath_GetterBasePathShouldGiveLogPath_ReturnLogPath()
        {
            //Arrange
            var actual = LogFileService.UmbracoSyncLogPath;
            var expected = "~/App_Plugins/umbraco-sync-dashboard/logs/";
            
            // Act

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void BuildFilePath_ShouldCombineStringIntoBasePath_ReturnConcadString()
        {
            // Arrange
            DateTime dateTime = new DateTime();
            
            // Act
            var actual = LogFileService.BuildFilePath();
            var expected = "/App_Plugins/umbraco-sync-dashboard/logs/UmbracoSyncLog" + "-" + dateTime.ToString("yy-MM-dd") + ".txt";
            
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void BuildFilePathWithHostName_ShouldBuildAPathWithTheHostName_ReturnPathWithHostName()
        {
            //Arrange
            string expected = "UmbracoSyncLog-01-01-01.txt";

            //Act
            string pathWithHostName = LogFileService.BuildFilePathWithHostName();

            //Assert
            Assert.That(pathWithHostName, Is.EqualTo(expected));
        }
    }
}
