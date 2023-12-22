namespace ValueObjects.Experimental.BaseClassNet7;

/// <summary>
/// Implementation of https://en.wikipedia.org/wiki/Port_(computer_networking)
/// </summary>
public sealed class PortNumber
	: PositiveIntegerBase<PortNumber, uint>
 {
	public PortNumber(uint number) : base(number) {
	}
}
