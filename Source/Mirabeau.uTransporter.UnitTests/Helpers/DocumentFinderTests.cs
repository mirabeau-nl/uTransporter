using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Models;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

namespace Mirabeau.uTransporter.UnitTests.Helpers
{
    [TestFixture]
    public class DocumentFinderTests
    {
        private IDocumentFinder _finder;

        [SetUp]
        public void SetUp()
        {
            _finder = new DocumentFinder();
        }

        [Test]
        public void GetAllIdocumentTypesBase_ShouldFindAssemblies_ReturnEmptyList()
        {
            var actual = _finder.GetSubClassOf(typeof(TestDocumentTypeBaseThirdLevel), false);
            var expected = new List<Type>();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAllIdocumentTypesBase_ShouldFindAssemblies_ReturnTestDocumentTypeBase()
        {
            var actual = _finder.GetSubClassOf(typeof(IDocumentTypeBase), false, Thread.GetDomain().GetAssemblies());
            Assert.Contains(typeof(TestDocumentTypeBase), actual);
        }

        [Test]
        public void GetAllIdocumentTypeBase_ShouldFindAssemblies_ReturnTestDocumentTypeBaseEmpty()
        {
            var actual = _finder.GetSubClassOf(typeof(IDocumentTypeBase), false, Thread.GetDomain().GetAssemblies());
            Assert.Contains(typeof(TestDocumentTypeBaseEmpty), actual);
        }

        [Test]
        public void GetAllIdocumentTypeBase_ShouldFindAssemblies_ReturnTestDocumentTypeBaseMissingAttributes()
        {
            var actual = _finder.GetSubClassOf(typeof(IDocumentTypeBase), false, Thread.GetDomain().GetAssemblies());
            Assert.Contains(typeof(TestDocumentTypeBaseMissingAttribute), actual);
        }

        [Test]
        public void GetAllIdocumentTypeBase_ShouldFindAssemblies_ReturnTestDocumentTypeBaseSecondLevel()
        {
            var actual = _finder.GetSubClassOf(typeof(IDocumentTypeBase), false, Thread.GetDomain().GetAssemblies());
            Assert.Contains(typeof(TestDocumentTypeBaseSecondLevel), actual);
        }

        [Test]
        public void GetSecondLevelTypes_WhenBaseClassHAsNoChilder_ReturnEmptyList()
        {
            var actual = _finder.GetChildTypes(typeof(TestDocumentTypeBaseEmpty));
            var expected = new List<Type>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSecondLevelTypes_WhenBaseClassHasChilder_ReturnChilderen()
        {
            var actual = _finder.GetChildTypes(typeof(TestDocumentTypeBase));
            Assert.Contains(typeof(TestDocumentTypeBaseSecondLevel), actual);
        }

        [Test]
        public void GetThridLevel_ShouldDo_ShouldReturn()
        {
            var actual = _finder.GetChildTypes(typeof(TestDocumentTypeBaseSecondLevel));
            Assert.Contains(typeof(TestDocumentTypeBaseThirdLevel), actual);
        }

        [Test]
        public void Method_ShouldDo_ShouldReturn()
        {
            IDocumentFinder finderStub = MockRepository.GenerateStub<IDocumentFinder>();
            finderStub.Expect(m => m.GetSubClassOf(null, true, null))
                .IgnoreArguments()
                .Throw(new ReflectionTypeLoadException(null, null));

            finderStub.VerifyAllExpectations();
        }
    }
}