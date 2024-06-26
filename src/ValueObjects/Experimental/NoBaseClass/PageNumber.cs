namespace ValueObjects.Experimental.NoBaseClass;

/// <summary>
/// Implementation of https://en.wikipedia.org/wiki/Page_numbering
/// </summary>
public sealed class PageNumber :
	IEquatable<PageNumber>,
	IComparable<PageNumber> {
	public PageNumber(int number) {
		if (number < 1 ) {
			throw new ArgumentException(
				$"A {nameof(PageNumber)} must have a value of at least 1.", nameof(number));
		}

		Value = number;
	}

	public int Value { get; }

	public int CompareTo(PageNumber? other) {
		if (other is null) {
			return 1;
		}
		return Value.CompareTo(other.Value);
	}

	public override bool Equals(object? obj) {
		if (obj is not PageNumber pageNumber) {
			return false;
		}
		return Equals(Value, pageNumber.Value);
	}

	public bool Equals(PageNumber? other) {
		if (other is null) {
			return false;
		}
		return Equals(Value, other.Value);
	}

	public override int GetHashCode() => Value.GetHashCode();

	public override string ToString() => Value.ToString();
}
