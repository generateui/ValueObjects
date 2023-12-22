namespace ValueObjects.Experimental.BaseClassNet7;

public abstract class StringBase<TDerived> :
	IEquatable<TDerived>,
	IComparable<TDerived>
	where TDerived : StringBase<TDerived>
{
	public StringBase(string value) {
		Value = value;
	}

	public string Value { get; }

	public int CompareTo(TDerived? other) {
		if (other is TDerived derived) {
			return string.Compare(Value, derived.Value);
		}
		return 0;
	}

	public bool Equals(TDerived? other) {
		if (other is TDerived derived) {
			return string.Equals(Value, derived.Value);
		}
		return false;
	}
}
