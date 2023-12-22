using FluentAssertions;
using FluentAssertions.Execution;

namespace ValueObjects.Experimental.NoBaseClass.Tests;

public class NonBaseClassesMustBeSealedTests {
	[Fact]
	public void Given_all_classes_When_not_baseclass_Then_it_must_be_sealed() {
		// Arrange 
		var classes = typeof(ForTesting).Assembly.GetTypes()
			.Where(t => t.IsClass);
		using AssertionScope assertionScope = new();

		// Assert
		foreach (var clazz in classes) {
			// generic parameters will result in ClassName`{GenericParameterCount}
			string nameWithoutGenericParameterSuffix = clazz.Name.Split('`')[0];
			if (nameWithoutGenericParameterSuffix.EndsWith("Base")) {
				continue;
			}
			clazz.IsSealed.Should().BeTrue(clazz.FullName);
		}
	}
}
