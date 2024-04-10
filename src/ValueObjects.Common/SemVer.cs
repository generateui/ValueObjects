using System.Diagnostics.CodeAnalysis;

namespace ValueObjects.Common;

/// <summary>
/// Implements Semantic version v2.0.0 as specified on
/// <see href="https://semver.org/" />.
/// </summary>
public sealed class SemVer
	: IEquatable<SemVer>, IParsable<SemVer>
{
	public int Major { get; }
	public int Minor { get; }
	public int Patch { get; }
	public string? PreRelease { get; }
	public string? Build { get; }

	public SemVer(int major, int minor, int patch, string? preRelease = null, string? build = null)
	{
		Major = major;
		Minor = minor;
		Patch = patch;
		PreRelease = preRelease;
		Build = build;
	}

	private enum ParseState { Major, Minor, Patch, PreRelease, Build };

	public static bool TryParse(
		[NotNullWhen(true)] string? s,
		IFormatProvider? _,
		[MaybeNullWhen(false)] out SemVer result) {

		result = null;
		if (s is null) {
			return false;
		}

		bool IsPositiveInteger(char c) => c == '1' || c == '2' || c == '3' ||
			c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9';
		bool IsNumericIdentifier(char c) =>
			c == '0' ||
			char.IsLetter(c) ||
			IsPositiveInteger(c);
		bool IsAlphaNumericIdentifier(char c) =>
			char.IsDigit(c) ||
			IsNonDigit(c) ||
			IsIdentifierCharacter(c);
		bool IsIdentifierCharacter(char c) => char.IsDigit(c) || IsNonDigit(c);
		bool IsNonDigit(char c) => char.IsLetter(c) || c == '-';

		bool ParseBuild(string value, ref int index) {
			int start = index;
			int end = value.Length - 1;
			for (; index < value.Length; index++) {
				char c = value[index];
				if ((c == '.' && index == start) ||
					(c == '.' && index == end)) {
					// dot can only be infixed
					return false;
				}
				if (IsAlphaNumericIdentifier(c) ||
					IsNumericIdentifier(c) ||
					c == '.') {
					index += 1;
					continue;
				} else {
					return false;
				}
			}
			return true;
		}

		bool ParsePreRelease(string value, ref int index) {
			int start = index;
			int end = value.Length - 1;
			for (; index < value.Length; index++) {
			// while (index < value.Length) {
				char c = value[index];
				if ((c == '.' && index == start) ||
					(c == '.' && index == end)) {
					// dot can only be infixed
					return false;
				} else if (IsAlphaNumericIdentifier(c) || 
					IsNumericIdentifier(c) ||
					c == '.') {
					continue;
				} else if (c == '+') {
					return true;
				} else {
					return false;
				}
			}
			return true;
		}
		var state = ParseState.Major;
		int majorLength = 0;
		int minorStart = 0;
		int minorLength = 0;
		int patchStart = 0;
		int patchLength = 0;
		int? preReleaseStart = null;
		int? preReleaseLength = null;
		int? buildStart = null;
		int? buildLength = null;
		for	(int i = 0; i < s.Length; i++)
		{
			char c = s[i];
			switch (state) {
				case ParseState.Major:
					if (IsNumericIdentifier(c)) {
						majorLength += 1;
						continue;
					} else if (c == '.') {
						minorStart = i + 1;
						state = ParseState.Minor;
						continue;
					} else {
						return false;
					}
				case ParseState.Minor:
					if (IsNumericIdentifier(c)) {
						minorLength += 1;
						continue;
					} else if (c == '.') {
						state = ParseState.Patch;
						patchStart = i + 1;
						continue;
					} else {
						return false;
					}
				case ParseState.Patch:
					if (IsNumericIdentifier(c)) {
						patchLength += 1;
						continue;
					} else if (c == '-') {
						state = ParseState.PreRelease;
						preReleaseStart = i + 1; // ignore '-'
						continue;
					} else if (c == '+') {
						state = ParseState.Build;
						buildStart = i + 1; // ignore '+'
						continue;
					} else {
						return false;
					}
				case ParseState.PreRelease:
					if (ParsePreRelease(s, ref i)) {
						preReleaseLength = i - preReleaseStart;
						if (i == s.Length) {
							break;
						} else {
							state = ParseState.Build;
							buildStart = i + 1; // ignore '+'
							continue;
						}
					} else {
						return false;
					}
				case ParseState.Build:
					buildStart = i;
					if (ParseBuild(s, ref i)) {
						buildLength = s.Length - buildStart;
						break;
					} else {
						return false;
					}
			}
		}
		if (state == ParseState.Major ||
			state == ParseState.Minor ||
			patchLength == 0) {
			return false;
		}
		int major = int.Parse(s.AsSpan(0, majorLength));
		int minor = int.Parse(s.AsSpan(minorStart, minorLength));
		int patch = int.Parse(s.AsSpan(patchStart, patchLength));
		string? preRelease = null;
		string? build = null;
		if (preReleaseStart is not null && preReleaseLength is null) {
			preReleaseLength = 0;
		}
		if (preReleaseStart is not null && preReleaseLength is not null) {
			preRelease = new(s.AsSpan(preReleaseStart.Value, preReleaseLength.Value));
		}
		if (buildStart is not null && buildLength is null) {
			buildLength = 0;
		}
		if (buildStart is not null && buildLength is not null) {
			build = new(s.AsSpan(buildStart.Value, buildLength.Value));
		}
		result = new SemVer(major, minor, patch, preRelease: preRelease, build: build);
		return true;
	}

	public override int GetHashCode() {
		unchecked {
			int hash = (int)2166136261;
			hash = hash * 16777619 + Major;
			hash = hash * 16777619 + Minor;
			hash = hash * 16777619 + Patch;
			hash = hash * 16777619 + (PreRelease is null ? 0 : PreRelease.GetHashCode());
			hash = hash * 16777619 + (Build is null ? 0 : Build.GetHashCode());
			return hash;
		}
	}

	public override bool Equals(object? obj) {
		if (obj is null) {
			return false;
		}
		if (obj is SemVer semVer) {
			return Equals(semVer);
		}
		return false;
	}

	public bool Equals(SemVer? other) {
		if (other is null) {
			return false;
		}
		return other.Major == Major &&
			other.Minor == Minor &&
			other.Patch == Patch &&
			other.PreRelease == PreRelease &&
			other.Build == Build;
	}

	public static SemVer Parse(string s, IFormatProvider? _) {
		if (TryParse(s, null, out var result)) {
			return result;
		}
		throw new ArgumentException(
			"provided string was not a semver");
	}

}
