namespace ValueObjects;

/// <summary>
/// Implementation of https://en.wikipedia.org/wiki/Port_(computer_networking)
/// </summary>
public sealed class PortNumber {
	public PortNumber(uint number) {
		Value = number;
	}
	public PortNumber(int number) {
		if (number < 0 ) {
			throw new ArgumentException(
				$"A {nameof(PortNumber)} must have a value of at least 0.", nameof(number));
		}

		if (number > 65535) {
			throw new ArgumentException(
				$"A {nameof(PortNumber)} must have a maximum value of 65535.", nameof(number));
		}

		Value = Convert.ToUInt16(number);
	}

	public uint Value { get; }
}
