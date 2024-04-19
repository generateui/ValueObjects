using FluentAssertions;
using FluentAssertions.Execution;
using static ValueObjects.Common.Tests.TestHelper;

namespace ValueObjects.Common.Tests;


// TODO: resolve the + vs %20 issue described in https://stackoverflow.com/questions/1634271
public sealed class DataUriTests
{
	[Theory]
	[ClassData(typeof(CorrectDataUris))]
	public void Given_correct_data_uri_When_parsed_Then_success(
		string value, DataUri expected)
	{
		// Arrange

		// Act
		bool parsed = DataUri.TryParse(value, null, out var dataUri);

		// Assert
		parsed.Should().BeTrue(value);
		AssertNotNull(dataUri);
		dataUri.MediaType.Should().Be(expected.MediaType, value);
		var scoic = StringComparison.OrdinalIgnoreCase;
		dataUri.MediaTypeParameters.Should()
			.Equal(
				expected.MediaTypeParameters,
				(x, y) =>
					string.Compare(x.Key, y.Key, scoic) == 0 &&
					string.Compare(x.Value, y.Value, scoic) == 0);
		dataUri.Base64.Should().Be(expected.Base64, value);
		dataUri.Data.Should().Be(expected.Data, value);
	}

	[Theory]
	[ClassData(typeof(CorrectDataUris))]
	public void Given_correct_data_uri_When_roundtrip_Then_Equal(
		string value, DataUri _)
	{
		// roundtrip parsing does not completely work for DataUri, since it is
		// a uri. Uris have UrlEncode/Decode which is roundtrip-anbiguous.
		// Arrange

		// Act
		bool parsed = DataUri.TryParse(value, null, out var dataUri);
		AssertNotNull(dataUri);
		string generated = dataUri.ToString();

		// Assert
		generated.Should().BeEquivalentTo(value);
	}

	[Theory]
	[InlineData("d")]
	[InlineData("da")]
	[InlineData("dat")]
	[InlineData("data:")]
	[InlineData("data:m")]
	[InlineData("data:m;")]
	[InlineData("data:a;b=c;")]
	[InlineData("data:a;b=c;base64")]
	[InlineData("data:base64;base64")]
	[InlineData("d:,")]
	[InlineData("da:,")]
	[InlineData("dat:,")]
	public void Given_incorrect_datauri_When_parsed_Then_fail(string value)
	{
		// Act
		bool parsed = DataUri.TryParse(value, null, out var _);

		// Assert
		parsed.Should().BeFalse();
	}

	[Fact]
	public void Given_unequal_datauris_When_tested_for_equality_Then_not_equal()
	{
		var dict1 = new Dictionary<string, string> {
			{ "param1", "value1" },
		};
		var dict2 = new Dictionary<string, string> {
			{ "param2", "value2" },
			{ "param3", "value3" },
		};
		var dataUris = new List<DataUri> {
			new(mediaType: null, mediaTypeParameters: null, base64: false, data: null),
			new("application/json", null, false, null),
			new(null, dict1, false, null),
			new("application/json", dict1, false, null),
			new("application/json", dict1, true, null),
			new("application/json", dict1, true, "d"),
			new("application/json", dict1, true, ""),
			new("application/json", dict1, false, null),
			new("application/json", dict1, false, "d"),
			new("application/json", dict1, false, ""),
			new("application/json", dict2, true, null),
			new("application/json", dict2, true, "d"),
			new("application/json", dict2, true, ""),
			new("application/json", dict2, false, null),
			new("application/json", dict2, false, "d"),
			new("application/json", dict2, false, ""),
		};

		using var assertionScope = new AssertionScope();
		for (var i = 0; i < dataUris.Count; i++) {
			for (var j = 0; i < dataUris.Count; i++) {
				var left = dataUris[i];
				var right = dataUris[j];
				if (i == j) {
					left.Should().BeEquivalentTo(right, left.ToString());
				} else {
					left.Should().NotBeEquivalentTo(right, left.ToString() + " and " + right.ToString());
				}
			}
		}
	}

	public class CorrectDataUris : TheoryData<string, DataUri>
	{
		public CorrectDataUris()
		{
			Add("data:,", new DataUri(null, null, false, null));
			Add("data:,%2C", new DataUri(null, null, false, ","));
			Add("data:,%2C%2C", new DataUri(null, null, false, ",,"));
			Add("data:base64,", new DataUri("base64", null, false, null));
			Add("data:base64;base64,", new DataUri("base64", null, true, null));
			Add("data:text/plain;charset=UTF-8;page=21,the+data%3a1%2C2",
				new DataUri(
					"text/plain",
					new Dictionary<string, string> {
						{ "charset", "UTF-8" },
						{ "page", "21" },
					},
					false,
					"the data:1,2"));
			Add("data:text/plain;charset=utf-8;page=21,the+data%3a1%2C2",
				new DataUri(
					"text/plain",
					new Dictionary<string, string> {
						{ "charset", "UTF-8" }, // note the lowercase
						{ "page", "21" },
					},
					false,
					"the data:1,2"));
			Add("data:mt;a=b;c=d;e=f,+",
				new DataUri(
					"mt",
					new Dictionary<string, string> {
						{ "a", "b" },
						{ "c", "d" },
						{ "e", "f" },
					},
					false,
					" "));
		}
	}
}
