namespace ValueObjects;

/// <summary>
/// Implementation of https://en.wikipedia.org/wiki/Page_numbering
/// </summary>
public sealed class PageNumber {
	public PageNumber(int number) {
		if (number < 1 ) {
			throw new ArgumentException(
				$"A {nameof(PageNumber)} must have a value of at least 1.", nameof(number));
		}

		Value = number;
	}

	public int Value { get; }
}
