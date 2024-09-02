# XmobiTea.Bean

**XmobiTea.Bean** is a powerful library designed to simplify the management of singleton objects and automate component binding in .NET applications. With its easy-to-use API, XmobiTea.Bean provides a streamlined approach to handling dependency injection, object initialization, and automatic binding.

## Key Features

### 1. Singleton Management
- **Create, Manage, Retrieve**: XmobiTea.Bean offers mechanisms to create, manage, and retrieve singleton objects across your application. This ensures that you have a consistent and easy way to work with shared instances.

### 2. Automatic Binding
- **AutoBindAttribute**: Using the `AutoBindAttribute`, properties and fields within your objects can be automatically bound without the need for manual coding, reducing boilerplate and potential errors.

### 3. Support for Binding Stages
- **Binding Lifecycle Interfaces**: XmobiTea.Bean includes support for different stages of the binding process:
  - `IBeforeAutoBind`: Perform actions before the automatic binding starts.
  - `IAfterAutoBind`: Execute logic after the binding is complete.
  - `IFinalAutoBind`: Finalize the binding process with any additional steps.

### 4. Class Scanning and Binding
- **Flexible Class Scanning**: XmobiTea.Bean allows you to scan classes within a specified namespace or assembly to identify classes that need to be bound, either by custom attributes or by inheritance.

### 5. Integration with Dependency Injection
- **Dependency Injection (DI) Friendly**: The library�s features integrate seamlessly with DI frameworks, making it easier to configure and manage dependencies among components in your application.

## Installation

To install XmobiTea.Bean, use NuGet Package Manager:

```bash
Install-Package XmobiTea.Bean
