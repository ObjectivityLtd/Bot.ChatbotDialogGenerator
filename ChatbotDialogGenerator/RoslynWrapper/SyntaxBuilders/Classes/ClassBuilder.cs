namespace RoslynWrapper.SyntaxBuilders.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Classes.Constructors;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.Attributes;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;
    using RoslynWrapper.SyntaxBuilders.Statements.Declarations;

    public class ClassBuilder
    {
        private readonly string _className;
        private readonly List<MemberDeclarationSyntax> _properties;
        private readonly List<MemberDeclarationSyntax> _dependencies;
        private readonly List<MemberDeclarationSyntax> _methods;

        private ConstructorBuilder _constructorBuilder;
        private AccessModifier? _accessModifier;
        private AttributeSyntax _attributeSyntax;
        private BaseTypeSyntax[] _baseTypes;
        private bool _isStatic;

        public ClassBuilder(string className)
        {
            _className = className;

            _methods = new List<MemberDeclarationSyntax>();
            _dependencies = new List<MemberDeclarationSyntax>();
            _properties = new List<MemberDeclarationSyntax>();
        }

        public ClassBuilder WithAttribute(string attributeName, string argumentType)
        {
            _attributeSyntax = new AttributeBuilder()
                .WithName(attributeName)
                .WithTypeOfArgument(argumentType).Build();

            return this;
        }

        public ClassBuilder WithBaseTypes(params string[] baseTypes)
        {
            _baseTypes = baseTypes.Select(type => SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(type)))
                .ToArray();

            return this;
        }

        public ClassBuilder WithFields(params FieldDeclarationSyntax[] fields)
        {
            _dependencies.AddRange(fields);

            return this;
        }

        public ClassBuilder WithDependencies(params ClassDependency[] classDependencies)
        {
            if (classDependencies == null)
            {
                throw new ArgumentNullException(nameof(classDependencies));
            }
            _constructorBuilder = new ConstructorBuilder(_className);

            _constructorBuilder.WithAttribute("ImportingConstructor");

            foreach (var dto in classDependencies)
            {
                _dependencies.Add(
                    FieldDeclarationSyntaxExtensions.Build(dto.TypeName, dto.FieldName).WithAccessModifier(AccessModifier.Private).SetReadonly());

                _constructorBuilder.InjectParameter(new Parameter(dto.TypeName, dto.FieldName));
            }

            return this;
        }

        public ClassBuilder AddFields(params FieldDeclarationSyntax[] properties)
        {
            _properties.AddRange(properties);

            return this;
        }

        public ClassBuilder AddMethod(MethodDeclarationSyntax method)
        {
            _methods.Add(method);

            return this;
        }

        public ClassBuilder AddMethods(MethodDeclarationSyntax[] methods)
        {
            _methods.AddRange(methods);

            return this;
        }

        public ClassBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            _accessModifier = accessModifier;

            return this;
        }

        public ClassBuilder SetStatic()
        {
            _isStatic = true;

            return this;
        }

        public ClassDeclarationSyntax Build()
        {
            var declaration = SyntaxFactory.ClassDeclaration(_className);

            if (_attributeSyntax != null)
                declaration = declaration.WithAttribute(_attributeSyntax);

            if (_baseTypes != null && _baseTypes.Length > 0)
                declaration = declaration.AddBaseListTypes(_baseTypes);

            if (_dependencies.Count > 0)
                declaration = declaration.AddMembers(_dependencies.ToArray());

            if (_constructorBuilder != null)
                declaration = declaration.AddMembers(_constructorBuilder.Build());

            if (_accessModifier != null)
                declaration = declaration.AddModifiers(SyntaxFactory.Token(_accessModifier.Value.MapToSyntaxKind()));

            if (_methods.Count > 0)
                declaration = declaration.AddMembers(_methods.ToArray());

            if (_properties.Count > 0)
                declaration = declaration.AddMembers(_properties.ToArray());

            if (_isStatic)
                declaration = declaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

            return declaration;
        }
    }
}