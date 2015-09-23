using Mirabeau.uTransporter.Builders;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Stubs
{
    public class TestCustomTemplateBuilder : TemplateBuilder
    {
        private readonly string _name;
        private readonly string _alias;

        public TestCustomTemplateBuilder(string name, string alias)
            : base(name, alias)
        {
            _name = name;
            _alias = alias;
        }

        public override ITemplate CreateTemplateInstance()
        {
            ITemplate template = MockRepository.GenerateStub<ITemplate>();
            template.Stub(x => x.Name).Return(_name);
            template.Stub(x => x.Alias).Return((_alias ?? string.Empty).ToLower());

            return template;
        }
    }
}
