# Queue Implementations

two queue implementations for comparison:

## LQueue
a generic queue implementation using list<t> as the underlying storage mechanism.
provides o(1) enqueue (amortized), o(n) dequeue, o(1) peek, and o(n) contains operations.

## AQueue  
a generic queue implementation using fixed size array as the storage mechanism.
provides o(1) enqueue, o(1) dequeue, o(1) peek, and o(n) contains operations.

## requirements

- .NET 8.0 SDK

## running tests

```bash
dotnet test
```

or for detailed output:

```bash
dotnet test --logger "console;verbosity=detailed"
```

## project structure

- `LQueueLib/` - LQueue<T> implementation
- `LQueueTests/` - LQueue unit tests
- `AQueueLib/` - AQueue<T> implementation  
- `AQueueTests/` - AQueue unit tests 