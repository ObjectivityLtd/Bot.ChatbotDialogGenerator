namespace RoslynWrapper.Tests.SyntaxBuilders.Classes
{
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Classes;
    using RoslynWrapper.SyntaxBuilders.Common;
    using Xunit;

    public class ClassBuilderTests
    {
        [Fact]
        public void Can_Create_Class_With_Name()
        {
            // arrange
            const string className = "TestClass";

            // act
            var classDeclaration = new ClassBuilder(className)
                .Build();

            //assert
            var validator = new ClassBuilderVerifier();
            validator.VerifyClassName(classDeclaration.Identifier, expectedClassName: className);
        }

        [Fact]
        public void Can_Create_Class_With_Attribute()
        {
            // arrange
            const string attributeName = "TestAttribute";
            const string attributeArgument = "TestArgument";

            // act
            var classDeclaration = new ClassBuilder("TestClass")
                .WithAttribute(attributeName, attributeArgument)
                .Build();

            //assert
            var validator = new ClassBuilderVerifier();

            var classAttributeList = classDeclaration.AttributeLists.SingleOrDefault();
            validator.VerifyClassAttribute(classAttributeList, attributeName, attributeArgument);
        }

        [Fact]
        public void Can_Create_Class_With_AccessModifier()
        {
            // arrange
            var accessModifier = AccessModifier.Public;

            // act
            var classDeclaration = new ClassBuilder("TestClass")
                .WithAccessModifier(accessModifier)
                .Build();

            //assert
            var validator = new ClassBuilderVerifier();
            validator.VerifyAccessModifier(classDeclaration.Modifiers, accessModifier);
        }

        [Fact]
        public void Can_Create_Class_With_BaseTypes()
        {
            // arrange
            const string someBaseType = "SomeBaseType";
            const string anotherBaseType = "AnotherBaseType";

            // act
            var classDeclaration = new ClassBuilder("TestClass")
                .WithBaseTypes(
                    someBaseType,
                    anotherBaseType)
                .Build();

            //assert
            var validator = new ClassBuilderVerifier();
            validator.VerifyBaseTypes(classDeclaration.BaseList, new[] { someBaseType, anotherBaseType });
        }

        [Fact]
        public void Can_Create_Class_With_Dependencies()
        {
            // arrange
            var firstDependency = new ClassDependency("IFirstDep", "firstDep");
            var secondDependency = new ClassDependency("ISecondDep", "secondDep");

            // act
            var classDeclaration = new ClassBuilder("TestClass")
                .WithDependencies(
                    firstDependency,
                    secondDependency)
                .Build();

            //assert
            var validator = new ClassBuilderVerifier();

            var privateFields = classDeclaration.Members.OfType<FieldDeclarationSyntax>().ToList();
            validator.VerifyPrivateFields(privateFields, new[] { firstDependency.FieldName, secondDependency.FieldName });

            var constructors = classDeclaration.Members.OfType<ConstructorDeclarationSyntax>().ToList();
            validator.VerifyDependencyInjection(constructors, new[] { firstDependency, secondDependency });
        }
    }
}