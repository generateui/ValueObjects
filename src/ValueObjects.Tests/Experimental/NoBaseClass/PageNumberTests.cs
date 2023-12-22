using FluentAssertions;

namespace ValueObjects.Experimental.NoBaseClass.Tests;

public class PageNumberTests {
	[Theory]
	[InlineData(int.MinValue)]
	[InlineData(-1)]
	[InlineData(0)]
	public void When_ctor_int_called_with_out_of_bounds_value_Then_ArgumentException(int value) {
		// Act
		Action createNew = () => {
			_ = new PageNumber(value);
		};

		// Assert
		createNew.Should().Throw<ArgumentException>();
	}
}