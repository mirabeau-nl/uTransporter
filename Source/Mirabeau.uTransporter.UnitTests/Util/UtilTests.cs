using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Util
{
    [TestFixture]
    public class UtilTests
    {
        private string _testStringLong;

        private string _testStringShort;

        private string _documentTypeStubDir;

        [SetUp]
        public void SetUp()
        {
            _testStringLong = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras et ante et metus vehicula ultricies " +
                                "sit amet nec nibh. Morbi viverra turpis lectus, vel tempor purus porttitor ut. In id pulvinar diam. " +
                                "Nunc vehicula sed orci sit amet sodales. Praesent mattis suscipit rhoncus. Curabitur pretium eu urna " +
                                "quis sagittis.";
            _testStringShort = "Lorem ipsum dolor";
            _documentTypeStubDir = "../../Stubs/DocumentTypes/";
        }

        [Test]
        public void ShouldReturnListofFileNames()
        {
            // Arrange
            List<string> actual = uTransporter.Utils.Util.GetFilesNamesFromDir(_documentTypeStubDir);

            // Act
            List<string> expected = Directory.GetFiles(_documentTypeStubDir, "*.cs")
                                            .Select(Path.GetFileNameWithoutExtension)
                                            .ToList();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void TrimLenght_TrimStringWhenLongerThan255_ReturnTrimedString()
        {
            var excual = uTransporter.Utils.Util.TrimLength(_testStringLong, 255);
            Assert.AreEqual(255, excual.Length);
        }

        [Test]
        public void TrimLenght_TrimStringWhenShorterThan255_ReturnString()
        {
            var excual = uTransporter.Utils.Util.TrimLength(_testStringShort, 255);
            Assert.AreEqual(17, excual.Length);
        }

        [Test]
        public void CombinePath_ShouldCombineMultipleStringsIntoPath_ReturnPath()
        {
            var actual = uTransporter.Utils.Util.CombinePaths("path1", "path2", "path3", "path4");
            var expected = @"path1\path2\path3\path4";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void DehumanizeAndTrim_ShouldDeHumanizeAndTrimAString_ShouldReturnDehuanizeedString()
        {
            var actual = uTransporter.Utils.Util.DehumanizeAndTrim("Lorem ipsum dolor sit amet");

            Assert.That(actual, Is.EqualTo("LoremIpsumDolorSitAmet"));
        }

        [Test]
        public void DoesDirExists_ShouldDetermineIfADirExists_ReturnTrue()
        {
            var actual = uTransporter.Utils.Util.DoesDirExists(@"C:\Program Files");

            Assert.That(actual, Is.True);
        }

        [Test]
        [Ignore]
        public void DoesFileExists_ShouldDetermineIfAFileExists_ReturnTrue()
        {
            var actual = uTransporter.Utils.Util.DoesFileExists(AppDomain.CurrentDomain.BaseDirectory + @"\Mirabeau.uTransporter.UnitTests.dll");

            Assert.That(actual, Is.True);
        }

        [Test]
        public void DoesDirExists_ShouldDetermineIfADirExists_ReturnFalse()
        {
            var actual = uTransporter.Utils.Util.DoesDirExists(@"C:\Path\That\Does\Not\Exists");

            Assert.That(actual, Is.False);
        }

        [Test]
        public void DoesFileExists_ShouldDetermineIfAFileExists_ReturnFalse()
        {
            var actual = uTransporter.Utils.Util.DoesFileExists(AppDomain.CurrentDomain.BaseDirectory + @"\DllThatDoesNotExists.dll");

            Assert.That(actual, Is.False);
        }
    }
}