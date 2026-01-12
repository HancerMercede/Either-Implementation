# Either Pattern for C# (.NET 10) 🛡️

A high-performance, type-safe implementation of the **Either Monad** for **.NET 10**. This library leverages the latest features of C# to provide a robust alternative to exceptions for handling business logic and infrastructure failures.

## 🚀 Why Use This in .NET 10?
In modern distributed systems, performance and clarity are key. Using `Either` and `EitherAsync` makes failure states **explicit** in your method signatures, improving maintainability and reducing the overhead caused by exception bubbling in high-throughput applications.

---

## ✨ Core Components

### 1. The Monadic Foundation (Functional Records)
The core is an `abstract record` that acts as a **Discriminated Union**. By using a private constructor, we ensure the hierarchy remains closed and type-safe.



```csharp
namespace Shared;

public abstract record Either<L, R>
{
    private Either() { }

    public sealed record Left(L Value) : Either<L, R>;
    public sealed record Right(R Value) : Either<L, R>;
    
    public static Either<L, R> ToLeft(L value) => new Left(value);
    public static Either<L, R> ToRight(R value) => new Right(value);

    public T Match<T>(Func<L, T> onLeft, Func<R, T> onRight)
        => this switch
        {
            Left l => onLeft(l.Value),
            Right r => onRight(r.Value),
            _ => throw new InvalidOperationException()
        };
}
```

### 2. Async Power: `EitherAsync`

Optimized for asynchronous workflows in .NET 10, this record uses **Lazy Evaluation** to chain operations. It allows for "Railway-Oriented Programming," where logic flows through a success track or short-circuits to a failure track.

#### Features:

* **`Try`**: Safely wraps `Task` operations and maps exceptions to `Left`.
* **`Ensure`**: Fluent validation to pivot from `Right` to `Left` based on a predicate.
* **`FlatMap`**: Asynchronous chaining of multiple monadic operations.

```csharp
public EitherAsync<string, Company> GetCompany(string id)
{
    return EitherAsync<string, Company>.Try(
        async () => await _repo.GetById(id),
        ex => $"Infrastructure Error: {ex.Message}"
    )
    .Ensure(c => c != null, "The company does not exist in the system");
}

```

---

## 💻 Real-World Example

### Clean Controller Integration

The `.Match()` method allows you to resolve the entire async chain into an `IActionResult` cleanly, keeping your endpoints free of boilerplate code.

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> Get(string id)
{
    // Execute the lazy async chain
    var result = await _service.GetCompany(id).Run(); 

    return result.Match<IActionResult>(
        error => BadRequest(new { Message = error }), // Handle Failure
        company => Ok(company)                         // Handle Success
    );
}

```

---

## 📋 API Summary

| Method | Type | Description |
| --- | --- | --- |
| **Try** | Static | Executes a `Task` and catches exceptions into a `Left`. |
| **Ensure** | Extension | Validates a condition; if false, transforms `Right` into `Left`. |
| **Match** | Resolver | Unwraps the final value by providing handlers for both cases. |
| **Map / FlatMap** | Chaining | Transforms or binds asynchronous results seamlessly. |

---

## 🔧 Installation & Requirements

* **Framework:** .NET 10+
* **C# Version:** 14 (Latest features)
