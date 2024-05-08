# ValueObject.Common
Contains commonly used value objects that have a handcrafted implementation.

## Design non-goals
- don't provide nor derive from a base `ValueObject` class, derive from `Object` instead for better portability
- don't provide nor implement a base interface like `IValueObject` for improved portability
- don't provide implementations for unit of measurement (length, mass, time, etc) or quantities
- don't provide implementations for money
- don't provide implementations for personal objects (gender, name)

## Implementations
- [SemVer](./SemVer.cs)
- [DataUri](./DataUri.cs)

## .NET's built-in value objects
- `System`
	- [`Guid`](https://learn.microsoft.com/en-us/dotnet/api/system.guid)
	- [`DateOnly`](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly)
	- [`TimeOnly`](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly)
	- [`DateTime`](https://learn.microsoft.com/en-us/dotnet/api/system.datetime)
	- [`DateTimeOffset`](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset)
	- [`Uri`](https://learn.microsoft.com/en-us/dotnet/api/system.uri)
	- [`Version`](https://learn.microsoft.com/en-us/dotnet/api/system.version)
	- [`Index`](https://learn.microsoft.com/en-us/dotnet/api/system.index)
	- [`TimeSpan`](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)
	- [`TimeZoneInfo`](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo)
	- [`CultureInfo`](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)
- `System.Net`
	- [`IPAddress`](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipaddress)

## Common interfaces implemented
This is a list of common interfaces implemented for generic value objects.
 - `IParseable<T>`
 - `IEquatable<T>`
 - `IComparable`
 - `IComparable<T>`