# ValueObjects
A set of types to represent commonly used values as typed objects, instead of primitive ints, strings and bools.

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

## State of repo
Currently mainly implementing many ways on achieving ValueObjects and learning the advantages and disadvantages of each approach for the different usecases.