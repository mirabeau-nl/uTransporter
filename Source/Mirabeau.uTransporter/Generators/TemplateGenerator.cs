using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Generators
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplateGenerator : ITemplateGenerator
    {
        private readonly IFileHelper _fileHelper;
        private readonly ITemplateReadRepository _templateReadRepository;
        private readonly IClassNameHelper _classNameHelper;
        private CodeCompileUnit _codeCompileUnit;
        private CodeNamespace _codeNameSpace;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGenerator"/> class.
        /// </summary>
        /// <param name="fileHelper">The file writer.</param>
        /// <param name="templateReadRepository">The template read repository.</param>
        /// <param name="util">The utility.</param>
        /// <param name="classNameHelper">The class name helper.</param>
        public TemplateGenerator(IFileHelper fileHelper, ITemplateReadRepository templateReadRepository, IClassNameHelper classNameHelper)
        {
            _fileHelper = fileHelper;
            _templateReadRepository = templateReadRepository;
            _classNameHelper = classNameHelper;
        }

        /// <summary>
        /// Generates a template file.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        public int? Generate(string targetPath)
        {
            IEnumerable<ITemplate> templates = _templateReadRepository.GetAllTemplates().ToList();

            foreach (ITemplate template in templates)
            {
                this.BuildImport();
                this.CreateTemplate(targetPath, template);
            }

            return templates.Count();
        }

        /// <summary>
        /// Creates the target path.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="targetPath">The target path.</param>
        /// <returns></returns>
        public string CreateTargetPath(string filename, string targetPath)
        {
            return Utils.Util.CombinePaths(targetPath, Properties.Settings.Default.TemplateDir, Utils.Util.DehumanizeAndTrim(filename) + Properties.Settings.Default.FileExtension);
        }

        /// <summary>
        /// Creates the template.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <param name="template">The template object.</param>
        private void CreateTemplate(string targetPath, ITemplate template)
        {
            _codeCompileUnit = new CodeCompileUnit();
            CodeTypeDeclaration targetClass = _classNameHelper.CreateClass(template.Name);

            CodeAttributeDeclaration classAttr = new CodeAttributeDeclaration();
            classAttr.Name = "Template";
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = "Name", Value = new CodePrimitiveExpression(template.Name) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = "Alias", Value = new CodePrimitiveExpression(template.Alias) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = "Content", Value = new CodePrimitiveExpression(template.Content) });

            targetClass.CustomAttributes.Add(classAttr);
            targetClass.IsClass = true;

            _codeNameSpace.Types.Add(targetClass);
            _codeCompileUnit.Namespaces.Add(_codeNameSpace);

            _fileHelper.WriteFile(this.CreateTargetPath(template.Name, targetPath), _codeCompileUnit);
        }

        private void BuildImport()
        {
            _codeNameSpace = new CodeNamespace(Properties.Settings.Default.TemplateNamespace);
            _codeNameSpace.Imports.Add(new CodeNamespaceImport(Properties.Namespaces.Default.SystemNamespace));
            _codeNameSpace.Imports.Add(new CodeNamespaceImport(Properties.Namespaces.Default.AttributesNamespace));
        }
    }
}
