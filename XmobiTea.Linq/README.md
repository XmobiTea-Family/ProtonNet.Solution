# XmobiTea.Linq

## Installation

To install `XmobiTea.Linq`, you can use the NuGet Package Manager:
Install-Package XmobiTea.Linq

Or use the .NET CLI:
dotnet add package XmobiTea.Linq

## Features

- **Extended LINQ Operations:** Provides additional extension methods to enhance LINQ operations.
- **High Performance:** Optimized design to ensure high performance in data queries.
- **Ease of Use:** User-friendly API, easy to learn, and integrate into existing projects.

## Usage Examples

using XmobiTea.Linq; using System.Collections.Generic; using System.Linq;

var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Use the extension method to filter even numbers var evenNumbers = numbers.FilterEven();

foreach (var number in evenNumbers) { Console.WriteLine(number); // Output: 2, 4 }

## Supported Data Types

- **IEnumerable<T>:** Supports any data type that implements IEnumerable.
- **IQueryable<T>:** Supports LINQ queries for data from databases or similar data sources.

## Extensibility

The `XmobiTea.Linq` library can be extended by adding custom extension methods for various data types.

## Contributing

If you wish to contribute to this project, please open a pull request on GitHub. All contributions are welcome!

## License

`XmobiTea.Linq` is released under the MIT License. See the LICENSE file for more details.

## Acknowledgments

Thanks to the .NET development community for providing the tools and resources to build this library.
