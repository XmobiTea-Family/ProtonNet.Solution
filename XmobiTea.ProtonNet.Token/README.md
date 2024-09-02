
# XmobiTea.ProtonNet.Token

## Installation
To install XmobiTea.ProtonNet.Token, clone the repository and include the project in your solution. You can also add it via a package manager if available.

## Features
- **Binary Token Serialization/Deserialization**: Provides interfaces and implementations for encoding and decoding tokens in binary format.
- **Attribute-Based Token Configuration**: Allows properties in token classes to be marked with attributes to define their binary representation.
- **Support for Multiple Hash Algorithms**: Includes implementations for various hashing algorithms like MD5, SHA1, SHA256, SHA384, SHA512, etc., to encode tokens securely.

## Usage
### Example 1: Basic Token Encoding and Decoding
```csharp
var encoder = new SHA256TokenAlgorithmEncode();
var token = encoder.Encrypt(data);

var decoder = new ProtocolTokenBinaryDecode(new BinaryConverter());
var deserializedData = decoder.DeserializePayload(token);
```

### Example 2: Attribute-Based Token Member
```csharp
public class ExampleToken
{
    [TokenMember(0x01)]
    public int Id { get; set; }

    [TokenMember(0x02)]
    public string Name { get; set; }
}
```

## Supported Data Types
- Byte arrays for encoding and decoding.
- Object arrays and dictionaries for structured data.
- Custom attributes for property-based token configuration.

## Extensibility
XmobiTea.ProtonNet.Token is designed with extensibility in mind. You can implement custom encoding/decoding algorithms by extending the `ITokenAlgorithmEncode` and `ITokenBinaryDecode` interfaces. You can also define custom attributes to enhance the token structure.
