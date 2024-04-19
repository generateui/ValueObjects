using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace ValueObjects.Common;

/// <summary>
/// Implements a data URI.
/// </summary>
/// <see href="https://datatracker.ietf.org/doc/html/rfc2397"/>
/// <see href="https://en.wikipedia.org/wiki/Data_URI_scheme"/>
/// <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URLs"/>
public sealed class DataUri : IParsable<DataUri>, IEquatable<DataUri>
{
	private static readonly Dictionary<string, string> noParameters = new();

	public DataUri(
		string? mediaType,
		Dictionary<string, string>? mediaTypeParameters,
		bool base64,
		string? data)
	{
		MediaType = mediaType;
		MediaTypeParameters = mediaTypeParameters is null
			? noParameters
			: new Dictionary<string, string>(
				mediaTypeParameters,
				StringComparer.OrdinalIgnoreCase);
		Base64 = base64;
		Data = data ?? string.Empty;
	}

	private enum State {
		Scheme,
		MediaType,
		MediaTypeParameterKey,
		MediaTypeParameterValue,
		Base64,
		Data,
	}

	public string? MediaType { get; }

	/// <summary>
	/// Case-insensitive key and value pair comparison.
	/// </summary>
	public IReadOnlyDictionary<string, string> MediaTypeParameters { get; }

	public bool Base64 { get; }

	/// <summary>
	/// When <see cref="Base64"/> is true, returns base64 encoded data,
	/// otherwise non-encoded data.
	/// The data is url-decoded using
	/// <see cref="HttpUtility.UrlDecode(string?)"/>.
	/// </summary>
	public string Data { get; }

	/// <summary>
	/// Parsed target string into a data uri.
	/// </summary>
	/// <param name="s">string to parse</param>
	/// <param name="formatProvider">Currently has no effect, pas in null</param>
	/// <param name="result">DataUri instance when parsing succeeded</param>
	/// <returns>true when parsing succeeded, false when parsing failed.</returns>
	/// <remarks>
	/// This method signature satisfies the .NET 7.0 IParseable<TSelf>
	/// interface <see href="https://learn.microsoft.com/en-us/dotnet/api/system.iparsable-1"/>.
	/// </remarks>
	public static bool TryParse(
		string? s,
		IFormatProvider? _,
		[NotNullWhen(true)] out DataUri? result)
	{
		result = null;
		if (s is null) {
			return false;
		}

		var state = State.Scheme;
		string scheme = string.Empty;
		string mediaType = string.Empty;
		string mediaTypeParameterKey = string.Empty;
		string mediaTypeParameterValue = string.Empty;
		Dictionary<string, string> mediaTypeParameters =
			new(StringComparer.OrdinalIgnoreCase);
		bool base64 = false;
		string? data = null;

		for (int i = 0; i < s.Length; i++) {
			char c = s[i];
			switch (state) {
				case State.Scheme:
					if (c == ':') {
						state = State.MediaType;
						continue;
					}
					else {
						scheme += c;
						continue;
					}

				case State.MediaType:
					if (c == ';') {
						state = State.MediaTypeParameterKey;
						continue;
					}
					else if (c == ',') {
						state = State.Data;
						continue;
					}
					else {
						mediaType += c;
						continue;
					}

				case State.MediaTypeParameterKey:
					if (c == '=') {
						state = State.MediaTypeParameterValue;
						continue;
					}
					else if (c == ',' && mediaTypeParameterKey == "base64") {
						state = State.Data;
						base64 = true;
						continue;
					}
					else {
						mediaTypeParameterKey += c;
						continue;
					}

				case State.MediaTypeParameterValue:
					if (c == ';') {
						mediaTypeParameters.Add(
							mediaTypeParameterKey,
							mediaTypeParameterValue);
						mediaTypeParameterKey = string.Empty;
						mediaTypeParameterValue = string.Empty;
						state = State.MediaTypeParameterKey;
						continue;
					}
					else if (c == ',') {
						state = State.Data;
						mediaTypeParameters.Add(
							mediaTypeParameterKey,
							mediaTypeParameterValue);
						continue;
					}
					else {
						mediaTypeParameterValue += c;
						continue;
					}

				case State.Data:
					data = s[i..];
					goto done;

				default:
					return false;
			}
		}

		if (scheme != "data" || state != State.Data) {
			return false;
		}

		done:
		result = new DataUri(
			mediaType == string.Empty ? null : mediaType,
			mediaTypeParameters.Count == 0 ? null : mediaTypeParameters,
			base64,
			HttpUtility.UrlDecode(data));
		return true;
	}

	public static DataUri Parse(string s, IFormatProvider? provider)
	{
		if (TryParse(s, provider, out var result)) {
			return result;
		}

		throw new ArgumentException("Argument was not in the specified format");
	}

	public bool Equals(DataUri? other)
	{
		if (other is null) {
			return false;
		}
		return
			Equals(MediaType, other.MediaType) &&
			Equals(MediaTypeParameters, other.MediaTypeParameters) &&
			Equals(Base64, other.Base64) &&
			Equals(Data, other.Data);
	}

	public override bool Equals(object? obj) => Equals(obj as DataUri);

	public override int GetHashCode()
	{
		unchecked {
			int hash = (int)2166136261;
			hash = hash * 16777619 + (MediaType is null ? 0 : MediaType.GetHashCode());
			hash = hash * 16777619 + (MediaTypeParameters is null ? 0 : MediaTypeParameters.GetHashCode());
			hash = hash * 16777619 + Base64.GetHashCode();
			hash = hash * 16777619 + (Data is null ? 0 : Data.GetHashCode());
			return hash;
		}
	}

	public override string ToString() {
		string mediaTypeParameters;
		if (MediaTypeParameters is null) {
			mediaTypeParameters = "";
		} else {
			var values = MediaTypeParameters.Select(p => $"{p.Key}={p.Value}");
			mediaTypeParameters = string.Join(';', values);
		}
		string base64 = Base64 ? "base64" : string.Empty;
		string semicolon;
		if (!string.IsNullOrEmpty(base64) || !string.IsNullOrEmpty(mediaTypeParameters)) {
			semicolon = ";";
		} else {
			semicolon = string.Empty;
		}
		string mediaType = MediaType is null ? "" : $"{MediaType}";
		string data = HttpUtility.UrlEncode(Data);
		return $"data:{mediaType}{semicolon}{mediaTypeParameters}{base64},{data}";
	}
}
