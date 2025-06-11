# iStringExtensions

A C# library that provides a collection of useful string extension methods for common string manipulation and validation tasks. This library consolidates various string utility methods used across different projects into a single, reusable package.

## Installation

You can install iStringExtensions via NuGet:

```bash
Install-Package iStringExtensions
```

Or using the .NET CLI:

```bash
dotnet add package iStringExtensions
```

## Features

### String Validation
- `IsInteger()`: Check if a string represents a valid integer
- `IsNumber()`: Check if a string represents a valid number (supports decimal numbers)
- `IsJson()`: Validate if a string is valid JSON
- `IsEmail()`: Validate email address format
- `IsIpV4()`: Validate IPv4 address format

### String Transformation
- `WrapWithAList()`: Convert string to List<string>
- `WrapWithAnArray()`: Convert string to string array
- `ConsolidateSpaces()`: Replace multiple consecutive spaces with a single space
- `ToMemoryStream()`: Convert string to MemoryStream

### String Parsing
- `SplitAsEnumerableOf<T>()`: Split string into enumerable of any primitive type
- `QueryStringToDictionary()`: Parse query string into dictionary
- `RegexMatch()`: Perform regex matching
- `RegexExtract()`: Extract matches using regex

### String Hashing
- `GetMd5Hash()`: Generate MD5 hash of a string
- `GetSha1Hash()`: Generate SHA1 hash of a string

### JSON Handling
- `DeserializeJson<T>()`: Deserialize JSON strings into objects

## Usage Examples

```csharp
using iStringExtensions;

// Validation examples
bool isInteger = "123".IsInteger(); // true
bool isNumber = "123.45".IsNumber(); // true
bool isValidJson = "{\"name\":\"test\"}".IsJson(); // true

// Transformation examples
List<string> list = "test".WrapWithAList();
string[] array = "test".WrapWithAnArray();

// Parsing examples
var numbers = "1,2,3".SplitAsEnumerableOf<int>(",");
var dictionary = "key1=value1;key2=value2".QueryStringToDictionary();

// Hashing examples
string md5 = "test".GetMd5Hash();
string sha1 = "test".GetSha1Hash();
```

## Error Handling

The library includes robust error handling with a custom `StringExtensionsException` class that is thrown when validation fails or invalid operations are attempted.

## Testing

The library includes comprehensive unit tests in the `UnitTests` project using xUnit and FluentAssertions.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
