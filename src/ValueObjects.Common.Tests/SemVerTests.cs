using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace ValueObjects.Common.Tests;

public sealed class SemVerTests
{
	[Theory]
	[InlineData("0.0.1", 0, 0, 1)]
	[InlineData("10.11.12", 10, 11, 12)]
	[InlineData("10.1.2", 10, 1, 2)]
	[InlineData("1.10.2", 1, 10, 2)]
	[InlineData("1.1.12", 1, 1, 12)]
	[InlineData("1.10.12", 1, 10, 12)]
	[InlineData("0.0.1-alpha1", 0, 0, 1, "alpha1")]
	[InlineData("0.0.1-1", 0, 0, 1, "1")]
	[InlineData("0.0.1--", 0, 0, 1, "-")]
	[InlineData("0.0.1-+", 0, 0, 1, "", "")]
	[InlineData("0.0.1+alpha2", 0, 0, 1, null, "alpha2")]
	[InlineData("0.0.1+2", 0, 0, 1, null, "2")]
	[InlineData("0.0.1+alpha", 0, 0, 1, null, "alpha")]
	[InlineData("0.0.1-alpha3+beta", 0, 0, 1, "alpha3", "beta")]
	[InlineData("0.0.1-alpha+beta4", 0, 0, 1, "alpha", "beta4")]
	[InlineData("0.0.1-alpha5+beta6", 0, 0, 1, "alpha5", "beta6")]
	[InlineData("0.0.1-", 0, 0, 1, "")]
	[InlineData("0.0.1+", 0, 0, 1, null, "")]
	public void Given_correct_semver_When_parsed_Then_correctly_parsed(
		string version,
		int major,
		int minor,
		int patch,
		string? preRelease = null,
		string? build = null)
	{
		bool parses = SemVer.TryParse(version, null, out var semVer);
		parses.Should().BeTrue();
		semVer.Should().NotBeNull();
		AssertNotNull(semVer);
		semVer.Major.Should().Be(major);
		semVer.Minor.Should().Be(minor);
		semVer.Patch.Should().Be(patch);
		semVer.PreRelease.Should().Be(preRelease);
		semVer.Build.Should().Be(build);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("0.")]
	[InlineData("0.0")]
	[InlineData("0.0.")]
	[InlineData("0.01")]
	public void Given_incorrect_semver_When_parsed_Then_not_parsed(string version)
	{
		bool parses = SemVer.TryParse(version, null, out var semVer);
		parses.Should().BeFalse();
	}

#nullable disable warnings
	private static void AssertNotNull<T>([NotNull] T value)
	{
		System.Diagnostics.Trace.Assert(value is { });
	}
#nullable enable warnings
}
