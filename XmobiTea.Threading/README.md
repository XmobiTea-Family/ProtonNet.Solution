
# XmobiTea.Threading

## Installation Instructions

1. Clone the repository: `git clone <repository-url>`
2. Navigate to the project directory: `cd XmobiTea.Threading`
3. Install dependencies using your preferred package manager.

## Features

- Provides scheduling and queuing mechanisms using fiber-based threading.
- Supports single-task and interval-based task scheduling.
- Includes performance monitoring and statistics tracking.

## Usage Examples

### Creating and Starting a Fiber

```csharp
var fiber = new RoundRobinFiber("FiberName", 4);
fiber.Start();
```

### Enqueuing a Task

```csharp
fiber.Enqueue(() => Console.WriteLine("Task executed"));
```

### Scheduling a Task

```csharp
fiber.Schedule(() => Console.WriteLine("Scheduled task executed"), 1000);
```

### Scheduling a Task on Interval

```csharp
fiber.ScheduleOnInterval(() => Console.WriteLine("Interval task executed"), 1000, 5000);
```

## Supported Data Types

- `Action` for task definitions.
- `ISingleTask`, `IScheduleTask`, `IScheduleOnIntervalTask` for task interfaces.

## Extensibility

- Implement custom task types by extending `SingleTask` or other task classes.
- Integrate with custom statistics counters by implementing `IStatisticsCounter`.

## Contributing Guidelines

1. Fork the repository and create a new branch for your changes.
2. Ensure your code adheres to the project's coding standards.
3. Submit a pull request with a detailed description of your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Thanks to the contributors and community for their support and feedback.
