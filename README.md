# Either Pattern Implementation for C# 🛡️

A professional, type-safe implementation of the **Either Monad** for .NET Core / .NET 8. This library promotes **Functional Programming** principles in C#, allowing you to handle errors as values rather than throwing exceptions.

## 🚀 The Problem
Traditional error handling in C# often relies on `try-catch` blocks and throwing exceptions. This leads to:
- **Performance overhead:** Exceptions are computationally expensive.
- **Polluted logic:** Controllers and services become filled with nested `if-else` or `try-catch` noise.
- **Invisible control flows:** Exceptions act as "jumps" that are hard to track in the business logic.

## ✨ The Solution: Either<L, R>
The `Either` type represents a value of one of two possible types.
- **`Left(L)`**: Represents the **Failure** or error state.
- **`Right(R)`**: Represents the **Success** or value state.



---

## 🛠️ Advanced Features & Implementation

### 1. The Service Layer (`EitherAsync` & Fluent API)
Our implementation leverages `EitherAsync` to handle asynchronous operations and provides a **Fluent API** to chain business rules using `.Try()` and `.Ensure()`.

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
Entiendo perfectamente, parece que hubo un corte en la generación anterior. Aquí tienes el **README.md** completo, de una sola vez, incluyendo todas las secciones fundamentales que desarrollamos: la arquitectura, el servicio asíncrono, el controlador y el núcleo técnico.

---

### 2. Clean Controllers (The "Match" Pattern)

By using the `.Match()` method, controllers remain thin and focused only on mapping the result to an HTTP response. No `try-catch` needed.

```csharp
[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _service;

    public CompanyController(ICompanyService service) => _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _service.GetCompany(id);

        return await result.Match<IActionResult>(
            error => BadRequest(new { Message = error }), // Handle Failure (Left)
            company => Ok(company)                         // Handle Success (Right)
        );
    }
}

```

---

## 🏗️ Technical Architecture

The core is built using an **Abstract Base Class** to ensure immutability and type safety.

```csharp
public abstract class Either<L, R>
{
    public abstract T Match<T>(Func<L, T> leftFn, Func<R, T> rightFn);
}

public sealed class Left<L, R> : Either<L, R>
{
    private readonly L _value;
    public Left(L value) => _value = value;
    public override T Match<T>(Func<L, T> leftFn, Func<R, T> rightFn) => leftFn(_value);
}

public sealed class Right<L, R> : Either<L, R>
{
    private readonly R _value;
    public Right(R value) => _value = value;
    public override T Match<T>(Func<L, T> leftFn, Func<R, T> rightFn) => rightFn(_value);
}

```

---

## 📋 API Reference Summary

| Method | Description |
| --- | --- |
| **Try** | Executes an async task and catches any exception, wrapping it in a `Left`. |
| **Ensure** | Validates a condition. If false, transforms `Right` into `Left`. |
| **Match** | Forces the developer to handle both `Left` and `Right` cases. |
| **Map** | Transforms the success value (`Right`) while preserving the error. |

---

## 🔧 Installation & Setup

1. **Clone the repository:**
```bash
git clone [https://github.com/your-username/either-pattern-csharp.git](https://github.com/your-username/either-pattern-csharp.git)

```


2. **Build the solution:**
```bash
dotnet build

```
