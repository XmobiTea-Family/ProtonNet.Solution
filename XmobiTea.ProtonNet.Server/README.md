# XmobiTea.ProtonNet.Server

## Installation
To install the XmobiTea.ProtonNet.Server package, you can use the following command:

```
dotnet add package XmobiTea.ProtonNet.Server
```

## Features
- **User Peer Management**: Manage user peers, including mapping, removing, and retrieving user peers based on session IDs or user IDs.
- **Session Management**: Handle session-related operations such as mapping, removing, and retrieving sessions.
- **Event Handling**: Manage and handle events with support for custom event handlers.
- **Request Handling**: Process and handle operation requests with support for custom request handlers.
- **RPC Protocol**: Support for RPC protocol operations, including serialization, deserialization, and encryption.
- **Authentication Token Management**: Generate and verify authentication tokens for user peers.

## Usage
Here is an example of how to use the `XmobiTea.ProtonNet.Server` package:

```csharp
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Models;

// Initialize services
var userPeerService = new UserPeerService();
var sessionService = new SessionService();
var eventService = new EventService();
var requestService = new RequestService();
var rpcProtocolService = new RpcProtocolService();
var userPeerAuthTokenService = new UserPeerAuthTokenService();

// Map a user peer to a session ID
userPeerService.MapUserPeer("sessionId123", new UserPeer());

// Retrieve a user peer
var userPeer = userPeerService.GetUserPeer("sessionId123");

// Handle an event
eventService.Handle(new OperationEvent(), new SendParameters(), userPeer, new Session());

// Generate an authentication token
var token = userPeerAuthTokenService.GenerateToken(new UserPeerTokenPayload { UserId = "user123" });

// Verify an authentication token
if (userPeerAuthTokenService.TryVerifyToken(token, out var header, out var payload))
{
    Console.WriteLine($"Token verified for user: {payload.UserId}");
}
```

## Supported Data Types
- `IUserPeer`: Interface for managing user peers.
- `ISession`: Interface for session management.
- `IEventHandler`: Interface for handling events.
- `IRequestHandler`: Interface for handling requests.
- `IAuthToken`: Interface for authentication token management.

## Extensibility
You can extend the functionalities by implementing your own versions of the provided interfaces such as `IUserPeerService`, `ISessionService`, `IEventService`, and `IRequestService`.

## Contributing
Contributions are welcome! Please fork this repository and submit a pull request if you'd like to contribute to the project.

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments
- Thanks to the developers of XmobiTea for providing the foundational libraries that this package builds upon.
