using FluentAssertions;

namespace ValueObjects.Experimental.NoBaseClass.Tests;

public class PortNumberTests {
	[Theory]
	[InlineData(65536)]
	[InlineData(int.MaxValue)]
	[InlineData(int.MinValue)]
	[InlineData(-1)]
	public void When_ctor_int_called_with_out_of_bounds_value_Then_ArgumentException(int value) {
		// Act
		Action createNew = () => {
			_ = new PortNumber(value);
		};

		// Assert
		createNew.Should().Throw<ArgumentException>();
	}
}