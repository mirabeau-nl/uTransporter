using System;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Managers;

using NUnit.Framework;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Managers
{
    [TestFixture]
    public class PropertyManagerTests
    {
      [Test]
      public void RemovePropertyType_ShouldRemovePropertyType_ReturnTrue()
      {
          IPropertyManager propertyManager = new PropertyManager();
          propertyManager.RemovePropertyType(new PropertyType(new DataTypeDefinition(-1, new Guid())), new ContentType(-1));
      }
    }
}