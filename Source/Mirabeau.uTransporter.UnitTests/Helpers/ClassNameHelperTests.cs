using System.CodeDom;

using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Helpers
{
    [TestFixture]
    public class ClassNameHelperTests
    {
        private IClassNameHelper classNameHelper;

        [SetUp]
        public void SetUp()
        {
            classNameHelper = new ClassNameHelper();
        }

        [Test]
        public void CreateClass_ShouldCreateANewcodeTypeDeclaration_ReturnCodeTypeDelaration()
        {
            var actual = classNameHelper.CreateClass("classname");
            var expected = new CodeTypeDeclaration("Classname");

            Assert.AreEqual(actual.Name, expected.Name);
        }

        [TestCase("SomeClass/", "SomeClass/\\")]
        [TestCase("SomeClass", "SomeClass\\")]
        public void Method_ShouldDo_ShouldReturn(string input, string expectedValue)
        {
            string cleanString = classNameHelper.CheckForTrailingSlash(input);

            Assert.That(cleanString, Is.EqualTo(expectedValue));
        }

        [TestCase("some class", "SomeClass")]
        [TestCase("404Handler", "FourZeroFourHandler")]
        [TestCase("[my class]", "MyClass")]
        [TestCase("*&^&^%%^^&*&*(my c*&&^^@#!@#lass]", "MyClass")]
        [TestCase("5646352410my class", "FiveSixFourSixThreeFiveTwoFourOneZeroMyClass")]
        public void ShouldCreateValidClassName(string input, string expectedValue)
        {
            string safeClassName = classNameHelper.CreateSafeClassName(input);

            Assert.That(safeClassName, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCase(@"\d#$!@(*&validString", "_d#$!@(_&validString")]
        [TestCase(@"\d#$!@(*&validSt\\\@@@r@ing", @"_d#$!@(_&validSt___@@@r@ing")]
        public void RemoveInvalidChars_ShouldRemoveAllLeadingInvalidCharFromString_ReturnValidString(string input, string expectedValue)
        {
            var actual = classNameHelper.RemoveInvalidFilenameChars(input);

            Assert.That(actual, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCase("123645validString", "validString")]
        [TestCase("5validString", "validString")]
        public void TrimDigitFromStartOfString_ShouldTrimAllNumbersFromStartOfTheString_ReturnStringWithoutNumberAtStart(string input, string expectedValue)
        {
            var actual = classNameHelper.TrimDigitFromStartOfString(input);

            Assert.That(actual, Is.EqualTo(expectedValue));
        }
    }
}
