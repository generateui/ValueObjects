using System.Diagnostics.CodeAnalysis;

namespace ValueObjects.Common.Tests;

internal static class TestHelper
{
#nullable disable warnings
	internal static void AssertNotNull<T>([NotNull] T value)
	{
		System.Diagnostics.Trace.Assert(value is { });
	}
#nullable enable warnings
}