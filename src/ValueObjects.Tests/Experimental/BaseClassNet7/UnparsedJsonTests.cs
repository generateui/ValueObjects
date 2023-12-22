using System.Text.Json;

namespace ValueObjects.Experimental.BaseClassNet7;

public class UnparsedJsonTests {
	[Fact]
	public void Example_usecase() {
		UnparsedJson json = """{ "property": "value" }""";
		TestObject? testObject = DoSomethingLikeParse(json);
	}

	private TestObject? DoSomethingLikeParse(UnparsedJson json) {
		TestObject? parsed = JsonSerializer.Deserialize<TestObject>(json.Value);
		return parsed;
	}

	private sealed class TestObject {
		public string Property { get; init; }
	}
}