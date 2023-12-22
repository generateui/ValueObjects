using System.Numerics;

namespace ValueObjects.Experimental.BaseClassNet7;

public abstract class PositiveIntegerBase<TDerived, TNumber> :
	IEquatable<TDerived>,
	IComparable<TDerived>
	where TDerived : PositiveIntegerBase<TDerived, TNumber>
	where TNumber :
		INumber<TNumber>,
		IUnsignedNumber<TNumber>
{
	public PositiveIntegerBase(TNumber number) {
		// if (number > IUnsignedNumber<TNumber>.) {
		// 	throw new ArgumentException(
		// 		$"A {GetType().Name} must have a value of at least 1.", nameof(number));
		// }

		Value = number;
	}

	public TNumber Value { get; }

	public int CompareTo(TDerived? other) {
		if (other is null)
		{
			return 1;
		}
		return Value.CompareTo(other.Value);
	}

	public override bool Equals(object? obj) {
		if (obj is not TDerived portNumber)
		{
			return false;
		}
		return Equals(Value, portNumber.Value);
	}

	public bool Equals(TDerived? other) {
		if (other is null)
		{
			return false;
		}
		return Equals(Value, other.Value);
	}

	public override int GetHashCode() => Value.GetHashCode();

	public override string? ToString() => Value.ToString();
}
