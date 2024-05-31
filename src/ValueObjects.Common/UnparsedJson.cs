namespace ValueObjects.Common;

/// <summary>
/// Marks a target string instance as a json value that is not (yet?) parsed.
/// </summary>
public class UnparsedJson
	: IEquatable<UnparsedJson>
{
	private readonly string json;

	public UnparsedJson(string json)
	{
		this.json = json;
	}

	public static implicit operator string(UnparsedJson unparsedJson)
	{
		return unparsedJson.json;
	}

	public override string ToString() => json;

	public override bool Equals(object? obj)
	{
		if (obj is UnparsedJson unparsedJson) {
			return Equals(unparsedJson);
		}

		return false;
	}

	public override int GetHashCode() => json.GetHashCode();

	/// <summary>
	/// Returns true when <paramref name="other"/> is equal to this. Since the
	/// content of the json is intentionally not (yet?) parsed, comparison can
	/// not happen in a cultural sensitive way. Therefore
	/// <see cref="StringComparison.Ordinal"/> is used for comparison.
	/// </summary>
	public bool Equals(UnparsedJson? other)
	{
		if (other is null) {
			return false;
		}

		return string.Equals(json, other.json, StringComparison.Ordinal);
	}
}