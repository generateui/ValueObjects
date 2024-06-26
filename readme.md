# ValueObjects
A set of types to represent commonly used values as typed objects, instead of primitive ints, strings and bools.

A `ValueObject`:
- is immutable
- may be parsed from a string
- may be represented by a string
- does not have identity itself, but may represent an identity
- may be interned
- are compared by value
- may reference other `ValueObjects`

A generic ValueObject is a `ValueObject` that represents something common in many domains/applications, such as a Guid, DataUri or TimeSpan. A specific `ValueObject` is a `ValueObject` that is specific to an application or domain. Your music app may have a SongId, which is only ever used within that music app.

`ValueObjects.Common` contains generic `ValueObjects` that are useful in any app, domain or context.

## Benefits
- more specifically typed values, so bugs are spotted sooner
- compiler is used to spot errors sooner
- better readability of the code
- represent the intent of values with 100% accuracy
- prevent bugs by building on top of typed objects instead of primitive values
- reduce uncessary code duplication by having one place to parse string, generate strings
- ability to map types directly to
	- Dapper queries
	- ASP.NET controller methods
	- `System.Text.Json` converters
	- `System.ComponentModel.TypeConverter` converters

## Structure
- [ValueObjects.Common](./src/ValueObjects.Common/Readme.md) contains generic types useful in many business domains. These are handcrafted implementations.

## State of repo
Currently mainly implementing many ways on achieving ValueObjects and learning the advantages and disadvantages of each approach for the different usecases.

## Further reading / links
- https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
- https://mmapped.blog/posts/25-domain-types.html
- https://andrewlock.net/using-strongly-typed-entity-ids-to-avoid-primitive-obsession-part-1/
- https://andrewlock.net/strongly-typed-id-updates/
- https://github.com/mcintyre321/ValueOf
- https://www.youtube.com/watch?v=h4uldNA1JUE
- https://siepman.nl/blog/tiny-types-wrapping-primitive-types
- https://martinfowler.com/bliki/ValueObject.html
 