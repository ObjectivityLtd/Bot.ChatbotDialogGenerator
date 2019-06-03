namespace RoslynWrapper.Tests.SyntaxBuilders.Classes.Constructors
{
    using System.Linq;
    using RoslynWrapper.SyntaxBuilders.Classes.Constructors;
    using Xunit;

    public class ConstructorBuilderTests
    {
        [Fact]
        public void Can_Create_Empty_Constructor()
        {
            // arrange
            const string className = "TestClass";

            // act
            var constructorDeclaration = new ConstructorBuilder(className)
                .Build();

            //assert
            var validator = new ConstructorBuilderVerifier();
            validator.VerifyConstructorName(constructorDeclaration.Identifier, expectedName: className);
        }

        [Fact]
        public void Can_Add_Attribute_To_Constructor()
        {
            // arrange
            const string attributeName = "TestAttribute";

            // act
            var constructorDeclaration = new ConstructorBuilder("TestClass")
                .WithAttribute(attributeName)
                .Build();

            //assert
            var validator = new ConstructorBuilderVerifier();

            var classAttributeList = constructorDeclaration.AttributeLists.SingleOrDefault();
            validator.VerifyClassAttribute(classAttributeList, expectedAttributeName: attributeName);
        }
    }
}