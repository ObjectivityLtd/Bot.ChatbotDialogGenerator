namespace RoslynWrapper.Tests.SyntaxBuilders.Namespaces
{
    using System.Linq;
    using FluentAssertions;
    using RoslynWrapper.SyntaxBuilders.Namespaces;
    using Xunit;

    public class NamespaceComparerTests
    {
        [Fact]
        public void Should_Order_Alphabetically_If_There_Are_No_System_Namespaces()
        {
            // arrange
            var usings = new[]
            {
                "Different.Hot.Space",
                "Some.Namespace",
                "Different.Namespace",
                "Another.Namespace"
            };

            // act
            var orderedList = usings.OrderBy(s => s, new NamespaceComparer()).ToArray();

            //assert
            var expectedUsings = new[]
            {
                "Another.Namespace",
                "Different.Hot.Space",
                "Different.Namespace",
                "Some.Namespace"
            };

            orderedList.Should().BeEquivalentTo(expectedUsings);
        }

        [Fact]
        public void Should_Order_System_Usings_First()
        {
            // arrange
            var usings = new[]
            {
                "Different.Hot.Space",
                "Some.Namespace",
                "Different.Namespace",
                "System.Namespace",
                "System.Another.Namespace"
            };

            // act
            var orderedList = usings.OrderBy(s => s, new NamespaceComparer()).ToArray();

            //assert
            var expectedUsings = new[]
            {
                "System.Another.Namespace",
                "System.Namespace",
                "Different.Hot.Space",
                "Different.Namespace",
                "Some.Namespace"
            };

            orderedList.Should().BeEquivalentTo(expectedUsings);
        }
    }
}