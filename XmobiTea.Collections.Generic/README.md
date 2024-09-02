# XmobiTea.Collections.Generic

**XmobiTea.Collections.Generic** is a .NET library that provides a set of enhanced and efficient generic collection types. These collections are designed to extend the capabilities of standard .NET collections, offering improved performance, additional features, and specialized data structures for various use cases.

## Key Features

### 1. Enhanced Collection Types
- **Thread-Safe Collections**: Provides thread-safe versions of commonly used collections like dictionaries and lists, allowing safe concurrent access in multi-threaded environments.
- **Custom Collection Implementations**: Offers specialized collections that are optimized for specific scenarios, such as fast lookups or memory-efficient storage.

### 2. High-Performance Operations
- **Optimized Algorithms**: Includes collections with algorithms that are fine-tuned for performance, reducing the time complexity of common operations like searching, sorting, and adding/removing items.
- **Memory Efficiency**: Collections are designed with memory optimization in mind, helping to minimize the memory footprint of large datasets.

### 3. Flexible APIs
- **Extended Functionality**: The library extends the standard .NET collection APIs, adding methods and properties that offer greater flexibility and control over data management.
- **Compatibility**: Works seamlessly with existing .NET collections and LINQ, allowing easy integration into existing projects.

### 4. Specialized Data Structures
- **Priority Queues**: Includes efficient implementations of priority queues for scenarios that require prioritized processing of items.
- **Circular Buffers**: Offers circular buffers that are ideal for scenarios where a fixed-size buffer is required with efficient use of memory.

### 5. Extensibility
- **Customizable Collections**: Developers can extend the provided collections or create their own by inheriting from base classes, allowing for custom behaviors and optimizations.

## Installation

To install XmobiTea.Collections.Generic, use NuGet Package Manager:
Install-Package XmobiTea.Collections.Generic

## Usage

### Example: Thread-Safe Dictionary
Here's how to use the thread-safe dictionary from XmobiTea.Collections.Generic:

```csharp
using XmobiTea.Collections.Generic;

var threadSafeDict = new ThreadSafeDictionary<int, string>();
threadSafeDict.Add(1, "Value1");
if (threadSafeDict.TryGetValue(1, out var value))
{
    Console.WriteLine(value); // Output: Value1
}
```
