# LQueue Implementation

A Queue<T> implementation using List<T> as the underlying storage mechanism.

## Requirements

- .NET 8.0 SDK

## Running Tests

```bash
dotnet test
```

Or if you want more detailed output, which I reccomend

```bash
dotnet test --logger "console;verbosity=detailed"
```

## Project Structure

- `LQueueLib/` - Contains the LQueue<T> implementation
- `LQueueTests/` - Contains unit tests 