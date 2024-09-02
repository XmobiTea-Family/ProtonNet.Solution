# XmobiTea.Binary

XmobiTea.Binary is a high-performance binary serialization and deserialization library for .NET applications. It offers a flexible and efficient way to convert objects to binary data and vice versa, making it ideal for scenarios where performance and data size are critical.

## Installation

To install XmobiTea.Binary via NuGet, run the following command:

    dotnet add package XmobiTea.Binary

## Features

- High Performance: Optimized for fast serialization and deserialization of objects.
- Flexible API: Simple and intuitive API for converting objects and collections.
- Custom Type Support: Easily extend the library to support custom types and structures.
- Integration: Seamlessly integrates with other XmobiTea libraries for enhanced functionality.

## Usage

### Serialization

You can serialize an object to binary data using the DataConverter:

    var myObject = new MyCustomObject();
    var binaryData = DataConverter.SerializeObject(myObject);

### Deserialization

To deserialize binary data back into an object:

    var binaryData = /* your binary data */;
    var deserializedObject = DataConverter.DeserializeObject<MyCustomObject>(binaryData);

### Working with Arrays

Serialization and deserialization of arrays or lists is also straightforward:

    var myArray = new List<MyCustomObject> { new MyCustomObject(), new MyCustomObject() };
    var binaryArray = DataConverter.SerializeArray(myArray);

    var deserializedArray = DataConverter.DeserializeArray<MyCustomObject>(binaryArray);

### Custom Binary Readers and Writers

If you need to handle custom data types, you can create custom binary readers and writers:

    public class MyCustomObjectBinaryReader : BinaryReader<MyCustomObject>
    {
        public override MyCustomObject Read(Stream stream)
        {
            // Custom deserialization logic here
        }
    }

### Extending the Data Converter

For advanced scenarios, you can extend the IDataConverter to implement custom serialization logic:

    public class MyCustomDataConverter : IDataConverter
    {
        // Implement custom serialization and deserialization methods
    }

## Supported Data Types

XmobiTea.Binary supports a wide range of data types, including:

- Primitive types: int, byte, bool, char, etc.
- Collections: IList, IDictionary, etc.
- Custom objects

## Extensibility

The library is designed with extensibility in mind. You can implement your own readers and writers to handle specific binary data formats or custom types not covered by the default implementation.

## Contributing

We welcome contributions to XmobiTea.Binary! If you'd like to contribute, please fork the repository, create a new branch, and submit a pull request.

## License

XmobiTea.Binary is licensed under the Apache License 2.0. For more information, see the LICENSE file.

## Acknowledgments

XmobiTea.Binary is part of the XmobiTea suite of libraries, providing efficient and scalable solutions for modern .NET applications.
