using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Mirabeau.uTransporter.Comparers;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core;
using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Generators
{
    /// <summary>
    /// Classs for generating document types
    /// </summary>
    /// <remarks></remarks>
    public class DocumentTypeGenerator : IDocumentTypeGenerator
    {
        private readonly IContentReadRepository _contentReadRepository;
        private readonly IFileHelper _fileHelper;
        private readonly IDataTypeManager _dataTypeManager;
        private readonly IPropertyReadRepository _propertyReadRepository;
        private readonly IClassNameHelper _classNameHelper;

        #region Public methods
        public DocumentTypeGenerator(IContentReadRepository contentReadRepository, IFileHelper fileHelper, IDataTypeManager dataTypeManager, IPropertyReadRepository propertyReadRepository, IClassNameHelper classNameHelper)
        {
            _contentReadRepository = contentReadRepository;
            _fileHelper = fileHelper;
            _dataTypeManager = dataTypeManager;
            _propertyReadRepository = propertyReadRepository;
            _classNameHelper = classNameHelper;
        }

        public int Generate(string targetPath)
        {
            Logger.WriteInfoLine<DocumentTypeGenerator>("Starting with generation of document types......");

            IEnumerable<IContentType> allContentTypes = _contentReadRepository.GetAllContentTypes().ToList();

            if (!allContentTypes.Any())
            {
                Logger.WriteInfoLine<DocumentTypeGenerator>("No document types to export because there are not document type in the database.");
            }

            allContentTypes.ForEach(contentType => CreateDocumentType(targetPath, contentType));

            return allContentTypes.Count();
        }

        public void Delete(string targetPath)
        {
            _fileHelper.DeleteFilesInDir(Path.Combine(targetPath, Properties.Settings.Default.DocumentTypesDir));
        }

        public CodeTypeDeclaration CreateClass(IContentType contentType)
        {
            // remove digit and invalid chars
            string className = _classNameHelper.CreateSafeClassName(contentType.Name);

            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(className);
            targetClass.IsClass = true;

            if (contentType.ParentId == -1)
            {
                targetClass.BaseTypes.Add(Properties.DocumentType.Default.DocumentTypeBaseClassName);
            }
            else
            {
                IContentType parentContentType = _contentReadRepository.GetContentTypesBasedOnId(contentType.ParentId);
                string parentClassName = _classNameHelper.CreateSafeClassName(parentContentType.Name);
                targetClass.BaseTypes.Add(parentClassName);
            }

            return targetClass;
        }

        private CodeTypeDeclaration CreateTabClass(string className)
        {
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(className);
            targetClass.IsClass = true;

            return targetClass;
        }
        #endregion

        #region Private methods
        private void CreateDocumentType(string targetPath, IContentType contentType)
        {
            CodeTypeDeclaration targetClass = CreateClass(contentType);

            CodeAttributeDeclaration attributes = CreateAttributes(contentType);
            targetClass.CustomAttributes.Add(attributes);

            CreateProperties(targetClass, contentType);

            CodeNamespace codeNameSpace = BuildUsings();
            codeNameSpace.Types.Add(targetClass);

            // get all the tabs for a document type
            List<PropertyGroup> tabNamesForDocumentType = _propertyReadRepository.GetTabNamesForDocumentType(contentType).Values.ToList();

            foreach (PropertyGroup propertyGroup in tabNamesForDocumentType.Distinct(new PropertyGroupEqualityCompairer()))
            {
                string combinedClassName = string.Format("{0}_{1}", targetClass.Name, propertyGroup.Name);
                CodeTypeDeclaration tabClass = CreateTabClass(combinedClassName);

                CodeAttributeDeclaration tabClassAttr = new CodeAttributeDeclaration();
                tabClassAttr.Name = Properties.Tab.Default.TabName;
                tabClassAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Tab.Default.Name, Value = new CodePrimitiveExpression(propertyGroup.Name) });
                tabClassAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Tab.Default.SortOrder, Value = new CodePrimitiveExpression(propertyGroup.SortOrder) });

                tabClass.CustomAttributes.Add(tabClassAttr);
                codeNameSpace.Types.Add(tabClass);
            }

            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            codeCompileUnit.Namespaces.Add(codeNameSpace);

            // create the filename
            string fileName = string.Format(
                "{0}{1}",
                Utils.Util.DehumanizeAndTrim(targetClass.Name),
                Properties.Settings.Default.FileExtension);

            // combine all paths
            string combinePaths = Utils.Util.CombinePaths(targetPath, Properties.Settings.Default.DocumentTypesDir, fileName);

            // write the class definition to file
            _fileHelper.WriteFile(combinePaths, codeCompileUnit);
        }

        private CodeAttributeDeclaration CreateAttributes(IContentType contentType)
        {
            CodeAttributeDeclaration classAttr = new CodeAttributeDeclaration();
            classAttr.Name = Properties.DocumentType.Default.DocumentTypeName;
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.Name, Value = new CodePrimitiveExpression(contentType.Name) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.Alias, Value = new CodePrimitiveExpression(contentType.Alias) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.Icon, Value = new CodePrimitiveExpression(contentType.Icon) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.Thumb, Value = new CodePrimitiveExpression(contentType.Thumbnail) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.Description, Value = new CodePrimitiveExpression(contentType.Description ?? string.Empty) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.AllowAtRoot, Value = new CodePrimitiveExpression(contentType.AllowedAsRoot) });

            string defeaultTemplate = BuildDefaultTemplate(contentType);

            if (defeaultTemplate != string.Empty)
            {
                classAttr.Arguments.Add(
                    new CodeAttributeArgument
                    {
                        Name = Properties.DocumentType.Default.DefaultTemplate,
                        Value = new CodeTypeOfExpression(defeaultTemplate)
                    });
            }

            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.AllowedTemplates, Value = GetCodeExpression(contentType.AllowedTemplates) });
            classAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.DocumentType.Default.AllowedChildNodeTypes, Value = CreateAllowedTypeOfArrayExpression(contentType.AllowedContentTypes) });

            return classAttr;
        }

        private string BuildDefaultTemplate(IContentType contentType)
        {
            string templateName = GetTemplateName(contentType);

            if (templateName == string.Empty)
            {
                return string.Empty;
            }

            return string.Format("{0}{1}{2}", Properties.Settings.Default.TemplateNamespace, ".", _classNameHelper.CreateSafeClassName(templateName));
        }

        private string GetTemplateName(IContentType contentType)
        {
            if (contentType.DefaultTemplate == null)
            {
                return string.Empty;
            }

            return contentType.DefaultTemplate.Name;
        }

        private CodeArrayCreateExpression CreateAllowedTypeOfArrayExpression(IEnumerable<ContentTypeSort> allowedChildNodeTypes)
        {
            CodeArrayCreateExpression codeArrayExpression = new CodeArrayCreateExpression(typeof(Type));

            foreach (ContentTypeSort allowedChild in allowedChildNodeTypes)
            {
                IContentType allowedContentType = _contentReadRepository.GetContentTypeBasedOnAlias(allowedChild.Alias);
                string allowedChildAsString = _classNameHelper.CreateSafeClassName(allowedContentType.Name);
                codeArrayExpression.Initializers.Add(new CodeTypeOfExpression(Properties.Settings.Default.DocumentTypesDir + "." + allowedChildAsString));
            }

            return codeArrayExpression;
        }

        private CodeArrayCreateExpression GetCodeExpression(IEnumerable<ITemplate> allowedTemplates)
        {
            CodeArrayCreateExpression codeArrayExpression = new CodeArrayCreateExpression(typeof(Type));

            foreach (ITemplate allowedTemplate in allowedTemplates)
            {
                string allowedTemplateAsString = _classNameHelper.CreateSafeClassName(allowedTemplate.Name);
                codeArrayExpression.Initializers.Add(new CodeTypeOfExpression(string.Format("{0}{1}{2}", Properties.Settings.Default.TemplateNamespace, ".", allowedTemplateAsString)));
            }

            return codeArrayExpression;
        }

        private CodeNamespace BuildUsings()
        {
            CodeNamespace codeNameSpace = new CodeNamespace(Properties.Namespaces.Default.DocumentTypeNamespace);
            codeNameSpace.Imports.Add(new CodeNamespaceImport(Properties.Namespaces.Default.SystemNamespace));
            codeNameSpace.Imports.Add(new CodeNamespaceImport(Properties.Namespaces.Default.AttributesNamespace));
            codeNameSpace.Imports.Add(new CodeNamespaceImport(Properties.Namespaces.Default.EnumsNamespace));
            codeNameSpace.Imports.Add(new CodeNamespaceImport(Properties.Namespaces.Default.ModelsNamespace));

            return codeNameSpace;
        }

        private void CreateProperties(CodeTypeDeclaration targetClass, IContentType contentType)
        {
            foreach (PropertyType propertyType in contentType.PropertyTypes)
            {
                string dataTypeDefinitionName = GetDataTypeDefinitionNameByGuidTrimSpace(propertyType);

                CodeAttributeDeclaration propertyAttr = new CodeAttributeDeclaration();
                propertyAttr.Name = Properties.Property.Default.DocumentTypeProperty;
                propertyAttr.Arguments.Add(new CodeAttributeArgument(new CodeTypeReferenceExpression(string.Format("UmbracoPropertyType.{0}", dataTypeDefinitionName))));

                if (dataTypeDefinitionName.Equals(Properties.Property.Default.Other))
                {
                    var customDataTypeName = this.GetDataTypeDefintion(propertyType);
                    propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.OtherTypeName, Value = new CodePrimitiveExpression(customDataTypeName) });
                }

                propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.Name, Value = new CodePrimitiveExpression(propertyType.Name) });
                propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.Alias, Value = new CodePrimitiveExpression(propertyType.Alias) });
                propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.Description, Value = new CodePrimitiveExpression(propertyType.Description) });

                // Get all the tabs for a document type
                IDictionary<string, PropertyGroup> tabNamesForDocumentType = _propertyReadRepository.GetTabNamesForDocumentType(contentType);

                bool propertyTypeHasTab = tabNamesForDocumentType.ContainsKey(propertyType.Alias);
                if (propertyTypeHasTab)
                {
                    string tabName = tabNamesForDocumentType.FirstOrDefault(x => x.Key == propertyType.Alias).Value.Name;

                    // pick only tab that matches with the property
                    CodeAttributeArgument tab = new CodeAttributeArgument();
                    tab.Name = Properties.Property.Default.Tab;
                    tab.Value =
                        new CodeTypeOfExpression(
                            string.Format("{0}_{1}", targetClass.Name, tabName));

                    propertyAttr.Arguments.Add(tab);
                }

                propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.SortOrder, Value = new CodePrimitiveExpression(propertyType.SortOrder) });
                propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.Mandatory, Value = new CodePrimitiveExpression(propertyType.Mandatory) });
                propertyAttr.Arguments.Add(new CodeAttributeArgument { Name = Properties.Property.Default.ValidationRegExp, Value = new CodePrimitiveExpression(propertyType.ValidationRegExp) });

                this.AddProperty(targetClass, propertyType.Alias, typeof(string), propertyAttr);
            }
        }

        private string GetDataTypeDefintion(PropertyType propertyType)
        {
            IDataTypeDefinition dataTypeDefinition = _dataTypeManager.GetDataTypeDefinitionByGuid(propertyType.DataTypeDefinitionId);

            return dataTypeDefinition.Name.Trim();
        }

        private string GetDataTypeDefinitionNameByGuidTrimSpace(PropertyType propertyType)
        {
            IDataTypeDefinition dataTypeDefinition = _dataTypeManager.GetDataTypeDefinitionByGuid(propertyType.DataTypeDefinitionId);

            if (Enum.GetValues(typeof(UmbracoPropertyType)).Cast<UmbracoPropertyType>().Any(property => ((int)property) == dataTypeDefinition.Id))
            {
                UmbracoPropertyType umbracoPropertyType = (UmbracoPropertyType)dataTypeDefinition.Id;
                return umbracoPropertyType.ToString();
            }

            return Properties.Property.Default.Other;
        }

        private void AddProperty(CodeTypeDeclaration target, string fieldName, Type type, CodeAttributeDeclaration propertyAttr)
        {
            string safeFieldName = _classNameHelper.CreateSafeClassName(fieldName);

            string privatefieldname = string.Format("_{0}", safeFieldName);

            CodeMemberField field = new CodeMemberField();
            field.Name = "_" + safeFieldName;
            field.Type = new CodeTypeReference(type);
            field.Attributes = MemberAttributes.Private;
            target.Members.Add(field);

            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Name = safeFieldName;
            prop.Type = new CodeTypeReference(type);
            prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            prop.CustomAttributes.Add(propertyAttr);

            prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), privatefieldname)));

            prop.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), privatefieldname),
                    new CodePropertySetValueReferenceExpression()));

            target.Members.Add(prop);
        }
        #endregion
    }
}