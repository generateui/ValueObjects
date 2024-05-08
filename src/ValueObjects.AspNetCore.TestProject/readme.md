# ValueObjects.AspNetCore.TestProject

A test project to demonstrate how ValueObjects work when used with an ASP.NET Core page model.

In general, since the ValueObject implementations support `IParseable<TSelf>` and have overridden `.ToString()` implementations, the ASP.NET Core engine is able to perform a roundtrip serialization to string and back to the object.
