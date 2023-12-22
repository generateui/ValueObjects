namespace ValueObjects.Experimental.BaseClassNet7;

public sealed class UnparsedJson : StringBase<UnparsedJson> {
	public UnparsedJson(string value) : base(value) {
	}

	public static implicit operator UnparsedJson(string value) => new(value);
}